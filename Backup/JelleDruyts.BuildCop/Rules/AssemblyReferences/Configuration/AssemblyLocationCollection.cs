using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace JelleDruyts.BuildCop.Rules.AssemblyReferences.Configuration
{
    /// <summary>
    /// Defines a collection of <see cref="AssemblyLocationElement"/> instances.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class AssemblyLocationCollection : ConfigurationElementCollection
    {
        #region Constants

        private const string AssemblyLocationPropertyName = "assemblyLocation";

        #endregion

        #region Indexer

        /// <summary>
        /// Gets the <see cref="AssemblyLocationElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="AssemblyLocationElement"/> to retrieve.</param>
        public AssemblyLocationElement this[int index]
        {
            get { return (AssemblyLocationElement)this.BaseGet(index); }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Configuration.ConfigurationElementCollectionType"></see> of this collection.</returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }

        /// <summary>
        /// Indicates whether the specified <see cref="T:System.Configuration.ConfigurationElement"></see> exists in the <see cref="T:System.Configuration.ConfigurationElementCollection"></see>.
        /// </summary>
        /// <param name="elementName">The name of the element to verify.</param>
        /// <returns>
        /// true if the element exists in the collection; otherwise, false. The default is false.
        /// </returns>
        protected override bool IsElementName(string elementName)
        {
            return (elementName == AssemblyLocationPropertyName);
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"></see> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"></see>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyLocationElement)element).AssemblyName;
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"></see>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"></see>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyLocationElement();
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds the specified asssembly location.
        /// </summary>
        /// <param name="asssemblyLocation">The asssembly location.</param>
        public void Add(AssemblyLocationElement asssemblyLocation)
        {
            base.BaseAdd(asssemblyLocation);
        }

        #endregion
    }
}