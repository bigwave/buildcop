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

        /// <summary>
        /// Creates a specific configuration element from the given XML reader.
        /// </summary>
        /// <typeparam name="TConfigurationType">The general type of configuration element to read (base class).</typeparam>
        /// <param name="reader">The reader from which to read.</param>
        /// <param name="specificConfigurationType">The specific type of configuration element to read (actual type).</param>
        /// <returns>The specific configuration element that was read.</returns>
        public static TConfigurationType ReadSpecificConfigurationElement<TConfigurationType>(XmlReader reader, Type specificConfigurationType) where TConfigurationType : BuildCopBaseElement
        {
            if (!typeof(TConfigurationType).IsAssignableFrom(specificConfigurationType))
            {
                throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, "The configuration type must derive from the {0} class. Type name: {1}", typeof(TConfigurationType).FullName, specificConfigurationType.FullName));
            }

            // Create a new XmlReader for the unrecognised element.
            // This is necessary because there is no wrapping element for the configuration element.
            // So we create a new XmlReader that uses the *outer* XML for the current element, which
            // will then act as the wrapping element.
            // We could also use reader.ReadSubtree() but then we can't remove the xmlns.
            string outerXml = reader.ReadOuterXml();

            // Strip out any xmlns attribute.
            outerXml = XmlnsExpression.Replace(outerXml, string.Empty);
            MemoryStream memStream = null;
            try
            {
                memStream = new MemoryStream();
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(memStream);

                    writer.Write(outerXml);
                    writer.Flush();
                    memStream.Position = 0;

                    using (XmlReader outerReader = XmlReader.Create(memStream))
                    {
                        memStream = null;
                        writer = null;
                        // Create the configuration type, passing in the new XmlReader.
                        ConstructorInfo ctor = specificConfigurationType.GetConstructor(new Type[] { typeof(XmlReader) });
                        if (ctor == null)
                        {
                            throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, "The configuration type must have a constructor that takes an XmlReader. Type name: {0}", specificConfigurationType.FullName));
                        }
                        TConfigurationType configuration = (TConfigurationType)ctor.Invoke(new object[] { outerReader });
                        return configuration;
                    }
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Dispose();
                        memStream = null;
                    }
                }
            }
            finally
            {
                if (memStream != null)
                {
                    memStream.Dispose();
                }
            }
        }
    }
}