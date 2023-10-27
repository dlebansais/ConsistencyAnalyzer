namespace ConsistencyAnalyzer;

/// <summary>
/// Represents how underscore are used in names.
/// </summary>
internal enum UnderscoreUse
{
    /// <summary>
    /// Not allowed.
    /// </summary>
    Never,

    /// <summary>
    /// At least one underscore required
    /// </summary>
    Always,

    /// <summary>
    /// The name can have underscore or not.
    /// </summary>
    Optional,
}
