namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.CodeAnalysis.Simplification;
    using Microsoft.CodeAnalysis.Text;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.CodeAnalysis.CodeFixes;
    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// Represents a code fix for a rule of the analyzer.
    /// </summary>
    public class CodeFixConA1602 : CodeFix
    {
        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFixConA1602(AnalyzerRule rule)
            : base(rule)
        {
        }

        private async Task<Document> AsyncHandler(Document document, EnumMemberDeclarationSyntax syntaxNode, CancellationToken cancellationToken)
        {
            int traceId = 0;
            Analyzer.Trace("CodeFixConA1602", ref traceId);

            SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
                return document;

            EnumDeclarationSyntax? Parent = syntaxNode.Parent as EnumDeclarationSyntax;
            if (Parent == null)
                return document;

            DocumentationCommentTriviaSyntax? existingDocumentationComment = syntaxNode.GetDocumentationCommentTriviaSyntax();
            if (existingDocumentationComment != null)
            {
                return document;
            }

            SyntaxTriviaList leadingTrivia = syntaxNode.GetLeadingTrivia();
            int insertionIndex = leadingTrivia.Count;
            while (insertionIndex > 0 && !leadingTrivia[insertionIndex - 1].HasBuiltinEndLine())
            {
                insertionIndex--;
            }

            string newLineText = document.Project.Solution.Workspace.Options.GetOption(FormattingOptions.NewLine, LanguageNames.CSharp);

            var documentationComment = XmlSyntaxFactory.DocumentationComment(newLineText, XmlSyntaxFactory.MultiLineElement(XmlCommentHelper.SummaryXmlTag, newLineText, XmlSyntaxFactory.List(XmlSyntaxFactory.Text(XmlSyntaxFactory.TextNewLine("TODO: Insert documentation here.", false)))));
            SyntaxTrivia docTrivia = SyntaxFactory.Trivia(documentationComment);

            SyntaxTriviaList newLeadingTrivia = leadingTrivia.Insert(insertionIndex, docTrivia);

            if (Parent.Members.IndexOf(syntaxNode) > 0)
            {
                SyntaxTrivia newLineTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.EndOfLineTrivia, newLineText);
                newLeadingTrivia = newLeadingTrivia.Insert(insertionIndex, newLineTrivia);
            }

            SyntaxNode newElement = syntaxNode.WithLeadingTrivia(newLeadingTrivia);
            Document Result =  document.WithSyntaxRoot(root.ReplaceNode(syntaxNode, newElement));

            Analyzer.Trace("Fixed", ref traceId);

            return Result;
        }

        /// <summary>
        /// Creates the action to perform to fix a document.
        /// </summary>
        /// <returns></returns>
        public override CodeAction? CreateDocumentHandler(CodeFixContext context, SyntaxNode root, TextSpan diagnosticSpan)
        {
            if (root == null)
                return null;

            SyntaxToken DiagnosticToken = root.FindToken(diagnosticSpan.Start);
            if (DiagnosticToken.Parent == null)
                return null;

            IEnumerable<SyntaxNode> Nodes = DiagnosticToken.Parent.AncestorsAndSelf();

            var declaration = Nodes.OfType<EnumMemberDeclarationSyntax>().First();

            var Action = CodeAction.Create(title: CodeFixResources.ConA1602FixTitle,
                    createChangedDocument: c => AsyncHandler(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.ConA1602FixTitle));

            return Action;
        }
    }
}
