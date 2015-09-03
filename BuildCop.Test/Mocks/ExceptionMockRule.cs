using System;
using System.Collections.Generic;

using BuildCop.Configuration;
using BuildCop.Reporting;
using BuildCop.Rules;

namespace BuildCop.Test.Mocks
{
    [BuildCopRule]
    public class ExceptionMockRule : BaseRule
    {
        public ExceptionMockRule()
            : base(null)
        {
        }

        public ExceptionMockRule(ruleElement configuration)
            : base(configuration)
        {
        }

        public override IList<LogEntry> Check(BuildFile project)
        {
            throw new InvalidOperationException("ExceptionMock was configured to throw exceptions.");
        }
    }
}