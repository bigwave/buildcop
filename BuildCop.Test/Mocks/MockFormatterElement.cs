using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using BuildCop.Configuration;

namespace BuildCop.Test.Mocks
{
    public class MockFormatterElement : FormatterConfigurationElement
    {
        public MockFormatterElement()
        {
        }

        public MockFormatterElement(XmlReader reader)
            : base(reader)
        {
        }
    }
}