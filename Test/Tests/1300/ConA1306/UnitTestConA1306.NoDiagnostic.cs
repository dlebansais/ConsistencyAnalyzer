namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1306
{
    private const string OneEnumMember = @"
enum Test
{
    twowords,
}
";

    private const string TwoEnumMembers = @"
enum Test
{
    twowords1,
    twowords2,
}
";

    private const string EnumMemberSchemetwowordsOk1 = @"
enum Test
{
    twowords1,
    twowords2,
    twoWords3,
}
";

    private const string EnumMemberSchemeTWOWORDSOk1 = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TwoWords3,
}
";

    private const string EnumMemberSchemetwoWordsOk1 = @"
enum Test
{
    twoWords1,
    twoWords2,
    twowords,
}
";

    private const string EnumMemberSchemeTwoWordsOk1 = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    TWOWORDS3,
}
";

    private const string EnumMemberSchemetwo_wordsOk1 = @"
enum Test
{
    two_words1,
    two_words2,
    twowords3,
}
";

    private const string EnumMemberSchemeTWO_WORDSOk1 = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TWOWORDS3,
}
";

    private const string EnumMemberSchemetwo_WordsOk1 = @"
enum Test
{
    two_Words1,
    two_Words2,
    twowords3,
}
";

    [DataTestMethod]
    [
    DataRow(OneEnumMember),
    DataRow(TwoEnumMembers),
    DataRow(EnumMemberSchemetwowordsOk1),
    DataRow(EnumMemberSchemeTWOWORDSOk1),
    DataRow(EnumMemberSchemetwoWordsOk1),
    DataRow(EnumMemberSchemeTwoWordsOk1),
    DataRow(EnumMemberSchemetwo_wordsOk1),
    DataRow(EnumMemberSchemeTWO_WORDSOk1),
    DataRow(EnumMemberSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
