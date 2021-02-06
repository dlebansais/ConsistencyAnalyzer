namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1707
    {
        private const string OneClassTwoRegionsProperty = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init1
        public int Test1 { get; set; }
#endregion

#region Init2
        public int Test2 { get; set; }
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
        DataRow(OneClassTwoRegionsProperty, 13, 9, "Test2", "Init1"),
        ]
        public void WhenTestCodeInvalidDiagnosticIsRaised(string test, int line, int column, string memberName, string regionName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1707MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, memberName, regionName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1707)),
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
