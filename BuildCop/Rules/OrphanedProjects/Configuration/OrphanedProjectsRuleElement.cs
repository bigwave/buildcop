using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

using BuildCop.Configuration;

namespace BuildCop.Rules.OrphanedProjects.Configuration
{
    /// <summary>
    /// Defines the configuration for the <see cref="BuildCop.Rules.OrphanedProjects.OrphanedProjectsRule"/>.
    /// </summary>
    public class OrphanedProjectsRuleElement : RuleConfigurationElement
    {
        #region Constants

        private const string SolutionsPropertyName = "solutions";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the strong naming settings.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty(SolutionsPropertyName, IsRequired = true)]
        public SolutionsElement Solutions
        {
            get
            {
                return (SolutionsElement)base[SolutionsPropertyName];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrphanedProjectsRuleElement"/> class.
        /// </summary>
        public OrphanedProjectsRuleElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrphanedProjectsRuleElement"/> class.
        /// </summary>
        /// <param name="reader">The XML reader from which to read the configuration.</param>
        public OrphanedProjectsRuleElement(XmlReader reader)
            : base(reader)
        {
        }

        #endregion
    }
}