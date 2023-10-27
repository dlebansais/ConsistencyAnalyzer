namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1313
{
    private const string TypeParameterSchemetwowords1 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords3 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords4 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords5 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords6 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords7 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwowords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS2 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTWOWORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWOWORDS1, TWOWORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords2 = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, tWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twoWords1, twoWords2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords1 = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, Twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TwoWords1, TwoWords2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words2 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words3 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words4 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words5 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words6 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words7 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_words1, two_words2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS3 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTWO_WORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<TWO_WORDS1, TWO_WORDS2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemetwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<two_Words1, two_Words2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words1 = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, Twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, TWOWORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, Twowords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, twoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, TwoWords3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, two_words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, TWO_WORDS3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, Two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, two_Words3>
    {
    }
}
";

    private const string TypeParameterSchemeTwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<Two_Words1, Two_Words2, Two_Words3>
    {
    }
}
";

    private const string Trivia1 = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, /* */TWOWORDS3/* */>
    {
    }
}
";

    private const string Trivia1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test<twowords1, twowords2, /* */twowords3/* */>
    {
    }
}
";

    [DataTestMethod]
    [
    DataRow(TypeParameterSchemetwowords1, TypeParameterSchemetwowords1Fixed, 4, 38, "TWOWORDS3"),
    DataRow(TypeParameterSchemetwowords3, TypeParameterSchemetwowords3Fixed, 4, 38, "TwoWords3"),
    DataRow(TypeParameterSchemetwowords4, TypeParameterSchemetwowords4Fixed, 4, 38, "two_words3"),
    DataRow(TypeParameterSchemetwowords5, TypeParameterSchemetwowords5Fixed, 4, 38, "TWO_WORDS3"),
    DataRow(TypeParameterSchemetwowords6, TypeParameterSchemetwowords6Fixed, 4, 38, "two_Words3"),
    DataRow(TypeParameterSchemetwowords7, TypeParameterSchemetwowords7Fixed, 4, 38, "Two_Words3"),
    DataRow(TypeParameterSchemeTWOWORDS1, TypeParameterSchemeTWOWORDS1Fixed, 4, 38, "twowords3"),
    DataRow(TypeParameterSchemeTWOWORDS2, TypeParameterSchemeTWOWORDS2Fixed, 4, 38, "twoWords3"),
    DataRow(TypeParameterSchemeTWOWORDS4, TypeParameterSchemeTWOWORDS4Fixed, 4, 38, "two_words3"),
    DataRow(TypeParameterSchemeTWOWORDS5, TypeParameterSchemeTWOWORDS5Fixed, 4, 38, "TWO_WORDS3"),
    DataRow(TypeParameterSchemeTWOWORDS6, TypeParameterSchemeTWOWORDS6Fixed, 4, 38, "two_Words3"),
    DataRow(TypeParameterSchemeTWOWORDS7, TypeParameterSchemeTWOWORDS7Fixed, 4, 38, "Two_Words3"),
    DataRow(TypeParameterSchemetwoWords2, TypeParameterSchemetwoWords2Fixed, 4, 38, "TWOWORDS3"),
    DataRow(TypeParameterSchemetwoWords3, TypeParameterSchemetwoWords3Fixed, 4, 38, "TwoWords3"),
    DataRow(TypeParameterSchemetwoWords4, TypeParameterSchemetwoWords4Fixed, 4, 38, "two_words3"),
    DataRow(TypeParameterSchemetwoWords5, TypeParameterSchemetwoWords5Fixed, 4, 38, "TWO_WORDS3"),
    DataRow(TypeParameterSchemetwoWords6, TypeParameterSchemetwoWords6Fixed, 4, 38, "two_Words3"),
    DataRow(TypeParameterSchemetwoWords7, TypeParameterSchemetwoWords7Fixed, 4, 38, "Two_Words3"),
    DataRow(TypeParameterSchemeTwoWords1, TypeParameterSchemeTwoWords1Fixed, 4, 38, "twowords3"),
    DataRow(TypeParameterSchemeTwoWords3, TypeParameterSchemeTwoWords3Fixed, 4, 38, "twoWords3"),
    DataRow(TypeParameterSchemeTwoWords4, TypeParameterSchemeTwoWords4Fixed, 4, 38, "two_words3"),
    DataRow(TypeParameterSchemeTwoWords5, TypeParameterSchemeTwoWords5Fixed, 4, 38, "TWO_WORDS3"),
    DataRow(TypeParameterSchemeTwoWords6, TypeParameterSchemeTwoWords6Fixed, 4, 38, "two_Words3"),
    DataRow(TypeParameterSchemeTwoWords7, TypeParameterSchemeTwoWords7Fixed, 4, 38, "Two_Words3"),
    DataRow(TypeParameterSchemetwo_words2, TypeParameterSchemetwo_words2Fixed, 4, 40, "TWOWORDS3"),
    DataRow(TypeParameterSchemetwo_words3, TypeParameterSchemetwo_words3Fixed, 4, 40, "twoWords3"),
    DataRow(TypeParameterSchemetwo_words4, TypeParameterSchemetwo_words4Fixed, 4, 40, "TwoWords3"),
    DataRow(TypeParameterSchemetwo_words5, TypeParameterSchemetwo_words5Fixed, 4, 40, "TWO_WORDS3"),
    DataRow(TypeParameterSchemetwo_words6, TypeParameterSchemetwo_words6Fixed, 4, 40, "two_Words3"),
    DataRow(TypeParameterSchemetwo_words7, TypeParameterSchemetwo_words7Fixed, 4, 40, "Two_Words3"),
    DataRow(TypeParameterSchemeTWO_WORDS1, TypeParameterSchemeTWO_WORDS1Fixed, 4, 40, "twowords3"),
    DataRow(TypeParameterSchemeTWO_WORDS3, TypeParameterSchemeTWO_WORDS3Fixed, 4, 40, "twoWords3"),
    DataRow(TypeParameterSchemeTWO_WORDS4, TypeParameterSchemeTWO_WORDS4Fixed, 4, 40, "TwoWords3"),
    DataRow(TypeParameterSchemeTWO_WORDS5, TypeParameterSchemeTWO_WORDS5Fixed, 4, 40, "two_words3"),
    DataRow(TypeParameterSchemeTWO_WORDS6, TypeParameterSchemeTWO_WORDS6Fixed, 4, 40, "two_Words3"),
    DataRow(TypeParameterSchemeTWO_WORDS7, TypeParameterSchemeTWO_WORDS7Fixed, 4, 40, "Two_Words3"),
    DataRow(TypeParameterSchemetwo_Words2, TypeParameterSchemetwo_Words2Fixed, 4, 40, "TWOWORDS3"),
    DataRow(TypeParameterSchemetwo_Words3, TypeParameterSchemetwo_Words3Fixed, 4, 40, "twoWords3"),
    DataRow(TypeParameterSchemetwo_Words4, TypeParameterSchemetwo_Words4Fixed, 4, 40, "TwoWords3"),
    DataRow(TypeParameterSchemetwo_Words5, TypeParameterSchemetwo_Words5Fixed, 4, 40, "two_words3"),
    DataRow(TypeParameterSchemetwo_Words6, TypeParameterSchemetwo_Words6Fixed, 4, 40, "TWO_WORDS3"),
    DataRow(TypeParameterSchemetwo_Words7, TypeParameterSchemetwo_Words7Fixed, 4, 40, "Two_Words3"),
    DataRow(TypeParameterSchemeTwo_Words1, TypeParameterSchemeTwo_Words1Fixed, 4, 40, "twowords3"),
    DataRow(TypeParameterSchemeTwo_Words2, TypeParameterSchemeTwo_Words2Fixed, 4, 40, "TWOWORDS3"),
    DataRow(TypeParameterSchemeTwo_Words3, TypeParameterSchemeTwo_Words3Fixed, 4, 40, "twoWords3"),
    DataRow(TypeParameterSchemeTwo_Words4, TypeParameterSchemeTwo_Words4Fixed, 4, 40, "TwoWords3"),
    DataRow(TypeParameterSchemeTwo_Words5, TypeParameterSchemeTwo_Words5Fixed, 4, 40, "two_words3"),
    DataRow(TypeParameterSchemeTwo_Words6, TypeParameterSchemeTwo_Words6Fixed, 4, 40, "TWO_WORDS3"),
    DataRow(TypeParameterSchemeTwo_Words7, TypeParameterSchemeTwo_Words7Fixed, 4, 40, "two_Words3"),
    DataRow(Trivia1, Trivia1Fixed, 4, 43, "TWOWORDS3"),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1313MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1313)),
            "title",
            FormatedMessage,
            "description",
            DiagnosticSeverity.Warning,
            true
            );

        var expected = new DiagnosticResult(descriptor);
        expected = expected.WithLocation("/0/Test0.cs", line, column);

        Task result = VerifyCS.VerifyCodeFixAsync(test, expected, fixedsource);
        result.Wait();
    }
}
