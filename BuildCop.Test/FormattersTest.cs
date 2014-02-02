using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Configuration;
using BuildCop.Formatters;
using BuildCop.Reporting;
using BuildCop.Test.Mocks;

namespace BuildCop.Test
{
    [TestClass]
    public class FormattersTest
    {
        [TestMethod]
        [DeploymentItem("BuildCopReport.xslt")]
        public void FormattersShouldNotThrowOnFormatting()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            buildGroupElement buildGroup = new buildGroupElement();
            buildGroup.name = "TestBuildGroup";
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

            // Execute the known formatters.
            formatterElement htmlFormatter = new formatterElement();
            htmlFormatter.output.fileName = "TestFormatterOutput.html";
            htmlFormatter.output.launch = false;
            htmlFormatter.output.stylesheet = string.Empty;
            htmlFormatter.WriteHtmlReport(report, LogLevel.Information);
            formatterElement xmlFormatter = new formatterElement();
            xmlFormatter.output.fileName = "TestFormatterOutput.xml";
            xmlFormatter.output.launch = false;
            xmlFormatter.output.stylesheet = string.Empty;
            xmlFormatter.WriteXmlReport(report, LogLevel.Information);
            formatterElement csvformatter = new formatterElement();
            csvformatter.output.fileName = "TestFormatterOutput.csv";
            csvformatter.output.launch = false;
            csvformatter.WriteCsvReport(report, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HtmlFormatterShouldThrowOnMissingFileName()
        {
            formatterElement formatter = new formatterElement();
            formatter.output.fileName = null;
            formatter.WriteHtmlReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void XmlFormatterShouldThrowOnMissingFileName()
        {
            formatterElement formatter = new formatterElement();
            formatter.output.fileName = null;
            formatter.WriteXmlReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CsvFormatterShouldThrowOnMissingFileName()
        {
            formatterElement formatter = new formatterElement();
            formatter.output.fileName = null;
            formatter.WriteCsvReport(null, LogLevel.Information);
        }
    }
}