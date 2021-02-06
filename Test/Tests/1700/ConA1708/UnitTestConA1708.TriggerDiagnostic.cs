namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1708
    {
        private const string StartHigherLevel = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Math
    {
        #region Test
        public double abs(double value)
        {
            if (value >= 0)
            {
        #endregion
                return value;
            }
            else
            {
                return -1 * value;
            }
        }
    }
}";

        private const string EndHigherLevel = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Math
    {
        public double abs(double value)
        {
            if (value >= 0)
            {
        #region Test
                return value;
            }
            else
            {
                return -1 * value;
            }
        }
        #endregion
    }
}";

        [DataTestMethod]
        [
        DataRow(StartHigherLevel, 8, 9, "Test"),
        DataRow(EndHigherLevel, 12, 9, "Test"),
        ]
        public void WhenTestCodeInvalidDiagnosticIsRaised(string test, int line, int column, string regionName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1708MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, regionName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1708)),
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
