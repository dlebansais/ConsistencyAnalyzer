namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1500
    {
        private const string UsingOutsideTooFar = @"
   using System;

namespace ConsistencyAnalyzer
{
    using Microsoft;
}
";

        private const string UsingOutsideTooFarFixed = @"
using System;

namespace ConsistencyAnalyzer
{
    using Microsoft;
}
";

        private const string UsingInsideTooFar = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
      using System;
}
";

        private const string UsingInsideTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    using System;
}
";

        private const string UsingInsideTooClose = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
  using System;
}
";

        private const string UsingInsideTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    using System;
}
";

        private const string NamespaceTooFar = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
}

  namespace CodeFixes
{
}
";

        private const string NamespaceTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
}

namespace CodeFixes
{
}
";

        private const string EnumDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
       public enum CodeFixes
    {
    }
}
";

        private const string EnumDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public enum CodeFixes
    {
    }
}
";

        private const string EnumDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
  public enum CodeFixes
    {
    }
}
";

        private const string EnumDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public enum CodeFixes
    {
    }
}
";

        private const string ClassDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
       public class CodeFixes
    {
    }
}
";

        private const string ClassDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public class CodeFixes
    {
    }
}
";

        private const string ClassDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
  public class CodeFixes
    {
    }
}
";

        private const string ClassDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public class CodeFixes
    {
    }
}
";

        private const string StructDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
       public struct CodeFixes
    {
    }
}
";

        private const string StructDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public struct CodeFixes
    {
    }
}
";

        private const string StructDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
  public struct CodeFixes
    {
    }
}
";

        private const string StructDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public struct CodeFixes
    {
    }
}
";

        private const string RecordDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
       public record CodeFixes
    {
    }
}
";

        private const string RecordDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public record CodeFixes
    {
    }
}
";

        private const string RecordDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
  public record CodeFixes
    {
    }
}
";

        private const string RecordDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public record CodeFixes
    {
    }
}
";

        private const string InterfaceDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
       public interface CodeFixes
    {
    }
}
";

        private const string InterfaceDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public interface CodeFixes
    {
    }
}
";

        private const string InterfaceDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
  public interface CodeFixes
    {
    }
}
";

        private const string InterfaceDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    public interface CodeFixes
    {
    }
}
";

        private const string DelegateDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
           public delegate void Test();
    }
}
";

        private const string DelegateDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public delegate void Test();
    }
}
";

        private const string DelegateDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public delegate void Test();
    }
}
";

        private const string DelegateDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public delegate void Test();
    }
}
";

        private const string EventDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
           public event System.EventHandler Test;
    }
}
";

        private const string EventDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public event System.EventHandler Test;
    }
}
";

        private const string EventDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public event System.EventHandler Test;
    }
}
";

        private const string EventDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public event System.EventHandler Test;
    }
}
";

        private const string FieldDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
           public int Test;
    }
}
";

        private const string FieldDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test;
    }
}
";

        private const string FieldDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public int Test;
    }
}
";

        private const string FieldDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test;
    }
}
";

        private const string MethodDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
           public void Test() {}
    }
}
";

        private const string MethodDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() {}
    }
}
";

        private const string MethodDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public void Test() {}
    }
}
";

        private const string MethodDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() {}
    }
}
";

        private const string PropertyDeclarationTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
           public int Test { get; set; }
    }
}
";

        private const string PropertyDeclarationTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test { get; set; }
    }
}
";

        private const string PropertyDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public int Test { get; set; }
    }
}
";

        private const string PropertyDeclarationTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test { get; set; }
    }
}
";

        private const string EnumMemberDeclarationTooFar = @"
namespace AnalyzerTest
{
    public enum CodeFixes
    {
           Analyzer,
    }
}
";

        private const string EnumMemberDeclarationTooFarFixed = @"
namespace AnalyzerTest
{
    public enum CodeFixes
    {
        Analyzer,
    }
}
";

        private const string EnumMemberDeclarationTooClose = @"
namespace AnalyzerTest
{
    public enum CodeFixes
    {
      Analyzer,
    }
}
";

        private const string EnumMemberDeclarationTooCloseFixed = @"
namespace AnalyzerTest
{
    public enum CodeFixes
    {
        Analyzer,
    }
}
";

        private const string ForStatementTooFar1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
               for (int i = 0; i < 1; i++)
            {
            }
        }
    }
}
";

        private const string ForStatementTooFar1Fixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
            }
        }
    }
}
";

        private const string ForStatementTooFar2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
                   for (int j = 0; j < 1; j++)
                {
                }
        }
    }
}
";

        private const string ForStatementTooFar2Fixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
                for (int j = 0; j < 1; j++)
                {
                }
        }
    }
}
";

        private const string ForStatementTooClose1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
          for (int i = 0; i < 1; i++)
            {
            }
        }
    }
}
";

        private const string ForStatementTooClose1Fixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
            }
        }
    }
}
";

        private const string ForStatementTooClose2 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
              for (int j = 0; j < 1; j++)
                {
                }
        }
    }
}
";

        private const string ForStatementTooClose2Fixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
                for (int j = 0; j < 1; j++)
                {
                }
        }
    }
}
";

        private const string BreakStatementTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                   break;
            }
        }
    }
}
";

        private const string BreakStatementTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                break;
            }
        }
    }
}
";

        private const string BreakStatementTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
              break;
            }
        }
    }
}
";

        private const string BreakStatementTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                break;
            }
        }
    }
}
";

        private const string ContinueStatementTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                   continue;
            }
        }
    }
}
";

        private const string ContinueStatementTooFarFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                continue;
            }
        }
    }
}
";

        private const string ContinueStatementTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
              continue;
            }
        }
    }
}
";

        private const string ContinueStatementTooCloseFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public void Test() 
        {
            for (int i = 0; i < 1; i++)
            {
                continue;
            }
        }
    }
}
";

        private const string CheckedStatementTooFar = @"
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

        private const string CheckedStatementTooFarFixed = @"
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

        private const string CheckedStatementTooClose = @"
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

        private const string CheckedStatementTooCloseFixed = @"
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

        private const string ForEachStatementTooFar = @"
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

        private const string ForEachStatementTooFarFixed = @"
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

        private const string ForEachStatementTooClose = @"
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

        private const string ForEachStatementTooCloseFixed = @"
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

        private const string DoStatementTooFar1 = @"
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

        private const string DoStatementTooFar1Fixed = @"
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

        private const string DoStatementTooClose1 = @"
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

        private const string DoStatementTooClose1Fixed = @"
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

        private const string DoStatementTooFar2 = @"
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

        private const string DoStatementTooFar2Fixed = @"
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

        private const string DoStatementTooClose2 = @"
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

        private const string DoStatementTooClose2Fixed = @"
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

        private const string FixedStatementTooFar = @"
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

        private const string FixedStatementTooFarFixed = @"
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

        private const string FixedStatementTooClose = @"
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

        private const string FixedStatementTooCloseFixed = @"
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

        private const string IfElseStatementTooFar1 = @"
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

        private const string IfElseStatementTooFar1Fixed = @"
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

        private const string IfElseStatementTooClose1 = @"
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

        private const string IfElseStatementTooClose1Fixed = @"
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

        private const string IfElseStatementTooFar2 = @"
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

        private const string IfElseStatementTooFar2Fixed = @"
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

        private const string IfElseStatementTooClose2 = @"
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

        private const string IfElseStatementTooClose2Fixed = @"
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

        private const string LocalDeclarationStatementTooFar = @"
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

        private const string LocalDeclarationStatementTooFarFixed = @"
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

        private const string LocalDeclarationStatementTooClose = @"
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

        private const string LocalDeclarationStatementTooCloseFixed = @"
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

        private const string LockStatementTooFar = @"
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

        private const string LockStatementTooFarFixed = @"
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

        private const string LockStatementTooClose = @"
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

        private const string LockStatementTooCloseFixed = @"
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

        private const string ReturnStatementTooFar = @"
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

        private const string ReturnStatementTooFarFixed = @"
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

        private const string ReturnStatementTooClose = @"
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

        private const string ReturnStatementTooCloseFixed = @"
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

        private const string SwitchBreakStatementTooFar1 = @"
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

        private const string SwitchBreakStatementTooFar1Fixed = @"
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

        private const string SwitchBreakStatementTooClose1 = @"
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

        private const string SwitchBreakStatementTooClose1Fixed = @"
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

        private const string SwitchBreakStatementTooFar2 = @"
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

        private const string SwitchBreakStatementTooFar2Fixed = @"
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

        private const string SwitchBreakStatementTooClose2 = @"
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

        private const string SwitchBreakStatementTooClose2Fixed = @"
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

        private const string SwitchBreakStatementTooFar3 = @"
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

        private const string SwitchBreakStatementTooFar3Fixed = @"
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

        private const string SwitchBreakStatementTooClose3 = @"
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

        private const string SwitchBreakStatementTooClose3Fixed = @"
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

        private const string ThrowStatementTooFar = @"
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

        private const string ThrowStatementTooFarFixed = @"
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

        private const string ThrowStatementTooClose = @"
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

        private const string ThrowStatementTooCloseFixed = @"
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

        private const string TryCatchStatementTooFar1 = @"
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

        private const string TryCatchStatementTooFar1Fixed = @"
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

        private const string TryCatchStatementTooClose1 = @"
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

        private const string TryCatchStatementTooClose1Fixed = @"
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

        private const string TryCatchStatementTooFar2 = @"
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

        private const string TryCatchStatementTooFar2Fixed = @"
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

        private const string TryCatchStatementTooClose2 = @"
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

        private const string TryCatchStatementTooClose2Fixed = @"
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

        private const string UnsafeStatementTooFar = @"
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

        private const string UnsafeStatementTooFarFixed = @"
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

        private const string UnsafeStatementTooClose = @"
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

        private const string UnsafeStatementTooCloseFixed = @"
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

        private const string UsingStatementTooFar = @"
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

        private const string UsingStatementTooFarFixed = @"
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

        private const string UsingStatementTooClose = @"
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

        private const string UsingStatementTooCloseFixed = @"
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

        private const string WhileStatementTooFar = @"
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

        private const string WhileStatementTooFarFixed = @"
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

        private const string WhileStatementTooClose = @"
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

        private const string WhileStatementTooCloseFixed = @"
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

        private const string YieldBreakStatementTooFar = @"
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

        private const string YieldBreakStatementTooFarFixed = @"
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

        private const string YieldBreakStatementTooClose = @"
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

        private const string YieldBreakStatementTooCloseFixed = @"
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

        [DataTestMethod]
        [
        DataRow(UsingOutsideTooFar, UsingOutsideTooFarFixed, 2, 4),
        DataRow(UsingInsideTooFar, UsingInsideTooFarFixed, 9, 7),
        DataRow(UsingInsideTooClose, UsingInsideTooCloseFixed, 9, 3),
        DataRow(NamespaceTooFar, NamespaceTooFarFixed, 7, 3),
        DataRow(EnumDeclarationTooFar, EnumDeclarationTooFarFixed, 9, 8),
        DataRow(EnumDeclarationTooClose, EnumDeclarationTooCloseFixed, 9, 3),
        DataRow(ClassDeclarationTooFar, ClassDeclarationTooFarFixed, 9, 8),
        DataRow(ClassDeclarationTooClose, ClassDeclarationTooCloseFixed, 9, 3),
        DataRow(StructDeclarationTooFar, StructDeclarationTooFarFixed, 9, 8),
        DataRow(StructDeclarationTooClose, StructDeclarationTooCloseFixed, 9, 3),
        DataRow(RecordDeclarationTooFar, RecordDeclarationTooFarFixed, 9, 8),
        DataRow(RecordDeclarationTooClose, RecordDeclarationTooCloseFixed, 9, 3),
        DataRow(InterfaceDeclarationTooFar, InterfaceDeclarationTooFarFixed, 9, 8),
        DataRow(InterfaceDeclarationTooClose, InterfaceDeclarationTooCloseFixed, 9, 3),
        DataRow(DelegateDeclarationTooFar, DelegateDeclarationTooFarFixed, 6, 12),
        DataRow(DelegateDeclarationTooClose, DelegateDeclarationTooCloseFixed, 6, 7),
        DataRow(EventDeclarationTooFar, EventDeclarationTooFarFixed, 6, 12),
        DataRow(EventDeclarationTooClose, EventDeclarationTooCloseFixed, 6, 7),
        DataRow(FieldDeclarationTooFar, FieldDeclarationTooFarFixed, 6, 12),
        DataRow(FieldDeclarationTooClose, FieldDeclarationTooCloseFixed, 6, 7),
        DataRow(MethodDeclarationTooFar, MethodDeclarationTooFarFixed, 6, 12),
        DataRow(MethodDeclarationTooClose, MethodDeclarationTooCloseFixed, 6, 7),
        DataRow(PropertyDeclarationTooFar, PropertyDeclarationTooFarFixed, 6, 12),
        DataRow(PropertyDeclarationTooClose, PropertyDeclarationTooCloseFixed, 6, 7),
        DataRow(EnumMemberDeclarationTooFar, EnumMemberDeclarationTooFarFixed, 6, 12),
        DataRow(EnumMemberDeclarationTooClose, EnumMemberDeclarationTooCloseFixed, 6, 7),
        DataRow(ForStatementTooFar1, ForStatementTooFar1Fixed, 8, 16),
        DataRow(ForStatementTooClose1, ForStatementTooClose1Fixed, 8, 11),
        DataRow(ForStatementTooFar2, ForStatementTooFar2Fixed, 9, 20),
        DataRow(ForStatementTooClose2, ForStatementTooClose2Fixed, 9, 15),
        DataRow(BreakStatementTooFar, BreakStatementTooFarFixed, 10, 20),
        DataRow(BreakStatementTooClose, BreakStatementTooCloseFixed, 10, 15),
        DataRow(ContinueStatementTooFar, ContinueStatementTooFarFixed, 10, 20),
        DataRow(ContinueStatementTooClose, ContinueStatementTooCloseFixed, 10, 15),
        DataRow(CheckedStatementTooFar, CheckedStatementTooFarFixed, 8, 16),
        DataRow(CheckedStatementTooClose, CheckedStatementTooCloseFixed, 8, 11),
        DataRow(ForEachStatementTooFar, ForEachStatementTooFarFixed, 8, 16),
        DataRow(ForEachStatementTooClose, ForEachStatementTooCloseFixed, 8, 11),
        DataRow(DoStatementTooFar1, DoStatementTooFar1Fixed, 8, 16),
        DataRow(DoStatementTooClose1, DoStatementTooClose1Fixed, 8, 11),
        DataRow(DoStatementTooFar2, DoStatementTooFar1Fixed, 11, 16),
        DataRow(DoStatementTooClose2, DoStatementTooClose1Fixed, 11, 11),
        DataRow(FixedStatementTooFar, FixedStatementTooFarFixed, 9, 16),
        DataRow(FixedStatementTooClose, FixedStatementTooCloseFixed, 9, 11),
        DataRow(IfElseStatementTooFar1, IfElseStatementTooFar2Fixed, 8, 16),
        DataRow(IfElseStatementTooClose1, IfElseStatementTooClose2Fixed, 8, 11),
        DataRow(IfElseStatementTooFar2, IfElseStatementTooFar2Fixed, 11, 16),
        DataRow(IfElseStatementTooClose2, IfElseStatementTooClose2Fixed, 11, 11),
        DataRow(LocalDeclarationStatementTooFar, LocalDeclarationStatementTooFarFixed, 8, 16),
        DataRow(LocalDeclarationStatementTooClose, LocalDeclarationStatementTooCloseFixed, 8, 11),
        DataRow(LockStatementTooFar, LockStatementTooFarFixed, 9, 16),
        DataRow(LockStatementTooClose, LockStatementTooCloseFixed, 9, 11),
        DataRow(ReturnStatementTooFar, ReturnStatementTooFarFixed, 8, 16),
        DataRow(ReturnStatementTooClose, ReturnStatementTooCloseFixed, 8, 11),
        DataRow(SwitchBreakStatementTooFar1, SwitchBreakStatementTooFar1Fixed, 8, 16),
        DataRow(SwitchBreakStatementTooClose1, SwitchBreakStatementTooClose1Fixed, 8, 11),
        DataRow(SwitchBreakStatementTooFar2, SwitchBreakStatementTooFar2Fixed, 10, 20),
        DataRow(SwitchBreakStatementTooClose2, SwitchBreakStatementTooClose2Fixed, 10, 15),
        DataRow(SwitchBreakStatementTooFar3, SwitchBreakStatementTooFar3Fixed, 11, 24),
        DataRow(SwitchBreakStatementTooClose3, SwitchBreakStatementTooClose3Fixed, 11, 19),
        DataRow(ThrowStatementTooFar, ThrowStatementTooFarFixed, 8, 16),
        DataRow(ThrowStatementTooClose, ThrowStatementTooCloseFixed, 8, 11),
        DataRow(TryCatchStatementTooFar1, TryCatchStatementTooFar1Fixed, 8, 16),
        DataRow(TryCatchStatementTooClose1, TryCatchStatementTooClose1Fixed, 8, 11),
        DataRow(TryCatchStatementTooFar2, TryCatchStatementTooFar2Fixed, 12, 16),
        DataRow(TryCatchStatementTooClose2, TryCatchStatementTooClose2Fixed, 12, 11),
        DataRow(UnsafeStatementTooFar, UnsafeStatementTooFarFixed, 8, 16),
        DataRow(UnsafeStatementTooClose, UnsafeStatementTooCloseFixed, 8, 11),
        DataRow(UsingStatementTooFar, UsingStatementTooFarFixed, 8, 16),
        DataRow(UsingStatementTooClose, UsingStatementTooCloseFixed, 8, 11),
        DataRow(WhileStatementTooFar, WhileStatementTooFarFixed, 8, 16),
        DataRow(WhileStatementTooClose, WhileStatementTooCloseFixed, 8, 11),
        DataRow(YieldBreakStatementTooFar, YieldBreakStatementTooFarFixed, 10, 20),
        DataRow(YieldBreakStatementTooClose, YieldBreakStatementTooCloseFixed, 10, 15),
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(string test, string fixedsource, int line, int column)
        {
            string AnalyzerMessage = new LocalizableResourceString(nameof(Resources.ConA1500MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1500)),
                "title",
                AnalyzerMessage,
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
