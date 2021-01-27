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
    public class AnalyzerRuleConA1700 : SingleSyntaxAnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1700));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.ClassDeclaration;
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1700Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1700MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1700Description), Resources.ResourceManager, typeof(Resources));

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
            int traceId = 0;
            Analyzer.Trace("AnalyzerRuleConA1700", ref traceId);

            ClassDeclarationSyntax Node = (ClassDeclarationSyntax)context.Node;

            lock (ClassExplorer.Current)
                ClassExplorer.Current.AddClass(Node);

            RegionExplorer Explorer;

            lock (ClassExplorer.Current)
                Explorer = ClassExplorer.Current.RegionExplorerTable[Node];

            if (Explorer.HasRegion)
            {
                lock (ProgramHasMembersOutsideRegion)
                {
                    ProgramHasMembersOutsideRegion.Update(Explorer.HasMembersOutsideRegion);
                }

                // Report for classes with members outside region only.
                if (ProgramHasMembersOutsideRegion.IsDifferent && Explorer.HasMembersOutsideRegion)
                {
                    string ClassName = Node.Identifier.ValueText;
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), ClassName));
                }
            }
        }

        private GlobalState<bool> ProgramHasMembersOutsideRegion = new GlobalState<bool>();
        #endregion
    }
}
