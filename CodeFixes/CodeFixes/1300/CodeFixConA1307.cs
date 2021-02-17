﻿namespace ConsistencyAnalyzer
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
    public class CodeFixConA1307 : CodeFix
    {
        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFixConA1307(AnalyzerRule rule)
            : base(rule)
        {
        }

        private async Task<Document> AsyncHandler(Document document, DelegateDeclarationSyntax syntaxNode, CancellationToken cancellationToken, string newValueText)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("CodeFixConA1307", TraceLevel);

            SyntaxNode? Root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (Root == null)
                return document;

            SyntaxTriviaList Leading = syntaxNode.Identifier.LeadingTrivia;
            SyntaxTriviaList Trailing = syntaxNode.Identifier.TrailingTrivia;

            DelegateDeclarationSyntax NewNode = syntaxNode.WithIdentifier(SyntaxFactory.Identifier(Leading, newValueText, Trailing));
            SyntaxNode NewRoot = Root.ReplaceNode(syntaxNode, NewNode);

            Document Result = document.WithSyntaxRoot(NewRoot);

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
            DelegateDeclarationSyntax Node = Nodes.OfType<DelegateDeclarationSyntax>().First();
            string ValueText = Node.Identifier.ValueText;
            string NewValueText = NameExplorer.FixName(ValueText, NameCategory.Delegate);

            string CodeFixMessageFormat = new LocalizableResourceString(nameof(CodeFixResources.ConA1307FixTitle), CodeFixResources.ResourceManager, typeof(CodeFixResources)).ToString();
            string FormatedMessage = string.Format(CodeFixMessageFormat, NewValueText);

            var Action = CodeAction.Create(title: FormatedMessage,
                    createChangedDocument: c => AsyncHandler(context.Document, Node, c, NewValueText),
                    equivalenceKey: nameof(CodeFixResources.ConA1307FixTitle));

            return Action;
        }
    }
}