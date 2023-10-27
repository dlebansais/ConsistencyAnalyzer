namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rule of the analyzer.
    /// </summary>
    public class AnalyzerRuleConA1300 : SingleSyntaxAnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1300));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.NamespaceDeclaration;
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1300Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1300MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1300Description), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule category.
        /// </summary>
        protected override string Category { get; } = "Usage";
        #endregion

        #region Client Interface
        /// <summary>
        /// Analyzes a source code node.
        /// </summary>
        /// <param name="context">The source code.</param>
        public override void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("AnalyzerRuleConA1300", TraceLevel);

            try
            {
                NamespaceDeclarationSyntax Node = (NamespaceDeclarationSyntax)context.Node;
                string? NodeNameText = NameExplorer.GetNameText(Node.Name);

                if (NodeNameText is null)
                {
                    Analyzer.Trace($"Null namespace declaration, exit", TraceLevel);
                    return;
                }

                string[] MultiValueText = NodeNameText.Split('.');
                ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
                NameExplorer Explorer = ContextExplorer.NameExplorer;

                foreach (string ValueText in MultiValueText)
                    if (Explorer.IsNameMismatch(ValueText, NameCategory.Namespace, out NamingSchemes ExpectedSheme, TraceLevel))
                    {
                        Analyzer.Trace($"Name {ValueText} is not consistent with the expected {ExpectedSheme} scheme", TraceLevel);
                        context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), ValueText));
                    }
            }
            catch (Exception e)
            {
                Analyzer.Trace(e.Message, TraceLevel.Critical);
                Analyzer.Trace(e.StackTrace, TraceLevel.Critical);

                throw e;
            }
        }
        #endregion
    }
}
