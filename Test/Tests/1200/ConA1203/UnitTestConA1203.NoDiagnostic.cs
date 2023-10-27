namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1203
{
    private const string NoUsing = @"
namespace ConsistencyAnalyzer
{
}
";

    private const string OneUsingOutsideSystem = @"
using System;

namespace ConsistencyAnalyzer
{
}
";

    private const string OneUsingInsideSystem = @"
namespace ConsistencyAnalyzer
{
    using System;
}
";

    private const string OneUsingOutsideNotSystem = @"
using Microsoft;

namespace ConsistencyAnalyzer
{
}
";

    private const string OneUsingInsideNotSystem = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
}
";

    private const string TwoUsingOutside = @"
using System;
using Microsoft;

namespace ConsistencyAnalyzer
{
}
";

    private const string TwoUsingInside = @"
namespace ConsistencyAnalyzer
{
    using System;
    using Microsoft;
}
";

    private const string TwoUsingMixed = @"
using System;

namespace ConsistencyAnalyzer
{
    using System.Collections.Immutable;
}
";

    private const string ThreeUsingOutside = @"
using System;
using System.Collections.Generic;
using Microsoft;

namespace ConsistencyAnalyzer
{
}
";

    private const string ThreeUsingInside = @"
namespace ConsistencyAnalyzer
{
    using System;
    using System.Collections.Generic;
    using Microsoft;
}
";

    [DataTestMethod]
    [
    DataRow(NoUsing),
    DataRow(OneUsingOutsideSystem),
    DataRow(OneUsingInsideSystem),
    DataRow(OneUsingOutsideNotSystem),
    DataRow(OneUsingInsideNotSystem),
    DataRow(TwoUsingOutside),
    DataRow(TwoUsingInside),
    DataRow(TwoUsingMixed),
    DataRow(ThreeUsingOutside),
    DataRow(ThreeUsingInside),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
