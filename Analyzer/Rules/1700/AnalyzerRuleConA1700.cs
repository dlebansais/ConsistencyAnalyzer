namespace ConsistencyAnalyzer;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Represents a rule of the analyzer.
/// </summary>
public class AnalyzerRuleConA1700 : SingleSyntaxAnalyzerRule
{
    #region Properties
    /// <summary>
    /// Gets the rule id.
    /// </summary>
    public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1700));

    /// <summary>
    /// Gets the kind of syntax this rule analyzes.
    /// </summary>
    public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.ClassDeclaration;
    #endregion

    #region Ancestor Interface
    /// <summary>
    /// Gets the rule title.
    /// </summary>
    protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1700Title), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule message format.
    /// </summary>
    protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1700MessageFormat), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule description.
    /// </summary>
    protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1700Description), Resources.ResourceManager, typeof(Resources));

    /// <summary>
    /// Gets the rule category.
    /// </summary>
    protected override string Category { get; } = "Usage";
    #endregion

    #region Client Interface
    /// <summary>
    /// Analyzes a source code node.
    /// </summary>
    /// <param name="context">The source code.</param>
    public override void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        TraceLevel TraceLevel = TraceLevel.Debug;
        Analyzer.Trace("AnalyzerRuleConA1700", TraceLevel);

        try
        {
            Analyzer.Trace("Traces 01", TraceLevel);

            ClassDeclarationSyntax Node = (ClassDeclarationSyntax)context.Node;

            Analyzer.Trace("Traces 02", TraceLevel);

            RegionExplorer RegionExplorer = ContextExplorer.Get(context, TraceLevel).GetRegionExplorer(Node);

            Analyzer.Trace("Traces 03", TraceLevel);

            IEnumerable<SyntaxNode> Nodes = Node.AncestorsAndSelf();

            Analyzer.Trace("Traces 04", TraceLevel);

            CompilationUnitSyntax Root = Nodes.OfType<CompilationUnitSyntax>().First();

            Analyzer.Trace("Traces 05", TraceLevel);

            GlobalState<bool?> ProgramHasMembersOutsideRegion;
            ClassSynchronizer Synchronizer;

            Analyzer.Trace("Traces 06", TraceLevel);

            lock (TableLock)
            {
                Analyzer.Trace("Traces 07", TraceLevel);

                object? NullableRoot = Root;
                string KeyList = $"Root: {NullableRoot?.GetType()?.FullName}({NullableRoot?.GetHashCode()})";
                foreach (KeyValuePair<CompilationUnitSyntax, CompilationUnitState> Entry in ClassInspectedTable)
                    KeyList += $"\r\n{Entry.Key.GetType().FullName}({Entry.Key.GetHashCode()}) {Entry.Value.GetType().FullName}({Entry.Value.GetHashCode()})";

                Analyzer.Trace(KeyList, TraceLevel);

                if (!ClassInspectedTable.ContainsKey(Root))
                {
                    Analyzer.Trace("Traces 08", TraceLevel);

                    ProgramHasMembersOutsideRegion = new GlobalState<bool?>();
                    Synchronizer = new ClassSynchronizer(context, TraceLevel);
                    ClassInspectedTable.Add(Root, new CompilationUnitState(ProgramHasMembersOutsideRegion, Synchronizer));

                    Analyzer.Trace($"Total added: {ClassInspectedTable.Count} context, {Synchronizer.ClassCount} classes", TraceLevel);
                }
                else
                {
                    Analyzer.Trace("Traces 09", TraceLevel);

                    var State = ClassInspectedTable[Root];

                    Analyzer.Trace("Traces 10", TraceLevel);

                    ProgramHasMembersOutsideRegion = State.ProgramHasMembersOutsideRegion;
                    Synchronizer = State.Synchronizer;

                    Analyzer.Trace("Traces 11", TraceLevel);
                }
            }

            Analyzer.Trace("Traces 12", TraceLevel);

            if (RegionExplorer.HasRegion)
                ProgramHasMembersOutsideRegion.Update(RegionExplorer.HasMembersOutsideRegion);

            Analyzer.Trace("Traces 13", TraceLevel);

            Synchronizer.WaitAll(TraceLevel);

            Analyzer.Trace("Traces 14", TraceLevel);

            if (!RegionExplorer.HasRegion)
            {
                Analyzer.Trace("No region to analyze, exit", TraceLevel);
                return;
            }

            // Report for classes with members outside region only.
            if (!RegionExplorer.HasMembersOutsideRegion)
            {
                Analyzer.Trace("No member outside region, exit", TraceLevel);
                return;
            }

            if (!ProgramHasMembersOutsideRegion.IsDifferent)
            {
                Analyzer.Trace("No difference with other classes, exit", TraceLevel);
                return;
            }

            Analyzer.Trace("Found classes with and without regions", TraceLevel);
            string ClassName = Node.Identifier.ValueText;
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation(), ClassName));
        }
        catch (Exception e)
        {
            Analyzer.Trace($"{e.Message}\n{e.StackTrace}", TraceLevel.Critical);

            throw e;
        }
    }

    private record CompilationUnitState(GlobalState<bool?> ProgramHasMembersOutsideRegion, ClassSynchronizer Synchronizer)
    {
    }

    private Dictionary<CompilationUnitSyntax, CompilationUnitState> ClassInspectedTable = new();
    private readonly object TableLock = new();
    #endregion
}
