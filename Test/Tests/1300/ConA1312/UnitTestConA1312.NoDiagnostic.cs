namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1312
{
    private const string OneParameter = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords) { }
    }
}
";

    private const string TwoParameters = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2) { }
    }
}
";

    private const string ParameterSchemetwowordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int twoWords3) { }
    }
}
";

    private const string ParameterSchemeTWOWORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TwoWords3) { }
    }
}
";

    private const string ParameterSchemetwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int twowords) { }
    }
}
";

    private const string ParameterSchemeTwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int TWOWORDS3) { }
    }
}
";

    private const string ParameterSchemetwo_wordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int twowords3) { }
    }
}
";

    private const string ParameterSchemeTWO_WORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TWOWORDS3) { }
    }
}
";

    private const string ParameterSchemetwo_WordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int twowords3) { }
    }
}
";

    [DataTestMethod]
    [
    DataRow(OneParameter),
    DataRow(TwoParameters),
    DataRow(ParameterSchemetwowordsOk1),
    DataRow(ParameterSchemeTWOWORDSOk1),
    DataRow(ParameterSchemetwoWordsOk1),
    DataRow(ParameterSchemeTwoWordsOk1),
    DataRow(ParameterSchemetwo_wordsOk1),
    DataRow(ParameterSchemeTWO_WORDSOk1),
    DataRow(ParameterSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
