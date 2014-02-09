using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Reporting;
using BuildCop.Configuration;
using BuildCop.Rules;

namespace BuildCop.Test
{
    [TestClass]
    public class NamingConventionsRuleTest
    {
		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void CheckCorrectlyNamedProjects()
		{
			BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.prefixes.assemblyNamePrefix = "Default";
			rule.prefixes.namespacePrefix = "Default";
			rule.prefixes.assemblyNameShouldMatchRootNamespace = true;
			rule.RuleChecker = new NamingConventionsRule(rule);
			rule.name = "NamingConventions";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(0, entries.Count);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void CheckProjectWithIncorrectAssemblyName()
		{
			BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.prefixes.assemblyNamePrefix = "Dummy";
			rule.prefixes.namespacePrefix = "Default";
			rule.prefixes.assemblyNameShouldMatchRootNamespace = true;
			rule.RuleChecker = new NamingConventionsRule(rule);
			rule.name = "NamingConventions";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(1, entries.Count);
			LogEntry entry = entries[0];
			Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
			Assert.AreEqual<string>("NamingConventions", entry.Rule);
			Assert.AreEqual<string>("IncorrectAssemblyName", entry.Code);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void CheckProjectWithIncorrectRootNamespace()
		{
			BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			rule.prefixes.assemblyNamePrefix = "Default";
			rule.prefixes.namespacePrefix = "Dummy";
			rule.prefixes.assemblyNameShouldMatchRootNamespace = true;
			rule.RuleChecker = new NamingConventionsRule(rule);
			rule.name = "NamingConventions";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(1, entries.Count);
			LogEntry entry = entries[0];
			Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
			Assert.AreEqual<string>("NamingConventions", entry.Rule);
			Assert.AreEqual<string>("IncorrectRootNamespace", entry.Code);
		}

		[TestMethod]
		[DeploymentItem("NamingBuildFiles", "NamingBuildFiles")]
		public void CheckProjectWithNonMatchingAssemblyName()
		{
			BuildFile file = new BuildFile(@"NamingBuildFiles\ConsoleApplicationMismatchingNames.csproj");
			ruleElement rule = new ruleElement();
			rule.prefixes.assemblyNamePrefix = "Mismatching";
			rule.prefixes.namespacePrefix = "Default";
			rule.prefixes.assemblyNameShouldMatchRootNamespace = true;
			rule.RuleChecker = new NamingConventionsRule(rule);
			rule.name = "NamingConventions";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(1, entries.Count);
			LogEntry entry = entries[0];
			Assert.AreEqual<LogLevel>(LogLevel.Error, entry.Level);
			Assert.AreEqual<string>("NamingConventions", entry.Rule);
			Assert.AreEqual<string>("AssemblyNameRootNamespaceMismatch", entry.Code);
		}

		[TestMethod]
		[DeploymentItem("NamingBuildFiles", "NamingBuildFiles")]
		public void CheckProjectWithAllSettingsIncorrect()
		{
			BuildFile file = new BuildFile(@"NamingBuildFiles\ConsoleApplicationMismatchingNames.csproj");
			ruleElement rule = new ruleElement();
			rule.prefixes.assemblyNamePrefix = "Dummy";
			rule.prefixes.namespacePrefix = "Dummy";
			rule.prefixes.assemblyNameShouldMatchRootNamespace = true;
			rule.RuleChecker = new NamingConventionsRule(rule);
			rule.name = "NamingConventions";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(3, entries.Count);
			Assert.AreEqual<LogLevel>(LogLevel.Error, entries[0].Level);
			Assert.AreEqual<string>("NamingConventions", entries[0].Rule);
			Assert.AreEqual<string>("IncorrectRootNamespace", entries[0].Code);
			Assert.AreEqual<LogLevel>(LogLevel.Error, entries[1].Level);
			Assert.AreEqual<string>("NamingConventions", entries[1].Rule);
			Assert.AreEqual<string>("IncorrectAssemblyName", entries[1].Code);
			Assert.AreEqual<LogLevel>(LogLevel.Error, entries[2].Level);
			Assert.AreEqual<string>("NamingConventions", entries[2].Rule);
			Assert.AreEqual<string>("AssemblyNameRootNamespaceMismatch", entries[2].Code);
		}
    }
}