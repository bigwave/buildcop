using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Reporting;
using BuildCop.Rules.OrphanedProjects;
using BuildCop.Rules.OrphanedProjects.Configuration;

namespace BuildCop.Test
{
    [TestClass]
    public class OrphanedProjectsRuleTest
    {
        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void ProjectInSolutionShouldBeFound()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            OrphanedProjectsRuleElement config = new OrphanedProjectsRuleElement();
            config.Solutions.SearchPath = "BuildFiles";
            OrphanedProjectsRule rule = new OrphanedProjectsRule(config);
            rule.Name = "OrphanedProjects";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void ProjectNotInSolutionShouldBeError()
        {
            BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
            OrphanedProjectsRuleElement config = new OrphanedProjectsRuleElement();
            config.Solutions.SearchPath = "BuildFiles";
            OrphanedProjectsRule rule = new OrphanedProjectsRule(config);
            rule.Name = "OrphanedProjects";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            LogEntry entry = entries[0];
            Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
            Assert.AreEqual<string>("OrphanedProjects", entry.Rule);
            Assert.AreEqual<string>("OrphanedProject", entry.Code);
        }
    }
}