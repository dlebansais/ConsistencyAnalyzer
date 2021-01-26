﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsistencyAnalyzer {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConsistencyAnalyzer.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inconsistent const-ness of unmodified variable.
        /// </summary>
        public static string ConA1000Description {
            get {
                return ResourceManager.GetString("ConA1000Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; can be made constant.
        /// </summary>
        public static string ConA1000MessageFormat {
            get {
                return ResourceManager.GetString("ConA1000MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Variable can be made constant.
        /// </summary>
        public static string ConA1000Title {
            get {
                return ResourceManager.GetString("ConA1000Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inconsistent documentation of enumeration element.
        /// </summary>
        public static string ConA1602Description {
            get {
                return ResourceManager.GetString("ConA1602Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; can be documented.
        /// </summary>
        public static string ConA1602MessageFormat {
            get {
                return ResourceManager.GetString("ConA1602MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enumeration element can be documented.
        /// </summary>
        public static string ConA1602Title {
            get {
                return ResourceManager.GetString("ConA1602Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use regions for all class code or not at all.
        /// </summary>
        public static string ConA1700Description {
            get {
                return ResourceManager.GetString("ConA1700Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is missing regions.
        /// </summary>
        public static string ConA1700MessageFormat {
            get {
                return ResourceManager.GetString("ConA1700MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Class is missing regions.
        /// </summary>
        public static string ConA1700Title {
            get {
                return ResourceManager.GetString("ConA1700Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Avoid nested regions and refactor the code or use partial classes instead.
        /// </summary>
        public static string ConA1701Description {
            get {
                return ResourceManager.GetString("ConA1701Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Region &apos;{0}&apos; is nested within region &apos;{1}&apos;.
        /// </summary>
        public static string ConA1701MessageFormat {
            get {
                return ResourceManager.GetString("ConA1701MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do not use nested regions.
        /// </summary>
        public static string ConA1701Title {
            get {
                return ResourceManager.GetString("ConA1701Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A public member of a class is not the same region as other public members.
        /// </summary>
        public static string ConA1702Description {
            get {
                return ResourceManager.GetString("ConA1702Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Class member &apos;{0}&apos; is not within region &apos;{1}&apos;.
        /// </summary>
        public static string ConA1702MessageFormat {
            get {
                return ResourceManager.GetString("ConA1702MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Public member outside the expected region.
        /// </summary>
        public static string ConA1702Title {
            get {
                return ResourceManager.GetString("ConA1702Title", resourceCulture);
            }
        }
    }
}
