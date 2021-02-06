namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1704
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

        private const string OneClassOneConstructorOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
        public Test() {}
#endregion
    }
}";

        private const string OneClassTwoConstructorOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
        public Test() {}
        public Test(int n) {}
#endregion
    }
}";

        private const string OneClassNoMemberFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
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

        private const string OneClassOneConstructorFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public Test() {}
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

        private const string OneClassOneConstructorOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
        public Test() {}
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

        private const string OneClassTwoConstructorOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
        public Test() {}
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
        DataRow(OneClassNoMember),
        DataRow(OneClassOneConstructor),
        DataRow(OneClassOneConstructorOneRegion),
        DataRow(OneClassTwoConstructorOneRegion),
        DataRow(OneClassNoMemberFull),
        DataRow(OneClassOneConstructorFull),
        DataRow(OneClassOneConstructorOneRegionFull),
        DataRow(OneClassTwoConstructorOneRegionFull),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
