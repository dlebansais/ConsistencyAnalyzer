namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1311
{
    private const string MethodSchemetwowords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwowords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemetwowords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemetwowords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemetwowords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemetwowords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemetwowords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemetwowords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemetwowords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemetwowords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemetwowords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemetwowords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemeTWOWORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWOWORDS1() { }
        public void TWOWORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwoWords2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwoWords2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void tWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemetwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemetwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemetwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemetwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemetwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemetwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemetwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemetwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemetwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twoWords1() { }
        public void twoWords2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemeTwoWords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemeTwoWords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void Twowords3() { }
    }
}
";

    private const string MethodSchemeTwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemeTwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemeTwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemeTwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemeTwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemeTwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemeTwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemeTwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemeTwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemeTwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TwoWords1() { }
        public void TwoWords2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemetwo_words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwo_words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemetwo_words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemetwo_words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemetwo_words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemetwo_words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemetwo_words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemetwo_words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemetwo_words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemetwo_words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemetwo_words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemetwo_words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_words1() { }
        public void two_words2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemeTWO_WORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void TWO_WORDS1() { }
        public void TWO_WORDS2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemetwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemetwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemetwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemetwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemetwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemetwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemetwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemetwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemetwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemetwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemetwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemetwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void two_Words1() { }
        public void two_Words2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemeTwo_Words1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void twowords3() { }
    }
}
";

    private const string MethodSchemeTwo_Words1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void Twowords3() { }
    }
}
";

    private const string MethodSchemeTwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void TWOWORDS3() { }
    }
}
";

    private const string MethodSchemeTwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void Twowords3() { }
    }
}
";

    private const string MethodSchemeTwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void twoWords3() { }
    }
}
";

    private const string MethodSchemeTwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemeTwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void TwoWords3() { }
    }
}
";

    private const string MethodSchemeTwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemeTwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void two_words3() { }
    }
}
";

    private const string MethodSchemeTwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemeTwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void TWO_WORDS3() { }
    }
}
";

    private const string MethodSchemeTwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void Two_Words3() { }
    }
}
";

    private const string MethodSchemeTwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void two_Words3() { }
    }
}
";

    private const string MethodSchemeTwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void Two_Words1() { }
        public void Two_Words2() { }
        public void Two_Words3() { }
    }
}
";

    private const string Trivia1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void /* */TWOWORDS3/* */() { }
    }
}
";

    private const string Trivia1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public void twowords1() { }
        public void twowords2() { }
        public void /* */twowords3/* */() { }
    }
}
";

    [DataTestMethod]
    [
    DataRow(MethodSchemetwowords1, MethodSchemetwowords1Fixed, 8, 9, "TWOWORDS3"),
    DataRow(MethodSchemetwowords3, MethodSchemetwowords3Fixed, 8, 9, "TwoWords3"),
    DataRow(MethodSchemetwowords4, MethodSchemetwowords4Fixed, 8, 9, "two_words3"),
    DataRow(MethodSchemetwowords5, MethodSchemetwowords5Fixed, 8, 9, "TWO_WORDS3"),
    DataRow(MethodSchemetwowords6, MethodSchemetwowords6Fixed, 8, 9, "two_Words3"),
    DataRow(MethodSchemetwowords7, MethodSchemetwowords7Fixed, 8, 9, "Two_Words3"),
    DataRow(MethodSchemeTWOWORDS1, MethodSchemeTWOWORDS1Fixed, 8, 9, "twowords3"),
    DataRow(MethodSchemeTWOWORDS2, MethodSchemeTWOWORDS2Fixed, 8, 9, "twoWords3"),
    DataRow(MethodSchemeTWOWORDS4, MethodSchemeTWOWORDS4Fixed, 8, 9, "two_words3"),
    DataRow(MethodSchemeTWOWORDS5, MethodSchemeTWOWORDS5Fixed, 8, 9, "TWO_WORDS3"),
    DataRow(MethodSchemeTWOWORDS6, MethodSchemeTWOWORDS6Fixed, 8, 9, "two_Words3"),
    DataRow(MethodSchemeTWOWORDS7, MethodSchemeTWOWORDS7Fixed, 8, 9, "Two_Words3"),
    DataRow(MethodSchemetwoWords2, MethodSchemetwoWords2Fixed, 8, 9, "TWOWORDS3"),
    DataRow(MethodSchemetwoWords3, MethodSchemetwoWords3Fixed, 8, 9, "TwoWords3"),
    DataRow(MethodSchemetwoWords4, MethodSchemetwoWords4Fixed, 8, 9, "two_words3"),
    DataRow(MethodSchemetwoWords5, MethodSchemetwoWords5Fixed, 8, 9, "TWO_WORDS3"),
    DataRow(MethodSchemetwoWords6, MethodSchemetwoWords6Fixed, 8, 9, "two_Words3"),
    DataRow(MethodSchemetwoWords7, MethodSchemetwoWords7Fixed, 8, 9, "Two_Words3"),
    DataRow(MethodSchemeTwoWords1, MethodSchemeTwoWords1Fixed, 8, 9, "twowords3"),
    DataRow(MethodSchemeTwoWords3, MethodSchemeTwoWords3Fixed, 8, 9, "twoWords3"),
    DataRow(MethodSchemeTwoWords4, MethodSchemeTwoWords4Fixed, 8, 9, "two_words3"),
    DataRow(MethodSchemeTwoWords5, MethodSchemeTwoWords5Fixed, 8, 9, "TWO_WORDS3"),
    DataRow(MethodSchemeTwoWords6, MethodSchemeTwoWords6Fixed, 8, 9, "two_Words3"),
    DataRow(MethodSchemeTwoWords7, MethodSchemeTwoWords7Fixed, 8, 9, "Two_Words3"),
    DataRow(MethodSchemetwo_words2, MethodSchemetwo_words2Fixed, 8, 9, "TWOWORDS3"),
    DataRow(MethodSchemetwo_words3, MethodSchemetwo_words3Fixed, 8, 9, "twoWords3"),
    DataRow(MethodSchemetwo_words4, MethodSchemetwo_words4Fixed, 8, 9, "TwoWords3"),
    DataRow(MethodSchemetwo_words5, MethodSchemetwo_words5Fixed, 8, 9, "TWO_WORDS3"),
    DataRow(MethodSchemetwo_words6, MethodSchemetwo_words6Fixed, 8, 9, "two_Words3"),
    DataRow(MethodSchemetwo_words7, MethodSchemetwo_words7Fixed, 8, 9, "Two_Words3"),
    DataRow(MethodSchemeTWO_WORDS1, MethodSchemeTWO_WORDS1Fixed, 8, 9, "twowords3"),
    DataRow(MethodSchemeTWO_WORDS3, MethodSchemeTWO_WORDS3Fixed, 8, 9, "twoWords3"),
    DataRow(MethodSchemeTWO_WORDS4, MethodSchemeTWO_WORDS4Fixed, 8, 9, "TwoWords3"),
    DataRow(MethodSchemeTWO_WORDS5, MethodSchemeTWO_WORDS5Fixed, 8, 9, "two_words3"),
    DataRow(MethodSchemeTWO_WORDS6, MethodSchemeTWO_WORDS6Fixed, 8, 9, "two_Words3"),
    DataRow(MethodSchemeTWO_WORDS7, MethodSchemeTWO_WORDS7Fixed, 8, 9, "Two_Words3"),
    DataRow(MethodSchemetwo_Words2, MethodSchemetwo_Words2Fixed, 8, 9, "TWOWORDS3"),
    DataRow(MethodSchemetwo_Words3, MethodSchemetwo_Words3Fixed, 8, 9, "twoWords3"),
    DataRow(MethodSchemetwo_Words4, MethodSchemetwo_Words4Fixed, 8, 9, "TwoWords3"),
    DataRow(MethodSchemetwo_Words5, MethodSchemetwo_Words5Fixed, 8, 9, "two_words3"),
    DataRow(MethodSchemetwo_Words6, MethodSchemetwo_Words6Fixed, 8, 9, "TWO_WORDS3"),
    DataRow(MethodSchemetwo_Words7, MethodSchemetwo_Words7Fixed, 8, 9, "Two_Words3"),
    DataRow(MethodSchemeTwo_Words1, MethodSchemeTwo_Words1Fixed, 8, 9, "twowords3"),
    DataRow(MethodSchemeTwo_Words2, MethodSchemeTwo_Words2Fixed, 8, 9, "TWOWORDS3"),
    DataRow(MethodSchemeTwo_Words3, MethodSchemeTwo_Words3Fixed, 8, 9, "twoWords3"),
    DataRow(MethodSchemeTwo_Words4, MethodSchemeTwo_Words4Fixed, 8, 9, "TwoWords3"),
    DataRow(MethodSchemeTwo_Words5, MethodSchemeTwo_Words5Fixed, 8, 9, "two_words3"),
    DataRow(MethodSchemeTwo_Words6, MethodSchemeTwo_Words6Fixed, 8, 9, "TWO_WORDS3"),
    DataRow(MethodSchemeTwo_Words7, MethodSchemeTwo_Words7Fixed, 8, 9, "two_Words3"),
    DataRow(Trivia1, Trivia1Fixed, 8, 9, "TWOWORDS3"),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1311MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1311)),
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
