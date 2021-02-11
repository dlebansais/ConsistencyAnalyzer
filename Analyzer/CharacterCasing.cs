namespace ConsistencyAnalyzer
{
    /// <summary>
    /// Represents allowed casing in a name.
    /// </summary>
    internal enum CharacterCasing
    {
        /// <summary>
        /// Can only contain lower case character.
        /// </summary>
        OnlyLower,

        /// <summary>
        /// Can only contain upper case character.
        /// </summary>
        OnlyUpper,

        /// <summary>
        /// The first character must lower case.
        /// </summary>
        FirstLower,

        /// <summary>
        /// The first character must upper case.
        /// </summary>
        FirstUpper,
    }
}
