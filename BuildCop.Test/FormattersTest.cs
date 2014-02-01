using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Configuration;
using BuildCop.Formatters;
using BuildCop.Formatters.Configuration;
using BuildCop.Formatters.Console;
using BuildCop.Formatters.Csv;
using BuildCop.Formatters.Html;
using BuildCop.Formatters.Xml;
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
            XsltFilebasedFormatterElement htmlFileConfiguration = new XsltFilebasedFormatterElement();
            htmlFileConfiguration.Output.FileName = "TestFormatterOutput.html";
            htmlFileConfiguration.Output.Launch = false;
            htmlFileConfiguration.Output.Stylesheet = string.Empty;
            formatter = new HtmlFormatter(htmlFileConfiguration);
            formatter.WriteReport(report, LogLevel.Information);
            XsltFilebasedFormatterElement xmlFileConfiguration = new XsltFilebasedFormatterElement();
            xmlFileConfiguration.Output.FileName = "TestFormatterOutput.xml";
            xmlFileConfiguration.Output.Launch = false;
            xmlFileConfiguration.Output.Stylesheet = string.Empty;
            formatter = new XmlFormatter(xmlFileConfiguration);
            formatter.WriteReport(report, LogLevel.Information);
            FilebasedFormatterElement csvFileConfiguration = new FilebasedFormatterElement();
            csvFileConfiguration.Output.FileName = "TestFormatterOutput.csv";
            csvFileConfiguration.Output.Launch = false;
            formatter = new CsvFormatter(csvFileConfiguration);
            formatter.WriteReport(report, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HtmlFormatterShouldThrowOnMissingFileName()
        {
            XsltFilebasedFormatterElement fileConfiguration = new XsltFilebasedFormatterElement();
            fileConfiguration.Output.FileName = null;
            HtmlFormatter formatter = new HtmlFormatter(fileConfiguration);
            formatter.WriteReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void XmlFormatterShouldThrowOnMissingFileName()
        {
            XsltFilebasedFormatterElement fileConfiguration = new XsltFilebasedFormatterElement();
            fileConfiguration.Output.FileName = null;
            XmlFormatter formatter = new XmlFormatter(fileConfiguration);
            formatter.WriteReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CsvFormatterShouldThrowOnMissingFileName()
        {
            FilebasedFormatterElement csvFileConfiguration = new FilebasedFormatterElement();
            csvFileConfiguration.Output.FileName = null;
            CsvFormatter formatter = new CsvFormatter(csvFileConfiguration);
            formatter.WriteReport(null, LogLevel.Information);
        }
    }
}