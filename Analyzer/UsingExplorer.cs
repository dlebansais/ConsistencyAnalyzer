namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents an object that provides info about using directives.
    /// </summary>
    public class UsingExplorer
    {
        #region Init
        /// <summary>
        /// Creates a UsingExplorer.
        /// </summary>
        /// <param name="compilationUnit">The source code.</param>
        /// <param name="context">A context for source code analysis.</param>
        /// <param name="traceLevel">The trace level.</param>
        public UsingExplorer(CompilationUnitSyntax compilationUnit, SyntaxNodeAnalysisContext? context, TraceLevel traceLevel)
        {
            CompilationUnit = compilationUnit;
            Context = context;

            foreach (UsingDirectiveSyntax UsingDirective in compilationUnit.Usings)
                RecordUsing(string.Empty, UsingDirective);

            foreach (MemberDeclarationSyntax MemberDeclaration in compilationUnit.Members)
                if (MemberDeclaration is NamespaceDeclarationSyntax AsNamespaceDeclaration)
                    ParseNamespaceDeclaration(AsNamespaceDeclaration);
        }

        private void ParseNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            string namespaceName = NameExplorer.NamespaceNameToString(namespaceDeclaration);

            foreach (UsingDirectiveSyntax UsingDirective in namespaceDeclaration.Usings)
                RecordUsing(namespaceName, UsingDirective);
        }

        private void RecordUsing(string namespaceName, UsingDirectiveSyntax usingDirective)
        {
            string FileName = usingDirective.SyntaxTree.FilePath;
            string UsingName = NameExplorer.NameToString(usingDirective.Name);
            string Path = $"{FileName}#{namespaceName}#{UsingName}";

            if (!UsingPathTable.ContainsKey(Path))
                UsingPathTable.Add(Path, usingDirective);

            if (namespaceName.Length == 0)
                OutsideUsingCount++;
            else
                InsideUsingCount++;
        }

        private SyntaxNodeAnalysisContext? Context;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source code.
        /// </summary>
        public CompilationUnitSyntax CompilationUnit { get; init; }

        /// <summary>
        /// Gets a value indicating whether using should be outside.
        /// </summary>
        public bool? IsOutsideUsingExpected
        {
            get { return OutsideUsingCount + InsideUsingCount >= 3 ? OutsideUsingCount > InsideUsingCount : null; }
        }
        #endregion

        #region Client Interface

        private Dictionary<string, UsingDirectiveSyntax> UsingPathTable = new();
        private int OutsideUsingCount;
        private int InsideUsingCount;
        #endregion
    }
}
