using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules;
using JelleDruyts.BuildCop.Rules.StrongNaming;
using JelleDruyts.BuildCop.Rules.StrongNaming.Configuration;

namespace JelleDruyts.BuildCop.Test
{
    [TestClass]
    public class StrongNamingRuleTest
    {
        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyUnsignedProjectThatShouldNotBeSigned()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            StrongNamingRuleElement config = new StrongNamingRuleElement();
            config.StrongNaming.StrongNameRequired = false;
            StrongNamingRule rule = new StrongNamingRule(config);
            rule.Name = "StrongNaming";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyUnsignedProjectThatShouldBeSigned()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            StrongNamingRuleElement config = new StrongNamingRuleElement();
            config.StrongNaming.KeyPath = "dummy";
            config.StrongNaming.StrongNameRequired = true;
            StrongNamingRule rule = new StrongNamingRule(config);
            rule.Name = "StrongNaming";
            IList<LogEntry> entries = rule.Check(file);
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
            StrongNamingRuleElement config = new StrongNamingRuleElement();
            config.StrongNaming.KeyPath = "dummy";
            config.StrongNaming.StrongNameRequired = true;
            config.StrongNaming.IgnoreUnsignedProjects = true;
            StrongNamingRule rule = new StrongNamingRule(config);
            rule.Name = "StrongNaming";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifySignedProjectThatShouldNotBeSigned()
        {
            BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
            StrongNamingRuleElement config = new StrongNamingRuleElement();
            config.StrongNaming.StrongNameRequired = false;
            StrongNamingRule rule = new StrongNamingRule(config);
            rule.Name = "StrongNaming";
            IList<LogEntry> entries = rule.Check(file);
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
            StrongNamingRuleElement config = new StrongNamingRuleElement();
            config.StrongNaming.KeyPath = "dummy";
            config.StrongNaming.StrongNameRequired = true;
            StrongNamingRule rule = new StrongNamingRule(config);
            rule.Name = "StrongNaming";
            IList<LogEntry> entries = rule.Check(file);
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
            StrongNamingRuleElement config = new StrongNamingRuleElement();
            config.StrongNaming.KeyPath = @"C:\MyKey.snk";
            config.StrongNaming.StrongNameRequired = true;
            StrongNamingRule rule = new StrongNamingRule(config);
            rule.Name = "StrongNaming";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }
    }
}