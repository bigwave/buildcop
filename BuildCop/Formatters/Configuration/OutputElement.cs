using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using BuildCop.Configuration;

namespace BuildCop.Formatters.Configuration
{
    /// <summary>
    /// Defines the configuration for file-based output.
    /// </summary>
    public class OutputElement : ConfigurationElement
    {
        #region Constants

        private const string FileNamePropertyName = "fileName";
        private const string LaunchPropertyName = "launch";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the file name to which to write the output.
        /// </summary>
        [ConfigurationProperty(FileNamePropertyName, IsRequired = true)]
        public string FileName
        {
            get
            {
                return (string)base[FileNamePropertyName];
            }
            set
            {
                base[FileNamePropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines if the file should be launched after it has been written to.
        /// </summary>
        [ConfigurationProperty(LaunchPropertyName, IsRequired = false)]
        public bool Launch
        {
            get
            {
                return (bool)base[LaunchPropertyName];
            }
            set
            {
                base[LaunchPropertyName] = value;
            }
        }

        #endregion
    }
}