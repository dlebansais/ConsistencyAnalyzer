namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1311
{
    private const string OneMethod = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords() { }
    }
}
";

    private const string TwoMethods = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
    }
}
";

    private const string MethodSchemetwowordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemeTWOWORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemetwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void twowords() { }
    }
}
";

    private const string MethodSchemeTwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwo_wordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwo_WordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void twowords3() { }
    }
}
";

    [DataTestMethod]
    [
    DataRow(OneMethod),
    DataRow(TwoMethods),
    DataRow(MethodSchemetwowordsOk1),
    DataRow(MethodSchemeTWOWORDSOk1),
    DataRow(MethodSchemetwoWordsOk1),
    DataRow(MethodSchemeTwoWordsOk1),
    DataRow(MethodSchemetwo_wordsOk1),
    DataRow(MethodSchemeTWO_WORDSOk1),
    DataRow(MethodSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
