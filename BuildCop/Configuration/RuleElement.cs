
using BuildCop.Rules;
using System.Text.RegularExpressions;

namespace BuildCop.Configuration
{
    /// <summary>
    /// Defines a rule. 
    /// </summary>
    public partial class ruleElement 
    {
        private static Regex SolutionProjectExpression = new Regex("Project\\(\"(?<TypeGuid>.*?)\"\\)\\s*=\\s*\"(?<ProjectName>.*?)\",\\s*\"(?<ProjectFileName>.*?)\",\\s*\"(?<ProjectGuid>.*?)\"", RegexOptions.Multiline | RegexOptions.Compiled);

        private BaseRule myRule;
        internal BaseRule RuleChecker
        {
            get
            {
                if (myRule != null)
                {
                    return myRule;

                }

                switch (type)
                {
                    case "BuildCop.Rules.BuildProperties.BuildPropertiesRule":
                        myRule = new BuildPropertiesRule(this);
                        break;
                    case "BuildCop.Rules.DocumentationFile.DocumentationFileRule":
                        myRule = new DocumentationFileRule(this);
                        break;
                    case "BuildCop.Rules.AssemblyReferences.AssemblyReferenceRule":
                        myRule = new AssemblyReferenceRule(this);
                        break;
                    case "BuildCop.Rules.NamingConventions.NamingConventionsRule":
                        myRule = new NamingConventionsRule(this);
                        break;
                    case "BuildCop.Rules.OrphanedProjects.OrphanedProjectsRule":
                        myRule = new OrphanedProjectsRule(this);
                        break;
                    case "BuildCop.Rules.StrongNaming.StrongNamingRule":
                        myRule = new StrongNamingRule(this);
                        break;
                    default:
                        myRule = null;
                        break;
                }
                return myRule;
            }
            set
            {
                myRule = value;
            }
        }


   }
}