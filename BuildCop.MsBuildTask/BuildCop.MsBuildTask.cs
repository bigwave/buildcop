using BuildCop.Reporting;
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
            BuildCopReport theReport;

            if (buildGroups == null || buildGroups.Length == 0)
            {
                theReport = BuildCopEngine.Execute();
            }
            else
            {
                theReport = BuildCopEngine.Execute(buildGroups.Select(m => m.ItemSpec).ToArray<string>());
            }

            int errorCount = theReport.BuildGroupReports.Sum(m => m.BuildFileReports.Sum(n => n.FindLogEntries(LogLevel.Error).Count));

            if (errorCount > 0)
            {
                foreach (BuildGroupReport aBuildGroupReport in theReport.BuildGroupReports)
                {
                    foreach (BuildFileReport aBuildFileReport in aBuildGroupReport.BuildFileReports)
                    {
                        foreach (LogEntry aLogEntry in aBuildFileReport.FindLogEntries(LogLevel.Information))
                        {
                            Log.LogMessage(MessageImportance.High, aLogEntry.Level + " " + aLogEntry.Detail);
                        }
                    }
                }

                return false;
            }

            return true;
        }

        public TaskItem[] buildGroups { get; set; }

        [Output]
        public ITaskItem[] Exceptions { get; private set; }

        [Output]
        public ITaskItem[] Errors { get; private set; }

        [Output]
        public ITaskItem[] Warnings { get; private set; }

        [Output]
        public ITaskItem[] Information { get; private set; }
    }
}
