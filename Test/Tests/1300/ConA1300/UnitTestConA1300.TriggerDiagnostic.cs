namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1300
{
    private const string NamespaceSchemetwowords1 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemetwowords1Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemetwowords3 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemetwowords3Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemetwowords4 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemetwowords4Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemetwowords5 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemetwowords5Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemetwowords6 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemetwowords6Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemetwowords7 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemetwowords7Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemeTWOWORDS1 = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemeTWOWORDS1Fixed = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWOWORDS2 = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemeTWOWORDS2Fixed = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWOWORDS4 = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemeTWOWORDS4Fixed = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWOWORDS5 = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemeTWOWORDS5Fixed = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWOWORDS6 = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemeTWOWORDS6Fixed = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWOWORDS7 = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemeTWOWORDS7Fixed = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemetwoWords2 = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemetwoWords2Fixed = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace tWOWORDS3
{
}
";

    private const string NamespaceSchemetwoWords3 = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemetwoWords3Fixed = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemetwoWords4 = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemetwoWords4Fixed = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemetwoWords5 = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemetwoWords5Fixed = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemetwoWords6 = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemetwoWords6Fixed = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemetwoWords7 = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemetwoWords7Fixed = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemeTwoWords1 = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemeTwoWords1Fixed = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace Twowords3
{
}
";

    private const string NamespaceSchemeTwoWords3 = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemeTwoWords3Fixed = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemeTwoWords4 = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemeTwoWords4Fixed = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemeTwoWords5 = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemeTwoWords5Fixed = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemeTwoWords6 = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemeTwoWords6Fixed = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemeTwoWords7 = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemeTwoWords7Fixed = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemetwo_words2 = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemetwo_words2Fixed = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemetwo_words3 = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemetwo_words3Fixed = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemetwo_words4 = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemetwo_words4Fixed = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemetwo_words5 = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemetwo_words5Fixed = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemetwo_words6 = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemetwo_words6Fixed = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemetwo_words7 = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemetwo_words7Fixed = @"
namespace two_words1
{
}

namespace two_words2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemeTWO_WORDS1 = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemeTWO_WORDS1Fixed = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWO_WORDS3 = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemeTWO_WORDS3Fixed = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWO_WORDS4 = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemeTWO_WORDS4Fixed = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTWO_WORDS5 = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemeTWO_WORDS5Fixed = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemeTWO_WORDS6 = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemeTWO_WORDS6Fixed = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemeTWO_WORDS7 = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemeTWO_WORDS7Fixed = @"
namespace TWO_WORDS1
{
}

namespace TWO_WORDS2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemetwo_Words2 = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemetwo_Words2Fixed = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemetwo_Words3 = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemetwo_Words3Fixed = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemetwo_Words4 = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemetwo_Words4Fixed = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemetwo_Words5 = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemetwo_Words5Fixed = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemetwo_Words6 = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemetwo_Words6Fixed = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemetwo_Words7 = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemetwo_Words7Fixed = @"
namespace two_Words1
{
}

namespace two_Words2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemeTwo_Words1 = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace twowords3
{
}
";

    private const string NamespaceSchemeTwo_Words1Fixed = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace Twowords3
{
}
";

    private const string NamespaceSchemeTwo_Words2 = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace TWOWORDS3
{
}
";

    private const string NamespaceSchemeTwo_Words2Fixed = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace Twowords3
{
}
";

    private const string NamespaceSchemeTwo_Words3 = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace twoWords3
{
}
";

    private const string NamespaceSchemeTwo_Words3Fixed = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemeTwo_Words4 = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace TwoWords3
{
}
";

    private const string NamespaceSchemeTwo_Words4Fixed = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemeTwo_Words5 = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace two_words3
{
}
";

    private const string NamespaceSchemeTwo_Words5Fixed = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemeTwo_Words6 = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace TWO_WORDS3
{
}
";

    private const string NamespaceSchemeTwo_Words6Fixed = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace Two_Words3
{
}
";

    private const string NamespaceSchemeTwo_Words7 = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace two_Words3
{
}
";

    private const string NamespaceSchemeTwo_Words7Fixed = @"
namespace Two_Words1
{
}

namespace Two_Words2
{
}

namespace Two_Words3
{
}
";

    private const string MultiNamespaceSchemetwowords1 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords.TWOWORDS3
{
}
";

    private const string MultiNamespaceSchemetwowords1Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords.twowords3
{
}
";

    private const string MultiNamespaceSchemetwowords2 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords.TWOWORDS3.twowords
{
}
";

    private const string MultiNamespaceSchemetwowords2Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twowords.twowords3.twowords
{
}
";

    private const string Trivia1 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace /* */TWOWORDS3/* */
{
}
";

    private const string Trivia1Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace /* */twowords3/* */
{
}
";

    private const string Trivia2 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace /* */twowords.TWOWORDS3.twowords/* */
{
}
";

    private const string Trivia2Fixed = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace /* */twowords.twowords3.twowords/* */
{
}
";

    [DataTestMethod]
    [
    DataRow(NamespaceSchemetwowords1, NamespaceSchemetwowords1Fixed, 10, 1, "TWOWORDS3"),
    DataRow(NamespaceSchemetwowords3, NamespaceSchemetwowords3Fixed, 10, 1, "TwoWords3"),
    DataRow(NamespaceSchemetwowords4, NamespaceSchemetwowords4Fixed, 10, 1, "two_words3"),
    DataRow(NamespaceSchemetwowords5, NamespaceSchemetwowords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(NamespaceSchemetwowords6, NamespaceSchemetwowords6Fixed, 10, 1, "two_Words3"),
    DataRow(NamespaceSchemetwowords7, NamespaceSchemetwowords7Fixed, 10, 1, "Two_Words3"),
    DataRow(NamespaceSchemeTWOWORDS1, NamespaceSchemeTWOWORDS1Fixed, 10, 1, "twowords3"),
    DataRow(NamespaceSchemeTWOWORDS2, NamespaceSchemeTWOWORDS2Fixed, 10, 1, "twoWords3"),
    DataRow(NamespaceSchemeTWOWORDS4, NamespaceSchemeTWOWORDS4Fixed, 10, 1, "two_words3"),
    DataRow(NamespaceSchemeTWOWORDS5, NamespaceSchemeTWOWORDS5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(NamespaceSchemeTWOWORDS6, NamespaceSchemeTWOWORDS6Fixed, 10, 1, "two_Words3"),
    DataRow(NamespaceSchemeTWOWORDS7, NamespaceSchemeTWOWORDS7Fixed, 10, 1, "Two_Words3"),
    DataRow(NamespaceSchemetwoWords2, NamespaceSchemetwoWords2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(NamespaceSchemetwoWords3, NamespaceSchemetwoWords3Fixed, 10, 1, "TwoWords3"),
    DataRow(NamespaceSchemetwoWords4, NamespaceSchemetwoWords4Fixed, 10, 1, "two_words3"),
    DataRow(NamespaceSchemetwoWords5, NamespaceSchemetwoWords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(NamespaceSchemetwoWords6, NamespaceSchemetwoWords6Fixed, 10, 1, "two_Words3"),
    DataRow(NamespaceSchemetwoWords7, NamespaceSchemetwoWords7Fixed, 10, 1, "Two_Words3"),
    DataRow(NamespaceSchemeTwoWords1, NamespaceSchemeTwoWords1Fixed, 10, 1, "twowords3"),
    DataRow(NamespaceSchemeTwoWords3, NamespaceSchemeTwoWords3Fixed, 10, 1, "twoWords3"),
    DataRow(NamespaceSchemeTwoWords4, NamespaceSchemeTwoWords4Fixed, 10, 1, "two_words3"),
    DataRow(NamespaceSchemeTwoWords5, NamespaceSchemeTwoWords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(NamespaceSchemeTwoWords6, NamespaceSchemeTwoWords6Fixed, 10, 1, "two_Words3"),
    DataRow(NamespaceSchemeTwoWords7, NamespaceSchemeTwoWords7Fixed, 10, 1, "Two_Words3"),
    DataRow(NamespaceSchemetwo_words2, NamespaceSchemetwo_words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(NamespaceSchemetwo_words3, NamespaceSchemetwo_words3Fixed, 10, 1, "twoWords3"),
    DataRow(NamespaceSchemetwo_words4, NamespaceSchemetwo_words4Fixed, 10, 1, "TwoWords3"),
    DataRow(NamespaceSchemetwo_words5, NamespaceSchemetwo_words5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(NamespaceSchemetwo_words6, NamespaceSchemetwo_words6Fixed, 10, 1, "two_Words3"),
    DataRow(NamespaceSchemetwo_words7, NamespaceSchemetwo_words7Fixed, 10, 1, "Two_Words3"),
    DataRow(NamespaceSchemeTWO_WORDS1, NamespaceSchemeTWO_WORDS1Fixed, 10, 1, "twowords3"),
    DataRow(NamespaceSchemeTWO_WORDS3, NamespaceSchemeTWO_WORDS3Fixed, 10, 1, "twoWords3"),
    DataRow(NamespaceSchemeTWO_WORDS4, NamespaceSchemeTWO_WORDS4Fixed, 10, 1, "TwoWords3"),
    DataRow(NamespaceSchemeTWO_WORDS5, NamespaceSchemeTWO_WORDS5Fixed, 10, 1, "two_words3"),
    DataRow(NamespaceSchemeTWO_WORDS6, NamespaceSchemeTWO_WORDS6Fixed, 10, 1, "two_Words3"),
    DataRow(NamespaceSchemeTWO_WORDS7, NamespaceSchemeTWO_WORDS7Fixed, 10, 1, "Two_Words3"),
    DataRow(NamespaceSchemetwo_Words2, NamespaceSchemetwo_Words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(NamespaceSchemetwo_Words3, NamespaceSchemetwo_Words3Fixed, 10, 1, "twoWords3"),
    DataRow(NamespaceSchemetwo_Words4, NamespaceSchemetwo_Words4Fixed, 10, 1, "TwoWords3"),
    DataRow(NamespaceSchemetwo_Words5, NamespaceSchemetwo_Words5Fixed, 10, 1, "two_words3"),
    DataRow(NamespaceSchemetwo_Words6, NamespaceSchemetwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(NamespaceSchemetwo_Words7, NamespaceSchemetwo_Words7Fixed, 10, 1, "Two_Words3"),
    DataRow(NamespaceSchemeTwo_Words1, NamespaceSchemeTwo_Words1Fixed, 10, 1, "twowords3"),
    DataRow(NamespaceSchemeTwo_Words2, NamespaceSchemeTwo_Words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(NamespaceSchemeTwo_Words3, NamespaceSchemeTwo_Words3Fixed, 10, 1, "twoWords3"),
    DataRow(NamespaceSchemeTwo_Words4, NamespaceSchemeTwo_Words4Fixed, 10, 1, "TwoWords3"),
    DataRow(NamespaceSchemeTwo_Words5, NamespaceSchemeTwo_Words5Fixed, 10, 1, "two_words3"),
    DataRow(NamespaceSchemeTwo_Words6, NamespaceSchemeTwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(NamespaceSchemeTwo_Words7, NamespaceSchemeTwo_Words7Fixed, 10, 1, "two_Words3"),
    DataRow(MultiNamespaceSchemetwowords1, MultiNamespaceSchemetwowords1Fixed, 10, 1, "TWOWORDS3"),
    DataRow(MultiNamespaceSchemetwowords2, MultiNamespaceSchemetwowords2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(Trivia1, Trivia1Fixed, 10, 1, "TWOWORDS3"),
    DataRow(Trivia2, Trivia2Fixed, 10, 1, "TWOWORDS3"),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1300MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1300)),
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
