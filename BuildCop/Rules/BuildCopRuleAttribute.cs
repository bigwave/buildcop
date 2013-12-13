using System;
using System.Collections.Generic;
using System.Text;

namespace BuildCop.Rules
{
    /// <summary>
    /// Specifies that a class represents a BuildCop rule.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public sealed class BuildCopRuleAttribute : Attribute
    {
        private Type configurationType;

        /// <summary>
        /// Gets or sets the type of the configuration class.
        /// </summary>
        public Type ConfigurationType
        {
            get { return this.configurationType; }
            set { this.configurationType = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildCopRuleAttribute"/> class.
        /// </summary>
        public BuildCopRuleAttribute()
        {
        }
    }
}