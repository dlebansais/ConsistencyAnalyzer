namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1310
    {
        private const string OneProperty = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords { get; set; }
    }
}
";

        private const string TwoPropertys = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1 { get; set; }
        public int twowords2 { get; set; }
    }
}
";

        private const string PropertySchemetwowordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1 { get; set; }
        public int twowords2 { get; set; }
        public int twoWords3 { get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1 { get; set; }
        public int TWOWORDS2 { get; set; }
        public int TwoWords3 { get; set; }
    }
}
";

        private const string PropertySchemetwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1 { get; set; }
        public int twoWords2 { get; set; }
        public int twowords { get; set; }
    }
}
";

        private const string PropertySchemeTwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1 { get; set; }
        public int TwoWords2 { get; set; }
        public int TWOWORDS3 { get; set; }
    }
}
";

        private const string PropertySchemetwo_wordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1 { get; set; }
        public int two_words2 { get; set; }
        public int twowords3 { get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1 { get; set; }
        public int TWO_WORDS2 { get; set; }
        public int TWOWORDS3 { get; set; }
    }
}
";

        private const string PropertySchemetwo_WordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1 { get; set; }
        public int two_Words2 { get; set; }
        public int twowords3 { get; set; }
    }
}
";

        [DataTestMethod]
        [
        DataRow(OneProperty),
        DataRow(TwoPropertys),
        DataRow(PropertySchemetwowordsOk1),
        DataRow(PropertySchemeTWOWORDSOk1),
        DataRow(PropertySchemetwoWordsOk1),
        DataRow(PropertySchemeTwoWordsOk1),
        DataRow(PropertySchemetwo_wordsOk1),
        DataRow(PropertySchemeTWO_WORDSOk1),
        DataRow(PropertySchemetwo_WordsOk1),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
