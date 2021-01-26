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
    public class AnalyzerRuleConA1701 : SingleSyntaxAnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1701));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.RegionDirectiveTrivia;
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1701Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1701MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1701Description), Resources.ResourceManager, typeof(Resources));

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
            int TraceId = 0;
            Analyzer.Trace("AnalyzerRuleConA1701", ref TraceId);

            RegionDirectiveTriviaSyntax Node = (RegionDirectiveTriviaSyntax)context.Node;

            var CurrentToken = Node.HashToken;

            do
            {
                CurrentToken = CurrentToken.GetPreviousToken();

                if (CurrentToken.Parent is ClassDeclarationSyntax AsClassDeclaration)
                {
                    AnalyzeNode(Node, context, AsClassDeclaration, TraceId);
                    break;
                }
            }
            while (CurrentToken != null);
        }

        private void AnalyzeNode(RegionDirectiveTriviaSyntax node, SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration, int traceId)
        {
            RegionDirectiveTriviaSyntax? RegionOwner = GetRegionDirectiveOwner(classDeclaration, node, traceId);

            // Report nested regions.
            if (RegionOwner != null)
            {
                string RegionText = GetRegionText(node, traceId);
                string RegionOwnerText = GetRegionText(RegionOwner, traceId);

                Analyzer.Trace($"Region {RegionText} is inside {RegionOwnerText}", ref traceId);
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation(), RegionText, RegionOwnerText));
            }
        }

        private string GetRegionText(RegionDirectiveTriviaSyntax regionDirective, int traceId)
        {
            string Result = regionDirective.ToString();
            string RegionPattern = "#region ";

            if (Result.StartsWith(RegionPattern))
                Result = Result.Substring(RegionPattern.Length);

            return Result;
        }

        private RegionDirectiveTriviaSyntax? GetRegionDirectiveOwner(ClassDeclarationSyntax classDeclaration, RegionDirectiveTriviaSyntax regionDirective, int traceId)
        {
            var CurrentToken = classDeclaration.OpenBraceToken;
            RegionDirectiveTriviaSyntax? RegionOwner = null;

            for (;;)
            {
                CurrentToken = CurrentToken.GetNextToken(includeZeroWidth: false, includeSkipped: false, includeDirectives: true, includeDocumentationComments: false);

                if (CurrentToken == classDeclaration.CloseBraceToken)
                    break;

                if (CurrentToken.Parent == regionDirective)
                    break;

                if (CurrentToken.Parent is RegionDirectiveTriviaSyntax AsRegionDirective)
                {
                    if (AsRegionDirective.HashToken == CurrentToken)
                        RegionOwner = AsRegionDirective;
                }
                else if (CurrentToken.Parent is EndRegionDirectiveTriviaSyntax AsEndRegionDirective)
                {
                    if (AsEndRegionDirective.HashToken == CurrentToken)
                        RegionOwner = null;
                }
            }

            return RegionOwner;
        }
        #endregion
    }
}
