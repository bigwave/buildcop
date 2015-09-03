using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Rules
{
    /// <summary>
    /// A rule that checks that an XML documentation file is generated as part of the build.
    /// </summary>
    public class DocumentationFileRule : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationFileRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        public DocumentationFileRule(ruleElement configuration)
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

            foreach (string condition in new string[] { "Debug", "Release" })
            {
                IList<BuildProperty> properties = project.FindProperties("DocumentationFile", condition);
                if (properties.Count == 0)
                {
                    LogEntry entry = CreateMissingDocumentationFileLogEntry(condition);
                    entries.Add(entry);
                }
                else
                {
                    foreach (BuildProperty property in properties)
                    {
                        if (string.IsNullOrEmpty(property.Value))
                        {
                            LogEntry entry = CreateMissingDocumentationFileLogEntry(condition);
                            entries.Add(entry);
                        }
                        else
                        {
                            string outputPath = project.GetPropertyValue("OutputPath", condition);
                            string expectedValue = Path.Combine(outputPath, project.GetPropertyValue("AssemblyName")) + ".xml";
                            if (!string.Equals(expectedValue, property.Value, StringComparison.OrdinalIgnoreCase))
                            {
                                string message = string.Format(CultureInfo.CurrentCulture, "The documentation file has an incorrect file name.");
                                string detail = string.Format(CultureInfo.CurrentCulture, "The XML documentation file is named \"{0}\" but it should be \"{1}\"", property.Value, expectedValue);
                                entries.Add(new LogEntry(Name, "IncorrectFileName", LogLevel.Error, message, detail));
                            }
                        }
                    }
                }
            }

            return entries;
        }

        /// <summary>
        /// Creates a missing documentation file log entry.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>A log entry for the missing documentation file.</returns>
        private LogEntry CreateMissingDocumentationFileLogEntry(string condition)
        {
            string message = string.Format(CultureInfo.CurrentCulture, "The documentation file is disabled for the project but it should be enabled.");
            string detail = string.Format(CultureInfo.CurrentCulture, "The project is not configured to create an XML documentation file for the {0} configuration.", condition);
            LogEntry entry = new LogEntry(Name, "MissingDocumentationFile", LogLevel.Error, message, detail);
            return entry;
        }

        #endregion
    }
}