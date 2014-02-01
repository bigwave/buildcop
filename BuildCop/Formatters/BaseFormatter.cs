////using System;
////using System.Collections.Generic;
////using System.Text;

////using BuildCop.Reporting;
////using BuildCop.Configuration;

////namespace BuildCop.Formatters
////{
////    /// <summary>
////    /// A base class for the different types of formatters for verification reports.
////    /// </summary>
////    public abstract class BaseFormatter
////    {
////        #region Properties

////        private readonly formatterElement configuration;

////        /// <summary>
////        /// Gets or sets the configuration for this rule.
////        /// </summary>
////        public formatterElement Configuration
////        {
////            get { return this.configuration; }
////        }

////        #endregion

////        #region Constructors

////        /// <summary>
////        /// Initializes a new instance of the <see cref="BaseFormatter"/> class.
////        /// </summary>
////        /// <param name="configuration">The configuration for this formatter.</param>
////        protected BaseFormatter(formatterElement configuration)
////        {
////            this.configuration = configuration;
////        }

////        #endregion


////    ////    #region GetConfiguration

////    ////    /// <summary>
////    ////    /// Gets the configuration as its original strongly typed instance.
////    ////    /// </summary>
////    ////    /// <typeparam name="TConfigurationType">The type of the configuration to return.</typeparam>
////    ////    /// <returns>The <see cref="Configuration"/> property typed as the requested configuration type.</returns>
////    ////    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
////    ////    protected TConfigurationType GetTypedConfiguration<TConfigurationType>() where TConfigurationType : FormatterConfigurationElement
////    ////    {
////    ////        if (this.configuration == null)
////    ////        {
////    ////            throw new InvalidOperationException("The configuration instance was null and could not be converted to the requested type: " + typeof(TConfigurationType).FullName);
////    ////        }

////    ////        TConfigurationType typedconfiguration = this.configuration as TConfigurationType;
////    ////        if (typedconfiguration == null)
////    ////        {
////    ////            throw new InvalidOperationException("The configuration instance could not be converted to the requested type: " + typeof(TConfigurationType).FullName);
////    ////        }

////    ////        return typedconfiguration;
////    ////    }

////    ////    #endregion
////    }
////}