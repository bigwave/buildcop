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
                foreach (LogEntry item in theReport.BuildGroupReports.Select(m => m.BuildFileReports.Select(n => n.FindLogEntries(LogLevel.Error))))
                {
                    Log.LogMessage(MessageImportance.High, item.Code + " " + item.Detail + " " + item.Code + " " + item.Level + " " + item.Message + " " + item.Rule);
                }

                return false;
            }

            return true;
        }

        public TaskItem[] buildGroups { get; set; }
    }
}
