using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Rules;
using JelleDruyts.BuildCop.Rules.AssemblyReferences.Configuration;
using JelleDruyts.BuildCop.Test.Mocks;

namespace JelleDruyts.BuildCop.Test
{
    [TestClass]
    public class BaseFormaterTest
    {
        [TestMethod]
        public void BaseFormatterConstructorShouldSetProperties()
        {
            MockFormatter formatter;

            formatter = new MockFormatter(null);
            Assert.IsNull(formatter.Configuration);

            MockFormatterElement configuration = new MockFormatterElement();
            formatter = new MockFormatter(configuration);
            Assert.IsNotNull(formatter.Configuration);
            Assert.AreSame(configuration, formatter.Configuration);

            MockFormatterElement typedConfiguration = formatter.GetTypedConfiguration<MockFormatterElement>();
            Assert.AreSame(configuration, typedConfiguration);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTypedConfigurationShouldThrowOnNull()
        {
            MockFormatter formatter = new MockFormatter(null);
            formatter.GetTypedConfiguration<MockFormatterElement>();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTypedConfigurationShouldThrowOnIncorrectType()
        {
            MockFormatter formatter = new MockFormatter(new MockFormatterElementOtherType());
            formatter.GetTypedConfiguration<MockFormatterElement>();
        }
    }
}