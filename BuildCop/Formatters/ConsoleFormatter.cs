using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SysConsole = System.Console;

using BuildCop.Reporting;
using BuildCop.Configuration;

namespace BuildCop.Formatters
{
    /// <summary>
    /// A verification report formatter that writes to the console.
    /// </summary>
    [BuildCopFormatter]
    public class ConsoleFormatter : BaseFormatter
    {
        #region Constants

        private static readonly string Separator = new string('-', 79);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleFormatter"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this formatter.</param>
        public ConsoleFormatter(formatterElement configuration)
            : base(configuration)
        {
        }

        #endregion

        #region WriteReport

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        public override void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            foreach (BuildGroupReport groupReport in report.BuildGroupReports)
            {
                SysConsole.WriteLine("Build Group '{0}' - Verification Report", groupReport.BuildGroupName);
                SysConsole.WriteLine("Minimum Log Level: " + minimumLogLevel.ToString());

                int totalEntries = 0;
                int totalShownEntries = 0;
                foreach (BuildFileReport fileReport in groupReport.BuildFileReports)
                {
                    IList<LogEntry> logEntries = fileReport.FindLogEntries(minimumLogLevel);
                    if (logEntries.Count > 0)
                    {
                        SysConsole.WriteLine(Separator);
                        SysConsole.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0}:", Path.GetFileName(fileReport.FileName)));

                        totalEntries += logEntries.Count;
                        int shownEntries = 0;
                        foreach (LogEntry entry in logEntries)
                        {
                            if ((int)entry.Level >= (int)minimumLogLevel)
                            {
                                totalShownEntries++;
                                shownEntries++;
                                SysConsole.WriteLine();
                                SysConsole.WriteLine("[#{0}] {1}: {2}", shownEntries, entry.Level.ToString(), entry.Message);
                                if (!string.IsNullOrEmpty(entry.Detail))
                                {
                                    SysConsole.WriteLine("Details: {0}", entry.Detail);
                                }
                            }
                        }
                        SysConsole.WriteLine();
                        SysConsole.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0} of {1} log entries shown for project file.", shownEntries, fileReport.LogEntries.Count));
                    }
                }

                SysConsole.WriteLine();
                SysConsole.WriteLine(Separator);
                SysConsole.WriteLine(string.Format(CultureInfo.CurrentCulture, "Total: {0} of {1} log entries shown in {2} project file(s).", totalEntries, totalShownEntries, groupReport.BuildFileReports.Count));
                SysConsole.WriteLine(Separator);
            }
        }

        #endregion
    }
}