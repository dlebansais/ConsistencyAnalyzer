namespace ConsistencyAnalyzer.Test
{
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

        [DataTestMethod]
        [DataRow(FirstEnumDocumentedOnly, FirstEnumDocumentedOnlyFixed, 12, 9)]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixTest, int line, int column)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1602MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, "Test2");

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
}
