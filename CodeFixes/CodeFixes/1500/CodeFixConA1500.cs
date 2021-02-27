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

    /// <summary>
    /// Represents a code fix for a rule of the analyzer.
    /// </summary>
    public class CodeFixConA1500 : CodeFix
    {
        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFixConA1500(AnalyzerRule rule)
            : base(rule)
        {
        }

        private async Task<Document> AsyncHandler(Document document, SyntaxNode syntaxNode, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);

            if (syntaxNode.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = syntaxNode.GetLeadingTrivia();
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxNode NewNode = syntaxNode.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewRoot = Root.ReplaceNode(syntaxNode, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncWhileHandler(Document document, DoStatementSyntax doStatement, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken WhileKeyword = doStatement.WhileKeyword;

            if (WhileKeyword.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = WhileKeyword.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewKeyword = WhileKeyword.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = doStatement.WithWhileKeyword(NewKeyword);
            SyntaxNode NewRoot = Root.ReplaceNode(doStatement, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        /// <summary>
        /// Creates the action to perform to fix a document.
        /// </summary>
        public override CodeAction? CreateDocumentHandler(CodeFixContext context, SyntaxNode root, TextSpan diagnosticSpan)
        {
            if (root == null)
                return null;

            SyntaxToken DiagnosticToken = root.FindToken(diagnosticSpan.Start, findInsideTrivia: true);
            if (DiagnosticToken.Parent == null)
                return null;

            CompilationUnitSyntax CompilationUnit = (CompilationUnitSyntax)root;
            NameExplorer NameExplorer = new NameExplorer(CompilationUnit, null, TraceLevel.Info);

            SyntaxNode? ParentNode = DiagnosticToken.Parent;
            while (ParentNode != null && ParentNode is not StatementSyntax && ParentNode is not ElseClauseSyntax && ParentNode is not SwitchSectionSyntax && ParentNode is not CatchClauseSyntax && ParentNode is not UsingDirectiveSyntax && ParentNode is not MemberDeclarationSyntax)
                ParentNode = ParentNode.Parent;

            if (ParentNode == null)
                return null;

            string IndentationString;

            if (NameExplorer.IsIndentationUsingTab)
                IndentationString = "\t";
            else
            {
                IndentationString = string.Empty;
                for (int i = 0; i < NameExplorer.WhitespaceIndentation; i++)
                    IndentationString += " ";
            }

            string NodeIndentation = string.Empty;
            int ExpectedIndentationLevel = AnalyzerRuleConA1500.GetExpectedIndentationLevel(ParentNode);

            for (int i = 0; i < ExpectedIndentationLevel; i++)
                NodeIndentation += IndentationString;

            string CodeFixMessage = new LocalizableResourceString(nameof(CodeFixResources.ConA1500FixTitle), CodeFixResources.ResourceManager, typeof(CodeFixResources)).ToString();
            CodeAction Action;

            if (ParentNode is DoStatementSyntax AsDoStatement && DiagnosticToken == AsDoStatement.WhileKeyword)
            {
                Action = CodeAction.Create(title: CodeFixMessage,
                        createChangedDocument: c => AsyncWhileHandler(context.Document, AsDoStatement, c, NodeIndentation),
                        equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
            }
            else
            {
                Action = CodeAction.Create(title: CodeFixMessage,
                        createChangedDocument: c => AsyncHandler(context.Document, ParentNode, c, NodeIndentation),
                        equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
            }

            return Action;
        }
    }
}
