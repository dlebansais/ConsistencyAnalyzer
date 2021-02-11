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
                    if (IsNameMatchingSchemeStrict(Name, EnumValue))
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
        public static bool IsNameMatchingCompositeScheme(string name, NamingSchemes scheme)
        {
            if (name.Length > 0)
            {
                foreach (NamingSchemes EnumValue in typeof(NamingSchemes).GetEnumValues())
                    if (scheme.HasFlag(EnumValue) && IsNameMatchingScheme(name, EnumValue))
                        return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a name is matching a given naming scheme.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static bool IsNameMatchingScheme(string name, NamingSchemes scheme)
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
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: true, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.TWOWORDS:
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: false, onlyUpper: true, startLower: false, startUpper: true);

                case NamingSchemes.twoWords:
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: false, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.TwoWords:
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: false, onlyUpper: false, startLower: false, startUpper: true);

                case NamingSchemes.two_words:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: false, afterUnderscoreLower: true, afterUnderscoreUpper: false, onlyLower: true, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.TWO_WORDS:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: true, onlyLower: false, onlyUpper: true, startLower: false, startUpper: true);

                case NamingSchemes.two_Words:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: true, onlyLower: false, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.Two_Words:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: true, onlyLower: false, onlyUpper: false, startLower: true, startUpper: true);
            }
        }

        /// <summary>
        /// Checks if a name is matching a given naming scheme, specifically.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static bool IsNameMatchingSchemeStrict(string name, NamingSchemes scheme)
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
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: true, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.TWOWORDS:
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: false, onlyUpper: true, startLower: false, startUpper: true);

                case NamingSchemes.twoWords:
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: false, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.TwoWords:
                    return IsNameMatchingScheme(name, noUnderscore: true, mandatoryUnderscore: false, afterUnderscoreLower: false, afterUnderscoreUpper: false, onlyLower: false, onlyUpper: false, startLower: false, startUpper: true);

                case NamingSchemes.two_words:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: true, afterUnderscoreLower: true, afterUnderscoreUpper: false, onlyLower: true, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.TWO_WORDS:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: true, afterUnderscoreLower: false, afterUnderscoreUpper: true, onlyLower: false, onlyUpper: true, startLower: false, startUpper: true);

                case NamingSchemes.two_Words:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: true, afterUnderscoreLower: false, afterUnderscoreUpper: true, onlyLower: false, onlyUpper: false, startLower: true, startUpper: false);

                case NamingSchemes.Two_Words:
                    return IsNameMatchingScheme(name, noUnderscore: false, mandatoryUnderscore: true, afterUnderscoreLower: false, afterUnderscoreUpper: true, onlyLower: false, onlyUpper: false, startLower: true, startUpper: true);
            }
        }

        private static bool IsNameMatchingScheme(string name, bool noUnderscore, bool mandatoryUnderscore, bool afterUnderscoreLower, bool afterUnderscoreUpper, bool onlyLower, bool onlyUpper, bool startLower, bool startUpper)
        {
            if (noUnderscore)
            {
                if (name.Contains("_"))
                    return false;
            }
            else
            {
                if (mandatoryUnderscore)
                {
                    if (!name.Contains("_"))
                        return false;
                }

                if (afterUnderscoreLower)
                {
                    for (int i = 0; i + 1 < name.Length; i++)
                        if (name[i] == '_' && !char.IsLower(name[i + 1]))
                            return false;
                }

                if (afterUnderscoreUpper)
                {
                    for (int i = 0; i + 1 < name.Length; i++)
                        if (name[i] == '_' && !char.IsUpper(name[i + 1]))
                            return false;
                }
            }

            if (onlyLower)
            {
                foreach (char c in name)
                    if (char.IsUpper(c))
                        return false;
            }

            if (onlyUpper)
            {
                foreach (char c in name)
                    if (char.IsLower(c))
                        return false;
            }

            if (startLower && !char.IsLower(name[0]))
                return false;

            if (startUpper && !char.IsUpper(name[0]))
                return false;

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
        public static string GetName(NameSyntax nameSyntax)
        {
            switch (nameSyntax)
            {
                case AliasQualifiedNameSyntax AsAliasQualifiedName:
                    return GetName(AsAliasQualifiedName.Name);
                case QualifiedNameSyntax AsQualifiedName:
                    return GetName(AsQualifiedName.Right);
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

            if (name.Contains("_") && !ExpectedSheme.HasFlag(NamingSchemes.two_words) && !ExpectedSheme.HasFlag(NamingSchemes.two_Words) && !ExpectedSheme.HasFlag(NamingSchemes.TWO_WORDS) && !ExpectedSheme.HasFlag(NamingSchemes.Two_Words))
            {
                if (ExpectedSheme.HasFlag(NamingSchemes.twowords))
                    Result = name.Replace("_", "").ToLower();
                else if (ExpectedSheme.HasFlag(NamingSchemes.TWOWORDS))
                    Result = name.Replace("_", "").ToUpper();
                else
                {
                    bool SetUpper = true;
                    Result = string.Empty;

                    for (int i = 0; i < name.Length; i++)
                    {
                        char c = name[i];

                        if (c == '_')
                            SetUpper = true;
                        else if (i == 0)
                        {
                            if (ExpectedSheme.HasFlag(NamingSchemes.twoWords))
                                Result += char.ToLower(c);
                            else if (ExpectedSheme.HasFlag(NamingSchemes.TwoWords))
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
                            Result += c;
                    }
                }
            }
            else
                Result = name;

            return Result;
        }

        private Dictionary<NameCategory, Dictionary<NamingSchemes, int>> SchemeTable = new();
        private Dictionary<NameCategory, NamingSchemes> ExpectedSchemeTable = new();
        #endregion
    }
}
