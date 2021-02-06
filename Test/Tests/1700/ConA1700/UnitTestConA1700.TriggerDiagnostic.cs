namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1700
    {
        private const string FirstClassWithRegionOnly = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test1
    {
#region Init
#endregion
    }

    public class Test2
    {
        private int x;
#region Init
#endregion
    }
}";

        private const string SecondClassWithRegionOnly = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test1
    {
        private int x;
#region Init
#endregion
    }

    public class Test2
    {
#region Init
#endregion
    }
}";

        [DataTestMethod]
        [
        DataRow(FirstClassWithRegionOnly, 12, 5, "Test2"),
        DataRow(SecondClassWithRegionOnly, 6, 5, "Test1"),
        ]
        public void WhenTestCodeInvalidDiagnosticIsRaised(string test, int line, int column, string variableName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1700MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, variableName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1700)),
                "title",
                FormatedMessage,
                "description",
                DiagnosticSeverity.Warning,
                true
                );

            var expected = new DiagnosticResult(descriptor);
            expected = expected.WithLocation("/0/Test0.cs", line, column);

            Task result = VerifyCS.VerifyAnalyzerAsync(test, expected);
            result.Wait();
        }
    }
}
