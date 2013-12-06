<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="42371a6d-0484-4e82-bdd2-32961e8381ad" namespace="JelleDruyts.BuildCop.Configuration" xmlSchemaNamespace="http://schemas.jelle.druyts.net/BuildCop" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
    <enumeratedType name="LogLevel" namespace="JelleDruyts.BuildCop.Reporting" documentation="Specifies log levels for log entries.">
      <literals>
        <enumerationLiteral name="Information" value="0" documentation="The log entry is considered informational." />
        <enumerationLiteral name="Warning" value="1" documentation="The log entry is considered a warning." />
        <enumerationLiteral name="Error" value="2" documentation="The log entry is considered an error." />
        <enumerationLiteral name="Exception" value="3" documentation="The log entry is an exception." />
      </literals>
    </enumeratedType>
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="BuildCopConfiguration" documentation="The configuration settings for BuildCop." codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="buildCopConfiguration">
      <elementProperties>
        <elementProperty name="BuildGroups" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="buildGroups" isReadOnly="true" documentation="The build groups.">
          <type>
            <configurationElementCollectionMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/BuildGroupCollection" />
          </type>
        </elementProperty>
        <elementProperty name="SharedRules" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="sharedRules" isReadOnly="true" documentation="The shared rules.">
          <type>
            <configurationElementCollectionMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/RuleCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Formatters" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="formatters" isReadOnly="true" documentation="The formatters.">
          <type>
            <configurationElementCollectionMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/FormatterCollection" />
          </type>
        </elementProperty>
        <elementProperty name="OutputTypeMappings" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="outputTypeMappings" isReadOnly="true" documentation="The output type mappings.">
          <type>
            <configurationElementCollectionMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/OutputTypeCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="BuildGroupElement" documentation="Defines a build group.">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false" documentation="The name of this build group.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Enabled" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="enabled" isReadOnly="false" documentation="A value that determines if this build group is enabled." defaultValue="true">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="BuildFiles" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="buildFiles" isReadOnly="true" documentation="The build files in this build group.">
          <type>
            <configurationElementMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/BuildFilesElement" />
          </type>
        </elementProperty>
        <elementProperty name="Rules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="rules" isReadOnly="true" documentation="The rules in this build group.">
          <type>
            <configurationElementCollectionMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/RuleCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="BuildGroupCollection" xmlItemName="buildGroup" codeGenOptions="Indexer, AddMethod">
      <itemType>
        <configurationElementMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/BuildGroupElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="FormatterCollection" xmlItemName="formatter" codeGenOptions="Indexer, AddMethod">
      <itemType>
        <configurationElementMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/FormatterElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="FormatterElement" documentation="Defines a formatter for a BuildCop report." hasCustomChildElements="true">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false" documentation="The name of this formatter.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false" documentation="The type of this formatter.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MinimumLogLevel" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="minimumLogLevel" isReadOnly="false" documentation="The minimum log level this formatter should display." defaultValue="JelleDruyts.BuildCop.Reporting.LogLevel.Information">
          <type>
            <enumeratedTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/LogLevel" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="RuleCollection" xmlItemName="rule" codeGenOptions="Indexer, AddMethod">
      <itemType>
        <configurationElementMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/RuleElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="OutputTypeCollection" xmlItemName="outputType" codeGenOptions="Indexer, AddMethod">
      <itemType>
        <configurationElementMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/OutputTypeElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="OutputTypeElement" documentation="Defines an output type alias for a project type GUID.">
      <attributeProperties>
        <attributeProperty name="Alias" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="alias" isReadOnly="false" documentation="The alias of this output type.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ProjectTypeGuid" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="projectTypeGuid" isReadOnly="false" documentation="The project type GUID of this output type alias.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="RuleElement" documentation="Defines a rule." hasCustomChildElements="true">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false" documentation="The name of this rule.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false" documentation="The type of this rule.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ExcludedFiles" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="excludedFiles" isReadOnly="false" documentation="The string to find in the names of files to exclude for this rule.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ExcludedOutputTypes" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="excludedOutputTypes" isReadOnly="false" documentation="The string to find in the output type of files to exclude for this rule.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Enabled" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="enabled" isReadOnly="false" documentation="A value that determines if this rule is enabled." defaultValue="true">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="BuildFilesElement" documentation="Defines the build files to be processed.">
      <attributeProperties>
        <attributeProperty name="ExcludedFiles" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="excludedFiles" isReadOnly="false" documentation="The string to find in the names of files to exclude in the given root path.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Paths" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="paths" isReadOnly="true" documentation="The paths to the build files to be processed.">
          <type>
            <configurationElementCollectionMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/BuildFilePathCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="BuildFilePathCollection" collectionType="BasicMap" xmlItemName="path" codeGenOptions="Indexer, AddMethod">
      <itemType>
        <configurationElementMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/BuildFilePathElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="BuildFilePathElement" documentation="Defines a build file path.">
      <attributeProperties>
        <attributeProperty name="RootPath" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="rootPath" isReadOnly="false" documentation="The root path to look for build files.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SearchPattern" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="searchPattern" isReadOnly="false" documentation="The search string to match against the names of files to include in the given root path.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ExcludedFiles" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="excludedFiles" isReadOnly="false" documentation="The string to find in the names of files to exclude in the given root path.">
          <type>
            <externalTypeMoniker name="/42371a6d-0484-4e82-bdd2-32961e8381ad/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>