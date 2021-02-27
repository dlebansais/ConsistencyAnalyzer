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

            SyntaxNode Node = context.Node;

            ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
            NameExplorer Explorer = ContextExplorer.NameExplorer;

            if (!Explorer.IsIndentationKnown)
            {
                Analyzer.Trace($"Unknown indentation, exit", TraceLevel);
                return;
            }

            AnalyzeNode(context, Node, Explorer, out bool IsValid, TraceLevel);

            if (IsValid && Node is DoStatementSyntax AsDoStatement)
                AnalyzeWhileKeyword(context, AsDoStatement, Explorer, TraceLevel);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context, SyntaxNode node, NameExplorer nameExplorer, out bool isValid, TraceLevel traceLevel)
        {
            isValid = true;

            if (!IsOnSeparateLine(node))
            {
                Analyzer.Trace($"Not on its own line, exit", traceLevel);
                return;
            }

            int ExpectedIndentationLevel = GetExpectedIndentationLevel(node);

            int ActualIndentationLevel;

            if (node.HasLeadingTrivia)
            {
                if (!nameExplorer.GetIndentationLevel(node.GetLeadingTrivia(), out ActualIndentationLevel, traceLevel))
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
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation()));

            isValid = false;
        }

        private void AnalyzeWhileKeyword(SyntaxNodeAnalysisContext context, DoStatementSyntax doStatement, NameExplorer nameExplorer, TraceLevel traceLevel)
        {
            SyntaxToken WhileKeyword = doStatement.WhileKeyword;

            if (!IsOnSeparateLine(WhileKeyword))
            {
                Analyzer.Trace($"Not on its own line, exit", traceLevel);
                return;
            }

            int ExpectedIndentationLevel = GetExpectedIndentationLevel(doStatement);

            int ActualIndentationLevel;

            if (WhileKeyword.HasLeadingTrivia)
            {
                if (!nameExplorer.GetIndentationLevel(WhileKeyword.LeadingTrivia, out ActualIndentationLevel, traceLevel))
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
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, WhileKeyword.GetLocation()));
        }

        /// <summary>
        /// Gets the expected indetation level of a node.
        /// </summary>
        /// <param name="node">The node.</param>
        public static int GetExpectedIndentationLevel(SyntaxNode node)
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
                case MethodDeclarationSyntax _:
                case PropertyDeclarationSyntax _:
                    ExpectedIndentationLevel = 2;
                    break;

                case EnumMemberDeclarationSyntax _:
                    ExpectedIndentationLevel = 2;
                    break;

                case StatementSyntax AsStatement:
                    ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(AsStatement);
                    break;

                case ElseClauseSyntax AsElseClause:
                    IfStatementSyntax IfStatement = (IfStatementSyntax)AsElseClause.Parent!;
                    ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(IfStatement);
                    break;

                case SwitchSectionSyntax AsSwitchSection:
                    SwitchStatementSyntax SwitchStatement = (SwitchStatementSyntax)AsSwitchSection.Parent!;
                    ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(SwitchStatement) + 1;
                    break;

                case CatchClauseSyntax AsCatchClause:
                    TryStatementSyntax TryStatement = (TryStatementSyntax)AsCatchClause.Parent!;
                    ExpectedIndentationLevel = GetExpectedIndentationLevelStatement(TryStatement);
                    break;

                default:
                    throw new InvalidCastException();
            }

            return ExpectedIndentationLevel;
        }

        private static int GetExpectedIndentationLevelStatement(StatementSyntax node)
        {
            switch (node)
            {
                case BreakStatementSyntax _:
                case CheckedStatementSyntax _:
                case ForEachStatementSyntax _:
                case ForEachVariableStatementSyntax _:
                case ContinueStatementSyntax _:
                case DoStatementSyntax _:
                case FixedStatementSyntax _:
                case ForStatementSyntax _:
                case IfStatementSyntax _:
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
                    case IfStatementSyntax _:
                        CurrentStatement = CurrentStatement.Parent as StatementSyntax;
                        NestedLevel++;
                        break;

                    case ElseClauseSyntax AsElseClause:
                        CurrentStatement = (IfStatementSyntax?)AsElseClause.Parent;
                        NestedLevel++;
                        break;

                    case CatchClauseSyntax AsCatchClause:
                        CurrentStatement = (TryStatementSyntax?)AsCatchClause.Parent;
                        NestedLevel++;
                        break;

                    case SwitchSectionSyntax AsSwitchSection:
                        CurrentStatement = (SwitchStatementSyntax?)AsSwitchSection.Parent;
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
        #endregion
    }
}
