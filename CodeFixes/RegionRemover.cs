namespace ConsistencyAnalyzer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// Remove #region and #endregion from code.
/// </summary>
public class RegionRemover : CSharpSyntaxRewriter
{
    /// <summary>
    /// Creates an instance of the RegionRemover class.
    /// </summary>
    /// <param name="regionDirective">The starting region node.</param>
    public RegionRemover(RegionDirectiveTriviaSyntax regionDirective)
        : base(true)
    {
        RemovedRegionDirective = regionDirective;
    }

    /// <summary>
    /// Gets the starting region node.
    /// </summary>
    public RegionDirectiveTriviaSyntax RemovedRegionDirective { get; init; }

    /// <summary>
    /// Visits the class with regions.
    /// </summary>
    /// <param name="classDeclaration">The class where to remove the region.</param>
    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax classDeclaration)
    {
        SyntaxToken CurrentToken = classDeclaration.OpenBraceToken;
        int RegionNestedLevel = 0;
        int RegionRemoveLevel = -1;

        for (; ; )
        {
            CurrentToken = CurrentToken.GetNextToken(includeZeroWidth: false, includeSkipped: false, includeDirectives: true, includeDocumentationComments: false);

            if (CurrentToken == classDeclaration.CloseBraceToken)
                break;

            if (CurrentToken.Parent is RegionDirectiveTriviaSyntax AsRegionDirective)
            {
                if (AsRegionDirective.HashToken == CurrentToken)
                {
                    if (AsRegionDirective == RemovedRegionDirective)
                        RegionRemoveLevel = RegionNestedLevel;

                    RegionNestedLevel++;
                }
            }
            else if (CurrentToken.Parent is EndRegionDirectiveTriviaSyntax AsEndRegionDirective)
            {
                if (AsEndRegionDirective.HashToken == CurrentToken)
                {
                    RegionNestedLevel--;

                    if (RegionRemoveLevel == RegionNestedLevel)
                        RemovedEndRegionDirective = AsEndRegionDirective;
                }
            }
        }

        return base.VisitClassDeclaration(classDeclaration);
    }

    /// <summary>
    /// Removes the #region directive.
    /// </summary>
    /// <param name="regionDirective">The directive to remove.</param>
    public override SyntaxNode? VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax regionDirective)
    {
        if (RemovedRegionDirective == regionDirective)
            return SyntaxFactory.SkippedTokensTrivia();

        return base.VisitRegionDirectiveTrivia(regionDirective);
    }

    /// <summary>
    /// Removes the #endregion directive.
    /// </summary>
    /// <param name="endRegionDirective">The directive to remove.</param>
    public override SyntaxNode? VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax endRegionDirective)
    {
        if (RemovedEndRegionDirective == endRegionDirective)
            return SyntaxFactory.SkippedTokensTrivia();

        return base.VisitEndRegionDirectiveTrivia(endRegionDirective);
    }

    private EndRegionDirectiveTriviaSyntax? RemovedEndRegionDirective;
}
