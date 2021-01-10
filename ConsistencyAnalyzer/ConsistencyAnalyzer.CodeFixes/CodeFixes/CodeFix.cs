namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public abstract class CodeFix
    {
        public CodeFix(AnalyzerRule rule)
        {
            Rule = rule;
        }

        public AnalyzerRule Rule { get; }

        public abstract Task<Document> AsyncHandler(Document document, LocalDeclarationStatementSyntax localDeclaration, CancellationToken cancellationToken);

        static CodeFix()
        {
            List<CodeFix> CodeFixList = new List<CodeFix>()
            {
                new CodeFixConA0001(AnalyzerRule.RuleTable[nameof(AnalyzerRuleConA0001)]),
            };

            Dictionary<string, CodeFix> Table = new Dictionary<string, CodeFix>();
            foreach (CodeFix CodeFix in CodeFixList)
                Table.Add(CodeFix.Rule.Id, CodeFix);

            CodeFixTable = Table;
        }

        public static IReadOnlyDictionary<string, CodeFix> CodeFixTable { get; }
    }
}
