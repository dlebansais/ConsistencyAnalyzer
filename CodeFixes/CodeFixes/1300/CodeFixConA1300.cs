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

        private async Task<Document> AsyncHandler(Document document, NamespaceDeclarationSyntax syntaxNode, CancellationToken cancellationToken)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1300", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            CompilationUnitSyntax? CompilationUnit = Root as CompilationUnitSyntax;
            if (CompilationUnit == null)
                return document;

            SimpleNameSyntax? SimpleName = syntaxNode.Name as SimpleNameSyntax;
            if (SimpleName == null)
                return document;

            var Leading = SimpleName.Identifier.LeadingTrivia;
            var Trailing = SimpleName.Identifier.TrailingTrivia;

            NameExplorer NameExplorer = new NameExplorer(CompilationUnit, TraceLevel.Info);
            string NewValueText = NameExplorer.FixName(SimpleName.Identifier.ValueText, NameCategory.Namespace);

            var NewSimpleName = SimpleName.WithIdentifier(SyntaxFactory.Identifier(Leading, NewValueText, Trailing));
            var newRoot = CompilationUnit.ReplaceNode(SimpleName, NewSimpleName);

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

            IEnumerable<SyntaxNode> Nodes = DiagnosticToken.Parent.AncestorsAndSelf();
            NamespaceDeclarationSyntax Node = Nodes.OfType<NamespaceDeclarationSyntax>().First();
            string Name = NameExplorer.GetName(Node.Name);

            string CodeFixMessageFormat = new LocalizableResourceString(nameof(CodeFixResources.ConA1300FixTitle), CodeFixResources.ResourceManager, typeof(CodeFixResources)).ToString();
            string FormatedMessage = string.Format(CodeFixMessageFormat, Name);

            var Action = CodeAction.Create(title: FormatedMessage,
                    createChangedDocument: c => AsyncHandler(context.Document, Node, c),
                    equivalenceKey: nameof(CodeFixResources.ConA1300FixTitle));

            return Action;
        }
    }
}
