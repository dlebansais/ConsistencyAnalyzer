namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1302
    {
        private const string RecordSchemetwowords1 = @"
record twowords1
{
}

record twowords2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemetwowords1Fixed = @"
record twowords1
{
}

record twowords2
{
}

record twowords3
{
}
";

        private const string RecordSchemetwowords3 = @"
record twowords1
{
}

record twowords2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemetwowords3Fixed = @"
record twowords1
{
}

record twowords2
{
}

record twowords3
{
}
";

        private const string RecordSchemetwowords4 = @"
record twowords1
{
}

record twowords2
{
}

record two_words3
{
}
";

        private const string RecordSchemetwowords4Fixed = @"
record twowords1
{
}

record twowords2
{
}

record twowords3
{
}
";

        private const string RecordSchemetwowords5 = @"
record twowords1
{
}

record twowords2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemetwowords5Fixed = @"
record twowords1
{
}

record twowords2
{
}

record twowords3
{
}
";

        private const string RecordSchemetwowords6 = @"
record twowords1
{
}

record twowords2
{
}

record two_Words3
{
}
";

        private const string RecordSchemetwowords6Fixed = @"
record twowords1
{
}

record twowords2
{
}

record twowords3
{
}
";

        private const string RecordSchemetwowords7 = @"
record twowords1
{
}

record twowords2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemetwowords7Fixed = @"
record twowords1
{
}

record twowords2
{
}

record twowords3
{
}
";

        private const string RecordSchemeTWOWORDS1 = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record twowords3
{
}
";

        private const string RecordSchemeTWOWORDS1Fixed = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWOWORDS2 = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record twoWords3
{
}
";

        private const string RecordSchemeTWOWORDS2Fixed = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWOWORDS4 = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record two_words3
{
}
";

        private const string RecordSchemeTWOWORDS4Fixed = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWOWORDS5 = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemeTWOWORDS5Fixed = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWOWORDS6 = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record two_Words3
{
}
";

        private const string RecordSchemeTWOWORDS6Fixed = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWOWORDS7 = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemeTWOWORDS7Fixed = @"
record TWOWORDS1
{
}

record TWOWORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemetwoWords2 = @"
record twoWords1
{
}

record twoWords2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemetwoWords2Fixed = @"
record twoWords1
{
}

record twoWords2
{
}

record tWOWORDS3
{
}
";

        private const string RecordSchemetwoWords3 = @"
record twoWords1
{
}

record twoWords2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemetwoWords3Fixed = @"
record twoWords1
{
}

record twoWords2
{
}

record twoWords3
{
}
";

        private const string RecordSchemetwoWords4 = @"
record twoWords1
{
}

record twoWords2
{
}

record two_words3
{
}
";

        private const string RecordSchemetwoWords4Fixed = @"
record twoWords1
{
}

record twoWords2
{
}

record twoWords3
{
}
";

        private const string RecordSchemetwoWords5 = @"
record twoWords1
{
}

record twoWords2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemetwoWords5Fixed = @"
record twoWords1
{
}

record twoWords2
{
}

record twoWords3
{
}
";

        private const string RecordSchemetwoWords6 = @"
record twoWords1
{
}

record twoWords2
{
}

record two_Words3
{
}
";

        private const string RecordSchemetwoWords6Fixed = @"
record twoWords1
{
}

record twoWords2
{
}

record twoWords3
{
}
";

        private const string RecordSchemetwoWords7 = @"
record twoWords1
{
}

record twoWords2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemetwoWords7Fixed = @"
record twoWords1
{
}

record twoWords2
{
}

record twoWords3
{
}
";

        private const string RecordSchemeTwoWords1 = @"
record TwoWords1
{
}

record TwoWords2
{
}

record twowords3
{
}
";

        private const string RecordSchemeTwoWords1Fixed = @"
record TwoWords1
{
}

record TwoWords2
{
}

record Twowords3
{
}
";

        private const string RecordSchemeTwoWords3 = @"
record TwoWords1
{
}

record TwoWords2
{
}

record twoWords3
{
}
";

        private const string RecordSchemeTwoWords3Fixed = @"
record TwoWords1
{
}

record TwoWords2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemeTwoWords4 = @"
record TwoWords1
{
}

record TwoWords2
{
}

record two_words3
{
}
";

        private const string RecordSchemeTwoWords4Fixed = @"
record TwoWords1
{
}

record TwoWords2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemeTwoWords5 = @"
record TwoWords1
{
}

record TwoWords2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemeTwoWords5Fixed = @"
record TwoWords1
{
}

record TwoWords2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemeTwoWords6 = @"
record TwoWords1
{
}

record TwoWords2
{
}

record two_Words3
{
}
";

        private const string RecordSchemeTwoWords6Fixed = @"
record TwoWords1
{
}

record TwoWords2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemeTwoWords7 = @"
record TwoWords1
{
}

record TwoWords2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemeTwoWords7Fixed = @"
record TwoWords1
{
}

record TwoWords2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemetwo_words2 = @"
record two_words1
{
}

record two_words2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemetwo_words2Fixed = @"
record two_words1
{
}

record two_words2
{
}

record twowords3
{
}
";

        private const string RecordSchemetwo_words3 = @"
record two_words1
{
}

record two_words2
{
}

record twoWords3
{
}
";

        private const string RecordSchemetwo_words3Fixed = @"
record two_words1
{
}

record two_words2
{
}

record two_words3
{
}
";

        private const string RecordSchemetwo_words4 = @"
record two_words1
{
}

record two_words2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemetwo_words4Fixed = @"
record two_words1
{
}

record two_words2
{
}

record two_words3
{
}
";

        private const string RecordSchemetwo_words5 = @"
record two_words1
{
}

record two_words2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemetwo_words5Fixed = @"
record two_words1
{
}

record two_words2
{
}

record two_words3
{
}
";

        private const string RecordSchemetwo_words6 = @"
record two_words1
{
}

record two_words2
{
}

record two_Words3
{
}
";

        private const string RecordSchemetwo_words6Fixed = @"
record two_words1
{
}

record two_words2
{
}

record two_words3
{
}
";

        private const string RecordSchemetwo_words7 = @"
record two_words1
{
}

record two_words2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemetwo_words7Fixed = @"
record two_words1
{
}

record two_words2
{
}

record two_words3
{
}
";

        private const string RecordSchemeTWO_WORDS1 = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record twowords3
{
}
";

        private const string RecordSchemeTWO_WORDS1Fixed = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWO_WORDS3 = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record twoWords3
{
}
";

        private const string RecordSchemeTWO_WORDS3Fixed = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWO_WORDS4 = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemeTWO_WORDS4Fixed = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTWO_WORDS5 = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record two_words3
{
}
";

        private const string RecordSchemeTWO_WORDS5Fixed = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemeTWO_WORDS6 = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record two_Words3
{
}
";

        private const string RecordSchemeTWO_WORDS6Fixed = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemeTWO_WORDS7 = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemeTWO_WORDS7Fixed = @"
record TWO_WORDS1
{
}

record TWO_WORDS2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemetwo_Words2 = @"
record two_Words1
{
}

record two_Words2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemetwo_Words2Fixed = @"
record two_Words1
{
}

record two_Words2
{
}

record twowords3
{
}
";

        private const string RecordSchemetwo_Words3 = @"
record two_Words1
{
}

record two_Words2
{
}

record twoWords3
{
}
";

        private const string RecordSchemetwo_Words3Fixed = @"
record two_Words1
{
}

record two_Words2
{
}

record two_Words3
{
}
";

        private const string RecordSchemetwo_Words4 = @"
record two_Words1
{
}

record two_Words2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemetwo_Words4Fixed = @"
record two_Words1
{
}

record two_Words2
{
}

record two_Words3
{
}
";

        private const string RecordSchemetwo_Words5 = @"
record two_Words1
{
}

record two_Words2
{
}

record two_words3
{
}
";

        private const string RecordSchemetwo_Words5Fixed = @"
record two_Words1
{
}

record two_Words2
{
}

record two_Words3
{
}
";

        private const string RecordSchemetwo_Words6 = @"
record two_Words1
{
}

record two_Words2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemetwo_Words6Fixed = @"
record two_Words1
{
}

record two_Words2
{
}

record two_Words3
{
}
";

        private const string RecordSchemetwo_Words7 = @"
record two_Words1
{
}

record two_Words2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemetwo_Words7Fixed = @"
record two_Words1
{
}

record two_Words2
{
}

record two_Words3
{
}
";

        private const string RecordSchemeTwo_Words1 = @"
record Two_Words1
{
}

record Two_Words2
{
}

record twowords3
{
}
";

        private const string RecordSchemeTwo_Words1Fixed = @"
record Two_Words1
{
}

record Two_Words2
{
}

record Twowords3
{
}
";

        private const string RecordSchemeTwo_Words2 = @"
record Two_Words1
{
}

record Two_Words2
{
}

record TWOWORDS3
{
}
";

        private const string RecordSchemeTwo_Words2Fixed = @"
record Two_Words1
{
}

record Two_Words2
{
}

record Twowords3
{
}
";

        private const string RecordSchemeTwo_Words3 = @"
record Two_Words1
{
}

record Two_Words2
{
}

record twoWords3
{
}
";

        private const string RecordSchemeTwo_Words3Fixed = @"
record Two_Words1
{
}

record Two_Words2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemeTwo_Words4 = @"
record Two_Words1
{
}

record Two_Words2
{
}

record TwoWords3
{
}
";

        private const string RecordSchemeTwo_Words4Fixed = @"
record Two_Words1
{
}

record Two_Words2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemeTwo_Words5 = @"
record Two_Words1
{
}

record Two_Words2
{
}

record two_words3
{
}
";

        private const string RecordSchemeTwo_Words5Fixed = @"
record Two_Words1
{
}

record Two_Words2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemeTwo_Words6 = @"
record Two_Words1
{
}

record Two_Words2
{
}

record TWO_WORDS3
{
}
";

        private const string RecordSchemeTwo_Words6Fixed = @"
record Two_Words1
{
}

record Two_Words2
{
}

record Two_Words3
{
}
";

        private const string RecordSchemeTwo_Words7 = @"
record Two_Words1
{
}

record Two_Words2
{
}

record two_Words3
{
}
";

        private const string RecordSchemeTwo_Words7Fixed = @"
record Two_Words1
{
}

record Two_Words2
{
}

record Two_Words3
{
}
";

        private const string Trivia1 = @"
record twowords1
{
}

record twowords2
{
}

record /* */TWOWORDS3/* */
{
}
";

        private const string Trivia1Fixed = @"
record twowords1
{
}

record twowords2
{
}

record /* */twowords3/* */
{
}
";

        [DataTestMethod]
        [
        DataRow(RecordSchemetwowords1, RecordSchemetwowords1Fixed, 10, 1, "TWOWORDS3"),
        DataRow(RecordSchemetwowords3, RecordSchemetwowords3Fixed, 10, 1, "TwoWords3"),
        DataRow(RecordSchemetwowords4, RecordSchemetwowords4Fixed, 10, 1, "two_words3"),
        DataRow(RecordSchemetwowords5, RecordSchemetwowords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(RecordSchemetwowords6, RecordSchemetwowords6Fixed, 10, 1, "two_Words3"),
        DataRow(RecordSchemetwowords7, RecordSchemetwowords7Fixed, 10, 1, "Two_Words3"),
        DataRow(RecordSchemeTWOWORDS1, RecordSchemeTWOWORDS1Fixed, 10, 1, "twowords3"),
        DataRow(RecordSchemeTWOWORDS2, RecordSchemeTWOWORDS2Fixed, 10, 1, "twoWords3"),
        DataRow(RecordSchemeTWOWORDS4, RecordSchemeTWOWORDS4Fixed, 10, 1, "two_words3"),
        DataRow(RecordSchemeTWOWORDS5, RecordSchemeTWOWORDS5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(RecordSchemeTWOWORDS6, RecordSchemeTWOWORDS6Fixed, 10, 1, "two_Words3"),
        DataRow(RecordSchemeTWOWORDS7, RecordSchemeTWOWORDS7Fixed, 10, 1, "Two_Words3"),
        DataRow(RecordSchemetwoWords2, RecordSchemetwoWords2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(RecordSchemetwoWords3, RecordSchemetwoWords3Fixed, 10, 1, "TwoWords3"),
        DataRow(RecordSchemetwoWords4, RecordSchemetwoWords4Fixed, 10, 1, "two_words3"),
        DataRow(RecordSchemetwoWords5, RecordSchemetwoWords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(RecordSchemetwoWords6, RecordSchemetwoWords6Fixed, 10, 1, "two_Words3"),
        DataRow(RecordSchemetwoWords7, RecordSchemetwoWords7Fixed, 10, 1, "Two_Words3"),
        DataRow(RecordSchemeTwoWords1, RecordSchemeTwoWords1Fixed, 10, 1, "twowords3"),
        DataRow(RecordSchemeTwoWords3, RecordSchemeTwoWords3Fixed, 10, 1, "twoWords3"),
        DataRow(RecordSchemeTwoWords4, RecordSchemeTwoWords4Fixed, 10, 1, "two_words3"),
        DataRow(RecordSchemeTwoWords5, RecordSchemeTwoWords5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(RecordSchemeTwoWords6, RecordSchemeTwoWords6Fixed, 10, 1, "two_Words3"),
        DataRow(RecordSchemeTwoWords7, RecordSchemeTwoWords7Fixed, 10, 1, "Two_Words3"),
        DataRow(RecordSchemetwo_words2, RecordSchemetwo_words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(RecordSchemetwo_words3, RecordSchemetwo_words3Fixed, 10, 1, "twoWords3"),
        DataRow(RecordSchemetwo_words4, RecordSchemetwo_words4Fixed, 10, 1, "TwoWords3"),
        DataRow(RecordSchemetwo_words5, RecordSchemetwo_words5Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(RecordSchemetwo_words6, RecordSchemetwo_words6Fixed, 10, 1, "two_Words3"),
        DataRow(RecordSchemetwo_words7, RecordSchemetwo_words7Fixed, 10, 1, "Two_Words3"),
        DataRow(RecordSchemeTWO_WORDS1, RecordSchemeTWO_WORDS1Fixed, 10, 1, "twowords3"),
        DataRow(RecordSchemeTWO_WORDS3, RecordSchemeTWO_WORDS3Fixed, 10, 1, "twoWords3"),
        DataRow(RecordSchemeTWO_WORDS4, RecordSchemeTWO_WORDS4Fixed, 10, 1, "TwoWords3"),
        DataRow(RecordSchemeTWO_WORDS5, RecordSchemeTWO_WORDS5Fixed, 10, 1, "two_words3"),
        DataRow(RecordSchemeTWO_WORDS6, RecordSchemeTWO_WORDS6Fixed, 10, 1, "two_Words3"),
        DataRow(RecordSchemeTWO_WORDS7, RecordSchemeTWO_WORDS7Fixed, 10, 1, "Two_Words3"),
        DataRow(RecordSchemetwo_Words2, RecordSchemetwo_Words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(RecordSchemetwo_Words3, RecordSchemetwo_Words3Fixed, 10, 1, "twoWords3"),
        DataRow(RecordSchemetwo_Words4, RecordSchemetwo_Words4Fixed, 10, 1, "TwoWords3"),
        DataRow(RecordSchemetwo_Words5, RecordSchemetwo_Words5Fixed, 10, 1, "two_words3"),
        DataRow(RecordSchemetwo_Words6, RecordSchemetwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(RecordSchemetwo_Words7, RecordSchemetwo_Words7Fixed, 10, 1, "Two_Words3"),
        DataRow(RecordSchemeTwo_Words1, RecordSchemeTwo_Words1Fixed, 10, 1, "twowords3"),
        DataRow(RecordSchemeTwo_Words2, RecordSchemeTwo_Words2Fixed, 10, 1, "TWOWORDS3"),
        DataRow(RecordSchemeTwo_Words3, RecordSchemeTwo_Words3Fixed, 10, 1, "twoWords3"),
        DataRow(RecordSchemeTwo_Words4, RecordSchemeTwo_Words4Fixed, 10, 1, "TwoWords3"),
        DataRow(RecordSchemeTwo_Words5, RecordSchemeTwo_Words5Fixed, 10, 1, "two_words3"),
        DataRow(RecordSchemeTwo_Words6, RecordSchemeTwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
        DataRow(RecordSchemeTwo_Words7, RecordSchemeTwo_Words7Fixed, 10, 1, "two_Words3"),
        DataRow(Trivia1, Trivia1Fixed, 10, 1, "TWOWORDS3"),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1302MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1302)),
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
