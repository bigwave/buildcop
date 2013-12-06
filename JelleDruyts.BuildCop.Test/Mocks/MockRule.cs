using System;
using System.Collections.Generic;
using System.Text;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules;

namespace JelleDruyts.BuildCop.Test.Mocks
{
    [BuildCopRule]
    internal class MockRule : BaseRule
    {
        public MockRule()
            : base(null)
        {
        }

        public MockRule(RuleConfigurationElement configuration)
            : base(configuration)
        {
        }

        public override IList<LogEntry> Check(BuildFile project)
        {
            return new LogEntry[] { new LogEntry(this.Name, "Mock", LogLevel.Information, "Checked by mock", "This is the detailed message that contains semicolons (;), commas (,) and new lines." + Environment.NewLine + "Which is nice; isn't it, really?") };
        }

        // Make it public on this mock type.
        public new TConfigurationType GetTypedConfiguration<TConfigurationType>() where TConfigurationType : RuleConfigurationElement
        {
            return base.GetTypedConfiguration<TConfigurationType>();
        }
    }
}