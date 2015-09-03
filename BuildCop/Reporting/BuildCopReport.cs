using System;
using System.Collections.Generic;

namespace BuildCop.Reporting
{
    /// <summary>
    /// A report containing the outcome of a verification for a list of build groups.
    /// </summary>
    public class BuildCopReport
    {
        #region Properties

        private readonly IList<BuildGroupReport> buildGroupReports;

        /// <summary>
        /// Gets the build group reports.
        /// </summary>
        public IList<BuildGroupReport> BuildGroupReports
        {
            get { return buildGroupReports; }
        }

        private readonly DateTime generatedTime;

        /// <summary>
        /// Gets the time when this report was generated.
        /// </summary>
        public DateTime GeneratedTime
        {
            get { return generatedTime; }
        }

        private readonly string engineVersion;

        /// <summary>
        /// Gets the version number of the BuildCop engine that was used to produce this report.
        /// </summary>
        public string EngineVersion
        {
            get { return engineVersion; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildCopReport"/> class.
        /// </summary>
        /// <param name="buildGroupReports">The build group reports.</param>
        internal BuildCopReport(IList<BuildGroupReport> buildGroupReports)
        {
            this.buildGroupReports = buildGroupReports;
            generatedTime = DateTime.Now;
            engineVersion = typeof(BuildCopEngine).Assembly.GetName().Version.ToString();
        }

        #endregion
    }
}