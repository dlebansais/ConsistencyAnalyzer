﻿namespace ConsistencyAnalyzer
{
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
        /// A class, record or struct name.
        /// </summary>
        ClassRecordOrStruct,

        /// <summary>
        /// An enum name.
        /// </summary>
        Enum,

        /// <summary>
        /// An interface name.
        /// </summary>
        Interface,

        /// <summary>
        /// A type name.
        /// </summary>
        Type,

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
        /// A local variable name.
        /// </summary>
        LocalVariable,
    }
}
