using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Reporting;
using BuildCop.Rules;
using BuildCop.Configuration;

namespace BuildCop.Test
{
    [TestClass]
    public class AssemblyReferenceRuleTest
    {
		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifyCorrectAssemblyReferences()
		{
			BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			ruleElementAssemblyLocation asmLocation = new ruleElementAssemblyLocation();
			asmLocation.assemblyName = "EnvDTE";
			asmLocation.assemblyPath = @"X:\References\Microsoft.Practices.RecipeFramework\1.0.60429.0\EnvDTE.dll";
			rule.assemblyLocations.Add(asmLocation);
			rule.RuleChecker = new AssemblyReferenceRule(rule);
			rule.name = "AssemblyReference";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(0, entries.Count);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifyIncorrectAssemblyReferences()
		{
			BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			ruleElementAssemblyLocation asmLocation = new ruleElementAssemblyLocation();
			asmLocation.assemblyName = "EnvDTE";
			asmLocation.assemblyPath = "dummy";
			rule.assemblyLocations.Add(asmLocation);
			rule.RuleChecker = new AssemblyReferenceRule(rule);
			rule.name = "AssemblyReference";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
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
			ruleElement rule = new ruleElement();
			ruleElementAssemblyLocation asmLocation = new ruleElementAssemblyLocation();
			asmLocation.assemblyName = "dummy";
			asmLocation.assemblyPath = "";
			rule.assemblyLocations.Add(asmLocation);
			asmLocation = new ruleElementAssemblyLocation();
			asmLocation.assemblyName = "EnvDTE";
			asmLocation.assemblyPath = "";
			rule.assemblyLocations.Add(asmLocation);
			rule.RuleChecker = new AssemblyReferenceRule(rule);
			rule.name = "AssemblyReference";
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(0, entries.Count);
		}

		[TestMethod]
		[DeploymentItem("BuildFiles", "BuildFiles")]
		public void VerifyMissingAssemblyLocation()
		{
			BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
			ruleElement rule = new ruleElement();
			ruleElementAssemblyLocation asmLocation = new ruleElementAssemblyLocation();
			rule.name = "AssemblyReference";
			rule.RuleChecker = new AssemblyReferenceRule(rule);
			IList<LogEntry> entries = rule.RuleChecker.Check(file);
			Assert.IsNotNull(entries);
			Assert.AreEqual<int>(1, entries.Count);
			LogEntry entry = entries[0];
			Assert.AreEqual<LogLevel>(LogLevel.Warning, entry.Level);
			Assert.AreEqual<string>("AssemblyReference", entry.Rule);
			Assert.AreEqual<string>("MissingAssemblyLocation", entry.Code);
		}
    }
}