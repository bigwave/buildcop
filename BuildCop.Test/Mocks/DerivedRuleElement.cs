using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using BuildCop.Configuration;

namespace BuildCop.Test.Mocks
{
    internal class DerivedRuleElement : ruleElement
    {
        public new static ruleElement Deserialize(string xml)
        {
            using (Stream memStream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(memStream))
            {
                writer.Write(xml);
                writer.Flush();
                memStream.Position = 0;
                using (XmlReader reader = XmlReader.Create(memStream))
                {
                    DerivedRuleElement element = new DerivedRuleElement();
                    reader.Read();
                    element.DeserializeElement(reader);
                    return element;
                }
            }
        }

        // Make the base class method public through this derived class.
        public void DeserializeElement(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }
    }
}