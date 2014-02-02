using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Configuration;
using BuildCop.Reporting;
using BuildCop.Rules;
using BuildCop.Test.Mocks;

namespace BuildCop.Test
{
    [TestClass]
    public class BuildCopEngineTest
    {
        #region Positive Tests

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildCopShouldExecuteRules()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "DefaultConsoleApplication.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            CheckMockFileReport(groupReport.BuildFileReports[0], @"BuildFiles\DefaultConsoleApplication.csproj");
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildCopShouldExecuteSharedRules()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "DefaultConsoleApplication.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            buildGroup.rules.Add(mockRule);
            ruleElement sharedMockRule = new ruleElement();
            sharedMockRule.name = "Mock";
            sharedMockRule.type = typeof(MockRule).AssemblyQualifiedName;
            config.sharedRules.Add(sharedMockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            CheckMockFileReport(groupReport.BuildFileReports[0], @"BuildFiles\DefaultConsoleApplication.csproj");
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildCopShouldExecuteFormatters()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "DefaultConsoleApplication.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);
            formatterElement formatter = new formatterElement();
            formatter.type = typeof(MockFormatter).AssemblyQualifiedName;
            formatter.minimumLogLevel = LogLevel.Information;
            config.formatters.Add(formatter);

            MockFormatter.LastReport = null;
            BuildCopEngine.Execute(config);
            BuildCopReport lastReport = MockFormatter.LastReport;
            Assert.IsNotNull(lastReport);
            Assert.AreEqual<string>(typeof(BuildCopEngine).Assembly.GetName().Version.ToString(), lastReport.EngineVersion);
            Assert.IsTrue(DateTime.Now - lastReport.GeneratedTime < TimeSpan.FromMinutes(1));
            IList<BuildGroupReport> groupReports = lastReport.BuildGroupReports;

            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            CheckMockFileReport(groupReport.BuildFileReports[0], @"BuildFiles\DefaultConsoleApplication.csproj");
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyMultipleFiles()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(2, groupReport.BuildFileReports.Count);
            CheckMockFileReport(groupReport.BuildFileReports[0], @"BuildFiles\DefaultConsoleApplication.csproj");
            CheckMockFileReport(groupReport.BuildFileReports[1], @"BuildFiles\SignedConsoleApplication.csproj");
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyMultipleFilesWithSearchPattern()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "default*.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            CheckMockFileReport(groupReport.BuildFileReports[0], @"BuildFiles\DefaultConsoleApplication.csproj");
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyMultipleFilesWithExclude()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.excludedFiles = "default";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);

            Assert.IsNotNull(report);
            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            CheckMockFileReport(groupReport.BuildFileReports[0], @"BuildFiles\SignedConsoleApplication.csproj");
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void VerifyMultipleFilesWithGlobalExclude()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildGroup.buildFiles.excludedFiles = "signed";
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.excludedFiles = "default";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);

            Assert.IsNotNull(report);
            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(0, groupReport.BuildFileReports.Count);
        }

        [TestMethod]
        public void BuildCopShouldExcludeBuildGroupsFromExplicitConfig()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.enabled = true;
            buildGroup.name = "TestBuildGroup";
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.excludedFiles = "default";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config, new string[] { });

            Assert.IsNotNull(report);
            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(0, groupReports.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildCopReport.xslt")]
        [DeploymentItem("BuildCop.config")]
        public void BuildCopShouldExcludeBuildGroupsFromAppConfig()
        {
            BuildCopReport report = BuildCopEngine.Execute(new string[] { });

            Assert.IsNotNull(report);
            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(0, groupReports.Count);
        }

        [TestMethod]
        public void BuildCopShouldExcludeBuildGroupsWithoutRules()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.excludedFiles = "default";
            buildGroup.buildFiles.paths.Add(path);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);

            Assert.IsNotNull(report);
            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(0, groupReports.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildCopShouldReportRuleExceptions()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "DefaultConsoleApplication.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(ExceptionMockRule).AssemblyQualifiedName;
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            BuildFileReport fileReport = groupReport.BuildFileReports[0];
            Assert.IsNotNull(fileReport);
            Assert.AreEqual<string>(@"BuildFiles\DefaultConsoleApplication.csproj", fileReport.FileName);
            Assert.IsNotNull(fileReport.LogEntries);
            Assert.AreEqual<int>(1, fileReport.LogEntries.Count);
            LogEntry entry = fileReport.LogEntries[0];
            Assert.AreEqual<LogLevel>(LogLevel.Exception, entry.Level);
            Assert.AreEqual<string>("An exception occurred while analysing the build file.", entry.Message);
            Assert.IsNotNull(entry.Detail);
            Assert.IsTrue(entry.Detail.Contains("ExceptionMock was configured to throw exceptions."));
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildCopShouldExcludeRulesOnFileName()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "DefaultConsoleApplication.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            mockRule.excludedFiles = "DefaultConsoleApplication.csproj";
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            Assert.AreEqual<int>(0, groupReport.BuildFileReports[0].LogEntries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildCopShouldExcludeRulesOnOutputType()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "DefaultConsoleApplication.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            mockRule.excludedOutputTypes = "Dummy;Web;Exe";
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            Assert.AreEqual<int>(0, groupReport.BuildFileReports[0].LogEntries.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildCopShouldExcludeRulesOnOutputTypeWithProjectTypeGuids()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            buildFilePathElement path = new buildFilePathElement();
            path.rootPath = @"BuildFiles";
            path.searchPattern = "SignedConsoleApplication.csproj";
            buildGroup.buildFiles.paths.Add(path);
            ruleElement mockRule = new ruleElement();
            mockRule.name = "Mock";
            mockRule.type = typeof(MockRule).AssemblyQualifiedName;
            mockRule.excludedOutputTypes = "Dummy;Web";
            buildGroup.rules.Add(mockRule);
            config.buildGroups.Add(buildGroup);

            BuildCopReport report = BuildCopEngine.Execute(config);
            Assert.IsNotNull(report);

            IList<BuildGroupReport> groupReports = report.BuildGroupReports;
            Assert.IsNotNull(groupReports);
            Assert.AreEqual<int>(1, groupReports.Count);
            BuildGroupReport groupReport = groupReports[0];
            Assert.IsNotNull(groupReport);
            Assert.AreEqual<string>("TestBuildGroup", groupReport.BuildGroupName);
            Assert.IsNotNull(groupReport.BuildFileReports);
            Assert.AreEqual<int>(1, groupReport.BuildFileReports.Count);
            Assert.AreEqual<int>(0, groupReport.BuildFileReports[0].LogEntries.Count);
        }

        #endregion

        #region Negative Tests

        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void RuleTypeShouldBeValidType()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            ruleElement invalidRule = new ruleElement();
            buildGroup.rules.Add(invalidRule);
            config.buildGroups.Add(buildGroup);
            BuildCopEngine.Execute(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException), "The rule type must derive from the BaseRule class. Type name: System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
        public void RuleTypeShouldBeBaseRuleType()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            ruleElement invalidRule = new ruleElement();
            invalidRule.type = typeof(string).AssemblyQualifiedName;
            buildGroup.rules.Add(invalidRule);
            config.buildGroups.Add(buildGroup);
            BuildCopEngine.Execute(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException), "The rule type must have a constructor that takes a RuleConfigurationElement. Type name: BuildCop.Test.Mocks.MockRuleInvalid")]
        public void RuleTypeShouldHaveExpectedConstructor()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
            buildGroup.enabled = true;
            ruleElement invalidRule = new ruleElement();
            invalidRule.type = typeof(MockRuleInvalid).AssemblyQualifiedName;
            buildGroup.rules.Add(invalidRule);
            config.buildGroups.Add(buildGroup);
            BuildCopEngine.Execute(config);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void FormatterTypeShouldBeValidType()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            formatterElement invalidFormatter = new formatterElement();
            config.formatters.Add(invalidFormatter);
            BuildCopEngine.Execute(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException), "The formatter type must derive from the BaseFormatter class. Type name: System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
        public void FormatterTypeShouldBeBaseRuleType()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            formatterElement invalidFormatter = new formatterElement();
            invalidFormatter.type = typeof(string).AssemblyQualifiedName;
            config.formatters.Add(invalidFormatter);
            BuildCopEngine.Execute(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException), "The formatter type must have a default constructor. Type name: BuildCop.Test.Mocks.MockFormatterInvalid")]
        public void FormatterTypeShouldHaveExpectedConstructor()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            formatterElement invalidFormatter = new formatterElement();
            invalidFormatter.type = typeof(MockFormatterInvalid).AssemblyQualifiedName;
            config.formatters.Add(invalidFormatter);
            BuildCopEngine.Execute(config);
        }

        [TestMethod]
        [DeploymentItem("BuildCopReport.xslt")]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void ExecuteShouldTakeAppConfig()
        {
            BuildCopReport report = BuildCopEngine.Execute();
        }

        #endregion

        #region Helper Methods

        private static void CheckMockFileReport(BuildFileReport fileReport, string expectedFileName)
        {
            Assert.IsNotNull(fileReport);
            Assert.AreEqual<string>(expectedFileName, fileReport.FileName);
            Assert.IsNotNull(fileReport.LogEntries);
            Assert.AreEqual<int>(1, fileReport.LogEntries.Count);
            LogEntry entry = fileReport.LogEntries[0];
            Assert.AreEqual<LogLevel>(LogLevel.Information, entry.Level);
            Assert.AreEqual<string>("Mock", entry.Rule);
            Assert.AreEqual<string>("Mock", entry.Code);
            Assert.AreEqual<string>("Checked by mock", entry.Message);
            Assert.IsTrue(entry.Detail.Contains("detailed message"));
        }

        #endregion
    }
}