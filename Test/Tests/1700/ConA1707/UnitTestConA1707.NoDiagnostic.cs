namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1707
    {
        private const string OneClassNoMember = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
    }
}";

        private const string OneClassOneProperty = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public int Test1 { get; set; }
    }
}";

        private const string OneClassOnePropertyOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Properties
        public int Test1 { get; set; }
#endregion
    }
}";

        private const string OneClassTwoPropertyOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Properties
        public int Test1 { get; set; }
        public int Test2 { get; set; }
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

        private const string OneClassOnePropertyFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public int Test1 { get; set; }
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

        private const string OneClassOnePropertyOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Properties
        public int Test1 { get; set; }
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

        private const string OneClassTwoPropertyOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Properties
        public int Test1 { get; set; }
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
        DataRow(OneClassNoMember),
        DataRow(OneClassOneProperty),
        DataRow(OneClassOnePropertyOneRegion),
        DataRow(OneClassTwoPropertyOneRegion),
        DataRow(OneClassNoMemberFull),
        DataRow(OneClassOnePropertyFull),
        DataRow(OneClassOnePropertyOneRegionFull),
        DataRow(OneClassTwoPropertyOneRegionFull),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
