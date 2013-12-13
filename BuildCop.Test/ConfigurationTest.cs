using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Configuration;
using BuildCop.Formatters;
using BuildCop.Formatters.Configuration;
using BuildCop.Reporting;
using BuildCop.Rules.AssemblyReferences.Configuration;
using BuildCop.Rules.BuildProperties.Configuration;
using BuildCop.Rules.NamingConventions.Configuration;
using BuildCop.Rules.StrongNaming.Configuration;
using BuildCop.Test.Mocks;
using BuildCop.Rules.BuildProperties;
using BuildCop.Rules.OrphanedProjects.Configuration;

namespace BuildCop.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void ConfigurationFileShouldBeReadCorrectly()
        {
            BuildCopConfiguration config = BuildCopConfiguration.Instance;
            Assert.IsNotNull(config);
            Assert.IsNotNull(config.BuildGroups);
            Assert.AreEqual<string>("http://schemas.jelle.druyts.net/BuildCop", config.Xmlns);
            Assert.AreEqual<int>(1, config.BuildGroups.Count);

            BuildGroupElement group = config.BuildGroups[0];
            Assert.IsNotNull(group);
            Assert.AreEqual<string>("TestBuildGroup", group.Name);
            Assert.IsTrue(group.Enabled);
            Assert.IsNotNull(group.BuildFiles);
            Assert.AreEqual<string>("jpg;gif", group.BuildFiles.ExcludedFiles);
            Assert.IsNotNull(group.BuildFiles.Paths);
            Assert.AreEqual<int>(1, group.BuildFiles.Paths.Count);
            BuildFilePathElement buildFilePath = group.BuildFiles.Paths[0];
            Assert.AreEqual<string>("TestRootPath", buildFilePath.RootPath);
            Assert.AreEqual<string>("*.csproj", buildFilePath.SearchPattern);
            Assert.AreEqual<string>("exclude;bak", buildFilePath.ExcludedFiles);
            Assert.IsNotNull(group.Rules);
            Assert.AreEqual<int>(7, group.Rules.Count);

            RuleElement asmRefRule = group.Rules[0];
            Assert.IsNotNull(asmRefRule);
            Assert.AreEqual<string>("AssemblyReferenceRule", asmRefRule.Name);
            Assert.AreEqual<string>("BuildCop.Rules.AssemblyReferences.AssemblyReferenceRule", asmRefRule.Type);
            Assert.AreEqual<string>("dat;bin", asmRefRule.ExcludedFiles);
            Assert.AreEqual<string>("WinExe;Exe", asmRefRule.ExcludedOutputTypes);
            Assert.AreEqual<bool>(true, asmRefRule.Enabled);
            Assert.IsNotNull(asmRefRule.RuleConfiguration);
            Assert.IsInstanceOfType(asmRefRule.RuleConfiguration, typeof(AssemblyReferenceRuleElement));
            AssemblyReferenceRuleElement asmRefRuleConfig = (AssemblyReferenceRuleElement)asmRefRule.RuleConfiguration;
            Assert.IsNotNull(asmRefRuleConfig.AssemblyLocations);
            Assert.AreEqual<int>(1, asmRefRuleConfig.AssemblyLocations.Count);
            AssemblyLocationElement asmLocation = asmRefRuleConfig.AssemblyLocations[0];
            Assert.IsNotNull(asmLocation);
            Assert.AreEqual<string>("TestAssemblyName", asmLocation.AssemblyName);
            Assert.AreEqual<string>("TestAssemblyPath", asmLocation.AssemblyPath);

            RuleElement strongNamingRule = group.Rules[1];
            Assert.IsNotNull(strongNamingRule);
            Assert.AreEqual<string>("StrongNamingRule", strongNamingRule.Name);
            Assert.AreEqual<string>("BuildCop.Rules.StrongNaming.StrongNamingRule", strongNamingRule.Type);
            Assert.AreEqual<string>(string.Empty, strongNamingRule.ExcludedFiles);
            Assert.AreEqual<string>(string.Empty, strongNamingRule.ExcludedOutputTypes);
            Assert.AreEqual<bool>(true, strongNamingRule.Enabled);
            Assert.IsNotNull(strongNamingRule.RuleConfiguration);
            Assert.IsInstanceOfType(strongNamingRule.RuleConfiguration, typeof(StrongNamingRuleElement));
            StrongNamingRuleElement strongNamingRuleConfig = (StrongNamingRuleElement)strongNamingRule.RuleConfiguration;
            Assert.IsNotNull(strongNamingRuleConfig.StrongNaming);
            Assert.AreEqual<bool>(true, strongNamingRuleConfig.StrongNaming.StrongNameRequired);
            Assert.AreEqual<string>("TestKeyPath", strongNamingRuleConfig.StrongNaming.KeyPath);
            Assert.AreEqual<bool>(false, strongNamingRuleConfig.StrongNaming.IgnoreUnsignedProjects);

            RuleElement namingConventionsRule = group.Rules[2];
            Assert.IsNotNull(namingConventionsRule);
            Assert.AreEqual<string>("NamingConventionsRule", namingConventionsRule.Name);
            Assert.AreEqual<string>("BuildCop.Rules.NamingConventions.NamingConventionsRule", namingConventionsRule.Type);
            Assert.AreEqual<string>(string.Empty, namingConventionsRule.ExcludedFiles);
            Assert.AreEqual<string>(string.Empty, namingConventionsRule.ExcludedOutputTypes);
            Assert.AreEqual<bool>(false, namingConventionsRule.Enabled);
            Assert.IsNotNull(namingConventionsRule.RuleConfiguration);
            Assert.IsInstanceOfType(namingConventionsRule.RuleConfiguration, typeof(NamingConventionsRuleElement));
            NamingConventionsRuleElement namingConventionsRuleConfig = (NamingConventionsRuleElement)namingConventionsRule.RuleConfiguration;
            Assert.IsNotNull(namingConventionsRuleConfig.Prefixes);
            Assert.AreEqual<string>("TestAssemblyNamePrefix", namingConventionsRuleConfig.Prefixes.AssemblyNamePrefix);
            Assert.AreEqual<string>("TestNamespacePrefix", namingConventionsRuleConfig.Prefixes.NamespacePrefix);
            Assert.AreEqual<bool>(true, namingConventionsRuleConfig.Prefixes.AssemblyNameShouldMatchRootNamespace);

            RuleElement buildPropertiesRule = group.Rules[3];
            Assert.IsNotNull(buildPropertiesRule);
            Assert.AreEqual<string>("BuildPropertiesRule", buildPropertiesRule.Name);
            Assert.AreEqual<string>("BuildCop.Rules.BuildProperties.BuildPropertiesRule", buildPropertiesRule.Type);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.ExcludedFiles);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.ExcludedOutputTypes);
            Assert.AreEqual<bool>(true, buildPropertiesRule.Enabled);
            Assert.IsNotNull(buildPropertiesRule.RuleConfiguration);
            Assert.IsInstanceOfType(buildPropertiesRule.RuleConfiguration, typeof(BuildPropertiesRuleElement));
            BuildPropertiesRuleElement buildPropertiesRuleConfig = (BuildPropertiesRuleElement)buildPropertiesRule.RuleConfiguration;
            Assert.IsNotNull(buildPropertiesRuleConfig.BuildProperties);
            Assert.AreEqual<int>(3, buildPropertiesRuleConfig.BuildProperties.Count);
            Assert.AreEqual<string>("ProductVersion", buildPropertiesRuleConfig.BuildProperties[0].Name);
            Assert.AreEqual<string>("8.0.50727", buildPropertiesRuleConfig.BuildProperties[0].Value);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRuleConfig.BuildProperties[0].Condition);
            Assert.AreEqual<CompareOption>(CompareOption.EqualTo, buildPropertiesRuleConfig.BuildProperties[0].CompareOption);
            Assert.AreEqual<StringComparison>(StringComparison.Ordinal, buildPropertiesRuleConfig.BuildProperties[0].StringCompareOption);
            Assert.AreEqual<string>("SchemaVersion", buildPropertiesRuleConfig.BuildProperties[1].Name);
            Assert.AreEqual<string>("2.0", buildPropertiesRuleConfig.BuildProperties[1].Value);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRuleConfig.BuildProperties[1].Condition);
            Assert.AreEqual<CompareOption>(CompareOption.EqualTo, buildPropertiesRuleConfig.BuildProperties[1].CompareOption);
            Assert.AreEqual<StringComparison>(StringComparison.Ordinal, buildPropertiesRuleConfig.BuildProperties[1].StringCompareOption);
            Assert.AreEqual<string>("DebugType", buildPropertiesRuleConfig.BuildProperties[2].Name);
            Assert.AreEqual<string>("full", buildPropertiesRuleConfig.BuildProperties[2].Value);
            Assert.AreEqual<string>("Debug", buildPropertiesRuleConfig.BuildProperties[2].Condition);
            Assert.AreEqual<CompareOption>(CompareOption.DoesNotExist, buildPropertiesRuleConfig.BuildProperties[2].CompareOption);
            Assert.AreEqual<StringComparison>(StringComparison.OrdinalIgnoreCase, buildPropertiesRuleConfig.BuildProperties[2].StringCompareOption);

            RuleElement documentationFileRule = group.Rules[4];
            Assert.IsNotNull(documentationFileRule);
            Assert.AreEqual<string>("DocumentationFileRule", documentationFileRule.Name);
            Assert.AreEqual<string>("BuildCop.Rules.DocumentationFile.DocumentationFileRule", documentationFileRule.Type);
            Assert.AreEqual<string>(string.Empty, documentationFileRule.ExcludedFiles);
            Assert.AreEqual<string>(string.Empty, documentationFileRule.ExcludedOutputTypes);
            Assert.AreEqual<bool>(true, documentationFileRule.Enabled);
            Assert.IsNull(documentationFileRule.RuleConfiguration);

            RuleElement orphanedProjectsRule = group.Rules[5];
            Assert.IsNotNull(orphanedProjectsRule);
            Assert.AreEqual<string>("OrphanedProjects", orphanedProjectsRule.Name);
            Assert.AreEqual<string>("BuildCop.Rules.OrphanedProjects.OrphanedProjectsRule", orphanedProjectsRule.Type);
            Assert.AreEqual<string>(string.Empty, orphanedProjectsRule.ExcludedFiles);
            Assert.AreEqual<string>(string.Empty, orphanedProjectsRule.ExcludedOutputTypes);
            Assert.AreEqual<bool>(true, orphanedProjectsRule.Enabled);
            Assert.IsNotNull(orphanedProjectsRule.RuleConfiguration);
            Assert.IsInstanceOfType(orphanedProjectsRule.RuleConfiguration, typeof(OrphanedProjectsRuleElement));
            OrphanedProjectsRuleElement orphanedProjectsRuleConfig = (OrphanedProjectsRuleElement)orphanedProjectsRule.RuleConfiguration;
            Assert.IsNotNull(orphanedProjectsRuleConfig.Solutions);
            Assert.AreEqual<string>("TestSearchPath", orphanedProjectsRuleConfig.Solutions.SearchPath);

            RuleElement sharedDocumentationFileRuleRef = group.Rules[6];
            Assert.IsNotNull(sharedDocumentationFileRuleRef);
            Assert.AreEqual<string>("SharedDocumentationFileRule", sharedDocumentationFileRuleRef.Name);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRuleRef.Type);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRuleRef.ExcludedFiles);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRuleRef.ExcludedOutputTypes);
            Assert.AreEqual<bool>(true, sharedDocumentationFileRuleRef.Enabled);

            Assert.IsNotNull(config.SharedRules);
            Assert.AreEqual<int>(1, config.SharedRules.Count);
            RuleElement sharedDocumentationFileRule = config.SharedRules[0];
            Assert.IsNotNull(sharedDocumentationFileRule);
            Assert.AreEqual<string>("SharedDocumentationFileRule", sharedDocumentationFileRule.Name);
            Assert.AreEqual<string>("BuildCop.Rules.DocumentationFile.DocumentationFileRule", sharedDocumentationFileRule.Type);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRule.ExcludedFiles);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRule.ExcludedOutputTypes);
            Assert.AreEqual<bool>(true, sharedDocumentationFileRule.Enabled);
            Assert.IsNull(sharedDocumentationFileRule.RuleConfiguration);

            Assert.IsNotNull(config.Formatters);
            Assert.AreEqual<int>(4, config.Formatters.Count);

            FormatterElement consoleFormatter = config.Formatters[0];
            Assert.IsNotNull(consoleFormatter);
            Assert.AreEqual<string>("Console", consoleFormatter.Name);
            Assert.AreEqual<string>("BuildCop.Formatters.Console.ConsoleFormatter", consoleFormatter.Type);
            Assert.AreEqual<LogLevel>(LogLevel.Warning, consoleFormatter.MinimumLogLevel);
            Assert.IsNull(consoleFormatter.FormatterConfiguration);

            FormatterElement htmlFormatter = config.Formatters[1];
            Assert.IsNotNull(htmlFormatter);
            Assert.AreEqual<string>("Html", htmlFormatter.Name);
            Assert.AreEqual<string>("BuildCop.Formatters.Html.HtmlFormatter", htmlFormatter.Type);
            Assert.AreEqual<LogLevel>(LogLevel.Information, htmlFormatter.MinimumLogLevel);
            Assert.IsNotNull(htmlFormatter.FormatterConfiguration);
            Assert.IsInstanceOfType(htmlFormatter.FormatterConfiguration, typeof(XsltFilebasedFormatterElement));
            XsltFilebasedFormatterElement htmlFormatterConfig = (XsltFilebasedFormatterElement)htmlFormatter.FormatterConfiguration;
            Assert.IsNotNull(htmlFormatterConfig.Output);
            Assert.AreEqual<string>("out.html", htmlFormatterConfig.Output.FileName);
            Assert.AreEqual<string>(string.Empty, htmlFormatterConfig.Output.Stylesheet);
            Assert.AreEqual<bool>(false, htmlFormatterConfig.Output.Launch);

            FormatterElement xmlFormatter = config.Formatters[2];
            Assert.IsNotNull(xmlFormatter);
            Assert.AreEqual<string>("Xml", xmlFormatter.Name);
            Assert.AreEqual<string>("BuildCop.Formatters.Xml.XmlFormatter", xmlFormatter.Type);
            Assert.AreEqual<LogLevel>(LogLevel.Error, xmlFormatter.MinimumLogLevel);
            Assert.IsNotNull(xmlFormatter.FormatterConfiguration);
            Assert.IsInstanceOfType(xmlFormatter.FormatterConfiguration, typeof(XsltFilebasedFormatterElement));
            XsltFilebasedFormatterElement xmlFormatterConfig = (XsltFilebasedFormatterElement)xmlFormatter.FormatterConfiguration;
            Assert.IsNotNull(xmlFormatterConfig.Output);
            Assert.AreEqual<string>("out.xml", xmlFormatterConfig.Output.FileName);
            Assert.AreEqual<string>("TestStylesheet.xslt", xmlFormatterConfig.Output.Stylesheet);
            Assert.AreEqual<bool>(false, xmlFormatterConfig.Output.Launch);

            FormatterElement csvFormatter = config.Formatters[3];
            Assert.IsNotNull(csvFormatter);
            Assert.AreEqual<string>("Csv", csvFormatter.Name);
            Assert.AreEqual<string>("BuildCop.Formatters.Csv.CsvFormatter", csvFormatter.Type);
            Assert.AreEqual<LogLevel>(LogLevel.Exception, csvFormatter.MinimumLogLevel);
            Assert.IsNotNull(csvFormatter.FormatterConfiguration);
            Assert.IsInstanceOfType(csvFormatter.FormatterConfiguration, typeof(FilebasedFormatterElement));
            FilebasedFormatterElement csvFormatterConfig = (FilebasedFormatterElement)csvFormatter.FormatterConfiguration;
            Assert.IsNotNull(csvFormatterConfig.Output);
            Assert.AreEqual<string>("out.csv", csvFormatterConfig.Output.FileName);
            Assert.AreEqual<bool>(false, csvFormatterConfig.Output.Launch);

            OutputTypeCollection outputTypeMappings = config.OutputTypeMappings;
            Assert.IsNotNull(outputTypeMappings);
            Assert.AreEqual<int>(1, outputTypeMappings.Count);
            OutputTypeElement outputType = outputTypeMappings[0];
            Assert.IsNotNull(outputType);
            Assert.AreEqual<string>("Web", outputType.Alias);
            Assert.AreEqual<string>("{349c5851-65df-11da-9384-00065b846f21}", outputType.ProjectTypeGuid);
        }

        [TestMethod]
        public void TestMiscProperties()
        {
            RuleElement rule = new RuleElement();
            rule.Name = "MyName";
            rule.Type = "MyType";
            rule.ExcludedFiles = "MyExcludedFiles";
            Assert.AreEqual<string>("MyName", rule.Name);
            Assert.AreEqual<string>("MyType", rule.Type);
            Assert.AreEqual<string>("MyExcludedFiles", rule.ExcludedFiles);

            FormatterElement formatter = new FormatterElement();
            formatter.Name = "MyName";
            formatter.Type = "MyType";
            Assert.AreEqual<string>("MyName", formatter.Name);
            Assert.AreEqual<string>("MyType", formatter.Type);

            OutputTypeElement outputType = new OutputTypeElement();
            outputType.Alias = "MyAlias";
            outputType.ProjectTypeGuid = "MyProjectTypeGuid";
            Assert.AreEqual<string>("MyAlias", outputType.Alias);
            Assert.AreEqual<string>("MyProjectTypeGuid", outputType.ProjectTypeGuid);
        }

        [TestMethod]
        public void ConfigurationWithValidRuleElementShouldNotThrow()
        {
            DerivedRuleElement.Deserialize("<rule name=\"MockRule\" type=\"BuildCop.Test.Mocks.MockRule, BuildCop.Test\"><dummy/></rule>");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ConfigurationWithInvalidRuleElementShouldThrowOnWrongBaseType()
        {
            DerivedRuleElement.Deserialize("<rule name=\"MockRule\" type=\"System.String\"><dummy/></rule>");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ConfigurationWithInvalidRuleElementShouldThrowOnMissingAttribute()
        {
            DerivedRuleElement.Deserialize("<rule name=\"MockRule\" type=\"BuildCop.Test.Mocks.MockRuleInvalid, BuildCop.Test\"><dummy/></rule>");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ConfigurationWithInvalidRuleElementShouldThrowOnWrongConfigurationType()
        {
            DerivedRuleElement.Deserialize("<rule name=\"MockRule\" type=\"BuildCop.Test.Mocks.MockRuleInvalidConfigurationType, BuildCop.Test\"><dummy/></rule>");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ConfigurationWithInvalidRuleElementShouldThrowOnWrongConfigurationDefinition()
        {
            DerivedRuleElement.Deserialize("<rule name=\"MockRule\" type=\"BuildCop.Test.Mocks.MockRuleInvalidConfigurationDefinition, BuildCop.Test\"><dummy/></rule>");
        }
    }
}