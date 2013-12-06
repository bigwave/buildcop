using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JelleDruyts.BuildCop.Test
{
    [TestClass]
    public class BuildFileTest
    {
        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildFileConstructorShouldInitializePropertiesBasic()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            Assert.AreEqual<string>(Path.Combine(Environment.CurrentDirectory, @"BuildFiles\DefaultConsoleApplication.csproj"), file.Path);
            Assert.AreEqual<string>("DefaultConsoleApplication", file.AssemblyName);
            Assert.AreEqual<string>(null, file.AssemblyOriginatorKeyFile);
            Assert.AreEqual<string>("Exe", file.OutputType);
            Assert.AreEqual<string>(@"BuildFiles\DefaultConsoleApplication.csproj", file.FileName);
            Assert.AreEqual<string>("DefaultConsoleApplication", file.RootNamespace);
            Assert.AreEqual<bool>(false, file.SignAssembly);
            Assert.IsNotNull(file.AssemblyReferences);
            Assert.AreEqual<int>(0, file.AssemblyReferences.Count);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildFileConstructorShouldInitializePropertiesExtended()
        {
            BuildFile file = new BuildFile(@"BuildFiles\SignedConsoleApplication.csproj");
            Assert.AreEqual<string>("SignedConsoleApplication", file.AssemblyName);
            Assert.AreEqual<string>(@"C:\MyKey.snk", file.AssemblyOriginatorKeyFile);
            Assert.AreEqual<string>("Exe", file.OutputType);
            Assert.AreEqual<string>(@"BuildFiles\SignedConsoleApplication.csproj", file.FileName);
            Assert.AreEqual<string>("SignedConsoleApplication", file.RootNamespace);
            Assert.AreEqual<bool>(true, file.SignAssembly);
            Assert.IsNotNull(file.AssemblyReferences);
            Assert.AreEqual<int>(1, file.AssemblyReferences.Count);
            AssemblyReference reference = file.AssemblyReferences[0];
            Assert.AreEqual<string>(@"EnvDTE, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", reference.AssemblyName);
            Assert.AreEqual<string>(@"X:\References\Microsoft.Practices.RecipeFramework\1.0.60429.0\EnvDTE.dll", reference.HintPath);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        public void BuildFileConstructorShouldInitializePropertiesCollection()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            Assert.IsNotNull(file.Properties);
            Assert.AreEqual<int>(22, file.Properties.Count);

            // Test properties.
            IList<BuildProperty> properties;
            properties = file.FindProperties("DebugType");
            Assert.AreEqual<int>(2, properties.Count);
            Assert.AreEqual<string>("full", properties[0].Value);
            Assert.AreEqual<string>("pdbonly", properties[1].Value);

            properties = file.FindProperties("DebugType", "debug");
            Assert.AreEqual<int>(1, properties.Count);
            Assert.AreEqual<string>("full", properties[0].Value);

            // Test regular property values.
            Assert.AreEqual<string>("Debug", file.GetPropertyValue("Configuration"));
            Assert.AreEqual<string>("AnyCPU", file.GetPropertyValue("Platform"));
            Assert.AreEqual<string>("8.0.50727", file.GetPropertyValue("ProductVersion"));
            Assert.AreEqual<string>("8.0.50727", file.GetPropertyValue("ProductVersion", null));
            Assert.AreEqual<string>("8.0.50727", file.GetPropertyValue("ProductVersion", ""));

            // Test invalid property values, e.g. because of case sensitivity.
            Assert.IsNull(file.GetPropertyValue("configuration"));
            Assert.IsNull(file.GetPropertyValue("Configuration "));

            // Test invalid conditions.
            Assert.IsNull(file.GetPropertyValue("ProductVersion", "dummy"));
            Assert.AreEqual<bool>(true, file.GetPropertyValueAsBoolean("DebugSymbols", "dummy", true));
            Assert.AreEqual<bool>(false, file.GetPropertyValueAsBoolean("DebugSymbols", "dummy", false));

            // Test valid conditions.
            Assert.AreEqual<string>("true", file.GetPropertyValue("DebugSymbols", "debug"));
            Assert.AreEqual<bool>(true, file.GetPropertyValueAsBoolean("DebugSymbols", "debug", false));
            Assert.AreEqual<string>("full", file.GetPropertyValue("DebugType", "debug"));
            Assert.AreEqual<string>("pdbonly", file.GetPropertyValue("DebugType", "release"));
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPropertyValueShouldThrowOnNullName()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            file.GetPropertyValue(null);
        }

        [TestMethod]
        [DeploymentItem("BuildFiles", "BuildFiles")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPropertyValueShouldThrowOnEmptyName()
        {
            BuildFile file = new BuildFile(@"BuildFiles\DefaultConsoleApplication.csproj");
            file.GetPropertyValue("");
        }
    }
}