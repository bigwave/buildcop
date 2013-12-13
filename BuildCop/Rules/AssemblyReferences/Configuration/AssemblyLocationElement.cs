using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BuildCop.Rules.AssemblyReferences.Configuration
{
    /// <summary>
    /// Defines an assembly location.
    /// </summary>
    public class AssemblyLocationElement : ConfigurationElement
    {
        #region Constants

        private const string AssemblyNamePropertyName = "assemblyName";
        private const string AssemblyPathPropertyName = "assemblyPath";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of this assembly.
        /// </summary>
        [ConfigurationProperty(AssemblyNamePropertyName, IsRequired = true)]
        public string AssemblyName
        {
            get
            {
                return (string)base[AssemblyNamePropertyName];
            }
            set
            {
                base[AssemblyNamePropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the path to this assembly.
        /// </summary>
        [ConfigurationProperty(AssemblyPathPropertyName, IsRequired = true)]
        public string AssemblyPath
        {
            get
            {
                return (string)base[AssemblyPathPropertyName];
            }
            set
            {
                base[AssemblyPathPropertyName] = value;
            }
        }

        #endregion
    }
}