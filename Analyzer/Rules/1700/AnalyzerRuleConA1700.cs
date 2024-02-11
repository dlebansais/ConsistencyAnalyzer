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
            ClassDeclarationSyntax Node = (ClassDeclarationSyntax)context.Node;
            Analyzer.Trace("1", TraceLevel);
            IEnumerable<SyntaxNode> Nodes = Node.AncestorsAndSelf();
            Analyzer.Trace("2", TraceLevel);

            if (IsSystemMicrosoftNamespace(Nodes, TraceLevel))
                return;
            Analyzer.Trace("3", TraceLevel);

            ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
            Analyzer.Trace("4", TraceLevel);
            RegionExplorer RegionExplorer = ContextExplorer.GetRegionExplorer(Node);
            Analyzer.Trace("5", TraceLevel);
            CompilationUnitSyntax Root = Nodes.OfType<CompilationUnitSyntax>().First();
            Analyzer.Trace("6", TraceLevel);

            SetOrAddState(context, Root, TraceLevel, out GlobalState<bool?> ProgramHasMembersOutsideRegion, out ClassSynchronizer Synchronizer);
            Analyzer.Trace("7", TraceLevel);

            if (RegionExplorer.HasRegion)
                ProgramHasMembersOutsideRegion.Update(RegionExplorer.HasMembersOutsideRegion);
            Analyzer.Trace("8", TraceLevel);

            Synchronizer.WaitAll(TraceLevel);
            Analyzer.Trace("9", TraceLevel);

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
            Analyzer.Trace("10", TraceLevel);
            Analyzer.Trace($"{e.Message}\n{e.StackTrace}", TraceLevel.Critical);

            throw e;
        }
    }

    private bool IsSystemMicrosoftNamespace(IEnumerable<SyntaxNode> nodes, TraceLevel traceLevel)
    {
        BaseNamespaceDeclarationSyntax? Namespace = nodes.OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault();
        if (Namespace is not null)
        {
            string NamespaceString = Namespace.Name.ToString();

            if (NamespaceString == "System" ||
                NamespaceString.StartsWith("System.") ||
                NamespaceString == "Microsoft" ||
                NamespaceString.StartsWith("Microsoft."))
            {
                Analyzer.Trace("System node, exit", traceLevel);
                return true;
            }
        }

        return false;
    }

    private void SetOrAddState(SyntaxNodeAnalysisContext context, CompilationUnitSyntax root, TraceLevel traceLevel, out GlobalState<bool?> programHasMembersOutsideRegion, out ClassSynchronizer synchronizer)
    {
        lock (TableLock)
        {
            if (ClassInspectedTable.TryGetValue(root, out CompilationUnitState State))
            {
                programHasMembersOutsideRegion = State.ProgramHasMembersOutsideRegion;
                synchronizer = State.Synchronizer;
            }
            else
            {
                programHasMembersOutsideRegion = new GlobalState<bool?>();
                synchronizer = new ClassSynchronizer(context, traceLevel);
                ClassInspectedTable.Add(root, new CompilationUnitState(programHasMembersOutsideRegion, synchronizer));

                Analyzer.Trace($"Total added: {ClassInspectedTable.Count} context, {synchronizer.ClassCount} classes", traceLevel);
            }
        }
    }

    private record CompilationUnitState(GlobalState<bool?> ProgramHasMembersOutsideRegion, ClassSynchronizer Synchronizer)
    {
    }

    private Dictionary<CompilationUnitSyntax, CompilationUnitState> ClassInspectedTable = new();
    private readonly object TableLock = new();
    #endregion
}
