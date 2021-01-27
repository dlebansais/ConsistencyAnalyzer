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
        /// The detected region mode.
        /// </summary>
        public static RegionModes RegionMode { get; private set; } = RegionModes.InterfaceCategorySimple;

        /// <summary>
        /// Gets regions by access level.
        /// </summary>
        internal Dictionary<AccessLevel, List<RegionDirectiveTriviaSyntax>> RegionsByAccelLevel { get; } = new Dictionary<AccessLevel, List<RegionDirectiveTriviaSyntax>>();

        /// <summary>
        /// Gets regions to which members belong.
        /// </summary>
        internal Dictionary<MemberDeclarationSyntax, RegionDirectiveTriviaSyntax> MemberRegionTable { get; } = new Dictionary<MemberDeclarationSyntax, RegionDirectiveTriviaSyntax>();

        /// <summary>
        /// Gets members by region.
        /// </summary>
        internal Dictionary<RegionDirectiveTriviaSyntax, List<MemberDeclarationSyntax>> RegionMemberTable { get; } = new Dictionary<RegionDirectiveTriviaSyntax, List<MemberDeclarationSyntax>>();
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

        internal static bool IsRegionMismatch(MemberDeclarationSyntax memberDeclaration, AccessLevel expectedAccessLevel, out string expectedRegionText, out string memberText)
        {
            expectedRegionText = string.Empty;
            memberText = string.Empty;

            if (RegionMode != RegionModes.InterfaceCategorySimple)
                return false;

            AccessLevel MemberAccessLevel = AccessLevelHelper.GetAccessLevel(memberDeclaration.Modifiers);
            if (MemberAccessLevel == AccessLevel.ProtectedInternal)
                MemberAccessLevel = AccessLevel.Protected;

            if (MemberAccessLevel != expectedAccessLevel)
                return false;

            Dictionary<MemberDeclarationSyntax, ClassDeclarationSyntax> MemberToClassTable;
            Dictionary<ClassDeclarationSyntax, RegionExplorer> RegionExplorerTable;

            lock (ClassExplorer.Current)
                MemberToClassTable = ClassExplorer.Current.MemberToClassTable;

            if (!MemberToClassTable.ContainsKey(memberDeclaration))
                return false;

            ClassDeclarationSyntax ClassDeclaration = MemberToClassTable[memberDeclaration];

            lock (ClassExplorer.Current)
                RegionExplorerTable = ClassExplorer.Current.RegionExplorerTable;

            RegionExplorer Explorer;

            lock (ClassExplorer.Current)
            {
                Explorer = RegionExplorerTable[ClassDeclaration];
            }

            if (!Explorer.RegionsByAccelLevel.ContainsKey(expectedAccessLevel))
                return false;

            List<RegionDirectiveTriviaSyntax> RegionList = Explorer.RegionsByAccelLevel[expectedAccessLevel];
            if (RegionList.Count <= 1)
                return false;

            RegionDirectiveTriviaSyntax FirstRegion = RegionList[0];
            RegionDirectiveTriviaSyntax MemberRegion = Explorer.MemberRegionTable[memberDeclaration];

            if (MemberRegion == FirstRegion)
                return false;

            expectedRegionText = RegionExplorer.GetRegionText(FirstRegion);
            memberText = "<Unknown>";

            switch (memberDeclaration)
            {
                case ConstructorDeclarationSyntax AsConstructorDeclaration:
                    memberText = AsConstructorDeclaration.Identifier.ToString();
                    break;
                case FieldDeclarationSyntax AsFieldDeclaration:
                    memberText = AsFieldDeclaration.Declaration.Variables[0].Identifier.ToString();
                    break;
                case MethodDeclarationSyntax AsMethodDeclaration:
                    memberText = AsMethodDeclaration.Identifier.ToString();
                    break;
                case PropertyDeclarationSyntax AsPropertyDeclaration:
                    memberText = AsPropertyDeclaration.Identifier.ToString();
                    break;
            }

            return true;
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
                        AccessLevel MemberAccessLevel = AccessLevelHelper.GetAccessLevel(AsMemberDeclaration.Modifiers);
                        if (MemberAccessLevel == AccessLevel.ProtectedInternal)
                            MemberAccessLevel = AccessLevel.Protected;

                        RegionDirectiveTriviaSyntax MemberRegion = LastRegionDirective!;

                        if (!RegionsByAccelLevel.ContainsKey(MemberAccessLevel))
                            RegionsByAccelLevel.Add(MemberAccessLevel, new List<RegionDirectiveTriviaSyntax>());

                        List<RegionDirectiveTriviaSyntax> RegionList = RegionsByAccelLevel[MemberAccessLevel];
                        if (!RegionList.Contains(MemberRegion))
                            RegionList.Add(MemberRegion);

                        MemberRegionTable.Add(AsMemberDeclaration, MemberRegion);

                        if (!RegionMemberTable.ContainsKey(MemberRegion))
                            RegionMemberTable.Add(MemberRegion, new List<MemberDeclarationSyntax>());

                        List<MemberDeclarationSyntax> MemberList = RegionMemberTable[MemberRegion];
                        MemberList.Add(AsMemberDeclaration);
                    }
                }
            }
        }
        #endregion
    }
}
