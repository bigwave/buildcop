using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

using JelleDruyts.BuildCop.Configuration;

namespace JelleDruyts.BuildCop.Rules.AssemblyReferences.Configuration
{
    /// <summary>
    /// Defines the configuration for the <see cref="JelleDruyts.BuildCop.Rules.AssemblyReferences.AssemblyReferenceRule"/>.
    /// </summary>
    public class AssemblyReferenceRuleElement : RuleConfigurationElement
    {
        #region Constants

        private const string AssemblyLocationsPropertyName = "assemblyLocations";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expected assembly locations.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty(AssemblyLocationsPropertyName, IsRequired = false)]
        public AssemblyLocationCollection AssemblyLocations
        {
            get
            {
                return (AssemblyLocationCollection)base[AssemblyLocationsPropertyName];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyReferenceRuleElement"/> class.
        /// </summary>
        public AssemblyReferenceRuleElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyReferenceRuleElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        public AssemblyReferenceRuleElement(XmlReader reader)
            : base(reader)
        {
        }

        #endregion
    }
}