namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1308
{
    private const string OneEvent = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords;
    }
}
";

    private const string TwoEvents = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
    }
}
";

    private const string EventSchemetwowordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler twoWords3;
    }
}
";

    private const string EventSchemeTWOWORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TwoWords3;
    }
}
";

    private const string EventSchemetwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler twowords;
    }
}
";

    private const string EventSchemeTwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

    private const string EventSchemetwo_wordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler twowords3;
    }
}
";

    private const string EventSchemeTWO_WORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

    private const string EventSchemetwo_WordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler twowords3;
    }
}
";

    [DataTestMethod]
    [
    DataRow(OneEvent),
    DataRow(TwoEvents),
    DataRow(EventSchemetwowordsOk1),
    DataRow(EventSchemeTWOWORDSOk1),
    DataRow(EventSchemetwoWordsOk1),
    DataRow(EventSchemeTwoWordsOk1),
    DataRow(EventSchemetwo_wordsOk1),
    DataRow(EventSchemeTWO_WORDSOk1),
    DataRow(EventSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
