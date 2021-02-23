namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Settings.ObjectModel;
    using StyleCop.Analyzers.Helpers;
    using System.Collections.Immutable;
    using System;

    /// <summary>
    /// Represents a code fix for a rule of the analyzer.
    /// </summary>
    public abstract class CodeFixUsingReorder : CodeFix
    {
        #region Init
        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFixUsingReorder(AnalyzerRule rule)
            : base(rule)
        {
        }
        #endregion

        #region Descendant Interface
        /// <summary>
        /// Reorders using directives in a document.
        /// </summary>
        /// <param name="document">The source document.</param>
        /// <param name="syntaxRoot">The root node.</param>
        /// <param name="syntaxNode">The node to move.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <param name="usingDirectivesPlacementSettings">The ordering.</param>
        protected async Task<Document> AsyncUsingReorder(Document document, SyntaxNode syntaxRoot, UsingDirectiveSyntax syntaxNode, CancellationToken cancellationToken, UsingDirectivesPlacement usingDirectivesPlacementSettings)
        {
            var fileHeader = GetFileHeader(syntaxRoot);
            var compilationUnit = (CompilationUnitSyntax)syntaxRoot;

            bool forcePreservePlacement = false;

            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var usingDirectivesPlacement = forcePreservePlacement ? UsingDirectivesPlacement.Preserve : DeterminePlacement(compilationUnit, usingDirectivesPlacementSettings);

            var usingsHelper = new UsingsSorter(false, false, semanticModel!, compilationUnit, fileHeader);
            IndentationSettings Indentation = new IndentationSettings();
            var usingsIndentation = DetermineIndentation(compilationUnit, Indentation, usingDirectivesPlacement);

            // - The strategy is to strip all using directive that are not inside a conditional directive and replace them later with a sorted list at the correct spot
            // - The using directives that are inside a conditional directive are replaced (in sorted order) on the spot.
            // - Conditional directives are not moved, as correctly parsing them is too tricky
            // - No using directives will be stripped when there are multiple namespaces. In that case everything is replaced on the spot.
            List<UsingDirectiveSyntax> stripList;
            var replaceMap = new Dictionary<UsingDirectiveSyntax, UsingDirectiveSyntax>();

            // When there are multiple namespaces, do not move using statements outside of them, only sort.
            if (usingDirectivesPlacement == UsingDirectivesPlacement.Preserve)
            {
                BuildReplaceMapForNamespaces(usingsHelper, replaceMap, Indentation, false);
                stripList = new List<UsingDirectiveSyntax>();
            }
            else
            {
                stripList = BuildStripList(usingsHelper);
            }

            BuildReplaceMapForConditionalDirectives(usingsHelper, replaceMap, Indentation, usingsHelper.ConditionalRoot);

            var usingSyntaxRewriter = new UsingSyntaxRewriter(stripList, replaceMap, fileHeader);
            var newSyntaxRoot = usingSyntaxRewriter.Visit(syntaxRoot);

            if (usingDirectivesPlacement == UsingDirectivesPlacement.InsideNamespace)
            {
                newSyntaxRoot = AddUsingsToNamespace(newSyntaxRoot, usingsHelper, usingsIndentation, replaceMap.Any());
            }
            else if (usingDirectivesPlacement == UsingDirectivesPlacement.OutsideNamespace)
            {
                newSyntaxRoot = AddUsingsToCompilationRoot(newSyntaxRoot, usingsHelper, usingsIndentation, replaceMap.Any());
            }

            // Final cleanup
            newSyntaxRoot = StripMultipleBlankLines(newSyntaxRoot);
            newSyntaxRoot = ReAddFileHeader(newSyntaxRoot, fileHeader);

            var newDocument = document.WithSyntaxRoot(newSyntaxRoot.WithoutFormatting());

            return newDocument;
        }
        #endregion

        #region Implementation

        private static readonly SymbolDisplayFormat FullNamespaceDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat
        .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)
        .WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);

        private const string SystemUsingDirectiveIdentifier = nameof(System);
        private static readonly List<UsingDirectiveSyntax> EmptyUsingsList = new List<UsingDirectiveSyntax>();

        private static string DetermineIndentation(CompilationUnitSyntax compilationUnit, IndentationSettings indentationSettings, UsingDirectivesPlacement usingDirectivesPlacement)
        {
            string usingsIndentation;

            if (usingDirectivesPlacement == UsingDirectivesPlacement.InsideNamespace)
            {
                var rootNamespace = compilationUnit.Members.OfType<NamespaceDeclarationSyntax>().First();
                var indentationLevel = IndentationHelper.GetIndentationSteps(indentationSettings, rootNamespace);
                usingsIndentation = IndentationHelper.GenerateIndentationString(indentationSettings, indentationLevel + 1);
            }
            else
            {
                usingsIndentation = string.Empty;
            }

            return usingsIndentation;
        }

        private static UsingDirectivesPlacement DeterminePlacement(CompilationUnitSyntax compilationUnit, UsingDirectivesPlacement usingDirectivesPlacement)
        {
            switch (usingDirectivesPlacement)
            {
                case UsingDirectivesPlacement.InsideNamespace:
                    var namespaceCount = CountNamespaces(compilationUnit.Members);

                    // Only move using declarations inside the namespace when
                    // - There are no global attributes
                    // - There is only a single namespace declared at the top level
                    // - OrderingSettings.UsingDirectivesPlacement is set to InsideNamespace
                    if (compilationUnit.AttributeLists.Any()
                        || compilationUnit.Members.Count > 1
                        || namespaceCount > 1)
                    {
                        // Override the user's setting with a more conservative one
                        return UsingDirectivesPlacement.Preserve;
                    }

                    if (namespaceCount == 0)
                    {
                        return UsingDirectivesPlacement.OutsideNamespace;
                    }

                    return UsingDirectivesPlacement.InsideNamespace;

                case UsingDirectivesPlacement.OutsideNamespace:
                    return UsingDirectivesPlacement.OutsideNamespace;

                case UsingDirectivesPlacement.Preserve:
                default:
                    return UsingDirectivesPlacement.Preserve;
            }
        }

        private static int CountNamespaces(SyntaxList<MemberDeclarationSyntax> members)
        {
            var result = 0;

            foreach (var namespaceDeclaration in members.OfType<NamespaceDeclarationSyntax>())
            {
                result += 1 + CountNamespaces(namespaceDeclaration.Members);
            }

            return result;
        }

        private static List<UsingDirectiveSyntax> BuildStripList(UsingsSorter usingsHelper)
        {
            return usingsHelper.GetContainedUsings(TreeTextSpan.Empty).ToList();
        }

        private static void BuildReplaceMapForNamespaces(UsingsSorter usingsHelper, Dictionary<UsingDirectiveSyntax, UsingDirectiveSyntax> replaceMap, IndentationSettings indentationSettings, bool qualifyNames)
        {
            var usingsPerNamespace = usingsHelper
                .GetContainedUsings(TreeTextSpan.Empty)
                .GroupBy(ud => ud.Parent)
                .Select(gr => gr.ToList());

            foreach (var usingList in usingsPerNamespace)
            {
                if (usingList.Count > 0)
                {
                    // sort the original using declarations on Span.Start, in order to have the correct replace mapping.
                    usingList.Sort(CompareSpanStart);

                    var indentationSteps = IndentationHelper.GetIndentationSteps(indentationSettings, usingList[0].Parent!);
                    if (usingList[0].Parent is NamespaceDeclarationSyntax)
                    {
                        indentationSteps++;
                    }

                    var indentation = IndentationHelper.GenerateIndentationString(indentationSettings, indentationSteps);

                    var modifiedUsings = usingsHelper.GenerateGroupedUsings(usingList, indentation, false, qualifyNames);

                    for (var i = 0; i < usingList.Count; i++)
                    {
                        replaceMap.Add(usingList[i], modifiedUsings[i]);
                    }
                }
            }
        }

        private static void BuildReplaceMapForConditionalDirectives(UsingsSorter usingsHelper, Dictionary<UsingDirectiveSyntax, UsingDirectiveSyntax> replaceMap, IndentationSettings indentationSettings, TreeTextSpan rootSpan)
        {
            foreach (var childSpan in rootSpan.Children)
            {
                var originalUsings = usingsHelper.GetContainedUsings(childSpan);
                if (originalUsings.Count > 0)
                {
                    // sort the original using declarations on Span.Start, in order to have the correct replace mapping.
                    originalUsings.Sort(CompareSpanStart);

                    var indentationSteps = IndentationHelper.GetIndentationSteps(indentationSettings, originalUsings[0].Parent!);
                    if (originalUsings[0].Parent is NamespaceDeclarationSyntax)
                    {
                        indentationSteps++;
                    }

                    var indentation = IndentationHelper.GenerateIndentationString(indentationSettings, indentationSteps);

                    var modifiedUsings = usingsHelper.GenerateGroupedUsings(childSpan, indentation, false, qualifyNames: false);

                    for (var i = 0; i < originalUsings.Count; i++)
                    {
                        replaceMap.Add(originalUsings[i], modifiedUsings[i]);
                    }
                }

                BuildReplaceMapForConditionalDirectives(usingsHelper, replaceMap, indentationSettings, childSpan);
            }
        }

        private static int CompareSpanStart(UsingDirectiveSyntax left, UsingDirectiveSyntax right)
        {
            return left.SpanStart - right.SpanStart;
        }

        private static SyntaxNode AddUsingsToNamespace(SyntaxNode newSyntaxRoot, UsingsSorter usingsHelper, string usingsIndentation, bool hasConditionalDirectives)
        {
            var rootNamespace = ((CompilationUnitSyntax)newSyntaxRoot).Members.OfType<NamespaceDeclarationSyntax>().First();
            var withTrailingBlankLine = hasConditionalDirectives || rootNamespace.Members.Any() || rootNamespace.Externs.Any();

            var groupedUsings = usingsHelper.GenerateGroupedUsings(TreeTextSpan.Empty, usingsIndentation, withTrailingBlankLine, qualifyNames: false);
            groupedUsings = groupedUsings.AddRange(rootNamespace.Usings);

            var newRootNamespace = rootNamespace.WithUsings(groupedUsings);
            newSyntaxRoot = newSyntaxRoot.ReplaceNode(rootNamespace, newRootNamespace);

            return newSyntaxRoot;
        }

        private static SyntaxNode AddUsingsToCompilationRoot(SyntaxNode newSyntaxRoot, UsingsSorter usingsHelper, string usingsIndentation, bool hasConditionalDirectives)
        {
            var newCompilationUnit = (CompilationUnitSyntax)newSyntaxRoot;
            var withTrailingBlankLine = hasConditionalDirectives || newCompilationUnit.AttributeLists.Any() || newCompilationUnit.Members.Any() || newCompilationUnit.Externs.Any();

            var groupedUsings = usingsHelper.GenerateGroupedUsings(TreeTextSpan.Empty, usingsIndentation, withTrailingBlankLine, qualifyNames: true);
            groupedUsings = groupedUsings.AddRange(newCompilationUnit.Usings);
            newSyntaxRoot = newCompilationUnit.WithUsings(groupedUsings);

            return newSyntaxRoot;
        }

        private static readonly SyntaxAnnotation UsingCodeFixAnnotation = new SyntaxAnnotation(nameof(UsingCodeFixAnnotation));

        private static SyntaxNode StripMultipleBlankLines(SyntaxNode syntaxRoot)
        {
            var replaceMap = new Dictionary<SyntaxToken, SyntaxToken>();

            var usingDirectives = syntaxRoot.GetAnnotatedNodes(UsingCodeFixAnnotation).Cast<UsingDirectiveSyntax>();

            foreach (var usingDirective in usingDirectives)
            {
                var nextToken = usingDirective.SemicolonToken.GetNextToken(true);

                // start at -1 to compensate for the always present end-of-line.
                var trailingCount = -1;

                // count the blanks lines at the end of the using statement.
                foreach (var trivia in usingDirective.SemicolonToken.TrailingTrivia.Reverse())
                {
                    if (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))
                    {
                        break;
                    }

                    trailingCount++;
                }

                // count the blank lines at the start of the next token
                var leadingCount = 0;

                foreach (var trivia in nextToken.LeadingTrivia)
                {
                    if (!trivia.IsKind(SyntaxKind.EndOfLineTrivia))
                    {
                        break;
                    }

                    leadingCount++;
                }

                if ((trailingCount + leadingCount) > 1)
                {
                    var totalStripCount = trailingCount + leadingCount - 1;

                    if (trailingCount > 0)
                    {
                        var trailingStripCount = Math.Min(totalStripCount, trailingCount);

                        var trailingTrivia = usingDirective.SemicolonToken.TrailingTrivia;
                        replaceMap[usingDirective.SemicolonToken] = usingDirective.SemicolonToken.WithTrailingTrivia(trailingTrivia.Take(trailingTrivia.Count - trailingStripCount));
                        totalStripCount -= trailingStripCount;
                    }

                    if (totalStripCount > 0)
                    {
                        replaceMap[nextToken] = nextToken.WithLeadingTrivia(nextToken.LeadingTrivia.Skip(totalStripCount));
                    }
                }
            }

            var newSyntaxRoot = syntaxRoot.ReplaceTokens(replaceMap.Keys, (original, rewritten) => replaceMap[original]);
            return newSyntaxRoot;
        }

        private static ImmutableArray<SyntaxTrivia> GetFileHeader(SyntaxNode syntaxRoot)
        {
            var onBlankLine = true;
            var hasHeader = false;
            var fileHeaderBuilder = ImmutableArray.CreateBuilder<SyntaxTrivia>();

            var firstToken = syntaxRoot.GetFirstToken(includeZeroWidth: true);
            var firstTokenLeadingTrivia = firstToken.LeadingTrivia;

            int i;
            for (i = 0; i < firstTokenLeadingTrivia.Count; i++)
            {
                bool done = false;
                switch (firstTokenLeadingTrivia[i].Kind())
                {
                    case SyntaxKind.SingleLineCommentTrivia:
                    case SyntaxKind.MultiLineCommentTrivia:
                        fileHeaderBuilder.Add(firstTokenLeadingTrivia[i]);
                        onBlankLine = false;
                        break;

                    case SyntaxKind.WhitespaceTrivia:
                        fileHeaderBuilder.Add(firstTokenLeadingTrivia[i]);
                        break;

                    case SyntaxKind.EndOfLineTrivia:
                        hasHeader = true;
                        fileHeaderBuilder.Add(firstTokenLeadingTrivia[i]);

                        if (onBlankLine)
                        {
                            done = true;
                        }
                        else
                        {
                            onBlankLine = true;
                        }

                        break;

                    default:
                        done = true;
                        break;
                }

                if (done)
                {
                    break;
                }
            }

            return hasHeader ? fileHeaderBuilder.ToImmutableArray() : ImmutableArray.Create<SyntaxTrivia>();
        }

        private static SyntaxNode ReAddFileHeader(SyntaxNode syntaxRoot, ImmutableArray<SyntaxTrivia> fileHeader)
        {
            if (fileHeader.IsEmpty)
            {
                // Only re-add the file header if it was stripped.
                return syntaxRoot;
            }

            var firstToken = syntaxRoot.GetFirstToken(includeZeroWidth: true);
            var newLeadingTrivia = firstToken.LeadingTrivia.InsertRange(0, fileHeader);
            return syntaxRoot.ReplaceToken(firstToken, firstToken.WithLeadingTrivia(newLeadingTrivia));
        }

        private class UsingSyntaxRewriter : CSharpSyntaxRewriter
        {
            private readonly List<UsingDirectiveSyntax> stripList;
            private readonly Dictionary<UsingDirectiveSyntax, UsingDirectiveSyntax> replaceMap;
            private readonly ImmutableArray<SyntaxTrivia> fileHeader;
            private readonly LinkedList<SyntaxToken> tokensToStrip = new LinkedList<SyntaxToken>();

            public UsingSyntaxRewriter(List<UsingDirectiveSyntax> stripList, Dictionary<UsingDirectiveSyntax, UsingDirectiveSyntax> replaceMap, ImmutableArray<SyntaxTrivia> fileHeader)
            {
                this.stripList = stripList;
                this.replaceMap = replaceMap;
                this.fileHeader = fileHeader;
            }

            public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
            {
                // The strip list is used to remove using directives that will be moved.
                if (this.stripList.Contains(node))
                {
                    var nextToken = node.SemicolonToken.GetNextToken();

                    if (!nextToken.IsKind(SyntaxKind.None))
                    {
                        var index = TriviaHelper.IndexOfFirstNonBlankLineTrivia(nextToken.LeadingTrivia);
                        if (index != 0)
                        {
                            this.tokensToStrip.AddLast(nextToken);
                        }
                    }

                    return null!;
                }

                // The replacement map is used to replace using declarations in place in sorted order (inside directive trivia)
                UsingDirectiveSyntax replacementNode;
                if (this.replaceMap.TryGetValue(node, out replacementNode))
                {
                    return replacementNode;
                }

                return base.VisitUsingDirective(node)!;
            }

            public override SyntaxToken VisitToken(SyntaxToken token)
            {
                if (this.tokensToStrip.Contains(token))
                {
                    this.tokensToStrip.Remove(token);

                    var index = TriviaHelper.IndexOfFirstNonBlankLineTrivia(token.LeadingTrivia);
                    var newLeadingTrivia = (index == -1) ? SyntaxFactory.TriviaList() : SyntaxFactory.TriviaList(token.LeadingTrivia.Skip(index));
                    return token.WithLeadingTrivia(newLeadingTrivia);
                }

                return base.VisitToken(token);
            }

            public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
            {
                if (this.fileHeader.Contains(trivia))
                {
                    return default;
                }

                return base.VisitTrivia(trivia);
            }
        }

        private class FixAll : DocumentBasedFixAllProvider
        {
            public static FixAllProvider Instance { get; } = new FixAll();

            /// <inheritdoc/>
            protected override string CodeActionTitle
                => "UsingCodeFix";

            /// <inheritdoc/>
            protected override async Task<SyntaxNode?> FixAllInDocumentAsync(FixAllContext fixAllContext, Document document, ImmutableArray<Diagnostic> diagnostics)
            {
                return await Task.Run(() => (SyntaxNode?)null);
            }
        }
        /// <summary>
        /// Helper class that will sort the using statements and generate new using groups based on the given settings.
        /// </summary>
        private class UsingsSorter
        {
            private readonly SemanticModel semanticModel;
            private readonly ImmutableArray<SyntaxTrivia> fileHeader;
            private readonly bool separateSystemDirectives;
            private readonly bool insertBlankLinesBetweenGroups;

            private readonly SourceMap sourceMap;

            private readonly Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> systemUsings = new Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>>();
            private readonly Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> namespaceUsings = new Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>>();
            private readonly Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> aliases = new Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>>();
            private readonly Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> systemStaticImports = new Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>>();
            private readonly Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> staticImports = new Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>>();

            public UsingsSorter(bool systemUsingDirectivesFirst, bool requireBlankLinesBetweenUsingGroups, SemanticModel semanticModel, CompilationUnitSyntax compilationUnit, ImmutableArray<SyntaxTrivia> fileHeader)
            {
                this.separateSystemDirectives = systemUsingDirectivesFirst;
                this.insertBlankLinesBetweenGroups = requireBlankLinesBetweenUsingGroups;

                this.semanticModel = semanticModel;
                this.fileHeader = fileHeader;

                this.sourceMap = SourceMap.FromCompilationUnit(compilationUnit);

                this.ProcessUsingDirectives(compilationUnit.Usings);
                this.ProcessMembers(compilationUnit.Members);
            }

            public TreeTextSpan ConditionalRoot
            {
                get { return this.sourceMap.ConditionalRoot; }
            }

            public List<UsingDirectiveSyntax> GetContainedUsings(TreeTextSpan directiveSpan)
            {
                List<UsingDirectiveSyntax> result = new List<UsingDirectiveSyntax>();
                List<UsingDirectiveSyntax> usingsList;

                if (this.systemUsings.TryGetValue(directiveSpan, out usingsList))
                {
                    result.AddRange(usingsList);
                }

                if (this.namespaceUsings.TryGetValue(directiveSpan, out usingsList))
                {
                    result.AddRange(usingsList);
                }

                if (this.aliases.TryGetValue(directiveSpan, out usingsList))
                {
                    result.AddRange(usingsList);
                }

                if (this.systemStaticImports.TryGetValue(directiveSpan, out usingsList))
                {
                    result.AddRange(usingsList);
                }

                if (this.staticImports.TryGetValue(directiveSpan, out usingsList))
                {
                    result.AddRange(usingsList);
                }

                return result;
            }

            public SyntaxList<UsingDirectiveSyntax> GenerateGroupedUsings(TreeTextSpan directiveSpan, string indentation, bool withTrailingBlankLine, bool qualifyNames)
            {
                var usingList = new List<UsingDirectiveSyntax>();
                List<SyntaxTrivia> triviaToMove = new List<SyntaxTrivia>();

                usingList.AddRange(this.GenerateUsings(this.systemUsings, directiveSpan, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.namespaceUsings, directiveSpan, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.systemStaticImports, directiveSpan, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.staticImports, directiveSpan, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.aliases, directiveSpan, indentation, triviaToMove, qualifyNames));

                if (triviaToMove.Count > 0)
                {
                    var newLeadingTrivia = SyntaxFactory.TriviaList(triviaToMove).AddRange(usingList[0].GetLeadingTrivia());
                    usingList[0] = usingList[0].WithLeadingTrivia(newLeadingTrivia);
                }

                if (withTrailingBlankLine && (usingList.Count > 0))
                {
                    var lastUsing = usingList[usingList.Count - 1];
                    usingList[usingList.Count - 1] = lastUsing.WithTrailingTrivia(lastUsing.GetTrailingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed));
                }

                return SyntaxFactory.List(usingList);
            }

            public SyntaxList<UsingDirectiveSyntax> GenerateGroupedUsings(List<UsingDirectiveSyntax> usingsList, string indentation, bool withTrailingBlankLine, bool qualifyNames)
            {
                var usingList = new List<UsingDirectiveSyntax>();
                List<SyntaxTrivia> triviaToMove = new List<SyntaxTrivia>();

                usingList.AddRange(this.GenerateUsings(this.systemUsings, usingsList, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.namespaceUsings, usingsList, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.systemStaticImports, usingsList, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.staticImports, usingsList, indentation, triviaToMove, qualifyNames));
                usingList.AddRange(this.GenerateUsings(this.aliases, usingsList, indentation, triviaToMove, qualifyNames));

                if (triviaToMove.Count > 0)
                {
                    var newLeadingTrivia = SyntaxFactory.TriviaList(triviaToMove).AddRange(usingList[0].GetLeadingTrivia());
                    usingList[0] = usingList[0].WithLeadingTrivia(newLeadingTrivia);
                }

                if (withTrailingBlankLine && (usingList.Count > 0))
                {
                    var lastUsing = usingList[usingList.Count - 1];
                    usingList[usingList.Count - 1] = lastUsing.WithTrailingTrivia(lastUsing.GetTrailingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed));
                }

                return SyntaxFactory.List(usingList);
            }

            private List<UsingDirectiveSyntax> GenerateUsings(Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> usingsGroup, TreeTextSpan directiveSpan, string indentation, List<SyntaxTrivia> triviaToMove, bool qualifyNames)
            {
                List<UsingDirectiveSyntax> result = new List<UsingDirectiveSyntax>();
                List<UsingDirectiveSyntax> usingsList;

                if (!usingsGroup.TryGetValue(directiveSpan, out usingsList))
                {
                    return result;
                }

                return this.GenerateUsings(usingsList, indentation, triviaToMove, qualifyNames);
            }

            private List<UsingDirectiveSyntax> GenerateUsings(List<UsingDirectiveSyntax> usingsList, string indentation, List<SyntaxTrivia> triviaToMove, bool qualifyNames)
            {
                List<UsingDirectiveSyntax> result = new List<UsingDirectiveSyntax>();

                if (!usingsList.Any())
                {
                    return result;
                }

                for (var i = 0; i < usingsList.Count; i++)
                {
                    var currentUsing = usingsList[i];

                    // strip the file header, if the using is the first node in the source file.
                    List<SyntaxTrivia> leadingTrivia;
                    if ((i == 0) && currentUsing.GetFirstToken().GetPreviousToken().IsMissingOrDefault())
                    {
                        leadingTrivia = currentUsing.GetLeadingTrivia().Except(this.fileHeader).ToList();
                    }
                    else
                    {
                        leadingTrivia = currentUsing.GetLeadingTrivia().ToList();
                    }

                    // when there is a directive trivia, add it (and any trivia before it) to the triviaToMove collection.
                    // when there are leading blank lines for the first entry, add them to the triviaToMove collection.
                    int triviaToMoveCount = triviaToMove.Count;
                    var previousIsEndOfLine = false;
                    for (var m = leadingTrivia.Count - 1; m >= 0; m--)
                    {
                        if (leadingTrivia[m].IsDirective)
                        {
                            // When a directive is followed by a blank line, keep the blank line with the directive.
                            int takeCount = previousIsEndOfLine ? m + 2 : m + 1;
                            triviaToMove.InsertRange(0, leadingTrivia.Take(takeCount));
                            break;
                        }

                        if ((i == 0) && leadingTrivia[m].IsKind(SyntaxKind.EndOfLineTrivia))
                        {
                            if (previousIsEndOfLine)
                            {
                                triviaToMove.InsertRange(0, leadingTrivia.Take(m + 2));
                                break;
                            }

                            previousIsEndOfLine = true;
                        }
                        else
                        {
                            previousIsEndOfLine = false;
                        }
                    }

                    // preserve leading trivia (excluding directive trivia), indenting each line as appropriate
                    var newLeadingTrivia = leadingTrivia.Except(triviaToMove).ToList();

                    // indent the triviaToMove if necessary so it behaves correctly later
                    bool atStartOfLine = triviaToMoveCount == 0 || triviaToMove.Last().HasBuiltinEndLine();
                    for (int m = triviaToMoveCount; m < triviaToMove.Count; m++)
                    {
                        bool currentAtStartOfLine = atStartOfLine;
                        atStartOfLine = triviaToMove[m].HasBuiltinEndLine();
                        if (!currentAtStartOfLine)
                        {
                            continue;
                        }

                        if (triviaToMove[m].IsKind(SyntaxKind.EndOfLineTrivia))
                        {
                            // This is a blank line; indenting it would only add trailing whitespace
                            continue;
                        }

                        if (triviaToMove[m].IsDirective)
                        {
                            // Only #region and #endregion directives get indented
                            if (!triviaToMove[m].IsKind(SyntaxKind.RegionDirectiveTrivia) && !triviaToMove[m].IsKind(SyntaxKind.EndRegionDirectiveTrivia))
                            {
                                // This is a preprocessor line that doesn't need to be indented
                                continue;
                            }
                        }

                        if (triviaToMove[m].IsKind(SyntaxKind.DisabledTextTrivia))
                        {
                            // This is text in a '#if false' block; just ignore it
                            continue;
                        }

                        if (string.IsNullOrEmpty(indentation))
                        {
                            if (triviaToMove[m].IsKind(SyntaxKind.WhitespaceTrivia))
                            {
                                // Remove the trivia and analyze the current position again
                                triviaToMove.RemoveAt(m);
                                m--;
                                atStartOfLine = true;
                            }
                        }
                        else
                        {
                            triviaToMove.Insert(m, SyntaxFactory.Whitespace(indentation));
                            m++;
                        }
                    }

                    // strip any leading whitespace on each line (and also blank lines)
                    var k = 0;
                    var startOfLine = true;
                    while (k < newLeadingTrivia.Count)
                    {
                        switch (newLeadingTrivia[k].Kind())
                        {
                            case SyntaxKind.WhitespaceTrivia:
                                newLeadingTrivia.RemoveAt(k);
                                break;

                            case SyntaxKind.EndOfLineTrivia:
                                if (startOfLine)
                                {
                                    newLeadingTrivia.RemoveAt(k);
                                }
                                else
                                {
                                    startOfLine = true;
                                    k++;
                                }

                                break;

                            default:
                                startOfLine = newLeadingTrivia[k].IsDirective;
                                k++;
                                break;
                        }
                    }

                    for (var j = newLeadingTrivia.Count - 1; j >= 0; j--)
                    {
                        if (newLeadingTrivia[j].IsKind(SyntaxKind.EndOfLineTrivia))
                        {
                            newLeadingTrivia.Insert(j + 1, SyntaxFactory.Whitespace(indentation));
                        }
                    }

                    newLeadingTrivia.Insert(0, SyntaxFactory.Whitespace(indentation));

                    // preserve trailing trivia, adding an end of line if necessary.
                    var currentTrailingTrivia = currentUsing.GetTrailingTrivia();
                    var newTrailingTrivia = currentTrailingTrivia;
                    if (!currentTrailingTrivia.Any() || !currentTrailingTrivia.Last().IsKind(SyntaxKind.EndOfLineTrivia))
                    {
                        newTrailingTrivia = newTrailingTrivia.Add(SyntaxFactory.CarriageReturnLineFeed);
                    }

                    var processedUsing = (qualifyNames ? this.QualifyUsingDirective(currentUsing) : currentUsing)
                        .WithLeadingTrivia(newLeadingTrivia)
                        .WithTrailingTrivia(newTrailingTrivia)
                        .WithAdditionalAnnotations(UsingCodeFixAnnotation);

                    result.Add(processedUsing);
                }

                result.Sort(this.CompareUsings);

                if (this.insertBlankLinesBetweenGroups)
                {
                    var last = result[result.Count - 1];

                    result[result.Count - 1] = last.WithTrailingTrivia(last.GetTrailingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed));
                }

                return result;
            }

            private UsingDirectiveSyntax QualifyUsingDirective(UsingDirectiveSyntax usingDirective)
            {
                NameSyntax originalName = usingDirective.Name;
                NameSyntax rewrittenName;
                switch (originalName.Kind())
                {
                    case SyntaxKind.QualifiedName:
                    case SyntaxKind.IdentifierName:
                    case SyntaxKind.GenericName:
                        if (originalName.Parent.IsKind(SyntaxKind.UsingDirective)
                            || originalName.Parent.IsKind(SyntaxKind.TypeArgumentList))
                        {
                            var symbol = this.semanticModel.GetSymbolInfo(originalName, cancellationToken: CancellationToken.None).Symbol;
                            if (symbol == null)
                            {
                                rewrittenName = originalName;
                                break;
                            }

                            if (symbol is INamespaceSymbol)
                            {
                                // TODO: Preserve inner trivia
                                string fullName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                                NameSyntax replacement = SyntaxFactory.ParseName(fullName);
                                if (!originalName.DescendantNodesAndSelf().OfType<AliasQualifiedNameSyntax>().Any())
                                {
                                    replacement = replacement.ReplaceNodes(
                                        replacement.DescendantNodesAndSelf().OfType<AliasQualifiedNameSyntax>(),
                                        (originalNode2, rewrittenNode2) => rewrittenNode2.Name);
                                }

                                rewrittenName = replacement.WithTriviaFrom(originalName);
                                break;
                            }
                            else if (symbol is INamedTypeSymbol namedTypeSymbol)
                            {
                                // TODO: Preserve inner trivia
                                // TODO: simplify after qualification
                                string fullName;
                                if (SpecialTypeHelper.IsPredefinedType(namedTypeSymbol.OriginalDefinition.SpecialType))
                                {
                                    fullName = "global::System." + symbol.Name;
                                }
                                /*else if (namedTypeSymbol.IsTupleType())
                                {
                                    fullName = namedTypeSymbol.TupleUnderlyingTypeOrSelf().ToFullyQualifiedValueTupleDisplayString();
                                }*/
                                else
                                {
                                    fullName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                                }

                                NameSyntax replacement = SyntaxFactory.ParseName(fullName);
                                if (!originalName.DescendantNodesAndSelf().OfType<AliasQualifiedNameSyntax>().Any())
                                {
                                    replacement = replacement.ReplaceNodes(
                                        replacement.DescendantNodesAndSelf().OfType<AliasQualifiedNameSyntax>(),
                                        (originalNode2, rewrittenNode2) => rewrittenNode2.Name);
                                }

                                rewrittenName = replacement.WithTriviaFrom(originalName);
                                break;
                            }
                            else
                            {
                                rewrittenName = originalName;
                                break;
                            }
                        }
                        else
                        {
                            rewrittenName = originalName;
                            break;
                        }

                    case SyntaxKind.AliasQualifiedName:
                    case SyntaxKind.PredefinedType:
                    default:
                        rewrittenName = originalName;
                        break;
                }

                if (rewrittenName == originalName)
                {
                    return usingDirective;
                }

                return usingDirective.ReplaceNode(originalName, rewrittenName);
            }

            private int CompareUsings(UsingDirectiveSyntax left, UsingDirectiveSyntax right)
            {
                if ((left.Alias != null) && (right.Alias != null))
                {
                    return NameSyntaxHelpers.Compare(left.Alias.Name, right.Alias.Name);
                }

                return NameSyntaxHelpers.Compare(left.Name, right.Name);
            }

            private bool IsSeparatedStaticSystemUsing(UsingDirectiveSyntax syntax)
            {
                if (!this.separateSystemDirectives)
                {
                    return false;
                }

                return this.StartsWithSystemUsingDirectiveIdentifier(syntax.Name);
            }

            private bool IsSeparatedSystemUsing(UsingDirectiveSyntax syntax)
            {
                if (!this.separateSystemDirectives
                    || syntax.HasNamespaceAliasQualifier())
                {
                    return false;
                }

                return this.StartsWithSystemUsingDirectiveIdentifier(syntax.Name);
            }

            private bool StartsWithSystemUsingDirectiveIdentifier(NameSyntax name)
            {
                if (!(this.semanticModel.GetSymbolInfo(name).Symbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol))
                {
                    return false;
                }

                var namespaceTypeName = namespaceOrTypeSymbol.ToDisplayString(FullNamespaceDisplayFormat);
                var firstPart = namespaceTypeName.Split('.')[0];

                return string.Equals(SystemUsingDirectiveIdentifier, firstPart, StringComparison.Ordinal);
            }

            private void ProcessMembers(SyntaxList<MemberDeclarationSyntax> members)
            {
                foreach (var namespaceDeclaration in members.OfType<NamespaceDeclarationSyntax>())
                {
                    this.ProcessUsingDirectives(namespaceDeclaration.Usings);
                    this.ProcessMembers(namespaceDeclaration.Members);
                }
            }

            private void ProcessUsingDirectives(SyntaxList<UsingDirectiveSyntax> usingDirectives)
            {
                foreach (var usingDirective in usingDirectives)
                {
                    TreeTextSpan containingSpan = this.sourceMap.GetContainingSpan(usingDirective);

                    if (usingDirective.Alias != null)
                    {
                        this.AddUsingDirective(this.aliases, usingDirective, containingSpan);
                    }
                    else if (usingDirective.StaticKeyword.IsKind(SyntaxKind.StaticKeyword))
                    {
                        if (this.IsSeparatedStaticSystemUsing(usingDirective))
                        {
                            this.AddUsingDirective(this.systemStaticImports, usingDirective, containingSpan);
                        }
                        else
                        {
                            this.AddUsingDirective(this.staticImports, usingDirective, containingSpan);
                        }
                    }
                    else if (this.IsSeparatedSystemUsing(usingDirective))
                    {
                        this.AddUsingDirective(this.systemUsings, usingDirective, containingSpan);
                    }
                    else
                    {
                        this.AddUsingDirective(this.namespaceUsings, usingDirective, containingSpan);
                    }
                }
            }

            private void AddUsingDirective(Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> container, UsingDirectiveSyntax usingDirective, TreeTextSpan containingSpan)
            {
                List<UsingDirectiveSyntax> usingList;

                if (!container.TryGetValue(containingSpan, out usingList))
                {
                    usingList = new List<UsingDirectiveSyntax>();
                    container.Add(containingSpan, usingList);
                }

                usingList.Add(usingDirective);
            }

            private List<UsingDirectiveSyntax> GenerateUsings(Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> usingsGroup, List<UsingDirectiveSyntax> usingsList, string indentation, List<SyntaxTrivia> triviaToMove, bool qualifyNames)
            {
                var filteredUsingsList = this.FilterRelevantUsings(usingsGroup, usingsList);

                return this.GenerateUsings(filteredUsingsList, indentation, triviaToMove, qualifyNames);
            }

            private List<UsingDirectiveSyntax> FilterRelevantUsings(Dictionary<TreeTextSpan, List<UsingDirectiveSyntax>> usingsGroup, List<UsingDirectiveSyntax> usingsList)
            {
                List<UsingDirectiveSyntax> groupList;

                if (!usingsGroup.TryGetValue(TreeTextSpan.Empty, out groupList))
                {
                    return EmptyUsingsList;
                }

                return groupList.Where(u => usingsList.Contains(u)).ToList();
            }
        }
        /// <summary>
        /// Immutable class representing a text span with a collection of children.
        /// </summary>
        private class TreeTextSpan : IEquatable<TreeTextSpan>, IComparable<TreeTextSpan>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TreeTextSpan"/> class.
            /// </summary>
            /// <param name="start">The start position for the span.</param>
            /// <param name="end">The end position for the span.</param>
            /// <param name="children">The children of the span.</param>
            internal TreeTextSpan(int start, int end, ImmutableArray<TreeTextSpan> children)
            {
                this.Start = start;
                this.End = end;
                this.Children = children;
            }

            internal static TreeTextSpan Empty { get; } = new TreeTextSpan(0, 0, ImmutableArray<TreeTextSpan>.Empty);

            /// <summary>
            /// Gets the start position of the span.
            /// </summary>
            /// <value>The start position within the source code.</value>
            internal int Start { get; }

            /// <summary>
            /// Gets the end position of the span.
            /// </summary>
            /// <value>The end position within the source code.</value>
            internal int End { get; }

            /// <summary>
            /// Gets the children of this span.
            /// </summary>
            /// <value>A read-only list containing the children.</value>
            internal ImmutableArray<TreeTextSpan> Children { get; }

            /// <summary>
            /// Determines if two instances of <see cref="TreeTextSpan"/> are the same.
            /// </summary>
            /// <param name="left">The first instance to compare.</param>
            /// <param name="right">The second instance to compare.</param>
            /// <returns>True if the instances are the same.</returns>
            public static bool operator ==(TreeTextSpan left, TreeTextSpan right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Determines if two instances of <see cref="TreeTextSpan"/> are the different.
            /// </summary>
            /// <param name="left">The first instance to compare.</param>
            /// <param name="right">The second instance to compare.</param>
            /// <returns>True if the instances are different.</returns>
            public static bool operator !=(TreeTextSpan left, TreeTextSpan right)
            {
                return !left.Equals(right);
            }

            /// <inheritdoc/>
            public bool Equals(TreeTextSpan other)
            {
                return (this.Start == other.Start) && (this.End == other.End);
            }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                return (obj is TreeTextSpan) && this.Equals((TreeTextSpan)obj);
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                unchecked
                {
                    return this.Start + (this.End << 16);
                }
            }

            /// <inheritdoc/>
            public int CompareTo(TreeTextSpan other)
            {
                var diff = this.Start - other.Start;
                if (diff == 0)
                {
                    diff = this.End - other.End;
                }

                return diff;
            }

            /// <summary>
            /// Creates a new builder for a <see cref="TreeTextSpan"/>.
            /// </summary>
            /// <param name="start">The start of the span.</param>
            /// <returns>The created builder.</returns>
            internal static Builder CreateBuilder(int start)
            {
                return new Builder(start);
            }

            /// <summary>
            /// Checks if the given <paramref name="span"/> is contained within this span.
            /// </summary>
            /// <param name="span">The <see cref="TreeTextSpan"/> to check.</param>
            /// <returns>True if the given <paramref name="span"/> is contained.</returns>
            internal bool Contains(TreeTextSpan span)
            {
                return (span.Start >= this.Start) && (span.End <= this.End);
            }

            /// <summary>
            /// Gets smallest (child) span that contains the given <paramref name="textSpan"/>.
            /// This assumes non-overlapping children.
            /// </summary>
            /// <param name="textSpan">The span to check.</param>
            /// <returns>The <see cref="TreeTextSpan"/> that is the best match, or null if there is no match.</returns>
            internal TreeTextSpan GetContainingSpan(TextSpan textSpan)
            {
                if ((textSpan.Start < this.Start) || (textSpan.End > this.End))
                {
                    return Empty;
                }

                foreach (var span in this.Children)
                {
                    var childSpan = span.GetContainingSpan(textSpan);
                    if (childSpan != Empty)
                    {
                        return childSpan;
                    }
                }

                return this;
            }

            /// <summary>
            /// Helper class that can be used to construct a tree of <see cref="TreeTextSpan"/> objects.
            /// </summary>
            internal class Builder
            {
                private readonly List<Builder> children = new List<Builder>();
                private readonly int start;
                private int end = int.MaxValue;

                /// <summary>
                /// Initializes a new instance of the <see cref="Builder"/> class.
                /// </summary>
                /// <param name="start">The start of the span.</param>
                internal Builder(int start)
                {
                    this.start = start;
                }

                private Builder(int start, int end)
                {
                    this.start = start;
                    this.end = end;
                }

                /// <summary>
                /// Sets the end of the span.
                /// </summary>
                /// <param name="end">The end of the span.</param>
                internal void SetEnd(int end)
                {
                    this.end = end;
                }

                /// <summary>
                /// Add a new child to the span.
                /// </summary>
                /// <param name="start">The start of the child span.</param>
                /// <returns>The <see cref="Builder"/> for the child.</returns>
                internal Builder AddChild(int start)
                {
                    var childBuilder = new Builder(start);
                    this.children.Add(childBuilder);

                    return childBuilder;
                }

                /// <summary>
                /// Makes sure that the gaps between children are filled.
                /// These extra spans are created to make sure that using statements will not be moved over directive boundaries.
                /// </summary>
                internal void FillGaps()
                {
                    Builder newFiller;

                    if (this.children.Count == 0)
                    {
                        return;
                    }

                    var previousEnd = int.MaxValue;
                    for (var i = 0; i < this.children.Count; i++)
                    {
                        var child = this.children[i];

                        if (child.start > previousEnd)
                        {
                            newFiller = new Builder(previousEnd, child.start);
                            this.children.Insert(i, newFiller);
                            i++;
                        }

                        child.FillGaps();

                        previousEnd = child.end;
                    }

                    if (previousEnd < this.end)
                    {
                        newFiller = new Builder(previousEnd, this.end);
                        this.children.Add(newFiller);
                    }
                }

                /// <summary>
                /// Converts the builder (and its children) to a <see cref="TreeTextSpan"/> object.
                /// </summary>
                /// <returns>The created <see cref="TreeTextSpan"/> object.</returns>
                internal TreeTextSpan ToSpan()
                {
                    var children = this.children.Select(x => x.ToSpan()).ToImmutableArray();

                    return new TreeTextSpan(this.start, this.end, children);
                }
            }
        }
        /// <summary>
        /// Contains a map of the different regions of a source file.
        /// </summary>
        /// <remarks>
        /// <para>Used source file regions are:</para>
        ///
        /// <list type="bullet">
        /// <item><description>conditional directives (#if, #else, #elif, #endif)</description></item>
        /// <item><description>pragma warning directives</description></item>
        /// <item><description>region directives</description></item>
        /// </list>
        /// </remarks>
        private class SourceMap
        {
            private readonly TreeTextSpan regionRoot;
            private readonly TreeTextSpan pragmaWarningRoot;

            private SourceMap(TreeTextSpan conditionalRoot, TreeTextSpan regionRoot, TreeTextSpan pragmaWarningRoot)
            {
                this.ConditionalRoot = conditionalRoot;
                this.regionRoot = regionRoot;
                this.pragmaWarningRoot = pragmaWarningRoot;
            }

            /// <summary>
            /// Gets the root entry for all conditional directive spans.
            /// </summary>
            /// <value>A <see cref="TreeTextSpan"/> object representing the root conditional directive span.</value>
            internal TreeTextSpan ConditionalRoot { get; }

            /// <summary>
            /// Constructs the directive map for the given <paramref name="compilationUnit"/>.
            /// </summary>
            /// <param name="compilationUnit">The compilation unit to scan for directive trivia.</param>
            /// <returns>A new <see cref="SourceMap"/> object containing the directive trivia information from the passed <paramref name="compilationUnit"/>.</returns>
            internal static SourceMap FromCompilationUnit(CompilationUnitSyntax compilationUnit)
            {
                TreeTextSpan conditionalRoot;
                TreeTextSpan regionRoot;
                TreeTextSpan pragmaWarningRoot;

                BuildDirectiveTriviaMaps(compilationUnit, out conditionalRoot, out regionRoot, out pragmaWarningRoot);

                return new SourceMap(conditionalRoot, regionRoot, pragmaWarningRoot);
            }

            /// <summary>
            /// Gets the containing span for the given <paramref name="node"/>.
            /// </summary>
            /// <param name="node">The node for which the containing span will be determined.</param>
            /// <returns>The span that contains the node.</returns>
            internal TreeTextSpan GetContainingSpan(SyntaxNode node)
            {
                var textSpan = node.GetLocation().SourceSpan;

                var containingSpans = this.pragmaWarningRoot.Children
                    .Where(child => (textSpan.Start >= child.Start) && (textSpan.End <= child.End))
                    .ToList();

                var containingConditionalSpan = this.ConditionalRoot.GetContainingSpan(textSpan);
                if (containingConditionalSpan != this.ConditionalRoot)
                {
                    containingSpans.Add(containingConditionalSpan);
                }

                var containingRegionSpan = this.regionRoot.GetContainingSpan(textSpan);
                if (containingRegionSpan != this.regionRoot)
                {
                    containingSpans.Add(containingRegionSpan);
                }

                if (containingSpans.Count == 0)
                {
                    return TreeTextSpan.Empty;
                }

                for (var i = containingSpans.Count - 1; i > 0; i--)
                {
                    if (containingSpans[i].Contains(containingSpans[i - 1]))
                    {
                        containingSpans.RemoveAt(i);
                    }
                    else if (containingSpans[i - 1].Contains(containingSpans[i]))
                    {
                        containingSpans.RemoveAt(i - 1);
                    }
                }

                if (containingSpans.Count == 1)
                {
                    return containingSpans[0];
                }

                var newStart = int.MinValue;
                var newEnd = int.MaxValue;

                foreach (var span in containingSpans)
                {
                    newStart = Math.Max(newStart, span.Start);
                    newEnd = Math.Min(newEnd, span.End);
                }

                return new TreeTextSpan(newStart, newEnd, ImmutableArray<TreeTextSpan>.Empty);
            }

            private static void ProcessNodeMembers(TreeTextSpan.Builder builder, SyntaxList<MemberDeclarationSyntax> members)
            {
                foreach (var namespaceDeclaration in members.OfType<NamespaceDeclarationSyntax>())
                {
                    var childBuilder = builder.AddChild(namespaceDeclaration.FullSpan.Start);
                    childBuilder.SetEnd(namespaceDeclaration.FullSpan.End);

                    ProcessNodeMembers(childBuilder, namespaceDeclaration.Members);
                }
            }

            private static void BuildDirectiveTriviaMaps(CompilationUnitSyntax compilationUnit, out TreeTextSpan conditionalRoot, out TreeTextSpan regionRoot, out TreeTextSpan pragmaWarningRoot)
            {
                var conditionalStack = new Stack<TreeTextSpan.Builder>();
                var regionStack = new Stack<TreeTextSpan.Builder>();
                var pragmaWarningList = new List<DirectiveTriviaSyntax>();

                var conditionalBuilder = SetupBuilder(compilationUnit, conditionalStack);
                var regionBuilder = SetupBuilder(compilationUnit, regionStack);

                for (var directiveTrivia = compilationUnit.GetFirstDirective(); directiveTrivia != null; directiveTrivia = directiveTrivia.GetNextDirective())
                {
                    switch (directiveTrivia.Kind())
                    {
                        case SyntaxKind.IfDirectiveTrivia:
                            AddNewDirectiveTriviaSpan(conditionalStack, directiveTrivia);
                            break;

                        case SyntaxKind.ElifDirectiveTrivia:
                        case SyntaxKind.ElseDirectiveTrivia:
                            var previousSpan = conditionalStack.Pop();
                            previousSpan.SetEnd(directiveTrivia.FullSpan.Start);

                            AddNewDirectiveTriviaSpan(conditionalStack, directiveTrivia);
                            break;

                        case SyntaxKind.EndIfDirectiveTrivia:
                            CloseDirectiveTriviaSpan(conditionalStack, directiveTrivia);
                            break;

                        case SyntaxKind.RegionDirectiveTrivia:
                            AddNewDirectiveTriviaSpan(regionStack, directiveTrivia);
                            break;

                        case SyntaxKind.EndRegionDirectiveTrivia:
                            CloseDirectiveTriviaSpan(regionStack, directiveTrivia);
                            break;

                        case SyntaxKind.PragmaWarningDirectiveTrivia:
                            pragmaWarningList.Add(directiveTrivia);
                            break;

                        default:
                            // ignore all other directive trivia
                            break;
                    }
                }

                conditionalRoot = FinalizeBuilder(conditionalBuilder, conditionalStack, compilationUnit.Span.End);
                regionRoot = FinalizeBuilder(regionBuilder, regionStack, compilationUnit.Span.End);
                pragmaWarningRoot = BuildPragmaWarningSpans(pragmaWarningList, compilationUnit);
            }

            private static TreeTextSpan.Builder SetupBuilder(CompilationUnitSyntax compilationUnit, Stack<TreeTextSpan.Builder> stack)
            {
                var rootBuilder = TreeTextSpan.CreateBuilder(compilationUnit.SpanStart);
                stack.Push(rootBuilder);

                return rootBuilder;
            }

            private static void AddNewDirectiveTriviaSpan(Stack<TreeTextSpan.Builder> spanStack, DirectiveTriviaSyntax directiveTrivia)
            {
                var parent = spanStack.Peek();
                var newDirectiveSpan = parent.AddChild(directiveTrivia.FullSpan.Start);
                spanStack.Push(newDirectiveSpan);
            }

            private static void CloseDirectiveTriviaSpan(Stack<TreeTextSpan.Builder> spanStack, DirectiveTriviaSyntax directiveTrivia)
            {
                var previousSpan = spanStack.Pop();
                previousSpan.SetEnd(directiveTrivia.FullSpan.End);
            }

            private static TreeTextSpan FinalizeBuilder(TreeTextSpan.Builder builder, Stack<TreeTextSpan.Builder> stack, int end)
            {
                // close all spans (including the root) that have not been closed yet
                while (stack.Count > 0)
                {
                    var span = stack.Pop();
                    span.SetEnd(end);
                }

                // Fill the gaps to make sure that directives on either side of an conditional directive group are not combined
                builder.FillGaps();

                return builder.ToSpan();
            }

            private static TreeTextSpan BuildPragmaWarningSpans(List<DirectiveTriviaSyntax> pragmaWarningList, CompilationUnitSyntax compilationUnit)
            {
                var map = new Dictionary<string, PragmaWarningDirectiveTriviaSyntax>();
                var builder = TreeTextSpan.CreateBuilder(compilationUnit.SpanStart);

                foreach (var pragmaWarning in pragmaWarningList.Cast<PragmaWarningDirectiveTriviaSyntax>())
                {
                    var errorCodes = GetErrorCodes(pragmaWarning);

                    switch (pragmaWarning.DisableOrRestoreKeyword.Kind())
                    {
                        case SyntaxKind.DisableKeyword:
                            foreach (var errorCode in errorCodes)
                            {
                                if (!map.ContainsKey(errorCode))
                                {
                                    // only add it if the warning isn't disabled already
                                    map[errorCode] = pragmaWarning;
                                }
                            }

                            break;

                        case SyntaxKind.RestoreKeyword:
                            foreach (var errorCode in errorCodes)
                            {
                                PragmaWarningDirectiveTriviaSyntax startOfSpan;

                                if (map.TryGetValue(errorCode, out startOfSpan))
                                {
                                    map.Remove(errorCode);

                                    var childSpan = builder.AddChild(startOfSpan.FullSpan.Start);
                                    childSpan.SetEnd(pragmaWarning.FullSpan.End);
                                }
                            }

                            break;
                    }
                }

                // create spans for all pragma warning disable statements that have not been closed.
                foreach (var pragmaWarning in map.Values)
                {
                    var childSpan = builder.AddChild(pragmaWarning.FullSpan.Start);
                    childSpan.SetEnd(compilationUnit.FullSpan.End);
                }

                builder.SetEnd(compilationUnit.FullSpan.End);
                return builder.ToSpan();
            }

            private static List<string> GetErrorCodes(PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTrivia)
            {
                return pragmaWarningDirectiveTrivia.ErrorCodes
                    .OfType<IdentifierNameSyntax>()
                    .Select(x => x.Identifier.ValueText)
                    .ToList();
            }
        }
        #endregion
    }
}
