namespace ConsistencyAnalyzer;

/// <summary>
/// The different region modes.
/// </summary>
public enum RegionModes
{
    /// <summary>
    /// Undecided.
    /// </summary>
    Undecided,

    /// <summary>
    /// Free grouping.
    /// </summary>
    Free,

    /// <summary>
    /// Accessibility, public/protected.
    /// </summary>
    AccessibilitySimple,

    /// <summary>
    /// Accessibility, public/protected, and constructor/property/field/event/method in public.
    /// </summary>
    AccessibilityFull,

    /// <summary>
    /// By topic.
    /// </summary>
    Topic,

    /// <summary>
    /// By interface implementation.
    /// </summary>
    InterfaceImplementation,

    /// <summary>
    /// AccessibilitySimple + interface implementation.
    /// </summary>
    MixedInterfaceAccessibilitySimple,

    /// <summary>
    /// AccessibilityFull + interface implementation.
    /// </summary>
    MixedInterfaceAccessibilityFull,
}
