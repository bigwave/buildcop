using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BuildCop.Configuration
{
    /// <summary> 
    /// A helper class for configuration.
    /// </summary>
    internal static class ConfigurationHelper
    {
        private static Regex XmlnsExpression = new Regex(" xmlns=\".*?\"", RegexOptions.Multiline | RegexOptions.Compiled);

    }
}