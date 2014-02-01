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
            BuildCopConfiguration config;
            Exception theException;
            BuildCopConfiguration.LoadFromFile(@"C:\Users\ian.BIGWAVE\Documents\GitHub\buildcop\BuildCop.Console\App.config", out config, out theException);

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.buildGroups);
            Assert.AreEqual<int>(1, config.buildGroups.Count);

            buildGroupElement group = config.buildGroups[0];
            Assert.IsNotNull(group);
            Assert.AreEqual<string>("TestBuildGroup", group.name);
            Assert.IsTrue(group.enabled);
            Assert.IsNotNull(group.buildFiles);
            Assert.AreEqual<string>("jpg;gif", group.buildFiles.excludedFiles);
            Assert.IsNotNull(group.buildFiles.paths);
            Assert.AreEqual<int>(1, group.buildFiles.paths.Count);
            buildFilePathElement buildFilePath = group.buildFiles.paths[0];
            Assert.AreEqual<string>("TestRootPath", buildFilePath.rootPath);
            Assert.AreEqual<string>("*.csproj", buildFilePath.searchPattern);
            Assert.AreEqual<string>("exclude;bak", buildFilePath.excludedFiles);
            Assert.IsNotNull(group.rules);
            Assert.AreEqual<int>(7, group.rules.Count);

            ruleElement asmRefRule = group.rules[0];
            Assert.IsNotNull(asmRefRule);
            Assert.AreEqual<string>("AssemblyReferenceRule", asmRefRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.AssemblyReferences.AssemblyReferenceRule", asmRefRule.type);
            Assert.AreEqual<string>("dat;bin", asmRefRule.excludedFiles);
            Assert.AreEqual<string>("WinExe;Exe", asmRefRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, asmRefRule.enabled);
            Assert.IsNotNull(asmRefRule.RuleConfiguration);
            Assert.IsInstanceOfType(asmRefRule.RuleConfiguration, typeof(AssemblyReferenceRuleElement));
            AssemblyReferenceRuleElement asmRefRuleConfig = (AssemblyReferenceRuleElement)asmRefRule.RuleConfiguration;
            Assert.IsNotNull(asmRefRuleConfig.AssemblyLocations);
            Assert.AreEqual<int>(1, asmRefRuleConfig.AssemblyLocations.Count);
            AssemblyLocationElement asmLocation = asmRefRuleConfig.AssemblyLocations[0];
            Assert.IsNotNull(asmLocation);
            Assert.AreEqual<string>("TestAssemblyName", asmLocation.AssemblyName);
            Assert.AreEqual<string>("TestAssemblyPath", asmLocation.AssemblyPath);

            ruleElement strongNamingRule = group.rules[1];
            Assert.IsNotNull(strongNamingRule);
            Assert.AreEqual<string>("StrongNamingRule", strongNamingRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.StrongNaming.StrongNamingRule", strongNamingRule.type);
            Assert.AreEqual<string>(string.Empty, strongNamingRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, strongNamingRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, strongNamingRule.enabled);
            Assert.IsNotNull(strongNamingRule.RuleConfiguration);
            Assert.IsInstanceOfType(strongNamingRule.RuleConfiguration, typeof(StrongNamingRuleElement));
            StrongNamingRuleElement strongNamingRuleConfig = (StrongNamingRuleElement)strongNamingRule.RuleConfiguration;
            Assert.IsNotNull(strongNamingRuleConfig.StrongNaming);
            Assert.AreEqual<bool>(true, strongNamingRuleConfig.StrongNaming.StrongNameRequired);
            Assert.AreEqual<string>("TestKeyPath", strongNamingRuleConfig.StrongNaming.KeyPath);
            Assert.AreEqual<bool>(false, strongNamingRuleConfig.StrongNaming.IgnoreUnsignedProjects);

            ruleElement namingConventionsRule = group.rules[2];
            Assert.IsNotNull(namingConventionsRule);
            Assert.AreEqual<string>("NamingConventionsRule", namingConventionsRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.NamingConventions.NamingConventionsRule", namingConventionsRule.type);
            Assert.AreEqual<string>(string.Empty, namingConventionsRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, namingConventionsRule.excludedOutputTypes);
            Assert.AreEqual<bool>(false, namingConventionsRule.enabled);
            Assert.IsNotNull(namingConventionsRule.RuleConfiguration);
            Assert.IsInstanceOfType(namingConventionsRule.RuleConfiguration, typeof(NamingConventionsRuleElement));
            NamingConventionsRuleElement namingConventionsRuleConfig = (NamingConventionsRuleElement)namingConventionsRule.RuleConfiguration;
            Assert.IsNotNull(namingConventionsRuleConfig.Prefixes);
            Assert.AreEqual<string>("TestAssemblyNamePrefix", namingConventionsRuleConfig.Prefixes.AssemblyNamePrefix);
            Assert.AreEqual<string>("TestNamespacePrefix", namingConventionsRuleConfig.Prefixes.NamespacePrefix);
            Assert.AreEqual<bool>(true, namingConventionsRuleConfig.Prefixes.AssemblyNameShouldMatchRootNamespace);

            ruleElement buildPropertiesRule = group.rules[3];
            Assert.IsNotNull(buildPropertiesRule);
            Assert.AreEqual<string>("BuildPropertiesRule", buildPropertiesRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.BuildProperties.BuildPropertiesRule", buildPropertiesRule.type);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, buildPropertiesRule.enabled);
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

            ruleElement documentationFileRule = group.rules[4];
            Assert.IsNotNull(documentationFileRule);
            Assert.AreEqual<string>("DocumentationFileRule", documentationFileRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.DocumentationFile.DocumentationFileRule", documentationFileRule.type);
            Assert.AreEqual<string>(string.Empty, documentationFileRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, documentationFileRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, documentationFileRule.enabled);
            Assert.IsNull(documentationFileRule.RuleConfiguration);

            ruleElement orphanedProjectsRule = group.rules[5];
            Assert.IsNotNull(orphanedProjectsRule);
            Assert.AreEqual<string>("OrphanedProjects", orphanedProjectsRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.OrphanedProjects.OrphanedProjectsRule", orphanedProjectsRule.type);
            Assert.AreEqual<string>(string.Empty, orphanedProjectsRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, orphanedProjectsRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, orphanedProjectsRule.enabled);
            Assert.IsNotNull(orphanedProjectsRule.RuleConfiguration);
            Assert.IsInstanceOfType(orphanedProjectsRule.RuleConfiguration, typeof(OrphanedProjectsRuleElement));
            OrphanedProjectsRuleElement orphanedProjectsRuleConfig = (OrphanedProjectsRuleElement)orphanedProjectsRule.RuleConfiguration;
            Assert.IsNotNull(orphanedProjectsRuleConfig.Solutions);
            Assert.AreEqual<string>("TestSearchPath", orphanedProjectsRuleConfig.Solutions.SearchPath);

            ruleElement sharedDocumentationFileRuleRef = group.rules[6];
            Assert.IsNotNull(sharedDocumentationFileRuleRef);
            Assert.AreEqual<string>("SharedDocumentationFileRule", sharedDocumentationFileRuleRef.name);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRuleRef.type);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRuleRef.excludedFiles);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRuleRef.excludedOutputTypes);
            Assert.AreEqual<bool>(true, sharedDocumentationFileRuleRef.enabled);

            Assert.IsNotNull(config.sharedRules);
            Assert.AreEqual<int>(1, config.sharedRules.Count);
            ruleElement sharedDocumentationFileRule = config.sharedRules[0];
            Assert.IsNotNull(sharedDocumentationFileRule);
            Assert.AreEqual<string>("SharedDocumentationFileRule", sharedDocumentationFileRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.DocumentationFile.DocumentationFileRule", sharedDocumentationFileRule.type);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, sharedDocumentationFileRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, sharedDocumentationFileRule.enabled);
            Assert.IsNull(sharedDocumentationFileRule.RuleConfiguration);

            Assert.IsNotNull(config.formatters);
            Assert.AreEqual<int>(4, config.formatters.Count);

            formatterElement consoleFormatter = config.formatters[0];
            Assert.IsNotNull(consoleFormatter);
            Assert.AreEqual<string>("Console", consoleFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Console.ConsoleFormatter", consoleFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Warning, consoleFormatter.minimumLogLevel);
            Assert.IsNull(consoleFormatter.FormatterConfiguration);

            formatterElement htmlFormatter = config.formatters[1];
            Assert.IsNotNull(htmlFormatter);
            Assert.AreEqual<string>("Html", htmlFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Html.HtmlFormatter", htmlFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Information, htmlFormatter.minimumLogLevel);
            Assert.IsNotNull(htmlFormatter.FormatterConfiguration);
            Assert.IsInstanceOfType(htmlFormatter.FormatterConfiguration, typeof(XsltFilebasedFormatterElement));
            XsltFilebasedFormatterElement htmlFormatterConfig = (XsltFilebasedFormatterElement)htmlFormatter.FormatterConfiguration;
            Assert.IsNotNull(htmlFormatterConfig.Output);
            Assert.AreEqual<string>("out.html", htmlFormatterConfig.Output.FileName);
            Assert.AreEqual<string>(string.Empty, htmlFormatterConfig.Output.Stylesheet);
            Assert.AreEqual<bool>(false, htmlFormatterConfig.Output.Launch);

            formatterElement xmlFormatter = config.formatters[2];
            Assert.IsNotNull(xmlFormatter);
            Assert.AreEqual<string>("Xml", xmlFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Xml.XmlFormatter", xmlFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Error, xmlFormatter.minimumLogLevel);
            Assert.IsNotNull(xmlFormatter.FormatterConfiguration);
            Assert.IsInstanceOfType(xmlFormatter.FormatterConfiguration, typeof(XsltFilebasedFormatterElement));
            XsltFilebasedFormatterElement xmlFormatterConfig = (XsltFilebasedFormatterElement)xmlFormatter.FormatterConfiguration;
            Assert.IsNotNull(xmlFormatterConfig.Output);
            Assert.AreEqual<string>("out.xml", xmlFormatterConfig.Output.FileName);
            Assert.AreEqual<string>("TestStylesheet.xslt", xmlFormatterConfig.Output.Stylesheet);
            Assert.AreEqual<bool>(false, xmlFormatterConfig.Output.Launch);

            formatterElement csvFormatter = config.formatters[3];
            Assert.IsNotNull(csvFormatter);
            Assert.AreEqual<string>("Csv", csvFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Csv.CsvFormatter", csvFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Exception, csvFormatter.minimumLogLevel);
            Assert.IsNotNull(csvFormatter.FormatterConfiguration);
            Assert.IsInstanceOfType(csvFormatter.FormatterConfiguration, typeof(FilebasedFormatterElement));
            FilebasedFormatterElement csvFormatterConfig = (FilebasedFormatterElement)csvFormatter.FormatterConfiguration;
            Assert.IsNotNull(csvFormatterConfig.Output);
            Assert.AreEqual<string>("out.csv", csvFormatterConfig.Output.FileName);
            Assert.AreEqual<bool>(false, csvFormatterConfig.Output.Launch);

            List<outputTypeElement> outputTypeMappings = config.outputTypeMappings;
            Assert.IsNotNull(outputTypeMappings);
            Assert.AreEqual<int>(1, outputTypeMappings.Count);
            outputTypeElement outputType = outputTypeMappings[0];
            Assert.IsNotNull(outputType);
            Assert.AreEqual<string>("Web", outputType.alias);
            Assert.AreEqual<string>("{349c5851-65df-11da-9384-00065b846f21}", outputType.projectTypeGuid);
        }

        [TestMethod]
        public void TestMiscProperties()
        {
            ruleElement rule = new ruleElement();
            rule.name = "MyName";
            rule.type = "MyType";
            rule.excludedFiles = "MyExcludedFiles";
            Assert.AreEqual<string>("MyName", rule.name);
            Assert.AreEqual<string>("MyType", rule.type);
            Assert.AreEqual<string>("MyExcludedFiles", rule.excludedFiles);

            formatterElement formatter = new formatterElement();
            formatter.name = "MyName";
            formatter.type = "MyType";
            Assert.AreEqual<string>("MyName", formatter.name);
            Assert.AreEqual<string>("MyType", formatter.type);

            outputTypeElement outputType = new outputTypeElement();
            outputType.alias = "MyAlias";
            outputType.projectTypeGuid = "MyProjectTypeGuid";
            Assert.AreEqual<string>("MyAlias", outputType.alias);
            Assert.AreEqual<string>("MyProjectTypeGuid", outputType.projectTypeGuid);
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