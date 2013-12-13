using System;
using System.Collections.Generic;
using System.Text;

using BuildCop.Formatters;
using BuildCop.Reporting;

namespace BuildCop.Test.Mocks
{
    internal class MockFormatterInvalid : BaseFormatter
    {
        public MockFormatterInvalid(string dummy)
            : base(null)
        {
        }

        public override void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
        }
    }
}