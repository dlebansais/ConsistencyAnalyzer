namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1304
    {
        private const string OneEnum = @"
enum twowords
{
}
";

        private const string TwoEnums = @"
enum twowords1
{
}

enum twowords2
{
}
";

        private const string EnumSchemetwowordsOk1 = @"
enum twowords1
{
}

enum twowords2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemeTWOWORDSOk1 = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemetwoWordsOk1 = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum twowords
{
}
";

        private const string EnumSchemeTwoWordsOk1 = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemetwo_wordsOk1 = @"
enum two_words1
{
}

enum two_words2
{
}

enum twowords3
{
}
";

        private const string EnumSchemeTWO_WORDSOk1 = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemetwo_WordsOk1 = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum twowords3
{
}
";

        [DataTestMethod]
        [
        DataRow(OneEnum),
        DataRow(TwoEnums),
        DataRow(EnumSchemetwowordsOk1),
        DataRow(EnumSchemeTWOWORDSOk1),
        DataRow(EnumSchemetwoWordsOk1),
        DataRow(EnumSchemeTwoWordsOk1),
        DataRow(EnumSchemetwo_wordsOk1),
        DataRow(EnumSchemeTWO_WORDSOk1),
        DataRow(EnumSchemetwo_WordsOk1),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
