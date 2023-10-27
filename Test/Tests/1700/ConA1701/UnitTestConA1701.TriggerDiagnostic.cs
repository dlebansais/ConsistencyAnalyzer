namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1701
{
    private const string ClassWithNestedRegion = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test1
    {
#region Test1
#region Test2
#endregion
#endregion
    }
}";

    private const string ClassWithNestedRegionFixed = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test1
    {
#region Test1
#endregion
    }
}";

    [DataTestMethod]
    [
    DataRow(ClassWithNestedRegion, ClassWithNestedRegionFixed, 9, 1, "Test2", "Test1")
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string nestedRegionName, string ownerRegionName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1701MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, nestedRegionName, ownerRegionName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1701)),
            "title",
            FormatedMessage,
            "description",
            DiagnosticSeverity.Warning,
            true
            );

        var expected = new DiagnosticResult(descriptor);
        expected = expected.WithLocation("/0/Test0.cs", line, column);

        Task result = VerifyCS.VerifyCodeFixAsync(test, expected, fixedsource);
        result.Wait();
    }
}
