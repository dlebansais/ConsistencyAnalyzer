namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1303
    {
        private const string StructSchemetwowords1 = @"
struct twowords1
{
}

struct twowords2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemetwowords1Fixed = @"
struct twowords1
{
}

struct twowords2
{
}

struct twowords3
{
}
";

        private const string StructSchemetwowords3 = @"
struct twowords1
{
}

struct twowords2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemetwowords3Fixed = @"
struct twowords1
{
}

struct twowords2
{
}

struct twowords3
{
}
";

        private const string StructSchemetwowords4 = @"
struct twowords1
{
}

struct twowords2
{
}

struct two_words3
{
}
";

        private const string StructSchemetwowords4Fixed = @"
struct twowords1
{
}

struct twowords2
{
}

struct twowords3
{
}
";

        private const string StructSchemetwowords5 = @"
struct twowords1
{
}

struct twowords2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemetwowords5Fixed = @"
struct twowords1
{
}

struct twowords2
{
}

struct twowords3
{
}
";

        private const string StructSchemetwowords6 = @"
struct twowords1
{
}

struct twowords2
{
}

struct two_Words3
{
}
";

        private const string StructSchemetwowords6Fixed = @"
struct twowords1
{
}

struct twowords2
{
}

struct twowords3
{
}
";

        private const string StructSchemetwowords7 = @"
struct twowords1
{
}

struct twowords2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemetwowords7Fixed = @"
struct twowords1
{
}

struct twowords2
{
}

struct twowords3
{
}
";

        private const string StructSchemeTWOWORDS1 = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct twowords3
{
}
";

        private const string StructSchemeTWOWORDS1Fixed = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWOWORDS2 = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct twoWords3
{
}
";

        private const string StructSchemeTWOWORDS2Fixed = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWOWORDS4 = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct two_words3
{
}
";

        private const string StructSchemeTWOWORDS4Fixed = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWOWORDS5 = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemeTWOWORDS5Fixed = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWOWORDS6 = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct two_Words3
{
}
";

        private const string StructSchemeTWOWORDS6Fixed = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWOWORDS7 = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemeTWOWORDS7Fixed = @"
struct TWOWORDS1
{
}

struct TWOWORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemetwoWords2 = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemetwoWords2Fixed = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct tWOWORDS3
{
}
";

        private const string StructSchemetwoWords3 = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemetwoWords3Fixed = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct twoWords3
{
}
";

        private const string StructSchemetwoWords4 = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct two_words3
{
}
";

        private const string StructSchemetwoWords4Fixed = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct twoWords3
{
}
";

        private const string StructSchemetwoWords5 = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemetwoWords5Fixed = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct twoWords3
{
}
";

        private const string StructSchemetwoWords6 = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct two_Words3
{
}
";

        private const string StructSchemetwoWords6Fixed = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct twoWords3
{
}
";

        private const string StructSchemetwoWords7 = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemetwoWords7Fixed = @"
struct twoWords1
{
}

struct twoWords2
{
}

struct twoWords3
{
}
";

        private const string StructSchemeTwoWords1 = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct twowords3
{
}
";

        private const string StructSchemeTwoWords1Fixed = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct Twowords3
{
}
";

        private const string StructSchemeTwoWords3 = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct twoWords3
{
}
";

        private const string StructSchemeTwoWords3Fixed = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemeTwoWords4 = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct two_words3
{
}
";

        private const string StructSchemeTwoWords4Fixed = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemeTwoWords5 = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemeTwoWords5Fixed = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemeTwoWords6 = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct two_Words3
{
}
";

        private const string StructSchemeTwoWords6Fixed = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemeTwoWords7 = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemeTwoWords7Fixed = @"
struct TwoWords1
{
}

struct TwoWords2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemetwo_words2 = @"
struct two_words1
{
}

struct two_words2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemetwo_words2Fixed = @"
struct two_words1
{
}

struct two_words2
{
}

struct twowords3
{
}
";

        private const string StructSchemetwo_words3 = @"
struct two_words1
{
}

struct two_words2
{
}

struct twoWords3
{
}
";

        private const string StructSchemetwo_words3Fixed = @"
struct two_words1
{
}

struct two_words2
{
}

struct two_words3
{
}
";

        private const string StructSchemetwo_words4 = @"
struct two_words1
{
}

struct two_words2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemetwo_words4Fixed = @"
struct two_words1
{
}

struct two_words2
{
}

struct two_words3
{
}
";

        private const string StructSchemetwo_words5 = @"
struct two_words1
{
}

struct two_words2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemetwo_words5Fixed = @"
struct two_words1
{
}

struct two_words2
{
}

struct two_words3
{
}
";

        private const string StructSchemetwo_words6 = @"
struct two_words1
{
}

struct two_words2
{
}

struct two_Words3
{
}
";

        private const string StructSchemetwo_words6Fixed = @"
struct two_words1
{
}

struct two_words2
{
}

struct two_words3
{
}
";

        private const string StructSchemetwo_words7 = @"
struct two_words1
{
}

struct two_words2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemetwo_words7Fixed = @"
struct two_words1
{
}

struct two_words2
{
}

struct two_words3
{
}
";

        private const string StructSchemeTWO_WORDS1 = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct twowords3
{
}
";

        private const string StructSchemeTWO_WORDS1Fixed = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWO_WORDS3 = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct twoWords3
{
}
";

        private const string StructSchemeTWO_WORDS3Fixed = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWO_WORDS4 = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemeTWO_WORDS4Fixed = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTWO_WORDS5 = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct two_words3
{
}
";

        private const string StructSchemeTWO_WORDS5Fixed = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemeTWO_WORDS6 = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct two_Words3
{
}
";

        private const string StructSchemeTWO_WORDS6Fixed = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemeTWO_WORDS7 = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemeTWO_WORDS7Fixed = @"
struct TWO_WORDS1
{
}

struct TWO_WORDS2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemetwo_Words2 = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemetwo_Words2Fixed = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct twowords3
{
}
";

        private const string StructSchemetwo_Words3 = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct twoWords3
{
}
";

        private const string StructSchemetwo_Words3Fixed = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct two_Words3
{
}
";

        private const string StructSchemetwo_Words4 = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemetwo_Words4Fixed = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct two_Words3
{
}
";

        private const string StructSchemetwo_Words5 = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct two_words3
{
}
";

        private const string StructSchemetwo_Words5Fixed = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct two_Words3
{
}
";

        private const string StructSchemetwo_Words6 = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemetwo_Words6Fixed = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct two_Words3
{
}
";

        private const string StructSchemetwo_Words7 = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemetwo_Words7Fixed = @"
struct two_Words1
{
}

struct two_Words2
{
}

struct two_Words3
{
}
";

        private const string StructSchemeTwo_Words1 = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct twowords3
{
}
";

        private const string StructSchemeTwo_Words1Fixed = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct Twowords3
{
}
";

        private const string StructSchemeTwo_Words2 = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct TWOWORDS3
{
}
";

        private const string StructSchemeTwo_Words2Fixed = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct Twowords3
{
}
";

        private const string StructSchemeTwo_Words3 = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct twoWords3
{
}
";

        private const string StructSchemeTwo_Words3Fixed = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemeTwo_Words4 = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct TwoWords3
{
}
";

        private const string StructSchemeTwo_Words4Fixed = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemeTwo_Words5 = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct two_words3
{
}
";

        private const string StructSchemeTwo_Words5Fixed = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemeTwo_Words6 = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct TWO_WORDS3
{
}
";

        private const string StructSchemeTwo_Words6Fixed = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct Two_Words3
{
}
";

        private const string StructSchemeTwo_Words7 = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct two_Words3
{
}
";

        private const string StructSchemeTwo_Words7Fixed = @"
struct Two_Words1
{
}

struct Two_Words2
{
}

struct Two_Words3
{
}
";

        private const string Trivia1 = @"
struct twowords1
{
}

struct twowords2
{
}

struct /* */TWOWORDS3/* */
{
}
";

        private const string Trivia1Fixed = @"
struct twowords1
{
}

struct twowords2
{
}

struct /* */twowords3/* */
{
}
";

        [DataTestMethod]
        [
        DataRow(StructSchemetwowords1, StructSchemetwowords1Fixed, 10, 1, "TWOWORDS3"),
        DataRow(StructSchemetwowords3, StructSchemetwowords3Fixed, 10, 1, "TwoWords3"),
        DataRow(StructSchemetwowords4, StructSchemetwowords4Fixed, 10, 1, "two_words3"),
        DataRow(StructSchemetwowords5, StructSchemetwowords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(StructSchemetwowords6, StructSchemetwowords6Fixed, 10, 1, "two_Words3"),
        DataRow(StructSchemetwowords7, StructSchemetwowords7Fixed, 10, 1, "Two_Words3"),
        DataRow(StructSchemeTWOWORDS1, StructSchemeTWOWORDS1Fixed, 10, 1, "twowords3"),
        DataRow(StructSchemeTWOWORDS2, StructSchemeTWOWORDS2Fixed, 10, 1, "twoWords3"),
        DataRow(StructSchemeTWOWORDS4, StructSchemeTWOWORDS4Fixed, 10, 1, "two_words3"),
        DataRow(StructSchemeTWOWORDS5, StructSchemeTWOWORDS5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(StructSchemeTWOWORDS6, StructSchemeTWOWORDS6Fixed, 10, 1, "two_Words3"),
        DataRow(StructSchemeTWOWORDS7, StructSchemeTWOWORDS7Fixed, 10, 1, "Two_Words3"),
        DataRow(StructSchemetwoWords2, StructSchemetwoWords2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(StructSchemetwoWords3, StructSchemetwoWords3Fixed, 10, 1, "TwoWords3"),
        DataRow(StructSchemetwoWords4, StructSchemetwoWords4Fixed, 10, 1, "two_words3"),
        DataRow(StructSchemetwoWords5, StructSchemetwoWords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(StructSchemetwoWords6, StructSchemetwoWords6Fixed, 10, 1, "two_Words3"),
        DataRow(StructSchemetwoWords7, StructSchemetwoWords7Fixed, 10, 1, "Two_Words3"),
        DataRow(StructSchemeTwoWords1, StructSchemeTwoWords1Fixed, 10, 1, "twowords3"),
        DataRow(StructSchemeTwoWords3, StructSchemeTwoWords3Fixed, 10, 1, "twoWords3"),
        DataRow(StructSchemeTwoWords4, StructSchemeTwoWords4Fixed, 10, 1, "two_words3"),
        DataRow(StructSchemeTwoWords5, StructSchemeTwoWords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(StructSchemeTwoWords6, StructSchemeTwoWords6Fixed, 10, 1, "two_Words3"),
        DataRow(StructSchemeTwoWords7, StructSchemeTwoWords7Fixed, 10, 1, "Two_Words3"),
        DataRow(StructSchemetwo_words2, StructSchemetwo_words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(StructSchemetwo_words3, StructSchemetwo_words3Fixed, 10, 1, "twoWords3"),
        DataRow(StructSchemetwo_words4, StructSchemetwo_words4Fixed, 10, 1, "TwoWords3"),
        DataRow(StructSchemetwo_words5, StructSchemetwo_words5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(StructSchemetwo_words6, StructSchemetwo_words6Fixed, 10, 1, "two_Words3"),
        DataRow(StructSchemetwo_words7, StructSchemetwo_words7Fixed, 10, 1, "Two_Words3"),
        DataRow(StructSchemeTWO_WORDS1, StructSchemeTWO_WORDS1Fixed, 10, 1, "twowords3"),
        DataRow(StructSchemeTWO_WORDS3, StructSchemeTWO_WORDS3Fixed, 10, 1, "twoWords3"),
        DataRow(StructSchemeTWO_WORDS4, StructSchemeTWO_WORDS4Fixed, 10, 1, "TwoWords3"),
        DataRow(StructSchemeTWO_WORDS5, StructSchemeTWO_WORDS5Fixed, 10, 1, "two_words3"),
        DataRow(StructSchemeTWO_WORDS6, StructSchemeTWO_WORDS6Fixed, 10, 1, "two_Words3"),
        DataRow(StructSchemeTWO_WORDS7, StructSchemeTWO_WORDS7Fixed, 10, 1, "Two_Words3"),
        DataRow(StructSchemetwo_Words2, StructSchemetwo_Words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(StructSchemetwo_Words3, StructSchemetwo_Words3Fixed, 10, 1, "twoWords3"),
        DataRow(StructSchemetwo_Words4, StructSchemetwo_Words4Fixed, 10, 1, "TwoWords3"),
        DataRow(StructSchemetwo_Words5, StructSchemetwo_Words5Fixed, 10, 1, "two_words3"),
        DataRow(StructSchemetwo_Words6, StructSchemetwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(StructSchemetwo_Words7, StructSchemetwo_Words7Fixed, 10, 1, "Two_Words3"),
        DataRow(StructSchemeTwo_Words1, StructSchemeTwo_Words1Fixed, 10, 1, "twowords3"),
        DataRow(StructSchemeTwo_Words2, StructSchemeTwo_Words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(StructSchemeTwo_Words3, StructSchemeTwo_Words3Fixed, 10, 1, "twoWords3"),
        DataRow(StructSchemeTwo_Words4, StructSchemeTwo_Words4Fixed, 10, 1, "TwoWords3"),
        DataRow(StructSchemeTwo_Words5, StructSchemeTwo_Words5Fixed, 10, 1, "two_words3"),
        DataRow(StructSchemeTwo_Words6, StructSchemeTwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(StructSchemeTwo_Words7, StructSchemeTwo_Words7Fixed, 10, 1, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 10, 1, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1303MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1303)),
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
