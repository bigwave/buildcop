using BuildCop.Configuration;
using System.Collections.Generic;

namespace BuildCop.Reporting
{
    /// <summary>
    /// A report containing the outcome of a verification for a single build file.
    /// </summary>
    public class BuildFileReport
    {
        #region Properties

        private readonly string fileName;

        /// <summary>
        /// Gets the file name of the project file that was subject to verification.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        private readonly IList<LogEntry> logEntries;

        /// <summary>
        /// Gets the log entries.
        /// </summary>
        public IList<LogEntry> LogEntries
        {
            get { return logEntries; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildFileReport"/> class.
        /// </summary>
        /// <param name="fileName">The name of the project file that was subject to verification.</param>
        /// <param name="logEntries">The log entries.</param>
        internal BuildFileReport(string fileName, IList<LogEntry> logEntries)
        {
            this.fileName = fileName;
            this.logEntries = logEntries;
        }

        #endregion

        #region FindLogEntries

        /// <summary>
        /// Finds all log entries in this report with the specified minimum log level.
        /// </summary>
        /// <param name="minimumLogLevel">The minimum log level for which to return log entries.</param>
        /// <returns>All log entries in this report with the specified minimum log level.</returns>
        public IList<LogEntry> FindLogEntries(LogLevel minimumLogLevel)
        {
            IList<LogEntry> foundLogEntries = new List<LogEntry>();
            foreach (LogEntry entry in LogEntries)
            {
                if ((int)entry.Level >= (int)minimumLogLevel)
                {
                    foundLogEntries.Add(entry);
                }
            }
            return foundLogEntries;
        }

        #endregion
    }
}