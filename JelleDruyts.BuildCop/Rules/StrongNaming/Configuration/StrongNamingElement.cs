using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace JelleDruyts.BuildCop.Rules.StrongNaming.Configuration
{
    /// <summary>
    /// Defines the strong naming configuration.
    /// </summary>
    public class StrongNamingElement : ConfigurationElement
    {
        #region Constants

        private const string StrongNameRequiredPropertyName = "strongNameRequired";
        private const string KeyPathPropertyName = "keyPath";
        private const string IgnoreUnsignedProjectsPropertyName = "ignoreUnsignedProjects";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that determines if strong naming should be enabled.
        /// </summary>
        [ConfigurationProperty(StrongNameRequiredPropertyName, IsRequired = true)]
        public bool StrongNameRequired
        {
            get
            {
                return (bool)base[StrongNameRequiredPropertyName];
            }
            set
            {
                base[StrongNameRequiredPropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the path to the strong naming key, if signing is required.
        /// </summary>
        [ConfigurationProperty(KeyPathPropertyName, IsRequired = false)]
        public string KeyPath
        {
            get
            {
                return (string)base[KeyPathPropertyName];
            }
            set
            {
                base[KeyPathPropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines if unsigned projects should be ignored when checking the key path.
        /// </summary>
        [ConfigurationProperty(IgnoreUnsignedProjectsPropertyName, IsRequired = false)]
        public bool IgnoreUnsignedProjects
        {
            get
            {
                return (bool)base[IgnoreUnsignedProjectsPropertyName];
            }
            set
            {
                base[IgnoreUnsignedProjectsPropertyName] = value;
            }
        }

        #endregion
    }
}