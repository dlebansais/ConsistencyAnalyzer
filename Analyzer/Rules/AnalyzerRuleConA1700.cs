namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rule of the analyzer.
    /// </summary>
    public class AnalyzerRuleConA1700 : AnalyzerRule
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
            
            var CurrentToken = Node.OpenBraceToken;
            int RegionNestedLevel = 0;
            bool HasRegion = false;
            bool HasMembersOutsideRegion = false;

            for (;;)
            {
                CurrentToken = CurrentToken.GetNextToken(includeZeroWidth: false, includeSkipped: false, includeDirectives: true, includeDocumentationComments: false);

                if (CurrentToken == Node.CloseBraceToken)
                    break;

                if (CurrentToken.Parent is RegionDirectiveTriviaSyntax AsRegionDirective)
                {
                    if (AsRegionDirective.HashToken == CurrentToken)
                    {
                        HasRegion = true;
                        RegionNestedLevel++;
                    }
                }
                else if (CurrentToken.Parent is EndRegionDirectiveTriviaSyntax AsEndRegionDirective)
                {
                    if (AsEndRegionDirective.HashToken == CurrentToken)
                        RegionNestedLevel--;
                }
                else if (CurrentToken.Parent is MemberDeclarationSyntax)
                {
                    if (RegionNestedLevel == 0)
                    {
                        if (!HasMembersOutsideRegion)
                            HasMembersOutsideRegion = true;
                    }
                }
            }

            if (HasRegion)
            {
                lock (ProgramHasMembersOutsideRegion)
                {
                    ProgramHasMembersOutsideRegion.Update(HasMembersOutsideRegion);
                }
            }

            // Report for classes with members outside region only.
            if (ProgramHasMembersOutsideRegion.IsDifferent && HasMembersOutsideRegion)
            {
                string ClassName = Node.Identifier.ValueText;
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), ClassName));
            }
        }

        private GlobalState<bool> ProgramHasMembersOutsideRegion = new GlobalState<bool>();
        #endregion
    }
}
