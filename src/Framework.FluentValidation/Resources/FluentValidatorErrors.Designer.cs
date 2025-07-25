﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Framework.FluentValidation.Resources {
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
    internal class FluentValidatorErrors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal FluentValidatorErrors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Framework.FluentValidation.Resources.FluentValidatorErrors", typeof(FluentValidatorErrors).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {PropertyName} must contain fewer than {MaxElements} items. The {PropertyName} contains {TotalElements} element..
        /// </summary>
        internal static string collection_maximum_elements {
            get {
                return ResourceManager.GetString("collection:maximum_elements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {PropertyName} must contain at least {MinElements} items. The {PropertyName} contains {TotalElements} element..
        /// </summary>
        internal static string collection_minimum_elements {
            get {
                return ResourceManager.GetString("collection:minimum_elements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {PropertyName} must contain unique items. The {PropertyName} contains {TotalDuplicates} duplicates..
        /// </summary>
        internal static string collection_unique_elements {
            get {
                return ResourceManager.GetString("collection:unique_elements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid latitude value. It must be between -90 and 90..
        /// </summary>
        internal static string geo_invalid_latitude {
            get {
                return ResourceManager.GetString("geo:invalid_latitude", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid longitude value. It must be between -180 and 180..
        /// </summary>
        internal static string geo_invalid_longitude {
            get {
                return ResourceManager.GetString("geo:invalid_longitude", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The entered national id is invalid..
        /// </summary>
        internal static string national_id_invalid_egyptian_national_id {
            get {
                return ResourceManager.GetString("national_id:invalid_egyptian_national_id", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number has an invalid country calling code..
        /// </summary>
        internal static string phone_number_invalid_country_code {
            get {
                return ResourceManager.GetString("phone_number:invalid_country_code", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{PropertyValue}&quot; is not a valid phone number..
        /// </summary>
        internal static string phone_number_invalid_number {
            get {
                return ResourceManager.GetString("phone_number:invalid_number", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The entered phone number don&apos;t have all the information to be dialled from anywhere inside or outside the country. Please enter the phone number in a global format..
        /// </summary>
        internal static string phone_number_local_number {
            get {
                return ResourceManager.GetString("phone_number:local_number", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field must contain only numbers..
        /// </summary>
        internal static string strings_only_numbers {
            get {
                return ResourceManager.GetString("strings:only_numbers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The URL is invalid..
        /// </summary>
        internal static string url_invalid {
            get {
                return ResourceManager.GetString("url:invalid", resourceCulture);
            }
        }
    }
}
