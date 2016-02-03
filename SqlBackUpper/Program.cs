using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using BackupLibrary;
using System.Reflection;
using SqlBackUpperLib;

namespace SqlBackUpper
{

    class Program
    {
        public static readonly string ASSEMBLY_NAME = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        static void Main(string[] args)
        {
            Display.SetWindowStyle(ProcessWindowStyle.Hidden);

            // init config file (and current log file)
            //SqlBackUpperConfig config = new SqlBackUpperConfig(args);
            SqlBackUpperLibConfig config = new SqlBackUpperLibConfig(ASSEMBLY_NAME, args);
            if (!config.IsSetConfig)
            {
                OnFinish(config);
                //Log.Add("Config file is not set. Exit..");
                return;
            }

            //ProcessWindowStyle windowStyle = config.WindowStyle;
            Display.SetWindowStyle(config.WindowStyle);

            // connecting to the server & execute sql query
            List<Connection> connections = config.Connections;
            if (connections != null)
            {
                if (config.UniteSameInst)
                {
                    // use grouping by server
                    List<ConnectionGroup> groups = ConnectionGroup.SpritToGroups(connections);
                    foreach(ConnectionGroup group in groups)
                    {
                        Log.AddLine();
                        SqlQuery.ExecuteGroup(group, config.ConnectionGroupMask, config.Timeout, config.SqlQueryMask, config.BackupNameMask);
                        foreach (Connection conn in group.Connections)
                        {
                            FileSystem.DeleteOldFiles(conn.BackupPath, conn.MaxBackups, conn.Database + "*");
                        }
                    }
                }
                else
                {
                    // execute all queries successively
                    foreach (Connection conn in connections)
                    {
                        Log.AddLine();
                        string backupName = conn.Database + String.Format(config.BackupNameMask, DateTime.Now);
                        SqlQuery.Execute(conn, config.ConnectionMask, config.Timeout, config.SqlQueryMask, backupName);
                        FileSystem.DeleteOldFiles(conn.BackupPath, conn.MaxBackups, conn.Database + "*");
                    }
                }
            }
            else Log.Add("Error in config ([Connections] == null)");

            // delete extra log files
            OnFinish(config);
            //FileSystem.DeleteOldFiles(config.LogPath, config.MaxLogs, "*.log");
            
            //Log.Add("Finish");
            ////
            //if ((windowStyle == ProcessWindowStyle.Normal || windowStyle == ProcessWindowStyle.Maximized) && config.ReadKeyInFinish)
            //{
            //    Console.WriteLine("Press any key to exit..");
            //    Console.ReadKey();
            //}
        }

        static void OnFinish(Config config)
        {
            if (config == null) return;

            string logsMask = ASSEMBLY_NAME + "*.log";
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

    }
}
