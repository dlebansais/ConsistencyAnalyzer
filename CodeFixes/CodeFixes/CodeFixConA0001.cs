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

    /// <summary>
    /// Represents a code fix for a rule of the analyzer.
    /// </summary>
    public class CodeFixConA0001 : CodeFix
    {
        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFixConA0001(AnalyzerRule rule)
            : base(rule)
        {
        }

        private async Task<Document> AsyncHandler(Document document, LocalDeclarationStatementSyntax localDeclaration, CancellationToken cancellationToken)
        {
            // Remove the leading trivia from the local declaration.
            var firstToken = localDeclaration.GetFirstToken();
            var leadingTrivia = firstToken.LeadingTrivia;
            LocalDeclarationStatementSyntax trimmedLocal = localDeclaration.ReplaceToken(
                firstToken, firstToken.WithLeadingTrivia(SyntaxTriviaList.Empty));
            string identifier = trimmedLocal.Declaration.Variables.First().Identifier.ToString();

            Analyzer.Trace($"Fixing {identifier}");

            // Create a const token with the leading trivia.
            var constToken = SyntaxFactory.Token(leadingTrivia, SyntaxKind.ConstKeyword, SyntaxFactory.TriviaList(SyntaxFactory.ElasticMarker));

            // Insert the const token into the modifiers list, creating a new modifiers list.
            var newModifiers = trimmedLocal.Modifiers.Insert(0, constToken);

            // If the type of the declaration is 'var', create a new type name
            // for the inferred type.
            var variableDeclaration = localDeclaration.Declaration;
            var variableTypeName = variableDeclaration.Type;
            if (variableTypeName.IsVar)
            {
                var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

                // Special case: Ensure that 'var' isn't actually an alias to another type
                // (e.g. using var = System.String).
                var aliasInfo = semanticModel?.GetAliasInfo(variableTypeName);
                if (aliasInfo == null)
                {
                    // Retrieve the type inferred for var.
                    var type = semanticModel.GetTypeInfo(variableTypeName).ConvertedType;

                    // Special case: Ensure that 'var' isn't actually a type named 'var'.
                    if (type != null && type.Name != "var")
                    {
                        // Create a new TypeSyntax for the inferred type. Be careful
                        // to keep any leading and trailing trivia from the var keyword.
                        var typeName = SyntaxFactory.ParseTypeName(type.ToDisplayString())
                            .WithLeadingTrivia(variableTypeName.GetLeadingTrivia())
                            .WithTrailingTrivia(variableTypeName.GetTrailingTrivia());

                        // Add an annotation to simplify the type name.
                        var simplifiedTypeName = typeName.WithAdditionalAnnotations(Simplifier.Annotation);

                        // Replace the type in the variable declaration.
                        variableDeclaration = variableDeclaration.WithType(simplifiedTypeName);
                    }
                }
            }

            // Produce the new local declaration.
            var newLocal = trimmedLocal.WithModifiers(newModifiers)
                                       .WithDeclaration(variableDeclaration);

            // Add an annotation to format the new local declaration.
            var formattedLocal = newLocal.WithAdditionalAnnotations(Formatter.Annotation);

            // Replace the old local declaration with the new local declaration.
            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            if (oldRoot != null)
            {
                var newRoot = oldRoot.ReplaceNode(localDeclaration, formattedLocal);

                // Return document with transformed tree.
                return document.WithSyntaxRoot(newRoot);
            }
            else
                return document;
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

            var declaration = Nodes.OfType<LocalDeclarationStatementSyntax>().First();

            var Action = CodeAction.Create(title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: c => AsyncHandler(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle));

            return Action;
        }
    }
}
