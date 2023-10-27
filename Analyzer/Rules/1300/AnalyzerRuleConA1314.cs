namespace ConsistencyAnalyzer;

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Represents a rule of the analyzer.
/// </summary>
public class AnalyzerRuleConA1314 : SingleSyntaxAnalyzerRule
{
    #region Properties
    /// <summary>
    /// Gets the rule id.
    /// </summary>
    public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1314));

    /// <summary>
    /// Gets the kind of syntax this rule analyzes.
    /// </summary>
    public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.VariableDeclarator;
    #endregion

    #region Ancestor Interface
    /// <summary>
    /// Gets the rule title.
    /// </summary>
    protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1314Title), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule message format.
    /// </summary>
    protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1314MessageFormat), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule description.
    /// </summary>
    protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1314Description), Resources.ResourceManager, typeof(Resources));

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
        Analyzer.Trace("AnalyzerRuleConA1314", TraceLevel);

        try
        {
            VariableDeclaratorSyntax Node = (VariableDeclaratorSyntax)context.Node;
            string ValueText = Node.Identifier.ValueText;

            ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
            NameExplorer Explorer = ContextExplorer.NameExplorer;

            NameCategory NameCategory = NameCategory.Neutral;

            switch (Node.Parent)
            {
                case VariableDeclarationSyntax AsVariableDeclaration:
                    switch (AsVariableDeclaration.Parent)
                    {
                        case LocalDeclarationStatementSyntax _:
                            NameCategory = NameCategory.LocalVariable;
                            break;
                    }
                    break;
            }

            if (NameCategory == NameCategory.Neutral)
            {
                Analyzer.Trace("Not a known declaration, exit", TraceLevel);
                return;
            }

            if (!Explorer.IsNameMismatch(ValueText, NameCategory, out NamingSchemes ExpectedSheme, TraceLevel))
                return;

            Analyzer.Trace($"Name {ValueText} is not consistent with the expected {ExpectedSheme} scheme", TraceLevel);
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
