namespace ConsistencyAnalyzer
{
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
        /// Interface Category, public/protected.
        /// </summary>
        InterfaceCategorySimple,

        /// <summary>
        /// Interface Category, public/protected, and constructor/property/field/event/method in public.
        /// </summary>
        InterfaceCategoryFull,

        /// <summary>
        /// By topic.
        /// </summary>
        Topic,

        /// <summary>
        /// By interface implementation.
        /// </summary>
        InterfaceImplementation,

        /// <summary>
        /// InterfaceCategorySimple + interface implementation.
        /// </summary>
        MixedInterfaceCategorySimple,

        /// <summary>
        /// InterfaceCategoryFull + interface implementation.
        /// </summary>
        MixedInterfaceCategoryFull,
    }
}
