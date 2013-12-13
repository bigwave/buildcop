using System;
using System.Collections.Generic;
using System.Text;

using BuildCop.Configuration;
using BuildCop.Reporting;
using BuildCop.Rules;

namespace BuildCop.Test.Mocks
{
    internal class MockRuleInvalid : BaseRule
    {
        public MockRuleInvalid()
            : base(null)
        {
        }

        public override IList<LogEntry> Check(BuildFile project)
        {
            return new LogEntry[] { new LogEntry(this.Name, "Mock", LogLevel.Information, "Checked by mock", "Detailed message") };
        }
    }
}