using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = ConsistencyAnalyzer.Test.CSharpCodeFixVerifier<
    ConsistencyAnalyzer.ConsistencyAnalyzerAnalyzer,
    ConsistencyAnalyzer.ConsistencyAnalyzerCodeFixProvider>;

namespace ConsistencyAnalyzer.Test
{
    [TestClass]
    public class ConsistencyAnalyzerUnitTest
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

        [DataTestMethod]
        [DataRow(""),
         DataRow(VariableAssigned),
         DataRow(AlreadyConst),
         DataRow(NoInitializer),
         DataRow(InitializerNotConstant),
         DataRow(MultipleInitializers),
         DataRow(DeclarationIsInvalid),
         DataRow(ReferenceTypeIsntString)]
        public async void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }

        [DataTestMethod]
        [DataRow(LocalIntCouldBeConstant, LocalIntCouldBeConstantFixed, 10, 13),
         DataRow(ConstantIsString, ConstantIsStringFixed, 10, 13),
         DataRow(DeclarationUsesVar, DeclarationUsesVarFixedHasType, 10, 13),
         DataRow(StringDeclarationUsesVar, StringDeclarationUsesVarFixedHasType, 10, 13)]
        public async void WhenDiagosticIsRaisedFixUpdatesCode(
                    string test,
            string fixTest,
            int line,
            int column)
        {
            var descriptor = new DiagnosticDescriptor(
                ConsistencyAnalyzerAnalyzer.DiagnosticId, 
                "title", 
                new LocalizableResourceString(nameof(ConsistencyAnalyzer.Resources.AnalyzerMessageFormat), ConsistencyAnalyzer.Resources.ResourceManager, typeof(ConsistencyAnalyzer.Resources)).ToString(),
                "description",
                DiagnosticSeverity.Warning,
                true
                );

            var expected = new DiagnosticResult(descriptor);
            expected = expected.WithLocation("Test0.cs", line, column);

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        private const string VariableAssigned = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            Console.WriteLine(i++);
        }
    }
}";

        private const string AlreadyConst = @"
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

        private const string NoInitializer = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int i;
            i = 0;
            Console.WriteLine(i);
        }
    }
}";

        private const string InitializerNotConstant = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = DateTime.Now.DayOfYear;
            Console.WriteLine(i);
        }
    }
}";

        private const string MultipleInitializers = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0, j = DateTime.Now.DayOfYear;
            Console.WriteLine(i, j);
        }
    }
}";

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

        private const string ReferenceTypeIsntString = @"
using System;

namespace ConsistencyAnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            object s = ""abc"";
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
            var item = 4;
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
            const int item = 4;
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
            var item = ""abc"";
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
            const string item = ""abc"";
        }
    }
}";
    }
}
