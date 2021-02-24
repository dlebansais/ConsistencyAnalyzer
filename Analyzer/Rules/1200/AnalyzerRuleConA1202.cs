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
    public class AnalyzerRuleConA1202 : MultipleSyntaxAnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1202));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override List<SyntaxKind> RuleSyntaxKinds { get; } = new List<SyntaxKind>()
        {
            SyntaxKind.ConstructorDeclaration,
            SyntaxKind.FieldDeclaration,
            SyntaxKind.MethodDeclaration,
            SyntaxKind.PropertyDeclaration,
        };
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1202Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1202MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1202Description), Resources.ResourceManager, typeof(Resources));

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
            Analyzer.Trace("AnalyzerRuleConA1202", TraceLevel);

            MemberDeclarationSyntax Node = (MemberDeclarationSyntax)context.Node;
            ClassDeclarationSyntax? ParentClass = Node.Parent as ClassDeclarationSyntax;

            if (ParentClass == null)
            {
                Analyzer.Trace($"No parent class, exit", TraceLevel);
                return;
            }

            string MemberTypeString;
            string MemberName;

            switch (Node)
            {
                case ConstructorDeclarationSyntax AsConstructorDeclaration:
                    MemberTypeString = "Constructor";
                    MemberName = $"'{AsConstructorDeclaration.Identifier.ValueText}'";
                    break;

                case PropertyDeclarationSyntax AsPropertyDeclaration:
                    MemberTypeString = "Property";
                    MemberName = $"'{AsPropertyDeclaration.Identifier.ValueText}'";
                    break;

                case FieldDeclarationSyntax AsFieldDeclaration:
                    if (AsFieldDeclaration.Declaration.Variables.Count == 0)
                    {
                        Analyzer.Trace($"No field names, exit", TraceLevel);
                        return;
                    }
                    if (AsFieldDeclaration.Declaration.Variables.Count == 1)
                    {
                        MemberTypeString = "Field";
                        MemberName = $"'{AsFieldDeclaration.Declaration.Variables[0].Identifier.ValueText}'";
                    }
                    else
                    {
                        MemberTypeString = "Fields";
                        MemberName = string.Empty;

                        for (int i = 0; i < AsFieldDeclaration.Declaration.Variables.Count; i++)
                        {
                            VariableDeclaratorSyntax VariableDeclarator = AsFieldDeclaration.Declaration.Variables[i];

                            if (MemberName.Length > 0)
                                if (i + 1 < AsFieldDeclaration.Declaration.Variables.Count)
                                    MemberName += ", ";
                                else
                                    MemberName += " and ";

                            MemberName += $"'{VariableDeclarator.Identifier.ValueText}'";
                        }
                    }
                    break;

                case MethodDeclarationSyntax AsMethodDeclaration:
                    MemberTypeString = "Method";
                    MemberName = $"'{AsMethodDeclaration.Identifier.ValueText}'";
                    break;
                default:
                    Analyzer.Trace($"Unsupported element type, exit", TraceLevel);
                    return;
            }

            ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
            ClassExplorer Explorer = ContextExplorer.ClassExplorer;

            if (!Explorer.IsAllClassMemberFullyOrdered())
            {
                Analyzer.Trace($"No full order, exit", TraceLevel);
                return;
            }

            if (!Explorer.IsClassMemberOutOfOrder(ParentClass, Node))
            {
                Analyzer.Trace($"Properly ordered vs members before, exit", TraceLevel);
                return;
            }

            string ParentClassName = ParentClass.Identifier.ValueText;

            Analyzer.Trace($"Member at the wrong place", TraceLevel);
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), MemberTypeString, MemberName, ParentClassName));
        }
        #endregion
    }
}
