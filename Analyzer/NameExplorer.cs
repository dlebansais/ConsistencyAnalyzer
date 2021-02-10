namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents an object that provides info about namees.
    /// </summary>
    public partial class NameExplorer
    {
        #region Init
        /// <summary>
        /// Creates a NameExplorer.
        /// </summary>
        /// <param name="context">The source code.</param>
        /// <param name="traceLevel">The trace level.</param>
        public NameExplorer(SyntaxNodeAnalysisContext context, TraceLevel traceLevel)
        {
            Context = context;

            CompilationUnitSyntax CompilationUnit = (CompilationUnitSyntax)context.SemanticModel.SyntaxTree.GetRoot();

            ParseCompilationUnit(CompilationUnit);
        }
        #endregion

        #region Name Parsing
        private void ParseIdentifier(SyntaxToken identifier, NameCategory nameCategory)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source code.
        /// </summary>
        public SyntaxNodeAnalysisContext Context { get; init; }
        #endregion

        #region Client Interface
        #endregion
    }
}
