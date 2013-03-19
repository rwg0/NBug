// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="NBusy Project">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using NBug.Enums;

namespace NBug.Examples.WinForms
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;

	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			// Check to see if test application is initialized by the configurator tool
			if (Environment.GetCommandLineArgs().Count() > 1)
			{
				var stream = new FileStream(Environment.GetCommandLineArgs()[1], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				NBug.Properties.SettingsOverride.LoadCustomSettings(stream);
			}

            NBug.Settings.Destination1 =
      "Type=HTTP;Url=http://errors.sharpcap.co.uk/upload.php;";
            NBug.Settings.ExitApplicationImmediately = true;
            NBug.Settings.SleepBeforeSend = 5;
            NBug.Settings.StopReportingAfter = 90;
          //  NBug.Settings.ReleaseMode = true;
            NBug.Settings.UIMode = UIMode.Full;
            NBug.Settings.WriteLogToDisk = false;
            NBug.Settings.StoragePath = StoragePath.WindowsTemp;
            NBug.Settings.TrySendingBeforeExit = true;
            //NBug.Settings.Destination1 =
            //       "Type=HTTP;Url=http://www.sharpcap.webspace.virginmedia.com/upload.php;";
            //NBug.Settings.ExitApplicationImmediately = false;
            //NBug.Settings.SleepBeforeSend = 5;
            //NBug.Settings.StopReportingAfter = 90;
            //// For demonstrational purposes only, normally this should be left with it's default value as false!
            //NBug.Settings.HandleProcessCorruptedStateExceptions = true;
            //NBug.Settings.UIMode = UIMode.Full;
            ////NBug.Settings.ReleaseMode = true;
		    

			// Sample NBug configuration for WinForms applications
			AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
			Application.ThreadException += NBug.Handler.ThreadException;
			System.Threading.Tasks.TaskScheduler.UnobservedTaskException += NBug.Handler.UnobservedTaskException;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
