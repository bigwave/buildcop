using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Reporting;
using BuildCop.Rules;
using BuildCop.Configuration;

namespace BuildCop.Test
{
    [TestClass]
    public class StrongNamingRuleTest
    {
        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyUnsignedProjectThatShouldNotBeSigned()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement rule = new  ruleElement();
            rule.strongNaming.strongNameRequired = false;
            rule.name = "StrongNaming";
            rule.RuleChecker = new StrongNamingRule(rule);
            IList<LogEntry> entries = rule.RuleChecker.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifyUnsignedProjectThatShouldBeSigned()
		{
			BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.strongNaming.strongNameRequired = false;
			rule.name = "StrongNaming";
			rule.RuleChecker = new StrongNamingRule(rule);
			rule.strongNaming.strongNameRequired = true;
			rule.name = "StrongNaming";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(1, entries.Count);
			LogEntry entry = entries[0];
			Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
			Assert.AreEqual<string>("StrongNaming", entry.Rule);
			Assert.AreEqual<string>("SigningShouldBeEnabled", entry.Code);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifyIgnoredUnsignedProjectThatShouldBeSigned()
		{
			BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.strongNaming.keyPath = "dummy";
			rule.strongNaming.strongNameRequired = true;
			rule.strongNaming.ignoreUnsignedProjects = true;
			rule.RuleChecker = new StrongNamingRule(rule);
			rule.name = "StrongNaming";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(0, entries.Count);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifySignedProjectThatShouldNotBeSigned()
		{
			BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.strongNaming.strongNameRequired = false;
			rule.RuleChecker = new StrongNamingRule(rule);
			rule.name = "StrongNaming";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(1, entries.Count);
			LogEntry entry = entries[0];
			Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
			Assert.AreEqual<string>("StrongNaming", entry.Rule);
			Assert.AreEqual<string>("SigningShouldBeDisabled", entry.Code);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifySignedProjectThatIsSignedWithIncorrectKey()
		{
			BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.strongNaming.keyPath = "dummy";
			rule.strongNaming.strongNameRequired = true;
			rule.RuleChecker = new StrongNamingRule(rule);
			rule.name = "StrongNaming";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(1, entries.Count);
			LogEntry entry = entries[0];
			Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
			Assert.AreEqual<string>("StrongNaming", entry.Rule);
			Assert.AreEqual<string>("SignedWithIncorrectKey", entry.Code);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifySignedProjectThatIsSignedWithCorrectKey()
		{
			BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.strongNaming.keyPath = @"C:\MyKey.snk";
			rule.strongNaming.strongNameRequired = true;
			rule.RuleChecker = new StrongNamingRule(rule);
			rule.name = "StrongNaming";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(0, entries.Count);
		}
    }
}