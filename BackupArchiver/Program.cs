using System;
using System.Collections.Generic;
using System.Diagnostics;
using BackupLibrary;
using System.Reflection;

namespace BackupArchiver
{
    class Program
    {
        static void Main(string[] args)
        {
            //Application.ThreadException += new ThreadExceptionEventHandler(AppThreadException);
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomainException);
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve); 

            Display.SetWindowStyle(ProcessWindowStyle.Hidden);

            // init config file (and current log file)
            Config config = new Config(args);
            if (!config.IsSetConfig)
            {
                //Log.Add("Config file is not set. Exit..");
                OnFinish(config);
                return;
            }

            //ProcessWindowStyle windowStyle = config.WindowStyle;
            Display.SetWindowStyle(config.WindowStyle);

            // check 7zip existing
            Zip.Init(config.ZipPath);
            if (!Zip.IsExist)
            {
                OnFinish(config);
                //Log.Add("'7zip' is missing! Exit..");
                //Console.ReadLine();
                return;
            }
            //else Log.Add("'7zip' on base");

            // archiving
            List<Folder> folders = config.Folders;
            if (folders != null)
            {
                foreach (Folder elem in folders)
                {
                    Log.AddLine();
                    string archiveName = elem.HeadMask + String.Format(config.TailMask, DateTime.Now);
                    if (Zip.AddTo_Zip_R_Y_Archive(elem.SourcePath, elem.DestPath, archiveName, config.ZipArgs) == ZipCodes.NoError.Value)
                        // delete old archives if no errors
                        FileSystem.DeleteOldFiles(elem.DestPath, elem.MaxArchives, elem.HeadMask + "*");
                    else Log.Add("Deleting old files in [{0}] will not be", elem.DestPath);
                }
            }
            else Log.Add("Error in config ([Folders] == null");

            // delete extra log files
            //Log.AddLine();
            //FileSystem.DeleteOldFiles(config.LogPath, config.MaxLogs, "*.log");
            
            //Log.Add("Finish");
            OnFinish(config);
        }

        static void OnFinish(Config config)
        {
            if (config == null) return;
            Log.AddLine();
            FileSystem.DeleteOldFiles(config.LogPath, config.MaxLogs, "*.log");
            Log.Add("Finish");
            //
            ProcessWindowStyle windowStyle = config.WindowStyle;
            if ((windowStyle == ProcessWindowStyle.Normal || windowStyle == ProcessWindowStyle.Maximized) && config.ReadKeyInFinish)
            {
                Console.WriteLine("Press any key to exit..");
                Console.ReadKey();
            }
        }

        //private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    return typeof(MyType).Assembly;
        //}

        //static void AppThreadException(object sender, ThreadExceptionEventArgs e)
        //{
        //  if (e.Exception is System.DllNotFoundException)
        //  {
        //      Console.WriteLine("AppThreadException");
        //      //Log.Add("AppThreadException");
        //  }
        //}

        //static void AppDomainException(object sender, UnhandledExceptionEventArgs e)
        //{
        //  if ((Exception)e.ExceptionObject is System.DllNotFoundException)
        //  {
        //      Console.WriteLine("AppDomainException");
        //      //Log.Add("AppDomainException");
        //  }
        //}
    }
}
