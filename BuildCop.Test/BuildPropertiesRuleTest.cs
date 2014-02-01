using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Reporting;
using BuildCop.Configuration;
using System.Globalization;

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
            ruleElement config = new ruleElement();
            ruleElementBuildProperty buildProperty;
            
            // Add correct build properties.
            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Debug";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Platform";
            buildProperty.value = "AnyCPU";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "ProductVersion";
            buildProperty.value = "8.0.50727";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "DebugSymbols";
            buildProperty.value = "true";
            buildProperty.condition = "debug";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "DebugType";
            buildProperty.value = "full";
            buildProperty.condition = "debug";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "DebugType";
            buildProperty.value = "pdbonly";
            buildProperty.condition = "release";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "ErrorReport";
            buildProperty.value = "prompt";
            buildProperty.condition = ""; // No condition means it should be true for all configurations (two in this case).
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyIncorrectBuildProperties()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();
            ruleElementBuildProperty buildProperty;

            // Add incorrect build properties.
            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "dummy";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "RootNamespace";
            buildProperty.value = "dummy";
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "DebugType";
            buildProperty.value = "dummy";
            buildProperty.condition = "debug";
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.buildProperties.Count, entries.Count);
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
            ruleElement config = new ruleElement();
            ruleElementBuildProperty buildProperty;

            // Add incorrect build properties.
            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Optimize";
            buildProperty.value = "dummy";
            buildProperty.condition = ""; // No condition means it should be true for all configurations (two in this case).
            config.buildProperties.Add(buildProperty);

            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Optimize";
            buildProperty.value = "dummy";
            buildProperty.condition = null; // Also try with null.
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.buildProperties.Count * 2, entries.Count);
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
            ruleElement config = new ruleElement();
            ruleElementBuildProperty buildProperty;

            // Add missing build properties.
            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "dummy";
            buildProperty.value = "dummy";
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.buildProperties.Count, entries.Count);
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
            ruleElement config = new ruleElement();
            ruleElementBuildProperty buildProperty;

            // Add missing build properties.
            buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "dummy";
            buildProperty.value = "dummy";
            buildProperty.condition = "Debug";
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(config.buildProperties.Count, entries.Count);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Debug";
            buildProperty.compareOption = CompareOption.EqualTo.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareEqualToCorrectCaseInsensitive()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "debug";
            buildProperty.compareOption = CompareOption.EqualTo.ToString();
            buildProperty.stringCompareOption = StringComparison.OrdinalIgnoreCase.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareEqualToIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Dummy";
            buildProperty.compareOption = CompareOption.EqualTo.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "debug";
            buildProperty.compareOption = CompareOption.EqualTo.ToString();
            buildProperty.stringCompareOption = StringComparison.Ordinal.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Dummy";
            buildProperty.value = "Dummy";
            buildProperty.compareOption = CompareOption.EqualTo.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Release";
            buildProperty.compareOption = CompareOption.NotEqualTo.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareNotEqualToIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Debug";
            buildProperty.compareOption = CompareOption.NotEqualTo.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.compareOption = CompareOption.Exists.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareExistsIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Dummy";
            buildProperty.compareOption = CompareOption.Exists.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Dummy";
            buildProperty.compareOption = CompareOption.DoesNotExist.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareDoesNotExistIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.compareOption = CompareOption.DoesNotExist.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Something Debug Release Custom Whatever";
            buildProperty.compareOption = CompareOption.In.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareInIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Something Release Custom Whatever";
            buildProperty.compareOption = CompareOption.In.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Dummy";
            buildProperty.value = "Dummy";
            buildProperty.compareOption = CompareOption.In.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
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
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Something Release Custom Whatever";
            buildProperty.compareOption = CompareOption.NotIn.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(0, entries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyCompareNotInIncorrect()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            ruleElement config = new ruleElement();

            ruleElementBuildProperty buildProperty = new ruleElementBuildProperty();
            buildProperty.name = "Configuration";
            buildProperty.value = "Something Debug Release Custom Whatever";
            buildProperty.compareOption = CompareOption.NotIn.ToString();
            config.buildProperties.Add(buildProperty);

            ruleElementBuildProperty rule = new ruleElementBuildProperty();
            rule.name = "BuildProperty";
            IList<LogEntry> entries = config.Check(file);
            Assert.IsNotNull(entries);
            Assert.AreEqual<int>(1, entries.Count);
            Assert.AreEqual<string>("BuildProperty", entries[0].Rule);
            Assert.AreEqual<string>("IncorrectValue", entries[0].Code);
        }
    }
}