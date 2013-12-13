using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using BuildCop.Formatters;
using BuildCop.Reporting;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a formatter for a BuildCop report.
    /// </summary>
    public partial class FormatterElement : ConfigurationElement
    {
        #region Formatter-Specific Configuration Handling

        /// <summary>
        /// Gets a value indicating whether an unknown element is encountered during deserialization.
        /// </summary>
        /// <param name="elementName">The name of the unknown subelement.</param>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"></see> object being used for deserialization.</param>
        /// <returns>true when an unknown element is encountered while deserializing.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "elementName")]
        private bool HandleUnrecognizedElement(string elementName, XmlReader reader)
        {
            // Determine the formatter type.
            System.Type formatterType = System.Type.GetType(this.Type, true, true);
            if (!typeof(BaseFormatter).IsAssignableFrom(formatterType))
            {
                throw new ConfigurationErrorsException("The formatter type must derive from the BaseFormatter class. Type name: " + this.Type);
            }

            // Find the BuildCopFormatter attribute to determine the formatter's configuration type.
            object[] attributes = formatterType.GetCustomAttributes(typeof(BuildCopFormatterAttribute), true);
            if (attributes.Length != 1)
            {
                throw new ConfigurationErrorsException("The formatter type must have the BuildCopFormatterAttribute applied. Type name: " + this.Type);
            }
            BuildCopFormatterAttribute formatterAttribute = (BuildCopFormatterAttribute)attributes[0];
            System.Type configType = formatterAttribute.ConfigurationType;
            if (configType != null)
            {
                this.formatterConfiguration = ConfigurationHelper.ReadSpecificConfigurationElement<FormatterConfigurationElement>(reader, configType);
            }

            return true;
        }

        private FormatterConfigurationElement formatterConfiguration;

        /// <summary>
        /// Gets the formatter-specific configuration element for this element.
        /// </summary>
        public FormatterConfigurationElement FormatterConfiguration
        {
            get
            {
                return this.formatterConfiguration;
            }
        }

        #endregion
    }
}