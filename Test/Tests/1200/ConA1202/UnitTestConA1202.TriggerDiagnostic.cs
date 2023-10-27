namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1202
{
    private const string ThreeClassesUnorderedOneField = @"
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
        public object AnalyzerTest;
        public object CodeFixes { get; set; }
    }
}
";

    private const string ThreeClassesUnorderedMultipleFields = @"
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
        public object AnalyzerTest1, AnalyzerTest2, AnalyzerTest3;
        public object CodeFixes { get; set; }
    }
}
";

    private const string ThreeClassesUnorderedProperty = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
        public object AnalyzerTest;
        public object CodeFixes { get; set; }
    }

    public class Test2
    {
        public object AnalyzerTest;
        public object CodeFixes { get; set; }
    }

    public class Test3
    {
        public object CodeFixes { get; set; }
        public object AnalyzerTest;
    }
}
";

    private const string ThreeClassesUnorderedMethod = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
        public object CodeFixes { get; set; }
        public void AnalyzerTest() {}
    }

    public class Test2
    {
        public object CodeFixes { get; set; }
        public void AnalyzerTest() {}
    }

    public class Test3
    {
        public void AnalyzerTest() {}
        public object CodeFixes { get; set; }
    }
}
";

    private const string ThreeClassesUnorderedConstructor = @"
namespace ConsistencyAnalyzer
{
    public class Test1
    {
        public object CodeFixes { get; set; }
        public Test1() {}
    }

    public class Test2
    {
        public object CodeFixes { get; set; }
        public Test2() {}
    }

    public class Test3
    {
        public Test3() {}
        public object CodeFixes { get; set; }
    }
}
";

    [DataTestMethod]
    [
    DataRow(ThreeClassesUnorderedOneField, 18, 9, "Field", "'AnalyzerTest'", "Test3"),
    DataRow(ThreeClassesUnorderedMultipleFields, 18, 9, "Fields", "'AnalyzerTest1', 'AnalyzerTest2' and 'AnalyzerTest3'", "Test3"),
    DataRow(ThreeClassesUnorderedProperty, 18, 9, "Property", "'CodeFixes'", "Test3"),
    DataRow(ThreeClassesUnorderedMethod, 18, 9, "Method", "'AnalyzerTest'", "Test3"),
    DataRow(ThreeClassesUnorderedConstructor, 18, 9, "Constructor", "'Test3'", "Test3"),
    ]
    public void WhenTestCodeInvalidDiagnosticIsRaised(string test, int line, int column, string memberType, string memberName, string className)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1202MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, memberType, memberName, className);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1202)),
            "title",
            FormatedMessage,
            "description",
            DiagnosticSeverity.Warning,
            true
            );

        var expected = new DiagnosticResult(descriptor);
        expected = expected.WithLocation("/0/Test0.cs", line, column);

        Task result = VerifyCS.VerifyAnalyzerAsync(test, expected);
        result.Wait();
    }
}
