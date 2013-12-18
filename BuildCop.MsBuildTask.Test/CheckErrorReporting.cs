﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildCop.MsBuildTask.Test
{
    [TestFixture]
    public class CheckErrorReporting
    {
        [Test]
        public void TestEmptyDefault()
        {
            FakeBuildEngine anEngine = new FakeBuildEngine();
            var buildCop = new BuildCop.MsBuildTask.BuildCopMsBuildTask();
            ((ITask)buildCop).BuildEngine = anEngine;
            buildCop.buildGroups = new TaskItem[1] { new TaskItem("Default") };
            Assert.IsTrue(buildCop.Execute());
        }

        [Test]
        public void TestOneError()
        {
            FakeBuildEngine anEngine = new FakeBuildEngine();
            var buildCop = new BuildCop.MsBuildTask.BuildCopMsBuildTask();
            ((ITask)buildCop).BuildEngine = anEngine;
            buildCop.buildGroups = new TaskItem[1] { new TaskItem("TestOneError") };
            Assert.IsFalse(buildCop.Execute());
        }
    }
}
