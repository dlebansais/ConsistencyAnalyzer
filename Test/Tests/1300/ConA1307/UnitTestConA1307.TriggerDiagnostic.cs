namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1307
    {
        private const string DelegateSchemetwowords1 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemetwowords1Fixed = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void twowords3();
}
";

        private const string DelegateSchemetwowords3 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemetwowords3Fixed = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void twowords3();
}
";

        private const string DelegateSchemetwowords4 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void two_words3();
}
";

        private const string DelegateSchemetwowords4Fixed = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void twowords3();
}
";

        private const string DelegateSchemetwowords5 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemetwowords5Fixed = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void twowords3();
}
";

        private const string DelegateSchemetwowords6 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemetwowords6Fixed = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void twowords3();
}
";

        private const string DelegateSchemetwowords7 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemetwowords7Fixed = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void twowords3();
}
";

        private const string DelegateSchemeTWOWORDS1 = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void twowords3();
}
";

        private const string DelegateSchemeTWOWORDS1Fixed = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWOWORDS2 = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemeTWOWORDS2Fixed = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWOWORDS4 = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void two_words3();
}
";

        private const string DelegateSchemeTWOWORDS4Fixed = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWOWORDS5 = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemeTWOWORDS5Fixed = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWOWORDS6 = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemeTWOWORDS6Fixed = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWOWORDS7 = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemeTWOWORDS7Fixed = @"
namespace Test
{
    delegate void TWOWORDS1();
    delegate void TWOWORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemetwoWords2 = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemetwoWords2Fixed = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void tWOWORDS3();
}
";

        private const string DelegateSchemetwoWords3 = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemetwoWords3Fixed = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemetwoWords4 = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void two_words3();
}
";

        private const string DelegateSchemetwoWords4Fixed = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemetwoWords5 = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemetwoWords5Fixed = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemetwoWords6 = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemetwoWords6Fixed = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemetwoWords7 = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemetwoWords7Fixed = @"
namespace Test
{
    delegate void twoWords1();
    delegate void twoWords2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemeTwoWords1 = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void twowords3();
}
";

        private const string DelegateSchemeTwoWords1Fixed = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void Twowords3();
}
";

        private const string DelegateSchemeTwoWords3 = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemeTwoWords3Fixed = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemeTwoWords4 = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void two_words3();
}
";

        private const string DelegateSchemeTwoWords4Fixed = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemeTwoWords5 = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemeTwoWords5Fixed = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemeTwoWords6 = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemeTwoWords6Fixed = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemeTwoWords7 = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemeTwoWords7Fixed = @"
namespace Test
{
    delegate void TwoWords1();
    delegate void TwoWords2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemetwo_words2 = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemetwo_words2Fixed = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void twowords3();
}
";

        private const string DelegateSchemetwo_words3 = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemetwo_words3Fixed = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void two_words3();
}
";

        private const string DelegateSchemetwo_words4 = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemetwo_words4Fixed = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void two_words3();
}
";

        private const string DelegateSchemetwo_words5 = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemetwo_words5Fixed = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void two_words3();
}
";

        private const string DelegateSchemetwo_words6 = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemetwo_words6Fixed = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void two_words3();
}
";

        private const string DelegateSchemetwo_words7 = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemetwo_words7Fixed = @"
namespace Test
{
    delegate void two_words1();
    delegate void two_words2();
    delegate void two_words3();
}
";

        private const string DelegateSchemeTWO_WORDS1 = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void twowords3();
}
";

        private const string DelegateSchemeTWO_WORDS1Fixed = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWO_WORDS3 = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemeTWO_WORDS3Fixed = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWO_WORDS4 = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemeTWO_WORDS4Fixed = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTWO_WORDS5 = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void two_words3();
}
";

        private const string DelegateSchemeTWO_WORDS5Fixed = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemeTWO_WORDS6 = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemeTWO_WORDS6Fixed = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemeTWO_WORDS7 = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemeTWO_WORDS7Fixed = @"
namespace Test
{
    delegate void TWO_WORDS1();
    delegate void TWO_WORDS2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemetwo_Words2 = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemetwo_Words2Fixed = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void twowords3();
}
";

        private const string DelegateSchemetwo_Words3 = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemetwo_Words3Fixed = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemetwo_Words4 = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemetwo_Words4Fixed = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemetwo_Words5 = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void two_words3();
}
";

        private const string DelegateSchemetwo_Words5Fixed = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemetwo_Words6 = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemetwo_Words6Fixed = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemetwo_Words7 = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemetwo_Words7Fixed = @"
namespace Test
{
    delegate void two_Words1();
    delegate void two_Words2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemeTwo_Words1 = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void twowords3();
}
";

        private const string DelegateSchemeTwo_Words1Fixed = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void Twowords3();
}
";

        private const string DelegateSchemeTwo_Words2 = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void TWOWORDS3();
}
";

        private const string DelegateSchemeTwo_Words2Fixed = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void Twowords3();
}
";

        private const string DelegateSchemeTwo_Words3 = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void twoWords3();
}
";

        private const string DelegateSchemeTwo_Words3Fixed = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemeTwo_Words4 = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void TwoWords3();
}
";

        private const string DelegateSchemeTwo_Words4Fixed = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemeTwo_Words5 = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void two_words3();
}
";

        private const string DelegateSchemeTwo_Words5Fixed = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemeTwo_Words6 = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void TWO_WORDS3();
}
";

        private const string DelegateSchemeTwo_Words6Fixed = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void Two_Words3();
}
";

        private const string DelegateSchemeTwo_Words7 = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void two_Words3();
}
";

        private const string DelegateSchemeTwo_Words7Fixed = @"
namespace Test
{
    delegate void Two_Words1();
    delegate void Two_Words2();
    delegate void Two_Words3();
}
";

        private const string Trivia1 = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void /* */TWOWORDS3/* */();
}
";

        private const string Trivia1Fixed = @"
namespace Test
{
    delegate void twowords1();
    delegate void twowords2();
    delegate void /* */twowords3/* */();
}
";

        [DataTestMethod]
        [
        DataRow(DelegateSchemetwowords1, DelegateSchemetwowords1Fixed, 6, 5, "TWOWORDS3"),
        DataRow(DelegateSchemetwowords3, DelegateSchemetwowords3Fixed, 6, 5, "TwoWords3"),
        DataRow(DelegateSchemetwowords4, DelegateSchemetwowords4Fixed, 6, 5, "two_words3"),
        DataRow(DelegateSchemetwowords5, DelegateSchemetwowords5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(DelegateSchemetwowords6, DelegateSchemetwowords6Fixed, 6, 5, "two_Words3"),
        DataRow(DelegateSchemetwowords7, DelegateSchemetwowords7Fixed, 6, 5, "Two_Words3"),
        DataRow(DelegateSchemeTWOWORDS1, DelegateSchemeTWOWORDS1Fixed, 6, 5, "twowords3"),
        DataRow(DelegateSchemeTWOWORDS2, DelegateSchemeTWOWORDS2Fixed, 6, 5, "twoWords3"),
        DataRow(DelegateSchemeTWOWORDS4, DelegateSchemeTWOWORDS4Fixed, 6, 5, "two_words3"),
        DataRow(DelegateSchemeTWOWORDS5, DelegateSchemeTWOWORDS5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(DelegateSchemeTWOWORDS6, DelegateSchemeTWOWORDS6Fixed, 6, 5, "two_Words3"),
        DataRow(DelegateSchemeTWOWORDS7, DelegateSchemeTWOWORDS7Fixed, 6, 5, "Two_Words3"),
        DataRow(DelegateSchemetwoWords2, DelegateSchemetwoWords2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(DelegateSchemetwoWords3, DelegateSchemetwoWords3Fixed, 6, 5, "TwoWords3"),
        DataRow(DelegateSchemetwoWords4, DelegateSchemetwoWords4Fixed, 6, 5, "two_words3"),
        DataRow(DelegateSchemetwoWords5, DelegateSchemetwoWords5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(DelegateSchemetwoWords6, DelegateSchemetwoWords6Fixed, 6, 5, "two_Words3"),
        DataRow(DelegateSchemetwoWords7, DelegateSchemetwoWords7Fixed, 6, 5, "Two_Words3"),
        DataRow(DelegateSchemeTwoWords1, DelegateSchemeTwoWords1Fixed, 6, 5, "twowords3"),
        DataRow(DelegateSchemeTwoWords3, DelegateSchemeTwoWords3Fixed, 6, 5, "twoWords3"),
        DataRow(DelegateSchemeTwoWords4, DelegateSchemeTwoWords4Fixed, 6, 5, "two_words3"),
        DataRow(DelegateSchemeTwoWords5, DelegateSchemeTwoWords5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(DelegateSchemeTwoWords6, DelegateSchemeTwoWords6Fixed, 6, 5, "two_Words3"),
        DataRow(DelegateSchemeTwoWords7, DelegateSchemeTwoWords7Fixed, 6, 5, "Two_Words3"),
        DataRow(DelegateSchemetwo_words2, DelegateSchemetwo_words2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(DelegateSchemetwo_words3, DelegateSchemetwo_words3Fixed, 6, 5, "twoWords3"),
        DataRow(DelegateSchemetwo_words4, DelegateSchemetwo_words4Fixed, 6, 5, "TwoWords3"),
        DataRow(DelegateSchemetwo_words5, DelegateSchemetwo_words5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(DelegateSchemetwo_words6, DelegateSchemetwo_words6Fixed, 6, 5, "two_Words3"),
        DataRow(DelegateSchemetwo_words7, DelegateSchemetwo_words7Fixed, 6, 5, "Two_Words3"),
        DataRow(DelegateSchemeTWO_WORDS1, DelegateSchemeTWO_WORDS1Fixed, 6, 5, "twowords3"),
        DataRow(DelegateSchemeTWO_WORDS3, DelegateSchemeTWO_WORDS3Fixed, 6, 5, "twoWords3"),
        DataRow(DelegateSchemeTWO_WORDS4, DelegateSchemeTWO_WORDS4Fixed, 6, 5, "TwoWords3"),
        DataRow(DelegateSchemeTWO_WORDS5, DelegateSchemeTWO_WORDS5Fixed, 6, 5, "two_words3"),
        DataRow(DelegateSchemeTWO_WORDS6, DelegateSchemeTWO_WORDS6Fixed, 6, 5, "two_Words3"),
        DataRow(DelegateSchemeTWO_WORDS7, DelegateSchemeTWO_WORDS7Fixed, 6, 5, "Two_Words3"),
        DataRow(DelegateSchemetwo_Words2, DelegateSchemetwo_Words2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(DelegateSchemetwo_Words3, DelegateSchemetwo_Words3Fixed, 6, 5, "twoWords3"),
        DataRow(DelegateSchemetwo_Words4, DelegateSchemetwo_Words4Fixed, 6, 5, "TwoWords3"),
        DataRow(DelegateSchemetwo_Words5, DelegateSchemetwo_Words5Fixed, 6, 5, "two_words3"),
        DataRow(DelegateSchemetwo_Words6, DelegateSchemetwo_Words6Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(DelegateSchemetwo_Words7, DelegateSchemetwo_Words7Fixed, 6, 5, "Two_Words3"),
        DataRow(DelegateSchemeTwo_Words1, DelegateSchemeTwo_Words1Fixed, 6, 5, "twowords3"),
        DataRow(DelegateSchemeTwo_Words2, DelegateSchemeTwo_Words2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(DelegateSchemeTwo_Words3, DelegateSchemeTwo_Words3Fixed, 6, 5, "twoWords3"),
        DataRow(DelegateSchemeTwo_Words4, DelegateSchemeTwo_Words4Fixed, 6, 5, "TwoWords3"),
        DataRow(DelegateSchemeTwo_Words5, DelegateSchemeTwo_Words5Fixed, 6, 5, "two_words3"),
        DataRow(DelegateSchemeTwo_Words6, DelegateSchemeTwo_Words6Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(DelegateSchemeTwo_Words7, DelegateSchemeTwo_Words7Fixed, 6, 5, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 6, 5, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1307MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1307)),
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
