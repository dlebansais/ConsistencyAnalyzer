namespace ConsistencyAnalyzer;

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Represents a rule of the analyzer.
/// </summary>
public class AnalyzerRuleConA1200 : SingleSyntaxAnalyzerRule
{
    #region Properties
    /// <summary>
    /// Gets the rule id.
    /// </summary>
    public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1200));

    /// <summary>
    /// Gets the kind of syntax this rule analyzes.
    /// </summary>
    public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.UsingDirective;
    #endregion

    #region Ancestor Interface
    /// <summary>
    /// Gets the rule title.
    /// </summary>
    protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1200Title), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule message format.
    /// </summary>
    protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1200MessageFormat), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule description.
    /// </summary>
    protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1200Description), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule category.
    /// </summary>
    protected override string Category { get; } = "Usage";

    /// <summary>
    /// Gets a value indicating whether using directives should appear before namespace declaration.
    /// </summary>
    public bool? IsOutsideUsingExpected { get; private set; }
    #endregion

    #region Client Interface
    /// <summary>
    /// Analyzes a source code node.
    /// </summary>
    /// <param name="context">The source code.</param>
    public override void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        TraceLevel TraceLevel = TraceLevel.Info;
        Analyzer.Trace("AnalyzerRuleConA1200", TraceLevel);

        try
        {
            UsingDirectiveSyntax Node = (UsingDirectiveSyntax)context.Node;
            string? NodeNameText = NameExplorer.GetNameText(Node.Name);
            string? NodeNameString = NameExplorer.NameToString(Node.Name);

            if (NodeNameText is null || NodeNameString is null)
            {
                Analyzer.Trace($"Null namespace name, exit", TraceLevel);
                return;
            }

            string[] MultiValueText = NodeNameText.Split('.');

            ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
            UsingExplorer Explorer = ContextExplorer.UsingExplorer;

            bool IsOutsideUsing = Node.Parent is not NamespaceDeclarationSyntax;
            IsOutsideUsingExpected = Explorer.IsOutsideUsingExpected;

            if (IsOutsideUsingExpected == null)
            {
                Analyzer.Trace($"Using directive order undecided or not applicable, exit", TraceLevel);
                return;
            }

            if (IsOutsideUsingExpected.Value == false)
            {
                Analyzer.Trace($"handled by ConA1201, exit", TraceLevel);
                return;
            }

            if (IsOutsideUsing == IsOutsideUsingExpected.Value)
            {
                Analyzer.Trace($"Using directive at the right place, exit", TraceLevel);
                return;
            }

            string Expected = IsOutsideUsingExpected.Value ? "outside" : "inside";
            string ValueText = NodeNameString;
            ResetContext();

            Analyzer.Trace($"Using directive at the wrong place, must be {Expected} namespace", TraceLevel);
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), ValueText));
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
