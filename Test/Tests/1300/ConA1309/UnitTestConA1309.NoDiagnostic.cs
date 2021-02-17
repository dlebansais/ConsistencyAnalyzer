namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1309
    {
        private const string OneField = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords;
    }
}
";

        private const string TwoFields = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
    }
}
";

        private const string FieldSchemetwowordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int twoWords3;
    }
}
";

        private const string FieldSchemeTWOWORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TwoWords3;
    }
}
";

        private const string FieldSchemetwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int twowords;
    }
}
";

        private const string FieldSchemeTwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int TWOWORDS3;
    }
}
";

        private const string FieldSchemetwo_wordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int twowords3;
    }
}
";

        private const string FieldSchemeTWO_WORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TWOWORDS3;
    }
}
";

        private const string FieldSchemetwo_WordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int twowords3;
    }
}
";

        [DataTestMethod]
        [
        DataRow(OneField),
        DataRow(TwoFields),
        DataRow(FieldSchemetwowordsOk1),
        DataRow(FieldSchemeTWOWORDSOk1),
        DataRow(FieldSchemetwoWordsOk1),
        DataRow(FieldSchemeTwoWordsOk1),
        DataRow(FieldSchemetwo_wordsOk1),
        DataRow(FieldSchemeTWO_WORDSOk1),
        DataRow(FieldSchemetwo_WordsOk1),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
