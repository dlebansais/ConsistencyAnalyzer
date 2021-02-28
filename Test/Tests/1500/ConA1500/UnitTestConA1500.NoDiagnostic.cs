namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1500
    {

        private const string Namespace1 = @"
namespace ConsistencyAnalyzer { }
";

        private const string Namespace2 = @"
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

        private const string EnumDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public enum CodeFixes { }
}
";

        private const string EnumDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public enum CodeFixes
    {
    }
}
";

        private const string ClassDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes { }
}
";

        private const string ClassDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
    }
}
";

        private const string StructDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public struct CodeFixes { }
}
";

        private const string StructDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public struct CodeFixes
    {
    }
}
";

        private const string RecordDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public record CodeFixes { }
}
";

        private const string RecordDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public record CodeFixes
    {
    }
}
";

        private const string InterfaceDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public interface ICodeFixes { }
}
";

        private const string InterfaceDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public interface ICodeFixes
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

        private const string MethodDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() {}
    }
}
";

        private const string MethodDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test()
        {
        }
    }
}
";

        private const string PropertyDeclaration1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test { get; set; }
    }
}
";

        private const string PropertyDeclaration2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test
        {
            get;
            set;
        }
    }
}
";

        private const string PropertyDeclaration3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test
        {
            get { return 0; }
            set { }
        }
    }
}
";

        private const string PropertyDeclaration4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test
        {
            get 
            {
                return 0;
            }
            set
            {
            }
        }
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

        private const string ForStatement = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++) { }
        }
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

        private const string CheckedStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            checked { }
        }
    }
}
";

        private const string CheckedStatement2 = @"
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

        private const string ForEachStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            foreach (int Item in new int[] { 0 }) { }
        }
    }
}
";

        private const string ForEachStatement2 = @"
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

        private const string DoStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do { } while (true);
        }
    }
}
";

        private const string DoStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do { }
            while (true);
        }
    }
}
";

        private const string DoStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            do
            {
            } while (true);
        }
    }
}
";

        private const string DoStatement4 = @"
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

        private const string FixedStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        unsafe public void Test() 
        {
            Point Point = new Point();
            fixed (int* p = &Point.x) { }
        }
    }

    public class Point
    {
        public int x;
        public int y;
    }
}
";

        private const string FixedStatement2 = @"
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

        private const string IfElseStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true) { } else { }
        }
    }
}
";

        private const string IfElseStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true)
            {
            } else { }
        }
    }
}
";

        private const string IfElseStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true) { }
            else { }
        }
    }
}
";

        private const string IfElseStatement4 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            if (true) { }
            else
            {
            }
        }
    }
}
";

        private const string IfElseStatement5 = @"
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

        private const string LockStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            int[] x = new int[0];
            lock (x) { }
        }
    }
}
";

        private const string LockStatement2 = @"
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

        private const string SwitchBreakStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i) { case 0: break; default: break; }
        }
    }
}
";

        private const string SwitchBreakStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i)
            { case 0: break; default: break;
            }
        }
    }
}
";

        private const string SwitchBreakStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test(int i)
        {
            switch (i)
            {
                case 0: break;
                default: break;
            }
        }
    }
}
";

        private const string SwitchBreakStatement4 = @"
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

        private const string TryCatchStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try { return; } catch { return; }
        }
    }
}
";

        private const string TryCatchStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try
            {
                return;
            } catch { return; }
        }
    }
}
";

        private const string TryCatchStatement3 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            try { return; }
            catch
            {
                return;
            }
        }
    }
}
";

        private const string TryCatchStatement4 = @"
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
            { return; }
        }
    }
}
";

        private const string TryCatchStatement5 = @"
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

        private const string UnsafeStatement1 = @"
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

        private const string UnsafeStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            unsafe { }
        }
    }
}
";

        private const string UsingStatement1 = @"
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

        private const string UsingStatement2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(string.Empty, System.IO.FileMode.Open)) { }
        }
    }
}
";

        private const string WhileStatement1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            while (true) { }
        }
    }
}
";

        private const string WhileStatement2 = @"
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
        DataRow(Namespace1),
        DataRow(Namespace2),
        DataRow(UsingOutside),
        DataRow(UsingInside),
        DataRow(EnumDeclaration1),
        DataRow(EnumDeclaration2),
        DataRow(ClassDeclaration1),
        DataRow(ClassDeclaration2),
        DataRow(StructDeclaration1),
        DataRow(StructDeclaration2),
        DataRow(RecordDeclaration1),
        DataRow(RecordDeclaration2),
        DataRow(InterfaceDeclaration1),
        DataRow(InterfaceDeclaration2),
        DataRow(DelegateDeclaration),
        DataRow(EventDeclaration),
        DataRow(FieldDeclaration),
        DataRow(MethodDeclaration1),
        DataRow(MethodDeclaration2),
        DataRow(PropertyDeclaration1),
        DataRow(PropertyDeclaration2),
        DataRow(PropertyDeclaration3),
        DataRow(PropertyDeclaration4),
        DataRow(EnumMemberDeclaration),
        DataRow(ForStatement),
        DataRow(ForBreakContinueStatement1),
        DataRow(ForBreakContinueStatement2),
        DataRow(ForBreakContinueStatement3),
        DataRow(ForBreakContinueStatement4),
        DataRow(CheckedStatement1),
        DataRow(CheckedStatement2),
        DataRow(ForEachStatement1),
        DataRow(ForEachStatement2),
        DataRow(DoStatement1),
        DataRow(DoStatement2),
        DataRow(DoStatement3),
        DataRow(DoStatement4),
        DataRow(FixedStatement1),
        DataRow(FixedStatement2),
        DataRow(IfElseStatement1),
        DataRow(IfElseStatement2),
        DataRow(IfElseStatement3),
        DataRow(IfElseStatement4),
        DataRow(IfElseStatement5),
        DataRow(LocalDeclarationStatement),
        DataRow(LockStatement1),
        DataRow(LockStatement2),
        DataRow(ReturnStatement),
        DataRow(SwitchBreakStatement1),
        DataRow(SwitchBreakStatement2),
        DataRow(SwitchBreakStatement3),
        DataRow(SwitchBreakStatement4),
        DataRow(ThrowStatement),
        DataRow(TryCatchStatement1),
        DataRow(TryCatchStatement2),
        DataRow(TryCatchStatement3),
        DataRow(TryCatchStatement4),
        DataRow(TryCatchStatement5),
        DataRow(UnsafeStatement1),
        DataRow(UnsafeStatement2),
        DataRow(UsingStatement1),
        DataRow(UsingStatement2),
        DataRow(WhileStatement1),
        DataRow(WhileStatement2),
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
