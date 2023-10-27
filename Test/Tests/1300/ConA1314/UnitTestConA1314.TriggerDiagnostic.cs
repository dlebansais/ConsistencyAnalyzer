namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1314
{
    private const string LocalVariableSchemetwowords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemetwowords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemetwowords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemetwowords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemetwowords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemetwowords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemetwowords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemetwowords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemetwowords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwowords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemetwowords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwowords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTWOWORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemetwoWords2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemetwoWords2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int tWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemetwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemetwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemetwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemetwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemetwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemetwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemetwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemetwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int Twowords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemetwo_words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemetwo_words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemetwo_words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemetwo_words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemetwo_words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemetwo_words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemetwo_words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemetwo_words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemetwo_words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwo_words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemetwo_words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwo_words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTWO_WORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemetwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int twowords3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int Twowords3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int TWOWORDS3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int Twowords3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int twoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int TwoWords3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int two_words3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int TWO_WORDS3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int Two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int two_Words3; }
    }
}
";

    private const string LocalVariableSchemeTwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int Two_Words1; int Two_Words2; int Two_Words3; }
    }
}
";

    private const string Trivia1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int /* */TWOWORDS3/* */; }
    }
}
";

    private const string Trivia1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int /* */twowords3/* */; }
    }
}
";

    [DataTestMethod]
    [
    DataRow(LocalVariableSchemetwowords1, LocalVariableSchemetwowords1Fixed, 6, 59, "TWOWORDS3"),
    DataRow(LocalVariableSchemetwowords3, LocalVariableSchemetwowords3Fixed, 6, 59, "TwoWords3"),
    DataRow(LocalVariableSchemetwowords4, LocalVariableSchemetwowords4Fixed, 6, 59, "two_words3"),
    DataRow(LocalVariableSchemetwowords5, LocalVariableSchemetwowords5Fixed, 6, 59, "TWO_WORDS3"),
    DataRow(LocalVariableSchemetwowords6, LocalVariableSchemetwowords6Fixed, 6, 59, "two_Words3"),
    DataRow(LocalVariableSchemetwowords7, LocalVariableSchemetwowords7Fixed, 6, 59, "Two_Words3"),
    DataRow(LocalVariableSchemeTWOWORDS1, LocalVariableSchemeTWOWORDS1Fixed, 6, 59, "twowords3"),
    DataRow(LocalVariableSchemeTWOWORDS2, LocalVariableSchemeTWOWORDS2Fixed, 6, 59, "twoWords3"),
    DataRow(LocalVariableSchemeTWOWORDS4, LocalVariableSchemeTWOWORDS4Fixed, 6, 59, "two_words3"),
    DataRow(LocalVariableSchemeTWOWORDS5, LocalVariableSchemeTWOWORDS5Fixed, 6, 59, "TWO_WORDS3"),
    DataRow(LocalVariableSchemeTWOWORDS6, LocalVariableSchemeTWOWORDS6Fixed, 6, 59, "two_Words3"),
    DataRow(LocalVariableSchemeTWOWORDS7, LocalVariableSchemeTWOWORDS7Fixed, 6, 59, "Two_Words3"),
    DataRow(LocalVariableSchemetwoWords2, LocalVariableSchemetwoWords2Fixed, 6, 59, "TWOWORDS3"),
    DataRow(LocalVariableSchemetwoWords3, LocalVariableSchemetwoWords3Fixed, 6, 59, "TwoWords3"),
    DataRow(LocalVariableSchemetwoWords4, LocalVariableSchemetwoWords4Fixed, 6, 59, "two_words3"),
    DataRow(LocalVariableSchemetwoWords5, LocalVariableSchemetwoWords5Fixed, 6, 59, "TWO_WORDS3"),
    DataRow(LocalVariableSchemetwoWords6, LocalVariableSchemetwoWords6Fixed, 6, 59, "two_Words3"),
    DataRow(LocalVariableSchemetwoWords7, LocalVariableSchemetwoWords7Fixed, 6, 59, "Two_Words3"),
    DataRow(LocalVariableSchemeTwoWords1, LocalVariableSchemeTwoWords1Fixed, 6, 59, "twowords3"),
    DataRow(LocalVariableSchemeTwoWords3, LocalVariableSchemeTwoWords3Fixed, 6, 59, "twoWords3"),
    DataRow(LocalVariableSchemeTwoWords4, LocalVariableSchemeTwoWords4Fixed, 6, 59, "two_words3"),
    DataRow(LocalVariableSchemeTwoWords5, LocalVariableSchemeTwoWords5Fixed, 6, 59, "TWO_WORDS3"),
    DataRow(LocalVariableSchemeTwoWords6, LocalVariableSchemeTwoWords6Fixed, 6, 59, "two_Words3"),
    DataRow(LocalVariableSchemeTwoWords7, LocalVariableSchemeTwoWords7Fixed, 6, 59, "Two_Words3"),
    DataRow(LocalVariableSchemetwo_words2, LocalVariableSchemetwo_words2Fixed, 6, 61, "TWOWORDS3"),
    DataRow(LocalVariableSchemetwo_words3, LocalVariableSchemetwo_words3Fixed, 6, 61, "twoWords3"),
    DataRow(LocalVariableSchemetwo_words4, LocalVariableSchemetwo_words4Fixed, 6, 61, "TwoWords3"),
    DataRow(LocalVariableSchemetwo_words5, LocalVariableSchemetwo_words5Fixed, 6, 61, "TWO_WORDS3"),
    DataRow(LocalVariableSchemetwo_words6, LocalVariableSchemetwo_words6Fixed, 6, 61, "two_Words3"),
    DataRow(LocalVariableSchemetwo_words7, LocalVariableSchemetwo_words7Fixed, 6, 61, "Two_Words3"),
    DataRow(LocalVariableSchemeTWO_WORDS1, LocalVariableSchemeTWO_WORDS1Fixed, 6, 61, "twowords3"),
    DataRow(LocalVariableSchemeTWO_WORDS3, LocalVariableSchemeTWO_WORDS3Fixed, 6, 61, "twoWords3"),
    DataRow(LocalVariableSchemeTWO_WORDS4, LocalVariableSchemeTWO_WORDS4Fixed, 6, 61, "TwoWords3"),
    DataRow(LocalVariableSchemeTWO_WORDS5, LocalVariableSchemeTWO_WORDS5Fixed, 6, 61, "two_words3"),
    DataRow(LocalVariableSchemeTWO_WORDS6, LocalVariableSchemeTWO_WORDS6Fixed, 6, 61, "two_Words3"),
    DataRow(LocalVariableSchemeTWO_WORDS7, LocalVariableSchemeTWO_WORDS7Fixed, 6, 61, "Two_Words3"),
    DataRow(LocalVariableSchemetwo_Words2, LocalVariableSchemetwo_Words2Fixed, 6, 61, "TWOWORDS3"),
    DataRow(LocalVariableSchemetwo_Words3, LocalVariableSchemetwo_Words3Fixed, 6, 61, "twoWords3"),
    DataRow(LocalVariableSchemetwo_Words4, LocalVariableSchemetwo_Words4Fixed, 6, 61, "TwoWords3"),
    DataRow(LocalVariableSchemetwo_Words5, LocalVariableSchemetwo_Words5Fixed, 6, 61, "two_words3"),
    DataRow(LocalVariableSchemetwo_Words6, LocalVariableSchemetwo_Words6Fixed, 6, 61, "TWO_WORDS3"),
    DataRow(LocalVariableSchemetwo_Words7, LocalVariableSchemetwo_Words7Fixed, 6, 61, "Two_Words3"),
    DataRow(LocalVariableSchemeTwo_Words1, LocalVariableSchemeTwo_Words1Fixed, 6, 61, "twowords3"),
    DataRow(LocalVariableSchemeTwo_Words2, LocalVariableSchemeTwo_Words2Fixed, 6, 61, "TWOWORDS3"),
    DataRow(LocalVariableSchemeTwo_Words3, LocalVariableSchemeTwo_Words3Fixed, 6, 61, "twoWords3"),
    DataRow(LocalVariableSchemeTwo_Words4, LocalVariableSchemeTwo_Words4Fixed, 6, 61, "TwoWords3"),
    DataRow(LocalVariableSchemeTwo_Words5, LocalVariableSchemeTwo_Words5Fixed, 6, 61, "two_words3"),
    DataRow(LocalVariableSchemeTwo_Words6, LocalVariableSchemeTwo_Words6Fixed, 6, 61, "TWO_WORDS3"),
    DataRow(LocalVariableSchemeTwo_Words7, LocalVariableSchemeTwo_Words7Fixed, 6, 61, "two_Words3"),
    DataRow(Trivia1, Trivia1Fixed, 6, 64, "TWOWORDS3"),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1314MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1314)),
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
