namespace ConsistencyAnalyzer;

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
            context.RegisterSyntaxNodeAction(Entry.Value.AnalyzeNode, Entry.Value.GetRuleSyntaxKinds());
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
    /// <param name="traceLevel">The trace level.</param>
    public static void Trace(string msg, TraceLevel traceLevel)
    {
        if (!TraceId.IsValueCreated)
            TraceId.Value = Interlocked.Increment(ref lastTraceId);

        if (traceLevel > MaxTraceLevel)
            return;

        string Line = $"{TraceId.Value}.  {msg}";

        System.Diagnostics.Debug.WriteLine(Line);
        FileTrace(Line);
        if (TestTrace != null)
            TestTrace(Line);
    }

    private static int lastTraceId;
    private static bool Started = false;
    private static bool StoppedOnError = false;
    private static Mutex FileMutex = new();
    private static void FileTrace(string msg)
    {
        if (StoppedOnError)
            return;

        FileMutex.WaitOne();

        try
        {
            const string ErrorFolder = @"C:\ConsistencyAnalyzer";

            using (FileStream fs = new FileStream(Path.Combine(ErrorFolder, "error.txt"), Started ? FileMode.Append : FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string LineWithCR = msg + "\n";
                    sw.Write(LineWithCR);
                }
            }
        }
        catch
        {
            StoppedOnError = true;
        }
        finally
        {
            FileMutex.ReleaseMutex();
            Started = true;
        }
    }

    /// <summary>
    /// Gets or sets the method that display traces during a unit test.
    /// </summary>
    public static Action<string> TestTrace { get; set; } = (string msg) => { };

    /// <summary>
    /// Minimum trace level.
    /// </summary>
    public static TraceLevel MaxTraceLevel { get; set; } = TraceLevel.Debug;

    private static ThreadLocal<int> TraceId = new ThreadLocal<int>();
    #endregion
}
