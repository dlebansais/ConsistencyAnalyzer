namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VerifyCS = ConsistencyAnalyzer.Test.CSharpCodeFixVerifier<ConsistencyAnalyzer.Analyzer, ConsistencyAnalyzer.Provider>;

    [TestClass]
    public class UnitTests
    {
        private static TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set
            {
                testContextInstance = value;
                Analyzer.TestTrace = TestTrace;
            }
        }

        public static void TestTrace(string msg)
        {
            testContextInstance.WriteLine(msg);
        }

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
         DataRow(ReferenceTypeIsntString)]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }

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

        [DataTestMethod]
        [DataRow(LocalIntCouldBeConstant, LocalIntCouldBeConstantFixed, 10, 13),
         DataRow(ConstantIsString, ConstantIsStringFixed, 10, 13),
         DataRow(DeclarationUsesVar, DeclarationUsesVarFixedHasType, 10, 13),
         DataRow(StringDeclarationUsesVar, StringDeclarationUsesVarFixedHasType, 10, 13)]
        public void WhenDiagosticIsRaisedFixUpdatesCode(
                    string test,
            string fixTest,
            int line,
            int column)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
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
            Console.WriteLine($""{i}, {j}"");
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
    }
}
