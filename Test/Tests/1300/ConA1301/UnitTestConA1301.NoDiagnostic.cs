namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1301
{
    private const string OneClass = @"
class twowords
{
}
";

    private const string TwoClasses = @"
class twowords1
{
}

class twowords2
{
}
";

    private const string ClassSchemetwowordsOk1 = @"
class twowords1
{
}

class twowords2
{
}

class twoWords3
{
}
";

    private const string ClassSchemeTWOWORDSOk1 = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemetwoWordsOk1 = @"
class twoWords1
{
}

class twoWords2
{
}

class twowords
{
}
";

    private const string ClassSchemeTwoWordsOk1 = @"
class TwoWords1
{
}

class TwoWords2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemetwo_wordsOk1 = @"
class two_words1
{
}

class two_words2
{
}

class twowords3
{
}
";

    private const string ClassSchemeTWO_WORDSOk1 = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemetwo_WordsOk1 = @"
class two_Words1
{
}

class two_Words2
{
}

class twowords3
{
}
";

    [DataTestMethod]
    [
    DataRow(OneClass),
    DataRow(TwoClasses),
    DataRow(ClassSchemetwowordsOk1),
    DataRow(ClassSchemeTWOWORDSOk1),
    DataRow(ClassSchemetwoWordsOk1),
    DataRow(ClassSchemeTwoWordsOk1),
    DataRow(ClassSchemetwo_wordsOk1),
    DataRow(ClassSchemeTWO_WORDSOk1),
    DataRow(ClassSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
