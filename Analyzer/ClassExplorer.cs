namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an object that provides info about classes.
    /// </summary>
    public static class ClassExplorer
    {
        #region Init
        static ClassExplorer()
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Adds a class to the known list of classes.
        /// </summary>
        /// <param name="classDeclaration">The class to add.</param>
        public static void AddClass(ClassDeclarationSyntax classDeclaration)
        {
            List<MemberDeclarationSyntax> MemberList;

            lock (InternalLock)
            {
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

                RegionExplorer Explorer = new RegionExplorer(classDeclaration);
                RegionExplorerTable.Add(classDeclaration, Explorer);
            }

            Analyzer.Trace($"Class {classDeclaration.Identifier} has {ClassToMemberTable[classDeclaration].Count} members");
            ClassAdded.Set();
        }

        /// <summary>
        /// Gets the class owning a member.
        /// </summary>
        /// <param name="memberDeclaration">The member.</param>
        public static ClassDeclarationSyntax GetClass(MemberDeclarationSyntax memberDeclaration)
        {
            while (!MemberToClassTable.ContainsKey(memberDeclaration))
                ClassAdded.WaitOne(TimeSpan.Zero);

            return MemberToClassTable[memberDeclaration];
        }

        /// <summary>
        /// Gets the region epxlorer of a class.
        /// </summary>
        /// <param name="classDeclaration">The class.</param>
        public static RegionExplorer GetRegionExplorer(ClassDeclarationSyntax classDeclaration)
        {
            while (!RegionExplorerTable.ContainsKey(classDeclaration))
                ClassAdded.WaitOne(TimeSpan.Zero);

            return RegionExplorerTable[classDeclaration];
        }

        private static int[] InternalLock = new int[0];
        private static AutoResetEvent ClassAdded = new AutoResetEvent(false);
        private static Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>> ClassToMemberTable { get; } = new Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>>();
        private static Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax> MemberToClassTable { get; } = new Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax>();
        private static Dictionary<ClassDeclarationSyntax, RegionExplorer> RegionExplorerTable { get; } = new Dictionary<ClassDeclarationSyntax, RegionExplorer>();
        #endregion
    }
}
