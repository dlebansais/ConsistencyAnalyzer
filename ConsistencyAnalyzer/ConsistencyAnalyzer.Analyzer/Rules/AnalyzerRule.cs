namespace ConsistencyAnalyzer
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Represents a rule of the analyzer.
    /// </summary>
    public abstract class AnalyzerRule
    {
        #region Init
        static AnalyzerRule()
        {
            List<AnalyzerRule> RuleList = new List<AnalyzerRule>()
            {
                new AnalyzerRuleConA0001(),
            };

            Dictionary<string, AnalyzerRule> Table = new Dictionary<string, AnalyzerRule>();
            foreach (AnalyzerRule Rule in RuleList)
                Table.Add(Rule.Id, Rule);

            RuleTable = Table;
        }

        /// <summary>
        /// Gets the table of supported rules, by their id.
        /// </summary>
        public static IReadOnlyDictionary<string, AnalyzerRule> RuleTable { get; }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public abstract SyntaxKind SyntaxKind { get; }

        /// <summary>
        /// Gets the descriptor of a rule.
        /// </summary>
        public virtual DiagnosticDescriptor Descriptor
        {
            get
            {
                if (DescriptorInternal == null)
                    DescriptorInternal = new DiagnosticDescriptor(Id, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

                return DescriptorInternal;
            }
        }
        private DiagnosticDescriptor? DescriptorInternal;
        #endregion

        #region Descendants Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected abstract LocalizableString Title { get; }

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected abstract LocalizableString MessageFormat { get; }

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected abstract LocalizableString Description { get; }

        /// <summary>
        /// Gets the rule category.
        /// </summary>
        protected abstract string Category { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Analyzes a source code node.
        /// </summary>
        /// <param name="context">The source code.</param>
        public abstract void AnalyzeNode(SyntaxNodeAnalysisContext context);
        #endregion
    }
}
