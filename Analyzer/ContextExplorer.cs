namespace ConsistencyAnalyzer
{
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

            ClassExplorer = new ClassExplorer(context, traceLevel);

            CompilationUnitSyntax CompilationUnit = (CompilationUnitSyntax)context.SemanticModel.SyntaxTree.GetRoot();
            NameExplorer = new NameExplorer(CompilationUnit, traceLevel);

            List<ClassDeclarationSyntax> ClassList = ClassExplorer.GetClassList();
            foreach (ClassDeclarationSyntax ClassDeclaration in ClassList)
            {
                List<MemberDeclarationSyntax> MemberList = ClassExplorer.GetMemberList(ClassDeclaration);
                RegionExplorerTable.Add(ClassDeclaration, new RegionExplorer(context, ClassDeclaration, MemberList, traceLevel));
            }

            List<RegionExplorer> RegionExplorerList = new List<RegionExplorer>(RegionExplorerTable.Values);
            GlobalRegionMode = RegionExplorer.GetGlobalRegionMode(RegionExplorerList, traceLevel);

            Analyzer.Trace($"Global Region Mode: {GlobalRegionMode}", traceLevel);
        }

        private static int[] InternalLock = new int[0];
        private static Dictionary<SyntaxNodeAnalysisContext, ContextExplorer> ExplorerTable = new Dictionary<SyntaxNodeAnalysisContext, ContextExplorer>();
        private Dictionary<ClassDeclarationSyntax, RegionExplorer> RegionExplorerTable = new Dictionary<ClassDeclarationSyntax, RegionExplorer>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source code.
        /// </summary>
        public SyntaxNodeAnalysisContext Context { get; init; }

        /// <summary>
        /// Gets the class explorer.
        /// </summary>
        public ClassExplorer ClassExplorer { get; init; }

        /// <summary>
        /// Gets the name explorer.
        /// </summary>
        public NameExplorer NameExplorer { get; init; }

        /// <summary>
        /// The global region mode.
        /// </summary>
        public RegionModes GlobalRegionMode { get; init; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the region explorer for a class.
        /// </summary>
        /// <param name="classDeclaration"></param>
        /// <returns></returns>
        public RegionExplorer GetRegionExplorer(ClassDeclarationSyntax classDeclaration)
        {
            return RegionExplorerTable[classDeclaration];
        }
        #endregion
    }
}
