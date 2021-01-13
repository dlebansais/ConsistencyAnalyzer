namespace ConsistencyAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Represent a source code analyzer.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzer : DiagnosticAnalyzer
    {
        #region Init
        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            List<DiagnosticDescriptor> RuleDescriptorList = new List<DiagnosticDescriptor>();
            foreach (KeyValuePair<string, AnalyzerRule> Entry in AnalyzerRule.RuleTable)
            {
                context.RegisterSyntaxNodeAction(Entry.Value.AnalyzeNode, Entry.Value.RuleSyntaxKind);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the list of supported diagnostics.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics 
        { 
            get 
            {
                List<DiagnosticDescriptor> RuleDescriptorList = new List<DiagnosticDescriptor>();
                foreach (KeyValuePair<string, AnalyzerRule> Entry in AnalyzerRule.RuleTable)
                    RuleDescriptorList.Add(Entry.Value.Descriptor);

                return RuleDescriptorList.ToImmutableArray();
            }
        }
        #endregion

        #region Traces
        /// <summary>
        /// Displays traces.
        /// </summary>
        /// <param name="msg">Message to display.</param>
        /// <param name="traceId">An id to use to distinguish parallel code.</param>
        public static void Trace(string msg, ref int traceId)
        {
            if (traceId == 0)
                traceId = Interlocked.Increment(ref lastTraceId);

            string Line = $"{traceId}.  {msg}";

            //System.Diagnostics.Debug.WriteLine(Line);
            //FileTrace(Line);
            if (TestTrace != null)
                TestTrace(Line);
        }

        private static int lastTraceId;
        private static bool Started = false;
        private static void FileTrace(string msg)
        {
            using (FileStream fs = new FileStream(@"C:\Applications\error.txt", Started ? FileMode.Append : FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(msg);
                }
            }

            Started = true;
        }

        /// <summary>
        /// Gets or sets the method that display traces during a unit test.
        /// </summary>
        public static Action<string> TestTrace { get; set; } = (string msg) => { };
        #endregion
    }
}
