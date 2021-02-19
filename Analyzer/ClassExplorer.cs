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
        /// <param name="compilationUnit">The source code.</param>
        /// <param name="context">A context for source code analysis.</param>
        /// <param name="traceLevel">The trace level.</param>
        public ClassExplorer(CompilationUnitSyntax compilationUnit, SyntaxNodeAnalysisContext? context, TraceLevel traceLevel)
        {
            CompilationUnit = compilationUnit;
            Context = context;

            List<ClassDeclarationSyntax> ClassDeclarationList = new List<ClassDeclarationSyntax>();
            AddClassMembers(CompilationUnit.Members, ClassDeclarationList);

            foreach (ClassDeclarationSyntax ClassDeclaration in ClassDeclarationList)
                AddClass(ClassDeclaration, traceLevel);
        }

        private void AddClassMembers(SyntaxList<MemberDeclarationSyntax> members, List<ClassDeclarationSyntax> classDeclarationList)
        {
            foreach (MemberDeclarationSyntax Member in members)
            {
                switch (Member)
                {
                    case NamespaceDeclarationSyntax AsNamespaceDeclaration:
                        AddClassMembers(AsNamespaceDeclaration.Members, classDeclarationList);
                        break;
                    case ClassDeclarationSyntax AsClassDeclaration:
                        classDeclarationList.Add(AsClassDeclaration);
                        break;
                }
            }
        }

        private SyntaxNodeAnalysisContext? Context;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source code.
        /// </summary>
        public CompilationUnitSyntax CompilationUnit { get; init; }
        #endregion

        #region Client Interface
        private void AddClass(ClassDeclarationSyntax classDeclaration, TraceLevel traceLevel)
        {
            List<MemberDeclarationSyntax> MemberList;

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

            Analyzer.Trace($"Class {classDeclaration.Identifier} has {MemberList.Count} members", traceLevel);
        }

        /// <summary>
        /// Gets the list of classes.
        /// </summary>
        public List<ClassDeclarationSyntax> GetClassList()
        {
            return new List<ClassDeclarationSyntax>(ClassToMemberTable.Keys);
        }

        /// <summary>
        /// Gets members of a class.
        /// </summary>
        /// <param name="classDeclaration"></param>
        /// <returns></returns>
        public List<MemberDeclarationSyntax> GetMemberList(ClassDeclarationSyntax classDeclaration)
        {
            return ClassToMemberTable[classDeclaration];
        }

        /// <summary>
        /// Gets the class owning a member.
        /// </summary>
        /// <param name="memberDeclaration">The member.</param>
        public ClassDeclarationSyntax GetMemberOwner(MemberDeclarationSyntax memberDeclaration)
        {
            return MemberToClassTable[memberDeclaration];
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

        private Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>> ClassToMemberTable { get; } = new Dictionary<ClassDeclarationSyntax, List<MemberDeclarationSyntax>>();
        private Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax> MemberToClassTable { get; } = new Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax>();
        #endregion
    }
}
