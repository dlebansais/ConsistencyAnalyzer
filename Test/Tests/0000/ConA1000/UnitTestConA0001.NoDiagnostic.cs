namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA0001
    {
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
    }
}
