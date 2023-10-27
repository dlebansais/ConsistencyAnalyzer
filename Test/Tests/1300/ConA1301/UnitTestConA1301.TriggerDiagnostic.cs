namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1301
{
    private const string ClassSchemetwowords1 = @"
class twowords1
{
}

class twowords2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemetwowords1Fixed = @"
class twowords1
{
}

class twowords2
{
}

class twowords3
{
}
";

    private const string ClassSchemetwowords3 = @"
class twowords1
{
}

class twowords2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemetwowords3Fixed = @"
class twowords1
{
}

class twowords2
{
}

class twowords3
{
}
";

    private const string ClassSchemetwowords4 = @"
class twowords1
{
}

class twowords2
{
}

class two_words3
{
}
";

    private const string ClassSchemetwowords4Fixed = @"
class twowords1
{
}

class twowords2
{
}

class twowords3
{
}
";

    private const string ClassSchemetwowords5 = @"
class twowords1
{
}

class twowords2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemetwowords5Fixed = @"
class twowords1
{
}

class twowords2
{
}

class twowords3
{
}
";

    private const string ClassSchemetwowords6 = @"
class twowords1
{
}

class twowords2
{
}

class two_Words3
{
}
";

    private const string ClassSchemetwowords6Fixed = @"
class twowords1
{
}

class twowords2
{
}

class twowords3
{
}
";

    private const string ClassSchemetwowords7 = @"
class twowords1
{
}

class twowords2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemetwowords7Fixed = @"
class twowords1
{
}

class twowords2
{
}

class twowords3
{
}
";

    private const string ClassSchemeTWOWORDS1 = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class twowords3
{
}
";

    private const string ClassSchemeTWOWORDS1Fixed = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWOWORDS2 = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class twoWords3
{
}
";

    private const string ClassSchemeTWOWORDS2Fixed = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWOWORDS4 = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class two_words3
{
}
";

    private const string ClassSchemeTWOWORDS4Fixed = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWOWORDS5 = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemeTWOWORDS5Fixed = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWOWORDS6 = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class two_Words3
{
}
";

    private const string ClassSchemeTWOWORDS6Fixed = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWOWORDS7 = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemeTWOWORDS7Fixed = @"
class TWOWORDS1
{
}

class TWOWORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemetwoWords2 = @"
class twoWords1
{
}

class twoWords2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemetwoWords2Fixed = @"
class twoWords1
{
}

class twoWords2
{
}

class tWOWORDS3
{
}
";

    private const string ClassSchemetwoWords3 = @"
class twoWords1
{
}

class twoWords2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemetwoWords3Fixed = @"
class twoWords1
{
}

class twoWords2
{
}

class twoWords3
{
}
";

    private const string ClassSchemetwoWords4 = @"
class twoWords1
{
}

class twoWords2
{
}

class two_words3
{
}
";

    private const string ClassSchemetwoWords4Fixed = @"
class twoWords1
{
}

class twoWords2
{
}

class twoWords3
{
}
";

    private const string ClassSchemetwoWords5 = @"
class twoWords1
{
}

class twoWords2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemetwoWords5Fixed = @"
class twoWords1
{
}

class twoWords2
{
}

class twoWords3
{
}
";

    private const string ClassSchemetwoWords6 = @"
class twoWords1
{
}

class twoWords2
{
}

class two_Words3
{
}
";

    private const string ClassSchemetwoWords6Fixed = @"
class twoWords1
{
}

class twoWords2
{
}

class twoWords3
{
}
";

    private const string ClassSchemetwoWords7 = @"
class twoWords1
{
}

class twoWords2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemetwoWords7Fixed = @"
class twoWords1
{
}

class twoWords2
{
}

class twoWords3
{
}
";

    private const string ClassSchemeTwoWords1 = @"
class TwoWords1
{
}

class TwoWords2
{
}

class twowords3
{
}
";

    private const string ClassSchemeTwoWords1Fixed = @"
class TwoWords1
{
}

class TwoWords2
{
}

class Twowords3
{
}
";

    private const string ClassSchemeTwoWords3 = @"
class TwoWords1
{
}

class TwoWords2
{
}

class twoWords3
{
}
";

    private const string ClassSchemeTwoWords3Fixed = @"
class TwoWords1
{
}

class TwoWords2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemeTwoWords4 = @"
class TwoWords1
{
}

class TwoWords2
{
}

class two_words3
{
}
";

    private const string ClassSchemeTwoWords4Fixed = @"
class TwoWords1
{
}

class TwoWords2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemeTwoWords5 = @"
class TwoWords1
{
}

class TwoWords2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemeTwoWords5Fixed = @"
class TwoWords1
{
}

class TwoWords2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemeTwoWords6 = @"
class TwoWords1
{
}

class TwoWords2
{
}

class two_Words3
{
}
";

    private const string ClassSchemeTwoWords6Fixed = @"
class TwoWords1
{
}

class TwoWords2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemeTwoWords7 = @"
class TwoWords1
{
}

class TwoWords2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemeTwoWords7Fixed = @"
class TwoWords1
{
}

class TwoWords2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemetwo_words2 = @"
class two_words1
{
}

class two_words2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemetwo_words2Fixed = @"
class two_words1
{
}

class two_words2
{
}

class twowords3
{
}
";

    private const string ClassSchemetwo_words3 = @"
class two_words1
{
}

class two_words2
{
}

class twoWords3
{
}
";

    private const string ClassSchemetwo_words3Fixed = @"
class two_words1
{
}

class two_words2
{
}

class two_words3
{
}
";

    private const string ClassSchemetwo_words4 = @"
class two_words1
{
}

class two_words2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemetwo_words4Fixed = @"
class two_words1
{
}

class two_words2
{
}

class two_words3
{
}
";

    private const string ClassSchemetwo_words5 = @"
class two_words1
{
}

class two_words2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemetwo_words5Fixed = @"
class two_words1
{
}

class two_words2
{
}

class two_words3
{
}
";

    private const string ClassSchemetwo_words6 = @"
class two_words1
{
}

class two_words2
{
}

class two_Words3
{
}
";

    private const string ClassSchemetwo_words6Fixed = @"
class two_words1
{
}

class two_words2
{
}

class two_words3
{
}
";

    private const string ClassSchemetwo_words7 = @"
class two_words1
{
}

class two_words2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemetwo_words7Fixed = @"
class two_words1
{
}

class two_words2
{
}

class two_words3
{
}
";

    private const string ClassSchemeTWO_WORDS1 = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class twowords3
{
}
";

    private const string ClassSchemeTWO_WORDS1Fixed = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWO_WORDS3 = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class twoWords3
{
}
";

    private const string ClassSchemeTWO_WORDS3Fixed = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWO_WORDS4 = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemeTWO_WORDS4Fixed = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTWO_WORDS5 = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class two_words3
{
}
";

    private const string ClassSchemeTWO_WORDS5Fixed = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemeTWO_WORDS6 = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class two_Words3
{
}
";

    private const string ClassSchemeTWO_WORDS6Fixed = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemeTWO_WORDS7 = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemeTWO_WORDS7Fixed = @"
class TWO_WORDS1
{
}

class TWO_WORDS2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemetwo_Words2 = @"
class two_Words1
{
}

class two_Words2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemetwo_Words2Fixed = @"
class two_Words1
{
}

class two_Words2
{
}

class twowords3
{
}
";

    private const string ClassSchemetwo_Words3 = @"
class two_Words1
{
}

class two_Words2
{
}

class twoWords3
{
}
";

    private const string ClassSchemetwo_Words3Fixed = @"
class two_Words1
{
}

class two_Words2
{
}

class two_Words3
{
}
";

    private const string ClassSchemetwo_Words4 = @"
class two_Words1
{
}

class two_Words2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemetwo_Words4Fixed = @"
class two_Words1
{
}

class two_Words2
{
}

class two_Words3
{
}
";

    private const string ClassSchemetwo_Words5 = @"
class two_Words1
{
}

class two_Words2
{
}

class two_words3
{
}
";

    private const string ClassSchemetwo_Words5Fixed = @"
class two_Words1
{
}

class two_Words2
{
}

class two_Words3
{
}
";

    private const string ClassSchemetwo_Words6 = @"
class two_Words1
{
}

class two_Words2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemetwo_Words6Fixed = @"
class two_Words1
{
}

class two_Words2
{
}

class two_Words3
{
}
";

    private const string ClassSchemetwo_Words7 = @"
class two_Words1
{
}

class two_Words2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemetwo_Words7Fixed = @"
class two_Words1
{
}

class two_Words2
{
}

class two_Words3
{
}
";

    private const string ClassSchemeTwo_Words1 = @"
class Two_Words1
{
}

class Two_Words2
{
}

class twowords3
{
}
";

    private const string ClassSchemeTwo_Words1Fixed = @"
class Two_Words1
{
}

class Two_Words2
{
}

class Twowords3
{
}
";

    private const string ClassSchemeTwo_Words2 = @"
class Two_Words1
{
}

class Two_Words2
{
}

class TWOWORDS3
{
}
";

    private const string ClassSchemeTwo_Words2Fixed = @"
class Two_Words1
{
}

class Two_Words2
{
}

class Twowords3
{
}
";

    private const string ClassSchemeTwo_Words3 = @"
class Two_Words1
{
}

class Two_Words2
{
}

class twoWords3
{
}
";

    private const string ClassSchemeTwo_Words3Fixed = @"
class Two_Words1
{
}

class Two_Words2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemeTwo_Words4 = @"
class Two_Words1
{
}

class Two_Words2
{
}

class TwoWords3
{
}
";

    private const string ClassSchemeTwo_Words4Fixed = @"
class Two_Words1
{
}

class Two_Words2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemeTwo_Words5 = @"
class Two_Words1
{
}

class Two_Words2
{
}

class two_words3
{
}
";

    private const string ClassSchemeTwo_Words5Fixed = @"
class Two_Words1
{
}

class Two_Words2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemeTwo_Words6 = @"
class Two_Words1
{
}

class Two_Words2
{
}

class TWO_WORDS3
{
}
";

    private const string ClassSchemeTwo_Words6Fixed = @"
class Two_Words1
{
}

class Two_Words2
{
}

class Two_Words3
{
}
";

    private const string ClassSchemeTwo_Words7 = @"
class Two_Words1
{
}

class Two_Words2
{
}

class two_Words3
{
}
";

    private const string ClassSchemeTwo_Words7Fixed = @"
class Two_Words1
{
}

class Two_Words2
{
}

class Two_Words3
{
}
";

    private const string Trivia1 = @"
class twowords1
{
}

class twowords2
{
}

class /* */TWOWORDS3/* */
{
}
";

    private const string Trivia1Fixed = @"
class twowords1
{
}

class twowords2
{
}

class /* */twowords3/* */
{
}
";

    [DataTestMethod]
    [
    DataRow(ClassSchemetwowords1, ClassSchemetwowords1Fixed, 10, 1, "TWOWORDS3"),
    DataRow(ClassSchemetwowords3, ClassSchemetwowords3Fixed, 10, 1, "TwoWords3"),
    DataRow(ClassSchemetwowords4, ClassSchemetwowords4Fixed, 10, 1, "two_words3"),
    DataRow(ClassSchemetwowords5, ClassSchemetwowords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(ClassSchemetwowords6, ClassSchemetwowords6Fixed, 10, 1, "two_Words3"),
    DataRow(ClassSchemetwowords7, ClassSchemetwowords7Fixed, 10, 1, "Two_Words3"),
    DataRow(ClassSchemeTWOWORDS1, ClassSchemeTWOWORDS1Fixed, 10, 1, "twowords3"),
    DataRow(ClassSchemeTWOWORDS2, ClassSchemeTWOWORDS2Fixed, 10, 1, "twoWords3"),
    DataRow(ClassSchemeTWOWORDS4, ClassSchemeTWOWORDS4Fixed, 10, 1, "two_words3"),
    DataRow(ClassSchemeTWOWORDS5, ClassSchemeTWOWORDS5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(ClassSchemeTWOWORDS6, ClassSchemeTWOWORDS6Fixed, 10, 1, "two_Words3"),
    DataRow(ClassSchemeTWOWORDS7, ClassSchemeTWOWORDS7Fixed, 10, 1, "Two_Words3"),
    DataRow(ClassSchemetwoWords2, ClassSchemetwoWords2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(ClassSchemetwoWords3, ClassSchemetwoWords3Fixed, 10, 1, "TwoWords3"),
    DataRow(ClassSchemetwoWords4, ClassSchemetwoWords4Fixed, 10, 1, "two_words3"),
    DataRow(ClassSchemetwoWords5, ClassSchemetwoWords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(ClassSchemetwoWords6, ClassSchemetwoWords6Fixed, 10, 1, "two_Words3"),
    DataRow(ClassSchemetwoWords7, ClassSchemetwoWords7Fixed, 10, 1, "Two_Words3"),
    DataRow(ClassSchemeTwoWords1, ClassSchemeTwoWords1Fixed, 10, 1, "twowords3"),
    DataRow(ClassSchemeTwoWords3, ClassSchemeTwoWords3Fixed, 10, 1, "twoWords3"),
    DataRow(ClassSchemeTwoWords4, ClassSchemeTwoWords4Fixed, 10, 1, "two_words3"),
    DataRow(ClassSchemeTwoWords5, ClassSchemeTwoWords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(ClassSchemeTwoWords6, ClassSchemeTwoWords6Fixed, 10, 1, "two_Words3"),
    DataRow(ClassSchemeTwoWords7, ClassSchemeTwoWords7Fixed, 10, 1, "Two_Words3"),
    DataRow(ClassSchemetwo_words2, ClassSchemetwo_words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(ClassSchemetwo_words3, ClassSchemetwo_words3Fixed, 10, 1, "twoWords3"),
    DataRow(ClassSchemetwo_words4, ClassSchemetwo_words4Fixed, 10, 1, "TwoWords3"),
    DataRow(ClassSchemetwo_words5, ClassSchemetwo_words5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(ClassSchemetwo_words6, ClassSchemetwo_words6Fixed, 10, 1, "two_Words3"),
    DataRow(ClassSchemetwo_words7, ClassSchemetwo_words7Fixed, 10, 1, "Two_Words3"),
    DataRow(ClassSchemeTWO_WORDS1, ClassSchemeTWO_WORDS1Fixed, 10, 1, "twowords3"),
    DataRow(ClassSchemeTWO_WORDS3, ClassSchemeTWO_WORDS3Fixed, 10, 1, "twoWords3"),
    DataRow(ClassSchemeTWO_WORDS4, ClassSchemeTWO_WORDS4Fixed, 10, 1, "TwoWords3"),
    DataRow(ClassSchemeTWO_WORDS5, ClassSchemeTWO_WORDS5Fixed, 10, 1, "two_words3"),
    DataRow(ClassSchemeTWO_WORDS6, ClassSchemeTWO_WORDS6Fixed, 10, 1, "two_Words3"),
    DataRow(ClassSchemeTWO_WORDS7, ClassSchemeTWO_WORDS7Fixed, 10, 1, "Two_Words3"),
    DataRow(ClassSchemetwo_Words2, ClassSchemetwo_Words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(ClassSchemetwo_Words3, ClassSchemetwo_Words3Fixed, 10, 1, "twoWords3"),
    DataRow(ClassSchemetwo_Words4, ClassSchemetwo_Words4Fixed, 10, 1, "TwoWords3"),
    DataRow(ClassSchemetwo_Words5, ClassSchemetwo_Words5Fixed, 10, 1, "two_words3"),
    DataRow(ClassSchemetwo_Words6, ClassSchemetwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(ClassSchemetwo_Words7, ClassSchemetwo_Words7Fixed, 10, 1, "Two_Words3"),
    DataRow(ClassSchemeTwo_Words1, ClassSchemeTwo_Words1Fixed, 10, 1, "twowords3"),
    DataRow(ClassSchemeTwo_Words2, ClassSchemeTwo_Words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(ClassSchemeTwo_Words3, ClassSchemeTwo_Words3Fixed, 10, 1, "twoWords3"),
    DataRow(ClassSchemeTwo_Words4, ClassSchemeTwo_Words4Fixed, 10, 1, "TwoWords3"),
    DataRow(ClassSchemeTwo_Words5, ClassSchemeTwo_Words5Fixed, 10, 1, "two_words3"),
    DataRow(ClassSchemeTwo_Words6, ClassSchemeTwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(ClassSchemeTwo_Words7, ClassSchemeTwo_Words7Fixed, 10, 1, "two_Words3"),
    DataRow(Trivia1, Trivia1Fixed, 10, 1, "TWOWORDS3"),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1301MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1301)),
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
