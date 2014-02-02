using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BuildCop.Configuration;
using BuildCop.Formatters;
using BuildCop.Reporting;
using BuildCop.Test.Mocks;

namespace BuildCop.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void ConfigurationFileShouldBeReadCorrectly()
        {
            BuildCopConfiguration config = BuildCopConfiguration.LoadFromFile(@"BuildCop.config");

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
            ////Assert.IsNotNull(asmRefRule.RuleConfiguration);
            ////Assert.IsInstanceOfType(asmRefRule.assemblyLocations, typeof(AssemblyReferenceRuleElement));
            ////AssemblyReferenceRuleElement asmRefRuleConfig = (AssemblyReferenceRuleElement)asmRefRule.RuleConfiguration;
            Assert.IsNotNull(asmRefRule.assemblyLocations);
            Assert.AreEqual<int>(1, asmRefRule.assemblyLocations.Count);
            ruleElementAssemblyLocation asmLocation = asmRefRule.assemblyLocations[0];
            Assert.IsNotNull(asmLocation);
            Assert.AreEqual<string>("TestAssemblyName", asmLocation.assemblyName);
            Assert.AreEqual<string>("TestAssemblyPath", asmLocation.assemblyPath);

            ruleElement strongNamingRule = group.rules[1];
            Assert.IsNotNull(strongNamingRule);
            Assert.AreEqual<string>("StrongNamingRule", strongNamingRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.StrongNaming.StrongNamingRule", strongNamingRule.type);
            Assert.AreEqual<string>(string.Empty, strongNamingRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, strongNamingRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, strongNamingRule.enabled);
            Assert.AreEqual<bool>(true, strongNamingRule.strongNaming.strongNameRequired);
            Assert.AreEqual<string>("TestKeyPath", strongNamingRule.strongNaming.keyPath);
            Assert.AreEqual<bool>(false, strongNamingRule.strongNaming.ignoreUnsignedProjects);

            ruleElement namingConventionsRule = group.rules[2];
            Assert.IsNotNull(namingConventionsRule);
            Assert.AreEqual<string>("NamingConventionsRule", namingConventionsRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.NamingConventions.NamingConventionsRule", namingConventionsRule.type);
            Assert.AreEqual<string>(string.Empty, namingConventionsRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, namingConventionsRule.excludedOutputTypes);
            Assert.AreEqual<bool>(false, namingConventionsRule.enabled);
            Assert.IsNotNull(namingConventionsRule.prefixes);
            Assert.AreEqual<string>("TestAssemblyNamePrefix", namingConventionsRule.prefixes.assemblyNamePrefix);
            Assert.AreEqual<string>("TestNamespacePrefix", namingConventionsRule.prefixes.namespacePrefix);
            Assert.AreEqual<bool>(true, namingConventionsRule.prefixes.assemblyNameShouldMatchRootNamespace);

            ruleElement buildPropertiesRule = group.rules[3];
            Assert.IsNotNull(buildPropertiesRule);
            Assert.AreEqual<string>("BuildPropertiesRule", buildPropertiesRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.BuildProperties.BuildPropertiesRule", buildPropertiesRule.type);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, buildPropertiesRule.enabled);
            Assert.IsNotNull(buildPropertiesRule.buildProperties);
            Assert.AreEqual<int>(3, buildPropertiesRule.buildProperties.Count);
            Assert.AreEqual<string>("ProductVersion", buildPropertiesRule.buildProperties[0].name);
            Assert.AreEqual<string>("8.0.50727", buildPropertiesRule.buildProperties[0].value);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.buildProperties[0].condition);
            Utility.CheckCompareOption(CompareOption.EqualTo, buildPropertiesRule.buildProperties[0].compareOption);
            Utility.CheckStringComparison(StringComparison.Ordinal, buildPropertiesRule.buildProperties[0].stringCompareOption);
            Assert.AreEqual<string>("SchemaVersion", buildPropertiesRule.buildProperties[1].name);
            Assert.AreEqual<string>("2.0", buildPropertiesRule.buildProperties[1].value);
            Assert.AreEqual<string>(string.Empty, buildPropertiesRule.buildProperties[1].condition);
            Utility.CheckCompareOption(CompareOption.EqualTo, buildPropertiesRule.buildProperties[1].compareOption);
            Utility.CheckStringComparison(StringComparison.Ordinal, buildPropertiesRule.buildProperties[1].stringCompareOption);
            Assert.AreEqual<string>("DebugType", buildPropertiesRule.buildProperties[2].name);
            Assert.AreEqual<string>("full", buildPropertiesRule.buildProperties[2].value);
            Assert.AreEqual<string>("Debug", buildPropertiesRule.buildProperties[2].condition);
            Utility.CheckCompareOption(CompareOption.DoesNotExist, buildPropertiesRule.buildProperties[2].compareOption);
            Utility.CheckStringComparison(StringComparison.OrdinalIgnoreCase, buildPropertiesRule.buildProperties[2].stringCompareOption);

            ruleElement documentationFileRule = group.rules[4];
            Assert.IsNotNull(documentationFileRule);
            Assert.AreEqual<string>("DocumentationFileRule", documentationFileRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.DocumentationFile.DocumentationFileRule", documentationFileRule.type);
            Assert.AreEqual<string>(string.Empty, documentationFileRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, documentationFileRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, documentationFileRule.enabled);

            ruleElement orphanedProjectsRule = group.rules[5];
            Assert.IsNotNull(orphanedProjectsRule);
            Assert.AreEqual<string>("OrphanedProjects", orphanedProjectsRule.name);
            Assert.AreEqual<string>("BuildCop.Rules.OrphanedProjects.OrphanedProjectsRule", orphanedProjectsRule.type);
            Assert.AreEqual<string>(string.Empty, orphanedProjectsRule.excludedFiles);
            Assert.AreEqual<string>(string.Empty, orphanedProjectsRule.excludedOutputTypes);
            Assert.AreEqual<bool>(true, orphanedProjectsRule.enabled);
            Assert.IsNotNull(orphanedProjectsRule.solutions);
            Assert.AreEqual<string>("TestSearchPath", orphanedProjectsRule.solutions.searchPath);

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

            Assert.IsNotNull(config.formatters);
            Assert.AreEqual<int>(4, config.formatters.Count);

            formatterElement consoleFormatter = config.formatters[0];
            Assert.IsNotNull(consoleFormatter);
            Assert.AreEqual<string>("Console", consoleFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Console.ConsoleFormatter", consoleFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Warning, consoleFormatter.minimumLogLevel);

            formatterElement htmlFormatter = config.formatters[1];
            Assert.IsNotNull(htmlFormatter);
            Assert.AreEqual<string>("Html", htmlFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Html.HtmlFormatter", htmlFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Information, htmlFormatter.minimumLogLevel);
            Assert.AreEqual<string>("out.html", htmlFormatter.output.fileName);
            Assert.AreEqual<string>(string.Empty, htmlFormatter.output.stylesheet);
            Assert.AreEqual<bool>(false, htmlFormatter.output.launch);

            formatterElement xmlFormatter = config.formatters[2];
            Assert.IsNotNull(xmlFormatter);
            Assert.AreEqual<string>("Xml", xmlFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Xml.XmlFormatter", xmlFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Error, xmlFormatter.minimumLogLevel);
            Assert.AreEqual<string>("out.xml", xmlFormatter.output.fileName);
            Assert.AreEqual<string>("TestStylesheet.xslt", xmlFormatter.output.stylesheet);
            Assert.AreEqual<bool>(false, xmlFormatter.output.launch);

            formatterElement csvFormatter = config.formatters[3];
            Assert.IsNotNull(csvFormatter);
            Assert.AreEqual<string>("Csv", csvFormatter.name);
            Assert.AreEqual<string>("BuildCop.Formatters.Csv.CsvFormatter", csvFormatter.type);
            Assert.AreEqual<LogLevel>(LogLevel.Exception, csvFormatter.minimumLogLevel);
            Assert.AreEqual<string>("out.csv", csvFormatter.output.fileName);
            Assert.AreEqual<bool>(false, csvFormatter.output.launch);

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