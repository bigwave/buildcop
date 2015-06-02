using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace BuildCop
{
    /// <summary>
    /// Represents an MSBuild project file.
    /// </summary>
    public class BuildFile
    {
        #region Properties

        private string fileName;

        /// <summary>
        /// Gets the name of the project file.
        /// </summary>
        public string FileName
        {
            get { return this.fileName; }
        }

        private string path;

        /// <summary>
        /// Gets the full path of the project file.
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }

        private IList<AssemblyReference> assemblyReferences;

        /// <summary>
        /// Gets the assembly references.
        /// </summary>
        public IList<AssemblyReference> AssemblyReferences
        {
            get { return this.assemblyReferences; }
        }

        private IList<BuildProperty> properties;

        /// <summary>
        /// Gets the build properties.
        /// </summary>
        public IList<BuildProperty> Properties
        {
            get { return this.properties; }
        }

        private IList<string> conditions;

        /// <summary>
        /// Gets the build conditions.
        /// </summary>
        public IList<string> Conditions
        {
            get { return this.conditions; }
        }

        #endregion

        #region Derived Properties

        private bool signAssembly;

        /// <summary>
        /// Gets a value indicating whether the assembly should be signed.
        /// </summary>
        public bool SignAssembly
        {
            get { return this.signAssembly; }
        }

        private string assemblyOriginatorKeyFile;

        /// <summary>
        /// Gets the assembly originator key file.
        /// </summary>
        public string AssemblyOriginatorKeyFile
        {
            get { return this.assemblyOriginatorKeyFile; }
        }

        private string rootNamespace;

        /// <summary>
        /// Gets the root namespace.
        /// </summary>
        public string RootNamespace
        {
            get { return this.rootNamespace; }
        }

        private string assemblyName;

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string AssemblyName
        {
            get { return this.assemblyName; }
        }

        private string outputType;

        /// <summary>
        /// Gets the output type.
        /// </summary>
        public string OutputType
        {
            get { return this.outputType; }
        }

        private string projectTypeGuids;

        /// <summary>
        /// Gets the project's type GUIDs.
        /// </summary>
        public string ProjectTypeGuids
        {
            get { return this.projectTypeGuids; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildFile"/> class.
        /// </summary>
        /// <param name="fileName">The name of the build file.</param>
        public BuildFile(string fileName)
            : this(fileName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildFile"/> class.
        /// </summary>
        /// <param name="fileName">The name of the build file.</param>
        /// <param name="delayParse">A value that indicates if the file should be parsed immediately.</param>
        public BuildFile(string fileName, bool delayParse)
        {
            this.fileName = fileName;
            this.path = System.IO.Path.GetFullPath(fileName);
            if (!delayParse)
            {
                Parse();
            }
        }

        #endregion

        #region Parse

        /// <summary>
        /// Parses the build file and populates the properties of this instance.
        /// </summary>
        public void Parse()
        {
            // Prepare.
            XmlDocument projectDocument = new XmlDocument();
            projectDocument.Load(fileName);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(projectDocument.NameTable);
            nsmgr.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

            // Find assembly references.
            this.assemblyReferences = new List<AssemblyReference>();
            foreach (XmlNode referenceNode in projectDocument.SelectNodes("/msb:Project/msb:ItemGroup/msb:Reference", nsmgr))
            {
                XmlAttribute includeAttribute = referenceNode.Attributes["Include"];
                if (includeAttribute != null && includeAttribute.InnerText != null)
                {
                    string referenceName = includeAttribute.InnerText;
                    XmlNode hintPathNode = referenceNode.SelectSingleNode("msb:HintPath", nsmgr);
                    if (hintPathNode != null && hintPathNode.InnerText != null)
                    {
                        string referenceHintPath = hintPathNode.InnerText;
                        this.assemblyReferences.Add(new AssemblyReference(referenceName, referenceHintPath));
                    }
                }
            }

            // Find all properties.
			this.properties = new List<BuildProperty>();
			this.conditions = new List<string>();
			foreach (XmlNode propertyGroupNode in projectDocument.SelectNodes("/msb:Project/msb:PropertyGroup", nsmgr))
            {
                if (propertyGroupNode.HasChildNodes)
                {
                    string condition = null;
                    XmlAttribute conditionAttribute = propertyGroupNode.Attributes["Condition"];
                    if (conditionAttribute != null && !string.IsNullOrEmpty(conditionAttribute.Value))
                    {
                        condition = conditionAttribute.Value;

						if (!this.conditions.Contains(condition))
						{
							this.conditions.Add(condition);
						}
                    }
                    foreach (XmlNode propertyNode in propertyGroupNode.ChildNodes)
                    {
                        BuildProperty property = new BuildProperty(propertyNode.Name, propertyNode.InnerXml, condition);
                        this.properties.Add(property);
                    }
                }
            }

            // Derive certain basic properties.
            this.signAssembly = GetPropertyValueAsBoolean("SignAssembly", false);
            this.assemblyOriginatorKeyFile = GetPropertyValue("AssemblyOriginatorKeyFile");
            this.rootNamespace = GetPropertyValue("RootNamespace");
            this.assemblyName = GetPropertyValue("AssemblyName");
            this.outputType = GetPropertyValue("OutputType");
            this.projectTypeGuids = GetPropertyValue("ProjectTypeGuids");
        }

        #endregion

        #region GetPropertyValue

        /// <summary>
        /// Finds the first build property with the given name and returns its value as a boolean.
        /// </summary>
        /// <param name="name">The name of the build property to find.</param>
        /// <param name="defaultValue">The value to return if the property could not be found.</param>
        /// <returns>The value of the first property with the given name.</returns>
        /// <exception cref="ArgumentException">The property with the given name could not be found.</exception>
        public bool GetPropertyValueAsBoolean(string name, bool defaultValue)
        {
            return GetPropertyValueAsBoolean(name, null, defaultValue);
        }

        /// <summary>
        /// Finds the first build property with the given name and returns its value as a boolean.
        /// </summary>
        /// <param name="name">The name of the build property to find.</param>
        /// <param name="conditionSubstring">The condition (or a part of it) that should be present in the build property's condition.</param>
        /// <param name="defaultValue">The value to return if the property could not be found.</param>
        /// <returns>The value of the first property with the given name that matches the given condition.</returns>
        public bool GetPropertyValueAsBoolean(string name, string conditionSubstring, bool defaultValue)
        {
            string value = GetPropertyValue(name, conditionSubstring);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return bool.Parse(value);
            }
        }

        /// <summary>
        /// Finds the first build property with the given name and returns its value.
        /// </summary>
        /// <param name="name">The name of the build property to find.</param>
        /// <returns>The value of the first property with the given name, or <see langword="null"/> if it could not be found.</returns>
        public string GetPropertyValue(string name)
        {
            return GetPropertyValue(name, null);
        }

        /// <summary>
        /// Finds the first build property with the given name and returns its value.
        /// </summary>
        /// <param name="name">The name of the build property to find.</param>
        /// <param name="conditionSubstring">The condition (or a part of it) that should be present in the build property's condition.</param>
        /// <returns>The value of the first property with the given name that matches the given condition, or <see langword="null"/> if it could not be found.</returns>
        public string GetPropertyValue(string name, string conditionSubstring)
        {
            IList<BuildProperty> foundProperties = FindProperties(name, conditionSubstring);
            if (foundProperties.Count == 0)
            {
                return null;
            }
            else
            {
                return foundProperties[0].Value;
            }
        }

        /// <summary>
        /// Finds all build properties with the given name.
        /// </summary>
        /// <param name="name">The name of the build property to find.</param>
        /// <returns>The properties with the given name that match the given condition.</returns>
        public IList<BuildProperty> FindProperties(string name)
        {
            return FindProperties(name, null);
        }

        /// <summary>
        /// Finds all build properties with the given name.
        /// </summary>
        /// <param name="name">The name of the build property to find.</param>
        /// <param name="conditionSubstring">The condition (or a part of it) that should be present in the build property's condition.</param>
        /// <returns>The properties with the given name that match the given condition.</returns>
        public IList<BuildProperty> FindProperties(string name, string conditionSubstring)
        {
            // Validate arguments.
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("The property name is a required argument.", "name");
            }

            IList<BuildProperty> foundProperties = new List<BuildProperty>();

            // Search through all properties.
            foreach (BuildProperty property in this.properties)
            {
                if (string.Equals(name, property.Name, StringComparison.Ordinal))
                {
                    // We have a matching name, check the condition (if given).
                    if (string.IsNullOrEmpty(conditionSubstring) || (property.Condition != null && property.Condition.IndexOf(conditionSubstring, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        foundProperties.Add(property);
                    }
                }
            }

            return foundProperties;
        }

        #endregion
    }
}