using BackupLibrary;
using Microsoft.Shell;
using SqlBackUpperLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SBUConfigurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ISingleInstanceApp
    {
        public static readonly CultureInfo AppCulture = CultureInfo.CreateSpecificCulture("ru-RU");
        public static readonly string AppAssemblyName = Assembly.GetEntryAssembly().FullName;
        public static readonly string ASSEMBLY_NAME = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly string SQLBACKUPPER_CONFIG_FILE_NAME = "SqlBackUpper.exe.cfg";

        public static SqlBackUpperLibConfig CurrentConfig;

        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        public static void Main()
        {
            bool res = AppInit();

            if (!res) return;

            AppStart();
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool AppInit()
        {
            CurrentConfig = new SqlBackUpperLibConfig(ASSEMBLY_NAME, SQLBACKUPPER_CONFIG_FILE_NAME);
            if (CurrentConfig.IsSetConfig)
            {
                Connection.AttachServerTypeObjects(CurrentConfig.Connections, CurrentConfig.ServerTypes);
            }
            else
            {
                return (MessageBoxes.CreateConfigFile() && CurrentConfig.WriteConfig());

                //if (MessageBoxes.CreateConfigFile())
                //{
                //    if (!CurrentConfig.WriteConfig())
                //        return false;
                //}
                //else
                //    //Log.Add("Config file is not set. Exit..");
                //    return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AppStart()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(AppAssemblyName))
            {
                AppRun();
                SingleInstance<App>.Cleanup();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void AppRun()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                this.MainWindow.WindowState = WindowState.Normal;
            }
            this.MainWindow.Activate();

            return true;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //var config = CurrentConfig;
            //string logsMask = ASSEMBLY_NAME + "*.log";
            //Log.AddLine();
            //FileSystem.DeleteOldFiles(config.LogPath, config.MaxLogs, logsMask);
        }
    }
}
