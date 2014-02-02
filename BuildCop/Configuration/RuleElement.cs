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
using System.Text.RegularExpressions;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a rule. 
    /// </summary>
    public partial class ruleElement : BuildCopBaseElement
    {
        private static Regex SolutionProjectExpression = new Regex("Project\\(\"(?<TypeGuid>.*?)\"\\)\\s*=\\s*\"(?<ProjectName>.*?)\",\\s*\"(?<ProjectFileName>.*?)\",\\s*\"(?<ProjectGuid>.*?)\"", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public IList<LogEntry> Check(BuildFile project)
        {
            List<LogEntry> entries = new List<LogEntry>();

            entries.AddRange(CheckBuildProperties(project));
            entries.AddRange(CheckDocumentationFileRule(project));
            entries.AddRange(CheckAssemblyReferenceRule(project));
            entries.AddRange(CheckNamingConventionsRule(project));
            if (!string.IsNullOrEmpty(this.solutions.searchPath))
            {
                entries.AddRange(OrphanedProjectsRule(project));
            }
            if (this.strongNaming.strongNameRequired)
            {
                entries.AddRange(CheckStrongNamingRule(project));
            }

            return entries;
        }

        #region CheckStrongNamingRule

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public IList<LogEntry> CheckStrongNamingRule(BuildFile project)
        {
            List<LogEntry> entries = new List<LogEntry>();

            if (!this.strongNaming.strongNameRequired)
            {
                // Signing should not be enabled.
                if (project.SignAssembly)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "Signing is enabled for the project but it should be disabled.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured to be strong named using the key \"{0}\", but strong naming should not be enabled.", project.AssemblyOriginatorKeyFile);
                    entries.Add(new LogEntry(this.name, "SigningShouldBeDisabled", LogLevel.Error, message, detail));
                }
            }
            else
            {
                // Signing should be enabled.
                if (!project.SignAssembly || string.IsNullOrEmpty(project.AssemblyOriginatorKeyFile))
                {
                    if (!this.strongNaming.ignoreUnsignedProjects)
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, "Signing is disabled for the project but it should be enabled.");
                        string detail = string.Format(CultureInfo.CurrentCulture, "The project is not configured to be strong named but should be signed with the key file \"{0}\"", this.strongNaming.keyPath);
                        entries.Add(new LogEntry(this.name, "SigningShouldBeEnabled", LogLevel.Error, message, detail));
                    }
                }
                else if (!project.AssemblyOriginatorKeyFile.Equals(this.strongNaming.keyPath, StringComparison.OrdinalIgnoreCase))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "Signing is enabled but an incorrect key is used: the key file \"{0}\" is used instead of the expected key file \"{1}\".", project.AssemblyOriginatorKeyFile, this.strongNaming.keyPath);
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured to be strong named using the key file \"{0}\", but it should use the key file \"{1}\".", project.AssemblyOriginatorKeyFile, this.strongNaming.keyPath);
                    entries.Add(new LogEntry(this.name, "SignedWithIncorrectKey", LogLevel.Error, message, detail));
                }
            }

            return entries;
        }

        #endregion StrongNamingRule
 
        #region CheckOrphanedProjectsRule

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public IList<LogEntry> OrphanedProjectsRule(BuildFile project)
        {
            List<LogEntry> entries = new List<LogEntry>();

            bool projectFound = false;
            string[] solutionFiles = Directory.GetFiles(this.solutions.searchPath, "*.sln", SearchOption.AllDirectories);
            foreach (string solutionFile in solutionFiles)
            {
                string solutionFileContents = File.ReadAllText(solutionFile);
                foreach (Match solutionProjectMatch in SolutionProjectExpression.Matches(solutionFileContents))
                {
                    string solutionProjectFileName = solutionProjectMatch.Groups["ProjectFileName"].Value;
                    if (!Path.IsPathRooted(solutionProjectFileName))
                    {
                        // The project file name is a relative path, resolve it against the solution path.
                        solutionProjectFileName = Path.Combine(Path.GetDirectoryName(solutionFile), solutionProjectFileName);
                        solutionProjectFileName = Path.GetFullPath(solutionProjectFileName);
                    }
                    if (string.Equals(solutionProjectFileName, project.Path, StringComparison.OrdinalIgnoreCase))
                    {
                        projectFound = true;
                    }
                }
            }

            if (!projectFound)
            {
                string message = string.Format(CultureInfo.CurrentCulture, "The project is not found in any solution.");
                string detail = string.Format(CultureInfo.CurrentCulture, "The project is not part of any solution file in the search path {0}.", this.solutions.searchPath);
                entries.Add(new LogEntry(this.name, "OrphanedProject", LogLevel.Error, message, detail));
            }

            return entries;
        }

        #endregion OrphanedProjectsRule
 
        #region CheckBuildProperties

        private IList<LogEntry> CheckBuildProperties(BuildFile project)
        {

            List<LogEntry> entries = new List<LogEntry>();

            foreach (ruleElementBuildProperty expectedProperty in this.buildProperties)
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

            return entries;
        }

        private static string GetConditionSubstring(ruleElementBuildProperty expectedProperty)
        {
            return (string.IsNullOrEmpty(expectedProperty.condition) ? "" : string.Format(CultureInfo.CurrentCulture, " with condition \"{0}\"", expectedProperty.condition));
        }

        #endregion CheckBuildProperties

        #region CheckDocumentationFileRule

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public IList<LogEntry> CheckDocumentationFileRule(BuildFile project)
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
                                entries.Add(new LogEntry(this.name, "IncorrectFileName", LogLevel.Error, message, detail));
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
            LogEntry entry = new LogEntry(this.name, "MissingDocumentationFile", LogLevel.Error, message, detail);
            return entry;
        }

        #endregion CheckDocumentationFileRule

        #region CheckAssemblyReferenceRule

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public IList<LogEntry> CheckAssemblyReferenceRule(BuildFile project)
        {
            List<LogEntry> entries = new List<LogEntry>();

            foreach (AssemblyReference reference in project.AssemblyReferences)
            {
                bool foundReference = false;
                foreach (ruleElementAssemblyLocation expectedLocation in this.assemblyLocations)
                {
                    if (!string.IsNullOrEmpty(expectedLocation.assemblyName))
                    {
                        if (reference.AssemblyName.StartsWith(expectedLocation.assemblyName, StringComparison.OrdinalIgnoreCase))
                        {
                            foundReference = true;
                            if (!string.IsNullOrEmpty(expectedLocation.assemblyPath) && !reference.HintPath.StartsWith(expectedLocation.assemblyPath, StringComparison.OrdinalIgnoreCase))
                            {
                                string message = string.Format(CultureInfo.CurrentCulture, "Invalid assembly reference.");
                                string detail = string.Format(CultureInfo.CurrentCulture, "The current hint path \"{0}\" for the assembly reference \"{1}\" is incorrect. The hint path should point to \"{2}\".", reference.HintPath, reference.AssemblyName, expectedLocation.assemblyPath);
                                entries.Add(new LogEntry(this.name, "IncorrectHintPath", LogLevel.Error, message, detail));
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
                    entries.Add(new LogEntry(this.name, "MissingAssemblyLocation", LogLevel.Warning, message, detail));
                }
            }

            return entries;
        }

        #endregion CheckAssemblyReferenceRule

        #region CheckNamingConventionsRule

        /// <summary>
        /// Checks the current rule on the given build file.
        /// </summary>
        /// <param name="project">The build file to verify.</param>
        /// <returns>The log entries for the specified build file.</returns>
        public IList<LogEntry> CheckNamingConventionsRule(BuildFile project)
        {
            List<LogEntry> entries = new List<LogEntry>();

            if (!string.IsNullOrEmpty(this.prefixes.namespacePrefix))
            {
                if (!project.RootNamespace.StartsWith(this.prefixes.namespacePrefix, StringComparison.Ordinal))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "The project has a wrong root namespace.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured with the root namespace \"{0}\", but the root namespace should start with \"{1}\".", project.RootNamespace, this.prefixes.namespacePrefix);
                    entries.Add(new LogEntry(this.name, "IncorrectRootNamespace", LogLevel.Error, message, detail));
                }
            }

            if (!string.IsNullOrEmpty(this.prefixes.assemblyNamePrefix))
            {
                if (!project.AssemblyName.StartsWith(this.prefixes.assemblyNamePrefix, StringComparison.Ordinal))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "The project has a wrong assembly name.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured with the assembly name \"{0}\", but the assembly name should start with \"{1}\".", project.AssemblyName, this.prefixes.assemblyNamePrefix);
                    entries.Add(new LogEntry(this.name, "IncorrectAssemblyName", LogLevel.Error, message, detail));
                }
            }

            if (this.prefixes.assemblyNameShouldMatchRootNamespace)
            {
                if (!string.Equals(project.AssemblyName, project.RootNamespace, StringComparison.Ordinal))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "The assembly name does not match the root namespace.");
                    string detail = string.Format(CultureInfo.CurrentCulture, "The project is configured with the assembly name \"{0}\", but it should be the same as the root namespace \"{1}\".", project.AssemblyName, project.RootNamespace);
                    entries.Add(new LogEntry(this.name, "AssemblyNameRootNamespaceMismatch", LogLevel.Error, message, detail));
                }
            }

            return entries;
        }

        #endregion NamingConventionsRule
   }
}