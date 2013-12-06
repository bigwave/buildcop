using System;
using System.Collections.Generic;
using System.Text;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules;

namespace JelleDruyts.BuildCop.Test.Mocks
{
    [BuildCopRule(ConfigurationType=typeof(string))]
    internal class MockRuleInvalidConfigurationDefinition : BaseRule
    {
        public MockRuleInvalidConfigurationDefinition()
            : base(null)
        {
        }

        public override IList<LogEntry> Check(BuildFile project)
        {
            return new LogEntry[] { new LogEntry(this.Name, "Mock", LogLevel.Information, "Checked by mock", "Detailed message") };
        }
    }
}