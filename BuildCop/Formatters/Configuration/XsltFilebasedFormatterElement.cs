using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

using BuildCop.Configuration;

namespace BuildCop.Formatters.Configuration
{
    /// <summary>
    /// Defines the configuration for file-based formatters that use XSLT.
    /// </summary>
    public class XsltFilebasedFormatterElement : FormatterConfigurationElement
    {
        #region Constants

        private const string OutputPropertyName = "output";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the output settings.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty(OutputPropertyName, IsRequired = false)]
        public XsltOutputElement Output
        {
            get
            {
                return (XsltOutputElement)base[OutputPropertyName];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XsltFilebasedFormatterElement"/> class.
        /// </summary>
        public XsltFilebasedFormatterElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XsltFilebasedFormatterElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        public XsltFilebasedFormatterElement(XmlReader reader)
            : base(reader)
        {
        }

        #endregion
    }
}