using System;
using System.Collections.Generic;
using System.Text;

using JelleDruyts.BuildCop.Formatters;
using JelleDruyts.BuildCop.Reporting;

namespace JelleDruyts.BuildCop.Test.Mocks
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