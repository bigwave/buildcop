using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

using JelleDruyts.BuildCop.Configuration;

namespace JelleDruyts.BuildCop.Rules.StrongNaming.Configuration
{
    /// <summary>
    /// Defines the configuration for the <see cref="JelleDruyts.BuildCop.Rules.StrongNaming.StrongNamingRule"/>.
    /// </summary>
    public class StrongNamingRuleElement : RuleConfigurationElement
    {
        #region Constants

        private const string StrongNamingPropertyName = "strongNaming";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the strong naming settings.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty(StrongNamingPropertyName, IsRequired = true)]
        public StrongNamingElement StrongNaming
        {
            get
            {
                return (StrongNamingElement)base[StrongNamingPropertyName];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StrongNamingRuleElement"/> class.
        /// </summary>
        public StrongNamingRuleElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrongNamingRuleElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        public StrongNamingRuleElement(XmlReader reader)
            : base(reader)
        {
        }

        #endregion
    }
}