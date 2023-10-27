namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1303
{
    private const string OneStruct = @"
struct twowords
{
}
";

    private const string TwoStructs = @"
struct twowords1
{
}

struct twowords2
{
}
";

    private const string StructSchemetwowordsOk1 = @"
struct twowords1
{
}

struct twowords2
{
}

struct twoWords3
{
}
";

    private const string StructSchemeTWOWORDSOk1 = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TwoWords3
{
}
";

    private const string StructSchemetwoWordsOk1 = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct twowords
{
}
";

    private const string StructSchemeTwoWordsOk1 = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct TWOWORDS3
{
}
";

    private const string StructSchemetwo_wordsOk1 = @"
struct two_words1
{
}

struct two_words2
{
}

struct twowords3
{
}
";

    private const string StructSchemeTWO_WORDSOk1 = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TWOWORDS3
{
}
";

    private const string StructSchemetwo_WordsOk1 = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct twowords3
{
}
";

    [DataTestMethod]
    [
    DataRow(OneStruct),
    DataRow(TwoStructs),
    DataRow(StructSchemetwowordsOk1),
    DataRow(StructSchemeTWOWORDSOk1),
    DataRow(StructSchemetwoWordsOk1),
    DataRow(StructSchemeTwoWordsOk1),
    DataRow(StructSchemetwo_wordsOk1),
    DataRow(StructSchemeTWO_WORDSOk1),
    DataRow(StructSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
