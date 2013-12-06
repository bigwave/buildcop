using System;
using System.Collections.Generic;
using System.Text;

namespace JelleDruyts.BuildCop
{
    /// <summary>
    /// An assembly reference with its correct hint path.
    /// </summary>
    public class AssemblyReference
    {
        #region Properties

        private readonly string assemblyName;

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string AssemblyName
        {
            get { return this.assemblyName; }
        }

        private readonly string hintPath;

        /// <summary>
        /// Gets the assembly reference hint path.
        /// </summary>
        public string HintPath
        {
            get { return this.hintPath; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyReference"/> class.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="hintPath">The assembly reference hint path.</param>
        public AssemblyReference(string assemblyName, string hintPath)
        {
            this.assemblyName = assemblyName;
            this.hintPath = hintPath;
        }

        #endregion
    }
}