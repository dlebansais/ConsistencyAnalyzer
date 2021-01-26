namespace ConsistencyAnalyzer
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Represents a rule of the analyzer.
    /// </summary>
    public abstract class MultipleSyntaxAnalyzerRule : AnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public abstract List<SyntaxKind> RuleSyntaxKinds { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override SyntaxKind[] GetRuleSyntaxKinds()
        {
            return RuleSyntaxKinds.ToArray();
        }
        #endregion
    }
}
