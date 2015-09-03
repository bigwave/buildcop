
using BuildCop.Reporting;
using BuildCop.Configuration;

namespace BuildCop.Formatters
{
    /// <summary>
    /// A base class for the different types of formatters for verification reports.
    /// </summary>
    public abstract class BaseFormatter
    {
        #region Properties

        private readonly formatterElement configuration;

        /// <summary>
        /// Gets or sets the configuration for this rule.
        /// </summary>
        public formatterElement Configuration
        {
            get { return configuration; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFormatter"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this formatter.</param>
        protected BaseFormatter(formatterElement configuration)
        {
            this.configuration = configuration;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        public abstract void WriteReport(BuildCopReport report, LogLevel minimumLogLevel);

        #endregion

    }
}