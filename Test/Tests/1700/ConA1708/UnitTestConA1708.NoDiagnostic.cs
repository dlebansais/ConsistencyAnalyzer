namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1708
    {
        private const string NoRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
    }
}";

        private const string EmptyRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
#endregion
    }
}";

        private const string ClassDeclarationRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
#region Init
    public class Test
    {
    }
#endregion
}";

        private const string MethodDeclarationRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
        public Test()
        {
        }
#endregion
    }
}";

        private const string StatementRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public Test(int n)
        {
#region Init
             if (n > 0)
                return;

            N = n;
#endregion
        }

        public int N { get; set; }
    }
}";

        private const string StatementWithBlockRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public Test(int n)
        {
#region Init
             if (n > 0)
             {
                return;
             }
#endregion
        }
    }
}";

        [DataTestMethod]
        [
        DataRow(NoRegion),
        DataRow(EmptyRegion),
        DataRow(ClassDeclarationRegion),
        DataRow(MethodDeclarationRegion),
        DataRow(StatementRegion),
        DataRow(StatementWithBlockRegion),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
