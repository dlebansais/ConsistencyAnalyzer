namespace ConsistencyAnalyzer.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Globalization;

    public class UnitTestBase
    {
        private static TestContext testContextInstance = null!;

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

        [TestInitialize]
        public void Init()
        {
            CultureInfo enUsCulture = new CultureInfo("en-US", false);
            if (CultureInfo.CurrentCulture != enUsCulture)
                CultureInfo.CurrentCulture = enUsCulture;
            if (CultureInfo.CurrentUICulture != enUsCulture)
                CultureInfo.CurrentUICulture = enUsCulture;

            if (CultureInfo.CurrentCulture != enUsCulture)
                throw new System.Exception($"Unable to use English messages: {CultureInfo.CurrentCulture}");
        }

        protected void UnifyCarriageReturn(ref string s)
        {
            s = s.Replace("\r\n", "\n");
            s = s.Replace("\n", "\r\n");
        }
    }
}
