using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

using JelleDruyts.BuildCop.Configuration;

namespace JelleDruyts.BuildCop.Formatters.Configuration
{
    /// <summary>
    /// Defines the configuration for file-based formatters.
    /// </summary>
    public class FilebasedFormatterElement : FormatterConfigurationElement
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
        public OutputElement Output
        {
            get
            {
                return (OutputElement)base[OutputPropertyName];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilebasedFormatterElement"/> class.
        /// </summary>
        public FilebasedFormatterElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilebasedFormatterElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        public FilebasedFormatterElement(XmlReader reader)
            : base(reader)
        {
        }

        #endregion
    }
}