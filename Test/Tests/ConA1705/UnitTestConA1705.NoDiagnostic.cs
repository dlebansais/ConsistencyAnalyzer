namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1705
    {
        private const string OneClassNoMember = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
    }
}";

        private const string OneClassOneField = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public int Test1;
    }
}";

        private const string OneClassOneFieldOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Fields
        public int Test1;
#endregion
    }
}";

        private const string OneClassTwoFieldOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Fields
        public int Test1;
        public int Test2;
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

        private const string OneClassOneFieldFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public int Test1;
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

        private const string OneClassOneFieldOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Fields
        public int Test1;
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

        private const string OneClassTwoFieldOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Fields
        public int Test1;
        public int Test2;
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
        DataRow(OneClassOneField),
        DataRow(OneClassOneFieldOneRegion),
        DataRow(OneClassTwoFieldOneRegion),
        DataRow(OneClassNoMemberFull),
        DataRow(OneClassOneFieldFull),
        DataRow(OneClassOneFieldOneRegionFull),
        DataRow(OneClassTwoFieldOneRegionFull),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
