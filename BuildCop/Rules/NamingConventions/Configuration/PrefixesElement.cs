using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using BuildCop.Configuration;

namespace BuildCop.Rules.NamingConventions.Configuration
{
    /// <summary>
    /// Defines the naming conventions configuration.
    /// </summary>
    public class PrefixesElement : ConfigurationElement
    {
        #region Constants

        private const string NamespacePrefixPropertyName = "namespacePrefix";
        private const string AssemblyNamePrefixPropertyName = "assemblyNamePrefix";
        private const string AssemblyNameShouldMatchRootNamespacePropertyName = "assemblyNameShouldMatchRootNamespace";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the expected namespace prefix.
        /// </summary>
        [ConfigurationProperty(NamespacePrefixPropertyName, IsRequired = false)]
        public string NamespacePrefix
        {
            get
            {
                return (string)base[NamespacePrefixPropertyName];
            }
            set
            {
                base[NamespacePrefixPropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the expected assembly name prefix.
        /// </summary>
        [ConfigurationProperty(AssemblyNamePrefixPropertyName, IsRequired = false)]
        public string AssemblyNamePrefix
        {
            get
            {
                return (string)base[AssemblyNamePrefixPropertyName];
            }
            set
            {
                base[AssemblyNamePrefixPropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the assembly name must be the same as the root namespace.
        /// </summary>
        [ConfigurationProperty(AssemblyNameShouldMatchRootNamespacePropertyName, IsRequired = false, DefaultValue=false)]
        public bool AssemblyNameShouldMatchRootNamespace
        {
            get
            {
                return (bool)base[AssemblyNameShouldMatchRootNamespacePropertyName];
            }
            set
            {
                base[AssemblyNameShouldMatchRootNamespacePropertyName] = value;
            }
        }

        #endregion
    }
}