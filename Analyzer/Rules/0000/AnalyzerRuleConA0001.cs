namespace ConsistencyAnalyzer
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Represents a rule of the analyzer.
    /// </summary>
    public class AnalyzerRuleConA0001 : SingleSyntaxAnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA0001));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.LocalDeclarationStatement;
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1000Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1000MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1000Description), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule category.
        /// </summary>
        protected override string Category { get; } = "Usage";
        #endregion

        #region Client Interface
        /// <summary>
        /// Analyzes the possible constness of local variables.
        /// </summary>
        /// <param name="context">A context to analyze the code.</param>
        /// <param name="node">The local variables.</param>
        public static bool AnalyzeConstness(SyntaxNodeAnalysisContext context, LocalDeclarationStatementSyntax node)
        {
            // make sure the declaration isn't already const:
            Debug.Assert(!node.Modifiers.Any(SyntaxKind.ConstKeyword));

            var variableTypeName = node.Declaration.Type;
            var variableType = context.SemanticModel.GetTypeInfo(variableTypeName).ConvertedType;
            if (variableType == null)
                return false;

            // Ensure that all variables in the local declaration have initializers that
            // are assigned with constant values.
            foreach (var variable in node.Declaration.Variables)
            {
                var initializer = variable.Initializer;
                if (initializer == null)
                    return false;

                var constantValue = context.SemanticModel.GetConstantValue(initializer.Value);
                if (!constantValue.HasValue)
                    return false;

                // Ensure that the initializer value can be converted to the type of the
                // local declaration without a user-defined conversion.
                var conversion = context.SemanticModel.ClassifyConversion(initializer.Value, variableType);
                if (!conversion.Exists || conversion.IsUserDefined)
                    return false;

                // Special cases:
                //  * If the constant value is a string, the type of the local declaration
                //    must be System.String.
                //  * If the constant value is null, the type of the local declaration must
                //    be a reference type.
                if (constantValue.Value is string)
                {
                    if (variableType.SpecialType != SpecialType.System_String)
                        return false;
                }
                else if (variableType.IsReferenceType && constantValue.Value != null)
                    return false;
            }

            // Perform data flow analysis on the local declaration.
            var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(node);
            if (dataFlowAnalysis == null)
                return false;

            foreach (var variable in node.Declaration.Variables)
            {
                // Retrieve the local symbol for each variable in the local declaration
                // and ensure that it is not written outside of the data flow analysis region.
                var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable);
                if (variableSymbol != null && dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Analyzes a source code node.
        /// </summary>
        /// <param name="context">The source code.</param>
        public override void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("AnalyzerRuleConA0001", TraceLevel);

            try
            {
                LocalDeclarationStatementSyntax Node = (LocalDeclarationStatementSyntax)context.Node;

                ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
                NameExplorer Explorer = ContextExplorer.NameExplorer;

                if (!Explorer.IsLocalVariableConstnessExpected)
                    return;

                // make sure the declaration isn't already const.
                bool IsConst = Node.Modifiers.Any(SyntaxKind.ConstKeyword);
                if (!IsConst)
                {
                    bool CanBeConst = AnalyzeConstness(context, Node);
                    if (CanBeConst)
                    {
                        foreach (var variable in Node.Declaration.Variables)
                            context.ReportDiagnostic(Diagnostic.Create(Descriptor, context.Node.GetLocation(), variable.Identifier.ValueText));
                    }
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
}
