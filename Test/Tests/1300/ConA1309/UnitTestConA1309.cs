namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    [TestClass]
    public partial class UnitTestConA1309 : UnitTestBase
    {
        [DataTestMethod]
        public void NoDiagnosticOnEmptyCode()
        {
            Task result = VerifyCS.VerifyAnalyzerAsync("");
            result.Wait();
        }
    }
}
