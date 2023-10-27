namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1706
{
    private const string OneClassNoMember = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
    }
}";

    private const string OneClassOneMethod = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public void Test1() {}
    }
}";

    private const string OneClassOneMethodOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Methods
        public void Test1() {}
#endregion
    }
}";

    private const string OneClassTwoMethodOneRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Methods
        public void Test1() {}
        public void Test2() {}
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

    private const string OneClassOneMethodFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
        public void Test1() {}
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

    private const string OneClassOneMethodOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Methods
        public void Test1() {}
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

    private const string OneClassTwoMethodOneRegionFull = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Methods
        public void Test1() {}
        public void Test2() {}
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
    DataRow(OneClassOneMethod),
    DataRow(OneClassOneMethodOneRegion),
    DataRow(OneClassTwoMethodOneRegion),
    DataRow(OneClassNoMemberFull),
    DataRow(OneClassOneMethodFull),
    DataRow(OneClassOneMethodOneRegionFull),
    DataRow(OneClassTwoMethodOneRegionFull),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
