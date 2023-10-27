namespace ConsistencyAnalyzer;

/// <summary>
/// Types of members of a class.
/// </summary>
public enum MemberTypes
{
    /// <summary>
    /// Class constructor.
    /// </summary>
    Contructor,

    /// <summary>
    /// Class property.
    /// </summary>
    Property,

    /// <summary>
    /// Class field.
    /// </summary>
    Field,

    /// <summary>
    /// Class method.
    /// </summary>
    Method,
}
