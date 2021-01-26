namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an object that provides info about classes.
    /// </summary>
    public class ClassExplorer
    {
        #region Init
        /// <summary>
        /// Gets the class explorer.
        /// </summary>
        public static ClassExplorer Current { get; } = new ClassExplorer();

        /// <summary>
        /// Creates an instance of ClassExplorer.
        /// </summary>
        private ClassExplorer()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the table of members per classes.
        /// </summary>
        public Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>> ClassToMemberTable { get; } = new Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>>();

        /// <summary>
        /// Gets the table of classes per members.
        /// </summary>
        public Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax> MemberToClassTable { get; } = new Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax>();

        /// <summary>
        /// Gets the table of class explorers.
        /// </summary>
        public Dictionary<ClassDeclarationSyntax, RegionExplorer> RegionExplorerTable { get; } = new Dictionary<ClassDeclarationSyntax, RegionExplorer>();
        #endregion

        #region Client Interface
        /// <summary>
        /// Adds a class to the known list of classes.
        /// </summary>
        /// <param name="classDeclaration">The class to add.</param>
        public void AddClass(ClassDeclarationSyntax classDeclaration)
        {
            if (ClassToMemberTable.ContainsKey(classDeclaration))
                return;

            List<MemberDeclarationSyntax> MemberList = new List<MemberDeclarationSyntax>();
            ClassToMemberTable.Add(classDeclaration, MemberList);

            SyntaxToken CurrentToken = classDeclaration.OpenBraceToken;

            for (;;)
            {
                CurrentToken = CurrentToken.GetNextToken(includeZeroWidth: false, includeSkipped: false, includeDirectives: true, includeDocumentationComments: false);

                if (CurrentToken == classDeclaration.CloseBraceToken)
                    break;

                if (CurrentToken.Parent is MemberDeclarationSyntax AsMemberDeclaration)
                {
                    MemberList.Add(AsMemberDeclaration);

                    if (!MemberToClassTable.ContainsKey(AsMemberDeclaration))
                        MemberToClassTable.Add(AsMemberDeclaration, classDeclaration);
                }
            }

            RegionExplorer Explorer = new RegionExplorer(classDeclaration);
            RegionExplorerTable.Add(classDeclaration, Explorer);

            int TraceId = 0;
            Analyzer.Trace($"Class {classDeclaration.Identifier} has {MemberToClassTable.Count} members", ref TraceId);
        }
        #endregion
    }
}
