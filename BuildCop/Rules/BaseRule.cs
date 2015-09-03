using System.Collections.Generic;

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

        private readonly ruleElement configuration;

        /// <summary>
        /// Gets or sets the configuration for this rule.
        /// </summary>
        public ruleElement config
        {
            get { return configuration; }
        }

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name
        {
            get { return config.name; }
            set { config.name = value; }
        }

        /// <summary>
        /// Gets or sets the string to find in the names of files to exclude for this rule.
        /// </summary>
        public string ExcludedFiles
        {
            get { return config.excludedFiles; }
            set { config.excludedFiles = value; }
        }

        /// <summary>
        /// Gets or sets the string to find in the output type of files to exclude for this rule.
        /// </summary>
        public string ExcludedOutputTypes
        {
            get { return config.excludedOutputTypes; }
            set { config.excludedOutputTypes = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        protected BaseRule(ruleElement configuration)
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

    }
}