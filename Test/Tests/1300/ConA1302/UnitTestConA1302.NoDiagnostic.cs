namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1302
{
    private const string OneRecord = @"
record twowords
{
}
";

    private const string TwoRecords = @"
record twowords1
{
}

record twowords2
{
}
";

    private const string RecordSchemetwowordsOk1 = @"
record twowords1
{
}

record twowords2
{
}

record twoWords3
{
}
";

    private const string RecordSchemeTWOWORDSOk1 = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TwoWords3
{
}
";

    private const string RecordSchemetwoWordsOk1 = @"
record twoWords1
{
}

record twoWords2
{
}

record twowords
{
}
";

    private const string RecordSchemeTwoWordsOk1 = @"
record TwoWords1
{
}

record TwoWords2
{
}

record TWOWORDS3
{
}
";

    private const string RecordSchemetwo_wordsOk1 = @"
record two_words1
{
}

record two_words2
{
}

record twowords3
{
}
";

    private const string RecordSchemeTWO_WORDSOk1 = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TWOWORDS3
{
}
";

    private const string RecordSchemetwo_WordsOk1 = @"
record two_Words1
{
}

record two_Words2
{
}

record twowords3
{
}
";

    [DataTestMethod]
    [
    DataRow(OneRecord),
    DataRow(TwoRecords),
    DataRow(RecordSchemetwowordsOk1),
    DataRow(RecordSchemeTWOWORDSOk1),
    DataRow(RecordSchemetwoWordsOk1),
    DataRow(RecordSchemeTwoWordsOk1),
    DataRow(RecordSchemetwo_wordsOk1),
    DataRow(RecordSchemeTWO_WORDSOk1),
    DataRow(RecordSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
