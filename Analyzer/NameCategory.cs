namespace ConsistencyAnalyzer;

/// <summary>
/// Represents a name category.
/// </summary>
public enum NameCategory
{
    /// <summary>
    /// No specific name.
    /// </summary>
    Neutral,

    /// <summary>
    /// A namespace name.
    /// </summary>
    Namespace,

    /// <summary>
    /// A class name.
    /// </summary>
    Class,

    /// <summary>
    /// A record name.
    /// </summary>
    Record,

    /// <summary>
    /// A struct name.
    /// </summary>
    Struct,

    /// <summary>
    /// An enum name.
    /// </summary>
    Enum,

    /// <summary>
    /// An interface name.
    /// </summary>
    Interface,

    /// <summary>
    /// A delegate name.
    /// </summary>
    Delegate,

    /// <summary>
    /// A field name.
    /// </summary>
    Field,

    /// <summary>
    /// An event name.
    /// </summary>
    Event,

    /// <summary>
    /// A method name.
    /// </summary>
    Method,

    /// <summary>
    /// A property name.
    /// </summary>
    Property,

    /// <summary>
    /// An enum member name.
    /// </summary>
    EnumMember,

    /// <summary>
    /// A parameter name.
    /// </summary>
    Parameter,

    /// <summary>
    /// A type parameter name.
    /// </summary>
    TypeParameter,

    /// <summary>
    /// A local variable name.
    /// </summary>
    LocalVariable,
}
