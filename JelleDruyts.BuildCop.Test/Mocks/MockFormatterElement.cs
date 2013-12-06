using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using JelleDruyts.BuildCop.Configuration;

namespace JelleDruyts.BuildCop.Test.Mocks
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