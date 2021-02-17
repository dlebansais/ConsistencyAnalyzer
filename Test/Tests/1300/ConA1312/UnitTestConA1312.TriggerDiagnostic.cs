namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1312
    {
        private const string ParameterSchemetwowords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemetwowords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int twowords3) { }
    }
}
";

        private const string ParameterSchemetwowords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemetwowords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int twowords3) { }
    }
}
";

        private const string ParameterSchemetwowords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int two_words3) { }
    }
}
";

        private const string ParameterSchemetwowords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int twowords3) { }
    }
}
";

        private const string ParameterSchemetwowords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemetwowords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int twowords3) { }
    }
}
";

        private const string ParameterSchemetwowords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemetwowords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int twowords3) { }
    }
}
";

        private const string ParameterSchemetwowords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemetwowords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int twowords3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int twowords3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int two_words3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemeTWOWORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWOWORDS1, int TWOWORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemetwoWords2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemetwoWords2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int tWOWORDS3) { }
    }
}
";

        private const string ParameterSchemetwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemetwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemetwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int two_words3) { }
    }
}
";

        private const string ParameterSchemetwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemetwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemetwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemetwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemetwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemetwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemetwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twoWords1, int twoWords2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int twowords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int Twowords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int two_words3) { }
    }
}
";

        private const string ParameterSchemeTwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemeTwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemeTwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TwoWords1, int TwoWords2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemetwo_words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemetwo_words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int twowords3) { }
    }
}
";

        private const string ParameterSchemetwo_words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemetwo_words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int two_words3) { }
    }
}
";

        private const string ParameterSchemetwo_words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemetwo_words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int two_words3) { }
    }
}
";

        private const string ParameterSchemetwo_words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemetwo_words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int two_words3) { }
    }
}
";

        private const string ParameterSchemetwo_words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemetwo_words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int two_words3) { }
    }
}
";

        private const string ParameterSchemetwo_words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemetwo_words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_words1, int two_words2, int two_words3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int twowords3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int two_words3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemeTWO_WORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int TWO_WORDS1, int TWO_WORDS2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemetwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemetwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int twowords3) { }
    }
}
";

        private const string ParameterSchemetwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemetwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemetwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemetwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemetwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int two_words3) { }
    }
}
";

        private const string ParameterSchemetwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemetwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemetwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemetwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemetwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int two_Words1, int two_Words2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int twowords3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int Twowords3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int TWOWORDS3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int Twowords3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int twoWords3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int TwoWords3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int two_words3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int TWO_WORDS3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int Two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int two_Words3) { }
    }
}
";

        private const string ParameterSchemeTwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int Two_Words1, int Two_Words2, int Two_Words3) { }
    }
}
";

        private const string Trivia1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int /* */TWOWORDS3/* */) { }
    }
}
";

        private const string Trivia1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test(int twowords1, int twowords2, int /* */twowords3/* */) { }
    }
}
";

        [DataTestMethod]
        [
        DataRow(ParameterSchemetwowords1, ParameterSchemetwowords1Fixed, 6, 51, "TWOWORDS3"),
        DataRow(ParameterSchemetwowords3, ParameterSchemetwowords3Fixed, 6, 51, "TwoWords3"),
        DataRow(ParameterSchemetwowords4, ParameterSchemetwowords4Fixed, 6, 51, "two_words3"),
        DataRow(ParameterSchemetwowords5, ParameterSchemetwowords5Fixed, 6, 51, "TWO_WORDS3"),
        DataRow(ParameterSchemetwowords6, ParameterSchemetwowords6Fixed, 6, 51, "two_Words3"),
        DataRow(ParameterSchemetwowords7, ParameterSchemetwowords7Fixed, 6, 51, "Two_Words3"),
        DataRow(ParameterSchemeTWOWORDS1, ParameterSchemeTWOWORDS1Fixed, 6, 51, "twowords3"),
        DataRow(ParameterSchemeTWOWORDS2, ParameterSchemeTWOWORDS2Fixed, 6, 51, "twoWords3"),
        DataRow(ParameterSchemeTWOWORDS4, ParameterSchemeTWOWORDS4Fixed, 6, 51, "two_words3"),
        DataRow(ParameterSchemeTWOWORDS5, ParameterSchemeTWOWORDS5Fixed, 6, 51, "TWO_WORDS3"),
        DataRow(ParameterSchemeTWOWORDS6, ParameterSchemeTWOWORDS6Fixed, 6, 51, "two_Words3"),
        DataRow(ParameterSchemeTWOWORDS7, ParameterSchemeTWOWORDS7Fixed, 6, 51, "Two_Words3"),
        DataRow(ParameterSchemetwoWords2, ParameterSchemetwoWords2Fixed, 6, 51, "TWOWORDS3"),
        DataRow(ParameterSchemetwoWords3, ParameterSchemetwoWords3Fixed, 6, 51, "TwoWords3"),
        DataRow(ParameterSchemetwoWords4, ParameterSchemetwoWords4Fixed, 6, 51, "two_words3"),
        DataRow(ParameterSchemetwoWords5, ParameterSchemetwoWords5Fixed, 6, 51, "TWO_WORDS3"),
        DataRow(ParameterSchemetwoWords6, ParameterSchemetwoWords6Fixed, 6, 51, "two_Words3"),
        DataRow(ParameterSchemetwoWords7, ParameterSchemetwoWords7Fixed, 6, 51, "Two_Words3"),
        DataRow(ParameterSchemeTwoWords1, ParameterSchemeTwoWords1Fixed, 6, 51, "twowords3"),
        DataRow(ParameterSchemeTwoWords3, ParameterSchemeTwoWords3Fixed, 6, 51, "twoWords3"),
        DataRow(ParameterSchemeTwoWords4, ParameterSchemeTwoWords4Fixed, 6, 51, "two_words3"),
        DataRow(ParameterSchemeTwoWords5, ParameterSchemeTwoWords5Fixed, 6, 51, "TWO_WORDS3"),
        DataRow(ParameterSchemeTwoWords6, ParameterSchemeTwoWords6Fixed, 6, 51, "two_Words3"),
        DataRow(ParameterSchemeTwoWords7, ParameterSchemeTwoWords7Fixed, 6, 51, "Two_Words3"),
        DataRow(ParameterSchemetwo_words2, ParameterSchemetwo_words2Fixed, 6, 53, "TWOWORDS3"),
        DataRow(ParameterSchemetwo_words3, ParameterSchemetwo_words3Fixed, 6, 53, "twoWords3"),
        DataRow(ParameterSchemetwo_words4, ParameterSchemetwo_words4Fixed, 6, 53, "TwoWords3"),
        DataRow(ParameterSchemetwo_words5, ParameterSchemetwo_words5Fixed, 6, 53, "TWO_WORDS3"),
        DataRow(ParameterSchemetwo_words6, ParameterSchemetwo_words6Fixed, 6, 53, "two_Words3"),
        DataRow(ParameterSchemetwo_words7, ParameterSchemetwo_words7Fixed, 6, 53, "Two_Words3"),
        DataRow(ParameterSchemeTWO_WORDS1, ParameterSchemeTWO_WORDS1Fixed, 6, 53, "twowords3"),
        DataRow(ParameterSchemeTWO_WORDS3, ParameterSchemeTWO_WORDS3Fixed, 6, 53, "twoWords3"),
        DataRow(ParameterSchemeTWO_WORDS4, ParameterSchemeTWO_WORDS4Fixed, 6, 53, "TwoWords3"),
        DataRow(ParameterSchemeTWO_WORDS5, ParameterSchemeTWO_WORDS5Fixed, 6, 53, "two_words3"),
        DataRow(ParameterSchemeTWO_WORDS6, ParameterSchemeTWO_WORDS6Fixed, 6, 53, "two_Words3"),
        DataRow(ParameterSchemeTWO_WORDS7, ParameterSchemeTWO_WORDS7Fixed, 6, 53, "Two_Words3"),
        DataRow(ParameterSchemetwo_Words2, ParameterSchemetwo_Words2Fixed, 6, 53, "TWOWORDS3"),
        DataRow(ParameterSchemetwo_Words3, ParameterSchemetwo_Words3Fixed, 6, 53, "twoWords3"),
        DataRow(ParameterSchemetwo_Words4, ParameterSchemetwo_Words4Fixed, 6, 53, "TwoWords3"),
        DataRow(ParameterSchemetwo_Words5, ParameterSchemetwo_Words5Fixed, 6, 53, "two_words3"),
        DataRow(ParameterSchemetwo_Words6, ParameterSchemetwo_Words6Fixed, 6, 53, "TWO_WORDS3"),
        DataRow(ParameterSchemetwo_Words7, ParameterSchemetwo_Words7Fixed, 6, 53, "Two_Words3"),
        DataRow(ParameterSchemeTwo_Words1, ParameterSchemeTwo_Words1Fixed, 6, 53, "twowords3"),
        DataRow(ParameterSchemeTwo_Words2, ParameterSchemeTwo_Words2Fixed, 6, 53, "TWOWORDS3"),
        DataRow(ParameterSchemeTwo_Words3, ParameterSchemeTwo_Words3Fixed, 6, 53, "twoWords3"),
        DataRow(ParameterSchemeTwo_Words4, ParameterSchemeTwo_Words4Fixed, 6, 53, "TwoWords3"),
        DataRow(ParameterSchemeTwo_Words5, ParameterSchemeTwo_Words5Fixed, 6, 53, "two_words3"),
        DataRow(ParameterSchemeTwo_Words6, ParameterSchemeTwo_Words6Fixed, 6, 53, "TWO_WORDS3"),
        DataRow(ParameterSchemeTwo_Words7, ParameterSchemeTwo_Words7Fixed, 6, 53, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 6, 51, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1312MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1312)),
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
}
