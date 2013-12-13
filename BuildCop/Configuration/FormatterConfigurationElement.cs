using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

namespace BuildCop.Configuration
{
    /// <summary>
    /// A base class for formatter-specific configuration elements.
    /// </summary>
    public abstract class FormatterConfigurationElement : ConfigurationElement
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterConfigurationElement"/> class.
        /// </summary>
        protected FormatterConfigurationElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterConfigurationElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        protected FormatterConfigurationElement(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }

        #endregion
    }
}