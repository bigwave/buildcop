using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BuildCop.Rules;
using BuildCop.Test.Mocks;

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
            Assert.IsNull(rule.config);

            MockRuleElement configuration = new MockRuleElement();
            rule = new MockRule(configuration);
            Assert.IsNotNull(rule.config);
            Assert.AreSame(configuration, rule.config);
        }
    }
}