using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using BuildCop.Configuration;
using BuildCop.Reporting;
using BuildCop.Rules;

namespace BuildCop.Configuration
{
    /// <summary>
    /// A rule that checks naming conventions for a project.
    /// </summary>
    [BuildCopRuleAttribute(ConfigurationType = typeof(ruleElementBuildProperty))]
    public partial class ruleElementBuildProperty 
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ruleElementBuildProperty"/> class.
        /// </summary>
        public ruleElementBuildProperty()
        {
        }

        #endregion
   }
}