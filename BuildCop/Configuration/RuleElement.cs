using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using BuildCop.Rules;
using BuildCop.Reporting;
using System.Globalization;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a rule. 
    /// </summary>
    public partial class ruleElement : BuildCopBaseElement
    {

        #region Check

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public IList<LogEntry> Check(BuildFile project)
        {
            List<LogEntry> entries = new List<LogEntry>();

            foreach (ruleElementBuildProperty buildProperty in this.buildProperties)
            {
                CheckBuildProperties(project, entries, buildProperty);
            }

            return entries;
        }

        private void CheckBuildProperties(BuildFile project, List<LogEntry> entries, ruleElementBuildProperty expectedProperty)
        {
            StringComparison comparisonType = (StringComparison)Enum.Parse(typeof(System.StringComparison), expectedProperty.stringCompareOption, true);
            CompareOption theCompareOption = (CompareOption)Enum.Parse(typeof(CompareOption), expectedProperty.compareOption, true);
            IList<BuildProperty> properties = project.FindProperties(expectedProperty.name, expectedProperty.condition);

            if (theCompareOption == CompareOption.Exists)
            {
                if (properties.Count == 0)
                {
                    string condition = GetConditionSubstring(expectedProperty);
                    string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" should exist.", expectedProperty.name);
                    string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} was not found but should exist in the build file.", expectedProperty.name, condition);
                    entries.Add(new LogEntry(this.name, "PropertyShouldExist", LogLevel.Error, message, detail));
                }
            }
            else if (theCompareOption == CompareOption.DoesNotExist)
            {
                if (properties.Count != 0)
                {
                    string condition = GetConditionSubstring(expectedProperty);
                    string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" should not exist.", expectedProperty.name);
                    string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} was found but should not exist in the build file.", expectedProperty.name, condition);
                    entries.Add(new LogEntry(this.name, "PropertyShouldNotExist", LogLevel.Error, message, detail));
                }
            }
            else if (theCompareOption == CompareOption.EqualTo)
            {
                if (properties.Count == 0)
                {
                    string condition = GetConditionSubstring(expectedProperty);
                    string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" was not found.", expectedProperty.name);
                    string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} does not exist in the build file.", expectedProperty.name, condition);
                    entries.Add(new LogEntry(this.name, "PropertyShouldExist", LogLevel.Error, message, detail));
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
                            entries.Add(new LogEntry(this.name, "IncorrectValue", LogLevel.Error, message, detail));
                        }
                    }
                }
            }
            else if (theCompareOption == CompareOption.NotEqualTo)
            {
                foreach (BuildProperty property in properties)
                {
                    if (string.Equals(property.Value, expectedProperty.value, comparisonType))
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should not have the value \"{2}\".", expectedProperty.name, condition, expectedProperty.value);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but this value is not allowed.", expectedProperty.name, condition, property.Value);
                        entries.Add(new LogEntry(this.name, "IncorrectValue", LogLevel.Error, message, detail));
                    }
                }
            }
            else if (theCompareOption == CompareOption.In)
            {
                if (properties.Count == 0)
                {
                    string condition = GetConditionSubstring(expectedProperty);
                    string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\" was not found.", expectedProperty.name);
                    string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} does not exist in the build file.", expectedProperty.name, condition);
                    entries.Add(new LogEntry(this.name, "PropertyShouldExist", LogLevel.Error, message, detail));
                }
                foreach (BuildProperty property in properties)
                {
                    if (expectedProperty.value.IndexOf(property.Value, comparisonType) < 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should be in the list \"{2}\".", expectedProperty.name, condition, expectedProperty.value);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should be in the list \"{3}\".", expectedProperty.name, condition, property.Value, expectedProperty.value);
                        entries.Add(new LogEntry(this.name, "IncorrectValue", LogLevel.Error, message, detail));
                    }
                }
            }
            else if (theCompareOption == CompareOption.NotIn)
            {
                foreach (BuildProperty property in properties)
                {
                    if (expectedProperty.value.IndexOf(property.Value, comparisonType) >= 0)
                    {
                        string condition = GetConditionSubstring(expectedProperty);
                        string message = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} should not be in the list \"{2}\".", expectedProperty.name, condition, expectedProperty.value);
                        string detail = string.Format(CultureInfo.CurrentCulture, "The build property \"{0}\"{1} has the value \"{2}\" but it should not be in the list \"{3}\".", expectedProperty.name, condition, property.Value, expectedProperty.value);
                        entries.Add(new LogEntry(this.name, "IncorrectValue", LogLevel.Error, message, detail));
                    }
                }
            }
        }

        private static string GetConditionSubstring(ruleElementBuildProperty expectedProperty)
        {
            return (string.IsNullOrEmpty(expectedProperty.condition) ? "" : string.Format(CultureInfo.CurrentCulture, " with condition \"{0}\"", expectedProperty.condition));
        }


        #endregion
 

    }
}