namespace ConsistencyAnalyzer;

using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Threading;

/// <summary>
/// Represents an object that synchronizes class analysis.
/// </summary>
public class ClassSynchronizer
{
    /// <summary>
    /// Creates a ClassSynchronizer.
    /// </summary>
    /// <param name="context">The source code.</param>
    /// <param name="traceLevel">The trace level.</param>
    public ClassSynchronizer(SyntaxNodeAnalysisContext context, TraceLevel traceLevel)
    {
        ClassOrStructExplorer ClassExplorer = ContextExplorer.Get(context, traceLevel).ClassOrStructExplorer;
        ClassCount = ClassExplorer.GetClassOrStructList().Count;
        CallCount = 0;
        AllCalledEvent = new ManualResetEvent(false);
    }

    /// <summary>
    /// Gets the class count.
    /// </summary>
    public int ClassCount { get; }

    /// <summary>
    /// Wait for all classes to execute this call.
    /// </summary>
    /// <param name="traceLevel">The trace level.</param>
    public void WaitAll(TraceLevel traceLevel)
    {
        int LastCallCount = Interlocked.Increment(ref CallCount);

        if (LastCallCount == ClassCount)
            AllCalledEvent.Set();
        else
        {
            bool IsSignaled = AllCalledEvent.WaitOne(TimeSpan.FromSeconds(50));
            if (!IsSignaled)
                Analyzer.Trace($"Logic error waiting for signal", TraceLevel.Critical);
        }
    }

    private int CallCount;
    private ManualResetEvent AllCalledEvent;
}
