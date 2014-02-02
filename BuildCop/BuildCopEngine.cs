using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

using BuildCop.Configuration;
using BuildCop.Formatters;
using BuildCop.Reporting;
using BuildCop.Rules;

namespace BuildCop
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
            BuildCopConfiguration theConfig = BuildCopConfiguration.LoadFromFile(@"BuildCop.config");
            return Execute(theConfig, null);
        }

        /// <summary>
        /// Starts analysis using the calling application's configuration.
        /// </summary>
        /// <param name="buildGroups">The build groups to verify.</param>
        /// <returns>The report containing the outcome of a verification for a list of build groups.</returns>
        public static BuildCopReport Execute(IList<string> buildGroups)
        {
            BuildCopConfiguration theConfig = BuildCopConfiguration.LoadFromFile(@"BuildCop.config");
            return Execute(theConfig, buildGroups);
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

            foreach (buildGroupElement buildGroup in configuration.buildGroups)
            {
                bool shouldVerifyBuildGroup = buildGroup.enabled;
                if (buildGroup.rules.Count == 0)
                {
                    shouldVerifyBuildGroup = false;
                }
                else if(buildGroups != null && !buildGroups.Contains(buildGroup.name))
                {
                    shouldVerifyBuildGroup = false;
                }
                if (shouldVerifyBuildGroup)
                {
                    IList<BuildFileReport> fileReports = new List<BuildFileReport>();

                    // Determine build files.
                    IList<BuildFile> buildFiles = GetBuildFiles(buildGroup.buildFiles);

                    // Determine rules.
                    IList<ruleElement> rules = new List<ruleElement>();
                    foreach (ruleElement ruleDefinition in buildGroup.rules)
                    {
                        ////BaseRule rule = CreateRule(ruleDefinition, configuration.sharedRules);
                        rules.Add(ruleDefinition);
                    }

                    // Run rules on all build files.
                    foreach (BuildFile buildFile in buildFiles)
                    {
                        List<LogEntry> allEntries = new List<LogEntry>();
                        try
                        {
                            buildFile.Parse();
                            foreach (ruleElement rule in rules)
                            {
                                if (!ShouldExcludeFile(buildFile.FileName, rule.excludedFiles))
                                {
                                    if (!ShouldExcludeOutputType(buildFile.OutputType, buildFile.ProjectTypeGuids, outputTypeMappings, rule.excludedOutputTypes))
                                    {
                                        if (string.IsNullOrEmpty(rule.type.Trim()))
                                        {
                                            // Shared rules entry
                                            foreach (var sharedRule in configuration.sharedRules)
                                            {
                                                if (sharedRule.name == rule.name)
                                                {
                                                    IList<LogEntry> ruleEntries = sharedRule.RuleChecker.Check(buildFile);
                                                    allEntries.AddRange(ruleEntries);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            IList<LogEntry> ruleEntries = rule.RuleChecker.Check(buildFile);
                                            allEntries.AddRange(ruleEntries);
                                        }
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

                    groupReports.Add(new BuildGroupReport(buildGroup.name, fileReports));
                }
            }

            BuildCopReport report = new BuildCopReport(groupReports);

            // Write reports to formatters.
            foreach (formatterElement formatterDefinition in configuration.formatters)
            {
                formatterDefinition.Formatter.WriteReport(report, formatterDefinition.minimumLogLevel);
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
        private static IList<BuildFile> GetBuildFiles(buildFilesElement buildFilesConfiguration)
        {
            List<BuildFile> buildFiles = new List<BuildFile>();

            foreach (buildFilePathElement pathConfiguration in buildFilesConfiguration.paths)
            {
                // Search the root path for build files.
                string rootPath = pathConfiguration.rootPath;
                string searchPattern = pathConfiguration.searchPattern;
                string excludedFiles = pathConfiguration.excludedFiles;
                if (!string.IsNullOrEmpty(buildFilesConfiguration.excludedFiles))
                {
                    // Append the globally excluded files.
                    excludedFiles += ExclusionSeparator + buildFilesConfiguration.excludedFiles;
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
            if (configuration.outputTypeMappings != null)
            {
                foreach (outputTypeElement outputType in configuration.outputTypeMappings)
                {
                    outputTypeMappings[outputType.alias] = outputType.projectTypeGuid;
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