namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an object that provides info about namees.
    /// </summary>
    public partial class NameExplorer
    {
        #region Init
        /// <summary>
        /// Creates a NameExplorer.
        /// </summary>
        /// <param name="compilationUnit">The source code.</param>
        /// <param name="traceLevel">The trace level.</param>
        public NameExplorer(CompilationUnitSyntax compilationUnit, TraceLevel traceLevel)
        {
            CompilationUnit = compilationUnit;

            ParseCompilationUnit(CompilationUnit);
            ReduceSchemes();
        }

        private void ReduceSchemes()
        {
            foreach (NameCategory NameCategory in typeof(NameCategory).GetEnumValues())
            {
                if (NameCategory != NameCategory.Neutral && SchemeTable.ContainsKey(NameCategory))
                {
                    Dictionary<NamingSchemes, int> Table = SchemeTable[NameCategory];

                    SortSchemes(Table, out List<NamingSchemes> SortedSchemeList, out List<int> CountList);
                    if (SortedSchemeList.Count > 0)
                    {
                        int LastCompatibleIndex = 1;
                        for (; LastCompatibleIndex < SortedSchemeList.Count; LastCompatibleIndex++)
                            if (CountList[LastCompatibleIndex] < CountList[0] && !IsSchemeCompatible(SortedSchemeList[LastCompatibleIndex], SortedSchemeList[0]))
                                break;

                        NamingSchemes CompatibleSchemes = NamingSchemes.None;
                        for (int i = 0; i < LastCompatibleIndex; i++)
                            CompatibleSchemes |= SortedSchemeList[i];

                        ExpectedSchemeTable.Add(NameCategory, CompatibleSchemes);
                    }
                }

                if (!ExpectedSchemeTable.ContainsKey(NameCategory))
                    ExpectedSchemeTable.Add(NameCategory, NamingSchemes.None);
            }
        }

        private void SortSchemes(Dictionary<NamingSchemes, int> table, out List<NamingSchemes> sortedSchemeList, out List<int> countList)
        {
            sortedSchemeList = new List<NamingSchemes>();
            countList = new List<int>();

            foreach (KeyValuePair<NamingSchemes, int> Entry in table)
            {
                int Count = Entry.Value;
                int Index;

                for (Index = 0; Index < sortedSchemeList.Count; Index++)
                    if (table[sortedSchemeList[Index]] < Count)
                        break;

                sortedSchemeList.Insert(Index, Entry.Key);
                countList.Insert(Index, Count);
            }
        }
        #endregion

        #region Name Parsing
        private void ParseIdentifier(SyntaxToken identifier, NameCategory nameCategory)
        {
            if (!SchemeTable.ContainsKey(nameCategory))
                SchemeTable.Add(nameCategory, new Dictionary<NamingSchemes, int>());

            string Name = identifier.ValueText;
            Dictionary<NamingSchemes, int> Table = SchemeTable[nameCategory];

            foreach (NamingSchemes EnumValue in typeof(NamingSchemes).GetEnumValues())
                if (EnumValue != NamingSchemes.None && EnumValue != NamingSchemes.All)
                    if (IsNameMatchingScheme(Name, EnumValue, UnderscoreUse.Always))
                    {
                        if (!Table.ContainsKey(EnumValue))
                            Table.Add(EnumValue, 0);

                        Table[EnumValue]++;
                    }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source code.
        /// </summary>
        public CompilationUnitSyntax CompilationUnit { get; init; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the scheme corresponding to a name category.
        /// </summary>
        /// <param name="nameCategory">The name category.</param>
        public NamingSchemes GetScheme(NameCategory nameCategory)
        {
            return ExpectedSchemeTable[nameCategory];
        }

        /// <summary>
        /// Checks if a name is matching a given naming scheme.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        internal static bool IsNameMatchingCompositeScheme(string name, NamingSchemes scheme)
        {
            if (name.Length > 0)
            {
                foreach (NamingSchemes EnumValue in typeof(NamingSchemes).GetEnumValues())
                    if (scheme.HasFlag(EnumValue) && IsNameMatchingScheme(name, EnumValue, UnderscoreUse.Optional))
                        return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a name is matching a given naming scheme.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <param name="scheme">The scheme to use.</param>
        /// <param name="underscoreUse">The specification about underscore.</param>
        internal static bool IsNameMatchingScheme(string name, NamingSchemes scheme, UnderscoreUse underscoreUse)
        {
            if (name.Length == 0)
                return false;

            switch (scheme)
            {
                default:
                case NamingSchemes.None:
                    return false;

                case NamingSchemes.All:
                    return true;

                case NamingSchemes.twowords:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.OnlyLower);

                case NamingSchemes.TWOWORDS:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.OnlyUpper);

                case NamingSchemes.twoWords:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.FirstLower);

                case NamingSchemes.TwoWords:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.FirstUpper);

                case NamingSchemes.two_words:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.OnlyLower);

                case NamingSchemes.TWO_WORDS:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.OnlyUpper);

                case NamingSchemes.two_Words:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.FirstLower);

                case NamingSchemes.Two_Words:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.FirstUpper);
            }
        }

        private static bool IsNameMatchingScheme(string name, UnderscoreUse underscoreUse, CharacterCasing characterCasing)
        {
            switch (underscoreUse)
            {
                case UnderscoreUse.Never:
                    if (name.Contains("_"))
                        return false;
                    break;
                case UnderscoreUse.Always:
                    if (!name.Contains("_"))
                        return false;
                    break;
            }

            switch (characterCasing)
            {
                case CharacterCasing.OnlyLower:
                    foreach (char c in name)
                        if (char.IsUpper(c))
                            return false;
                    break;
                case CharacterCasing.OnlyUpper:
                    foreach (char c in name)
                        if (char.IsLower(c))
                            return false;
                    break;
                case CharacterCasing.FirstLower:
                    if (!char.IsLower(name[0]))
                        return false;
                    break;
                case CharacterCasing.FirstUpper:
                    if (!char.IsUpper(name[0]))
                        return false;
                    break;
            }

            if ((characterCasing == CharacterCasing.FirstLower || characterCasing == CharacterCasing.FirstUpper) && underscoreUse != UnderscoreUse.Never)
            {
                bool isPreviousUnderscore = false;

                for (int i = 0; i < name.Length; i++)
                {
                    char c = name[i];

                    if (c == '_')
                        isPreviousUnderscore = true;
                    else if (isPreviousUnderscore)
                    {
                        if (!char.IsUpper(c))
                            return false;
                        isPreviousUnderscore = false;
                    }
                    else if (i > 0 && char.IsUpper(c))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if a name is matching the naming scheme of its category.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <param name="nameCategory">The name category.</param>
        /// <param name="expectedSheme">The expected naming scheme upon return.</param>
        /// <param name="traceLevel">The trace level.</param>
        internal bool IsNameMismatch(string name, NameCategory nameCategory, out NamingSchemes expectedSheme, TraceLevel traceLevel)
        {
            expectedSheme = GetScheme(nameCategory);

            if (expectedSheme == NamingSchemes.None)
            {
                Analyzer.Trace($"No scheme for {nameCategory}, exit", traceLevel);
                return false;
            }

            if (IsNameMatchingCompositeScheme(name, expectedSheme))
            {
                Analyzer.Trace($"Name {name} is matching scheme for {nameCategory}, exit", traceLevel);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the name from a syntax none.
        /// </summary>
        /// <param name="nameSyntax">The syntax node.</param>
        /// <returns></returns>
        public static string GetNameText(NameSyntax nameSyntax)
        {
            switch (nameSyntax)
            {
                case AliasQualifiedNameSyntax AsAliasQualifiedName:
                    return GetNameText(AsAliasQualifiedName.Name);
                case QualifiedNameSyntax AsQualifiedName:
                    return GetNameText(AsQualifiedName.Left) + "." + GetNameText(AsQualifiedName.Right);
                case GenericNameSyntax AsGenericName:
                    return AsGenericName.Identifier.ValueText;
                case IdentifierNameSyntax AsIdentifierName:
                    return AsIdentifierName.Identifier.ValueText;
                default:
                    return null!;
            }
        }

        /// <summary>
        /// Checks if <paramref name="scheme"/> is compatible with <paramref name="schemeReference"/>.
        /// </summary>
        /// <param name="scheme">The scheme to check.</param>
        /// <param name="schemeReference">The reference scheme.</param>
        public static bool IsSchemeCompatible(NamingSchemes scheme, NamingSchemes schemeReference)
        {
            switch (schemeReference)
            {
                default:
                case NamingSchemes.None:
                    return false;
                case NamingSchemes.All:
                    return true;

                case NamingSchemes.twowords:
                    switch (scheme)
                    {
                        case NamingSchemes.twowords:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.TWOWORDS:
                    switch (scheme)
                    {
                        case NamingSchemes.TWOWORDS:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.twoWords:
                    switch (scheme)
                    {
                        case NamingSchemes.twowords:
                        case NamingSchemes.twoWords:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.TwoWords:
                    switch (scheme)
                    {
                        case NamingSchemes.TwoWords:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.two_words:
                    switch (scheme)
                    {
                        case NamingSchemes.twowords:
                        case NamingSchemes.two_words:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.TWO_WORDS:
                    switch (scheme)
                    {
                        case NamingSchemes.TWOWORDS:
                        case NamingSchemes.TWO_WORDS:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.two_Words:
                    switch (scheme)
                    {
                        case NamingSchemes.twowords:
                        case NamingSchemes.two_Words:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.Two_Words:
                    switch (scheme)
                    {
                        case NamingSchemes.Two_Words:
                            return true;
                        default:
                            return false;
                    }
            }
        }

        /// <summary>
        /// Returns <paramref name="name"/> modified to match other names of <paramref name="nameCategory"/>.
        /// </summary>
        /// <param name="name">The name to modify.</param>
        /// <param name="nameCategory">The name category.</param>
        public string FixName(string name, NameCategory nameCategory)
        {
            NamingSchemes ExpectedSheme = GetScheme(nameCategory);

            if (ExpectedSheme == NamingSchemes.None)
                return name;

            string Result;

            if (name.Contains("_"))
                Result = FixNameWithUnderscore(name, ExpectedSheme);
            else
                Result = FixNameWithoutUnderscore(name, ExpectedSheme);

            return Result;
        }

        private string FixNameWithUnderscore(string name, NamingSchemes expectedSheme)
        {
            string Result;

            if (!expectedSheme.HasFlag(NamingSchemes.two_words) && !expectedSheme.HasFlag(NamingSchemes.two_Words) && !expectedSheme.HasFlag(NamingSchemes.TWO_WORDS) && !expectedSheme.HasFlag(NamingSchemes.Two_Words))
                Result = FixNameRemoveUnderscore(name, expectedSheme);
            else if (expectedSheme.HasFlag(NamingSchemes.two_words))
                Result = name.ToLower();
            else if (expectedSheme.HasFlag(NamingSchemes.TWO_WORDS))
                Result = name.ToUpper();
            else if (expectedSheme.HasFlag(NamingSchemes.two_Words))
                Result = FixNameRemoveUnderscoreFirstLower(name, expectedSheme);
            else if (expectedSheme.HasFlag(NamingSchemes.Two_Words))
                Result = FixNameRemoveUnderscoreFirstUpper(name, expectedSheme);
            else
                Result = name;

            return Result;
        }

        private string FixNameRemoveUnderscore(string name, NamingSchemes expectedSheme)
        {
            string Result;

            if (expectedSheme.HasFlag(NamingSchemes.twowords))
                Result = name.Replace("_", "").ToLower();
            else if (expectedSheme.HasFlag(NamingSchemes.TWOWORDS))
                Result = name.Replace("_", "").ToUpper();
            else
            {
                bool SetUpper = false;
                Result = string.Empty;

                for (int i = 0; i < name.Length; i++)
                {
                    char c = name[i];

                    if (c == '_')
                        SetUpper = true;
                    else if (i == 0)
                    {
                        if (expectedSheme.HasFlag(NamingSchemes.twoWords))
                            Result += char.ToLower(c);
                        else if (expectedSheme.HasFlag(NamingSchemes.TwoWords))
                            Result += char.ToUpper(c);
                        else
                            Result += c;
                    }
                    else if (SetUpper)
                    {
                        Result += char.ToUpper(c);
                        SetUpper = false;
                    }
                    else
                        Result += char.ToLower(c);
                }
            }

            return Result;
        }

        private string FixNameRemoveUnderscoreFirstLower(string name, NamingSchemes expectedSheme)
        {
            string Result = string.Empty;
            bool SetUpper = false;

            int i;

            for (i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (c == '_')
                {
                    SetUpper = true;
                    break;
                }
                else if (!char.IsUpper(c))
                    break;

                Result += char.ToLower(c);
            }

            for (; i < name.Length; i++)
            {
                char c = name[i];

                if (c == '_')
                {
                    Result += c;
                    SetUpper = true;
                }
                else if (SetUpper)
                {
                    Result += char.ToUpper(c);
                    SetUpper = false;
                }
                else
                    Result += char.ToLower(c);
            }

            return Result;
        }

        private string FixNameRemoveUnderscoreFirstUpper(string name, NamingSchemes expectedSheme)
        {
            string Result = string.Empty;
            bool SetUpper = false;

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (c == '_')
                {
                    Result += c;
                    SetUpper = true;
                }
                else if (i == 0)
                    Result += char.ToUpper(c);
                else if (SetUpper)
                {
                    Result += char.ToUpper(c);
                    SetUpper = false;
                }
                else
                    Result += char.ToLower(c);
            }

            return Result;
        }

        private string FixNameWithoutUnderscore(string name, NamingSchemes expectedSheme)
        {
            string Result;

            if (expectedSheme.HasFlag(NamingSchemes.twowords))
                Result = name.ToLower();
            else if (expectedSheme.HasFlag(NamingSchemes.TWOWORDS) || expectedSheme.HasFlag(NamingSchemes.TWO_WORDS))
                Result = name.ToUpper();
            else if (expectedSheme.HasFlag(NamingSchemes.twoWords))
                Result = char.ToLower(name[0]) + name.Substring(1);
            else if (expectedSheme.HasFlag(NamingSchemes.TwoWords))
                Result = char.ToUpper(name[0]) + name.Substring(1);
            else
                Result = FixNameInsertUnderscore(name, expectedSheme);

            return Result;
        }

        private string FixNameInsertUnderscore(string name, NamingSchemes expectedSheme)
        {
            string Result;

            if (expectedSheme.HasFlag(NamingSchemes.two_words))
            {
                Result = string.Empty;
                bool IsPreviousLower = false;

                for (int i = 0; i < name.Length; i++)
                {
                    char c = name[i];

                    if (IsPreviousLower && char.IsUpper(c))
                        Result += "_";

                    Result += char.ToLower(c);
                    IsPreviousLower = char.IsLower(c);
                }
            }
            else if (expectedSheme.HasFlag(NamingSchemes.two_Words))
                Result = FixNameInsertUnderscoreFirstLower(name, expectedSheme);
            else if (expectedSheme.HasFlag(NamingSchemes.Two_Words))
                Result = FixNameInsertUnderscoreFirstUpper(name, expectedSheme);
            else
                Result = name;

            return Result;
        }

        private string FixNameInsertUnderscoreFirstLower(string name, NamingSchemes expectedSheme)
        {
            string Result = string.Empty;
            int i;

            for (i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (!char.IsUpper(c))
                    break;

                Result += char.ToLower(c);
            }

            bool IsUnderscoreInserted = false;

            for (; i < name.Length; i++)
            {
                char c = name[i];

                if (char.IsUpper(c))
                {
                    if (!IsUnderscoreInserted)
                    {
                        IsUnderscoreInserted = true;
                        Result += "_";
                        Result += c;
                    }
                    else
                        Result += char.ToLower(c);
                }
                else
                {
                    IsUnderscoreInserted = false;
                    Result += char.ToLower(c);
                }
            }

            return Result;
        }

        private string FixNameInsertUnderscoreFirstUpper(string name, NamingSchemes expectedSheme)
        {
            string Result = string.Empty;
            bool IsUnderscoreInserted = true;

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (i == 0)
                    Result += char.ToUpper(c);
                else if (char.IsUpper(c))
                {
                    if (!IsUnderscoreInserted)
                    {
                        IsUnderscoreInserted = true;
                        Result += "_";
                        Result += c;
                    }
                    else
                        Result += char.ToLower(c);
                }
                else
                {
                    IsUnderscoreInserted = false;
                    Result += char.ToLower(c);
                }
            }

            return Result;
        }

        private Dictionary<NameCategory, Dictionary<NamingSchemes, int>> SchemeTable = new();
        private Dictionary<NameCategory, NamingSchemes> ExpectedSchemeTable = new();
        #endregion
    }
}
