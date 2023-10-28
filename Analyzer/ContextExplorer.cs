namespace ConsistencyAnalyzer;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// Represents data associated to a context.
/// </summary>
public class ContextExplorer
{
    #region Init
    /// <summary>
    /// Gets or creates the explorer associated to a context.
    /// </summary>
    /// <param name="context">The source code.</param>
    /// <param name="traceLevel">The trace level.</param>
    public static ContextExplorer Get(SyntaxNodeAnalysisContext context, TraceLevel traceLevel)
    {
        lock (InternalLock)
        {
            if (!ExplorerTable.ContainsKey(context))
                ExplorerTable.Add(context, new ContextExplorer(context, traceLevel));
        }

        return ExplorerTable[context];
    }

    /// <summary>
    /// Creates a ContextExplorer.
    /// </summary>
    /// <param name="context">The source code.</param>
    /// <param name="traceLevel">The trace level.</param>
    private ContextExplorer(SyntaxNodeAnalysisContext context, TraceLevel traceLevel)
    {
        Context = context;

        CompilationUnitSyntax CompilationUnit = (CompilationUnitSyntax)context.SemanticModel.SyntaxTree.GetRoot();
        ClassOrStructExplorer = new ClassOrStructExplorer(CompilationUnit, context, traceLevel);
        NameExplorer = new NameExplorer(CompilationUnit, context, traceLevel);
        UsingExplorer = new UsingExplorer(CompilationUnit, context, traceLevel);

        List<TypeDeclarationSyntax> ClassOrStructList = ClassOrStructExplorer.GetClassOrStructList();
        foreach (TypeDeclarationSyntax ClassOrStructDeclaration in ClassOrStructList)
        {
            Analyzer.Trace($"Adding member: {ClassOrStructDeclaration.Identifier}", traceLevel);

            List<MemberDeclarationSyntax> MemberList = ClassOrStructExplorer.GetMemberList(ClassOrStructDeclaration);
            RegionExplorerTable.Add(ClassOrStructDeclaration, new RegionExplorer(context, ClassOrStructDeclaration, MemberList, traceLevel));
        }

        List<RegionExplorer> RegionExplorerList = new List<RegionExplorer>(RegionExplorerTable.Values);
        GlobalRegionMode = RegionExplorer.GetGlobalRegionMode(RegionExplorerList, traceLevel);

        Analyzer.Trace($"Global Region Mode: {GlobalRegionMode}", traceLevel);
    }

    private static object InternalLock = new();
    private static Dictionary<SyntaxNodeAnalysisContext, ContextExplorer> ExplorerTable = new Dictionary<SyntaxNodeAnalysisContext, ContextExplorer>();
    private Dictionary<TypeDeclarationSyntax, RegionExplorer> RegionExplorerTable = new Dictionary<TypeDeclarationSyntax, RegionExplorer>();
    #endregion

    #region Properties
    /// <summary>
    /// Gets the source code.
    /// </summary>
    public SyntaxNodeAnalysisContext Context { get; init; }

    /// <summary>
    /// Gets the class or struct explorer.
    /// </summary>
    public ClassOrStructExplorer ClassOrStructExplorer { get; init; }

    /// <summary>
    /// Gets the name explorer.
    /// </summary>
    public NameExplorer NameExplorer { get; init; }

    /// <summary>
    /// Gets the using explorer.
    /// </summary>
    public UsingExplorer UsingExplorer { get; init; }

    /// <summary>
    /// The global region mode.
    /// </summary>
    public RegionModes GlobalRegionMode { get; init; }
    #endregion

    #region Client Interface
    /// <summary>
    /// Gets the region explorer for a class or struct.
    /// </summary>
    /// <param name="typeDeclaration"></param>
    /// <returns></returns>
    public RegionExplorer GetRegionExplorer(TypeDeclarationSyntax typeDeclaration)
    {
        Analyzer.Trace($"Accessing member: {typeDeclaration.Identifier}", TraceLevel.Critical);

        return RegionExplorerTable[typeDeclaration];
    }
    #endregion
}
