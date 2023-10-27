namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1305
{
    private const string OneInterface = @"
interface twowords
{
}
";

    private const string TwoInterfaces = @"
interface twowords1
{
}

interface twowords2
{
}
";

    private const string InterfaceSchemetwowordsOk1 = @"
interface twowords1
{
}

interface twowords2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemeTWOWORDSOk1 = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemetwoWordsOk1 = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface twowords
{
}
";

    private const string InterfaceSchemeTwoWordsOk1 = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemetwo_wordsOk1 = @"
interface two_words1
{
}

interface two_words2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemeTWO_WORDSOk1 = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemetwo_WordsOk1 = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface twowords3
{
}
";

    [DataTestMethod]
    [
    DataRow(OneInterface),
    DataRow(TwoInterfaces),
    DataRow(InterfaceSchemetwowordsOk1),
    DataRow(InterfaceSchemeTWOWORDSOk1),
    DataRow(InterfaceSchemetwoWordsOk1),
    DataRow(InterfaceSchemeTwoWordsOk1),
    DataRow(InterfaceSchemetwo_wordsOk1),
    DataRow(InterfaceSchemeTWO_WORDSOk1),
    DataRow(InterfaceSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
