namespace ConsistencyAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzer : DiagnosticAnalyzer
    {
        public static void Trace(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
            //FileTrace(msg);
            if (TestTrace != null)
                TestTrace(msg);
        }

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
        
        public static Action<string> TestTrace { get; set; }

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

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            List<DiagnosticDescriptor> RuleDescriptorList = new List<DiagnosticDescriptor>();
            foreach (KeyValuePair<string, AnalyzerRule> Entry in AnalyzerRule.RuleTable)
            {
                context.RegisterSyntaxNodeAction(Entry.Value.AnalyzeNode, Entry.Value.SyntaxKind);
            }
        }
    }
}
