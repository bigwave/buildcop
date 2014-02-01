using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Formatters.Xml
{
    /// <summary>
    /// A verification report formatter that writes XML output.
    /// </summary>
    [BuildCopFormatter(ConfigurationType = typeof(formatterElement))]
    public class XmlFormatter : XsltFilebasedFormatter
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

        #region WriteReport

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
        protected override void WriteReportCore(BuildCopReport report, LogLevel minimumLogLevel)
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
                WriteReport(report, minimumLogLevel, outputStream, stylesheet);
            }
        }

        /// <summary>
        /// Writes the specified BuildCop report as XML to an output stream.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        /// <param name="outputStream">The output stream to which to write the XML.</param>
        /// <param name="stylesheet">The XSLT stylesheet to apply, an empty string to not use a stylesheet, or <see langword="null"/> to use the default stylesheet.</param>
        internal static void WriteReport(BuildCopReport report, LogLevel minimumLogLevel, Stream outputStream, string stylesheet)
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
    }
}