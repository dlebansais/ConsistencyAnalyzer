namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1201
{
    private const string NoUsing = @"
namespace ConsistencyAnalyzer
{
}
";

    private const string OneUsingOutside = @"
using System;

namespace ConsistencyAnalyzer
{
}
";

    private const string OneUsingInside = @"
namespace ConsistencyAnalyzer
{
    using System;
}
";

    private const string TwoUsingOutside = @"
using System;
using System.Collections.Immutable;

namespace ConsistencyAnalyzer
{
}
";

    private const string TwoUsingInside = @"
namespace ConsistencyAnalyzer
{
    using System;
    using System.Collections.Immutable;
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
using System.Collections.Immutable;

namespace ConsistencyAnalyzer
{
}
";

    private const string ThreeUsingInside = @"
namespace ConsistencyAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
}
";

    [DataTestMethod]
    [
    DataRow(NoUsing),
    DataRow(OneUsingOutside),
    DataRow(OneUsingInside),
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
