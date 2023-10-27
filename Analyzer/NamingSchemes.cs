namespace ConsistencyAnalyzer;

/// <summary>
/// Represents a naming scheme.
/// </summary>
public enum NamingSchemes
{
    /// <summary>
    /// No scheme.
    /// </summary>
    None = 0,

    /// <summary>
    /// twowords naming scheme.
    /// </summary>
    twowords = 0x0001,

    /// <summary>
    /// TWOWORDS naming scheme.
    /// </summary>
    TWOWORDS = 0x0002,

    /// <summary>
    /// twoWords naming scheme.
    /// </summary>
    twoWords = 0x0004,

    /// <summary>
    /// TwoWords naming scheme.
    /// </summary>
    TwoWords = 0x0008,

    /// <summary>
    /// two_words naming scheme.
    /// </summary>
    two_words = 0x0010,

    /// <summary>
    /// TWO_WORDS naming scheme.
    /// </summary>
    TWO_WORDS = 0x0020,

    /// <summary>
    /// two_Words naming scheme.
    /// </summary>
    two_Words = 0x0040,

    /// <summary>
    /// Two_Words naming scheme.
    /// </summary>
    Two_Words = 0x0080,

    /// <summary>
    /// All schemes combined.
    /// </summary>
    All = 0x00FF,

    /// <summary>
    /// Has the leading I for interface.
    /// </summary>
    Interface = 0x0100,
}
