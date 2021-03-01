namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1602
    {
        private const string OneEnumNotDocumented = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        Test1
    }
}";

        private const string OneEnumDocumented = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        /// <summary>
        /// Test1 doc
        /// </summary>
        Test1
    }
}";

        private const string TwoEnumsNotDocumented = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        Test1,
        Test2
    }
}";

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

        private const string ThreeEnumsNotDocumented = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        Test1,
        Test2,
        Test3
    }
}";

        private const string ThreeEnumsDocumented = @"
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
        Test2,

        /// <summary>
        /// Test3 doc
        /// </summary>
        Test3
    }
}";

        [DataTestMethod]
        [
        DataRow(OneEnumNotDocumented),
        DataRow(OneEnumDocumented),
        DataRow(TwoEnumsNotDocumented),
        DataRow(TwoEnumsDocumented),
        DataRow(ThreeEnumsNotDocumented),
        DataRow(ThreeEnumsDocumented),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
