namespace ConsistencyAnalyzer;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

/// <summary>
/// Represents a code fix for a rule of the analyzer.
/// </summary>
public class CodeFixConA1701 : CodeFix
{
    /// <summary>
    /// Creates an instance of the <see cref="CodeFix"/> class.
    /// </summary>
    /// <param name="rule">The associated rule.</param>
    public CodeFixConA1701(AnalyzerRule rule)
        : base(rule)
    {
    }

    private async Task<Document> AsyncHandler(Document document, RegionDirectiveTriviaSyntax syntaxNode, CancellationToken cancellationToken)
    {
        TraceLevel TraceLevel = TraceLevel.Info;
        Analyzer.Trace("CodeFixConA1701", TraceLevel);

        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
            return document;

        RegionRemover Remover = new RegionRemover(syntaxNode);
        SyntaxNode? UpdatedRoot = Remover.Visit(root);
        Document Result = document.WithSyntaxRoot(UpdatedRoot);

        Analyzer.Trace("Fixed", TraceLevel);

        return Result;
    }

    /// <summary>
    /// Creates the action to perform to fix a document.
    /// </summary>
    /// <returns></returns>
    public override CodeAction? CreateDocumentHandler(CodeFixContext context, SyntaxNode root, TextSpan diagnosticSpan)
    {
        if (root == null)
            return null;

        SyntaxToken DiagnosticToken = root.FindToken(diagnosticSpan.Start, findInsideTrivia: true);
        if (DiagnosticToken.Parent == null)
            return null;

        IEnumerable<SyntaxNode> Nodes = DiagnosticToken.Parent.AncestorsAndSelf();
        RegionDirectiveTriviaSyntax Node = Nodes.OfType<RegionDirectiveTriviaSyntax>().First();

        var Action = CodeAction.Create(title: CodeFixResources.ConA1701FixTitle,
                createChangedDocument: c => AsyncHandler(context.Document, Node, c),
                equivalenceKey: nameof(CodeFixResources.ConA1701FixTitle));

        return Action;
    }
}
