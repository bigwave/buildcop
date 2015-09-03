using System.Collections.Generic;

namespace BuildCop.Reporting
{
    /// <summary>
    /// A report containing the outcome of a verification for a build group.
    /// </summary>
    public class BuildGroupReport
    {
        #region Properties

        private readonly string buildGroupName;

        /// <summary>
        /// Gets the name of the build group.
        /// </summary>
        public string BuildGroupName
        {
            get { return buildGroupName; }
        }

        private readonly IList<BuildFileReport> buildFileReports;

        /// <summary>
        /// Gets the build file reports for this build group.
        /// </summary>
        public IList<BuildFileReport> BuildFileReports
        {
            get { return buildFileReports; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildGroupReport"/> class.
        /// </summary>
        /// <param name="buildGroupName">The name of the build group.</param>
        /// <param name="buildFileReports">The reports for this build group.</param>
        internal BuildGroupReport(string buildGroupName, IList<BuildFileReport> buildFileReports)
        {
            this.buildGroupName = buildGroupName;
            this.buildFileReports = buildFileReports;
        }

        #endregion
    }
}