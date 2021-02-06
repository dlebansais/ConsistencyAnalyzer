namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1704
    {
        private const string OneClassTwoRegionsConstructor = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init1
        public Test() {}
#endregion

#region Init2
        public Test(int n) {}
#endregion
    }

    public class EnableInterfaceCategoryFull1
    {
#region Init
        public EnableInterfaceCategoryFull1() {}
#endregion
    }

    public class EnableInterfaceCategoryFull2
    {
#region Init
        public EnableInterfaceCategoryFull2() {}
#endregion
    }
}";

        [DataTestMethod]
        [
        DataRow(OneClassTwoRegionsConstructor, 13, 9, "Test", "Init1"),
        ]
        public void WhenTestCodeInvalidDiagnosticIsRaised(string test, int line, int column, string memberName, string regionName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1704MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, memberName, regionName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1704)),
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
