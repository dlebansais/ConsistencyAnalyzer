﻿namespace ConsistencyAnalyzer
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
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("AnalyzerRuleConA1700", TraceLevel);

            ClassDeclarationSyntax Node = (ClassDeclarationSyntax)context.Node;
            RegionExplorer Explorer = ContextExplorer.Get(context, TraceLevel).GetRegionExplorer(Node);

            if (!Explorer.HasRegion)
            {
                Analyzer.Trace("No region to analyze, exit", TraceLevel);
                return;
            }

            ProgramHasMembersOutsideRegion.Update(Explorer.HasMembersOutsideRegion);

            // Report for classes with members outside region only.
            if (!Explorer.HasMembersOutsideRegion)
            {
                Analyzer.Trace("No member outside region, exit", TraceLevel);
                return;
            }

            if (!ProgramHasMembersOutsideRegion.IsDifferent)
            {
                Analyzer.Trace("No difference with other classes, exit", TraceLevel);
                return;
            }

            string ClassName = Node.Identifier.ValueText;
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), ClassName));
        }

        private GlobalState<bool> ProgramHasMembersOutsideRegion = new GlobalState<bool>();
        #endregion
    }
}
