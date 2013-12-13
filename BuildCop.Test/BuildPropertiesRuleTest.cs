using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Reporting;
using BuildCop.Rules.BuildProperties;
using BuildCop.Rules.BuildProperties.Configuration;

namespace BuildCop.Test
{
    [TestClass]
    public class BuildPropertiesRuleTest
    {
        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCorrectBuildProperties()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();
            BuildPropertyElement buildProperty;
            
            // Add correct build properties.
            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Debug";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Platform";
            buildProperty.Value = "AnyCPU";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "ProductVersion";
            buildProperty.Value = "8.0.50727";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "DebugSymbols";
            buildProperty.Value = "true";
            buildProperty.Condition = "debug";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "DebugType";
            buildProperty.Value = "full";
            buildProperty.Condition = "debug";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "DebugType";
            buildProperty.Value = "pdbonly";
            buildProperty.Condition = "release";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "ErrorReport";
            buildProperty.Value = "prompt";
            buildProperty.Condition = ""; // No condition means it should be true for all configurations (two in this case).
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyIncorrectBuildProperties()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();
            BuildPropertyElement buildProperty;

            // Add incorrect build properties.
            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "dummy";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "RootNamespace";
            buildProperty.Value = "dummy";
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "DebugType";
            buildProperty.Value = "dummy";
            buildProperty.Condition = "debug";
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.BuildProperties.Count, entries.Count);
            foreach (LogEntry entry in entries)
            {
                Assert.AreEqual<string>("BuildProperty", entry.Rule);
                Assert.AreEqual<string>("IncorrectValue", entry.Code);
            }
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyIncorrectBuildPropertiesForMultipleConditions()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();
            BuildPropertyElement buildProperty;

            // Add incorrect build properties.
            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Optimize";
            buildProperty.Value = "dummy";
            buildProperty.Condition = ""; // No condition means it should be true for all configurations (two in this case).
            config.BuildProperties.Add(buildProperty);

            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Optimize";
            buildProperty.Value = "dummy";
            buildProperty.Condition = null; // Also try with null.
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.BuildProperties.Count * 2, entries.Count);
            foreach (LogEntry entry in entries)
            {
                Assert.AreEqual<string>("BuildProperty", entry.Rule);
                Assert.AreEqual<string>("IncorrectValue", entry.Code);
            }
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyMissingBuildProperties()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();
            BuildPropertyElement buildProperty;

            // Add missing build properties.
            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "dummy";
            buildProperty.Value = "dummy";
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.BuildProperties.Count, entries.Count);
            foreach (LogEntry entry in entries)
            {
                Assert.AreEqual<string>("BuildProperty", entry.Rule);
                Assert.AreEqual<string>("PropertyShouldExist", entry.Code);
            }
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyMissingBuildPropertiesWithCondition()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();
            BuildPropertyElement buildProperty;

            // Add missing build properties.
            buildProperty = new BuildPropertyElement();
            buildProperty.Name = "dummy";
            buildProperty.Value = "dummy";
            buildProperty.Condition = "Debug";
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.BuildProperties.Count, entries.Count);
            foreach (LogEntry entry in entries)
            {
                Assert.AreEqual<string>("BuildProperty", entry.Rule);
                Assert.AreEqual<string>("PropertyShouldExist", entry.Code);
            }
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareEqualToCorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Debug";
            buildProperty.CompareOption = CompareOption.EqualTo;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareEqualToCorrectCaseInsensitive()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "debug";
            buildProperty.CompareOption = CompareOption.EqualTo;
            buildProperty.StringCompareOption = StringComparison.OrdinalIgnoreCase;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareEqualToIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Dummy";
            buildProperty.CompareOption = CompareOption.EqualTo;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("IncorrectValue", entries[0].Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareEqualToIncorrectCaseSensitive()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "debug";
            buildProperty.CompareOption = CompareOption.EqualTo;
            buildProperty.StringCompareOption = StringComparison.Ordinal;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("IncorrectValue", entries[0].Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareEqualToMissing()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Dummy";
            buildProperty.Value = "Dummy";
            buildProperty.CompareOption = CompareOption.EqualTo;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("PropertyShouldExist", entries[0].Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareNotEqualToCorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Release";
            buildProperty.CompareOption = CompareOption.NotEqualTo;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareNotEqualToIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Debug";
            buildProperty.CompareOption = CompareOption.NotEqualTo;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("IncorrectValue", entries[0].Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareExistsCorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.CompareOption = CompareOption.Exists;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareExistsIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Dummy";
            buildProperty.CompareOption = CompareOption.Exists;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("PropertyShouldExist", entries[0].Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareDoesNotExistCorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Dummy";
            buildProperty.CompareOption = CompareOption.DoesNotExist;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareDoesNotExistIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.CompareOption = CompareOption.DoesNotExist;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("PropertyShouldNotExist", entries[0].Code);
        }
        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareInCorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Something Debug Release Custom Whatever";
            buildProperty.CompareOption = CompareOption.In;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareInIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Something Release Custom Whatever";
            buildProperty.CompareOption = CompareOption.In;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("IncorrectValue", entries[0].Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareInMissing()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Dummy";
            buildProperty.Value = "Dummy";
            buildProperty.CompareOption = CompareOption.In;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("PropertyShouldExist", entries[0].Code);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareNotInCorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Something Release Custom Whatever";
            buildProperty.CompareOption = CompareOption.NotIn;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareNotInIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            BuildPropertiesRuleElement config = new BuildPropertiesRuleElement();

            BuildPropertyElement buildProperty = new BuildPropertyElement();
            buildProperty.Name = "Configuration";
            buildProperty.Value = "Something Debug Release Custom Whatever";
            buildProperty.CompareOption = CompareOption.NotIn;
            config.BuildProperties.Add(buildProperty);

            BuildPropertiesRule rule = new BuildPropertiesRule(config);
            rule.Name = "BuildProperty";
            IList<LogEntry> entries = rule.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("IncorrectValue", entries[0].Code);
        }
    }
}