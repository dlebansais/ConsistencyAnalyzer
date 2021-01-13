namespace ConsistencyAnalyzer.Test
{
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
            int i = 0;
            Console.WriteLine(i);
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
            const int i = 0;
            Console.WriteLine(i);
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
            string i = ""abc"";
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
            const string i = ""abc"";
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
            var i = 4;
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
            const int i = 4;
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
            var i = ""abc"";
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
            const string i = ""abc"";
        }
    }
}";

        [DataTestMethod]
        [
        DataRow(LocalIntCouldBeConstant, LocalIntCouldBeConstantFixed, 10, 13),
        DataRow(ConstantIsString, ConstantIsStringFixed, 10, 13),
        DataRow(DeclarationUsesVar, DeclarationUsesVarFixedHasType, 10, 13),
        DataRow(StringDeclarationUsesVar, StringDeclarationUsesVarFixedHasType, 10, 13)
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(
                    string test,
            string fixTest,
            int line,
            int column)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1000MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, "i");

            var descriptor = new DiagnosticDescriptor(
                nameof(AnalyzerRuleConA0001),
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
