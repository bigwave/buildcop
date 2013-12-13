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
        public MockFormatter(FormatterConfigurationElement configuration)
            : base(configuration)
        {
        }

        public static BuildCopReport LastReport;

        public override void WriteReport(BuildCopReport report, LogLevel minimumLogLevel)
        {
            LastReport = report;
        }

        // Make it public on this mock type.
        public new TConfigurationType GetTypedConfiguration<TConfigurationType>() where TConfigurationType : FormatterConfigurationElement
        {
            return base.GetTypedConfiguration<TConfigurationType>();
        }
    }
}