namespace ConsistencyAnalyzer.Test;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

public partial class UnitTestConA1305
{
    private const string InterfaceSchemetwowords1 = @"
interface twowords1
{
}

interface twowords2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemetwowords1Fixed = @"
interface twowords1
{
}

interface twowords2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemetwowords3 = @"
interface twowords1
{
}

interface twowords2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemetwowords3Fixed = @"
interface twowords1
{
}

interface twowords2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemetwowords4 = @"
interface twowords1
{
}

interface twowords2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemetwowords4Fixed = @"
interface twowords1
{
}

interface twowords2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemetwowords5 = @"
interface twowords1
{
}

interface twowords2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemetwowords5Fixed = @"
interface twowords1
{
}

interface twowords2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemetwowords6 = @"
interface twowords1
{
}

interface twowords2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemetwowords6Fixed = @"
interface twowords1
{
}

interface twowords2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemetwowords7 = @"
interface twowords1
{
}

interface twowords2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemetwowords7Fixed = @"
interface twowords1
{
}

interface twowords2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemeTWOWORDS1 = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemeTWOWORDS1Fixed = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWOWORDS2 = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemeTWOWORDS2Fixed = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWOWORDS4 = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemeTWOWORDS4Fixed = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWOWORDS5 = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemeTWOWORDS5Fixed = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWOWORDS6 = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemeTWOWORDS6Fixed = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWOWORDS7 = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemeTWOWORDS7Fixed = @"
interface TWOWORDS1
{
}

interface TWOWORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemetwoWords2 = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemetwoWords2Fixed = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface tWOWORDS3
{
}
";

    private const string InterfaceSchemetwoWords3 = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemetwoWords3Fixed = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemetwoWords4 = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemetwoWords4Fixed = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemetwoWords5 = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemetwoWords5Fixed = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemetwoWords6 = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemetwoWords6Fixed = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemetwoWords7 = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemetwoWords7Fixed = @"
interface twoWords1
{
}

interface twoWords2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemeTwoWords1 = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemeTwoWords1Fixed = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface Twowords3
{
}
";

    private const string InterfaceSchemeTwoWords3 = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemeTwoWords3Fixed = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemeTwoWords4 = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemeTwoWords4Fixed = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemeTwoWords5 = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemeTwoWords5Fixed = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemeTwoWords6 = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemeTwoWords6Fixed = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemeTwoWords7 = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemeTwoWords7Fixed = @"
interface TwoWords1
{
}

interface TwoWords2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemetwo_words2 = @"
interface two_words1
{
}

interface two_words2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemetwo_words2Fixed = @"
interface two_words1
{
}

interface two_words2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemetwo_words3 = @"
interface two_words1
{
}

interface two_words2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemetwo_words3Fixed = @"
interface two_words1
{
}

interface two_words2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemetwo_words4 = @"
interface two_words1
{
}

interface two_words2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemetwo_words4Fixed = @"
interface two_words1
{
}

interface two_words2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemetwo_words5 = @"
interface two_words1
{
}

interface two_words2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemetwo_words5Fixed = @"
interface two_words1
{
}

interface two_words2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemetwo_words6 = @"
interface two_words1
{
}

interface two_words2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemetwo_words6Fixed = @"
interface two_words1
{
}

interface two_words2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemetwo_words7 = @"
interface two_words1
{
}

interface two_words2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemetwo_words7Fixed = @"
interface two_words1
{
}

interface two_words2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemeTWO_WORDS1 = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemeTWO_WORDS1Fixed = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWO_WORDS3 = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemeTWO_WORDS3Fixed = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWO_WORDS4 = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemeTWO_WORDS4Fixed = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTWO_WORDS5 = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemeTWO_WORDS5Fixed = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemeTWO_WORDS6 = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemeTWO_WORDS6Fixed = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemeTWO_WORDS7 = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemeTWO_WORDS7Fixed = @"
interface TWO_WORDS1
{
}

interface TWO_WORDS2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemetwo_Words2 = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemetwo_Words2Fixed = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemetwo_Words3 = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemetwo_Words3Fixed = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemetwo_Words4 = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemetwo_Words4Fixed = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemetwo_Words5 = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemetwo_Words5Fixed = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemetwo_Words6 = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemetwo_Words6Fixed = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemetwo_Words7 = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemetwo_Words7Fixed = @"
interface two_Words1
{
}

interface two_Words2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemeTwo_Words1 = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface twowords3
{
}
";

    private const string InterfaceSchemeTwo_Words1Fixed = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface Twowords3
{
}
";

    private const string InterfaceSchemeTwo_Words2 = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface TWOWORDS3
{
}
";

    private const string InterfaceSchemeTwo_Words2Fixed = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface Twowords3
{
}
";

    private const string InterfaceSchemeTwo_Words3 = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface twoWords3
{
}
";

    private const string InterfaceSchemeTwo_Words3Fixed = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemeTwo_Words4 = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface TwoWords3
{
}
";

    private const string InterfaceSchemeTwo_Words4Fixed = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemeTwo_Words5 = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface two_words3
{
}
";

    private const string InterfaceSchemeTwo_Words5Fixed = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemeTwo_Words6 = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface TWO_WORDS3
{
}
";

    private const string InterfaceSchemeTwo_Words6Fixed = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface Two_Words3
{
}
";

    private const string InterfaceSchemeTwo_Words7 = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface two_Words3
{
}
";

    private const string InterfaceSchemeTwo_Words7Fixed = @"
interface Two_Words1
{
}

interface Two_Words2
{
}

interface Two_Words3
{
}
";

    private const string Trivia1 = @"
interface twowords1
{
}

interface twowords2
{
}

interface /* */TWOWORDS3/* */
{
}
";

    private const string Trivia1Fixed = @"
interface twowords1
{
}

interface twowords2
{
}

interface /* */twowords3/* */
{
}
";

    private const string InterfaceSchemeItwowords1 = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeItwowords1Fixed = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeItwowords3 = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeItwowords3Fixed = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeItwowords4 = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeItwowords4Fixed = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeItwowords5 = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeItwowords5Fixed = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeItwowords6 = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeItwowords6Fixed = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeItwowords7 = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeItwowords7Fixed = @"
interface Itwowords1
{
}

interface Itwowords2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeITWOWORDS1 = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeITWOWORDS1Fixed = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWOWORDS2 = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeITWOWORDS2Fixed = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWOWORDS4 = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeITWOWORDS4Fixed = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWOWORDS5 = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeITWOWORDS5Fixed = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWOWORDS6 = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeITWOWORDS6Fixed = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWOWORDS7 = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeITWOWORDS7Fixed = @"
interface ITWOWORDS1
{
}

interface ITWOWORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeItwoWords2 = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeItwoWords2Fixed = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ItWOWORDS3
{
}
";

    private const string InterfaceSchemeItwoWords3 = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeItwoWords3Fixed = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeItwoWords4 = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeItwoWords4Fixed = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeItwoWords5 = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeItwoWords5Fixed = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeItwoWords6 = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeItwoWords6Fixed = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeItwoWords7 = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeItwoWords7Fixed = @"
interface ItwoWords1
{
}

interface ItwoWords2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeITwoWords1 = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeITwoWords1Fixed = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITwowords3
{
}
";

    private const string InterfaceSchemeITwoWords3 = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeITwoWords3Fixed = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeITwoWords4 = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeITwoWords4Fixed = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeITwoWords5 = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeITwoWords5Fixed = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeITwoWords6 = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeITwoWords6Fixed = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeITwoWords7 = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeITwoWords7Fixed = @"
interface ITwoWords1
{
}

interface ITwoWords2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeItwo_words2 = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeItwo_words2Fixed = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeItwo_words3 = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeItwo_words3Fixed = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeItwo_words4 = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeItwo_words4Fixed = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeItwo_words5 = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeItwo_words5Fixed = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeItwo_words6 = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeItwo_words6Fixed = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeItwo_words7 = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeItwo_words7Fixed = @"
interface Itwo_words1
{
}

interface Itwo_words2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeITWO_WORDS1 = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeITWO_WORDS1Fixed = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWO_WORDS3 = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeITWO_WORDS3Fixed = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWO_WORDS4 = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeITWO_WORDS4Fixed = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITWO_WORDS5 = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeITWO_WORDS5Fixed = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeITWO_WORDS6 = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeITWO_WORDS6Fixed = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeITWO_WORDS7 = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeITWO_WORDS7Fixed = @"
interface ITWO_WORDS1
{
}

interface ITWO_WORDS2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeItwo_Words2 = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeItwo_Words2Fixed = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeItwo_Words3 = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeItwo_Words3Fixed = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeItwo_Words4 = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeItwo_Words4Fixed = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeItwo_Words5 = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeItwo_Words5Fixed = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeItwo_Words6 = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeItwo_Words6Fixed = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeItwo_Words7 = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeItwo_Words7Fixed = @"
interface Itwo_Words1
{
}

interface Itwo_Words2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeITwo_Words1 = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface Itwowords3
{
}
";

    private const string InterfaceSchemeITwo_Words1Fixed = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwowords3
{
}
";

    private const string InterfaceSchemeITwo_Words2 = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITWOWORDS3
{
}
";

    private const string InterfaceSchemeITwo_Words2Fixed = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwowords3
{
}
";

    private const string InterfaceSchemeITwo_Words3 = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ItwoWords3
{
}
";

    private const string InterfaceSchemeITwo_Words3Fixed = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeITwo_Words4 = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwoWords3
{
}
";

    private const string InterfaceSchemeITwo_Words4Fixed = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeITwo_Words5 = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface Itwo_words3
{
}
";

    private const string InterfaceSchemeITwo_Words5Fixed = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeITwo_Words6 = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITWO_WORDS3
{
}
";

    private const string InterfaceSchemeITwo_Words6Fixed = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwo_Words3
{
}
";

    private const string InterfaceSchemeITwo_Words7 = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface Itwo_Words3
{
}
";

    private const string InterfaceSchemeITwo_Words7Fixed = @"
interface ITwo_Words1
{
}

interface ITwo_Words2
{
}

interface ITwo_Words3
{
}
";

    [DataTestMethod]
    [
    DataRow(InterfaceSchemetwowords1, InterfaceSchemetwowords1Fixed, 10, 1, "TWOWORDS3"),
    DataRow(InterfaceSchemetwowords3, InterfaceSchemetwowords3Fixed, 10, 1, "TwoWords3"),
    DataRow(InterfaceSchemetwowords4, InterfaceSchemetwowords4Fixed, 10, 1, "two_words3"),
    DataRow(InterfaceSchemetwowords5, InterfaceSchemetwowords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(InterfaceSchemetwowords6, InterfaceSchemetwowords6Fixed, 10, 1, "two_Words3"),
    DataRow(InterfaceSchemetwowords7, InterfaceSchemetwowords7Fixed, 10, 1, "Two_Words3"),
    DataRow(InterfaceSchemeTWOWORDS1, InterfaceSchemeTWOWORDS1Fixed, 10, 1, "twowords3"),
    DataRow(InterfaceSchemeTWOWORDS2, InterfaceSchemeTWOWORDS2Fixed, 10, 1, "twoWords3"),
    DataRow(InterfaceSchemeTWOWORDS4, InterfaceSchemeTWOWORDS4Fixed, 10, 1, "two_words3"),
    DataRow(InterfaceSchemeTWOWORDS5, InterfaceSchemeTWOWORDS5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(InterfaceSchemeTWOWORDS6, InterfaceSchemeTWOWORDS6Fixed, 10, 1, "two_Words3"),
    DataRow(InterfaceSchemeTWOWORDS7, InterfaceSchemeTWOWORDS7Fixed, 10, 1, "Two_Words3"),
    DataRow(InterfaceSchemetwoWords2, InterfaceSchemetwoWords2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(InterfaceSchemetwoWords3, InterfaceSchemetwoWords3Fixed, 10, 1, "TwoWords3"),
    DataRow(InterfaceSchemetwoWords4, InterfaceSchemetwoWords4Fixed, 10, 1, "two_words3"),
    DataRow(InterfaceSchemetwoWords5, InterfaceSchemetwoWords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(InterfaceSchemetwoWords6, InterfaceSchemetwoWords6Fixed, 10, 1, "two_Words3"),
    DataRow(InterfaceSchemetwoWords7, InterfaceSchemetwoWords7Fixed, 10, 1, "Two_Words3"),
    DataRow(InterfaceSchemeTwoWords1, InterfaceSchemeTwoWords1Fixed, 10, 1, "twowords3"),
    DataRow(InterfaceSchemeTwoWords3, InterfaceSchemeTwoWords3Fixed, 10, 1, "twoWords3"),
    DataRow(InterfaceSchemeTwoWords4, InterfaceSchemeTwoWords4Fixed, 10, 1, "two_words3"),
    DataRow(InterfaceSchemeTwoWords5, InterfaceSchemeTwoWords5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(InterfaceSchemeTwoWords6, InterfaceSchemeTwoWords6Fixed, 10, 1, "two_Words3"),
    DataRow(InterfaceSchemeTwoWords7, InterfaceSchemeTwoWords7Fixed, 10, 1, "Two_Words3"),
    DataRow(InterfaceSchemetwo_words2, InterfaceSchemetwo_words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(InterfaceSchemetwo_words3, InterfaceSchemetwo_words3Fixed, 10, 1, "twoWords3"),
    DataRow(InterfaceSchemetwo_words4, InterfaceSchemetwo_words4Fixed, 10, 1, "TwoWords3"),
    DataRow(InterfaceSchemetwo_words5, InterfaceSchemetwo_words5Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(InterfaceSchemetwo_words6, InterfaceSchemetwo_words6Fixed, 10, 1, "two_Words3"),
    DataRow(InterfaceSchemetwo_words7, InterfaceSchemetwo_words7Fixed, 10, 1, "Two_Words3"),
    DataRow(InterfaceSchemeTWO_WORDS1, InterfaceSchemeTWO_WORDS1Fixed, 10, 1, "twowords3"),
    DataRow(InterfaceSchemeTWO_WORDS3, InterfaceSchemeTWO_WORDS3Fixed, 10, 1, "twoWords3"),
    DataRow(InterfaceSchemeTWO_WORDS4, InterfaceSchemeTWO_WORDS4Fixed, 10, 1, "TwoWords3"),
    DataRow(InterfaceSchemeTWO_WORDS5, InterfaceSchemeTWO_WORDS5Fixed, 10, 1, "two_words3"),
    DataRow(InterfaceSchemeTWO_WORDS6, InterfaceSchemeTWO_WORDS6Fixed, 10, 1, "two_Words3"),
    DataRow(InterfaceSchemeTWO_WORDS7, InterfaceSchemeTWO_WORDS7Fixed, 10, 1, "Two_Words3"),
    DataRow(InterfaceSchemetwo_Words2, InterfaceSchemetwo_Words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(InterfaceSchemetwo_Words3, InterfaceSchemetwo_Words3Fixed, 10, 1, "twoWords3"),
    DataRow(InterfaceSchemetwo_Words4, InterfaceSchemetwo_Words4Fixed, 10, 1, "TwoWords3"),
    DataRow(InterfaceSchemetwo_Words5, InterfaceSchemetwo_Words5Fixed, 10, 1, "two_words3"),
    DataRow(InterfaceSchemetwo_Words6, InterfaceSchemetwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(InterfaceSchemetwo_Words7, InterfaceSchemetwo_Words7Fixed, 10, 1, "Two_Words3"),
    DataRow(InterfaceSchemeTwo_Words1, InterfaceSchemeTwo_Words1Fixed, 10, 1, "twowords3"),
    DataRow(InterfaceSchemeTwo_Words2, InterfaceSchemeTwo_Words2Fixed, 10, 1, "TWOWORDS3"),
    DataRow(InterfaceSchemeTwo_Words3, InterfaceSchemeTwo_Words3Fixed, 10, 1, "twoWords3"),
    DataRow(InterfaceSchemeTwo_Words4, InterfaceSchemeTwo_Words4Fixed, 10, 1, "TwoWords3"),
    DataRow(InterfaceSchemeTwo_Words5, InterfaceSchemeTwo_Words5Fixed, 10, 1, "two_words3"),
    DataRow(InterfaceSchemeTwo_Words6, InterfaceSchemeTwo_Words6Fixed, 10, 1, "TWO_WORDS3"),
    DataRow(InterfaceSchemeTwo_Words7, InterfaceSchemeTwo_Words7Fixed, 10, 1, "two_Words3"),
    DataRow(Trivia1, Trivia1Fixed, 10, 1, "TWOWORDS3"),
    DataRow(InterfaceSchemeItwowords1, InterfaceSchemeItwowords1Fixed, 10, 1, "ITWOWORDS3"),
    DataRow(InterfaceSchemeItwowords3, InterfaceSchemeItwowords3Fixed, 10, 1, "ITwoWords3"),
    DataRow(InterfaceSchemeItwowords4, InterfaceSchemeItwowords4Fixed, 10, 1, "Itwo_words3"),
    DataRow(InterfaceSchemeItwowords5, InterfaceSchemeItwowords5Fixed, 10, 1, "ITWO_WORDS3"),
    DataRow(InterfaceSchemeItwowords6, InterfaceSchemeItwowords6Fixed, 10, 1, "Itwo_Words3"),
    DataRow(InterfaceSchemeItwowords7, InterfaceSchemeItwowords7Fixed, 10, 1, "ITwo_Words3"),
    DataRow(InterfaceSchemeITWOWORDS1, InterfaceSchemeITWOWORDS1Fixed, 10, 1, "Itwowords3"),
    DataRow(InterfaceSchemeITWOWORDS2, InterfaceSchemeITWOWORDS2Fixed, 10, 1, "ItwoWords3"),
    DataRow(InterfaceSchemeITWOWORDS4, InterfaceSchemeITWOWORDS4Fixed, 10, 1, "Itwo_words3"),
    DataRow(InterfaceSchemeITWOWORDS5, InterfaceSchemeITWOWORDS5Fixed, 10, 1, "ITWO_WORDS3"),
    DataRow(InterfaceSchemeITWOWORDS6, InterfaceSchemeITWOWORDS6Fixed, 10, 1, "Itwo_Words3"),
    DataRow(InterfaceSchemeITWOWORDS7, InterfaceSchemeITWOWORDS7Fixed, 10, 1, "ITwo_Words3"),
    DataRow(InterfaceSchemeItwoWords2, InterfaceSchemeItwoWords2Fixed, 10, 1, "ITWOWORDS3"),
    DataRow(InterfaceSchemeItwoWords3, InterfaceSchemeItwoWords3Fixed, 10, 1, "ITwoWords3"),
    DataRow(InterfaceSchemeItwoWords4, InterfaceSchemeItwoWords4Fixed, 10, 1, "Itwo_words3"),
    DataRow(InterfaceSchemeItwoWords5, InterfaceSchemeItwoWords5Fixed, 10, 1, "ITWO_WORDS3"),
    DataRow(InterfaceSchemeItwoWords6, InterfaceSchemeItwoWords6Fixed, 10, 1, "Itwo_Words3"),
    DataRow(InterfaceSchemeItwoWords7, InterfaceSchemeItwoWords7Fixed, 10, 1, "ITwo_Words3"),
    DataRow(InterfaceSchemeITwoWords1, InterfaceSchemeITwoWords1Fixed, 10, 1, "Itwowords3"),
    DataRow(InterfaceSchemeITwoWords3, InterfaceSchemeITwoWords3Fixed, 10, 1, "ItwoWords3"),
    DataRow(InterfaceSchemeITwoWords4, InterfaceSchemeITwoWords4Fixed, 10, 1, "Itwo_words3"),
    DataRow(InterfaceSchemeITwoWords5, InterfaceSchemeITwoWords5Fixed, 10, 1, "ITWO_WORDS3"),
    DataRow(InterfaceSchemeITwoWords6, InterfaceSchemeITwoWords6Fixed, 10, 1, "Itwo_Words3"),
    DataRow(InterfaceSchemeITwoWords7, InterfaceSchemeITwoWords7Fixed, 10, 1, "ITwo_Words3"),
    DataRow(InterfaceSchemeItwo_words2, InterfaceSchemeItwo_words2Fixed, 10, 1, "ITWOWORDS3"),
    DataRow(InterfaceSchemeItwo_words3, InterfaceSchemeItwo_words3Fixed, 10, 1, "ItwoWords3"),
    DataRow(InterfaceSchemeItwo_words4, InterfaceSchemeItwo_words4Fixed, 10, 1, "ITwoWords3"),
    DataRow(InterfaceSchemeItwo_words5, InterfaceSchemeItwo_words5Fixed, 10, 1, "ITWO_WORDS3"),
    DataRow(InterfaceSchemeItwo_words6, InterfaceSchemeItwo_words6Fixed, 10, 1, "Itwo_Words3"),
    DataRow(InterfaceSchemeItwo_words7, InterfaceSchemeItwo_words7Fixed, 10, 1, "ITwo_Words3"),
    DataRow(InterfaceSchemeITWO_WORDS1, InterfaceSchemeITWO_WORDS1Fixed, 10, 1, "Itwowords3"),
    DataRow(InterfaceSchemeITWO_WORDS3, InterfaceSchemeITWO_WORDS3Fixed, 10, 1, "ItwoWords3"),
    DataRow(InterfaceSchemeITWO_WORDS4, InterfaceSchemeITWO_WORDS4Fixed, 10, 1, "ITwoWords3"),
    DataRow(InterfaceSchemeITWO_WORDS5, InterfaceSchemeITWO_WORDS5Fixed, 10, 1, "Itwo_words3"),
    DataRow(InterfaceSchemeITWO_WORDS6, InterfaceSchemeITWO_WORDS6Fixed, 10, 1, "Itwo_Words3"),
    DataRow(InterfaceSchemeITWO_WORDS7, InterfaceSchemeITWO_WORDS7Fixed, 10, 1, "ITwo_Words3"),
    DataRow(InterfaceSchemeItwo_Words2, InterfaceSchemeItwo_Words2Fixed, 10, 1, "ITWOWORDS3"),
    DataRow(InterfaceSchemeItwo_Words3, InterfaceSchemeItwo_Words3Fixed, 10, 1, "ItwoWords3"),
    DataRow(InterfaceSchemeItwo_Words4, InterfaceSchemeItwo_Words4Fixed, 10, 1, "ITwoWords3"),
    DataRow(InterfaceSchemeItwo_Words5, InterfaceSchemeItwo_Words5Fixed, 10, 1, "Itwo_words3"),
    DataRow(InterfaceSchemeItwo_Words6, InterfaceSchemeItwo_Words6Fixed, 10, 1, "ITWO_WORDS3"),
    DataRow(InterfaceSchemeItwo_Words7, InterfaceSchemeItwo_Words7Fixed, 10, 1, "ITwo_Words3"),
    DataRow(InterfaceSchemeITwo_Words1, InterfaceSchemeITwo_Words1Fixed, 10, 1, "Itwowords3"),
    DataRow(InterfaceSchemeITwo_Words2, InterfaceSchemeITwo_Words2Fixed, 10, 1, "ITWOWORDS3"),
    DataRow(InterfaceSchemeITwo_Words3, InterfaceSchemeITwo_Words3Fixed, 10, 1, "ItwoWords3"),
    DataRow(InterfaceSchemeITwo_Words4, InterfaceSchemeITwo_Words4Fixed, 10, 1, "ITwoWords3"),
    DataRow(InterfaceSchemeITwo_Words5, InterfaceSchemeITwo_Words5Fixed, 10, 1, "Itwo_words3"),
    DataRow(InterfaceSchemeITwo_Words6, InterfaceSchemeITwo_Words6Fixed, 10, 1, "ITWO_WORDS3"),
    DataRow(InterfaceSchemeITwo_Words7, InterfaceSchemeITwo_Words7Fixed, 10, 1, "Itwo_Words3"),
    ]
    public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column, string badName)
    {
        string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1305MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
        string FormatedMessage = string.Format(AnalyzerMessageFormat, badName);

        var descriptor = new DiagnosticDescriptor(
            AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1305)),
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
