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
public class AnalyzerRuleConA1602 : SingleSyntaxAnalyzerRule
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
        TraceLevel TraceLevel = TraceLevel.Info;
        Analyzer.Trace("AnalyzerRuleConA1602", TraceLevel);

        try
        {
            EnumMemberDeclarationSyntax Node = (EnumMemberDeclarationSyntax)context.Node;

            EnumDeclarationSyntax? Parent = Node.Parent as EnumDeclarationSyntax;
            if (Parent == null)
                return;

            bool IsDocumented = XmlCommentHelper.HasDocumentation(Node);
            List<EnumMemberDeclarationSyntax> OtherEnumList = new List<EnumMemberDeclarationSyntax>(Parent.Members);

            Analyzer.Trace($"{Node.Identifier}: {IsDocumented}", TraceLevel);

            bool IsDocumentedDifferently = false;
            foreach (EnumMemberDeclarationSyntax OtherEnum in OtherEnumList)
            {
                if (OtherEnum == Node)
                    continue;

                bool IsOtherDocumented = XmlCommentHelper.HasDocumentation(OtherEnum);

                Analyzer.Trace($"{Node.Identifier}: {IsDocumented}, {OtherEnum.Identifier}: {IsOtherDocumented}", TraceLevel);

                if (IsOtherDocumented != IsDocumented)
                {
                    IsDocumentedDifferently = true;
                    break;
                }
            }

            // Report for undocumented enums, so they can be fixed by adding doc.
            if (IsDocumentedDifferently && !IsDocumented)
            {
                string EnumName = Node.Identifier.ValueText;
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), EnumName));
            }
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
