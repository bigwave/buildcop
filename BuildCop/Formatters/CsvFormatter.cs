using System;
using System.IO;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Formatters
{
    /// <summary>
    /// A verification report formatter that writes XML output.
    /// </summary>
    [BuildCopFormatter(ConfigurationType = typeof(formatterElement))]
    public class CsvFormatter : FilebasedFormatter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvFormatter"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this formatter.</param>
        public CsvFormatter(formatterElement configuration)
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
        /// <remarks>
        /// Override this method to write the report. The <see cref="FilebasedFormatter"/> base
        /// class will ensure that the file is launched after this method is called, depending on
        /// the outputElement.launch configuration setting.
        /// </remarks>
        protected override void WriteReportCore(BuildCopReport report, LogLevel minimumLogLevel)
        {
            string fileName = Configuration.output.fileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("The CSV formatter did not have an output file name specified in its configuration.");
            }

            FileStream outputStream = null;
            try
            {
                outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using (StreamWriter writer = new StreamWriter(outputStream))
                {
                    outputStream = null;
                    CsvWriteLine(writer, "Build Group", "Build File", "Log Level", "Rule", "Code", "Message", "Detail");
                    foreach (BuildGroupReport groupReport in report.BuildGroupReports)
                    {
                        foreach (BuildFileReport fileReport in groupReport.BuildFileReports)
                        {
                            foreach (LogEntry entry in fileReport.FindLogEntries(minimumLogLevel))
                            {
                                CsvWriteLine(writer, groupReport.BuildGroupName, fileReport.FileName, entry.Level.ToString(), entry.Rule, entry.Code, entry.Message, entry.Detail);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (outputStream != null)
                {
                    outputStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Writes a CSV line to the given stream writer.
        /// </summary>
        /// <param name="writer">The stream writer to write to.</param>
        /// <param name="fields">The fields to write.</param>
        private static void CsvWriteLine(StreamWriter writer, params string[] fields)
        {
            for(int i = 0; i < fields.Length; i++)
            {
                if (i > 0)
                {
                    writer.Write(';');
                }
                writer.Write(CsvEscape(fields[i]));
            }
            writer.WriteLine();
        }

        /// <summary>
        /// Escapes a string for CSV output.
        /// </summary>
        /// <param name="input">The input string to escape.</param>
        /// <returns>The CSV-escaped output string.</returns>
        private static string CsvEscape(string input)
        {
            if (input.Contains(";") || input.Contains(Environment.NewLine))
            {
                input = input.Replace("\"", "\"\"");
                input = "\"" + input + "\"";
            }
            return input;
        }

        #endregion
    }
}