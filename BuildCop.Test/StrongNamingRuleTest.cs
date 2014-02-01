using System;
using System.Collections.Generic;
using System.Text;

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
            IList<LogEntry> entries = rule.CheckStrongNamingRule(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }
        // TODO Fix these first
    ////    [TestMethod]
    ////    [DeploymentItem("BuildFiles", "BuildFiles")]
    ////    public void VerifyUnsignedProjectThatShouldBeSigned()
    ////    {
    ////        BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
    ////        ruleElementStrongNaming config = new ruleElementStrongNaming();
    ////        config.StrongNaming.KeyPath = "dummy";
    ////        config.StrongNaming.StrongNameRequired = true;
    ////        StrongNamingRule rule = new StrongNamingRule(config);
    ////        rule.Name = "StrongNaming";
    ////        IList<LogEntry> entries = rule.Check(file);
    ////        Assert.IsNotNull(entries);
    ////        Assert.AreEqual<int>(1, entries.Count);
    ////        LogEntry entry = entries[0];
    ////        Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
    ////        Assert.AreEqual<string>("StrongNaming", entry.Rule);
    ////        Assert.AreEqual<string>("SigningShouldBeEnabled", entry.Code);
    ////    }

    ////    [TestMethod]
    ////    [DeploymentItem("BuildFiles", "BuildFiles")]
    ////    public void VerifyIgnoredUnsignedProjectThatShouldBeSigned()
    ////    {
    ////        BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
    ////        ruleElementStrongNaming config = new ruleElementStrongNaming();
    ////        config.StrongNaming.KeyPath = "dummy";
    ////        config.StrongNaming.StrongNameRequired = true;
    ////        config.StrongNaming.IgnoreUnsignedProjects = true;
    ////        StrongNamingRule rule = new StrongNamingRule(config);
    ////        rule.Name = "StrongNaming";
    ////        IList<LogEntry> entries = rule.Check(file);
    ////        Assert.IsNotNull(entries);
    ////        Assert.AreEqual<int>(0, entries.Count);
    ////    }

    ////    [TestMethod]
    ////    [DeploymentItem("BuildFiles", "BuildFiles")]
    ////    public void VerifySignedProjectThatShouldNotBeSigned()
    ////    {
    ////        BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
    ////        ruleElementStrongNaming config = new ruleElementStrongNaming();
    ////        config.StrongNaming.StrongNameRequired = false;
    ////        StrongNamingRule rule = new StrongNamingRule(config);
    ////        rule.Name = "StrongNaming";
    ////        IList<LogEntry> entries = rule.Check(file);
    ////        Assert.IsNotNull(entries);
    ////        Assert.AreEqual<int>(1, entries.Count);
    ////        LogEntry entry = entries[0];
    ////        Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
    ////        Assert.AreEqual<string>("StrongNaming", entry.Rule);
    ////        Assert.AreEqual<string>("SigningShouldBeDisabled", entry.Code);
    ////    }

    ////    [TestMethod]
    ////    [DeploymentItem("BuildFiles", "BuildFiles")]
    ////    public void VerifySignedProjectThatIsSignedWithIncorrectKey()
    ////    {
    ////        BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
    ////        ruleElementStrongNaming config = new ruleElementStrongNaming();
    ////        config.StrongNaming.KeyPath = "dummy";
    ////        config.StrongNaming.StrongNameRequired = true;
    ////        StrongNamingRule rule = new StrongNamingRule(config);
    ////        rule.Name = "StrongNaming";
    ////        IList<LogEntry> entries = rule.Check(file);
    ////        Assert.IsNotNull(entries);
    ////        Assert.AreEqual<int>(1, entries.Count);
    ////        LogEntry entry = entries[0];
    ////        Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
    ////        Assert.AreEqual<string>("StrongNaming", entry.Rule);
    ////        Assert.AreEqual<string>("SignedWithIncorrectKey", entry.Code);
    ////    }

    ////    [TestMethod]
    ////    [DeploymentItem("BuildFiles", "BuildFiles")]
    ////    public void VerifySignedProjectThatIsSignedWithCorrectKey()
    ////    {
    ////        BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
    ////        ruleElementStrongNaming config = new ruleElementStrongNaming();
    ////        config.StrongNaming.KeyPath = @"C:\MyKey.snk";
    ////        config.StrongNaming.StrongNameRequired = true;
    ////        StrongNamingRule rule = new StrongNamingRule(config);
    ////        rule.Name = "StrongNaming";
    ////        IList<LogEntry> entries = rule.Check(file);
    ////        Assert.IsNotNull(entries);
    ////        Assert.AreEqual<int>(0, entries.Count);
    ////    }
    }
}