using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using JelleDruyts.BuildCop.Configuration;

namespace JelleDruyts.BuildCop.Test.Mocks
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