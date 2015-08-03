using System;
using System.IO;
using System.Reflection;

namespace BackupLibrary
{
    public class Log
    {
        //public static string LOG_FILE_NAME = Application.ExecutablePath + "_log.log";
        // System.AppDomain.CurrentDomain.FriendlyName - with extension
        //public static string LOG_FILE_NAME = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "_log.log";
        //public static string LOG_FULL_PATH = Config.GetLogPath() + LOG_FILE_NAME;

        public static string CurrentLogFileName = String.Format(".\\{0}_{1:dd-MM-yyyy_HH-mm-ss}_log.log",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, DateTime.Now);

        private static bool isFirstStart = true;

        public static void Init(string logPath)
        {
            try
            {
                DirectoryInfo logDir = new DirectoryInfo(logPath);
                if (!logDir.Exists) logDir.Create();

                CurrentLogFileName = String.Format("{0}\\{1}_{2:dd-MM-yyyy_HH-mm-ss}_log.log",
                    logPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, DateTime.Now);
               
                Log.Add("Log file: [{0}]", CurrentLogFileName);
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        public static void Add(string s, params object[] args)
        {
            string res = String.Format(s, args);
            Add(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public static void Add(string s, bool isNeedDataTime = true)
        {
            try
            {
                StreamWriter sw = new StreamWriter(CurrentLogFileName, true);

                // add program name & version
                if (isFirstStart)
                {
                    isFirstStart = false;
                    AssemblyName name = Assembly.GetExecutingAssembly().GetName();
                    string hello = String.Format("Start [{0}, {1}]", name.Name, name.Version);
                    sw.WriteLine("{0} - {1}", DateTime.Now, hello);
                    Console.WriteLine(hello);
                }
                if (isNeedDataTime)
                    sw.WriteLine("{0} - {1}", DateTime.Now, s);
                else sw.WriteLine(s);
                sw.Close();
            }
            catch (Exception ex)
            {
                //Log.Add(ex.Message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine(s);
            }
        }

        public static void AddLine()
        {
            Add("", false);
        }
    }
}
