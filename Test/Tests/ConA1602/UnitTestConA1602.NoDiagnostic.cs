namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1602
    {
        private const string TwoEnumsDocumented = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        /// <summary>
        /// Test1 doc
        /// </summary>
        Test1,

        /// <summary>
        /// Test2 doc
        /// </summary>
        Test2
    }
}";

        [DataTestMethod]
        [
        //DataRow(""),
        DataRow(TwoEnumsDocumented)
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
