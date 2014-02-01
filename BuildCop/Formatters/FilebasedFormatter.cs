using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Formatters
{
    /// <summary>
    /// A base class for a file-based formatter.
    /// </summary>
    public abstract class FilebasedFormatter : formatterElement
    {
        #region WriteReport

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public sealed override void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            WriteReportCore(report, minimumLogLevel);

            if (this.output.launch)
            {
                string fileName = this.output.fileName;
                Process.Start(fileName);
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        /// <remarks>
        /// Override this method to write the report. The <see cref="FilebasedFormatter"/> base
        /// class will ensure that the file is launched after this method is called, depending on
        /// the output.launch configuration setting.
        /// </remarks>
        protected abstract void WriteReportCore(BuildCopReport report, LogLevel minimumLogLevel);

        #endregion
    }
}