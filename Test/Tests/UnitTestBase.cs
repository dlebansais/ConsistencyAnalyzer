namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class UnitTestBase
    {
        private static TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set
            {
                testContextInstance = value;
                Analyzer.TestTrace = TestTrace;
            }
        }

        public static void TestTrace(string msg)
        {
            testContextInstance.WriteLine(msg);
        }
    }
}
