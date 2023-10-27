namespace ConsistencyAnalyzer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using StyleCop.Analyzers.Helpers;
using System.Collections.Generic;
using System.Threading;

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
    /// <param name="memberList">The list of class members.</param>
    /// <param name="traceLevel">The trace level.</param>
    public RegionExplorer(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration, List<MemberDeclarationSyntax> memberList, TraceLevel traceLevel)
    {
        Context = context;
        ClassDeclaration = classDeclaration;
        MemberList = memberList;

        Explore(traceLevel);
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
    /// Gets the list of class members.
    /// </summary>
    public List<MemberDeclarationSyntax> MemberList { get; init; }

    /// <summary>
    /// Gets a value indicating whether the class has regions.
    /// </summary>
    public bool HasRegion { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the class has regions.
    /// </summary>
    public bool HasMembersOutsideRegion { get; private set; }

    /// <summary>
    /// The region mode.
    /// </summary>
    public RegionModes ThisRegionMode { get; private set; } = RegionModes.Undecided;

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

    internal static bool IsRegionMismatch(SyntaxNodeAnalysisContext context, MemberDeclarationSyntax memberDeclaration, AccessLevel expectedAccessLevel, bool isSimpleAccessibilityCheck, TraceLevel traceLevel, out string expectedRegionText, out string memberText)
    {
        expectedRegionText = string.Empty;
        memberText = string.Empty;

        TypeDeclarationSyntax TypeDeclaration = (TypeDeclarationSyntax)memberDeclaration.Parent!;
        ContextExplorer ContextExplorer = ContextExplorer.Get(context, traceLevel);
        RegionModes GlobalRegionMode = ContextExplorer.GlobalRegionMode;

        // Another diagnostic will apply if not in a particular mode.
        if ((GlobalRegionMode != RegionModes.AccessibilitySimple || !isSimpleAccessibilityCheck) && (GlobalRegionMode != RegionModes.AccessibilityFull || isSimpleAccessibilityCheck))
        {
            Analyzer.Trace($"Region mode is {GlobalRegionMode}, exit", traceLevel);
            return false;
        }

        AccessLevel MemberAccessLevel = AccessLevelHelper.GetAccessLevel(memberDeclaration.Modifiers);
        if (MemberAccessLevel == AccessLevel.ProtectedInternal)
            MemberAccessLevel = AccessLevel.Protected;

        if (MemberAccessLevel != expectedAccessLevel)
        {
            Analyzer.Trace($"Member Access Level is {MemberAccessLevel}, exit", traceLevel);
            return false;
        }

        RegionExplorer Explorer = ContextExplorer.GetRegionExplorer(TypeDeclaration);

        if (!Explorer.RegionsByAccelLevel.ContainsKey(expectedAccessLevel))
        {
            Analyzer.Trace($"No other member with the same access level, exit", traceLevel);
            return false;
        }

        List<RegionDirectiveTriviaSyntax> RegionList = Explorer.RegionsByAccelLevel[expectedAccessLevel];
        if (RegionList.Count <= 1)
        {
            Analyzer.Trace($"Only one region with the members with same access level, exit", traceLevel);
            return false;
        }

        RegionDirectiveTriviaSyntax FirstRegion = RegionList[0];
        RegionDirectiveTriviaSyntax MemberRegion = Explorer.MemberRegionTable[memberDeclaration];

        if (MemberRegion == FirstRegion)
        {
            Analyzer.Trace($"Analyzing the first region, exit", traceLevel);
            return false;
        }

        expectedRegionText = GetRegionText(FirstRegion);
        memberText = "<Unknown>";

        if (Explorer.IsHomogeneousRegionPair(FirstRegion, MemberRegion) && isSimpleAccessibilityCheck)
        {
            Analyzer.Trace($"Handled in a separate diagnostic, exit", traceLevel);
            return false;
        }

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

    private bool IsHomogeneousRegionPair(RegionDirectiveTriviaSyntax region1, RegionDirectiveTriviaSyntax region2)
    {
        List<MemberDeclarationSyntax> MemberList1 = RegionMemberTable[region1];
        List<MemberDeclarationSyntax> MemberList2 = RegionMemberTable[region2];

        if (MemberList1.Count == 0 || MemberList2.Count == 0)
            return false;

        MemberDeclarationSyntax FirstMember1 = MemberList1[0];
        MemberTypes FirstMemberType1 = ClassExplorer.GetMemberType(FirstMember1);
        MemberDeclarationSyntax FirstMember2 = MemberList2[0];
        MemberTypes FirstMemberType2 = ClassExplorer.GetMemberType(FirstMember2);

        foreach (MemberDeclarationSyntax Member1 in MemberList1)
            if (ClassExplorer.GetMemberType(Member1) != FirstMemberType2)
                return false;

        foreach (MemberDeclarationSyntax Member2 in MemberList2)
            if (ClassExplorer.GetMemberType(Member2) != FirstMemberType1)
                return false;

        return true;
    }

    /// <summary>
    /// Calculates a global region mode.
    /// </summary>
    /// <param name="regionExplorerList">The source code.</param>
    /// <param name="traceLevel">The trace level.</param>
    public static RegionModes GetGlobalRegionMode(List<RegionExplorer> regionExplorerList, TraceLevel traceLevel)
    {
        if (regionExplorerList.Count < 3)
        {
            Analyzer.Trace("Not enough classes with content to set region mode", traceLevel);
            return RegionModes.Free;
        }

        Dictionary<RegionModes, int> ModeTable = new Dictionary<RegionModes, int>();
        foreach (RegionModes Mode in typeof(RegionModes).GetEnumValues())
            if (Mode != RegionModes.Undecided)
                ModeTable.Add(Mode, 0);

        foreach (RegionExplorer Item in regionExplorerList)
        {
            RegionModes Mode = Item.ThisRegionMode;
            if (Mode != RegionModes.Undecided)
                ModeTable[Mode]++;
        }

        int MainModeCount = 0;
        RegionModes MainMode = RegionModes.Undecided;

        foreach (RegionModes Mode in typeof(RegionModes).GetEnumValues())
            if (Mode != RegionModes.Undecided && MainModeCount < ModeTable[Mode])
            {
                MainModeCount = ModeTable[Mode];
                MainMode = Mode;
            }

        return MainMode;
    }
    #endregion

    #region Implementation
    private void Explore(TraceLevel traceLevel)
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

        bool HasAccessLevelDifference = false;
        bool HasTypeDifference = false;

        foreach (KeyValuePair<RegionDirectiveTriviaSyntax, List<MemberDeclarationSyntax>> Entry in RegionMemberTable)
        {
            List<MemberDeclarationSyntax> RegionMemberList = Entry.Value;

            if (RegionMemberList.Count > 0)
            {
                AccessLevel FirstMemberAccessLevel = AccessLevelHelper.GetAccessLevel(RegionMemberList[0].Modifiers);
                MemberTypes FirstMemberType = ClassExplorer.GetMemberType(RegionMemberList[0]);

                foreach (MemberDeclarationSyntax Member in RegionMemberList)
                {
                    HasAccessLevelDifference |= FirstMemberAccessLevel != AccessLevelHelper.GetAccessLevel(Member.Modifiers);
                    HasTypeDifference |= FirstMemberType != ClassExplorer.GetMemberType(Member);
                }
            }
        }

        if (RegionMemberTable.Count == 0)
            ThisRegionMode = RegionModes.Undecided;
        else if (HasAccessLevelDifference)
            ThisRegionMode = RegionModes.Free;
        else if (HasTypeDifference)
            ThisRegionMode = RegionModes.AccessibilitySimple;
        else
            ThisRegionMode = RegionModes.AccessibilityFull;

        Analyzer.Trace($"Class {ClassDeclaration.Identifier} region mode is {ThisRegionMode}", traceLevel);
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
    }
    #endregion
}
