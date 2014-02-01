////using System;
////using System.Collections.Generic;
////using System.Text;

////namespace BuildCop.Rules.BuildProperties
////{
////    /// <summary>
////    /// Specifies comparison options for build properties.
////    /// </summary>
////    public enum CompareOption
////    {
////        /// <summary>
////        /// The build property's value must be exactly equal to the given value.
////        /// </summary>
////        EqualTo = 0,

////        /// <summary>
////        /// The build property's value may not be exactly equal to the given value.
////        /// </summary>
////        NotEqualTo = 1,

////        /// <summary>
////        /// The build property must exist (and can have any value).
////        /// </summary>
////        Exists = 2,

////        /// <summary>
////        /// The build property may not exist.
////        /// </summary>
////        DoesNotExist = 3,

////        /// <summary>
////        /// The build property's value must appear anywhere in the given value.
////        /// </summary>
////        In = 4,

////        /// <summary>
////        /// The build property's value may not appear anywhere in the given value.
////        /// </summary>
////        NotIn = 5
////    }
////}