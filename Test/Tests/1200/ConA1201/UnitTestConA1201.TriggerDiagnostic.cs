namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1201
{
    private const string ThreeUsingMixedInside = @"
using System;

namespace ConsistencyAnalyzer
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
}
";

    private const string ThreeUsingMixedInsideFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
}
";

    [DataTestMethod]
    [
    DataRow(ThreeUsingMixedInside, ThreeUsingMixedInsideFixed, 2, 1, nameof(Resources.ConA1201MessageFormat)),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string resourceName)
    {
        string AnalyzerMessage = new LocalizableResourceString(resourceName, Resources.ResourceManager, typeof(Resources)).ToString();

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1201)),
            "title",
            AnalyzerMessage,
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
