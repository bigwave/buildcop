using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using BuildCop.Rules;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a rule.
    /// </summary>
    public partial class RuleElement : ConfigurationElement
    {
        #region Rule-Specific Configuration Handling

        /// <summary>
        /// Gets a value indicating whether an unknown element is encountered during deserialization.
        /// </summary>
        /// <param name="elementName">The name of the unknown subelement.</param>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"></see> object being used for deserialization.</param>
        /// <returns>true when an unknown element is encountered while deserializing.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "elementName")]
        private bool HandleUnrecognizedElement(string elementName, XmlReader reader)
        {
            // Determine the rule type.
            System.Type ruleType = System.Type.GetType(this.Type, true, true);
            if (!typeof(BaseRule).IsAssignableFrom(ruleType))
            {
                throw new ConfigurationErrorsException("The rule type must derive from the BaseRule class. Type name: " + this.Type);
            }

            // Find the BuildCopRule attribute to determine the rule's configuration type.
            object[] attributes = ruleType.GetCustomAttributes(typeof(BuildCopRuleAttribute), true);
            if (attributes.Length != 1)
            {
                throw new ConfigurationErrorsException("The rule type must have the BuildCopRuleAttribute applied. Type name: " + this.Type);
            }
            BuildCopRuleAttribute ruleAttribute = (BuildCopRuleAttribute)attributes[0];
            System.Type configType = ruleAttribute.ConfigurationType;
            if (configType != null)
            {
                this.ruleConfiguration = ConfigurationHelper.ReadSpecificConfigurationElement<RuleConfigurationElement>(reader, configType);
            }

            return true;
        }

        private RuleConfigurationElement ruleConfiguration;

        /// <summary>
        /// Gets the rule-specific configuration element for this element.
        /// </summary>
        public RuleConfigurationElement RuleConfiguration
        {
            get
            {
                return this.ruleConfiguration;
            }
        }

        #endregion
    }
}