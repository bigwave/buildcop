﻿using NUnit.Framework;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildCop.MsBuildTask.Test
{
    [TestFixture]
    public class CheckErrorReporting
    {
        [Test]
        public void Execute_WithMinimalConfig_ReturnsTrue()
        {
            FakeBuildEngine anEngine = new FakeBuildEngine();
            var buildCop = new BuildCopMsBuildTask();
            ((ITask)buildCop).BuildEngine = anEngine;
            buildCop.buildGroups = new TaskItem[1] { new TaskItem("MinimalConfig") };
            Assert.IsTrue(buildCop.Execute());
            Assert.That(buildCop.Errors.Length, Is.EqualTo(0));
        }

        [Test]
        public void Execute_WithDefaultNewVisualStudioProjectAndSampleBuildCopConfig_FailsOnTreatErrorsAsWarnings()
        {
            FakeBuildEngine anEngine = new FakeBuildEngine();
            var buildCop = new BuildCopMsBuildTask();
            ((ITask)buildCop).BuildEngine = anEngine;
            buildCop.buildGroups = new TaskItem[1] { new TaskItem("DefaultNewVisualStudioProjectAndSampleBuildCopConfig") };
            Assert.IsFalse(buildCop.Execute());
            Assert.That(buildCop.Errors.Length, Is.EqualTo(1));
            Assert.That(buildCop.Errors[0].ItemSpec, Is.EqualTo("Error .\\VisualStudioNewProject.proj The build property \"TreatWarningsAsErrors\" does not exist in the build file."));
        }
    }
}
