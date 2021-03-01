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

        private async Task<Document> AsyncNamespaceOpenBraceHandler(Document document, NamespaceDeclarationSyntax namespaceDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken OpenBraceToken = namespaceDeclaration.OpenBraceToken;

            if (OpenBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = OpenBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = OpenBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = namespaceDeclaration.WithOpenBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(namespaceDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncNamespaceCloseBraceHandler(Document document, NamespaceDeclarationSyntax namespaceDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken CloseBraceToken = namespaceDeclaration.CloseBraceToken;

            if (CloseBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = CloseBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = CloseBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = namespaceDeclaration.WithCloseBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(namespaceDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncBaseTypeOpenBraceHandler(Document document, BaseTypeDeclarationSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken OpenBraceToken = baseTypeDeclaration.OpenBraceToken;

            if (OpenBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = OpenBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = OpenBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithOpenBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncBaseTypeCloseBraceHandler(Document document, BaseTypeDeclarationSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken CloseBraceToken = baseTypeDeclaration.CloseBraceToken;

            if (CloseBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = CloseBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = CloseBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithCloseBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncAccessorListOpenBraceHandler(Document document, AccessorListSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken OpenBraceToken = baseTypeDeclaration.OpenBraceToken;

            if (OpenBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = OpenBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = OpenBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithOpenBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncAccessorListCloseBraceHandler(Document document, AccessorListSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken CloseBraceToken = baseTypeDeclaration.CloseBraceToken;

            if (CloseBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = CloseBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = CloseBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithCloseBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncBlockOpenBraceHandler(Document document, BlockSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken OpenBraceToken = baseTypeDeclaration.OpenBraceToken;

            if (OpenBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = OpenBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = OpenBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithOpenBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncBlockCloseBraceHandler(Document document, BlockSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken CloseBraceToken = baseTypeDeclaration.CloseBraceToken;

            if (CloseBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = CloseBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = CloseBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithCloseBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncSwitchOpenBraceHandler(Document document, SwitchStatementSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken OpenBraceToken = baseTypeDeclaration.OpenBraceToken;

            if (OpenBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = OpenBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = OpenBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithOpenBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private async Task<Document> AsyncSwitchCloseBraceHandler(Document document, SwitchStatementSyntax baseTypeDeclaration, CancellationToken cancellationToken, string nodeIndentation)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1500", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTrivia IndentationTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, nodeIndentation);
            SyntaxTriviaList NewLeadingTrivia = SyntaxTriviaList.Create(IndentationTrivia);
            SyntaxToken CloseBraceToken = baseTypeDeclaration.CloseBraceToken;

            if (CloseBraceToken.HasLeadingTrivia)
            {
                SyntaxTriviaList LeadingTrivia = CloseBraceToken.LeadingTrivia;
                if (LeadingTrivia.Count > 0 && LeadingTrivia[0].IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    NewLeadingTrivia = new SyntaxTriviaList(new SyntaxTrivia[] { LeadingTrivia[0], IndentationTrivia });
                }
            }

            SyntaxToken NewToken = CloseBraceToken.WithLeadingTrivia(NewLeadingTrivia);
            SyntaxNode NewNode = baseTypeDeclaration.WithCloseBraceToken(NewToken);
            SyntaxNode NewRoot = Root.ReplaceNode(baseTypeDeclaration, NewNode);

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
            while (ParentNode != null &&
                   ParentNode is not AccessorListSyntax &&
                   ParentNode is not AccessorDeclarationSyntax &&
                   ParentNode is not StatementSyntax && 
                   ParentNode is not ElseClauseSyntax && 
                   ParentNode is not SwitchSectionSyntax && 
                   ParentNode is not CatchClauseSyntax && 
                   ParentNode is not UsingDirectiveSyntax && 
                   ParentNode is not MemberDeclarationSyntax)
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
            bool IsInNamespace = AnalyzerRuleConA1500.CheckIfIsInNamespace(ParentNode);
            int ExpectedIndentationLevel = AnalyzerRuleConA1500.GetExpectedIndentationLevel(ParentNode, IsInNamespace);

            for (int i = 0; i < ExpectedIndentationLevel; i++)
                NodeIndentation += IndentationString;

            string CodeFixMessage = new LocalizableResourceString(nameof(CodeFixResources.ConA1500FixTitle), CodeFixResources.ResourceManager, typeof(CodeFixResources)).ToString();

            CodeAction? Action = null;

            switch (ParentNode)
            {
                case NamespaceDeclarationSyntax AsNamespaceDeclaration:
                    if (DiagnosticToken == AsNamespaceDeclaration.OpenBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncNamespaceOpenBraceHandler(context.Document, AsNamespaceDeclaration, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    else if (DiagnosticToken == AsNamespaceDeclaration.CloseBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncNamespaceCloseBraceHandler(context.Document, AsNamespaceDeclaration, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    break;

                case BaseTypeDeclarationSyntax AsBaseTypeDeclaration:
                    if (DiagnosticToken == AsBaseTypeDeclaration.OpenBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncBaseTypeOpenBraceHandler(context.Document, AsBaseTypeDeclaration, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    else if (DiagnosticToken == AsBaseTypeDeclaration.CloseBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncBaseTypeCloseBraceHandler(context.Document, AsBaseTypeDeclaration, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    break;

                case AccessorListSyntax AsAccessorList:
                    if (DiagnosticToken == AsAccessorList.OpenBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncAccessorListOpenBraceHandler(context.Document, AsAccessorList, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    else if (DiagnosticToken == AsAccessorList.CloseBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncAccessorListCloseBraceHandler(context.Document, AsAccessorList, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    break;

                case BlockSyntax AsBlock:
                    if (DiagnosticToken == AsBlock.OpenBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncBlockOpenBraceHandler(context.Document, AsBlock, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    else if (DiagnosticToken == AsBlock.CloseBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncBlockCloseBraceHandler(context.Document, AsBlock, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    break;

                case SwitchStatementSyntax AsSwitchStatement:
                    if (DiagnosticToken == AsSwitchStatement.OpenBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncSwitchOpenBraceHandler(context.Document, AsSwitchStatement, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    else if (DiagnosticToken == AsSwitchStatement.CloseBraceToken)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncSwitchCloseBraceHandler(context.Document, AsSwitchStatement, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    break;

                case DoStatementSyntax AsDoStatement:
                    if (DiagnosticToken == AsDoStatement.WhileKeyword)
                    {
                        Action = CodeAction.Create(title: CodeFixMessage,
                                createChangedDocument: c => AsyncWhileHandler(context.Document, AsDoStatement, c, NodeIndentation),
                                equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
                    }
                    break;
            }

            if (Action == null)
            {
                Action = CodeAction.Create(title: CodeFixMessage,
                        createChangedDocument: c => AsyncHandler(context.Document, ParentNode, c, NodeIndentation),
                        equivalenceKey: nameof(CodeFixResources.ConA1500FixTitle));
            }

            return Action;
        }
    }
}
