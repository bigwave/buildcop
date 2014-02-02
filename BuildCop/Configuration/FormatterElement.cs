using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using SysConsole = System.Console;

using BuildCop.Formatters;
using BuildCop.Reporting;
using System.Xml.Xsl;
using System.Diagnostics;
using System.Globalization;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a formatter for a BuildCop report. 
    /// </summary>
    public partial class formatterElement : BuildCopBaseElement
    {

        #region Properties

        /// <summary>
        /// Gets the full path to the default XSLT stylesheet.
        /// </summary>
        public static string DefaultStylesheet
        {
            get
            {
                string applicationDirectory = Path.GetDirectoryName(typeof(BuildCopEngine).Assembly.CodeBase);
                string stylesheet = Path.Combine(applicationDirectory, "BuildCopReport.xslt");
                return stylesheet;
            }
        }

        #endregion

        #region WriteXmlReport

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        public void WriteXmlReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            string fileName = this.output.fileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("The XML formatter did not have an output file name specified in its configuration.");
            }

            using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                string stylesheet = this.output.stylesheet;
                if (string.IsNullOrEmpty(stylesheet))
                {
                    // Use the default stylesheet if no stylesheet was provided.
                    stylesheet = DefaultStylesheet;
                }
            }
            if (this.output.launch)
            {
                Process.Start(fileName);
            }

        }

        /// <summary>
        /// Writes the specified BuildCop report as XML to an output stream.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        /// <param name="outputStream">The output stream to which to write the XML.</param>
        /// <param name="stylesheet">The XSLT stylesheet to apply, an empty string to not use a stylesheet, or <see langword="null"/> to use the default stylesheet.</param>
        public static void WriteXmlReport(BuildCopReport report, LogLevel minimumLogLevel, Stream outputStream, string stylesheet)
        {
            if (stylesheet == null)
            {
                stylesheet = DefaultStylesheet;
            }
            using (XmlWriter writer = XmlWriter.Create(outputStream))
            {
                writer.WriteStartDocument();
                if (!string.IsNullOrEmpty(stylesheet))
                {
                    writer.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"" + stylesheet + "\"");
                }

                writer.WriteStartElement("BuildCopReport");
                writer.WriteAttributeString("engineVersion", report.EngineVersion);
                writer.WriteAttributeString("generated", report.GeneratedTime.ToString("r", CultureInfo.InvariantCulture));
                writer.WriteAttributeString("minimumLogLevel", minimumLogLevel.ToString());

                foreach (BuildGroupReport groupReport in report.BuildGroupReports)
                {
                    writer.WriteStartElement("BuildGroup");
                    writer.WriteAttributeString("name", groupReport.BuildGroupName);

                    foreach (BuildFileReport fileReport in groupReport.BuildFileReports)
                    {
                        IList<LogEntry> logEntries = fileReport.FindLogEntries(minimumLogLevel);
                        writer.WriteStartElement("BuildFile");
                        writer.WriteAttributeString("name", Path.GetFileName(fileReport.FileName));
                        writer.WriteAttributeString("path", fileReport.FileName);

                        foreach (LogEntry entry in logEntries)
                        {
                            writer.WriteStartElement("Entry");
                            writer.WriteAttributeString("level", entry.Level.ToString());
                            writer.WriteAttributeString("rule", entry.Rule);
                            writer.WriteAttributeString("code", entry.Code);
                            writer.WriteStartElement("Message");
                            writer.WriteString(entry.Message);
                            writer.WriteEndElement();
                            writer.WriteStartElement("Detail");
                            writer.WriteString(entry.Detail);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        #endregion

        #region WriteHtmlReport

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        public void WriteHtmlReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            string fileName = this.output.fileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidOperationException("The HTML formatter did not have an output file name specified in its configuration.");
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                // Use the XML formatter to create an XML document in memory.
                WriteXmlReport(report, minimumLogLevel, memStream, null);
                memStream.Flush();
                memStream.Position = 0;

                // Use the XSLT to transform the XML into HTML.
                XslCompiledTransform transform = new XslCompiledTransform();
                string stylesheet = this.output.stylesheet;
                if (string.IsNullOrEmpty(stylesheet))
                {
                    stylesheet = DefaultStylesheet;
                }
                transform.Load(stylesheet);
                XmlReader input = XmlReader.Create(memStream);
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    transform.Transform(input, XmlWriter.Create(outputStream));
                }
            }
            if (this.output.launch)
            {
                Process.Start(fileName);
            }

        }

        #endregion
 
        #region WriteCsvReport

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        public void WriteCsvReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            string fileName = this.output.fileName;
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
            for (int i = 0; i < fields.Length; i++)
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
 
        #region WriteConsoleReport

        private static readonly string Separator = new string('-', 79);

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        public void WriteConsoleReport(BuildCopReport report, LogLevel minimumLogLevel)
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