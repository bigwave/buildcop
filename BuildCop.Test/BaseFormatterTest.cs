using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Configuration;
using BuildCop.Rules;
using BuildCop.Test.Mocks;

namespace BuildCop.Test
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
		}
    }
}