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

            Document Result;

            switch (syntaxNode.Name)
            {
                case SimpleNameSyntax AsSimpleName:
                    Result = FixSimpleName(document, Root, AsSimpleName, newValueText);
                    break;
                case QualifiedNameSyntax AsQualifiedName:
                    Result = FixQualifiedName(document, Root, AsQualifiedName, newValueText);
                    break;
                default:
                    return document;
            }

            Analyzer.Trace("Fixed", TraceLevel);

            return Result;
        }

        private Document FixSimpleName(Document document, SyntaxNode root, SimpleNameSyntax simpleName, string newValueText)
        {
            SyntaxTriviaList Leading = simpleName.Identifier.LeadingTrivia;
            SyntaxTriviaList Trailing = simpleName.Identifier.TrailingTrivia;

            SimpleNameSyntax NewSimpleName = simpleName.WithIdentifier(SyntaxFactory.Identifier(Leading, newValueText, Trailing));
            SyntaxNode NewRoot = root.ReplaceNode(simpleName, NewSimpleName);

            return document.WithSyntaxRoot(NewRoot);
        }

        private Document FixQualifiedName(Document document, SyntaxNode root, QualifiedNameSyntax qualifiedName, string newValueText)
        {
            NameSyntax Current = qualifiedName;
            SyntaxTriviaList Leading = qualifiedName.GetLeadingTrivia();
            SyntaxTriviaList Trailing = qualifiedName.GetTrailingTrivia();
            string[] Splitted = newValueText.Split('.');

            NameSyntax Name = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(Leading, Splitted[0], SyntaxTriviaList.Empty));

            for (int i = 1; i < Splitted.Length; i++)
            {
                SimpleNameSyntax RightName;
                string Text = Splitted[i];

                if (i + 1 < Splitted.Length)
                    RightName = SyntaxFactory.IdentifierName(Text);
                else
                    RightName = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(SyntaxTriviaList.Empty, Text, Trailing));

                Name = SyntaxFactory.QualifiedName(Name, RightName);
            }

            QualifiedNameSyntax NewQualifiedName = (QualifiedNameSyntax)Name;
            SyntaxNode NewRoot = root.ReplaceNode(qualifiedName, NewQualifiedName);

            return document.WithSyntaxRoot(NewRoot);
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
            NameExplorer NameExplorer = new NameExplorer(CompilationUnit, null, TraceLevel.Info);

            IEnumerable<SyntaxNode> Nodes = DiagnosticToken.Parent.AncestorsAndSelf();
            NamespaceDeclarationSyntax Node = Nodes.OfType<NamespaceDeclarationSyntax>().First();
            string[] MultiValueText = NameExplorer.GetNameText(Node.Name).Split('.');

            string NewValueText = string.Empty;
            foreach (string ValueText in MultiValueText)
            {
                if (NewValueText.Length > 0)
                    NewValueText += ".";

                NewValueText += NameExplorer.FixName(ValueText, NameCategory.Namespace);
            }

            string CodeFixMessageFormat = new LocalizableResourceString(nameof(CodeFixResources.ConA1300FixTitle), CodeFixResources.ResourceManager, typeof(CodeFixResources)).ToString();
            string FormatedMessage = string.Format(CodeFixMessageFormat, NewValueText);

            var Action = CodeAction.Create(title: FormatedMessage,
                    createChangedDocument: c => AsyncHandler(context.Document, Node, c, NewValueText),
                    equivalenceKey: nameof(CodeFixResources.ConA1300FixTitle));

            return Action;
        }
    }
}
