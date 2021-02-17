namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1310
    {
        private const string PropertySchemetwowords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwowords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTWOWORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1{ get; set; }
        public int TWOWORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int tWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1{ get; set; }
        public int twoWords2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int Twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1{ get; set; }
        public int TwoWords2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1{ get; set; }
        public int two_words2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTWO_WORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1{ get; set; }
        public int TWO_WORDS2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemetwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1{ get; set; }
        public int two_Words2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int Twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int TWOWORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int Twowords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int twoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int TwoWords3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int two_words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int TWO_WORDS3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int two_Words3{ get; set; }
    }
}
";

        private const string PropertySchemeTwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1{ get; set; }
        public int Two_Words2{ get; set; }
        public int Two_Words3{ get; set; }
    }
}
";

        private const string Trivia1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int /* */TWOWORDS3/* */{ get; set; }
    }
}
";

        private const string Trivia1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1{ get; set; }
        public int twowords2{ get; set; }
        public int /* */twowords3/* */{ get; set; }
    }
}
";

        [DataTestMethod]
        [
        DataRow(PropertySchemetwowords1, PropertySchemetwowords1Fixed, 8, 9, "TWOWORDS3"),
        DataRow(PropertySchemetwowords3, PropertySchemetwowords3Fixed, 8, 9, "TwoWords3"),
        DataRow(PropertySchemetwowords4, PropertySchemetwowords4Fixed, 8, 9, "two_words3"),
        DataRow(PropertySchemetwowords5, PropertySchemetwowords5Fixed, 8, 9, "TWO_WORDS3"),
        DataRow(PropertySchemetwowords6, PropertySchemetwowords6Fixed, 8, 9, "two_Words3"),
        DataRow(PropertySchemetwowords7, PropertySchemetwowords7Fixed, 8, 9, "Two_Words3"),
        DataRow(PropertySchemeTWOWORDS1, PropertySchemeTWOWORDS1Fixed, 8, 9, "twowords3"),
        DataRow(PropertySchemeTWOWORDS2, PropertySchemeTWOWORDS2Fixed, 8, 9, "twoWords3"),
        DataRow(PropertySchemeTWOWORDS4, PropertySchemeTWOWORDS4Fixed, 8, 9, "two_words3"),
        DataRow(PropertySchemeTWOWORDS5, PropertySchemeTWOWORDS5Fixed, 8, 9, "TWO_WORDS3"),
        DataRow(PropertySchemeTWOWORDS6, PropertySchemeTWOWORDS6Fixed, 8, 9, "two_Words3"),
        DataRow(PropertySchemeTWOWORDS7, PropertySchemeTWOWORDS7Fixed, 8, 9, "Two_Words3"),
        DataRow(PropertySchemetwoWords2, PropertySchemetwoWords2Fixed, 8, 9, "TWOWORDS3"),
        DataRow(PropertySchemetwoWords3, PropertySchemetwoWords3Fixed, 8, 9, "TwoWords3"),
        DataRow(PropertySchemetwoWords4, PropertySchemetwoWords4Fixed, 8, 9, "two_words3"),
        DataRow(PropertySchemetwoWords5, PropertySchemetwoWords5Fixed, 8, 9, "TWO_WORDS3"),
        DataRow(PropertySchemetwoWords6, PropertySchemetwoWords6Fixed, 8, 9, "two_Words3"),
        DataRow(PropertySchemetwoWords7, PropertySchemetwoWords7Fixed, 8, 9, "Two_Words3"),
        DataRow(PropertySchemeTwoWords1, PropertySchemeTwoWords1Fixed, 8, 9, "twowords3"),
        DataRow(PropertySchemeTwoWords3, PropertySchemeTwoWords3Fixed, 8, 9, "twoWords3"),
        DataRow(PropertySchemeTwoWords4, PropertySchemeTwoWords4Fixed, 8, 9, "two_words3"),
        DataRow(PropertySchemeTwoWords5, PropertySchemeTwoWords5Fixed, 8, 9, "TWO_WORDS3"),
        DataRow(PropertySchemeTwoWords6, PropertySchemeTwoWords6Fixed, 8, 9, "two_Words3"),
        DataRow(PropertySchemeTwoWords7, PropertySchemeTwoWords7Fixed, 8, 9, "Two_Words3"),
        DataRow(PropertySchemetwo_words2, PropertySchemetwo_words2Fixed, 8, 9, "TWOWORDS3"),
        DataRow(PropertySchemetwo_words3, PropertySchemetwo_words3Fixed, 8, 9, "twoWords3"),
        DataRow(PropertySchemetwo_words4, PropertySchemetwo_words4Fixed, 8, 9, "TwoWords3"),
        DataRow(PropertySchemetwo_words5, PropertySchemetwo_words5Fixed, 8, 9, "TWO_WORDS3"),
        DataRow(PropertySchemetwo_words6, PropertySchemetwo_words6Fixed, 8, 9, "two_Words3"),
        DataRow(PropertySchemetwo_words7, PropertySchemetwo_words7Fixed, 8, 9, "Two_Words3"),
        DataRow(PropertySchemeTWO_WORDS1, PropertySchemeTWO_WORDS1Fixed, 8, 9, "twowords3"),
        DataRow(PropertySchemeTWO_WORDS3, PropertySchemeTWO_WORDS3Fixed, 8, 9, "twoWords3"),
        DataRow(PropertySchemeTWO_WORDS4, PropertySchemeTWO_WORDS4Fixed, 8, 9, "TwoWords3"),
        DataRow(PropertySchemeTWO_WORDS5, PropertySchemeTWO_WORDS5Fixed, 8, 9, "two_words3"),
        DataRow(PropertySchemeTWO_WORDS6, PropertySchemeTWO_WORDS6Fixed, 8, 9, "two_Words3"),
        DataRow(PropertySchemeTWO_WORDS7, PropertySchemeTWO_WORDS7Fixed, 8, 9, "Two_Words3"),
        DataRow(PropertySchemetwo_Words2, PropertySchemetwo_Words2Fixed, 8, 9, "TWOWORDS3"),
        DataRow(PropertySchemetwo_Words3, PropertySchemetwo_Words3Fixed, 8, 9, "twoWords3"),
        DataRow(PropertySchemetwo_Words4, PropertySchemetwo_Words4Fixed, 8, 9, "TwoWords3"),
        DataRow(PropertySchemetwo_Words5, PropertySchemetwo_Words5Fixed, 8, 9, "two_words3"),
        DataRow(PropertySchemetwo_Words6, PropertySchemetwo_Words6Fixed, 8, 9, "TWO_WORDS3"),
        DataRow(PropertySchemetwo_Words7, PropertySchemetwo_Words7Fixed, 8, 9, "Two_Words3"),
        DataRow(PropertySchemeTwo_Words1, PropertySchemeTwo_Words1Fixed, 8, 9, "twowords3"),
        DataRow(PropertySchemeTwo_Words2, PropertySchemeTwo_Words2Fixed, 8, 9, "TWOWORDS3"),
        DataRow(PropertySchemeTwo_Words3, PropertySchemeTwo_Words3Fixed, 8, 9, "twoWords3"),
        DataRow(PropertySchemeTwo_Words4, PropertySchemeTwo_Words4Fixed, 8, 9, "TwoWords3"),
        DataRow(PropertySchemeTwo_Words5, PropertySchemeTwo_Words5Fixed, 8, 9, "two_words3"),
        DataRow(PropertySchemeTwo_Words6, PropertySchemeTwo_Words6Fixed, 8, 9, "TWO_WORDS3"),
        DataRow(PropertySchemeTwo_Words7, PropertySchemeTwo_Words7Fixed, 8, 9, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 8, 9, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1310MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1310)),
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
