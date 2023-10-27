namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA0001
{
    private const string DeclarationIsInvalid = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = ""abc"";
        }
    }
}";

    [DataTestMethod]
    [DataRow(DeclarationIsInvalid, 10, 21)]
    public void WhenTestCodeIsValidOtherDiagnosticIsTriggered(
                string test,
                int line,
                int column)
    {
        var descriptor = new DiagnosticDescriptor(
            "CS0029",
            "title",
            "Cannot implicitly convert type 'string' to 'int'",
            "description",
            DiagnosticSeverity.Error,
            true
            );

        var expected = new DiagnosticResult(descriptor);
        expected = expected.WithLocation("/0/Test0.cs", line, column);

        Task result = VerifyCS.VerifyAnalyzerAsync(test, expected);
        result.Wait();
    }
}
