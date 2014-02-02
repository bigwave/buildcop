using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Rules
{
    /// <summary>
    /// A rule that checks naming conventions for a project.
    /// </summary>
    [BuildCopRuleAttribute(ConfigurationType = typeof(ruleElement))]
    public class NamingConventionsRule : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NamingConventionsRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        public NamingConventionsRule(ruleElement configuration)
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

            if (!string.IsNullOrEmpty(config.prefixes.namespacePrefix))
            {
                if (!project.RootNamespace.StartsWith(config.prefixes.namespacePrefix, StringComparison.Ordinal))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "The project has a wrong root namespace.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured with the root namespace \"{0}\", but the root namespace should start with \"{1}\".", project.RootNamespace, config.prefixes.namespacePrefix);
                    entries.Add(new LogEntry(this.Name, "IncorrectRootNamespace", LogLevel.Error, message, detail));
                }
            }

            if (!string.IsNullOrEmpty(config.prefixes.assemblyNamePrefix))
            {
                if (!project.AssemblyName.StartsWith(config.prefixes.assemblyNamePrefix, StringComparison.Ordinal))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "The project has a wrong assembly name.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured with the assembly name \"{0}\", but the assembly name should start with \"{1}\".", project.AssemblyName, config.prefixes.assemblyNamePrefix);
                    entries.Add(new LogEntry(this.Name, "IncorrectAssemblyName", LogLevel.Error, message, detail));
                }
            }

            if (config.prefixes.assemblyNameShouldMatchRootNamespace)
            {
                if (!string.Equals(project.AssemblyName, project.RootNamespace, StringComparison.Ordinal))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "The assembly name does not match the root namespace.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured with the assembly name \"{0}\", but it should be the same as the root namespace \"{1}\".", project.AssemblyName, project.RootNamespace);
                    entries.Add(new LogEntry(this.Name, "AssemblyNameRootNamespaceMismatch", LogLevel.Error, message, detail));
                }
            }

            return entries;
        }

        #endregion
    }
}