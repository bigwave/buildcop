using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JelleDruyts.BuildCop.Configuration;
using JelleDruyts.BuildCop.Formatters;
using JelleDruyts.BuildCop.Formatters.Configuration;
using JelleDruyts.BuildCop.Formatters.Console;
using JelleDruyts.BuildCop.Formatters.Csv;
using JelleDruyts.BuildCop.Formatters.Html;
using JelleDruyts.BuildCop.Formatters.Xml;
using JelleDruyts.BuildCop.Reporting;
using JelleDruyts.BuildCop.Test.Mocks;

namespace JelleDruyts.BuildCop.Test
{
    [TestClass]
    public class FormattersTest
    {
        [TestMethod]
        [DeploymentItem("BuildCopReport.xslt")]
        public void FormattersShouldNotThrowOnFormatting()
        {
            BuildCopConfiguration config = new BuildCopConfiguration();
            BuildGroupElement buildGroup = new BuildGroupElement();
            buildGroup.Name = "TestBuildGroup";
            BuildFilePathElement path = new BuildFilePathElement();
            path.RootPath = @"BuildFiles";
            path.SearchPattern = "DefaultConsoleApplication.csproj";
            buildGroup.BuildFiles.Paths.Add(path);
            RuleElement mockRule = new RuleElement();
            mockRule.Name = "Mock";
            mockRule.Type = typeof(MockRule).AssemblyQualifiedName;
            buildGroup.Rules.Add(mockRule);
            config.BuildGroups.Add(buildGroup);
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