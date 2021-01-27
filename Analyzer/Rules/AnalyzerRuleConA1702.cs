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
    public class AnalyzerRuleConA1702 : MultipleSyntaxAnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1702));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override List<SyntaxKind> RuleSyntaxKinds { get; } = new List<SyntaxKind>()
        {
            SyntaxKind.ConstructorDeclaration,
            SyntaxKind.FieldDeclaration,
            SyntaxKind.MethodDeclaration,
            SyntaxKind.PropertyDeclaration,
            //SyntaxKind.DelegateDeclaration,
            //SyntaxKind.EventDeclaration,
        };
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1702Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1702MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1702Description), Resources.ResourceManager, typeof(Resources));

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
            Analyzer.Trace("AnalyzerRuleConA1702", ref TraceId);

            MemberDeclarationSyntax Node = (MemberDeclarationSyntax)context.Node;

            bool IsPublic = AccessLevelHelper.GetAccessLevel(Node.Modifiers) == AccessLevel.Public;
            if (!IsPublic)
                return;

            Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax> MemberToClassTable;
            Dictionary<ClassDeclarationSyntax, RegionExplorer> RegionExplorerTable;

            lock (ClassExplorer.Current)
                MemberToClassTable = ClassExplorer.Current.MemberToClassTable;

            if (!MemberToClassTable.ContainsKey(Node))
                return;

            ClassDeclarationSyntax ClassDeclaration = MemberToClassTable[Node];

            lock (ClassExplorer.Current)
                RegionExplorerTable = ClassExplorer.Current.RegionExplorerTable;

            RegionExplorer Explorer;

            lock (ClassExplorer.Current)
            {
                Explorer = RegionExplorerTable[ClassDeclaration];
            }

            if (!Explorer.RegionsByAccelLevel.ContainsKey(AccessLevel.Public))
                return;

            List<RegionDirectiveTriviaSyntax> RegionList = Explorer.RegionsByAccelLevel[AccessLevel.Public];
            if (RegionList.Count <= 1)
                return;

            RegionDirectiveTriviaSyntax FirstRegion = RegionList[0];
            RegionDirectiveTriviaSyntax MemberRegion = Explorer.MemberRegionTable[Node];

            if (MemberRegion == FirstRegion)
                return;

            string FirstRegionText = RegionExplorer.GetRegionText(FirstRegion);
            string MemberText = "<Unknown>";

            switch (Node)
            {
                case ConstructorDeclarationSyntax AsConstructorDeclaration:
                    MemberText = AsConstructorDeclaration.Identifier.ToString();
                    break;
                case FieldDeclarationSyntax AsFieldDeclaration:
                    MemberText = AsFieldDeclaration.Declaration.Variables[0].Identifier.ToString();
                    break;
                case MethodDeclarationSyntax AsMethodDeclaration:
                    MemberText = AsMethodDeclaration.Identifier.ToString();
                    break;
                case PropertyDeclarationSyntax AsPropertyDeclaration:
                    MemberText = AsPropertyDeclaration.Identifier.ToString();
                    break;
            }

            Analyzer.Trace($"Member {MemberText} shoudd be inside {FirstRegionText}", ref TraceId);
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), MemberText, FirstRegionText));
        }
        #endregion
    }
}
