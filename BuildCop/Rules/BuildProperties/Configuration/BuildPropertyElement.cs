using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using BuildCop.Configuration;

namespace BuildCop.Rules.BuildProperties.Configuration
{
    /// <summary>
    /// Defines the property value configuration.
    /// </summary>
    public class BuildPropertyElement : ConfigurationElement
    {
        #region Constants

        private const string NamePropertyName = "name";
        private const string ValuePropertyName = "value";
        private const string ConditionPropertyName = "condition";
        private const string CompareOptionPropertyName = "compareOption";
        private const string StringCompareOptionPropertyName = "stringCompareOption";
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the property to check.
        /// </summary>
        [ConfigurationProperty(NamePropertyName, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base[NamePropertyName];
            }
            set
            {
                base[NamePropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the expected value of the property to check.
        /// </summary>
        [ConfigurationProperty(ValuePropertyName, IsRequired = false)]
        public string Value
        {
            get
            {
                return (string)base[ValuePropertyName];
            }
            set
            {
                base[ValuePropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the condition (or a part of it) that should be present in the build property's condition.
        /// </summary>
        [ConfigurationProperty(ConditionPropertyName, IsRequired = false)]
        public string Condition
        {
            get
            {
                return (string)base[ConditionPropertyName];
            }
            set
            {
                base[ConditionPropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the comparison option to use when checking build property values.
        /// </summary>
        [ConfigurationProperty(CompareOptionPropertyName, IsRequired = false, DefaultValue = CompareOption.EqualTo)]
        public CompareOption CompareOption
        {
            get
            {
                return (CompareOption)base[CompareOptionPropertyName];
            }
            set
            {
                base[CompareOptionPropertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the comparison option to use when comparing strings.
        /// </summary>
        [ConfigurationProperty(StringCompareOptionPropertyName, IsRequired = false, DefaultValue = StringComparison.Ordinal)]
        public StringComparison StringCompareOption
        {
            get
            {
                return (StringComparison)base[StringCompareOptionPropertyName];
            }
            set
            {
                base[StringCompareOptionPropertyName] = value;
            }
        }

        #endregion
    }
}