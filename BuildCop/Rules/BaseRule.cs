using System;
using System.Collections.Generic;
using System.Text;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Rules
{
    /// <summary>
    /// A base class for the different types of rules for build files.
    /// </summary>
    public abstract class BaseRule
    {
        #region Properties

        private readonly RuleConfigurationElement configuration;

        /// <summary>
        /// Gets or sets the configuration for this rule.
        /// </summary>
        public RuleConfigurationElement Configuration
        {
            get { return this.configuration; }
        }

        private string name;

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        private string excludedFiles;

        /// <summary>
        /// Gets or sets the string to find in the names of files to exclude for this rule.
        /// </summary>
        public string ExcludedFiles
        {
            get { return this.excludedFiles; }
            set { this.excludedFiles = value; }
        }

        private string excludedOutputTypes;

        /// <summary>
        /// Gets or sets the string to find in the output type of files to exclude for this rule.
        /// </summary>
        public string ExcludedOutputTypes
        {
            get { return this.excludedOutputTypes; }
            set { this.excludedOutputTypes = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        protected BaseRule(RuleConfigurationElement configuration)
        {
            this.configuration = configuration;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public abstract IList<LogEntry> Check(BuildFile project);

        #endregion

        #region GetTypedConfiguration

        /// <summary>
        /// Gets the configuration as its original strongly typed instance.
        /// </summary>
        /// <typeparam name="TConfigurationType">The type of the configuration to return.</typeparam>
        /// <returns>The <see cref="Configuration"/> property typed as the requested configuration type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected TConfigurationType GetTypedConfiguration<TConfigurationType>() where TConfigurationType : RuleConfigurationElement
        {
            if (this.configuration == null)
            {
                throw new InvalidOperationException("The configuration instance was null and could not be converted to the requested type: " + typeof(TConfigurationType).FullName);
            }

            TConfigurationType typedconfiguration = this.configuration as TConfigurationType;
            if (typedconfiguration == null)
            {
                throw new InvalidOperationException("The configuration instance could not be converted to the requested type: " + typeof(TConfigurationType).FullName);
            }

            return typedconfiguration;
        }

        #endregion
    }
}