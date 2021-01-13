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
    public class AnalyzerRuleConA1602 : AnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1602));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.EnumMemberDeclaration;
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1602Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1602MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1602Description), Resources.ResourceManager, typeof(Resources));

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
            Analyzer.Trace("AnalyzerRuleConA1602", ref traceId);

            EnumMemberDeclarationSyntax Declaration = (EnumMemberDeclarationSyntax)context.Node;

            EnumDeclarationSyntax? Parent = Declaration.Parent as EnumDeclarationSyntax;
            if (Parent == null)
                return;

            bool IsDocumented = XmlCommentHelper.HasDocumentation(Declaration);
            List<EnumMemberDeclarationSyntax> OtherEnumList = new List<EnumMemberDeclarationSyntax>(Parent.Members);

            Analyzer.Trace($"{Declaration.Identifier}: {IsDocumented}", ref traceId);

            foreach (EnumMemberDeclarationSyntax OtherEnum in OtherEnumList)
            {
                bool IsOtherDocumented = XmlCommentHelper.HasDocumentation(OtherEnum);

                Analyzer.Trace($"{Declaration.Identifier}: {IsDocumented}, {OtherEnum.Identifier}: {IsOtherDocumented}", ref traceId);

                if (OtherEnum == Declaration)
                    break;

                if (IsOtherDocumented != IsDocumented)
                {
                    string EnumName = Declaration.Identifier.ValueText;
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, context.Node.GetLocation(), EnumName));
                    break;
                }
            }
        }
        #endregion
    }
}
