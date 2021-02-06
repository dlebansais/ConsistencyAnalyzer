namespace ConsistencyAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using VerifyCS = CSharpCodeFixVerifier<Analyzer, Provider>;

    public partial class UnitTestConA1702
    {
        private const string OneClassTwoRegionsConstructor = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init1
        public void Test1() {}
#endregion

#region Init2
        public Test() {}
#endregion
    }

    public class EnableInterfaceCategorySimple1
    {
#region Init
        public EnableInterfaceCategorySimple1() {}
        public void EnableTest1() {}
#endregion
    }

    public class EnableInterfaceCategorySimple2
    {
#region Init
        public EnableInterfaceCategorySimple2() {}
        public void EnableTest2() {}
#endregion
    }
}";

        private const string OneClassTwoRegionsField = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init1
        public void Test1() {}
#endregion

#region Init2
        public int Test2;
#endregion
    }

    public class EnableInterfaceCategorySimple1
    {
#region Init
        public EnableInterfaceCategorySimple1() {}
        public void EnableTest1() {}
#endregion
    }

    public class EnableInterfaceCategorySimple2
    {
#region Init
        public EnableInterfaceCategorySimple2() {}
        public void EnableTest2() {}
#endregion
    }
}";

        private const string OneClassTwoRegionsMethod = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init1
        public int Test1;
#endregion

#region Init2
        public void Test2() {}
#endregion
    }

    public class EnableInterfaceCategorySimple1
    {
#region Init
        public EnableInterfaceCategorySimple1() {}
        public void EnableTest1() {}
#endregion
    }

    public class EnableInterfaceCategorySimple2
    {
#region Init
        public EnableInterfaceCategorySimple2() {}
        public void EnableTest2() {}
#endregion
    }
}";

        private const string OneClassTwoRegionsProperty = @"
using System;

namespace ConsistencyAnalyzerTest
{
    public class Test
    {
#region Init1
        public void Test1() {}
#endregion

#region Init2
        public int Test2 { get; set; }
#endregion
    }

    public class EnableInterfaceCategorySimple1
    {
#region Init
        public EnableInterfaceCategorySimple1() {}
        public void EnableTest1() {}
#endregion
    }

    public class EnableInterfaceCategorySimple2
    {
#region Init
        public EnableInterfaceCategorySimple2() {}
        public void EnableTest2() {}
#endregion
    }
}";

        [DataTestMethod]
        [
        DataRow(OneClassTwoRegionsConstructor, 13, 9, "Test", "Init1"),
        DataRow(OneClassTwoRegionsField, 13, 9, "Test2", "Init1"),
        DataRow(OneClassTwoRegionsMethod, 13, 9, "Test2", "Init1"),
        DataRow(OneClassTwoRegionsProperty, 13, 9, "Test2", "Init1"),
        ]
        public void WhenTestCodeInvalidDiagnosticIsRaised(string test, int line, int column, string memberName, string regionName)
        {
            string AnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConA1702MessageFormat), Resources.ResourceManager, typeof(Resources)).ToString();
            string FormatedMessage = string.Format(AnalyzerMessageFormat, memberName, regionName);

            var descriptor = new DiagnosticDescriptor(
                AnalyzerRule.ToRuleId(nameof(AnalyzerRuleConA1702)),
                "title",
                FormatedMessage,
                "description",
                DiagnosticSeverity.Warning,
                true
                );

            var expected = new DiagnosticResult(descriptor);
            expected = expected.WithLocation("/0/Test0.cs", line, column);

            Task result = VerifyCS.VerifyAnalyzerAsync(test, expected);
            result.Wait();
        }
    }
}
