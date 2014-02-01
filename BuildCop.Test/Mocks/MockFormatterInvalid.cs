using System;
using System.Collections.Generic;
using System.Text;

using BuildCop.Formatters;
using BuildCop.Reporting;
using BuildCop.Configuration;

namespace BuildCop.Test.Mocks
{
    internal class MockFormatterInvalid : formatterElement
    {

        public override void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
        }
    }
}