using System;
using System.Collections.Generic;
using System.Globalization;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Rules
{
    /// <summary>
    /// A rule that checks naming conventions for a project.
    /// </summary>
    [BuildCopRuleAttribute(ConfigurationType = typeof(ruleElement))]
    public class BuildPropertiesRule : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPropertiesRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        public BuildPropertiesRule(ruleElement configuration)
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

            foreach (ruleElementBuildProperty expectedProperty in config.buildProperties)
            {
                StringComparison comparisonType = (StringComparison)Enum.Parse(typeof(StringComparison), expectedProperty.stringCompareOption, true);
                IList<BuildProperty> properties = project.FindProperties(expectedProperty.name, expectedProperty.condition);

                 if (StringMatchesEnum(expectedProperty.compareOption,CompareOption.Exists))
                {
                    if (properties.Count == 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" should exist.", expectedProperty.name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} was not found but should exist in the build file.", expectedProperty.name, condition);
                        entries.Add(new LogEntry(Name, "PropertyShouldExist", LogLevel.Error, message, detail));
                    }
                }
                else if (StringMatchesEnum(expectedProperty.compareOption, CompareOption.DoesNotExist))
                {
                    if (properties.Count != 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" should not exist.", expectedProperty.name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} was found but should not exist in the build file.", expectedProperty.name, condition);
                        entries.Add(new LogEntry(Name, "PropertyShouldNotExist", LogLevel.Error, message, detail));
                    }
                }
                else if (StringMatchesEnum(expectedProperty.compareOption, CompareOption.EqualTo))
                {
                    if (properties.Count == 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" was not found.", expectedProperty.name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} does not exist in the build file.", expectedProperty.name, condition);
                        entries.Add(new LogEntry(Name, "PropertyShouldExist", LogLevel.Error, message, detail));
                    }
                    else if (string.IsNullOrEmpty(expectedProperty.condition) && // No condition: either global property, or should exist for all conditions
                             !(properties.Count == 1 && string.IsNullOrEmpty(properties[0].Condition)) && // If only one entry in project and it's condition is empty means it is a global property
                             properties.Count != project.Conditions.Count) // If not a global property, then check that it is present for all conditions
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" was not found in all configurations.", expectedProperty.name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} does not exist in the build file for all configurations.", expectedProperty.name, condition);
                        entries.Add(new LogEntry(Name, "PropertyShouldExist", LogLevel.Error, message, detail));
                    }
                    else
                    {
                        foreach (BuildProperty property in properties)
                        {
                            if (!string.Equals(property.Value, expectedProperty.value, comparisonType))
                            {
                                string condition = GetConditionSubstring(expectedProperty);
                                string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should have the expected value \"{2}\".", expectedProperty.name, condition, expectedProperty.value);
                                string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should be \"{3}\".", expectedProperty.name, condition, property.Value, expectedProperty.value);
                                entries.Add(new LogEntry(Name, "IncorrectValue", LogLevel.Error, message, detail));
                            }
                        }
                    }
                }
                else if (StringMatchesEnum(expectedProperty.compareOption, CompareOption.NotEqualTo))
                {
                    foreach (BuildProperty property in properties)
                    {
                        if (string.Equals(property.Value, expectedProperty.value, comparisonType))
                        {
                            string condition = GetConditionSubstring(expectedProperty);
                            string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should not have the value \"{2}\".", expectedProperty.name, condition, expectedProperty.value);
                            string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but this value is not allowed.", expectedProperty.name, condition, property.Value);
                            entries.Add(new LogEntry(Name, "IncorrectValue", LogLevel.Error, message, detail));
                        }
                    }
                }
                else if (StringMatchesEnum(expectedProperty.compareOption, CompareOption.In))
                {
                    if (properties.Count == 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" was not found.", expectedProperty.name);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} does not exist in the build file.", expectedProperty.name, condition);
                        entries.Add(new LogEntry(Name, "PropertyShouldExist", LogLevel.Error, message, detail));
                    }
                    foreach (BuildProperty property in properties)
                    {
                        if (expectedProperty.value.IndexOf(property.Value, comparisonType) < 0)
                        {
                            string condition = GetConditionSubstring(expectedProperty);
                            string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should be in the list \"{2}\".", expectedProperty.name, condition, expectedProperty.value);
                            string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should be in the list \"{3}\".", expectedProperty.name, condition, property.Value, expectedProperty.value);
                            entries.Add(new LogEntry(Name, "IncorrectValue", LogLevel.Error, message, detail));
                        }
                    }
                }
                else if (StringMatchesEnum(expectedProperty.compareOption, CompareOption.NotIn))
                {
                    foreach (BuildProperty property in properties)
                    {
                        if (expectedProperty.value.IndexOf(property.Value, comparisonType) >= 0)
                        {
                            string condition = GetConditionSubstring(expectedProperty);
                            string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should not be in the list \"{2}\".", expectedProperty.name, condition, expectedProperty.value);
                            string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should not be in the list \"{3}\".", expectedProperty.name, condition, property.Value, expectedProperty.value);
                            entries.Add(new LogEntry(Name, "IncorrectValue", LogLevel.Error, message, detail));
                        }
                    }
                }
            }

            return entries;
        }

        private bool StringMatchesEnum(string p, CompareOption compareOption)
        {
            return ((CompareOption)Enum.Parse(typeof(CompareOption), p, true) == compareOption);
        }

        private static string GetConditionSubstring(ruleElementBuildProperty expectedProperty)
        {
            return (string.IsNullOrEmpty(expectedProperty.condition) ? "" : string.Format(CultureInfo.CurrentCulture, " with condition \"{0}\"", expectedProperty.condition));
        }

        #endregion
        /// <summary>
        /// Specifies comparison options for build properties.
        /// </summary>
        public enum CompareOption
        {
            /// <summary>
            /// The build property's value must be exactly equal to the given value.
            /// </summary>
            EqualTo = 0,

            /// <summary>
            /// The build property's value may not be exactly equal to the given value.
            /// </summary>
            NotEqualTo = 1,

            /// <summary>
            /// The build property must exist (and can have any value).
            /// </summary>
            Exists = 2,

            /// <summary>
            /// The build property may not exist.
            /// </summary>
            DoesNotExist = 3,

            /// <summary>
            /// The build property's value must appear anywhere in the given value.
            /// </summary>
            In = 4,

            /// <summary>
            /// The build property's value may not appear anywhere in the given value.
            /// </summary>
            NotIn = 5
        }
    }
}