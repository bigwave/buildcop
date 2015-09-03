using System;

namespace BuildCop.Formatters
{
    /// <summary>
    /// Specifies that a class represents a BuildCop formatter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public sealed class BuildCopFormatterAttribute : Attribute
    {
        private Type configurationType;

        /// <summary>
        /// Gets or sets the type of the configuration class.
        /// </summary>
        public Type ConfigurationType
        {
            get { return configurationType; }
            set { configurationType = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildCopFormatterAttribute"/> class.
        /// </summary>
        public BuildCopFormatterAttribute()
        {
        }
    }
}