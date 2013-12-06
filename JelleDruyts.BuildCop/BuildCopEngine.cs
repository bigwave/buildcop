using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Formatters;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules;

namespace JelleDruyts.BuildCop
{
    /// <summary>
    /// Provides the entry points for BuildCop analysis.
    /// </summary>
    public static class BuildCopEngine
    {
        #region Constants

        /// <summary>
        /// The separator character for file and output type exclusion strings.
        /// </summary>
        private const char ExclusionSeparator = ';';

        #endregion

        #region Execute

        /// <summary>
        /// Starts analysis using the calling application's configuration.
        /// </summary>
        /// <returns>The report containing the outcome of a verification for a list of build groups.</returns>
        public static BuildCopReport Execute()
        {
            return Execute(BuildCopConfiguration.Instance, null);
        }

        /// <summary>
        /// Starts analysis using the calling application's configuration.
        /// </summary>
        /// <param name="buildGroups">The build groups to verify.</param>
        /// <returns>The report containing the outcome of a verification for a list of build groups.</returns>
        public static BuildCopReport Execute(IList<string> buildGroups)
        {
            return Execute(BuildCopConfiguration.Instance, buildGroups);
        }

        /// <summary>
        /// Starts analysis using the specified configuration.
        /// </summary>
        /// <param name="configuration">The BuildCop configuration to use.</param>
        /// <returns>The report containing the outcome of a verification for a list of build groups.</returns>
        public static BuildCopReport Execute(BuildCopConfiguration configuration)
        {
            return Execute(configuration, null);
        }

        /// <summary>
        /// Starts analysis using the specified configuration.
        /// </summary>
        /// <param name="configuration">The BuildCop configuration to use.</param>
        /// <param name="buildGroups">The build groups to verify.</param>
        /// <returns>The report containing the outcome of a verification for a list of build groups.</returns>
        public static BuildCopReport Execute(BuildCopConfiguration configuration, IList<string> buildGroups)
        {
            IList<BuildGroupReport> groupReports = new List<BuildGroupReport>();

            IDictionary<string, string> outputTypeMappings = GetOutputTypeMappings(configuration);

            foreach (BuildGroupElement buildGroup in configuration.BuildGroups)
            {
                bool shouldVerifyBuildGroup = buildGroup.Enabled;
                if (buildGroup.Rules.Count == 0)
                {
                    shouldVerifyBuildGroup = false;
                }
                else if(buildGroups != null && !buildGroups.Contains(buildGroup.Name))
                {
                    shouldVerifyBuildGroup = false;
                }
                if (shouldVerifyBuildGroup)
                {
                    IList<BuildFileReport> fileReports = new List<BuildFileReport>();

                    // Determine build files.
                    IList<BuildFile> buildFiles = GetBuildFiles(buildGroup.BuildFiles);

                    // Determine rules.
                    IList<BaseRule> rules = new List<BaseRule>();
                    foreach (RuleElement ruleDefinition in buildGroup.Rules)
                    {
                        BaseRule rule = CreateRule(ruleDefinition, configuration.SharedRules);
                        rules.Add(rule);
                    }

                    // Run rules on all build files.
                    foreach (BuildFile buildFile in buildFiles)
                    {
                        List<LogEntry> allEntries = new List<LogEntry>();
                        try
                        {
                            buildFile.Parse();
                            foreach (BaseRule rule in rules)
                            {
                                if (!ShouldExcludeFile(buildFile.FileName, rule.ExcludedFiles))
                                {
                                    if (!ShouldExcludeOutputType(buildFile.OutputType, buildFile.ProjectTypeGuids, outputTypeMappings, rule.ExcludedOutputTypes))
                                    {
                                        IList<LogEntry> ruleEntries = rule.Check(buildFile);
                                        allEntries.AddRange(ruleEntries);
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            LogEntry entry = CreateExceptionLogEntry(exc);
                            allEntries.Add(entry);
                        }
                        BuildFileReport fileReport = new BuildFileReport(buildFile.FileName, allEntries);
                        fileReports.Add(fileReport);
                    }

                    groupReports.Add(new BuildGroupReport(buildGroup.Name, fileReports));
                }
            }

            BuildCopReport report = new BuildCopReport(groupReports);

            // Write reports to formatters.
            foreach (FormatterElement formatterDefinition in configuration.Formatters)
            {
                BaseFormatter formatter = CreateFormatter(formatterDefinition);
                formatter.WriteReport(report, formatterDefinition.MinimumLogLevel);
            }

            return report;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets all the build files to be included.
        /// </summary>
        /// <param name="buildFilesConfiguration">The build files configuration.</param>
        /// <returns>A list of all build files to be included in the analysis.</returns>
        private static IList<BuildFile> GetBuildFiles(BuildFilesElement buildFilesConfiguration)
        {
            List<BuildFile> buildFiles = new List<BuildFile>();

            foreach (BuildFilePathElement pathConfiguration in buildFilesConfiguration.Paths)
            {
                // Search the root path for build files.
                string rootPath = pathConfiguration.RootPath;
                string searchPattern = pathConfiguration.SearchPattern;
                string excludedFiles = pathConfiguration.ExcludedFiles;
                if (!string.IsNullOrEmpty(buildFilesConfiguration.ExcludedFiles))
                {
                    // Append the globally excluded files.
                    excludedFiles += ExclusionSeparator + buildFilesConfiguration.ExcludedFiles;
                }

                if (string.IsNullOrEmpty(rootPath))
                {
                    rootPath = AppDomain.CurrentDomain.BaseDirectory;
                }

                string[] excludedExtensions = new string[0];

                // If no search pattern is given, default to *.proj files and exclude certain extensions.
                if (string.IsNullOrEmpty(searchPattern))
                {
                    searchPattern = "*.*proj";

                    // Create a default array of excluded extensions.
                    excludedExtensions = new string[] { ".proj", ".vddproj", ".vdproj", ".csdproj" };
                }

                // Get all files recursively and add them if not excluded.
                string[] fileNames = Directory.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories);
                foreach (string fileName in fileNames)
                {
                    bool excluded = false;
                    string extension = Path.GetExtension(fileName);
                    foreach (string excludedExtension in excludedExtensions)
                    {
                        if (string.Equals(excludedExtension, extension, StringComparison.OrdinalIgnoreCase))
                        {
                            excluded = true;
                            break;
                        }
                    }
                    if (ShouldExcludeFile(fileName, excludedFiles))
                    {
                        excluded = true;
                    }
                    if (!excluded)
                    {
                        buildFiles.Add(new BuildFile(fileName, true));
                    }
                }
            }

            return buildFiles;
        }

        /// <summary>
        /// Creates a rule.
        /// </summary>
        /// <param name="ruleDefinition">The rule definition.</param>
        /// <param name="sharedRules">The rules that are shared between build groups.</param>
        /// <returns>The rule for the given definition.</returns>
        private static BaseRule CreateRule(RuleElement ruleDefinition, RuleCollection sharedRules)
        {
            string ruleTypeName = ruleDefinition.Type;
            RuleConfigurationElement ruleConfig = ruleDefinition.RuleConfiguration;
            string ruleName = ruleDefinition.Name;
            string excludedFiles = ruleDefinition.ExcludedFiles;
            string excludedOutputTypes = ruleDefinition.ExcludedOutputTypes;

            // Look for matching shared rules.
            foreach (RuleElement sharedRuleDefinition in sharedRules)
            {
                if (string.Equals(ruleDefinition.Name, sharedRuleDefinition.Name, StringComparison.OrdinalIgnoreCase))
                {
                    // A shared rule with the same name was found, use that rule's definition.
                    if (!string.IsNullOrEmpty(ruleDefinition.Type) || ruleDefinition.RuleConfiguration != null)
                    {
                        throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, "The rule \"{0}\" has a Type and/or configuration element defined but is also defined as a shared rule. A reference to a shared rule can only include the rule's Name and optionally ExcludedFiles and ExcludedOutputTypes.", ruleDefinition.Name));
                    }

                    ruleTypeName = sharedRuleDefinition.Type;
                    ruleConfig = sharedRuleDefinition.RuleConfiguration;
                    ruleName = sharedRuleDefinition.Name;

                    // Merge the excluded files and output types.
                    if (!string.IsNullOrEmpty(excludedFiles))
                    {
                        excludedFiles += ExclusionSeparator;
                    }
                    excludedFiles += sharedRuleDefinition.ExcludedFiles;

                    if (!string.IsNullOrEmpty(excludedOutputTypes))
                    {
                        excludedOutputTypes += ExclusionSeparator;
                    }
                    excludedOutputTypes += sharedRuleDefinition.ExcludedOutputTypes;
                    break;
                }
            }

            Type ruleType = Type.GetType(ruleTypeName, true, true);
            if (!typeof(BaseRule).IsAssignableFrom(ruleType))
            {
                throw new ConfigurationErrorsException("The rule type must derive from the BaseRule class. Type name: " + ruleTypeName);
            }
            ConstructorInfo ctor = ruleType.GetConstructor(new Type[] { typeof(RuleConfigurationElement) });
            if (ctor == null)
            {
                throw new ConfigurationErrorsException("The rule type must have a constructor that takes a RuleConfigurationElement. Type name: " + ruleType.FullName);
            }
            BaseRule rule = (BaseRule)ctor.Invoke(new object[] { ruleConfig });
            rule.Name = ruleName;
            rule.ExcludedFiles = excludedFiles;
            rule.ExcludedOutputTypes = excludedOutputTypes;
            return rule;
        }

        /// <summary>
        /// Creates a formatter.
        /// </summary>
        /// <param name="formatterDefinition">The formatter definition.</param>
        /// <returns>The formatter for the given definition.</returns>
        private static BaseFormatter CreateFormatter(FormatterElement formatterDefinition)
        {
            Type formatterType = Type.GetType(formatterDefinition.Type, true, true);
            if (!typeof(BaseFormatter).IsAssignableFrom(formatterType))
            {
                throw new ConfigurationErrorsException("The formatter type must derive from the BaseFormatter class. Type name: " + formatterDefinition.Type);
            }

            ConstructorInfo ctor = formatterType.GetConstructor(new Type[] { typeof(FormatterConfigurationElement) });
            if (ctor == null)
            {
                throw new ConfigurationErrorsException("The formatter type must have a constructor that takes a FormatterConfigurationElement. Type name: " + formatterType.FullName);
            }
            BaseFormatter formatter = (BaseFormatter)ctor.Invoke(new object[] { formatterDefinition.FormatterConfiguration });
            return formatter;
        }

        /// <summary>
        /// Creates an exception log entry.
        /// </summary>
        /// <param name="exception">The exception for which to create a log entry.</param>
        /// <returns>A log entry representing the given exception.</returns>
        private static LogEntry CreateExceptionLogEntry(Exception exception)
        {
            string message = string.Format(CultureInfo.CurrentCulture, "An exception occurred while analysing the build file.");
            string detail = string.Format(CultureInfo.CurrentCulture, "{0}: {1}{2}{3}", exception.GetType().Name, exception.Message, Environment.NewLine, exception.StackTrace);
            LogEntry entry = new LogEntry("BuildCop", "Exception", LogLevel.Exception, message, detail);
            return entry;
        }

        /// <summary>
        /// Determines if a file should be excluded based on a file name.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="excludedFiles">The semicolon-separated list of strings to find in the names of files to exclude.</param>
        /// <returns><see langword="true"/> if the file should be excluded, <see langword="false"/> otherwise.</returns>
        private static bool ShouldExcludeFile(string fileName, string excludedFiles)
        {
            bool excluded = false;

            // Create an array of excluded files.
            string[] excludedFilesList = new string[0];
            if (!string.IsNullOrEmpty(excludedFiles))
            {
                excludedFilesList = excludedFiles.Split(ExclusionSeparator);
            }

            foreach (string excludedFile in excludedFilesList)
            {
                if (fileName.IndexOf(excludedFile, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    excluded = true;
                    break;
                }
            }
            return excluded;
        }

        /// <summary>
        /// Gets the output type mappings.
        /// </summary>
        /// <param name="configuration">The configuration from which to read additional mappings..</param>
        /// <returns>A dictionary with output type aliases as key and their project type GUID as value.</returns>
        private static IDictionary<string, string> GetOutputTypeMappings(BuildCopConfiguration configuration)
        {
            // Populate the output type mappings dictionary.
            IDictionary<string, string> outputTypeMappings = new Dictionary<string, string>();

            // Add the Web Application Projects alias by default.
            outputTypeMappings.Add("Web", "{349c5851-65df-11da-9384-00065b846f21}");

            // Add the mappings from the configuration (overwriting existing entries if needed).
            if (configuration.OutputTypeMappings != null)
            {
                foreach (OutputTypeElement outputType in configuration.OutputTypeMappings)
                {
                    outputTypeMappings[outputType.Alias] = outputType.ProjectTypeGuid;
                }
            }
            return outputTypeMappings;
        }

        /// <summary>
        /// Determines if a file should be excluded based on an output type.
        /// </summary>
        /// <param name="outputType">The output type of the file.</param>
        /// <param name="projectTypeGuids">The project type GUIDs of the file.</param>
        /// <param name="outputTypeMappings">The mappings of "fake" output type names to their project type GUIDs.</param>
        /// <param name="excludedOutputTypes">The semicolon-separated list of output types to exclude.</param>
        /// <returns><see langword="true"/> if the file should be excluded, <see langword="false"/> otherwise.</returns>
        private static bool ShouldExcludeOutputType(string outputType, string projectTypeGuids, IDictionary<string, string> outputTypeMappings, string excludedOutputTypes)
        {
            bool excluded = false;

            // Create an array of excluded output types.
            string[] excludedOutputTypesList = new string[0];
            if (!string.IsNullOrEmpty(excludedOutputTypes))
            {
                excludedOutputTypesList = excludedOutputTypes.Split(ExclusionSeparator);
            }

            foreach (string excludedOutputType in excludedOutputTypesList)
            {
                if (outputType.Equals(excludedOutputType, StringComparison.OrdinalIgnoreCase))
                {
                    excluded = true;
                    break;
                }

                if (outputTypeMappings.ContainsKey(excludedOutputType))
                {
                    // The excluded output type is actually a friendly name for a project type GUID.
                    // Check if that GUID is present in the build file.
                    if (projectTypeGuids != null && projectTypeGuids.Contains(outputTypeMappings[excludedOutputType]))
                    {
                        excluded = true;
                        break;
                    }
                }
            }

            return excluded;
        }

        #endregion
    }
}