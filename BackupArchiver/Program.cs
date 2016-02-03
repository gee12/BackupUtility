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
            Display.SetWindowStyle(ProcessWindowStyle.Hidden);

            // init config file (and current log file)
            BackupArchiverConfig config = new BackupArchiverConfig(args);
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
                int i = 1;
                foreach (Folder folder in folders)
                {
                    Log.AddLine();
                    Log.Add(">>> Task {0}", i);

                    string archiveName = folder.HeadMask + String.Format(config.TailMask, DateTime.Now);
                    if (Zip.AddToZipArchive(folder.SourcePath, folder.DestPath, archiveName, config.ZipArgs) == ZipCodes.NoError.Value)
                        // delete old archives if no errors
                        FileSystem.DeleteOldFiles(folder.DestPath, folder.MaxArchives, folder.HeadMask + "*");
                    else Log.Add("Deleting old files in [{0}] will not be", folder.DestPath);

                    Log.Add("<<< Task {0}", i);
                    i++;
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

            string logsMask = BackupArchiverConfig.ASSEMBLY_NAME + "*.log";
            Log.AddLine();
            FileSystem.DeleteOldFiles(config.LogPath, config.MaxLogs, logsMask);
            //Log.AddLine();
            //FileSystem.DeleteOldFiles(config.LogPath, config.MaxLogs, "*.log");
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
