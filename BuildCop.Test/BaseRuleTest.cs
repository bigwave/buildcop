using Microsoft.VisualStudio.TestTools.UnitTesting;
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