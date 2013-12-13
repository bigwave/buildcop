using System;
using System.Collections.Generic;
using System.Text;

namespace BuildCop
{
    /// <summary>
    /// Represents a property in a build file.
    /// </summary>
    public class BuildProperty
    {
        #region Properties

        private readonly string name;

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        private readonly string value;

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }

        private readonly string condition;

        /// <summary>
        /// Gets the condition of the property.
        /// </summary>
        public string Condition
        {
            get { return this.condition; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProperty"/> class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="condition">The condition of the property.</param>
        public BuildProperty(string name, string value, string condition)
        {
            this.name = name;
            this.value = value;
            this.condition = condition;
        }

        #endregion
    }
}