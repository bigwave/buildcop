using System;
using System.Collections.Generic;
using System.Globalization;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Rules
{
    /// <summary>
    /// A rule that checks strong naming for a project.
    /// </summary>
    [BuildCopRuleAttribute(ConfigurationType = typeof(ruleElement))]
    public class StrongNamingRule : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StrongNamingRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        public StrongNamingRule(ruleElement configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Check

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public override IList<LogEntry> Check(BuildFile project)
        {
            List<LogEntry> entries = new List<LogEntry>();

            if (!config.strongNaming.strongNameRequired)
            {
                // Signing should not be enabled.
                if (project.SignAssembly)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "Signing is enabled for the project but it should be disabled.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured to be strong named using the key \"{0}\", but strong naming should not be enabled.", project.AssemblyOriginatorKeyFile);
                    entries.Add(new LogEntry(Name, "SigningShouldBeDisabled", LogLevel.Error, message, detail));
                }
            }
            else
            {
                // Signing should be enabled.
                if (!project.SignAssembly || string.IsNullOrEmpty(project.AssemblyOriginatorKeyFile))
                {
                    if (!config.strongNaming.ignoreUnsignedProjects)
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, "Signing is disabled for the project but it should be enabled.");
                        string detail = string.Format(CultureInfo.CurrentCulture, "The project is not configured to be strong named but should be signed with the key file \"{0}\"", config.strongNaming.keyPath);
                        entries.Add(new LogEntry(Name, "SigningShouldBeEnabled", LogLevel.Error, message, detail));
                    }
                }
                else if (!project.AssemblyOriginatorKeyFile.Equals(config.strongNaming.keyPath, StringComparison.OrdinalIgnoreCase))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "Signing is enabled but an incorrect key is used: the key file \"{0}\" is used instead of the expected key file \"{1}\".", project.AssemblyOriginatorKeyFile, config.strongNaming.keyPath);
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured to be strong named using the key file \"{0}\", but it should use the key file \"{1}\".", project.AssemblyOriginatorKeyFile, config.strongNaming.keyPath);
                    entries.Add(new LogEntry(Name, "SignedWithIncorrectKey", LogLevel.Error, message, detail));
                }
            }

            return entries;
        }

        #endregion
    }
}