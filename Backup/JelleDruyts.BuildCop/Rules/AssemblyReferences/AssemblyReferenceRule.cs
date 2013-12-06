using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules.AssemblyReferences.Configuration;

namespace JelleDruyts.BuildCop.Rules.AssemblyReferences
{
    /// <summary>
    /// A rule that checks the assembly references in a project.
    /// </summary>
    [BuildCopRuleAttribute(ConfigurationType = typeof(AssemblyReferenceRuleElement))]
    public class AssemblyReferenceRule : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyReferenceRule"/> class.
        /// </summary>
        public AssemblyReferenceRule(RuleConfigurationElement configuration)
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
            AssemblyReferenceRuleElement config = this.GetTypedConfiguration<AssemblyReferenceRuleElement>();
            List<LogEntry> entries = new List<LogEntry>();

            foreach (AssemblyReference reference in project.AssemblyReferences)
            {
                bool foundReference = false;
                foreach (AssemblyLocationElement expectedLocation in config.AssemblyLocations)
                {
                    if (!string.IsNullOrEmpty(expectedLocation.AssemblyName))
                    {
                        if (reference.AssemblyName.StartsWith(expectedLocation.AssemblyName, StringComparison.OrdinalIgnoreCase))
                        {
                            foundReference = true;
                            if (!string.IsNullOrEmpty(expectedLocation.AssemblyPath) && !reference.HintPath.StartsWith(expectedLocation.AssemblyPath, StringComparison.OrdinalIgnoreCase))
                            {
                                string message = string.Format(CultureInfo.CurrentCulture, "Invalid assembly reference.");
                                string detail = string.Format(CultureInfo.CurrentCulture, "The current hint path \"{0}\" for the assembly reference \"{1}\" is incorrect. The hint path should point to \"{2}\".", reference.HintPath, reference.AssemblyName, expectedLocation.AssemblyPath);
                                entries.Add(new LogEntry(this.Name, "IncorrectHintPath", LogLevel.Error, message, detail));
                            }
                            break;
                        }
                    }
                }

                if (!foundReference)
                {
                    // The expected assembly location is missing for this reference, log a warning.
                    string message = string.Format(CultureInfo.CurrentCulture, "An assembly reference was encountered without a matching expected assembly location.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The assembly reference \"{0}\" with current hint path \"{1}\" was not found in the provided list of expected assembly locations. Add an assembly location for this reference.", reference.AssemblyName, reference.HintPath);
                    entries.Add(new LogEntry(this.Name, "MissingAssemblyLocation", LogLevel.Warning, message, detail));
                }
            }

            return entries;
        }

        #endregion
    }
}