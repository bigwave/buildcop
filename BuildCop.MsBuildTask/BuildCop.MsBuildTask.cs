using BuildCop.Configuration;
using BuildCop.Reporting;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            List<TaskItem> exceptionList = new List<TaskItem>();
            List<TaskItem> errorList = new List<TaskItem>();
            List<TaskItem> warningList = new List<TaskItem>();
            List<TaskItem> informationList = new List<TaskItem>();

            if (errorCount > 0)
            {
                /// TODO - How do you do this in Linq?
                foreach (BuildGroupReport aBuildGroupReport in theReport.BuildGroupReports)
                {
                    foreach (BuildFileReport aBuildFileReport in aBuildGroupReport.BuildFileReports)
                    {
                        foreach (LogEntry aLogEntry in aBuildFileReport.FindLogEntries(LogLevel.Information))
                        {
                            string message = aLogEntry.Level + " " + aBuildFileReport.FileName + " " + aLogEntry.Detail;
                            Log.LogMessage(MessageImportance.High, message);

                            switch (aLogEntry.Level)
                            {
                                case LogLevel.Exception:
                                    exceptionList.Add(new TaskItem(message));
                                    break;
                                case LogLevel.Error:
                                    errorList.Add(new TaskItem(message));
                                    break;
                                case LogLevel.Warning:
                                    warningList.Add(new TaskItem(message));
                                    break;
                                default:
                                    informationList.Add(new TaskItem(message));
                                    break;
                            }

                        }
                    }
                }

            }

            Exceptions = exceptionList.ToArray();
            Errors = errorList.ToArray();
            Warnings = warningList.ToArray();
            Information = informationList.ToArray();

            if (errorCount > 0)
            {
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
