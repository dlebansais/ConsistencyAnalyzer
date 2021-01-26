namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1702
    {
        private const string OneClassNoMember = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
    }
}";

        private const string OneClassOneConstructor = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public Test() {}
    }
}";

        [DataTestMethod]
        [
        DataRow(OneClassNoMember),
        DataRow(OneClassOneConstructor)
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
