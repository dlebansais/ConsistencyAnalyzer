namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1702
    {

        private const string OneClassTwoRegions = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init1
        public void Test1() {}
#endregion

#region Init2
        public void Test2() {}
#endregion
    }
}";

        [DataTestMethod]
        [
        DataRow(OneClassTwoRegions, 13, 9, "Test2", "Init1")
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, int line, int column, string memberName, string regionName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1702MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, memberName, regionName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1702)),
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
