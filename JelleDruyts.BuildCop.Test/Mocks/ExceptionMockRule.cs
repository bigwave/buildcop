using System;
using System.Collections.Generic;
using System.Text;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules;

namespace JelleDruyts.BuildCop.Test.Mocks
{
    [BuildCopRule]
    public class ExceptionMockRule : BaseRule
    {
        public ExceptionMockRule()
            : base(null)
        {
        }

        public ExceptionMockRule(RuleConfigurationElement configuration)
            : base(configuration)
        {
        }

        public override IList<LogEntry> Check(BuildFile project)
        {
            throw new InvalidOperationException("ExceptionMock was configured to throw exceptions.");
        }
    }
}