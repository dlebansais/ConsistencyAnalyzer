namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
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
        /// <param name="context">The source code.</param>
        /// <param name="classDeclaration">The class with regions.</param>
        public RegionExplorer(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            Context = context;
            ClassDeclaration = classDeclaration;

            Explore();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source code.
        /// </summary>
        public SyntaxNodeAnalysisContext Context { get; init; }

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

        /// <summary>
        /// Gets members types in classes.
        /// </summary>
        internal static Dictionary<ClassDeclarationSyntax, Dictionary<MemberTypes, List<RegionDirectiveTriviaSyntax>>> SortedRegionTable { get; } = new Dictionary<ClassDeclarationSyntax, Dictionary<MemberTypes, List<RegionDirectiveTriviaSyntax>>>();
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

        internal static bool IsRegionMismatch(SyntaxNodeAnalysisContext context, MemberDeclarationSyntax memberDeclaration, AccessLevel expectedAccessLevel, out string expectedRegionText, out string memberText)
        {
            expectedRegionText = string.Empty;
            memberText = string.Empty;

            ClassDeclarationSyntax ClassDeclaration = (ClassDeclarationSyntax)memberDeclaration.Parent!;
            ClassExplorer.AddClass(context, ClassDeclaration);

            if (RegionMode != RegionModes.InterfaceCategorySimple && RegionMode != RegionModes.InterfaceCategoryFull)
            {
                Analyzer.Trace($"Region mode is {RegionMode}, exit");
                return false;
            }

            AccessLevel MemberAccessLevel = AccessLevelHelper.GetAccessLevel(memberDeclaration.Modifiers);
            if (MemberAccessLevel == AccessLevel.ProtectedInternal)
                MemberAccessLevel = AccessLevel.Protected;

            if (MemberAccessLevel != expectedAccessLevel)
            {
                Analyzer.Trace($"Member Access Level is {MemberAccessLevel}, exit");
                return false;
            }

            RegionExplorer Explorer = ClassExplorer.GetRegionExplorer(context, ClassDeclaration);

            if (!Explorer.RegionsByAccelLevel.ContainsKey(expectedAccessLevel))
            {
                Analyzer.Trace($"No other member with the same access level, exit");
                return false;
            }

            List<RegionDirectiveTriviaSyntax> RegionList = Explorer.RegionsByAccelLevel[expectedAccessLevel];
            if (RegionList.Count <= 1)
            {
                Analyzer.Trace($"Only one region with the members with same access level, exit");
                return false;
            }

            RegionDirectiveTriviaSyntax FirstRegion = RegionList[0];
            RegionDirectiveTriviaSyntax MemberRegion = Explorer.MemberRegionTable[memberDeclaration];

            if (MemberRegion == FirstRegion)
            {
                Analyzer.Trace($"Analyzing the first region, exit");
                return false;
            }

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
                case PropertyDeclarationSyntax AsPropertyDeclaration:
                    memberText = AsPropertyDeclaration.Identifier.ToString();
                    break;
                case MethodDeclarationSyntax AsMethodDeclaration:
                    memberText = AsMethodDeclaration.Identifier.ToString();
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
                        MemberClassification(AsMemberDeclaration, LastRegionDirective!);
                    }
                }
            }

            int ClassWithContentCount = GetCountOfClassWithContent();

            if (ClassWithContentCount < 3)
            {
                Analyzer.Trace("Not enough classes with content to set region mode");
                RegionMode = RegionModes.Undecided;
            }
            else
            {
                CheckInterfaceCategoryRegionMode(ClassWithContentCount);

                Analyzer.Trace($"Region mode: {RegionMode}");
            }
        }

        private void MemberClassification(MemberDeclarationSyntax memberDeclaration, RegionDirectiveTriviaSyntax memberRegion)
        {
            AccessLevel MemberAccessLevel = AccessLevelHelper.GetAccessLevel(memberDeclaration.Modifiers);
            if (MemberAccessLevel == AccessLevel.ProtectedInternal)
                MemberAccessLevel = AccessLevel.Protected;

            if (!RegionsByAccelLevel.ContainsKey(MemberAccessLevel))
                RegionsByAccelLevel.Add(MemberAccessLevel, new List<RegionDirectiveTriviaSyntax>());

            List<RegionDirectiveTriviaSyntax> RegionList = RegionsByAccelLevel[MemberAccessLevel];
            if (!RegionList.Contains(memberRegion))
                RegionList.Add(memberRegion);

            MemberRegionTable.Add(memberDeclaration, memberRegion);

            if (!RegionMemberTable.ContainsKey(memberRegion))
                RegionMemberTable.Add(memberRegion, new List<MemberDeclarationSyntax>());

            List<MemberDeclarationSyntax> MemberList = RegionMemberTable[memberRegion];
            MemberList.Add(memberDeclaration);

            ClassDeclarationSyntax OwnerClass = ClassExplorer.GetClass(Context, memberDeclaration);
            if (!SortedRegionTable.ContainsKey(OwnerClass))
                SortedRegionTable.Add(OwnerClass, new Dictionary<MemberTypes, List<RegionDirectiveTriviaSyntax>>());

            Dictionary<MemberTypes, List<RegionDirectiveTriviaSyntax>> RegionByMemberType = SortedRegionTable[OwnerClass];
            MemberTypes MemberType = ClassExplorer.GetMemberType(memberDeclaration);

            if (!RegionByMemberType.ContainsKey(MemberType))
                RegionByMemberType.Add(MemberType, new List<RegionDirectiveTriviaSyntax>());

            List<RegionDirectiveTriviaSyntax> RegionListForThisType = RegionByMemberType[MemberType];
            if (!RegionListForThisType.Contains(memberRegion))
                RegionListForThisType.Add(memberRegion);
        }

        private int GetCountOfClassWithContent()
        {
            int Count = 0;

            foreach (KeyValuePair<ClassDeclarationSyntax, Dictionary<MemberTypes, List<RegionDirectiveTriviaSyntax>>> ClassEntry in SortedRegionTable)
            {
                if (ClassEntry.Value.Count > 0)
                    Count++;
            }

            return Count;
        }

        private void CheckInterfaceCategoryRegionMode(int classWithContentCount)
        {
            Dictionary<MemberTypes, int> DispersedTypeTable = new Dictionary<MemberTypes, int>();
            foreach (MemberTypes Value in typeof(MemberTypes).GetEnumValues())
                DispersedTypeTable.Add(Value, 0);

            foreach (KeyValuePair<ClassDeclarationSyntax, Dictionary<MemberTypes, List<RegionDirectiveTriviaSyntax>>> ClassEntry in SortedRegionTable)
            {
                foreach (KeyValuePair<MemberTypes, List<RegionDirectiveTriviaSyntax>> TypeEntry in ClassEntry.Value)
                {
                    MemberTypes MemberType = TypeEntry.Key;
                    List<RegionDirectiveTriviaSyntax> RegionList = TypeEntry.Value;

                    if (RegionList.Count > 1)
                    {
                        DispersedTypeTable[MemberType]++;
                    }
                }
            }

            int MaxDispersedCount = 0;
            foreach (KeyValuePair<MemberTypes, int> Entry in DispersedTypeTable)
                if (MaxDispersedCount < Entry.Value)
                    MaxDispersedCount = Entry.Value;

            int MinConsistencyCount = (classWithContentCount + 1) / 2;

            if (MaxDispersedCount < classWithContentCount - MinConsistencyCount)
                RegionMode = RegionModes.InterfaceCategoryFull;
            else
                RegionMode = RegionModes.Undecided;
        }
        #endregion
    }
}
