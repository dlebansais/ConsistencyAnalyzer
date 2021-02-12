namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1300
    {
        private const string OneNamespace = @"
namespace twowords
{
}
";

        private const string TwoNamespaces = @"
namespace twowords1
{
}

namespace twowords2
{
}
";

        private const string NamespaceSchemetwowordsOk1 = @"
namespace twowords1
{
}

namespace twowords2
{
}

namespace twoWords3
{
}
";

        private const string NamespaceSchemeTWOWORDSOk1 = @"
namespace TWOWORDS1
{
}

namespace TWOWORDS2
{
}

namespace TwoWords3
{
}
";

        private const string NamespaceSchemetwoWordsOk1 = @"
namespace twoWords1
{
}

namespace twoWords2
{
}

namespace twowords
{
}
";

        private const string NamespaceSchemeTwoWordsOk1 = @"
namespace TwoWords1
{
}

namespace TwoWords2
{
}

namespace TWOWORDS3
{
}
";

        private const string NamespaceSchemetwo_wordsOk1 = @"
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

        private const string NamespaceSchemeTWO_WORDSOk1 = @"
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

        private const string NamespaceSchemetwo_WordsOk1 = @"
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

        private const string OneNamespaceMulti1 = @"
namespace twowords.twowords
{
}
";

        private const string TwoNamespacesMulti1 = @"
namespace twowords.twowords1
{
}

namespace twowords.twowords2
{
}
";

        private const string OneNamespaceMulti2 = @"
namespace twowords.twowords.twowords
{
}
";

        private const string TwoNamespacesMulti2 = @"
namespace twowords.twowords1.twowords1
{
}

namespace twowords.twowords2.twowords2
{
}
";

        [DataTestMethod]
        [
        DataRow(OneNamespace),
        DataRow(TwoNamespaces),
        DataRow(NamespaceSchemetwowordsOk1),
        DataRow(NamespaceSchemeTWOWORDSOk1),
        DataRow(NamespaceSchemetwoWordsOk1),
        DataRow(NamespaceSchemeTwoWordsOk1),
        DataRow(NamespaceSchemetwo_wordsOk1),
        DataRow(NamespaceSchemeTWO_WORDSOk1),
        DataRow(NamespaceSchemetwo_WordsOk1),
        DataRow(OneNamespaceMulti1),
        DataRow(TwoNamespacesMulti1),
        DataRow(OneNamespaceMulti2),
        DataRow(TwoNamespacesMulti2),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
