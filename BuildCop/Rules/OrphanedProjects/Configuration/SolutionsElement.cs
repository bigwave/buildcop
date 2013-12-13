using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BuildCop.Rules.OrphanedProjects.Configuration
{
    /// <summary>
    /// Defines the orphaned projects configuration.
    /// </summary>
    public class SolutionsElement : ConfigurationElement
    {
        #region Constants

        private const string SearchPathPropertyName = "searchPath";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the path where to search for solutions.
        /// </summary>
        [ConfigurationProperty(SearchPathPropertyName, IsRequired = true)]
        public string SearchPath
        {
            get
            {
                return (string)base[SearchPathPropertyName];
            }
            set
            {
                base[SearchPathPropertyName] = value;
            }
        }

        #endregion
    }
}