using System;
using System.Collections.Generic;
using System.Text;

namespace BuildCop.Reporting
{
    /// <summary>
    /// An entry in a project verification report.
    /// </summary>
    public class LogEntry
    {
        #region Properties

        private readonly string rule;

        /// <summary>
        /// Gets the code that identifies the rule from which this log entry was created.
        /// </summary>
        public string Rule
        {
            get { return this.rule; }
        }

        private readonly string code;

        /// <summary>
        /// Gets the code that uniquely identifies the type of log entry.
        /// </summary>
        public string Code
        {
            get { return this.code; }
        }

        private readonly LogLevel level;

        /// <summary>
        /// Gets the violation level of the entry.
        /// </summary>
        public LogLevel Level
        {
            get { return this.level; }
        }

        private readonly string message;

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }

        private readonly string detail;

        /// <summary>
        /// Gets the message detail.
        /// </summary>
        public string Detail
        {
            get { return this.detail; }
        }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="rule">The code that identifies the rule from which this log entry was created.</param>
        /// <param name="code">The code that uniquely identifies the type of log entry.</param>
        /// <param name="level">The violation level of the log entry.</param>
        /// <param name="message">The message.</param>
        /// <param name="detail">The message detail.</param>
        public LogEntry(string rule, string code, LogLevel level, string message, string detail)
        {
            this.rule = rule;
            this.code = code;
            this.message = message;
            this.level = level;
            this.detail = detail;
        }

        #endregion
    }
}