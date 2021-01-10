namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Provider)), Shared]
    public class Provider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get 
            {
                List<string> IdList = new List<string>();
                foreach (KeyValuePair<string, CodeFix> Entry in CodeFix.CodeFixTable)
                {
                    IdList.Add(Entry.Key);
                }

                return IdList.ToImmutableArray();
            }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                CodeFix CodeFix = CodeFix.CodeFixTable[diagnostic.Id];
                var diagnosticSpan = diagnostic.Location.SourceSpan;

                var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().First();

                var Action = CodeAction.Create( title: CodeFixResources.CodeFixTitle,
                        createChangedDocument: c => CodeFix.AsyncHandler(context.Document, declaration, c),
                        equivalenceKey: nameof(CodeFixResources.CodeFixTitle));

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(Action, diagnostic);
            }
        }
    }
}
