namespace ConsistencyAnalyzer;

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using StyleCop.Analyzers.Helpers;

/// <summary>
/// Represents a rule of the analyzer.
/// </summary>
public class AnalyzerRuleConA1703 : MultipleSyntaxAnalyzerRule
{
    #region Properties
    /// <summary>
    /// Gets the rule id.
    /// </summary>
    public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1703));

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
    protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1703Title), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule message format.
    /// </summary>
    protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1703MessageFormat), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule description.
    /// </summary>
    protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1703Description), Resources.ResourceManager, typeof(Resources));

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
        Analyzer.Trace("AnalyzerRuleConA1703", TraceLevel);

        try
        {
            MemberDeclarationSyntax Node = (MemberDeclarationSyntax)context.Node;

            if (!RegionExplorer.IsRegionMismatch(context, Node, AccessLevel.Protected, isSimpleAccessibilityCheck: true, TraceLevel, out string ExpectedRegionText, out string MemberText))
                return;

            Analyzer.Trace($"Member {MemberText} should be inside {ExpectedRegionText}", TraceLevel);
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), MemberText, ExpectedRegionText));
        }
        catch (Exception e)
        {
            Analyzer.Trace($"{e.Message}\n{e.StackTrace}", TraceLevel.Critical);

            throw e;
        }
    }
    #endregion
}
