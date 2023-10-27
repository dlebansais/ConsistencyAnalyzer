namespace ConsistencyAnalyzer.Test;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1313
{
    private const string OneTypeParameter = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords>
    {
    }
}
";

    private const string TwoTypeParameters = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2>
    {
    }
}
";

    private const string TypeParameterSchemetwowordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, twowords>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_wordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_WordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, twowords3>
    {
    }
}
";

    [DataTestMethod]
    [
    DataRow(OneTypeParameter),
    DataRow(TwoTypeParameters),
    DataRow(TypeParameterSchemetwowordsOk1),
    DataRow(TypeParameterSchemeTWOWORDSOk1),
    DataRow(TypeParameterSchemetwoWordsOk1),
    DataRow(TypeParameterSchemeTwoWordsOk1),
    DataRow(TypeParameterSchemetwo_wordsOk1),
    DataRow(TypeParameterSchemeTWO_WORDSOk1),
    DataRow(TypeParameterSchemetwo_WordsOk1),
    ]
    public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
    {
        Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
        result.Wait();
    }
}
