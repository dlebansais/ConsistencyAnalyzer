namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1203
    {
        private const string TwoInside = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
    using System.Collections.Generic;
    using System.Collections.Immutable;
}
";

        private const string TwoInsideFixed = @"
namespace ConsistencyAnalyzer
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft;
}
";

        private const string TwoOutside = @"
using Microsoft;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ConsistencyAnalyzer
{
}
";

        private const string TwoOutsideFixed = @"
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft;

namespace ConsistencyAnalyzer
{
}
";

        [DataTestMethod]
        [
        DataRow(TwoInside, TwoInsideFixed, 4, 5, nameof(Resources.ConA1203MessageFormat)),
        DataRow(TwoOutside, TwoOutsideFixed, 2, 1, nameof(Resources.ConA1203MessageFormat)),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string resourceName)
        {
            string AnalyzerMessage = new LocalizableResourceString(resourceName, Resources.ResourceManager, typeof(Resources)).ToString();

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1203)),
                "title",
                AnalyzerMessage,
                "description",
                DiagnosticSeverity.Warning,
                true
                );

            var expected = new DiagnosticResult(descriptor);
            expected = expected.WithLocation("/0/Test0.cs", line, column);

            Task result = VerifyCS.VerifyCodeFixAsync(test, expected, fixedsource);
            result.Wait();
        }
    }
}
