using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using BuildCop.Configuration;
using BuildCop.Reporting;
using BuildCop.Rules.BuildProperties.Configuration;

namespace BuildCop.Rules.BuildProperties
{
    /// <summary>
    /// A rule that checks naming conventions for a project.
    /// </summary>
    [BuildCopRuleAttribute(ConfigurationType = typeof(BuildPropertiesRuleElement))]
    public class BuildPropertiesRule : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPropertiesRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        public BuildPropertiesRule(RuleConfigurationElement configuration)
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
            BuildPropertiesRuleElement config = this.GetTypedConfiguration<BuildPropertiesRuleElement>();
            List<LogEntry> entries = new List<LogEntry>();

            foreach (BuildPropertyElement expectedProperty in config.BuildProperties)
            {
                StringComparison comparisonType = expectedProperty.StringCompareOption;
                IList<BuildProperty> properties = project.FindProperties(expectedProperty.Name, expectedProperty.Condition);

                if (expectedProperty.CompareOption == CompareOption.Exists)
                {
                    if (properties.Count == 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" should exist.", expectedProperty.Name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} was not found but should exist in the build file.", expectedProperty.Name, condition);
                        entries.Add(new LogEntry(this.Name, "PropertyShouldExist", LogLevel.Error, message, detail));
                    }
                }
                else if (expectedProperty.CompareOption == CompareOption.DoesNotExist)
                {
                    if (properties.Count != 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" should not exist.", expectedProperty.Name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} was found but should not exist in the build file.", expectedProperty.Name, condition);
                        entries.Add(new LogEntry(this.Name, "PropertyShouldNotExist", LogLevel.Error, message, detail));
                    }
                }
                else if (expectedProperty.CompareOption == CompareOption.EqualTo)
                {
                    if (properties.Count == 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" was not found.", expectedProperty.Name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} does not exist in the build file.", expectedProperty.Name, condition);
                        entries.Add(new LogEntry(this.Name, "PropertyShouldExist", LogLevel.Error, message, detail));
                    }
                    else
                    {
                        foreach (BuildProperty property in properties)
                        {
                            if (!string.Equals(property.Value, expectedProperty.Value, comparisonType))
                            {
                                string condition = GetConditionSubstring(expectedProperty);
                                string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should have the expected value \"{2}\".", expectedProperty.Name, condition, expectedProperty.Value);
                                string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should be \"{3}\".", expectedProperty.Name, condition, property.Value, expectedProperty.Value);
                                entries.Add(new LogEntry(this.Name, "IncorrectValue", LogLevel.Error, message, detail));
                            }
                        }
                    }
                }
                else if (expectedProperty.CompareOption == CompareOption.NotEqualTo)
                {
                    foreach (BuildProperty property in properties)
                    {
                        if (string.Equals(property.Value, expectedProperty.Value, comparisonType))
                        {
                            string condition = GetConditionSubstring(expectedProperty);
                            string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should not have the value \"{2}\".", expectedProperty.Name, condition, expectedProperty.Value);
                            string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but this value is not allowed.", expectedProperty.Name, condition, property.Value);
                            entries.Add(new LogEntry(this.Name, "IncorrectValue", LogLevel.Error, message, detail));
                        }
                    }
                }
                else if (expectedProperty.CompareOption == CompareOption.In)
                {
                    if (properties.Count == 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" was not found.", expectedProperty.Name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} does not exist in the build file.", expectedProperty.Name, condition);
                        entries.Add(new LogEntry(this.Name, "PropertyShouldExist", LogLevel.Error, message, detail));
                    }
                    foreach (BuildProperty property in properties)
                    {
                        if (expectedProperty.Value.IndexOf(property.Value, comparisonType) < 0)
                        {
                            string condition = GetConditionSubstring(expectedProperty);
                            string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should be in the list \"{2}\".", expectedProperty.Name, condition, expectedProperty.Value);
                            string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should be in the list \"{3}\".", expectedProperty.Name, condition, property.Value, expectedProperty.Value);
                            entries.Add(new LogEntry(this.Name, "IncorrectValue", LogLevel.Error, message, detail));
                        }
                    }
                }
                else if (expectedProperty.CompareOption == CompareOption.NotIn)
                {
                    foreach (BuildProperty property in properties)
                    {
                        if (expectedProperty.Value.IndexOf(property.Value, comparisonType) >= 0)
                        {
                            string condition = GetConditionSubstring(expectedProperty);
                            string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should not be in the list \"{2}\".", expectedProperty.Name, condition, expectedProperty.Value);
                            string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should not be in the list \"{3}\".", expectedProperty.Name, condition, property.Value, expectedProperty.Value);
                            entries.Add(new LogEntry(this.Name, "IncorrectValue", LogLevel.Error, message, detail));
                        }
                    }
                }
            }

            return entries;
        }

        private static string GetConditionSubstring(BuildPropertyElement expectedProperty)
        {
            return (string.IsNullOrEmpty(expectedProperty.Condition) ? "" : string.Format(CultureInfo.CurrentCulture, " with condition \"{0}\"", expectedProperty.Condition));
        }

        #endregion
    }
}