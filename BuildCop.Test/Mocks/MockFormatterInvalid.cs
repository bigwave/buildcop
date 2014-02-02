using System;
using System.Collections.Generic;
using System.Text;

using BuildCop.Formatters;
using BuildCop.Reporting;
using BuildCop.Configuration;

namespace BuildCop.Test.Mocks
{
    internal class MockFormatterInvalid : BaseFormatter
    {
        public MockFormatterInvalid(formatterElement config)
            : base(null)
        {

        }
        public override void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
        }
    }
}