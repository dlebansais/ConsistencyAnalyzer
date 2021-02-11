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
    public class CodeFixConA1300 : CodeFix
    {
        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFixConA1300(AnalyzerRule rule)
            : base(rule)
        {
        }

        private async Task<Document> AsyncHandler(Document document, NamespaceDeclarationSyntax syntaxNode, CancellationToken cancellationToken, string newValueText)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1300", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SimpleNameSyntax? SimpleName = syntaxNode.Name as SimpleNameSyntax;
            if (SimpleName == null)
                return document;

            var Leading = SimpleName.Identifier.LeadingTrivia;
            var Trailing = SimpleName.Identifier.TrailingTrivia;

            var NewSimpleName = SimpleName.WithIdentifier(SyntaxFactory.Identifier(Leading, newValueText, Trailing));
            var newRoot = Root.ReplaceNode(SimpleName, NewSimpleName);

            Document Result = document.WithSyntaxRoot(newRoot);

            Analyzer.Trace("Fixed", TraceLevel);

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

            SyntaxToken DiagnosticToken = root.FindToken(diagnosticSpan.Start, findInsideTrivia: true);
            if (DiagnosticToken.Parent == null)
                return null;

            CompilationUnitSyntax CompilationUnit = (CompilationUnitSyntax)root;
            NameExplorer NameExplorer = new NameExplorer(CompilationUnit, TraceLevel.Info);

            IEnumerable<SyntaxNode> Nodes = DiagnosticToken.Parent.AncestorsAndSelf();
            NamespaceDeclarationSyntax Node = Nodes.OfType<NamespaceDeclarationSyntax>().First();
            SimpleNameSyntax SimpleName = (SimpleNameSyntax)Node.Name;
            string ValueText = NameExplorer.GetName(SimpleName);

            string NewValueText = NameExplorer.FixName(ValueText, NameCategory.Namespace);

            string CodeFixMessageFormat = new LocalizableResourceString(nameof(CodeFixResources.ConA1300FixTitle), CodeFixResources.ResourceManager, typeof(CodeFixResources)).ToString();
            string FormatedMessage = string.Format(CodeFixMessageFormat, NewValueText);

            var Action = CodeAction.Create(title: FormatedMessage,
                    createChangedDocument: c => AsyncHandler(context.Document, Node, c, NewValueText),
                    equivalenceKey: nameof(CodeFixResources.ConA1300FixTitle));

            return Action;
        }
    }
}
