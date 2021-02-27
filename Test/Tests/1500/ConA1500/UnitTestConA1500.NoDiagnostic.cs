namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1500
    {

        private const string Namespace = @"
namespace ConsistencyAnalyzer
{
}
";

        private const string UsingOutside = @"
using System;

namespace ConsistencyAnalyzer
{
}
";

        private const string UsingInside = @"
namespace ConsistencyAnalyzer
{
    using System;
}
";

        private const string EnumDeclaration = @"
namespace ConsistencyAnalyzer
{
    public enum CodeFixes
    {
    }
}
";

        private const string ClassDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
    }
}
";

        private const string StructDeclaration = @"
namespace ConsistencyAnalyzer
{
    public struct CodeFixes
    {
    }
}
";

        private const string InterfaceDeclaration = @"
namespace ConsistencyAnalyzer
{
    public interface ICodeFixes
    {
    }
}
";

        private const string RecordDeclaration = @"
namespace ConsistencyAnalyzer
{
    public record CodeFixes
    {
    }
}
";

        private const string DelegateDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public delegate void Test();
    }
}
";

        private const string EventDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public event System.EventHandler Test;
    }
}
";

        private const string FieldDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test;
    }
}
";

        private const string MethodDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() {}
    }
}
";

        private const string PropertyDeclaration = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test { get; set; }
    }
}
";

        private const string EnumMemberDeclaration = @"
namespace ConsistencyAnalyzer
{
    public enum CodeFixes
    {
        Analyzer,
    }
}
";

        private const string ForBreakContinueStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                continue;
                break;
            }
        }
    }
}
";

        private const string ForBreakContinueStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                {
                    continue;
                    break;
                }
            }
        }
    }
}
";

        private const string ForBreakContinueStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            {
                for (int i = 0; i < 1; i++)
                {
                    continue;
                    break;
                }
            }
        }
    }
}
";

        private const string ForBreakContinueStatement4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
                for (int j = 0; j < 1; j++)
                {
                    continue;
                    break;
                }
        }
    }
}
";

        private const string CheckedStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            checked
            {
            }
        }
    }
}
";

        private const string ForEachStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            foreach (int Item in new int[] { 0 })
            {
            }
        }
    }
}
";

        private const string DoStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do
            {
            }
            while (true);
        }
    }
}
";

        private const string FixedStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        unsafe public void Test() 
        {
            Point Point = new Point();
            fixed (int* p = &Point.x)
            {
            }
        }
    }

    public class Point
    {
        public int x;
        public int y;
    }
}
";

        private const string IfElseStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true)
            {
            }
            else
            {
            }
        }
    }
}
";

        private const string LocalDeclarationStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            int i = 0;
        }
    }
}
";

        private const string LockStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            int[] x = new int[0];
            lock (x)
            {
            }
        }
    }
}
";

        private const string ReturnStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test() 
        {
            return 0;
        }
    }
}
";

        private const string SwitchBreakStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i)
            {
                case 0:
                    break;
                default:
                    break;
            }
        }
    }
}
";

        private const string ThrowStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            throw new System.Exception(""msg"");
        }
    }
}
";

        private const string TryCatchStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try
            {
                return;
            }
            catch
            {
                return;
            }
        }
    }
}
";

        private const string UnsafeStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            unsafe
            {
            }
        }
    }
}
";

        private const string UsingStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(string.Empty, System.IO.FileMode.Open))
            {
            }
        }
    }
}
";

        private const string WhileStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            while (true)
            {
            }
        }
    }
}
";

        private const string YieldBreakStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public System.Collections.Generic.IEnumerable<int> Test()
        {
            for (int i = 0; i < 1; i++)
            {
                yield break;
            }
        }
    }
}
";

        private const string YieldReturnStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public System.Collections.Generic.IEnumerable<int> Test()
        {
            for (int i = 0; i < 1; i++)
            {
                yield return i;
            }
        }
    }
}
";

        [DataTestMethod]
        [
        DataRow(Namespace),
        DataRow(UsingOutside),
        DataRow(UsingInside),
        DataRow(EnumDeclaration),
        DataRow(ClassDeclaration),
        DataRow(StructDeclaration),
        DataRow(RecordDeclaration),
        DataRow(InterfaceDeclaration),
        DataRow(DelegateDeclaration),
        DataRow(EventDeclaration),
        DataRow(FieldDeclaration),
        DataRow(MethodDeclaration),
        DataRow(PropertyDeclaration),
        DataRow(EnumMemberDeclaration),
        DataRow(ForBreakContinueStatement1),
        DataRow(ForBreakContinueStatement2),
        DataRow(ForBreakContinueStatement3),
        DataRow(ForBreakContinueStatement4),
        DataRow(CheckedStatement),
        DataRow(ForEachStatement),
        DataRow(DoStatement),
        DataRow(FixedStatement),
        DataRow(IfElseStatement),
        DataRow(LocalDeclarationStatement),
        DataRow(LockStatement),
        DataRow(ReturnStatement),
        DataRow(SwitchBreakStatement),
        DataRow(ThrowStatement),
        DataRow(TryCatchStatement),
        DataRow(UsingStatement),
        DataRow(WhileStatement),
        DataRow(YieldBreakStatement),
        DataRow(YieldReturnStatement),
        ]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            Task result = VerifyCS.VerifyAnalyzerAsync(testCode);
            result.Wait();
        }
    }
}
