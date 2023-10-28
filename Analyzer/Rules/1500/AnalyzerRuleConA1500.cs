namespace ConsistencyAnalyzer;

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Represents a rule of the analyzer.
/// </summary>
public class AnalyzerRuleConA1500 : MultipleSyntaxAnalyzerRule
{
    #region Properties
    /// <summary>
    /// Gets the rule id.
    /// </summary>
    public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1500));

    /// <summary>
    /// Gets the kind of syntax this rule analyzes.
    /// </summary>
    public override List<SyntaxKind> RuleSyntaxKinds { get; } = new List<SyntaxKind>()
    {
        SyntaxKind.UsingDirective,
        SyntaxKind.NamespaceDeclaration,
        SyntaxKind.EnumDeclaration,
        SyntaxKind.ClassDeclaration,
        SyntaxKind.StructDeclaration,
        SyntaxKind.RecordDeclaration,
        SyntaxKind.InterfaceDeclaration,
        SyntaxKind.DelegateDeclaration,
        SyntaxKind.EventFieldDeclaration,
        SyntaxKind.FieldDeclaration,
        SyntaxKind.MethodDeclaration,
        SyntaxKind.PropertyDeclaration,
        SyntaxKind.EnumMemberDeclaration,
        SyntaxKind.BreakStatement,
        SyntaxKind.CheckedStatement,
        SyntaxKind.ForEachStatement,
        SyntaxKind.ContinueStatement,
        SyntaxKind.DoStatement,
        SyntaxKind.FixedStatement,
        SyntaxKind.ForStatement,
        SyntaxKind.IfStatement,
        SyntaxKind.LocalDeclarationStatement,
        SyntaxKind.LockStatement,
        SyntaxKind.ReturnStatement,
        SyntaxKind.SwitchStatement,
        SyntaxKind.ThrowStatement,
        SyntaxKind.TryStatement,
        SyntaxKind.UnsafeStatement,
        SyntaxKind.UsingStatement,
        SyntaxKind.WhileStatement,
        SyntaxKind.YieldBreakStatement,
        SyntaxKind.YieldReturnStatement,
        SyntaxKind.ElseClause,
        SyntaxKind.SwitchSection,
        SyntaxKind.CatchClause,
        SyntaxKind.Block,
        SyntaxKind.GetAccessorDeclaration,
        SyntaxKind.SetAccessorDeclaration,
    };
    #endregion

    #region Ancestor Interface
    /// <summary>
    /// Gets the rule title.
    /// </summary>
    protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1500Title), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule message format.
    /// </summary>
    protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1500MessageFormat), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule description.
    /// </summary>
    protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1500Description), Resources.ResourceManager, typeof(Resources));

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
        Analyzer.Trace("AnalyzerRuleConA1500", TraceLevel);

        try
        {
            SyntaxNode Node = context.Node;

            ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
            NameExplorer Explorer = ContextExplorer.NameExplorer;

            if (!Explorer.IsIndentationKnown)
            {
                Analyzer.Trace($"Unknown indentation, exit", TraceLevel);
                return;
            }

            bool IsInNamespace = CheckIfIsInNamespace(Node);

            bool IsValid;
            if (Node is BlockSyntax _)
                IsValid = true;
            else
                AnalyzeNode(context, Node, Explorer, IsInNamespace, out IsValid, TraceLevel);

            if (IsValid)
            {
                switch (Node)
                {
                    case NamespaceDeclarationSyntax AsNamespaceDeclaration:
                        AnalyzeNodeToken(context, AsNamespaceDeclaration, AsNamespaceDeclaration.OpenBraceToken, IsInNamespace, Explorer, TraceLevel);
                        AnalyzeNodeToken(context, AsNamespaceDeclaration, AsNamespaceDeclaration.CloseBraceToken, IsInNamespace, Explorer, TraceLevel);
                        break;

                    case BaseTypeDeclarationSyntax AsBaseTypeDeclaration:
                        AnalyzeNodeToken(context, AsBaseTypeDeclaration, AsBaseTypeDeclaration.OpenBraceToken, IsInNamespace, Explorer, TraceLevel);
                        AnalyzeNodeToken(context, AsBaseTypeDeclaration, AsBaseTypeDeclaration.CloseBraceToken, IsInNamespace, Explorer, TraceLevel);
                        break;

                    case PropertyDeclarationSyntax AsPropertyDeclaration:
                        if (AsPropertyDeclaration.AccessorList != null)
                        {
                            AccessorListSyntax AccessorList = AsPropertyDeclaration.AccessorList;
                            AnalyzeNodeToken(context, AccessorList, AccessorList.OpenBraceToken, IsInNamespace, Explorer, TraceLevel);
                            AnalyzeNodeToken(context, AccessorList, AccessorList.CloseBraceToken, IsInNamespace, Explorer, TraceLevel);
                        }
                        break;

                    case AccessorDeclarationSyntax AsAccessorDeclaration:
                        AnalyzeNodeToken(context, AsAccessorDeclaration, AsAccessorDeclaration.Keyword, IsInNamespace, Explorer, TraceLevel);
                        break;

                    case BlockSyntax AsBlock:
                        AnalyzeNodeToken(context, AsBlock, AsBlock.OpenBraceToken, IsInNamespace, Explorer, TraceLevel);
                        AnalyzeNodeToken(context, AsBlock, AsBlock.CloseBraceToken, IsInNamespace, Explorer, TraceLevel);
                        break;

                    case SwitchStatementSyntax AsSwitchStatement:
                        AnalyzeNodeToken(context, AsSwitchStatement, AsSwitchStatement.OpenBraceToken, IsInNamespace, Explorer, TraceLevel);
                        AnalyzeNodeToken(context, AsSwitchStatement, AsSwitchStatement.CloseBraceToken, IsInNamespace, Explorer, TraceLevel);
                        break;

                    case DoStatementSyntax AsDoStatement:
                        AnalyzeNodeToken(context, AsDoStatement, AsDoStatement.WhileKeyword, IsInNamespace, Explorer, TraceLevel);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Analyzer.Trace($"{e.Message}\n{e.StackTrace}", TraceLevel.Critical);

            throw e;
        }
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context, SyntaxNode node, NameExplorer nameExplorer, bool isInNamespace, out bool isValid, TraceLevel traceLevel)
    {
        isValid = true;

        if (!IsOnSeparateLine(node))
        {
            Analyzer.Trace($"Not on its own line, exit", traceLevel);
            return;
        }

        int ExpectedIndentationLevel = GetExpectedIndentationLevel(node, isInNamespace);

        int ActualIndentationLevel;

        if (node.HasLeadingTrivia)
        {
            if (!nameExplorer.GetIndentationLevel(node.GetLeadingTrivia(), out ActualIndentationLevel, traceLevel))
                ActualIndentationLevel = -1;
            else
            {
                if (!isInNamespace && ActualIndentationLevel > 0)
                    ActualIndentationLevel--;
            }
        }
        else
            ActualIndentationLevel = 0;

        if (ActualIndentationLevel == ExpectedIndentationLevel)
        {
            Analyzer.Trace($"Valid indentation level, exit", traceLevel);
            return;
        }

        Analyzer.Trace($"Inconsistent indentation", traceLevel);
        context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation()));

        isValid = false;
    }

    private void AnalyzeNodeToken(SyntaxNodeAnalysisContext context, SyntaxNode node, SyntaxToken token, bool isInNamespace, NameExplorer nameExplorer, TraceLevel traceLevel)
    {
        if (!IsOnSeparateLine(token))
        {
            Analyzer.Trace($"Not on its own line, exit", traceLevel);
            return;
        }

        int ExpectedIndentationLevel = GetExpectedIndentationLevel(node, isInNamespace);

        int ActualIndentationLevel;
        string TokenTrivia = string.Empty;

        if (token.HasLeadingTrivia)
        {
            TokenTrivia = token.LeadingTrivia.ToString();
            if (!nameExplorer.GetIndentationLevel(token.LeadingTrivia, out ActualIndentationLevel, traceLevel))
                ActualIndentationLevel = -1;
        }
        else
            ActualIndentationLevel = 0;

        if (ActualIndentationLevel == ExpectedIndentationLevel)
        {
            Analyzer.Trace($"Valid indentation level, exit", traceLevel);
            return;
        }

        Analyzer.Trace($"Inconsistent indentation", traceLevel);
        context.ReportDiagnostic(Diagnostic.Create(Descriptor, token.GetLocation()));
    }

    /// <summary>
    /// Checks if the node is declared within a namespace.
    /// </summary>
    /// <param name="node">The node.</param>
    public static bool CheckIfIsInNamespace(SyntaxNode node)
    {
        bool IsInNamespace = false;
        SyntaxNode? CurrentNode = node;

        while (CurrentNode != null)
        {
            if (CurrentNode is NamespaceDeclarationSyntax)
            {
                IsInNamespace = true;
                break;
            }

            CurrentNode = GetTrueParent(CurrentNode);
        }

        return IsInNamespace;
    }

    /// <summary>
    /// Gets the expected indetation level of a node.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="isInNamespace">True if the node is within a namespace.</param>
    public static int GetExpectedIndentationLevel(SyntaxNode node, bool isInNamespace)
    {
        int ExpectedIndentationLevel;

        switch (node)
        {
            case UsingDirectiveSyntax AsUsingDirective:
                ExpectedIndentationLevel = AsUsingDirective.Parent is NamespaceDeclarationSyntax ? 1 : 0;
                break;

            case NamespaceDeclarationSyntax _:
                ExpectedIndentationLevel = 0;
                break;

            case EnumDeclarationSyntax _:
            case ClassDeclarationSyntax _:
            case StructDeclarationSyntax _:
            case RecordDeclarationSyntax _:
            case InterfaceDeclarationSyntax _:
                ExpectedIndentationLevel = 1;
                break;

            case DelegateDeclarationSyntax _:
            case EventFieldDeclarationSyntax _:
            case FieldDeclarationSyntax _:
            case BaseMethodDeclarationSyntax _:
            case PropertyDeclarationSyntax _:
                ExpectedIndentationLevel = 2;
                break;

            case EnumMemberDeclarationSyntax _:
                ExpectedIndentationLevel = 2;
                break;

            case AccessorListSyntax _:
                ExpectedIndentationLevel = 2;
                break;

            case AccessorDeclarationSyntax _:
                ExpectedIndentationLevel = 3;
                break;

            case BlockSyntax AsBlock:
                switch (GetTrueParent(AsBlock))
                {
                    case BlockSyntax:
                        ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(AsBlock);
                        break;
                    case StatementSyntax AsStatement:
                        ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(AsStatement);
                        break;
                    case AccessorDeclarationSyntax _:
                        ExpectedIndentationLevel = 3;
                        break;
                    case BaseMethodDeclarationSyntax _:
                        ExpectedIndentationLevel = 2;
                        break;
                    default:
                        throw new InvalidCastException();
                }
                break;

            case StatementSyntax AsStatement:
                ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(AsStatement);
                break;

            case ElseClauseSyntax AsElseClause:
                IfStatementSyntax IfStatement = (IfStatementSyntax)GetTrueParent(AsElseClause)!;
                ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(IfStatement);
                break;

            case SwitchSectionSyntax AsSwitchSection:
                SwitchStatementSyntax SwitchStatement = (SwitchStatementSyntax)GetTrueParent(AsSwitchSection)!;
                ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(SwitchStatement) + 1;
                break;

            case CatchClauseSyntax AsCatchClause:
                TryStatementSyntax TryStatement = (TryStatementSyntax)GetTrueParent(AsCatchClause)!;
                ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(TryStatement);
                break;

            default:
                throw new InvalidCastException();
        }

        if (!isInNamespace && ExpectedIndentationLevel > 0)
            ExpectedIndentationLevel--;

        return ExpectedIndentationLevel;
    }

    private static int GetExpectedIndentationLevelStatement(StatementSyntax node)
    {
        switch (node)
        {
            case BlockSyntax _:
            case BreakStatementSyntax _:
            case CheckedStatementSyntax _:
            case ForEachStatementSyntax _:
            case ForEachVariableStatementSyntax _:
            case IfStatementSyntax _:
            case ContinueStatementSyntax _:
            case DoStatementSyntax _:
            case FixedStatementSyntax _:
            case ForStatementSyntax _:
            case LocalDeclarationStatementSyntax _:
            case LockStatementSyntax _:
            case ReturnStatementSyntax _:
            case SwitchStatementSyntax _:
            case ThrowStatementSyntax _:
            case TryStatementSyntax _:
            case UnsafeStatementSyntax _:
            case UsingStatementSyntax _:
            case WhileStatementSyntax _:
            case YieldStatementSyntax _:
                return 2 + GetBlockLevel(node);

            default:
                throw new InvalidCastException();
        }
    }

    private static int GetBlockLevel(StatementSyntax statement)
    {
        int NestedLevel = 0;
        StatementSyntax? CurrentStatement = statement;

        while (CurrentStatement != null)
        {
            if (CurrentStatement.Parent is MemberDeclarationSyntax)
            {
                NestedLevel++;
                break;
            }

            if (CurrentStatement.Parent is AccessorDeclarationSyntax)
            {
                NestedLevel += 2;
                break;
            }

            switch (CurrentStatement.Parent)
            {
                case BlockSyntax _:
                    CurrentStatement = CurrentStatement.Parent as StatementSyntax;
                    if (CurrentStatement?.Parent is BlockSyntax)
                        NestedLevel++;
                    break;

                case ForEachStatementSyntax _:
                case ForStatementSyntax _:
                case DoStatementSyntax _:
                case CheckedStatementSyntax _:
                case TryStatementSyntax _:
                    CurrentStatement = CurrentStatement.Parent as StatementSyntax;
                    NestedLevel++;
                    break;

                case IfStatementSyntax _:
                    CurrentStatement = GetTrueParent(CurrentStatement) as StatementSyntax;
                    NestedLevel++;
                    break;

                case ElseClauseSyntax AsElseClause:
                    CurrentStatement = (IfStatementSyntax?)GetTrueParent(AsElseClause);
                    NestedLevel++;
                    break;

                case CatchClauseSyntax AsCatchClause:
                    CurrentStatement = (TryStatementSyntax?)GetTrueParent(AsCatchClause);
                    NestedLevel++;
                    break;

                case SwitchSectionSyntax AsSwitchSection:
                    CurrentStatement = (SwitchStatementSyntax?)GetTrueParent(AsSwitchSection);
                    NestedLevel += 2;
                    break;

                default:
                    throw new InvalidCastException();
            }
        }

        return NestedLevel;
    }

    private bool IsOnSeparateLine(SyntaxToken token)
    {
        if (!token.HasLeadingTrivia)
            return false;

        SyntaxTriviaList LeadingTrivia = token.LeadingTrivia;
        if (token.SyntaxTree != null)
        {
            var triviaSpan = token.SyntaxTree.GetLineSpan(LeadingTrivia.FullSpan);

            // There is no indentation when the leading trivia doesn't begin at the start of the line.
            if ((triviaSpan.StartLinePosition == triviaSpan.EndLinePosition) && (triviaSpan.StartLinePosition.Character > 0))
                return false;
        }

        return true;
    }

    private bool IsOnSeparateLine(SyntaxNode node)
    {
        if (!node.HasLeadingTrivia)
            return false;

        SyntaxTriviaList LeadingTrivia = node.GetLeadingTrivia();
        var triviaSpan = node.SyntaxTree.GetLineSpan(LeadingTrivia.FullSpan);

        // There is no indentation when the leading trivia doesn't begin at the start of the line.
        if ((triviaSpan.StartLinePosition == triviaSpan.EndLinePosition) && (triviaSpan.StartLinePosition.Character > 0))
            return false;

        return true;
    }

    private static SyntaxNode? GetTrueParent(SyntaxNode node)
    {
        SyntaxNode CurrentNode = node;

        if (CurrentNode.Parent is CatchClauseSyntax AsCatchClauseParent)
            CurrentNode = AsCatchClauseParent;
        else
        {
            if (CurrentNode.Parent is ElseClauseSyntax AsElseClauseParent)
                CurrentNode = AsElseClauseParent;

            while (CurrentNode.Parent is IfStatementSyntax AsIfStatement && AsIfStatement.Parent is ElseClauseSyntax AsElseClause)
                CurrentNode = AsElseClause;
        }

        return CurrentNode.Parent;
    }
    #endregion
}
