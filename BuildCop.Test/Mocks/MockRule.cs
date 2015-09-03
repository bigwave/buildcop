using System;
using System.Collections.Generic;

using BuildCop.Configuration;
using BuildCop.Reporting;
using BuildCop.Rules;

namespace BuildCop.Test.Mocks
{
    [BuildCopRule]
    internal class MockRule : BaseRule
    {
        public MockRule()
            : base(null)
        {
        }

        public MockRule(ruleElement configuration)
            : base(configuration)
        {
        }

        public override IList<LogEntry> Check(BuildFile project)
        {
            return new LogEntry[] { new LogEntry(Name, "Mock", LogLevel.Information, "Checked by mock", "This is the detailed message that contains semicolons (;), commas (,) and new lines." + Environment.NewLine + "Which is nice; isn't it, really?") };
        }

    }
}