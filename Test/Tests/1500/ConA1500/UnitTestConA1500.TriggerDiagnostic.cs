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

        private const string UsingInsideFixed = @"
namespace ConsistencyAnalyzer
{
    using System;
}

namespace AnalyzerTest
{
    using System;
}
";

        private const string NamespaceTooFar1 = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
}

  namespace CodeFixes
{
}
";

        private const string NamespaceTooFar2 = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
}

namespace CodeFixes
  {
}
";

        private const string NamespaceTooFar3 = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
}

namespace CodeFixes
{
  }
";

        private const string NamespaceFixed = @"
namespace ConsistencyAnalyzer
{
    using Microsoft;
}

namespace CodeFixes
{
}
";

        private const string EnumDeclarationTooFar1 = @"
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

        private const string EnumDeclarationTooClose1 = @"
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

        private const string EnumDeclarationTooFar2 = @"
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

        private const string EnumDeclarationTooClose2 = @"
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

        private const string EnumDeclarationTooFar3 = @"
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

        private const string EnumDeclarationTooClose3 = @"
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

        private const string EnumDeclarationFixed = @"
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

        private const string ClassDeclarationTooFar1 = @"
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

        private const string ClassDeclarationTooClose1 = @"
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

        private const string ClassDeclarationTooFar2 = @"
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

        private const string ClassDeclarationTooClose2 = @"
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

        private const string ClassDeclarationTooFar3 = @"
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

        private const string ClassDeclarationTooClose3 = @"
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

        private const string ClassDeclarationFixed = @"
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

        private const string StructDeclarationTooFar1 = @"
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

        private const string StructDeclarationTooClose1 = @"
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

        private const string StructDeclarationTooFar2 = @"
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

        private const string StructDeclarationTooClose2 = @"
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

        private const string StructDeclarationTooFar3 = @"
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

        private const string StructDeclarationTooClose3 = @"
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

        private const string StructDeclarationFixed = @"
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

        private const string RecordDeclarationTooFar1 = @"
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

        private const string RecordDeclarationTooClose1 = @"
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

        private const string RecordDeclarationTooFar2 = @"
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

        private const string RecordDeclarationTooClose2 = @"
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

        private const string RecordDeclarationTooFar3 = @"
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

        private const string RecordDeclarationTooClose3 = @"
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

        private const string RecordDeclarationFixed = @"
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

        private const string InterfaceDeclarationTooFar1 = @"
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

        private const string InterfaceDeclarationTooClose1 = @"
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

        private const string InterfaceDeclarationTooFar2 = @"
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

        private const string InterfaceDeclarationTooClose2 = @"
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

        private const string InterfaceDeclarationTooFar3 = @"
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

        private const string InterfaceDeclarationTooClose3 = @"
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

        private const string InterfaceDeclarationFixed = @"
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

        private const string DelegateDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public delegate void Test();
    }
}
";

        private const string DelegateDeclarationFixed = @"
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

        private const string EventDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public event System.EventHandler Test;
    }
}
";

        private const string EventDeclarationFixed = @"
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

        private const string FieldDeclarationTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public int Test;
    }
}
";

        private const string FieldDeclarationFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test;
    }
}
";

        private const string MethodDeclarationTooFar1 = @"
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

        private const string MethodDeclarationTooClose1 = @"
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

        private const string MethodDeclarationTooFar2 = @"
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

        private const string MethodDeclarationTooClose2 = @"
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

        private const string MethodDeclarationTooFar3 = @"
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

        private const string MethodDeclarationTooClose3 = @"
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

        private const string MethodDeclarationFixed = @"
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

        private const string PropertyDeclarationTooFar1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
           public int Test { get; set; }
    }
}
";

        private const string PropertyDeclarationTooClose1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
      public int Test { get; set; }
    }
}
";

        private const string PropertyDeclarationFixed1 = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public int Test { get; set; }
    }
}
";

        private const string PropertyDeclarationTooFar2_1 = @"
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

        private const string PropertyDeclarationTooClose2_1 = @"
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

        private const string PropertyDeclarationTooFar2_2 = @"
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

        private const string PropertyDeclarationTooClose2_2 = @"
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

        private const string PropertyDeclarationTooFar2_3 = @"
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

        private const string PropertyDeclarationTooClose2_3 = @"
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

        private const string PropertyDeclarationTooFar2_4 = @"
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

        private const string PropertyDeclarationTooClose2_4 = @"
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

        private const string PropertyDeclarationFixed2 = @"
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

        private const string PropertyDeclarationTooFar3_1 = @"
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

        private const string PropertyDeclarationTooClose3_1 = @"
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

        private const string PropertyDeclarationTooFar3_2 = @"
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

        private const string PropertyDeclarationTooClose3_2 = @"
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

        private const string PropertyDeclarationTooFar3_3 = @"
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

        private const string PropertyDeclarationTooClose3_3 = @"
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

        private const string PropertyDeclarationTooFar3_4 = @"
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

        private const string PropertyDeclarationTooClose3_4 = @"
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

        private const string PropertyDeclarationFixed3 = @"
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

        private const string EnumMemberDeclarationTooFar = @"
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

        private const string EnumMemberDeclarationFixed = @"
namespace AnalyzerTest
{
    public enum CodeFixes
    {
        Analyzer,
    }
}
";

        private const string OneForStatementTooFar1 = @"
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

        private const string OneForStatementTooClose1 = @"
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

        private const string OneForStatementTooFar2 = @"
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

        private const string OneForStatementTooClose2 = @"
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

        private const string OneForStatementTooFar3 = @"
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

        private const string OneForStatementTooClose3 = @"
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

        private const string OneForStatementFixed = @"
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

        private const string TwoForStatementTooFar1 = @"
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

        private const string TwoForStatementTooClose1 = @"
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

        private const string TwoForStatementFixed = @"
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

        private const string BreakStatementFixed = @"
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

        private const string ContinueStatementFixed = @"
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

        private const string CheckedStatementFixed = @"
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

        private const string ForEachStatementFixed = @"
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

        private const string DoStatementTooFar3 = @"
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

        private const string DoStatementTooClose3 = @"
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

        private const string DoStatementTooFar4 = @"
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

        private const string DoStatementTooClose4 = @"
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

        private const string DoStatementFixed = @"
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

        private const string FixedStatementFixed = @"
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

        private const string IfElseStatementFixed = @"
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

        private const string LocalDeclarationStatementFixed = @"
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

        private const string LockStatementFixed = @"
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

        private const string ReturnStatementFixed = @"
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

        private const string SwitchBreakStatementFixed = @"
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

        private const string ThrowStatementFixed = @"
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

        private const string TryCatchStatementTooFar3 = @"
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

        private const string TryCatchStatementTooClose3 = @"
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

        private const string TryCatchStatementTooFar4 = @"
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

        private const string TryCatchStatementTooClose4 = @"
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

        private const string TryCatchStatementTooFar5 = @"
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

        private const string TryCatchStatementTooClose5 = @"
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

        private const string TryCatchStatementTooFar6 = @"
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

        private const string TryCatchStatementTooClose6 = @"
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

        private const string TryCatchStatementFixed = @"
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

        private const string UnsafeStatementFixed = @"
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

        private const string UsingStatementFixed = @"
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

        private const string WhileStatementFixed = @"
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

        private const string YieldBreakStatementFixed = @"
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

        private const string YieldReturnStatementTooFar = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public System.Collections.Generic.IEnumerable<int> Test()
        {
            for (int i = 0; i < 1; i++)
            {
                   yield return 0;
            }
        }
    }
}
";

        private const string YieldReturnStatementTooClose = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public System.Collections.Generic.IEnumerable<int> Test()
        {
            for (int i = 0; i < 1; i++)
            {
              yield return 0;
            }
        }
    }
}
";

        private const string YieldReturnStatementFixed = @"
namespace ConsistencyAnalyzer
{
    public class CodeFixes
    {
        public System.Collections.Generic.IEnumerable<int> Test()
        {
            for (int i = 0; i < 1; i++)
            {
                yield return 0;
            }
        }
    }
}
";

        [DataTestMethod]
        [
        DataRow(UsingOutsideTooFar, UsingOutsideTooFarFixed, 2, 4),
        DataRow(UsingInsideTooFar, UsingInsideFixed, 9, 7),
        DataRow(UsingInsideTooClose, UsingInsideFixed, 9, 3),
        DataRow(NamespaceTooFar1, NamespaceFixed, 7, 3),
        DataRow(NamespaceTooFar2, NamespaceFixed, 8, 3),
        DataRow(NamespaceTooFar3, NamespaceFixed, 9, 3),
        DataRow(EnumDeclarationTooFar1, EnumDeclarationFixed, 9, 8),
        DataRow(EnumDeclarationTooClose1, EnumDeclarationFixed, 9, 3),
        DataRow(EnumDeclarationTooFar2, EnumDeclarationFixed, 10, 8),
        DataRow(EnumDeclarationTooClose2, EnumDeclarationFixed, 10, 3),
        DataRow(EnumDeclarationTooFar3, EnumDeclarationFixed, 11, 8),
        DataRow(EnumDeclarationTooClose3, EnumDeclarationFixed, 11, 3),
        DataRow(ClassDeclarationTooFar1, ClassDeclarationFixed, 9, 8),
        DataRow(ClassDeclarationTooClose1, ClassDeclarationFixed, 9, 3),
        DataRow(ClassDeclarationTooFar2, ClassDeclarationFixed, 10, 8),
        DataRow(ClassDeclarationTooClose2, ClassDeclarationFixed, 10, 3),
        DataRow(ClassDeclarationTooFar3, ClassDeclarationFixed, 11, 8),
        DataRow(ClassDeclarationTooClose3, ClassDeclarationFixed, 11, 3),
        DataRow(StructDeclarationTooFar1, StructDeclarationFixed, 9, 8),
        DataRow(StructDeclarationTooClose1, StructDeclarationFixed, 9, 3),
        DataRow(StructDeclarationTooFar2, StructDeclarationFixed, 10, 8),
        DataRow(StructDeclarationTooClose2, StructDeclarationFixed, 10, 3),
        DataRow(StructDeclarationTooFar3, StructDeclarationFixed, 11, 8),
        DataRow(StructDeclarationTooClose3, StructDeclarationFixed, 11, 3),
        DataRow(RecordDeclarationTooFar1, RecordDeclarationFixed, 9, 8),
        DataRow(RecordDeclarationTooClose1, RecordDeclarationFixed, 9, 3),
        DataRow(RecordDeclarationTooFar2, RecordDeclarationFixed, 10, 8),
        DataRow(RecordDeclarationTooClose2, RecordDeclarationFixed, 10, 3),
        DataRow(RecordDeclarationTooFar3, RecordDeclarationFixed, 11, 8),
        DataRow(RecordDeclarationTooClose3, RecordDeclarationFixed, 11, 3),
        DataRow(InterfaceDeclarationTooFar1, InterfaceDeclarationFixed, 9, 8),
        DataRow(InterfaceDeclarationTooClose1, InterfaceDeclarationFixed, 9, 3),
        DataRow(InterfaceDeclarationTooFar2, InterfaceDeclarationFixed, 10, 8),
        DataRow(InterfaceDeclarationTooClose2, InterfaceDeclarationFixed, 10, 3),
        DataRow(InterfaceDeclarationTooFar3, InterfaceDeclarationFixed, 11, 8),
        DataRow(InterfaceDeclarationTooClose3, InterfaceDeclarationFixed, 11, 3),
        DataRow(DelegateDeclarationTooFar, DelegateDeclarationFixed, 6, 12),
        DataRow(DelegateDeclarationTooClose, DelegateDeclarationFixed, 6, 7),
        DataRow(EventDeclarationTooFar, EventDeclarationFixed, 6, 12),
        DataRow(EventDeclarationTooClose, EventDeclarationFixed, 6, 7),
        DataRow(FieldDeclarationTooFar, FieldDeclarationFixed, 6, 12),
        DataRow(FieldDeclarationTooClose, FieldDeclarationFixed, 6, 7),
        DataRow(MethodDeclarationTooFar1, MethodDeclarationFixed, 6, 12),
        DataRow(MethodDeclarationTooClose1, MethodDeclarationFixed, 6, 7),
        DataRow(MethodDeclarationTooFar2, MethodDeclarationFixed, 7, 12),
        DataRow(MethodDeclarationTooClose2, MethodDeclarationFixed, 7, 7),
        DataRow(MethodDeclarationTooFar3, MethodDeclarationFixed, 8, 12),
        DataRow(MethodDeclarationTooClose3, MethodDeclarationFixed, 8, 7),
        DataRow(PropertyDeclarationTooFar1, PropertyDeclarationFixed1, 6, 12),
        DataRow(PropertyDeclarationTooClose1, PropertyDeclarationFixed1, 6, 7),
        DataRow(PropertyDeclarationTooFar2_1, PropertyDeclarationFixed2, 7, 12),
        DataRow(PropertyDeclarationTooClose2_1, PropertyDeclarationFixed2, 7, 7),
        DataRow(PropertyDeclarationTooFar2_2, PropertyDeclarationFixed2, 8, 16),
        DataRow(PropertyDeclarationTooClose2_2, PropertyDeclarationFixed2, 8, 11),
        DataRow(PropertyDeclarationTooFar2_3, PropertyDeclarationFixed2, 9, 16),
        DataRow(PropertyDeclarationTooClose2_3, PropertyDeclarationFixed2, 9, 11),
        DataRow(PropertyDeclarationTooFar2_4, PropertyDeclarationFixed2, 10, 12),
        DataRow(PropertyDeclarationTooClose2_4, PropertyDeclarationFixed2, 10, 7),
        DataRow(PropertyDeclarationTooFar3_1, PropertyDeclarationFixed3, 9, 16),
        DataRow(PropertyDeclarationTooClose3_1, PropertyDeclarationFixed3, 9, 11),
        DataRow(PropertyDeclarationTooFar3_2, PropertyDeclarationFixed3, 11, 16),
        DataRow(PropertyDeclarationTooClose3_2, PropertyDeclarationFixed3, 11, 11),
        DataRow(PropertyDeclarationTooFar3_3, PropertyDeclarationFixed3, 13, 16),
        DataRow(PropertyDeclarationTooClose3_3, PropertyDeclarationFixed3, 13, 11),
        DataRow(PropertyDeclarationTooFar3_4, PropertyDeclarationFixed3, 14, 16),
        DataRow(PropertyDeclarationTooClose3_4, PropertyDeclarationFixed3, 14, 11),
        DataRow(EnumMemberDeclarationTooFar, EnumMemberDeclarationFixed, 6, 12),
        DataRow(EnumMemberDeclarationTooClose, EnumMemberDeclarationFixed, 6, 7),
        DataRow(OneForStatementTooFar1, OneForStatementFixed, 8, 16),
        DataRow(OneForStatementTooClose1, OneForStatementFixed, 8, 11),
        DataRow(TwoForStatementTooFar1, TwoForStatementFixed, 9, 20),
        DataRow(TwoForStatementTooClose1, TwoForStatementFixed, 9, 15),
        DataRow(BreakStatementTooFar, BreakStatementFixed, 10, 20),
        DataRow(BreakStatementTooClose, BreakStatementFixed, 10, 15),
        DataRow(ContinueStatementTooFar, ContinueStatementFixed, 10, 20),
        DataRow(ContinueStatementTooClose, ContinueStatementFixed, 10, 15),
        DataRow(CheckedStatementTooFar, CheckedStatementFixed, 8, 16),
        DataRow(CheckedStatementTooClose, CheckedStatementFixed, 8, 11),
        DataRow(ForEachStatementTooFar, ForEachStatementFixed, 8, 16),
        DataRow(ForEachStatementTooClose, ForEachStatementFixed, 8, 11),
        DataRow(DoStatementTooFar1, DoStatementFixed, 8, 16),
        DataRow(DoStatementTooClose1, DoStatementFixed, 8, 11),
        DataRow(DoStatementTooFar2, DoStatementFixed, 9, 16),
        DataRow(DoStatementTooClose2, DoStatementFixed, 9, 11),
        DataRow(DoStatementTooFar3, DoStatementFixed, 10, 16),
        DataRow(DoStatementTooClose3, DoStatementFixed, 10, 11),
        DataRow(DoStatementTooFar4, DoStatementFixed, 11, 16),
        DataRow(DoStatementTooClose4, DoStatementFixed, 11, 11),
        DataRow(FixedStatementTooFar, FixedStatementFixed, 9, 16),
        DataRow(FixedStatementTooClose, FixedStatementFixed, 9, 11),
        DataRow(IfElseStatementTooFar1, IfElseStatementFixed, 8, 16),
        DataRow(IfElseStatementTooClose1, IfElseStatementFixed, 8, 11),
        DataRow(IfElseStatementTooFar2, IfElseStatementFixed, 11, 16),
        DataRow(IfElseStatementTooClose2, IfElseStatementFixed, 11, 11),
        DataRow(LocalDeclarationStatementTooFar, LocalDeclarationStatementFixed, 8, 16),
        DataRow(LocalDeclarationStatementTooClose, LocalDeclarationStatementFixed, 8, 11),
        DataRow(LockStatementTooFar, LockStatementFixed, 9, 16),
        DataRow(LockStatementTooClose, LockStatementFixed, 9, 11),
        DataRow(ReturnStatementTooFar, ReturnStatementFixed, 8, 16),
        DataRow(ReturnStatementTooClose, ReturnStatementFixed, 8, 11),
        DataRow(SwitchBreakStatementTooFar1, SwitchBreakStatementFixed, 8, 16),
        DataRow(SwitchBreakStatementTooClose1, SwitchBreakStatementFixed, 8, 11),
        DataRow(SwitchBreakStatementTooFar2, SwitchBreakStatementFixed, 10, 20),
        DataRow(SwitchBreakStatementTooClose2, SwitchBreakStatementFixed, 10, 15),
        DataRow(SwitchBreakStatementTooFar3, SwitchBreakStatementFixed, 11, 24),
        DataRow(SwitchBreakStatementTooClose3, SwitchBreakStatementFixed, 11, 19),
        DataRow(ThrowStatementTooFar, ThrowStatementFixed, 8, 16),
        DataRow(ThrowStatementTooClose, ThrowStatementFixed, 8, 11),
        DataRow(TryCatchStatementTooFar1, TryCatchStatementFixed, 8, 16),
        DataRow(TryCatchStatementTooClose1, TryCatchStatementFixed, 8, 11),
        DataRow(TryCatchStatementTooFar2, TryCatchStatementFixed, 9, 16),
        DataRow(TryCatchStatementTooClose2, TryCatchStatementFixed, 9, 11),
        DataRow(TryCatchStatementTooFar3, TryCatchStatementFixed, 11, 16),
        DataRow(TryCatchStatementTooClose3, TryCatchStatementFixed, 11, 11),
        DataRow(TryCatchStatementTooFar4, TryCatchStatementFixed, 12, 16),
        DataRow(TryCatchStatementTooClose4, TryCatchStatementFixed, 12, 11),
        DataRow(TryCatchStatementTooFar5, TryCatchStatementFixed, 13, 16),
        DataRow(TryCatchStatementTooClose5, TryCatchStatementFixed, 13, 11),
        DataRow(TryCatchStatementTooFar6, TryCatchStatementFixed, 15, 16),
        DataRow(TryCatchStatementTooClose6, TryCatchStatementFixed, 15, 11),
        DataRow(UnsafeStatementTooFar, UnsafeStatementFixed, 8, 16),
        DataRow(UnsafeStatementTooClose, UnsafeStatementFixed, 8, 11),
        DataRow(UsingStatementTooFar, UsingStatementFixed, 8, 16),
        DataRow(UsingStatementTooClose, UsingStatementFixed, 8, 11),
        DataRow(WhileStatementTooFar, WhileStatementFixed, 8, 16),
        DataRow(WhileStatementTooClose, WhileStatementFixed, 8, 11),
        DataRow(YieldBreakStatementTooFar, YieldBreakStatementFixed, 10, 20),
        DataRow(YieldBreakStatementTooClose, YieldBreakStatementFixed, 10, 15),
        DataRow(YieldReturnStatementTooFar, YieldReturnStatementFixed, 10, 20),
        DataRow(YieldReturnStatementTooClose, YieldReturnStatementFixed, 10, 15),
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
