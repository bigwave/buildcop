using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

using JelleDruyts.BuildCop.Configuration;

namespace JelleDruyts.BuildCop.Rules.NamingConventions.Configuration
{
    /// <summary>
    /// Defines the configuration for the <see cref="JelleDruyts.BuildCop.Rules.NamingConventions.NamingConventionsRule"/>.
    /// </summary>
    public class NamingConventionsRuleElement : RuleConfigurationElement
    {
        #region Constants

        private const string PrefixesPropertyName = "prefixes";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the prefix settings.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty(PrefixesPropertyName, IsRequired = false)]
        public PrefixesElement Prefixes
        {
            get
            {
                return (PrefixesElement)base[PrefixesPropertyName];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NamingConventionsRuleElement"/> class.
        /// </summary>
        public NamingConventionsRuleElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamingConventionsRuleElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        public NamingConventionsRuleElement(XmlReader reader)
            : base(reader)
        {
        }

        #endregion
    }
}