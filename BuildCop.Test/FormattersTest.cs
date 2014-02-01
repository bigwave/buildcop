using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Configuration;
using BuildCop.Formatters;
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
            HtmlFormatter htmlFormatter = new HtmlFormatter();
            htmlFormatter.output.fileName = "TestFormatterOutput.html";
            htmlFormatter.output.launch = false;
            htmlFormatter.output.stylesheet = string.Empty;
            htmlFormatter.WriteReport(report, LogLevel.Information);
            XmlFormatter xmlFormatter = new XmlFormatter();
            xmlFormatter.output.fileName = "TestFormatterOutput.xml";
            xmlFormatter.output.launch = false;
            xmlFormatter.output.stylesheet = string.Empty;
            xmlFormatter = new XmlFormatter();
            xmlFormatter.WriteReport(report, LogLevel.Information);
            CsvFormatter csvformatter = new CsvFormatter();
            csvformatter.output.fileName = "TestFormatterOutput.csv";
            csvformatter.output.launch = false;
            csvformatter.WriteReport(report, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HtmlFormatterShouldThrowOnMissingFileName()
        {
            HtmlFormatter formatter = new HtmlFormatter();
            formatter.output.fileName = null;
            formatter.WriteReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void XmlFormatterShouldThrowOnMissingFileName()
        {
            XmlFormatter formatter = new XmlFormatter();
            formatter.output.fileName = null;
            formatter.WriteReport(null, LogLevel.Information);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CsvFormatterShouldThrowOnMissingFileName()
        {
            CsvFormatter formatter = new CsvFormatter();
            formatter.output.fileName = null;
            formatter.WriteReport(null, LogLevel.Information);
        }
    }
}