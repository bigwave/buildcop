using System;
using System.Collections.Generic;
using System.Text;

namespace JelleDruyts.BuildCop.Reporting
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
            get { return this.buildGroupReports; }
        }

        private readonly DateTime generatedTime;

        /// <summary>
        /// Gets the time when this report was generated.
        /// </summary>
        public DateTime GeneratedTime
        {
            get { return this.generatedTime; }
        }

        private readonly string engineVersion;

        /// <summary>
        /// Gets the version number of the BuildCop engine that was used to produce this report.
        /// </summary>
        public string EngineVersion
        {
            get { return this.engineVersion; }
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
            this.generatedTime = DateTime.Now;
            this.engineVersion = typeof(BuildCopEngine).Assembly.GetName().Version.ToString();
        }

        #endregion
    }
}