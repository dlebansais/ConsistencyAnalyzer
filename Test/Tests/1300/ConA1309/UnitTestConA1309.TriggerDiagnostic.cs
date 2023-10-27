namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1309
{
    private const string FieldSchemetwowords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemetwowords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int twowords3;
    }
}
";

    private const string FieldSchemetwowords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemetwowords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int twowords3;
    }
}
";

    private const string FieldSchemetwowords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int two_words3;
    }
}
";

    private const string FieldSchemetwowords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int twowords3;
    }
}
";

    private const string FieldSchemetwowords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemetwowords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int twowords3;
    }
}
";

    private const string FieldSchemetwowords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemetwowords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int twowords3;
    }
}
";

    private const string FieldSchemetwowords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemetwowords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int twowords3;
    }
}
";

    private const string FieldSchemeTWOWORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int twowords3;
    }
}
";

    private const string FieldSchemeTWOWORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWOWORDS2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemeTWOWORDS2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWOWORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int two_words3;
    }
}
";

    private const string FieldSchemeTWOWORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWOWORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemeTWOWORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWOWORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemeTWOWORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWOWORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemeTWOWORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWOWORDS1;
        public int TWOWORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemetwoWords2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemetwoWords2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int tWOWORDS3;
    }
}
";

    private const string FieldSchemetwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemetwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemetwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int two_words3;
    }
}
";

    private const string FieldSchemetwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemetwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemetwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemetwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemetwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemetwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemetwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twoWords1;
        public int twoWords2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemeTwoWords1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int twowords3;
    }
}
";

    private const string FieldSchemeTwoWords1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int Twowords3;
    }
}
";

    private const string FieldSchemeTwoWords3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemeTwoWords3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemeTwoWords4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int two_words3;
    }
}
";

    private const string FieldSchemeTwoWords4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemeTwoWords5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemeTwoWords5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemeTwoWords6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemeTwoWords6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemeTwoWords7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemeTwoWords7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TwoWords1;
        public int TwoWords2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemetwo_words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemetwo_words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int twowords3;
    }
}
";

    private const string FieldSchemetwo_words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemetwo_words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int two_words3;
    }
}
";

    private const string FieldSchemetwo_words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemetwo_words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int two_words3;
    }
}
";

    private const string FieldSchemetwo_words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemetwo_words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int two_words3;
    }
}
";

    private const string FieldSchemetwo_words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemetwo_words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int two_words3;
    }
}
";

    private const string FieldSchemetwo_words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemetwo_words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_words1;
        public int two_words2;
        public int two_words3;
    }
}
";

    private const string FieldSchemeTWO_WORDS1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int twowords3;
    }
}
";

    private const string FieldSchemeTWO_WORDS1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWO_WORDS3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemeTWO_WORDS3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWO_WORDS4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemeTWO_WORDS4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTWO_WORDS5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int two_words3;
    }
}
";

    private const string FieldSchemeTWO_WORDS5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemeTWO_WORDS6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemeTWO_WORDS6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemeTWO_WORDS7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemeTWO_WORDS7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int TWO_WORDS1;
        public int TWO_WORDS2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemetwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemetwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int twowords3;
    }
}
";

    private const string FieldSchemetwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemetwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemetwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemetwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemetwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int two_words3;
    }
}
";

    private const string FieldSchemetwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemetwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemetwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemetwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemetwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int two_Words1;
        public int two_Words2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemeTwo_Words1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int twowords3;
    }
}
";

    private const string FieldSchemeTwo_Words1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int Twowords3;
    }
}
";

    private const string FieldSchemeTwo_Words2 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int TWOWORDS3;
    }
}
";

    private const string FieldSchemeTwo_Words2Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int Twowords3;
    }
}
";

    private const string FieldSchemeTwo_Words3 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int twoWords3;
    }
}
";

    private const string FieldSchemeTwo_Words3Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemeTwo_Words4 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int TwoWords3;
    }
}
";

    private const string FieldSchemeTwo_Words4Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemeTwo_Words5 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int two_words3;
    }
}
";

    private const string FieldSchemeTwo_Words5Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemeTwo_Words6 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int TWO_WORDS3;
    }
}
";

    private const string FieldSchemeTwo_Words6Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int Two_Words3;
    }
}
";

    private const string FieldSchemeTwo_Words7 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int two_Words3;
    }
}
";

    private const string FieldSchemeTwo_Words7Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int Two_Words1;
        public int Two_Words2;
        public int Two_Words3;
    }
}
";

    private const string Trivia1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int /* */TWOWORDS3/* */;
    }
}
";

    private const string Trivia1Fixed = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public int twowords1;
        public int twowords2;
        public int /* */twowords3/* */;
    }
}
";

    [DataTestMethod]
    [
    DataRow(FieldSchemetwowords1, FieldSchemetwowords1Fixed, 8, 20, "TWOWORDS3"),
    DataRow(FieldSchemetwowords3, FieldSchemetwowords3Fixed, 8, 20, "TwoWords3"),
    DataRow(FieldSchemetwowords4, FieldSchemetwowords4Fixed, 8, 20, "two_words3"),
    DataRow(FieldSchemetwowords5, FieldSchemetwowords5Fixed, 8, 20, "TWO_WORDS3"),
    DataRow(FieldSchemetwowords6, FieldSchemetwowords6Fixed, 8, 20, "two_Words3"),
    DataRow(FieldSchemetwowords7, FieldSchemetwowords7Fixed, 8, 20, "Two_Words3"),
    DataRow(FieldSchemeTWOWORDS1, FieldSchemeTWOWORDS1Fixed, 8, 20, "twowords3"),
    DataRow(FieldSchemeTWOWORDS2, FieldSchemeTWOWORDS2Fixed, 8, 20, "twoWords3"),
    DataRow(FieldSchemeTWOWORDS4, FieldSchemeTWOWORDS4Fixed, 8, 20, "two_words3"),
    DataRow(FieldSchemeTWOWORDS5, FieldSchemeTWOWORDS5Fixed, 8, 20, "TWO_WORDS3"),
    DataRow(FieldSchemeTWOWORDS6, FieldSchemeTWOWORDS6Fixed, 8, 20, "two_Words3"),
    DataRow(FieldSchemeTWOWORDS7, FieldSchemeTWOWORDS7Fixed, 8, 20, "Two_Words3"),
    DataRow(FieldSchemetwoWords2, FieldSchemetwoWords2Fixed, 8, 20, "TWOWORDS3"),
    DataRow(FieldSchemetwoWords3, FieldSchemetwoWords3Fixed, 8, 20, "TwoWords3"),
    DataRow(FieldSchemetwoWords4, FieldSchemetwoWords4Fixed, 8, 20, "two_words3"),
    DataRow(FieldSchemetwoWords5, FieldSchemetwoWords5Fixed, 8, 20, "TWO_WORDS3"),
    DataRow(FieldSchemetwoWords6, FieldSchemetwoWords6Fixed, 8, 20, "two_Words3"),
    DataRow(FieldSchemetwoWords7, FieldSchemetwoWords7Fixed, 8, 20, "Two_Words3"),
    DataRow(FieldSchemeTwoWords1, FieldSchemeTwoWords1Fixed, 8, 20, "twowords3"),
    DataRow(FieldSchemeTwoWords3, FieldSchemeTwoWords3Fixed, 8, 20, "twoWords3"),
    DataRow(FieldSchemeTwoWords4, FieldSchemeTwoWords4Fixed, 8, 20, "two_words3"),
    DataRow(FieldSchemeTwoWords5, FieldSchemeTwoWords5Fixed, 8, 20, "TWO_WORDS3"),
    DataRow(FieldSchemeTwoWords6, FieldSchemeTwoWords6Fixed, 8, 20, "two_Words3"),
    DataRow(FieldSchemeTwoWords7, FieldSchemeTwoWords7Fixed, 8, 20, "Two_Words3"),
    DataRow(FieldSchemetwo_words2, FieldSchemetwo_words2Fixed, 8, 20, "TWOWORDS3"),
    DataRow(FieldSchemetwo_words3, FieldSchemetwo_words3Fixed, 8, 20, "twoWords3"),
    DataRow(FieldSchemetwo_words4, FieldSchemetwo_words4Fixed, 8, 20, "TwoWords3"),
    DataRow(FieldSchemetwo_words5, FieldSchemetwo_words5Fixed, 8, 20, "TWO_WORDS3"),
    DataRow(FieldSchemetwo_words6, FieldSchemetwo_words6Fixed, 8, 20, "two_Words3"),
    DataRow(FieldSchemetwo_words7, FieldSchemetwo_words7Fixed, 8, 20, "Two_Words3"),
    DataRow(FieldSchemeTWO_WORDS1, FieldSchemeTWO_WORDS1Fixed, 8, 20, "twowords3"),
    DataRow(FieldSchemeTWO_WORDS3, FieldSchemeTWO_WORDS3Fixed, 8, 20, "twoWords3"),
    DataRow(FieldSchemeTWO_WORDS4, FieldSchemeTWO_WORDS4Fixed, 8, 20, "TwoWords3"),
    DataRow(FieldSchemeTWO_WORDS5, FieldSchemeTWO_WORDS5Fixed, 8, 20, "two_words3"),
    DataRow(FieldSchemeTWO_WORDS6, FieldSchemeTWO_WORDS6Fixed, 8, 20, "two_Words3"),
    DataRow(FieldSchemeTWO_WORDS7, FieldSchemeTWO_WORDS7Fixed, 8, 20, "Two_Words3"),
    DataRow(FieldSchemetwo_Words2, FieldSchemetwo_Words2Fixed, 8, 20, "TWOWORDS3"),
    DataRow(FieldSchemetwo_Words3, FieldSchemetwo_Words3Fixed, 8, 20, "twoWords3"),
    DataRow(FieldSchemetwo_Words4, FieldSchemetwo_Words4Fixed, 8, 20, "TwoWords3"),
    DataRow(FieldSchemetwo_Words5, FieldSchemetwo_Words5Fixed, 8, 20, "two_words3"),
    DataRow(FieldSchemetwo_Words6, FieldSchemetwo_Words6Fixed, 8, 20, "TWO_WORDS3"),
    DataRow(FieldSchemetwo_Words7, FieldSchemetwo_Words7Fixed, 8, 20, "Two_Words3"),
    DataRow(FieldSchemeTwo_Words1, FieldSchemeTwo_Words1Fixed, 8, 20, "twowords3"),
    DataRow(FieldSchemeTwo_Words2, FieldSchemeTwo_Words2Fixed, 8, 20, "TWOWORDS3"),
    DataRow(FieldSchemeTwo_Words3, FieldSchemeTwo_Words3Fixed, 8, 20, "twoWords3"),
    DataRow(FieldSchemeTwo_Words4, FieldSchemeTwo_Words4Fixed, 8, 20, "TwoWords3"),
    DataRow(FieldSchemeTwo_Words5, FieldSchemeTwo_Words5Fixed, 8, 20, "two_words3"),
    DataRow(FieldSchemeTwo_Words6, FieldSchemeTwo_Words6Fixed, 8, 20, "TWO_WORDS3"),
    DataRow(FieldSchemeTwo_Words7, FieldSchemeTwo_Words7Fixed, 8, 20, "two_Words3"),
    DataRow(Trivia1, Trivia1Fixed, 8, 25, "TWOWORDS3"),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1309MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1309)),
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
