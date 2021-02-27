namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
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
        /// <param name="context">A context for source code analysis.</param>
        /// <param name="traceLevel">The trace level.</param>
        public NameExplorer(CompilationUnitSyntax compilationUnit, SyntaxNodeAnalysisContext? context, TraceLevel traceLevel)
        {
            CompilationUnit = compilationUnit;
            Context = context;

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

        private SyntaxNodeAnalysisContext? Context;
        #endregion

        #region Name Parsing
        private void ParseIdentifier(SyntaxToken identifier, NameCategory nameCategory)
        {
            if (!SchemeTable.ContainsKey(nameCategory))
                SchemeTable.Add(nameCategory, new Dictionary<NamingSchemes, int>());

            string Name = identifier.ValueText;
            Dictionary<NamingSchemes, int> Table = SchemeTable[nameCategory];
            bool IsStartingAsInterface = Name.Length > 0 && Name[0] == 'I';

            foreach (NamingSchemes EnumValue in typeof(NamingSchemes).GetEnumValues())
                if (EnumValue != NamingSchemes.None && EnumValue != NamingSchemes.All && EnumValue != NamingSchemes.Interface)
                {
                    NamingSchemes Scheme = EnumValue;

                    if (nameCategory == NameCategory.Interface)
                    {
                        if (IsNameMatchingScheme(Name, Scheme | NamingSchemes.Interface, UnderscoreUse.Always))
                            IncrementMatchingSchemeCount(Table, Scheme | NamingSchemes.Interface);
                        else if (!IsStartingAsInterface && IsNameMatchingScheme(Name, Scheme, UnderscoreUse.Always))
                            IncrementMatchingSchemeCount(Table, Scheme);
                    }
                    else if (IsNameMatchingScheme(Name, Scheme, UnderscoreUse.Always))
                        IncrementMatchingSchemeCount(Table, Scheme);
                }
        }

        private void IncrementMatchingSchemeCount(Dictionary<NamingSchemes, int> table, NamingSchemes scheme)
        {
            if (!table.ContainsKey(scheme))
                table.Add(scheme, 0);

            table[scheme]++;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source code.
        /// </summary>
        public CompilationUnitSyntax CompilationUnit { get; init; }

        /// <summary>
        /// Gets a value indicating whether constness of local variable is expected.
        /// </summary>
        public bool IsLocalVariableConstnessExpected
        {
            get { return LocalVariableConstCount + LocalVariableConstCandidateCount >= 3 && LocalVariableConstCount > LocalVariableConstCandidateCount; } 
        }

        /// <summary>
        /// Gets a value indicating whether how indentation is defined is known.
        /// </summary>
        public bool IsIndentationKnown { get { return IsIndentationUsingTab || WhitespaceIndentation > 0; } }

        /// <summary>
        /// Gets a value indicating whether indentation uses the tab character.
        /// </summary>
        public bool IsIndentationUsingTab { get; private set; }

        /// <summary>
        /// Gets a value indicating whether indentation uses the whitespace and how many are used for one level of indentation.
        /// </summary>
        public int WhitespaceIndentation { get; private set; }
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
        /// <param name="name">The name to check.</param>
        /// <param name="scheme">The scheme.</param>
        /// <param name="isInterface">True if the test is for an interface name.</param>
        internal static bool IsNameMatchingCompositeScheme(string name, NamingSchemes scheme, bool isInterface)
        {
            if (name.Length > 0)
            {
                NamingSchemes Additional = scheme & NamingSchemes.Interface;

                foreach (NamingSchemes EnumValue in typeof(NamingSchemes).GetEnumValues())
                    if (EnumValue != NamingSchemes.None && EnumValue != NamingSchemes.All && EnumValue != NamingSchemes.Interface)
                        if (scheme.HasFlag(EnumValue) && IsNameMatchingScheme(name, EnumValue | Additional, UnderscoreUse.Optional))
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

            bool IsInterfaceScheme = scheme.HasFlag(NamingSchemes.Interface);
            NamingSchemes RootScheme = scheme & ~NamingSchemes.Interface;

            switch (RootScheme)
            {
                default:
                case NamingSchemes.None:
                    return false;

                case NamingSchemes.All:
                    return true;

                case NamingSchemes.twowords:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.OnlyLower, IsInterfaceScheme);

                case NamingSchemes.TWOWORDS:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.OnlyUpper, IsInterfaceScheme);

                case NamingSchemes.twoWords:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.FirstLower, IsInterfaceScheme);

                case NamingSchemes.TwoWords:
                    return IsNameMatchingScheme(name, UnderscoreUse.Never, CharacterCasing.FirstUpper, IsInterfaceScheme);

                case NamingSchemes.two_words:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.OnlyLower, IsInterfaceScheme);

                case NamingSchemes.TWO_WORDS:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.OnlyUpper, IsInterfaceScheme);

                case NamingSchemes.two_Words:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.FirstLower, IsInterfaceScheme);

                case NamingSchemes.Two_Words:
                    return IsNameMatchingScheme(name, underscoreUse, CharacterCasing.FirstUpper, IsInterfaceScheme);
            }
        }

        private static bool IsNameMatchingScheme(string name, UnderscoreUse underscoreUse, CharacterCasing characterCasing, bool isStartingWithI)
        {
            if (isStartingWithI && name[0] != 'I')
                return false;

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

            int FirstLetterIndex = isStartingWithI && name.Length > 1 ? 1 : 0;

            switch (characterCasing)
            {
                case CharacterCasing.OnlyLower:
                    for (int i = FirstLetterIndex; i < name.Length; i++)
                        if (char.IsUpper(name[i]))
                            return false;
                    break;
                case CharacterCasing.OnlyUpper:
                    for (int i = FirstLetterIndex; i < name.Length; i++)
                        if (char.IsLower(name[i]))
                            return false;
                    break;
                case CharacterCasing.FirstLower:
                    if (!char.IsLower(name[FirstLetterIndex]))
                        return false;
                    break;
                case CharacterCasing.FirstUpper:
                    if (!char.IsUpper(name[FirstLetterIndex]))
                        return false;
                    break;
            }

            if ((characterCasing == CharacterCasing.FirstLower || characterCasing == CharacterCasing.FirstUpper) && underscoreUse != UnderscoreUse.Never)
            {
                bool isPreviousUnderscore = false;

                for (int i = FirstLetterIndex; i < name.Length; i++)
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
                    else if (i > FirstLetterIndex && char.IsUpper(c))
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

            if (IsNameMatchingCompositeScheme(name, expectedSheme, nameCategory == NameCategory.Interface))
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
            if (scheme.HasFlag(NamingSchemes.Interface) != schemeReference.HasFlag(NamingSchemes.Interface))
                return false;

            NamingSchemes RootScheme = scheme & ~NamingSchemes.Interface;
            NamingSchemes RootSchemeReference = schemeReference & ~NamingSchemes.Interface;

            switch (RootSchemeReference)
            {
                default:
                case NamingSchemes.None:
                    return false;
                case NamingSchemes.All:
                    return true;

                case NamingSchemes.twowords:
                    switch (RootScheme)
                    {
                        case NamingSchemes.twowords:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.TWOWORDS:
                    switch (RootScheme)
                    {
                        case NamingSchemes.TWOWORDS:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.twoWords:
                    switch (RootScheme)
                    {
                        case NamingSchemes.twowords:
                        case NamingSchemes.twoWords:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.TwoWords:
                    switch (RootScheme)
                    {
                        case NamingSchemes.TwoWords:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.two_words:
                    switch (RootScheme)
                    {
                        case NamingSchemes.twowords:
                        case NamingSchemes.two_words:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.TWO_WORDS:
                    switch (RootScheme)
                    {
                        case NamingSchemes.TWOWORDS:
                        case NamingSchemes.TWO_WORDS:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.two_Words:
                    switch (RootScheme)
                    {
                        case NamingSchemes.twowords:
                        case NamingSchemes.two_Words:
                            return true;
                        default:
                            return false;
                    }

                case NamingSchemes.Two_Words:
                    switch (RootScheme)
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
            string RootName = ExpectedSheme.HasFlag(NamingSchemes.Interface) && name[0] == 'I' ? name.Substring(1) : name;
            NamingSchemes RootScheme = ExpectedSheme & ~NamingSchemes.Interface;

            if (RootName.Contains("_"))
                Result = FixNameWithUnderscore(RootName, RootScheme);
            else
                Result = FixNameWithoutUnderscore(RootName, RootScheme);

            if (ExpectedSheme.HasFlag(NamingSchemes.Interface) && name[0] == 'I')
                Result = "I" + Result;

            return Result;
        }

        private static bool SchemeHasUnderscore(NamingSchemes scheme)
        {
            return scheme.HasFlag(NamingSchemes.two_words) ||
                   scheme.HasFlag(NamingSchemes.two_Words) ||
                   scheme.HasFlag(NamingSchemes.TWO_WORDS) ||
                   scheme.HasFlag(NamingSchemes.Two_Words);
        }

        private string FixNameWithUnderscore(string name, NamingSchemes expectedSheme)
        {
            string Result;

            if (!SchemeHasUnderscore(expectedSheme))
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

        /// <summary>
        /// Gets the string corresponding to <paramref name="namespaceDeclaration"/>.
        /// </summary>
        /// <param name="namespaceDeclaration">The namespace.</param>
        public static string NamespaceNameToString(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            return NameToString(namespaceDeclaration.Name);
        }

        /// <summary>
        /// Gets the string corresponding to <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        public static string NameToString(NameSyntax name)
        {
            switch (name)
            {
                case AliasQualifiedNameSyntax AsAliasQualifiedName:
                    return AliasQualifiedNameToString(AsAliasQualifiedName);
                case QualifiedNameSyntax AsQualifiedName:
                    return QualifiedNameToString(AsQualifiedName);
                case SimpleNameSyntax AsSimpleName:
                    return SimpleNameToString(AsSimpleName);
                default:
                    return null!;
            }
        }

        /// <summary>
        /// Gets the string corresponding to <paramref name="aliasQualifiedName"/>.
        /// </summary>
        /// <param name="aliasQualifiedName">The alias name.</param>
        public static string AliasQualifiedNameToString(AliasQualifiedNameSyntax aliasQualifiedName)
        {
            return $"{IdentifierNameToString(aliasQualifiedName.Alias)}::{SimpleNameToString(aliasQualifiedName.Name)}";
        }

        /// <summary>
        /// Gets the string corresponding to <paramref name="qualifiedName"/>.
        /// </summary>
        /// <param name="qualifiedName">The qualified name.</param>
        public static string QualifiedNameToString(QualifiedNameSyntax qualifiedName)
        {
            return $"{NameToString(qualifiedName.Left)}.{SimpleNameToString(qualifiedName.Right)}";
        }

        /// <summary>
        /// Gets the string corresponding to <paramref name="simpleName"/>.
        /// </summary>
        /// <param name="simpleName">The simple name.</param>
        public static string SimpleNameToString(SimpleNameSyntax simpleName)
        {
            switch (simpleName)
            {
                case IdentifierNameSyntax AsIdentifierName:
                    return IdentifierNameToString(AsIdentifierName);
                default:
                    return null!;
            }
        }

        /// <summary>
        /// Gets the string corresponding to <paramref name="identifierName"/>.
        /// </summary>
        /// <param name="identifierName">The identifier name.</param>
        public static string IdentifierNameToString(IdentifierNameSyntax identifierName)
        {
            return identifierName.Identifier.ValueText;
        }

        private void CheckIndentation(SyntaxNode node)
        {
            if (IsIndentationKnown)
                return;

            if (!node.HasLeadingTrivia)
                return;

            SyntaxTriviaList LeadingTriviaList = node.GetLeadingTrivia();

            foreach (SyntaxTrivia Trivia in LeadingTriviaList.Reverse())
                if (Trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    string Text = Trivia.ToString();
                    if (Text.Contains("\t"))
                        IsIndentationUsingTab = true;
                    else
                        WhitespaceIndentation += Trivia.Span.End - Trivia.Span.Start;
                }

            if (IsIndentationUsingTab && WhitespaceIndentation > 0)
            {
                IsIndentationUsingTab = false;
                WhitespaceIndentation = 0;
            }
        }

        /// <summary>
        /// Gets the indentation level corresponding to a list of trivia.
        /// </summary>
        /// <param name="triviaList">The trivia list.</param>
        /// <param name="indentationLevel">The indentation level upon return.</param>
        /// <param name="traceLevel">The trace level.</param>
        /// <returns>True if indentation could be calculated; otherwise, false.</returns>
        public bool GetIndentationLevel(SyntaxTriviaList triviaList, out int indentationLevel, TraceLevel traceLevel)
        {
            indentationLevel = 0;

            foreach (SyntaxTrivia Trivia in triviaList.Reverse())
                if (Trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    if (!UpdateWhitespaceTriviaIndentationLevel(Trivia, ref indentationLevel, traceLevel))
                        return false;
                }
                else if (Trivia.IsKind(SyntaxKind.EndOfLineTrivia) || Trivia.IsKind(SyntaxKind.RegionDirectiveTrivia) || Trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia))
                    break;
                else
                {
                    string Text = Trivia.ToString();
                    if (Text.Contains("\n"))
                        break;
                }

            return true;
        }

        private bool UpdateWhitespaceTriviaIndentationLevel(SyntaxTrivia trivia, ref int indentationLevel, TraceLevel traceLevel)
        {
            if (IsIndentationUsingTab)
            {
                if (!UpdateTabIndentationLevel(trivia, ref indentationLevel))
                    return false;
            }
            else
            {
                if (!UpdateWhitespaceIndentationLevel(trivia, ref indentationLevel, traceLevel))
                    return false;
            }

            return true;
        }

        private bool UpdateTabIndentationLevel(SyntaxTrivia trivia, ref int indentationLevel)
        {
            string Text = trivia.ToString();
            foreach (char c in Text)
                if (c != '\t')
                    return false;

            int SpanLength = trivia.Span.End - trivia.Span.Start;
            indentationLevel += SpanLength;
            return true;
        }

        private bool UpdateWhitespaceIndentationLevel(SyntaxTrivia trivia, ref int indentationLevel, TraceLevel traceLevel)
        {
            if (WhitespaceIndentation == 0)
                return false;

            string Text = trivia.ToString();
            foreach (char c in Text)
                if (!char.IsWhiteSpace(c) || c == '\t')
                    return false;

            int SpanLength = trivia.Span.End - trivia.Span.Start;
            int Level = SpanLength / WhitespaceIndentation;

            Analyzer.Trace($"SpanLength: {SpanLength}", traceLevel);

            indentationLevel += Level;

            if (SpanLength > Level * WhitespaceIndentation)
                return false;

            return true;
        }

        private Dictionary<NameCategory, Dictionary<NamingSchemes, int>> SchemeTable = new();
        private Dictionary<NameCategory, NamingSchemes> ExpectedSchemeTable = new();
        private int LocalVariableConstCount;
        private int LocalVariableConstCandidateCount;
        #endregion
    }
}
