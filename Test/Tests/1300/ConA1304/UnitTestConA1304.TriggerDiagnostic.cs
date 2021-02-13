namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1304
    {
        private const string EnumSchemetwowords1 = @"
enum twowords1
{
}

enum twowords2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemetwowords1Fixed = @"
enum twowords1
{
}

enum twowords2
{
}

enum twowords3
{
}
";

        private const string EnumSchemetwowords3 = @"
enum twowords1
{
}

enum twowords2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemetwowords3Fixed = @"
enum twowords1
{
}

enum twowords2
{
}

enum twowords3
{
}
";

        private const string EnumSchemetwowords4 = @"
enum twowords1
{
}

enum twowords2
{
}

enum two_words3
{
}
";

        private const string EnumSchemetwowords4Fixed = @"
enum twowords1
{
}

enum twowords2
{
}

enum twowords3
{
}
";

        private const string EnumSchemetwowords5 = @"
enum twowords1
{
}

enum twowords2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemetwowords5Fixed = @"
enum twowords1
{
}

enum twowords2
{
}

enum twowords3
{
}
";

        private const string EnumSchemetwowords6 = @"
enum twowords1
{
}

enum twowords2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemetwowords6Fixed = @"
enum twowords1
{
}

enum twowords2
{
}

enum twowords3
{
}
";

        private const string EnumSchemetwowords7 = @"
enum twowords1
{
}

enum twowords2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemetwowords7Fixed = @"
enum twowords1
{
}

enum twowords2
{
}

enum twowords3
{
}
";

        private const string EnumSchemeTWOWORDS1 = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum twowords3
{
}
";

        private const string EnumSchemeTWOWORDS1Fixed = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWOWORDS2 = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemeTWOWORDS2Fixed = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWOWORDS4 = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum two_words3
{
}
";

        private const string EnumSchemeTWOWORDS4Fixed = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWOWORDS5 = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemeTWOWORDS5Fixed = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWOWORDS6 = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemeTWOWORDS6Fixed = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWOWORDS7 = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemeTWOWORDS7Fixed = @"
enum TWOWORDS1
{
}

enum TWOWORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemetwoWords2 = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemetwoWords2Fixed = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum tWOWORDS3
{
}
";

        private const string EnumSchemetwoWords3 = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemetwoWords3Fixed = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemetwoWords4 = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum two_words3
{
}
";

        private const string EnumSchemetwoWords4Fixed = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemetwoWords5 = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemetwoWords5Fixed = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemetwoWords6 = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemetwoWords6Fixed = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemetwoWords7 = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemetwoWords7Fixed = @"
enum twoWords1
{
}

enum twoWords2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemeTwoWords1 = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum twowords3
{
}
";

        private const string EnumSchemeTwoWords1Fixed = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum Twowords3
{
}
";

        private const string EnumSchemeTwoWords3 = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemeTwoWords3Fixed = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemeTwoWords4 = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum two_words3
{
}
";

        private const string EnumSchemeTwoWords4Fixed = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemeTwoWords5 = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemeTwoWords5Fixed = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemeTwoWords6 = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemeTwoWords6Fixed = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemeTwoWords7 = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemeTwoWords7Fixed = @"
enum TwoWords1
{
}

enum TwoWords2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemetwo_words2 = @"
enum two_words1
{
}

enum two_words2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemetwo_words2Fixed = @"
enum two_words1
{
}

enum two_words2
{
}

enum twowords3
{
}
";

        private const string EnumSchemetwo_words3 = @"
enum two_words1
{
}

enum two_words2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemetwo_words3Fixed = @"
enum two_words1
{
}

enum two_words2
{
}

enum two_words3
{
}
";

        private const string EnumSchemetwo_words4 = @"
enum two_words1
{
}

enum two_words2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemetwo_words4Fixed = @"
enum two_words1
{
}

enum two_words2
{
}

enum two_words3
{
}
";

        private const string EnumSchemetwo_words5 = @"
enum two_words1
{
}

enum two_words2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemetwo_words5Fixed = @"
enum two_words1
{
}

enum two_words2
{
}

enum two_words3
{
}
";

        private const string EnumSchemetwo_words6 = @"
enum two_words1
{
}

enum two_words2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemetwo_words6Fixed = @"
enum two_words1
{
}

enum two_words2
{
}

enum two_words3
{
}
";

        private const string EnumSchemetwo_words7 = @"
enum two_words1
{
}

enum two_words2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemetwo_words7Fixed = @"
enum two_words1
{
}

enum two_words2
{
}

enum two_words3
{
}
";

        private const string EnumSchemeTWO_WORDS1 = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum twowords3
{
}
";

        private const string EnumSchemeTWO_WORDS1Fixed = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWO_WORDS3 = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemeTWO_WORDS3Fixed = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWO_WORDS4 = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemeTWO_WORDS4Fixed = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTWO_WORDS5 = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum two_words3
{
}
";

        private const string EnumSchemeTWO_WORDS5Fixed = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemeTWO_WORDS6 = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemeTWO_WORDS6Fixed = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemeTWO_WORDS7 = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemeTWO_WORDS7Fixed = @"
enum TWO_WORDS1
{
}

enum TWO_WORDS2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemetwo_Words2 = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemetwo_Words2Fixed = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum twowords3
{
}
";

        private const string EnumSchemetwo_Words3 = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemetwo_Words3Fixed = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemetwo_Words4 = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemetwo_Words4Fixed = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemetwo_Words5 = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum two_words3
{
}
";

        private const string EnumSchemetwo_Words5Fixed = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemetwo_Words6 = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemetwo_Words6Fixed = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemetwo_Words7 = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemetwo_Words7Fixed = @"
enum two_Words1
{
}

enum two_Words2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemeTwo_Words1 = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum twowords3
{
}
";

        private const string EnumSchemeTwo_Words1Fixed = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum Twowords3
{
}
";

        private const string EnumSchemeTwo_Words2 = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum TWOWORDS3
{
}
";

        private const string EnumSchemeTwo_Words2Fixed = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum Twowords3
{
}
";

        private const string EnumSchemeTwo_Words3 = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum twoWords3
{
}
";

        private const string EnumSchemeTwo_Words3Fixed = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemeTwo_Words4 = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum TwoWords3
{
}
";

        private const string EnumSchemeTwo_Words4Fixed = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemeTwo_Words5 = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum two_words3
{
}
";

        private const string EnumSchemeTwo_Words5Fixed = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemeTwo_Words6 = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum TWO_WORDS3
{
}
";

        private const string EnumSchemeTwo_Words6Fixed = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum Two_Words3
{
}
";

        private const string EnumSchemeTwo_Words7 = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum two_Words3
{
}
";

        private const string EnumSchemeTwo_Words7Fixed = @"
enum Two_Words1
{
}

enum Two_Words2
{
}

enum Two_Words3
{
}
";

        private const string Trivia1 = @"
enum twowords1
{
}

enum twowords2
{
}

enum /* */TWOWORDS3/* */
{
}
";

        private const string Trivia1Fixed = @"
enum twowords1
{
}

enum twowords2
{
}

enum /* */twowords3/* */
{
}
";

        [DataTestMethod]
        [
        DataRow(EnumSchemetwowords1, EnumSchemetwowords1Fixed, 10, 1, "TWOWORDS3"),
        DataRow(EnumSchemetwowords3, EnumSchemetwowords3Fixed, 10, 1, "TwoWords3"),
        DataRow(EnumSchemetwowords4, EnumSchemetwowords4Fixed, 10, 1, "two_words3"),
        DataRow(EnumSchemetwowords5, EnumSchemetwowords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(EnumSchemetwowords6, EnumSchemetwowords6Fixed, 10, 1, "two_Words3"),
        DataRow(EnumSchemetwowords7, EnumSchemetwowords7Fixed, 10, 1, "Two_Words3"),
        DataRow(EnumSchemeTWOWORDS1, EnumSchemeTWOWORDS1Fixed, 10, 1, "twowords3"),
        DataRow(EnumSchemeTWOWORDS2, EnumSchemeTWOWORDS2Fixed, 10, 1, "twoWords3"),
        DataRow(EnumSchemeTWOWORDS4, EnumSchemeTWOWORDS4Fixed, 10, 1, "two_words3"),
        DataRow(EnumSchemeTWOWORDS5, EnumSchemeTWOWORDS5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(EnumSchemeTWOWORDS6, EnumSchemeTWOWORDS6Fixed, 10, 1, "two_Words3"),
        DataRow(EnumSchemeTWOWORDS7, EnumSchemeTWOWORDS7Fixed, 10, 1, "Two_Words3"),
        DataRow(EnumSchemetwoWords2, EnumSchemetwoWords2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(EnumSchemetwoWords3, EnumSchemetwoWords3Fixed, 10, 1, "TwoWords3"),
        DataRow(EnumSchemetwoWords4, EnumSchemetwoWords4Fixed, 10, 1, "two_words3"),
        DataRow(EnumSchemetwoWords5, EnumSchemetwoWords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(EnumSchemetwoWords6, EnumSchemetwoWords6Fixed, 10, 1, "two_Words3"),
        DataRow(EnumSchemetwoWords7, EnumSchemetwoWords7Fixed, 10, 1, "Two_Words3"),
        DataRow(EnumSchemeTwoWords1, EnumSchemeTwoWords1Fixed, 10, 1, "twowords3"),
        DataRow(EnumSchemeTwoWords3, EnumSchemeTwoWords3Fixed, 10, 1, "twoWords3"),
        DataRow(EnumSchemeTwoWords4, EnumSchemeTwoWords4Fixed, 10, 1, "two_words3"),
        DataRow(EnumSchemeTwoWords5, EnumSchemeTwoWords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(EnumSchemeTwoWords6, EnumSchemeTwoWords6Fixed, 10, 1, "two_Words3"),
        DataRow(EnumSchemeTwoWords7, EnumSchemeTwoWords7Fixed, 10, 1, "Two_Words3"),
        DataRow(EnumSchemetwo_words2, EnumSchemetwo_words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(EnumSchemetwo_words3, EnumSchemetwo_words3Fixed, 10, 1, "twoWords3"),
        DataRow(EnumSchemetwo_words4, EnumSchemetwo_words4Fixed, 10, 1, "TwoWords3"),
        DataRow(EnumSchemetwo_words5, EnumSchemetwo_words5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(EnumSchemetwo_words6, EnumSchemetwo_words6Fixed, 10, 1, "two_Words3"),
        DataRow(EnumSchemetwo_words7, EnumSchemetwo_words7Fixed, 10, 1, "Two_Words3"),
        DataRow(EnumSchemeTWO_WORDS1, EnumSchemeTWO_WORDS1Fixed, 10, 1, "twowords3"),
        DataRow(EnumSchemeTWO_WORDS3, EnumSchemeTWO_WORDS3Fixed, 10, 1, "twoWords3"),
        DataRow(EnumSchemeTWO_WORDS4, EnumSchemeTWO_WORDS4Fixed, 10, 1, "TwoWords3"),
        DataRow(EnumSchemeTWO_WORDS5, EnumSchemeTWO_WORDS5Fixed, 10, 1, "two_words3"),
        DataRow(EnumSchemeTWO_WORDS6, EnumSchemeTWO_WORDS6Fixed, 10, 1, "two_Words3"),
        DataRow(EnumSchemeTWO_WORDS7, EnumSchemeTWO_WORDS7Fixed, 10, 1, "Two_Words3"),
        DataRow(EnumSchemetwo_Words2, EnumSchemetwo_Words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(EnumSchemetwo_Words3, EnumSchemetwo_Words3Fixed, 10, 1, "twoWords3"),
        DataRow(EnumSchemetwo_Words4, EnumSchemetwo_Words4Fixed, 10, 1, "TwoWords3"),
        DataRow(EnumSchemetwo_Words5, EnumSchemetwo_Words5Fixed, 10, 1, "two_words3"),
        DataRow(EnumSchemetwo_Words6, EnumSchemetwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(EnumSchemetwo_Words7, EnumSchemetwo_Words7Fixed, 10, 1, "Two_Words3"),
        DataRow(EnumSchemeTwo_Words1, EnumSchemeTwo_Words1Fixed, 10, 1, "twowords3"),
        DataRow(EnumSchemeTwo_Words2, EnumSchemeTwo_Words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(EnumSchemeTwo_Words3, EnumSchemeTwo_Words3Fixed, 10, 1, "twoWords3"),
        DataRow(EnumSchemeTwo_Words4, EnumSchemeTwo_Words4Fixed, 10, 1, "TwoWords3"),
        DataRow(EnumSchemeTwo_Words5, EnumSchemeTwo_Words5Fixed, 10, 1, "two_words3"),
        DataRow(EnumSchemeTwo_Words6, EnumSchemeTwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(EnumSchemeTwo_Words7, EnumSchemeTwo_Words7Fixed, 10, 1, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 10, 1, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1304MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1304)),
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
