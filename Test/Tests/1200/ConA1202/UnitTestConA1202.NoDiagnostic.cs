namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1202
{
    private const string NoClass = @"
namespace ConsistencyAnalyzer
{
}
";

    private const string OneClass = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
    }
}
";

    private const string TwoClasses = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
    }

    public class Test2
    {
    }
}
";

    private const string TwoClassesOrdered = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
        public object CodeFixes { get; set; }
        public object AnalyzerTest;
    }

    public class Test2
    {
        public object CodeFixes { get; set; }
        public object AnalyzerTest;
    }
}
";

    private const string TwoClassesUnordered = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
        public object CodeFixes { get; set; }
        public object AnalyzerTest;
    }

    public class Test2
    {
        public object AnalyzerTest;
        public object CodeFixes { get; set; }
    }
}
";

    private const string ThreeClasses = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
    }

    public class Test2
    {
    }

    public class Test3
    {
    }
}
";

    private const string ThreeClassesOrdered = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
        public object CodeFixes { get; set; }
        public object AnalyzerTest;
    }

    public class Test2
    {
        public object CodeFixes { get; set; }
        public object AnalyzerTest;
    }

    public class Test3
    {
        public object CodeFixes { get; set; }
        public object AnalyzerTest;
    }
}
";

    [DataTestMethod]
    [
    DataRow(NoClass),
    DataRow(OneClass),
    DataRow(TwoClasses),
    DataRow(TwoClassesOrdered),
    DataRow(TwoClassesUnordered),
    DataRow(ThreeClasses),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
