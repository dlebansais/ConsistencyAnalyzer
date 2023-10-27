namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA0001
{
    private const string LocalIntCouldBeConstant = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            Console.WriteLine(n);
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";

    private const string LocalIntCouldBeConstantFixed = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const int n = 0;
            Console.WriteLine(n);
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";

    private const string ConstantIsString = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = ""abc"";
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";

    private const string ConstantIsStringFixed = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string s = ""abc"";
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";

    private const string DeclarationUsesVar = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = 4;
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";

    private const string DeclarationUsesVarFixedHasType = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const int v = 4;
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";

    private const string StringDeclarationUsesVar = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = ""abc"";
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";
    private const string StringDeclarationUsesVarFixedHasType = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string v = ""abc"";
        }

        static void Test()
        {
            const int i = 0;
            const int j = 0;
        }
    }
}";

    [DataTestMethod]
    [
    DataRow(LocalIntCouldBeConstant, LocalIntCouldBeConstantFixed, 10, 13, "n"),
    DataRow(ConstantIsString, ConstantIsStringFixed, 10, 13, "s"),
    DataRow(DeclarationUsesVar, DeclarationUsesVarFixedHasType, 10, 13, "v"),
    DataRow(StringDeclarationUsesVar, StringDeclarationUsesVarFixedHasType, 10, 13, "v")
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixTest, int line, int column, string variableName)
    {
        UnifyCarriageReturn(ref test);
        UnifyCarriageReturn(ref fixTest);

        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1000MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, variableName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA0001)),
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
