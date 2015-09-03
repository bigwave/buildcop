using System;

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
            BaseFormatter formatter;
            formatter = new ConsoleFormatter(null);
            formatter.WriteReport(report, LogLevel.Information);
            formatterElement htmlFileConfiguration = new formatterElement();
            htmlFileConfiguration.output.fileName = "TestFormatterOutput.html";
            htmlFileConfiguration.output.launch = false;
            htmlFileConfiguration.output.stylesheet = string.Empty;
            formatter = new HtmlFormatter(htmlFileConfiguration);
            formatter.WriteReport(report, LogLevel.Information);
            formatterElement xmlFileConfiguration = new formatterElement();
            xmlFileConfiguration.output.fileName = "TestFormatterOutput.xml";
            xmlFileConfiguration.output.launch = false;
            xmlFileConfiguration.output.stylesheet = string.Empty;
            formatter = new XmlFormatter(xmlFileConfiguration);
            formatter.WriteReport(report, LogLevel.Information);
            formatterElement csvFileConfiguration = new formatterElement();
            csvFileConfiguration.output.fileName = "TestFormatterOutput.csv";
            csvFileConfiguration.output.launch = false;
            formatter = new CsvFormatter(csvFileConfiguration);
            formatter.WriteReport(report, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HtmlFormatterShouldThrowOnMissingFileName()
        {
            formatterElement fileConfiguration = new formatterElement();
            fileConfiguration.output.fileName = null;
            HtmlFormatter formatter = new HtmlFormatter(fileConfiguration);
            formatter.WriteReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void XmlFormatterShouldThrowOnMissingFileName()
        {
            formatterElement fileConfiguration = new formatterElement();
            fileConfiguration.output.fileName = null;
            XmlFormatter formatter = new XmlFormatter(fileConfiguration);
            formatter.WriteReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CsvFormatterShouldThrowOnMissingFileName()
        {
            formatterElement csvFileConfiguration = new formatterElement();
            csvFileConfiguration.output.fileName = null;
            CsvFormatter formatter = new CsvFormatter(csvFileConfiguration);
            formatter.WriteReport(null, LogLevel.Information);
        }
    }
}