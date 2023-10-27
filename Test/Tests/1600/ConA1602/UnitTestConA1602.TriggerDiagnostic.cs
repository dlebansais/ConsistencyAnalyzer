namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1602
{
    private const string FirstEnumDocumentedOnly = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        /// <summary>
        /// Test1 doc
        /// </summary>
        Test1,
        Test2
    }
}";

    private const string FirstEnumDocumentedOnlyFixed = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        /// <summary>
        /// Test1 doc
        /// </summary>
        Test1,

        /// <summary>
        /// TODO: Insert documentation here.
        /// </summary>
        Test2
    }
}";

    private const string SecondEnumDocumentedOnly = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        Test1,

        /// <summary>
        /// Test2 doc
        /// </summary>
        Test2
    }
}";

    private const string SecondEnumDocumentedOnlyFixed = @"
using System;

namespace ConsistencyAnalyzerTest
{
    enum Test
    {
        /// <summary>
        /// TODO: Insert documentation here.
        /// </summary>
        Test1,

        /// <summary>
        /// Test2 doc
        /// </summary>
        Test2
    }
}";

    [DataTestMethod]
    [
    DataRow(FirstEnumDocumentedOnly, FirstEnumDocumentedOnlyFixed, 12, 9, "Test2"),
    DataRow(SecondEnumDocumentedOnly, SecondEnumDocumentedOnlyFixed, 8, 9, "Test1")
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixTest, int line, int column, string variableName)
    {
        UnifyCarriageReturn(ref test);
        UnifyCarriageReturn(ref fixTest);

        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1602MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, variableName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1602)),
            "title",
            FormatedMessage,
            "description",
            DiagnosticSeverity.Warning,
            true
            );

        var expected = new DiagnosticResult(descriptor);
        expected = expected.WithLocation("/0/Test0.cs", line, column);

        Task result = VerifyCS.VerifyCodeFixAsync(test, expected, fixTest);
        result.Wait();
    }
}
