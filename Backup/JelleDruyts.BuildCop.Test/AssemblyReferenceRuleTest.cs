using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Rules;
using JelleDruyts.BuildCop.Rules.AssemblyReferences;
using JelleDruyts.BuildCop.Rules.AssemblyReferences.Configuration;

namespace JelleDruyts.BuildCop.Test
{
    [TestClass]
    public class AssemblyReferenceRuleTest
    {
        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCorrectAssemblyReferences()
        {
            BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
            AssemblyReferenceRuleElement config = new AssemblyReferenceRuleElement();
            AssemblyLocationElement asmLocation = new AssemblyLocationElement();
            asmLocation.AssemblyName = "EnvDTE";
            asmLocation.AssemblyPath = @"X:\References\Microsoft.Practices.RecipeFramework\1.0.60429.0\EnvDTE.dll";
            config.AssemblyLocations.Add(asmLocation);
            AssemblyReferenceRule rule = new AssemblyReferenceRule(config);
            rule.Name = "AssemblyReference";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyIncorrectAssemblyReferences()
        {
            BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
            AssemblyReferenceRuleElement config = new AssemblyReferenceRuleElement();
            AssemblyLocationElement asmLocation = new AssemblyLocationElement();
            asmLocation.AssemblyName = "EnvDTE";
            asmLocation.AssemblyPath = "dummy";
            config.AssemblyLocations.Add(asmLocation);
            AssemblyReferenceRule rule = new AssemblyReferenceRule(config);
            rule.Name = "AssemblyReference";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            LogEntry entry = entries[0];
            Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
            Assert.AreEqual<string>("AssemblyReference", entry.Rule);
            Assert.AreEqual<string>("IncorrectHintPath", entry.Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCorrectAssemblyReferencesBecauseOfMissingAssemblyPath()
        {
            BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
            AssemblyReferenceRuleElement config = new AssemblyReferenceRuleElement();
            AssemblyLocationElement asmLocation = new AssemblyLocationElement();
            asmLocation.AssemblyName = "dummy";
            asmLocation.AssemblyPath = "";
            config.AssemblyLocations.Add(asmLocation);
            asmLocation = new AssemblyLocationElement();
            asmLocation.AssemblyName = "EnvDTE";
            asmLocation.AssemblyPath = "";
            config.AssemblyLocations.Add(asmLocation);
            AssemblyReferenceRule rule = new AssemblyReferenceRule(config);
            rule.Name = "AssemblyReference";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyMissingAssemblyLocation()
        {
            BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
            AssemblyReferenceRuleElement config = new AssemblyReferenceRuleElement();
            AssemblyReferenceRule rule = new AssemblyReferenceRule(config);
            rule.Name = "AssemblyReference";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            LogEntry entry = entries[0];
            Assert.AreEqual<LogLevel>(LogLevel.Warning, entry.Level);
            Assert.AreEqual<string>("AssemblyReference", entry.Rule);
            Assert.AreEqual<string>("MissingAssemblyLocation", entry.Code);
        }
    }
}