using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace BuildCop.MsBuildTask.Test
{
    [TestFixture]
    public class CheckErrorReporting
    {
        [Test]
        public void Test()
        {
            FakeBuildEngine anEngine = new FakeBuildEngine();
            var buildCop = new BuildCop.MsBuildTask.BuildCopMsBuildTask();
            ((ITask)buildCop).BuildEngine = anEngine;
            Assert.IsTrue(buildCop.Execute());
        }
    }
}
