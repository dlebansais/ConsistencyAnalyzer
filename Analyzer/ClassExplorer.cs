namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents an object that provides info about classes.
    /// </summary>
    public class ClassExplorer
    {
        #region Init
        /// <summary>
        /// Creates a ClassExplorer.
        /// </summary>
        /// <param name="context">The source code.</param>
        public ClassExplorer(SyntaxNodeAnalysisContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets the source code.
        /// </summary>
        public SyntaxNodeAnalysisContext Context { get; init; }

        private static Dictionary<SyntaxNodeAnalysisContext, ClassExplorer> ExplorerTable = new Dictionary<SyntaxNodeAnalysisContext, ClassExplorer>();
        #endregion

        #region Client Interface
        /// <summary>
        /// Adds a class to the known list of classes.
        /// </summary>
        /// <param name="context">The source code.</param>
        /// <param name="classDeclaration">The class to add.</param>
        public static void AddClass(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            lock (InternalLock)
            {
                if (!ExplorerTable.ContainsKey(context))
                    ExplorerTable.Add(context, new ClassExplorer(context));

                ExplorerTable[context].AddClass(classDeclaration);
            }

            ClassAdded.Set();
        }

        private void AddClass(ClassDeclarationSyntax classDeclaration)
        {
            List<MemberDeclarationSyntax> MemberList;

            if (ClassToMemberTable.ContainsKey(classDeclaration))
                return;

            MemberList = new List<MemberDeclarationSyntax>();
            ClassToMemberTable.Add(classDeclaration, MemberList);

            SyntaxToken CurrentToken = classDeclaration.OpenBraceToken;

            for (; ; )
            {
                CurrentToken = CurrentToken.GetNextToken(includeZeroWidth: false, includeSkipped: false, includeDirectives: true, includeDocumentationComments: false);

                if (CurrentToken == classDeclaration.CloseBraceToken)
                    break;

                if (CurrentToken.Parent is MemberDeclarationSyntax AsMemberDeclaration)
                {
                    if (!MemberToClassTable.ContainsKey(AsMemberDeclaration))
                    {
                        MemberToClassTable.Add(AsMemberDeclaration, classDeclaration);
                        MemberList.Add(AsMemberDeclaration);
                    }
                }
            }

            Analyzer.Trace($"Class {classDeclaration.Identifier} has {MemberList.Count} members");

            RegionExplorer Explorer = new RegionExplorer(Context, classDeclaration);
            RegionExplorerTable.Add(classDeclaration, Explorer);
        }

        /// <summary>
        /// Gets the class owning a member.
        /// </summary>
        /// <param name="context">The source code.</param>
        /// <param name="memberDeclaration">The member.</param>
        public static ClassDeclarationSyntax GetClass(SyntaxNodeAnalysisContext context, MemberDeclarationSyntax memberDeclaration)
        {
            while (!ExplorerTable.ContainsKey(context))
                ClassAdded.WaitOne(TimeSpan.Zero);

            return ExplorerTable[context].GetClass(memberDeclaration);
        }

        private ClassDeclarationSyntax GetClass(MemberDeclarationSyntax memberDeclaration)
        {
            while (!MemberToClassTable.ContainsKey(memberDeclaration))
                ClassAdded.WaitOne(TimeSpan.Zero);

            return MemberToClassTable[memberDeclaration];
        }

        /// <summary>
        /// Gets the region epxlorer of a class.
        /// </summary>
        /// <param name="context">The source code.</param>
        /// <param name="classDeclaration">The class.</param>
        public static RegionExplorer GetRegionExplorer(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            return ExplorerTable[context].RegionExplorerTable[classDeclaration];
        }

        /// <summary>
        /// Gets the type of a member.
        /// </summary>
        /// <param name="memberDeclaration"></param>
        /// <returns></returns>
        public static MemberTypes GetMemberType(MemberDeclarationSyntax memberDeclaration)
        {

            switch (memberDeclaration)
            {
                case ConstructorDeclarationSyntax:
                    return MemberTypes.Contructor;
                case FieldDeclarationSyntax:
                    return MemberTypes.Field;
                case PropertyDeclarationSyntax:
                    return MemberTypes.Property;

                default:
                case MethodDeclarationSyntax:
                    return MemberTypes.Method;
            }
        }

        private static int[] InternalLock = new int[0];
        private static AutoResetEvent ClassAdded = new AutoResetEvent(false);
        private Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>> ClassToMemberTable { get; } = new Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>>();
        private Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax> MemberToClassTable { get; } = new Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax>();
        private Dictionary<ClassDeclarationSyntax, RegionExplorer> RegionExplorerTable { get; } = new Dictionary<ClassDeclarationSyntax, RegionExplorer>();
        #endregion
    }
}
