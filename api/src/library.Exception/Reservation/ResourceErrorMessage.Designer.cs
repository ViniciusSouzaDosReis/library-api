﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace library.Exception.Reservation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourceErrorMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceErrorMessage() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("library.Exception.Reservation.ResourceErrorMessage", typeof(ResourceErrorMessage).Assembly);
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
        ///   Looks up a localized string similar to The book is reserved.
        /// </summary>
        public static string BOOK_IS_RESERVED {
            get {
                return ResourceManager.GetString("BOOK_IS_RESERVED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reservation already picked.
        /// </summary>
        public static string RESERVATION_ALREADY_PICKED {
            get {
                return ResourceManager.GetString("RESERVATION_ALREADY_PICKED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reservation already requested.
        /// </summary>
        public static string RESERVATION_ALREADY_REQUESTED {
            get {
                return ResourceManager.GetString("RESERVATION_ALREADY_REQUESTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reservation already returned.
        /// </summary>
        public static string RESERVATION_ALREADY_RETURNED {
            get {
                return ResourceManager.GetString("RESERVATION_ALREADY_RETURNED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reservetion not found.
        /// </summary>
        public static string RESERVATION_NOT_FOUND {
            get {
                return ResourceManager.GetString("RESERVATION_NOT_FOUND", resourceCulture);
            }
        }
    }
}
