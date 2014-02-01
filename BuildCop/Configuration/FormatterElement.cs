using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using BuildCop.Formatters;
using BuildCop.Reporting;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a formatter for a BuildCop report. 
    /// </summary>
    public partial class formatterElement : BuildCopBaseElement
    {
        #region Abstract Methods

        /// <summary>
        /// Writes the specified BuildCop report.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="minimumLogLevel">The minimum log level to write.</param>
        public virtual void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            throw new InvalidOperationException("Oops, how did we get here?");
        }

        #endregion

 
    }
}