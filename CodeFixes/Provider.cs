namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Threading.Tasks;

    /// <summary>
    /// Represent a provider of source code fixes.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Provider)), Shared]
    public class Provider : CodeFixProvider
    {
        /// <summary>
        /// Gets the list of diagnostic IDs that this provider can provide fixes for.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get 
            {
                List<string> IdList = new List<string>();
                foreach (KeyValuePair<string, CodeFix> Entry in CodeFix.CodeFixTable)
                    IdList.Add(Entry.Key);

                return IdList.ToImmutableArray();
            }
        }

        /// <summary>
        /// Gets an optional Microsoft.CodeAnalysis.CodeFixes.FixAllProvider.
        /// </summary>
        /// <returns></returns>
        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>
        /// Computes one or more fixes for the specified Microsoft.CodeAnalysis.CodeFixes.CodeFixContext.
        /// </summary>
        /// <param name="context">A Microsoft.CodeAnalysis.CodeFixes.CodeFixContext containing context information.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;

            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                CodeFix CodeFix = CodeFix.CodeFixTable[diagnostic.Id];
                var diagnosticSpan = diagnostic.Location.SourceSpan;

                CodeAction? Action = CodeFix.CreateDocumentHandler(context, root, diagnosticSpan);

                // Register a code action that will invoke the fix.
                if (Action != null)
                    context.RegisterCodeFix(Action, diagnostic);
            }
        }
    }
}
