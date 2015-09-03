using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

using BuildCop.Configuration;
using BuildCop.Reporting;

namespace BuildCop.Formatters
{
    /// <summary>
    /// A verification report formatter that writes HTML output.
    /// </summary>
    [BuildCopFormatter(ConfigurationType = typeof(formatterElement))]
    public class HtmlFormatter : XsltFilebasedFormatter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlFormatter"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this formatter.</param>
        public HtmlFormatter(formatterElement configuration)
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
                throw new InvalidOperationException("The HTML formatter did not have an output file name specified in its configuration.");
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                // Use the XML formatter to create an XML document in memory.
                XmlFormatter.WriteReport(report, minimumLogLevel, memStream, null);
                memStream.Flush();
                memStream.Position = 0;

                // Use the XSLT to transform the XML into HTML.
                XslCompiledTransform transform = new XslCompiledTransform();
                string stylesheet = Configuration.output.stylesheet;
                if (string.IsNullOrEmpty(stylesheet))
                {
                    stylesheet = XmlFormatter.DefaultStylesheet;
                }
                transform.Load(stylesheet);
                XmlReader input = XmlReader.Create(memStream);
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    transform.Transform(input, XmlWriter.Create(outputStream));
                }
            }
        }

        #endregion
    }
}