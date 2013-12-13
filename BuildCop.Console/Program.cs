using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BuildCop;

namespace BuildCop.Console
{
    /// <summary>
    /// Program is the entry point class of the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main is the entry point of the application.
        /// </summary>
        /// <param name="args">contains the program arguments</param>
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    BuildCopEngine.Execute();
                }
                else
                {
                    BuildCopEngine.Execute(args);
                }
            }
            catch (Exception exc)
            {
                System.Console.WriteLine(string.Format("{2}{0}: {1}{2}{3}", exc.GetType().Name, exc.Message, Environment.NewLine, exc.StackTrace));
            }
        }
    }
}