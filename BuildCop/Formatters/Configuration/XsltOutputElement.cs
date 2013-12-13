using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using BuildCop.Configuration;

namespace BuildCop.Formatters.Configuration
{
    /// <summary>
    /// Defines the configuration for file-based output with XSLT.
    /// </summary>
    public class XsltOutputElement : OutputElement
    {
        #region Constants

        private const string StylesheetPropertyName = "stylesheet";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the XSLT stylesheet to use.
        /// </summary>
        [ConfigurationProperty(StylesheetPropertyName, IsRequired = false)]
        public string Stylesheet
        {
            get
            {
                return (string)base[StylesheetPropertyName];
            }
            set
            {
                base[StylesheetPropertyName] = value;
            }
        }

        #endregion
    }
}