namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1314
    {
        private const string OneLocalVariable = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords; }
    }
}
";

        private const string TwoLocalVariables = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; }
    }
}
";

        private const string LocalVariableSchemetwowordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twowords1; int twowords2; int twoWords3; }
    }
}
";

        private const string LocalVariableSchemeTWOWORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWOWORDS1; int TWOWORDS2; int TwoWords3; }
    }
}
";

        private const string LocalVariableSchemetwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int twoWords1; int twoWords2; int twowords; }
    }
}
";

        private const string LocalVariableSchemeTwoWordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TwoWords1; int TwoWords2; int TWOWORDS3; }
    }
}
";

        private const string LocalVariableSchemetwo_wordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_words1; int two_words2; int twowords3; }
    }
}
";

        private const string LocalVariableSchemeTWO_WORDSOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int TWO_WORDS1; int TWO_WORDS2; int TWOWORDS3; }
    }
}
";

        private const string LocalVariableSchemetwo_WordsOk1 = @"
namespace ConsistencyAnalyzer
{
    class Test
    {
        public Test() { int two_Words1; int two_Words2; int twowords3; }
    }
}
";

        [DataTestMethod]
        [
        DataRow(OneLocalVariable),
        DataRow(TwoLocalVariables),
        DataRow(LocalVariableSchemetwowordsOk1),
        DataRow(LocalVariableSchemeTWOWORDSOk1),
        DataRow(LocalVariableSchemetwoWordsOk1),
        DataRow(LocalVariableSchemeTwoWordsOk1),
        DataRow(LocalVariableSchemetwo_wordsOk1),
        DataRow(LocalVariableSchemeTWO_WORDSOk1),
        DataRow(LocalVariableSchemetwo_WordsOk1),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
