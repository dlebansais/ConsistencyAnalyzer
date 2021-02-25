namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.CodeAnalysis.CodeFixes;
    using System.Diagnostics;

    /// <summary>
    /// Represents a code fix for source code that triggered a rule in the analyzer.
    /// </summary>
    public abstract class CodeFix
    {
        #region Init
        static CodeFix()
        {
            List<CodeFix> CodeFixList = new List<CodeFix>()
            {
                new CodeFixConA0001(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA0001))]),
                new CodeFixConA1602(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1602))]),
                new CodeFixConA1701(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1701))]),
                new CodeFixConA1300(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1300))]),
                new CodeFixConA1301(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1301))]),
                new CodeFixConA1302(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1302))]),
                new CodeFixConA1303(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1303))]),
                new CodeFixConA1304(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1304))]),
                new CodeFixConA1305(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1305))]),
                new CodeFixConA1306(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1306))]),
                new CodeFixConA1307(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1307))]),
                new CodeFixConA1308(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1308))]),
                new CodeFixConA1309(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1309))]),
                new CodeFixConA1310(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1310))]),
                new CodeFixConA1311(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1311))]),
                new CodeFixConA1312(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1312))]),
                new CodeFixConA1313(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1313))]),
                new CodeFixConA1314(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1314))]),
                new CodeFixConA1200(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1200))]),
                new CodeFixConA1201(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1201))]),
                new CodeFixConA1203(AnalyzerRule.RuleTable[AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1203))]),
            };

            Dictionary<string, CodeFix> Table = new Dictionary<string, CodeFix>();
            foreach (CodeFix CodeFix in CodeFixList)
                Table.Add(CodeFix.Rule.Id, CodeFix);

            CodeFixTable = Table;
        }

        /// <summary>
        /// Gets the table of code fixes by id.
        /// </summary>
        public static IReadOnlyDictionary<string, CodeFix> CodeFixTable { get; }

        /// <summary>
        /// Creates an instance of the <see cref="CodeFix"/> class.
        /// </summary>
        /// <param name="rule">The associated rule.</param>
        public CodeFix(AnalyzerRule rule)
        {
            Rule = rule;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the associated rule.
        /// </summary>
        public AnalyzerRule Rule { get; init; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Handles a code fix for the rule in a documents.
        /// </summary>
        /// <param name="context">The code fix context.</param>
        /// <param name="root">The source code root.</param>
        /// <param name="diagnosticSpan">The location of source code to fix.</param>
        /// <returns>A code action that will invoke the fix.</returns>
        public abstract CodeAction? CreateDocumentHandler(CodeFixContext context, SyntaxNode root, TextSpan diagnosticSpan);
        #endregion
    }
}
