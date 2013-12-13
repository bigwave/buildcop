using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using BuildCop.Configuration;

namespace BuildCop.Test.Mocks
{
    public class MockFormatterElementOtherType : FormatterConfigurationElement
    {
        public MockFormatterElementOtherType()
        {
        }

        public MockFormatterElementOtherType(XmlReader reader)
            : base(reader)
        {
        }
    }
}