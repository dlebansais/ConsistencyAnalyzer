namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rule of the analyzer.
    /// </summary>
    public class AnalyzerRuleConA1203 : SingleSyntaxAnalyzerRule
    {
        #region Properties
        /// <summary>
        /// Gets the rule id.
        /// </summary>
        public override string Id { get; } = ToRuleId(nameof(AnalyzerRuleConA1203));

        /// <summary>
        /// Gets the kind of syntax this rule analyzes.
        /// </summary>
        public override SyntaxKind RuleSyntaxKind { get; } = SyntaxKind.UsingDirective;
        #endregion

        #region Ancestor Interface
        /// <summary>
        /// Gets the rule title.
        /// </summary>
        protected override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ConA1203Title), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule message format.
        /// </summary>
        protected override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ConA1203MessageFormat), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule description.
        /// </summary>
        protected override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ConA1203Description), Resources.ResourceManager, typeof(Resources));

        /// <summary>
        /// Gets the rule category.
        /// </summary>
        protected override string Category { get; } = "Usage";

        /// <summary>
        /// Gets a value indicating whether using directives should appear before namespace declaration.
        /// </summary>
        public bool? IsOutsideUsingExpected { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Analyzes a source code node.
        /// </summary>
        /// <param name="context">The source code.</param>
        public override void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            TraceLevel TraceLevel = TraceLevel.Info;
            Analyzer.Trace("AnalyzerRuleConA1203", TraceLevel);

            try
            {
                UsingDirectiveSyntax Node = (UsingDirectiveSyntax)context.Node;

                if (UsingExplorer.IsSystemUsing(Node))
                {
                    Analyzer.Trace($"Using directive for the System namespace, exit", TraceLevel);
                    return;
                }

                ContextExplorer ContextExplorer = ContextExplorer.Get(context, TraceLevel);
                UsingExplorer Explorer = ContextExplorer.UsingExplorer;

                bool? IsSystemUsingFirstExpected = Explorer.IsSystemUsingFirstExpected;

                if (IsSystemUsingFirstExpected == null)
                {
                    Analyzer.Trace($"Using directive order for System undecided, exit", TraceLevel);
                    return;
                }

                if (IsSystemUsingFirstExpected.Value == false)
                {
                    Analyzer.Trace($"Using directive order is just alphabetical, exit", TraceLevel);
                    return;
                }

                SyntaxList<UsingDirectiveSyntax> Usings;
                switch (Node.Parent)
                {
                    case CompilationUnitSyntax AsCompilationUnit:
                        Usings = AsCompilationUnit.Usings;
                        break;
                    case NamespaceDeclarationSyntax AsNamespaceDeclaration:
                        Usings = AsNamespaceDeclaration.Usings;
                        break;
                    default:
                        Analyzer.Trace($"Unsupported using directive parent, exit", TraceLevel);
                        return;
                }

                int NodeIndex = Usings.IndexOf(Node);
                if (NodeIndex < 0)
                {
                    Analyzer.Trace($"Inconsistent syntax, exit", TraceLevel);
                    return;
                }

                bool IsAppearingBeforeLastSystem = false;

                for (int i = NodeIndex + 1; i < Usings.Count; i++)
                    if (UsingExplorer.IsSystemUsing(Usings[i]))
                    {
                        IsAppearingBeforeLastSystem = true;
                        break;
                    }

                if (!IsAppearingBeforeLastSystem)
                {
                    Analyzer.Trace($"Using directive at the right place, exit", TraceLevel);
                    return;
                }

                Analyzer.Trace($"Using directive at the wrong place, must be after directives for the System namespace", TraceLevel);
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, Node.GetLocation()));
            }
            catch (Exception e)
            {
                Analyzer.Trace(e.Message, TraceLevel.Critical);
                Analyzer.Trace(e.StackTrace, TraceLevel.Critical);

                throw e;
            }
        }
        #endregion
    }
}
