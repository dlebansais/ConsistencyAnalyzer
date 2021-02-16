namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1308
    {
        private const string EventSchemetwowords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemetwowords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemetwowords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemetwowords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemetwowords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemetwowords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemetwowords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemetwowords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemetwowords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemetwowords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemetwowords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemetwowords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemeTWOWORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemeTWOWORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWOWORDS2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemeTWOWORDS2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWOWORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemeTWOWORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWOWORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemeTWOWORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWOWORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemeTWOWORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWOWORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemeTWOWORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWOWORDS1;
        public event System.EventHandler TWOWORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemetwoWords2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemetwoWords2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler tWOWORDS3;
    }
}
";

        private const string EventSchemetwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemetwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemetwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemetwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemetwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemetwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemetwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemetwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemetwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemetwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twoWords1;
        public event System.EventHandler twoWords2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemeTwoWords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemeTwoWords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler Twowords3;
    }
}
";

        private const string EventSchemeTwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemeTwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemeTwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemeTwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemeTwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemeTwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemeTwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemeTwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemeTwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemeTwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TwoWords1;
        public event System.EventHandler TwoWords2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemetwo_words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemetwo_words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemetwo_words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemetwo_words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemetwo_words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemetwo_words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemetwo_words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemetwo_words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemetwo_words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemetwo_words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemetwo_words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemetwo_words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_words1;
        public event System.EventHandler two_words2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemeTWO_WORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemeTWO_WORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWO_WORDS3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemeTWO_WORDS3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWO_WORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemeTWO_WORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTWO_WORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemeTWO_WORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemeTWO_WORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemeTWO_WORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemeTWO_WORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemeTWO_WORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler TWO_WORDS1;
        public event System.EventHandler TWO_WORDS2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemetwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemetwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemetwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemetwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemetwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemetwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemetwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemetwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemetwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemetwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemetwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemetwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler two_Words1;
        public event System.EventHandler two_Words2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemeTwo_Words1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler twowords3;
    }
}
";

        private const string EventSchemeTwo_Words1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler Twowords3;
    }
}
";

        private const string EventSchemeTwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler TWOWORDS3;
    }
}
";

        private const string EventSchemeTwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler Twowords3;
    }
}
";

        private const string EventSchemeTwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler twoWords3;
    }
}
";

        private const string EventSchemeTwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemeTwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler TwoWords3;
    }
}
";

        private const string EventSchemeTwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemeTwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler two_words3;
    }
}
";

        private const string EventSchemeTwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemeTwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler TWO_WORDS3;
    }
}
";

        private const string EventSchemeTwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string EventSchemeTwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler two_Words3;
    }
}
";

        private const string EventSchemeTwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler Two_Words1;
        public event System.EventHandler Two_Words2;
        public event System.EventHandler Two_Words3;
    }
}
";

        private const string Trivia1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler /* */TWOWORDS3/* */;
    }
}
";

        private const string Trivia1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public event System.EventHandler twowords1;
        public event System.EventHandler twowords2;
        public event System.EventHandler /* */twowords3/* */;
    }
}
";

        [DataTestMethod]
        [
        DataRow(EventSchemetwowords1, EventSchemetwowords1Fixed, 8, 42, "TWOWORDS3"),
        DataRow(EventSchemetwowords3, EventSchemetwowords3Fixed, 8, 42, "TwoWords3"),
        DataRow(EventSchemetwowords4, EventSchemetwowords4Fixed, 8, 42, "two_words3"),
        DataRow(EventSchemetwowords5, EventSchemetwowords5Fixed, 8, 42, "TWO_WORDS3"),
        DataRow(EventSchemetwowords6, EventSchemetwowords6Fixed, 8, 42, "two_Words3"),
        DataRow(EventSchemetwowords7, EventSchemetwowords7Fixed, 8, 42, "Two_Words3"),
        DataRow(EventSchemeTWOWORDS1, EventSchemeTWOWORDS1Fixed, 8, 42, "twowords3"),
        DataRow(EventSchemeTWOWORDS2, EventSchemeTWOWORDS2Fixed, 8, 42, "twoWords3"),
        DataRow(EventSchemeTWOWORDS4, EventSchemeTWOWORDS4Fixed, 8, 42, "two_words3"),
        DataRow(EventSchemeTWOWORDS5, EventSchemeTWOWORDS5Fixed, 8, 42, "TWO_WORDS3"),
        DataRow(EventSchemeTWOWORDS6, EventSchemeTWOWORDS6Fixed, 8, 42, "two_Words3"),
        DataRow(EventSchemeTWOWORDS7, EventSchemeTWOWORDS7Fixed, 8, 42, "Two_Words3"),
        DataRow(EventSchemetwoWords2, EventSchemetwoWords2Fixed, 8, 42, "TWOWORDS3"),
        DataRow(EventSchemetwoWords3, EventSchemetwoWords3Fixed, 8, 42, "TwoWords3"),
        DataRow(EventSchemetwoWords4, EventSchemetwoWords4Fixed, 8, 42, "two_words3"),
        DataRow(EventSchemetwoWords5, EventSchemetwoWords5Fixed, 8, 42, "TWO_WORDS3"),
        DataRow(EventSchemetwoWords6, EventSchemetwoWords6Fixed, 8, 42, "two_Words3"),
        DataRow(EventSchemetwoWords7, EventSchemetwoWords7Fixed, 8, 42, "Two_Words3"),
        DataRow(EventSchemeTwoWords1, EventSchemeTwoWords1Fixed, 8, 42, "twowords3"),
        DataRow(EventSchemeTwoWords3, EventSchemeTwoWords3Fixed, 8, 42, "twoWords3"),
        DataRow(EventSchemeTwoWords4, EventSchemeTwoWords4Fixed, 8, 42, "two_words3"),
        DataRow(EventSchemeTwoWords5, EventSchemeTwoWords5Fixed, 8, 42, "TWO_WORDS3"),
        DataRow(EventSchemeTwoWords6, EventSchemeTwoWords6Fixed, 8, 42, "two_Words3"),
        DataRow(EventSchemeTwoWords7, EventSchemeTwoWords7Fixed, 8, 42, "Two_Words3"),
        DataRow(EventSchemetwo_words2, EventSchemetwo_words2Fixed, 8, 42, "TWOWORDS3"),
        DataRow(EventSchemetwo_words3, EventSchemetwo_words3Fixed, 8, 42, "twoWords3"),
        DataRow(EventSchemetwo_words4, EventSchemetwo_words4Fixed, 8, 42, "TwoWords3"),
        DataRow(EventSchemetwo_words5, EventSchemetwo_words5Fixed, 8, 42, "TWO_WORDS3"),
        DataRow(EventSchemetwo_words6, EventSchemetwo_words6Fixed, 8, 42, "two_Words3"),
        DataRow(EventSchemetwo_words7, EventSchemetwo_words7Fixed, 8, 42, "Two_Words3"),
        DataRow(EventSchemeTWO_WORDS1, EventSchemeTWO_WORDS1Fixed, 8, 42, "twowords3"),
        DataRow(EventSchemeTWO_WORDS3, EventSchemeTWO_WORDS3Fixed, 8, 42, "twoWords3"),
        DataRow(EventSchemeTWO_WORDS4, EventSchemeTWO_WORDS4Fixed, 8, 42, "TwoWords3"),
        DataRow(EventSchemeTWO_WORDS5, EventSchemeTWO_WORDS5Fixed, 8, 42, "two_words3"),
        DataRow(EventSchemeTWO_WORDS6, EventSchemeTWO_WORDS6Fixed, 8, 42, "two_Words3"),
        DataRow(EventSchemeTWO_WORDS7, EventSchemeTWO_WORDS7Fixed, 8, 42, "Two_Words3"),
        DataRow(EventSchemetwo_Words2, EventSchemetwo_Words2Fixed, 8, 42, "TWOWORDS3"),
        DataRow(EventSchemetwo_Words3, EventSchemetwo_Words3Fixed, 8, 42, "twoWords3"),
        DataRow(EventSchemetwo_Words4, EventSchemetwo_Words4Fixed, 8, 42, "TwoWords3"),
        DataRow(EventSchemetwo_Words5, EventSchemetwo_Words5Fixed, 8, 42, "two_words3"),
        DataRow(EventSchemetwo_Words6, EventSchemetwo_Words6Fixed, 8, 42, "TWO_WORDS3"),
        DataRow(EventSchemetwo_Words7, EventSchemetwo_Words7Fixed, 8, 42, "Two_Words3"),
        DataRow(EventSchemeTwo_Words1, EventSchemeTwo_Words1Fixed, 8, 42, "twowords3"),
        DataRow(EventSchemeTwo_Words2, EventSchemeTwo_Words2Fixed, 8, 42, "TWOWORDS3"),
        DataRow(EventSchemeTwo_Words3, EventSchemeTwo_Words3Fixed, 8, 42, "twoWords3"),
        DataRow(EventSchemeTwo_Words4, EventSchemeTwo_Words4Fixed, 8, 42, "TwoWords3"),
        DataRow(EventSchemeTwo_Words5, EventSchemeTwo_Words5Fixed, 8, 42, "two_words3"),
        DataRow(EventSchemeTwo_Words6, EventSchemeTwo_Words6Fixed, 8, 42, "TWO_WORDS3"),
        DataRow(EventSchemeTwo_Words7, EventSchemeTwo_Words7Fixed, 8, 42, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 8, 47, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1308MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1308)),
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
