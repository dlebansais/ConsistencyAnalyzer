namespace ConsistencyAnalyzer
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

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

        public static IReadOnlyDictionary<string, AnalyzerRule> RuleTable { get; }
        #endregion

        #region Properties
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
        public abstract string Id { get; }
        public abstract LocalizableString Title { get; }
        public abstract LocalizableString MessageFormat { get; }
        public abstract LocalizableString Description { get; }
        public abstract string Category { get; }
        public abstract SyntaxKind SyntaxKind { get; }
        public abstract void AnalyzeNode(SyntaxNodeAnalysisContext context);
        #endregion
    }
}
