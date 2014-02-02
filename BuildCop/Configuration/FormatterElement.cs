using System.IO;

using BuildCop.Formatters;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a formatter for a BuildCop report. 
    /// </summary>
    public partial class formatterElement 
    {
        private BaseFormatter myFormatter;

        internal BaseFormatter Formatter
        {
            get
            {
                if (myFormatter != null)
                {
                    return myFormatter;

                }

                if (this.name == null)
                {
                    myFormatter = new ConsoleFormatter(this);
                }

                switch (this.name.ToUpperInvariant())
                {
                    case "HTML":
                        myFormatter = new HtmlFormatter(this);
                        break;
                    case "CSV":
                        myFormatter = new CsvFormatter(this);
                        break;
                    case "XML":
                        myFormatter = new XmlFormatter(this);
                        break;
                    default:
                        myFormatter = new ConsoleFormatter(this);
                        break;
                }

                return myFormatter;
            }

            set
            {
                myFormatter = value;
            }
        }

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

    }
}