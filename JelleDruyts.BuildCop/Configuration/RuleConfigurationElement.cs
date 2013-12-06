using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

namespace JelleDruyts.BuildCop.Configuration
{
    /// <summary>
    /// A base class for rule-specific configuration elements.
    /// </summary>
    public abstract class RuleConfigurationElement : ConfigurationElement
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleConfigurationElement"/> class.
        /// </summary>
        protected RuleConfigurationElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleConfigurationElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        protected RuleConfigurationElement(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }

        #endregion
    }
}