using System;
using System.Collections.Generic;
using System.Text;

using BuildCop.Formatters;
using BuildCop.Reporting;
using BuildCop.Configuration;

namespace BuildCop.Test.Mocks
{
    internal class MockFormatter : BaseFormatter
    {
        public MockFormatter(formatterElement configuration)
            : base(configuration)
        {
        }

        public static BuildCopReport LastReport;

        public override void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            LastReport = report;
        }
    }
}