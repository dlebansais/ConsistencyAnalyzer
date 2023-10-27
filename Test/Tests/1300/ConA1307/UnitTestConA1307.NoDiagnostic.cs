namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1307
{
    private const string OneDelegate = @"
namespace Test
{
    delegate void twowords();
}
";

    private const string TwoDelegates = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
}
";

    private const string DelegateSchemetwowordsOk1 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void twoWords3();
}
";

    private const string DelegateSchemeTWOWORDSOk1 = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TwoWords3();
}
";

    private const string DelegateSchemetwoWordsOk1 = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void twowords();
}
";

    private const string DelegateSchemeTwoWordsOk1 = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void TWOWORDS3();
}
";

    private const string DelegateSchemetwo_wordsOk1 = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void twowords3();
}
";

    private const string DelegateSchemeTWO_WORDSOk1 = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TWOWORDS3();
}
";

    private const string DelegateSchemetwo_WordsOk1 = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void twowords3();
}
";

    [DataTestMethod]
    [
    DataRow(OneDelegate),
    DataRow(TwoDelegates),
    DataRow(DelegateSchemetwowordsOk1),
    DataRow(DelegateSchemeTWOWORDSOk1),
    DataRow(DelegateSchemetwoWordsOk1),
    DataRow(DelegateSchemeTwoWordsOk1),
    DataRow(DelegateSchemetwo_wordsOk1),
    DataRow(DelegateSchemeTWO_WORDSOk1),
    DataRow(DelegateSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
