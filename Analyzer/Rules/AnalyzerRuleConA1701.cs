﻿namespace ConsistencyAnalyzer
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
            Analyzer.Trace("AnalyzerRuleConA1701");

            RegionDirectiveTriviaSyntax Node = (RegionDirectiveTriviaSyntax)context.Node;

            if (!GetOwnerClass(Node, out ClassDeclarationSyntax ClassDeclaration))
            {
                Analyzer.Trace("Region outside class, exit");
                return;
            }

            RegionDirectiveTriviaSyntax? RegionOwner = GetRegionDirectiveOwner(ClassDeclaration, Node);

            // Report nested regions.
            if (RegionOwner == null)
            {
                Analyzer.Trace("Region not nested, exit");
                return;
            }

            string RegionText = RegionExplorer.GetRegionText(Node);
            string RegionOwnerText = RegionExplorer.GetRegionText(RegionOwner);

            Analyzer.Trace($"Region {RegionText} is inside {RegionOwnerText}");
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), RegionText, RegionOwnerText));
        }

        private bool GetOwnerClass(RegionDirectiveTriviaSyntax node, out ClassDeclarationSyntax classDeclaration)
        {
            classDeclaration = null!;

            var CurrentToken = node.HashToken;

            do
            {
                CurrentToken = CurrentToken.GetPreviousToken();

                if (CurrentToken.Parent is ClassDeclarationSyntax AsClassDeclaration)
                {
                    classDeclaration = AsClassDeclaration;
                    return true;
                }
            }
            while (CurrentToken != null);

            return false;
        }

        private RegionDirectiveTriviaSyntax? GetRegionDirectiveOwner(ClassDeclarationSyntax classDeclaration, RegionDirectiveTriviaSyntax regionDirective)
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
