namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1200
{
    private const string ThreeUsingMixedOutside = @"
using System;
using System.Collections.Generic;

namespace ConsistencyAnalyzer
{
    using System.Collections.Immutable;
}
";

    private const string ThreeUsingMixedOutsideFixed = @"
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ConsistencyAnalyzer
{
}
";

    [DataTestMethod]
    [
    DataRow(ThreeUsingMixedOutside, ThreeUsingMixedOutsideFixed, 7, 5, nameof(Resources.ConA1200MessageFormat)),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string resourceName)
    {
        string AnalyzerMessage = new LocalizableResourceString(resourceName, Resources.ResourceManager, typeof(Resources)).ToString();

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1200)),
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
