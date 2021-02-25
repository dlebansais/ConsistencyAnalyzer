namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using StyleCop.Analyzers.Settings.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a code fix for a rule of the analyzer.
    /// </summary>
    public class CodeFixConA1203 : CodeFixUsingReorder
    {
        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFixConA1203(AnalyzerRule rule)
            : base(rule)
        {
        }

        private async Task<Document> AsyncHandler(Document document, SyntaxNode syntaxRoot, UsingDirectiveSyntax syntaxNode, CancellationToken cancellationToken, bool systemUsingDirectivesFirst)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1203", TraceLevel);

            return await AsyncUsingReorder(document, syntaxRoot, syntaxNode, cancellationToken, UsingDirectivesPlacement.Preserve, systemUsingDirectivesFirst);
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
            UsingExplorer UsingExplorer = new UsingExplorer(CompilationUnit, null, TraceLevel.Info);
            bool SystemUsingDirectivesFirst = UsingExplorer.IsSystemUsingFirstExpected.HasValue && UsingExplorer.IsSystemUsingFirstExpected.Value;

            IEnumerable <SyntaxNode> Nodes = DiagnosticToken.Parent.AncestorsAndSelf();
            UsingDirectiveSyntax Node = Nodes.OfType<UsingDirectiveSyntax>().First();

            string CodeFixMessage = new LocalizableResourceString(nameof(CodeFixResources.ConA1203FixTitle), CodeFixResources.ResourceManager, typeof(CodeFixResources)).ToString();

            var Action = CodeAction.Create(title: CodeFixMessage,
                    createChangedDocument: c => AsyncHandler(context.Document, root, Node, c, SystemUsingDirectivesFirst),
                    equivalenceKey: nameof(CodeFixResources.ConA1203FixTitle));

            return Action;
        }
    }
}
