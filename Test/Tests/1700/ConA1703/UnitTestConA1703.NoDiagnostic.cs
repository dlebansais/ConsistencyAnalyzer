namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1703
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
        protected Test() {}
    }
}";

        private const string OneClassOneConstructorOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
        protected Test() {}
#endregion
    }
}";

        [DataTestMethod]
        [
        DataRow(OneClassNoMember),
        DataRow(OneClassOneConstructor),
        DataRow(OneClassOneConstructorOneRegion)
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
