using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildCop.MsBuildTask
{
    public class BuildCopMsBuildTask : Task
    {
        public override bool Execute()
        {
            throw new NotImplementedException();
        }

        [Required]
        public IList<string> buildGroups { get; set; }
    }
}
