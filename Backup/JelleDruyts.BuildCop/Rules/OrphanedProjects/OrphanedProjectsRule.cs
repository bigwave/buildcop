using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules.OrphanedProjects.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace JelleDruyts.BuildCop.Rules.OrphanedProjects
{
    /// <summary>
    /// A rule that checks if a project is orphaned, i.e. not a part of any solution.
    /// </summary>
    [BuildCopRule(ConfigurationType=typeof(OrphanedProjectsRuleElement))]
    public class OrphanedProjectsRule : BaseRule
    {
        #region Fields
        
        private static Regex SolutionProjectExpression = new Regex("Project\\(\"(?<TypeGuid>.*?)\"\\)\\s*=\\s*\"(?<ProjectName>.*?)\",\\s*\"(?<ProjectFileName>.*?)\",\\s*\"(?<ProjectGuid>.*?)\"", RegexOptions.Multiline | RegexOptions.Compiled);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrphanedProjectsRule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this rule.</param>
        public OrphanedProjectsRule(RuleConfigurationElement configuration)
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
            OrphanedProjectsRuleElement config = this.GetTypedConfiguration<OrphanedProjectsRuleElement>();
            List<LogEntry> entries = new List<LogEntry>();

            bool projectFound = false;
            string[] solutionFiles = Directory.GetFiles(config.Solutions.SearchPath, "*.sln", SearchOption.AllDirectories);
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
                string detail = string.Format(CultureInfo.CurrentCulture, "The project is not part of any solution file in the search path {0}.", config.Solutions.SearchPath);
                entries.Add(new LogEntry(this.Name, "OrphanedProject", LogLevel.Error, message, detail));
            }

            return entries;
        }

        #endregion
    }
}