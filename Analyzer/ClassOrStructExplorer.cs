namespace ConsistencyAnalyzer;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Represents an object that provides info about classes and structs.
/// </summary>
public class ClassOrStructExplorer
{
    #region Init
    /// <summary>
    /// Creates a ClassOrStructExplorer.
    /// </summary>
    /// <param name="compilationUnit">The source code.</param>
    /// <param name="context">A context for source code analysis.</param>
    /// <param name="traceLevel">The trace level.</param>
    public ClassOrStructExplorer(CompilationUnitSyntax compilationUnit, SyntaxNodeAnalysisContext? context, TraceLevel traceLevel)
    {
        CompilationUnit = compilationUnit;
        Context = context;

        List<TypeDeclarationSyntax> ClassOrStructDeclarationList = new();
        AddClassOrStructMembers(CompilationUnit.Members, ClassOrStructDeclarationList);

        foreach (TypeDeclarationSyntax ClassOrStructDeclaration in ClassOrStructDeclarationList)
            AddClassOrStruct(ClassOrStructDeclaration, traceLevel);

        CheckMemberTypeFullyOrdered();
    }

    private void AddClassOrStructMembers(SyntaxList<MemberDeclarationSyntax> members, List<TypeDeclarationSyntax> classOrStructDeclarationList)
    {
        foreach (MemberDeclarationSyntax Member in members)
        {
            switch (Member)
            {
                case NamespaceDeclarationSyntax AsNamespaceDeclaration:
                    AddClassOrStructMembers(AsNamespaceDeclaration.Members, classOrStructDeclarationList);
                    break;
                case ClassDeclarationSyntax AsClassDeclaration:
                    classOrStructDeclarationList.Add(AsClassDeclaration);
                    break;
                case StructDeclarationSyntax AsStructDeclaration:
                    classOrStructDeclarationList.Add(AsStructDeclaration);
                    break;
            }
        }
    }

    private SyntaxNodeAnalysisContext? Context;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the source code.
    /// </summary>
    public CompilationUnitSyntax CompilationUnit { get; init; }
    #endregion

    #region Client Interface
    private void AddClassOrStruct(TypeDeclarationSyntax classOrStructDeclaration, TraceLevel traceLevel)
    {
        List<MemberDeclarationSyntax> MemberList;

        MemberList = new List<MemberDeclarationSyntax>();
        ClassOrStructToMemberTable.Add(classOrStructDeclaration, MemberList);

        SyntaxToken CurrentToken = classOrStructDeclaration.OpenBraceToken;

        for (; ; )
        {
            CurrentToken = CurrentToken.GetNextToken(includeZeroWidth: false, includeSkipped: false, includeDirectives: true, includeDocumentationComments: false);

            if (CurrentToken == classOrStructDeclaration.CloseBraceToken)
                break;

            if (CurrentToken.Parent is MemberDeclarationSyntax AsMemberDeclaration)
            {
                if (!MemberToClassOrStructTable.ContainsKey(AsMemberDeclaration))
                {
                    MemberToClassOrStructTable.Add(AsMemberDeclaration, classOrStructDeclaration);
                    MemberList.Add(AsMemberDeclaration);
                }
            }
        }

        Analyzer.Trace($"Class {classOrStructDeclaration.Identifier} has {MemberList.Count} members", traceLevel);
    }

    /// <summary>
    /// Gets the list of classes and structs.
    /// </summary>
    public List<TypeDeclarationSyntax> GetClassOrStructList()
    {
        return new List<TypeDeclarationSyntax>(ClassOrStructToMemberTable.Keys);
    }

    /// <summary>
    /// Gets members of a class or struct.
    /// </summary>
    /// <param name="classOrStructDeclaration">The class or struct with members.</param>
    public List<MemberDeclarationSyntax> GetMemberList(TypeDeclarationSyntax classOrStructDeclaration)
    {
        return ClassOrStructToMemberTable[classOrStructDeclaration];
    }

    /// <summary>
    /// Gets the class or struct owning a member.
    /// </summary>
    /// <param name="memberDeclaration">The member.</param>
    public TypeDeclarationSyntax GetMemberOwner(MemberDeclarationSyntax memberDeclaration)
    {
        return MemberToClassOrStructTable[memberDeclaration];
    }

    /// <summary>
    /// Gets the type of a member.
    /// </summary>
    /// <param name="memberDeclaration">The member type.</param>
    public static MemberTypes GetMemberType(MemberDeclarationSyntax memberDeclaration)
    {

        switch (memberDeclaration)
        {
            case ConstructorDeclarationSyntax:
                return MemberTypes.Contructor;
            case FieldDeclarationSyntax:
                return MemberTypes.Field;
            case PropertyDeclarationSyntax:
                return MemberTypes.Property;

            default:
            case MethodDeclarationSyntax:
                return MemberTypes.Method;
        }
    }

    /// <summary>
    /// Checks if all class or struct members are almost always after and before other types of members.
    /// </summary>
    public bool IsAllClassMemberFullyOrdered()
    {
        return FullyOrderedMemberTypeList.Count > 0;
    }

    /// <summary>
    /// Checks if all class or struct members are almost always after and before other types of members.
    /// </summary>
    private void CheckMemberTypeFullyOrdered()
    {
        int UnorderedClassCount = 0;

        foreach (KeyValuePair<TypeDeclarationSyntax, List<MemberDeclarationSyntax>> Entry in ClassOrStructToMemberTable)
        {
            List<MemberDeclarationSyntax> MemberList = Entry.Value;

            if (IsMemberTypeFullyOrdered(MemberList, out List<MemberTypes> MemberTypeList))
            {
                int Key = GetMemberTypeOrderKey(MemberTypeList);

                if (!MemberTypeFullyOrderedCountTable.ContainsKey(Key))
                {
                    MemberTypeFullyOrderedCountTable.Add(Key, 0);
                    MemberTypeFullyOrderedListTable.Add(Key, MemberTypeList);
                }

                MemberTypeFullyOrderedCountTable[Key]++;
            }
            else
                UnorderedClassCount++;
        }

        int BestKey = 0;
        int FullyOrderedClassCount = 0;
        foreach (KeyValuePair<int, int> Entry in MemberTypeFullyOrderedCountTable)
            if (FullyOrderedClassCount < Entry.Value)
            {
                FullyOrderedClassCount = Entry.Value;
                BestKey = Entry.Key;
            }

        foreach (KeyValuePair<int, int> Entry in MemberTypeFullyOrderedCountTable)
            if (Entry.Key != BestKey)
                UnorderedClassCount++;

        if (FullyOrderedClassCount + UnorderedClassCount >= 3 && FullyOrderedClassCount > UnorderedClassCount)
            FullyOrderedMemberTypeList = MemberTypeFullyOrderedListTable[BestKey];
    }

    private static bool IsMemberTypeFullyOrdered(List<MemberDeclarationSyntax> MemberList, out List<MemberTypes> memberTypeList)
    {
        memberTypeList = new();

        Dictionary<MemberTypes, int> FirstMemberIndexTable = new();
        Dictionary<MemberTypes, int> LastMemberIndexTable = new();

        for (int Index = 0; Index < MemberList.Count; Index++)
        {
            MemberDeclarationSyntax MemberDeclaration = MemberList[Index];
            MemberTypes MemberType = GetMemberType(MemberDeclaration);

            if (!FirstMemberIndexTable.ContainsKey(MemberType))
                FirstMemberIndexTable.Add(MemberType, Index);

            if (!LastMemberIndexTable.ContainsKey(MemberType))
                LastMemberIndexTable.Add(MemberType, Index);
            else
                LastMemberIndexTable[MemberType] = Index;
        }

        foreach (KeyValuePair<MemberTypes, int> EntryFirst in FirstMemberIndexTable)
            foreach (KeyValuePair<MemberTypes, int> EntryLast in LastMemberIndexTable)
                if (EntryFirst.Key != EntryLast.Key && EntryFirst.Value < EntryLast.Value && LastMemberIndexTable[EntryFirst.Key] > FirstMemberIndexTable[EntryLast.Key])
                    return false;

        foreach (MemberDeclarationSyntax Item in MemberList)
        {
            MemberTypes ItemType = GetMemberType(Item);
            if (!memberTypeList.Contains(ItemType))
                memberTypeList.Add(ItemType);
        }

        return true;
    }

    private static int GetMemberTypeOrderKey(List<MemberTypes> memberTypeList)
    {
        int ValueCount = typeof(MemberTypes).GetEnumValues().Length;
        int BitSize = LowestPowerOfTwoGreaterThan(ValueCount);

        int Key = 0;
        for (int i = 0; i < memberTypeList.Count; i++)
        {
            int Position = ((int)memberTypeList[i]) * BitSize;
            Key |= i << Position;
        }

        return Key;
    }

    private static int LowestPowerOfTwoGreaterThan(int value)
    {
        int PowerOfTwo = 1;
        while ((1 << PowerOfTwo) <= value)
            PowerOfTwo++;

        return PowerOfTwo;
    }

    /// <summary>
    /// Checks if a class or struct member is after and before other types of members.
    /// </summary>
    /// <param name="classOrStructDeclaration">The class or struct hosting <paramref name="memberDeclaration"/>.</param>
    /// <param name="memberDeclaration">The member node.</param>
    public bool IsClassMemberOutOfOrder(TypeDeclarationSyntax classOrStructDeclaration, MemberDeclarationSyntax memberDeclaration)
    {
        MemberTypes MemberType = GetMemberType(memberDeclaration);

        List<MemberTypes> ExcludedMemberTypeBefore = new List<MemberTypes>();
        foreach (MemberTypes Item in FullyOrderedMemberTypeList)
            if (Item == MemberType)
                break;
            else
                ExcludedMemberTypeBefore.Add(Item);

        List<MemberDeclarationSyntax> MemberList = ClassOrStructToMemberTable[classOrStructDeclaration];
        int MemberIndex = MemberList.IndexOf(memberDeclaration);

        for (int Index = MemberIndex; Index < MemberList.Count; Index++)
        {
            MemberDeclarationSyntax OtherMember = MemberList[Index];
            MemberTypes OtherMemberType = GetMemberType(OtherMember);

            if (ExcludedMemberTypeBefore.Contains(OtherMemberType))
                return true;
        }

        return false;
    }

    private Dictionary<TypeDeclarationSyntax, List<MemberDeclarationSyntax>> ClassOrStructToMemberTable { get; } = new();
    private Dictionary<MemberDeclarationSyntax, TypeDeclarationSyntax> MemberToClassOrStructTable { get; } = new();
    private Dictionary<int, int> MemberTypeFullyOrderedCountTable { get; } = new();
    private Dictionary<int, List<MemberTypes>> MemberTypeFullyOrderedListTable { get; } = new();
    private List<MemberTypes> FullyOrderedMemberTypeList = new ();
    #endregion
}
