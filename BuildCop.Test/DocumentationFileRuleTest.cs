using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Reporting;
using BuildCop.Configuration;

namespace BuildCop.Test
{
    [TestClass]
    public class DocumentationFileRuleTest
    {
        ////[TestMethod]
        ////[DeploymentItem("BuildFiles", "BuildFiles")]
        ////public void VerifyCorrectDocumentationFile()
        ////{
        ////    BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
        ////    DocumentationFileRule rule = new DocumentationFileRule(null);
        ////    rule.Name = "DocumentationFile";
        ////    IList<LogEntry> entries = rule.Check(file);
        ////    Assert.IsNotNull(entries);
        ////    Assert.AreEqual<int>(0, entries.Count);
        ////}

        ////[TestMethod]
        ////[DeploymentItem("DocumentationBuildFiles", "DocumentationBuildFiles")]
        ////public void VerifyIncorrectDocumentationFile()
        ////{
        ////    BuildFile file = new BuildFile(@"DocumentationBuildFiles\SignedConsoleApplicationIncorrectDocumentation.csproj");
        ////    DocumentationFileRule rule = new DocumentationFileRule(null);
        ////    rule.Name = "DocumentationFile";
        ////    IList<LogEntry> entries = rule.Check(file);
        ////    Assert.IsNotNull(entries);
        ////    Assert.AreEqual<int>(2, entries.Count);
        ////    Assert.AreEqual<LogLevel>(LogLevel.Error, entries[0].Level);
        ////    Assert.AreEqual<string>("DocumentationFile", entries[0].Rule);
        ////    Assert.AreEqual<string>("IncorrectFileName", entries[0].Code);
        ////    Assert.AreEqual<LogLevel>(LogLevel.Error, entries[1].Level);
        ////    Assert.AreEqual<string>("DocumentationFile", entries[1].Rule);
        ////    Assert.AreEqual<string>("IncorrectFileName", entries[1].Code);
        ////}

        ////[TestMethod]
        ////[DeploymentItem("BuildFiles", "BuildFiles")]
        ////public void VerifyMissingDocumentationFileByMissingProperty()
        ////{
        ////    BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
        ////    DocumentationFileRule rule = new DocumentationFileRule(null);
        ////    rule.Name = "DocumentationFile";
        ////    IList<LogEntry> entries = rule.Check(file);
        ////    Assert.IsNotNull(entries);
        ////    Assert.AreEqual<int>(2, entries.Count);
        ////    Assert.AreEqual<LogLevel>(LogLevel.Error, entries[0].Level);
        ////    Assert.AreEqual<string>("DocumentationFile", entries[0].Rule);
        ////    Assert.AreEqual<string>("MissingDocumentationFile", entries[0].Code);
        ////    Assert.AreEqual<LogLevel>(LogLevel.Error, entries[1].Level);
        ////    Assert.AreEqual<string>("DocumentationFile", entries[1].Rule);
        ////    Assert.AreEqual<string>("MissingDocumentationFile", entries[1].Code);
        ////}

        ////[TestMethod]
        ////[DeploymentItem("DocumentationBuildFiles", "DocumentationBuildFiles")]
        ////public void VerifyMissingDocumentationFileByEmptyProperty()
        ////{
        ////    BuildFile file = new BuildFile(@"DocumentationBuildFiles\SignedConsoleApplicationMissingDocumentation.csproj");
        ////    DocumentationFileRule rule = new DocumentationFileRule(null);
        ////    rule.Name = "DocumentationFile";
        ////    IList<LogEntry> entries = rule.Check(file);
        ////    Assert.IsNotNull(entries);
        ////    Assert.AreEqual<int>(2, entries.Count);
        ////    Assert.AreEqual<LogLevel>(LogLevel.Error, entries[0].Level);
        ////    Assert.AreEqual<string>("DocumentationFile", entries[0].Rule);
        ////    Assert.AreEqual<string>("MissingDocumentationFile", entries[0].Code);
        ////    Assert.AreEqual<LogLevel>(LogLevel.Error, entries[1].Level);
        ////    Assert.AreEqual<string>("DocumentationFile", entries[1].Rule);
        ////    Assert.AreEqual<string>("MissingDocumentationFile", entries[1].Code);
        ////}
    }
}