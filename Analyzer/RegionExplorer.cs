namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using StyleCop.Analyzers.Helpers;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an object that provides info about regions.
    /// </summary>
    public class RegionExplorer
    {
        #region Init
        /// <summary>
        /// Creates an instance of RegionExplorer.
        /// </summary>
        /// <param name="classDeclaration">The class with regions.</param>
        public RegionExplorer(ClassDeclarationSyntax classDeclaration)
        {
            ClassDeclaration = classDeclaration;

            Explore();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the class with regions.
        /// </summary>
        public ClassDeclarationSyntax ClassDeclaration { get; init; }

        /// <summary>
        /// Gets a value indicating whether the class has regions.
        /// </summary>
        public bool HasRegion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the class has regions.
        /// </summary>
        public bool HasMembersOutsideRegion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the class has regions.
        /// </summary>
        internal Dictionary<AccessLevel, List<RegionDirectiveTriviaSyntax>> RegionsByAccelLevel { get; } = new Dictionary<AccessLevel, List<RegionDirectiveTriviaSyntax>>();

        /// <summary>
        /// Gets a value indicating whether the class has regions.
        /// </summary>
        internal Dictionary<MemberDeclarationSyntax, RegionDirectiveTriviaSyntax> MemberRegionTable { get; } = new Dictionary<MemberDeclarationSyntax, RegionDirectiveTriviaSyntax>();
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the name of a region.
        /// </summary>
        /// <param name="regionDirective">The region.</param>
        public static string GetRegionText(RegionDirectiveTriviaSyntax regionDirective)
        {
            string Result = regionDirective.ToString();
            string RegionPattern = "#region ";

            if (Result.StartsWith(RegionPattern))
                Result = Result.Substring(RegionPattern.Length);

            return Result;
        }
        #endregion

        #region Implementation
        private void Explore()
        {
            SyntaxToken CurrentToken = ClassDeclaration.OpenBraceToken;
            int RegionNestedLevel = 0;
            RegionDirectiveTriviaSyntax? LastRegionDirective = null;

            for (;;)
            {
                CurrentToken = CurrentToken.GetNextToken(includeZeroWidth: false, includeSkipped: false, includeDirectives: true, includeDocumentationComments: false);

                if (CurrentToken == ClassDeclaration.CloseBraceToken)
                    break;

                if (CurrentToken.Parent is RegionDirectiveTriviaSyntax AsRegionDirective)
                {
                    if (AsRegionDirective.HashToken == CurrentToken)
                    {
                        HasRegion = true;
                        RegionNestedLevel++;
                        LastRegionDirective = AsRegionDirective;
                    }
                }
                else if (CurrentToken.Parent is EndRegionDirectiveTriviaSyntax AsEndRegionDirective)
                {
                    if (AsEndRegionDirective.HashToken == CurrentToken)
                        RegionNestedLevel--;
                }
                else if (CurrentToken.Parent is MemberDeclarationSyntax AsMemberDeclaration)
                {
                    if (RegionNestedLevel == 0)
                    {
                        if (!HasMembersOutsideRegion)
                            HasMembersOutsideRegion = true;
                    }
                    else if (!MemberRegionTable.ContainsKey(AsMemberDeclaration))
                    {
                        AccessLevel AccessLevel = AccessLevelHelper.GetAccessLevel(AsMemberDeclaration.Modifiers);
                        RegionDirectiveTriviaSyntax MemberRegion = LastRegionDirective!;

                        if (!RegionsByAccelLevel.ContainsKey(AccessLevel))
                            RegionsByAccelLevel.Add(AccessLevel, new List<RegionDirectiveTriviaSyntax>());

                        List<RegionDirectiveTriviaSyntax> RegionList = RegionsByAccelLevel[AccessLevel];
                        if (!RegionList.Contains(MemberRegion))
                            RegionList.Add(MemberRegion);

                        MemberRegionTable.Add(AsMemberDeclaration, MemberRegion);
                    }
                }
            }
        }
        #endregion
    }
}
