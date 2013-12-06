using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

using JelleDruyts.BuildCop.Configuration;

namespace JelleDruyts.BuildCop.Rules.BuildProperties.Configuration
{
    /// <summary>
    /// Defines the configuration for the <see cref="JelleDruyts.BuildCop.Rules.BuildProperties.BuildPropertiesRule"/>.
    /// </summary>
    public class BuildPropertiesRuleElement : RuleConfigurationElement
    {
        #region Constants

        private const string BuildPropertiesPropertyName = "buildProperties";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the prefix settings.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty(BuildPropertiesPropertyName, IsRequired = false)]
        public BuildPropertyCollection BuildProperties
        {
            get
            {
                return (BuildPropertyCollection)base[BuildPropertiesPropertyName];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPropertiesRuleElement"/> class.
        /// </summary>
        public BuildPropertiesRuleElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPropertiesRuleElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        public BuildPropertiesRuleElement(XmlReader reader)
            : base(reader)
        {
        }

        #endregion
    }
}