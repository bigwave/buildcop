using System;
using System.Collections.Generic;
using System.Text;
using BuildCop.Configuration;
using System.Xml;

namespace BuildCop.Test.Mocks
{
    public class MockRuleElement : RuleConfigurationElement
    {
        public MockRuleElement()
        {
        }

        public MockRuleElement(XmlReader reader)
            : base(reader)
        {
        }
    }
}