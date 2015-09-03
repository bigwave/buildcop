using System.Text.RegularExpressions;

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