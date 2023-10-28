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

        string Trace = "Starting traces\r\n";

        try
        {
            ClassDeclarationSyntax Node = (ClassDeclarationSyntax)context.Node;
            RegionExplorer RegionExplorer = ContextExplorer.Get(context, TraceLevel).GetRegionExplorer(Node);

            IEnumerable<SyntaxNode> Nodes = Node.AncestorsAndSelf();
            CompilationUnitSyntax Root = Nodes.OfType<CompilationUnitSyntax>().First();

            GlobalState<bool?> ProgramHasMembersOutsideRegion;
            ClassSynchronizer Synchronizer;

            lock (TableLock)
            {
                object? NullableRoot = Root;
                string KeyList = $"Root: {NullableRoot?.GetType()?.FullName}({NullableRoot?.GetHashCode()})\r\n";
                foreach (KeyValuePair<CompilationUnitSyntax, CompilationUnitState> Entry in ClassInspectedTable)
                    KeyList += $"{Entry.Key.GetType().FullName}({Entry.Key.GetHashCode()}) {Entry.Value.GetType().FullName}({Entry.Value.GetHashCode()})\r\n";

                Trace += KeyList;

                if (!ClassInspectedTable.ContainsKey(Root))
                {
                    ProgramHasMembersOutsideRegion = new GlobalState<bool?>();
                    Synchronizer = new ClassSynchronizer(context, TraceLevel);
                    ClassInspectedTable.Add(Root, new CompilationUnitState(ProgramHasMembersOutsideRegion, Synchronizer));

                    Trace += $"Total added: {ClassInspectedTable.Count} context, {Synchronizer.ClassCount} classes";
                }
                else
                {
                    Trace += "Key already exist!\r\n";
                    var State = ClassInspectedTable[Root];
                    Trace += "Table read!";

                    ProgramHasMembersOutsideRegion = State.ProgramHasMembersOutsideRegion;
                    Synchronizer = State.Synchronizer;
                }
            }

            Analyzer.Trace(Trace, TraceLevel);

            if (RegionExplorer.HasRegion)
                ProgramHasMembersOutsideRegion.Update(RegionExplorer.HasMembersOutsideRegion);

            Synchronizer.WaitAll(TraceLevel);

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
            Analyzer.Trace(e.Message, TraceLevel.Critical);
            Analyzer.Trace(Trace, TraceLevel);
            Analyzer.Trace(e.StackTrace, TraceLevel.Critical);

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
