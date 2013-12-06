using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules.StrongNaming.Configuration;

namespace JelleDruyts.BuildCop.Rules.StrongNaming
{
    /// <summary>
    /// A rule that checks strong naming for a project.
    /// </summary>
    [BuildCopRuleAttribute(ConfigurationType = typeof(StrongNamingRuleElement))]
    public class StrongNamingRule : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StrongNamingRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        public StrongNamingRule(RuleConfigurationElement configuration)
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
            StrongNamingRuleElement config = this.GetTypedConfiguration<StrongNamingRuleElement>();
            List<LogEntry> entries = new List<LogEntry>();

            if (!config.StrongNaming.StrongNameRequired)
            {
                // Signing should not be enabled.
                if (project.SignAssembly)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "Signing is enabled for the project but it should be disabled.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured to be strong named using the key \"{0}\", but strong naming should not be enabled.", project.AssemblyOriginatorKeyFile);
                    entries.Add(new LogEntry(this.Name, "SigningShouldBeDisabled", LogLevel.Error, message, detail));
                }
            }
            else
            {
                // Signing should be enabled.
                if (!project.SignAssembly || string.IsNullOrEmpty(project.AssemblyOriginatorKeyFile))
                {
                    if (!config.StrongNaming.IgnoreUnsignedProjects)
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, "Signing is disabled for the project but it should be enabled.");
                        string detail = string.Format(CultureInfo.CurrentCulture, "The project is not configured to be strong named but should be signed with the key file \"{0}\"", config.StrongNaming.KeyPath);
                        entries.Add(new LogEntry(this.Name, "SigningShouldBeEnabled", LogLevel.Error, message, detail));
                    }
                }
                else if (!project.AssemblyOriginatorKeyFile.Equals(config.StrongNaming.KeyPath, StringComparison.OrdinalIgnoreCase))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "Signing is enabled but an incorrect key is used: the key file \"{0}\" is used instead of the expected key file \"{1}\".", project.AssemblyOriginatorKeyFile, config.StrongNaming.KeyPath);
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured to be strong named using the key file \"{0}\", but it should use the key file \"{1}\".", project.AssemblyOriginatorKeyFile, config.StrongNaming.KeyPath);
                    entries.Add(new LogEntry(this.Name, "SignedWithIncorrectKey", LogLevel.Error, message, detail));
                }
            }

            return entries;
        }

        #endregion
    }
}