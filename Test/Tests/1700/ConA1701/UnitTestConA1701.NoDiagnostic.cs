namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1701
{
    private const string OneClassNoRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
    }
}";

    private const string OneClassFullRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init
#endregion
    }
}";

    private const string TwoClassesNoRegion = @"
using System;

namespace ConsistencyAnalyzerTest1
{
    public class Test1
    {
    }

    public class Test2
    {
    }
}";

    private const string TwoClassesFullRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test1
    {
#region Init
#endregion
    }

    public class Test2
    {
#region Init
#endregion
    }
}";

    private const string ThreeClassesManyRegions = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test1
    {
#region Init
#endregion

    }

    public class Test2
    {
#region Init
#endregion

#region Init
#endregion
    }

    public class Test3
    {

#region Init
#endregion

#region Init
#endregion
    }
}";

    [DataTestMethod]
    [
    DataRow(OneClassNoRegion),
    DataRow(OneClassFullRegion),
    DataRow(TwoClassesNoRegion),
    DataRow(TwoClassesFullRegion)
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
