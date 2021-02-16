namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1306
    {
        private const string EnumMemberSchemetwowords1 = @"
enum Test
{
    twowords1,
    twowords2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemetwowords1Fixed = @"
enum Test
{
    twowords1,
    twowords2,
    twowords3,
}
";

        private const string EnumMemberSchemetwowords3 = @"
enum Test
{
    twowords1,
    twowords2,
    TwoWords3,
}
";

        private const string EnumMemberSchemetwowords3Fixed = @"
enum Test
{
    twowords1,
    twowords2,
    twowords3,
}
";

        private const string EnumMemberSchemetwowords4 = @"
enum Test
{
    twowords1,
    twowords2,
    two_words3,
}
";

        private const string EnumMemberSchemetwowords4Fixed = @"
enum Test
{
    twowords1,
    twowords2,
    twowords3,
}
";

        private const string EnumMemberSchemetwowords5 = @"
enum Test
{
    twowords1,
    twowords2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemetwowords5Fixed = @"
enum Test
{
    twowords1,
    twowords2,
    twowords3,
}
";

        private const string EnumMemberSchemetwowords6 = @"
enum Test
{
    twowords1,
    twowords2,
    two_Words3,
}
";

        private const string EnumMemberSchemetwowords6Fixed = @"
enum Test
{
    twowords1,
    twowords2,
    twowords3,
}
";

        private const string EnumMemberSchemetwowords7 = @"
enum Test
{
    twowords1,
    twowords2,
    Two_Words3,
}
";

        private const string EnumMemberSchemetwowords7Fixed = @"
enum Test
{
    twowords1,
    twowords2,
    twowords3,
}
";

        private const string EnumMemberSchemeTWOWORDS1 = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    twowords3,
}
";

        private const string EnumMemberSchemeTWOWORDS1Fixed = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWOWORDS2 = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    twoWords3,
}
";

        private const string EnumMemberSchemeTWOWORDS2Fixed = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWOWORDS4 = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    two_words3,
}
";

        private const string EnumMemberSchemeTWOWORDS4Fixed = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWOWORDS5 = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemeTWOWORDS5Fixed = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWOWORDS6 = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    two_Words3,
}
";

        private const string EnumMemberSchemeTWOWORDS6Fixed = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWOWORDS7 = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    Two_Words3,
}
";

        private const string EnumMemberSchemeTWOWORDS7Fixed = @"
enum Test
{
    TWOWORDS1,
    TWOWORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemetwoWords2 = @"
enum Test
{
    twoWords1,
    twoWords2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemetwoWords2Fixed = @"
enum Test
{
    twoWords1,
    twoWords2,
    tWOWORDS3,
}
";

        private const string EnumMemberSchemetwoWords3 = @"
enum Test
{
    twoWords1,
    twoWords2,
    TwoWords3,
}
";

        private const string EnumMemberSchemetwoWords3Fixed = @"
enum Test
{
    twoWords1,
    twoWords2,
    twoWords3,
}
";

        private const string EnumMemberSchemetwoWords4 = @"
enum Test
{
    twoWords1,
    twoWords2,
    two_words3,
}
";

        private const string EnumMemberSchemetwoWords4Fixed = @"
enum Test
{
    twoWords1,
    twoWords2,
    twoWords3,
}
";

        private const string EnumMemberSchemetwoWords5 = @"
enum Test
{
    twoWords1,
    twoWords2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemetwoWords5Fixed = @"
enum Test
{
    twoWords1,
    twoWords2,
    twoWords3,
}
";

        private const string EnumMemberSchemetwoWords6 = @"
enum Test
{
    twoWords1,
    twoWords2,
    two_Words3,
}
";

        private const string EnumMemberSchemetwoWords6Fixed = @"
enum Test
{
    twoWords1,
    twoWords2,
    twoWords3,
}
";

        private const string EnumMemberSchemetwoWords7 = @"
enum Test
{
    twoWords1,
    twoWords2,
    Two_Words3,
}
";

        private const string EnumMemberSchemetwoWords7Fixed = @"
enum Test
{
    twoWords1,
    twoWords2,
    twoWords3,
}
";

        private const string EnumMemberSchemeTwoWords1 = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    twowords3,
}
";

        private const string EnumMemberSchemeTwoWords1Fixed = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    Twowords3,
}
";

        private const string EnumMemberSchemeTwoWords3 = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    twoWords3,
}
";

        private const string EnumMemberSchemeTwoWords3Fixed = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    TwoWords3,
}
";

        private const string EnumMemberSchemeTwoWords4 = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    two_words3,
}
";

        private const string EnumMemberSchemeTwoWords4Fixed = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    TwoWords3,
}
";

        private const string EnumMemberSchemeTwoWords5 = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemeTwoWords5Fixed = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    TwoWords3,
}
";

        private const string EnumMemberSchemeTwoWords6 = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    two_Words3,
}
";

        private const string EnumMemberSchemeTwoWords6Fixed = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    TwoWords3,
}
";

        private const string EnumMemberSchemeTwoWords7 = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    Two_Words3,
}
";

        private const string EnumMemberSchemeTwoWords7Fixed = @"
enum Test
{
    TwoWords1,
    TwoWords2,
    TwoWords3,
}
";

        private const string EnumMemberSchemetwo_words2 = @"
enum Test
{
    two_words1,
    two_words2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemetwo_words2Fixed = @"
enum Test
{
    two_words1,
    two_words2,
    twowords3,
}
";

        private const string EnumMemberSchemetwo_words3 = @"
enum Test
{
    two_words1,
    two_words2,
    twoWords3,
}
";

        private const string EnumMemberSchemetwo_words3Fixed = @"
enum Test
{
    two_words1,
    two_words2,
    two_words3,
}
";

        private const string EnumMemberSchemetwo_words4 = @"
enum Test
{
    two_words1,
    two_words2,
    TwoWords3,
}
";

        private const string EnumMemberSchemetwo_words4Fixed = @"
enum Test
{
    two_words1,
    two_words2,
    two_words3,
}
";

        private const string EnumMemberSchemetwo_words5 = @"
enum Test
{
    two_words1,
    two_words2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemetwo_words5Fixed = @"
enum Test
{
    two_words1,
    two_words2,
    two_words3,
}
";

        private const string EnumMemberSchemetwo_words6 = @"
enum Test
{
    two_words1,
    two_words2,
    two_Words3,
}
";

        private const string EnumMemberSchemetwo_words6Fixed = @"
enum Test
{
    two_words1,
    two_words2,
    two_words3,
}
";

        private const string EnumMemberSchemetwo_words7 = @"
enum Test
{
    two_words1,
    two_words2,
    Two_Words3,
}
";

        private const string EnumMemberSchemetwo_words7Fixed = @"
enum Test
{
    two_words1,
    two_words2,
    two_words3,
}
";

        private const string EnumMemberSchemeTWO_WORDS1 = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    twowords3,
}
";

        private const string EnumMemberSchemeTWO_WORDS1Fixed = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWO_WORDS3 = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    twoWords3,
}
";

        private const string EnumMemberSchemeTWO_WORDS3Fixed = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWO_WORDS4 = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TwoWords3,
}
";

        private const string EnumMemberSchemeTWO_WORDS4Fixed = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTWO_WORDS5 = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    two_words3,
}
";

        private const string EnumMemberSchemeTWO_WORDS5Fixed = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemeTWO_WORDS6 = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    two_Words3,
}
";

        private const string EnumMemberSchemeTWO_WORDS6Fixed = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemeTWO_WORDS7 = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    Two_Words3,
}
";

        private const string EnumMemberSchemeTWO_WORDS7Fixed = @"
enum Test
{
    TWO_WORDS1,
    TWO_WORDS2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemetwo_Words2 = @"
enum Test
{
    two_Words1,
    two_Words2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemetwo_Words2Fixed = @"
enum Test
{
    two_Words1,
    two_Words2,
    twowords3,
}
";

        private const string EnumMemberSchemetwo_Words3 = @"
enum Test
{
    two_Words1,
    two_Words2,
    twoWords3,
}
";

        private const string EnumMemberSchemetwo_Words3Fixed = @"
enum Test
{
    two_Words1,
    two_Words2,
    two_Words3,
}
";

        private const string EnumMemberSchemetwo_Words4 = @"
enum Test
{
    two_Words1,
    two_Words2,
    TwoWords3,
}
";

        private const string EnumMemberSchemetwo_Words4Fixed = @"
enum Test
{
    two_Words1,
    two_Words2,
    two_Words3,
}
";

        private const string EnumMemberSchemetwo_Words5 = @"
enum Test
{
    two_Words1,
    two_Words2,
    two_words3,
}
";

        private const string EnumMemberSchemetwo_Words5Fixed = @"
enum Test
{
    two_Words1,
    two_Words2,
    two_Words3,
}
";

        private const string EnumMemberSchemetwo_Words6 = @"
enum Test
{
    two_Words1,
    two_Words2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemetwo_Words6Fixed = @"
enum Test
{
    two_Words1,
    two_Words2,
    two_Words3,
}
";

        private const string EnumMemberSchemetwo_Words7 = @"
enum Test
{
    two_Words1,
    two_Words2,
    Two_Words3,
}
";

        private const string EnumMemberSchemetwo_Words7Fixed = @"
enum Test
{
    two_Words1,
    two_Words2,
    two_Words3,
}
";

        private const string EnumMemberSchemeTwo_Words1 = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    twowords3,
}
";

        private const string EnumMemberSchemeTwo_Words1Fixed = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    Twowords3,
}
";

        private const string EnumMemberSchemeTwo_Words2 = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    TWOWORDS3,
}
";

        private const string EnumMemberSchemeTwo_Words2Fixed = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    Twowords3,
}
";

        private const string EnumMemberSchemeTwo_Words3 = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    twoWords3,
}
";

        private const string EnumMemberSchemeTwo_Words3Fixed = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    Two_Words3,
}
";

        private const string EnumMemberSchemeTwo_Words4 = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    TwoWords3,
}
";

        private const string EnumMemberSchemeTwo_Words4Fixed = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    Two_Words3,
}
";

        private const string EnumMemberSchemeTwo_Words5 = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    two_words3,
}
";

        private const string EnumMemberSchemeTwo_Words5Fixed = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    Two_Words3,
}
";

        private const string EnumMemberSchemeTwo_Words6 = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    TWO_WORDS3,
}
";

        private const string EnumMemberSchemeTwo_Words6Fixed = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    Two_Words3,
}
";

        private const string EnumMemberSchemeTwo_Words7 = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    two_Words3,
}
";

        private const string EnumMemberSchemeTwo_Words7Fixed = @"
enum Test
{
    Two_Words1,
    Two_Words2,
    Two_Words3,
}
";

        private const string Trivia1 = @"
enum Test
{
    twowords1,
    twowords2,
    /* */TWOWORDS3/* */,
}
";

        private const string Trivia1Fixed = @"
enum Test
{
    twowords1,
    twowords2,
    /* */twowords3/* */,
}
";

        [DataTestMethod]
        [
        DataRow(EnumMemberSchemetwowords1, EnumMemberSchemetwowords1Fixed, 6, 5, "TWOWORDS3"),
        DataRow(EnumMemberSchemetwowords3, EnumMemberSchemetwowords3Fixed, 6, 5, "TwoWords3"),
        DataRow(EnumMemberSchemetwowords4, EnumMemberSchemetwowords4Fixed, 6, 5, "two_words3"),
        DataRow(EnumMemberSchemetwowords5, EnumMemberSchemetwowords5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(EnumMemberSchemetwowords6, EnumMemberSchemetwowords6Fixed, 6, 5, "two_Words3"),
        DataRow(EnumMemberSchemetwowords7, EnumMemberSchemetwowords7Fixed, 6, 5, "Two_Words3"),
        DataRow(EnumMemberSchemeTWOWORDS1, EnumMemberSchemeTWOWORDS1Fixed, 6, 5, "twowords3"),
        DataRow(EnumMemberSchemeTWOWORDS2, EnumMemberSchemeTWOWORDS2Fixed, 6, 5, "twoWords3"),
        DataRow(EnumMemberSchemeTWOWORDS4, EnumMemberSchemeTWOWORDS4Fixed, 6, 5, "two_words3"),
        DataRow(EnumMemberSchemeTWOWORDS5, EnumMemberSchemeTWOWORDS5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(EnumMemberSchemeTWOWORDS6, EnumMemberSchemeTWOWORDS6Fixed, 6, 5, "two_Words3"),
        DataRow(EnumMemberSchemeTWOWORDS7, EnumMemberSchemeTWOWORDS7Fixed, 6, 5, "Two_Words3"),
        DataRow(EnumMemberSchemetwoWords2, EnumMemberSchemetwoWords2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(EnumMemberSchemetwoWords3, EnumMemberSchemetwoWords3Fixed, 6, 5, "TwoWords3"),
        DataRow(EnumMemberSchemetwoWords4, EnumMemberSchemetwoWords4Fixed, 6, 5, "two_words3"),
        DataRow(EnumMemberSchemetwoWords5, EnumMemberSchemetwoWords5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(EnumMemberSchemetwoWords6, EnumMemberSchemetwoWords6Fixed, 6, 5, "two_Words3"),
        DataRow(EnumMemberSchemetwoWords7, EnumMemberSchemetwoWords7Fixed, 6, 5, "Two_Words3"),
        DataRow(EnumMemberSchemeTwoWords1, EnumMemberSchemeTwoWords1Fixed, 6, 5, "twowords3"),
        DataRow(EnumMemberSchemeTwoWords3, EnumMemberSchemeTwoWords3Fixed, 6, 5, "twoWords3"),
        DataRow(EnumMemberSchemeTwoWords4, EnumMemberSchemeTwoWords4Fixed, 6, 5, "two_words3"),
        DataRow(EnumMemberSchemeTwoWords5, EnumMemberSchemeTwoWords5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(EnumMemberSchemeTwoWords6, EnumMemberSchemeTwoWords6Fixed, 6, 5, "two_Words3"),
        DataRow(EnumMemberSchemeTwoWords7, EnumMemberSchemeTwoWords7Fixed, 6, 5, "Two_Words3"),
        DataRow(EnumMemberSchemetwo_words2, EnumMemberSchemetwo_words2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(EnumMemberSchemetwo_words3, EnumMemberSchemetwo_words3Fixed, 6, 5, "twoWords3"),
        DataRow(EnumMemberSchemetwo_words4, EnumMemberSchemetwo_words4Fixed, 6, 5, "TwoWords3"),
        DataRow(EnumMemberSchemetwo_words5, EnumMemberSchemetwo_words5Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(EnumMemberSchemetwo_words6, EnumMemberSchemetwo_words6Fixed, 6, 5, "two_Words3"),
        DataRow(EnumMemberSchemetwo_words7, EnumMemberSchemetwo_words7Fixed, 6, 5, "Two_Words3"),
        DataRow(EnumMemberSchemeTWO_WORDS1, EnumMemberSchemeTWO_WORDS1Fixed, 6, 5, "twowords3"),
        DataRow(EnumMemberSchemeTWO_WORDS3, EnumMemberSchemeTWO_WORDS3Fixed, 6, 5, "twoWords3"),
        DataRow(EnumMemberSchemeTWO_WORDS4, EnumMemberSchemeTWO_WORDS4Fixed, 6, 5, "TwoWords3"),
        DataRow(EnumMemberSchemeTWO_WORDS5, EnumMemberSchemeTWO_WORDS5Fixed, 6, 5, "two_words3"),
        DataRow(EnumMemberSchemeTWO_WORDS6, EnumMemberSchemeTWO_WORDS6Fixed, 6, 5, "two_Words3"),
        DataRow(EnumMemberSchemeTWO_WORDS7, EnumMemberSchemeTWO_WORDS7Fixed, 6, 5, "Two_Words3"),
        DataRow(EnumMemberSchemetwo_Words2, EnumMemberSchemetwo_Words2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(EnumMemberSchemetwo_Words3, EnumMemberSchemetwo_Words3Fixed, 6, 5, "twoWords3"),
        DataRow(EnumMemberSchemetwo_Words4, EnumMemberSchemetwo_Words4Fixed, 6, 5, "TwoWords3"),
        DataRow(EnumMemberSchemetwo_Words5, EnumMemberSchemetwo_Words5Fixed, 6, 5, "two_words3"),
        DataRow(EnumMemberSchemetwo_Words6, EnumMemberSchemetwo_Words6Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(EnumMemberSchemetwo_Words7, EnumMemberSchemetwo_Words7Fixed, 6, 5, "Two_Words3"),
        DataRow(EnumMemberSchemeTwo_Words1, EnumMemberSchemeTwo_Words1Fixed, 6, 5, "twowords3"),
        DataRow(EnumMemberSchemeTwo_Words2, EnumMemberSchemeTwo_Words2Fixed, 6, 5, "TWOWORDS3"),
        DataRow(EnumMemberSchemeTwo_Words3, EnumMemberSchemeTwo_Words3Fixed, 6, 5, "twoWords3"),
        DataRow(EnumMemberSchemeTwo_Words4, EnumMemberSchemeTwo_Words4Fixed, 6, 5, "TwoWords3"),
        DataRow(EnumMemberSchemeTwo_Words5, EnumMemberSchemeTwo_Words5Fixed, 6, 5, "two_words3"),
        DataRow(EnumMemberSchemeTwo_Words6, EnumMemberSchemeTwo_Words6Fixed, 6, 5, "TWO_WORDS3"),
        DataRow(EnumMemberSchemeTwo_Words7, EnumMemberSchemeTwo_Words7Fixed, 6, 5, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 6, 10, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1306MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1306)),
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
