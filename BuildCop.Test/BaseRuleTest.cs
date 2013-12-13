using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BuildCop.Rules;
using BuildCop.Test.Mocks;
using BuildCop.Rules.AssemblyReferences.Configuration;

namespace BuildCop.Test
{
    [TestClass]
    public class BaseRuleTest
    {
        [TestMethod]
        public void BaseRuleConstructorShouldSetProperties()
        {
            MockRule rule;
            
            rule = new MockRule(null);
            Assert.IsNull(rule.Configuration);

            MockRuleElement configuration = new MockRuleElement();
            rule = new MockRule(configuration);
            Assert.IsNotNull(rule.Configuration);
            Assert.AreSame(configuration, rule.Configuration);

            MockRuleElement typedConfiguration = rule.GetTypedConfiguration<MockRuleElement>();
            Assert.AreSame(configuration, typedConfiguration);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTypedConfigurationShouldThrowOnNull()
        {
            MockRule rule = new MockRule(null);
            rule.GetTypedConfiguration<MockRuleElement>();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTypedConfigurationShouldThrowOnIncorrectType()
        {
            MockRule rule = new MockRule(new AssemblyReferenceRuleElement());
            rule.GetTypedConfiguration<MockRuleElement>();
        }
    }
}