using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using BackupLibrary;

namespace BackupArchiver
{
    public class ZipCodes : Enumeration
    {
        public static ZipCodes NoError = new ZipCodes(0, "No error");
        public static ZipCodes Warning = new ZipCodes(1, "Warning (Non fatal error(s)). For example, one or more files were locked by some other application, so they were not compressed.");
        public static ZipCodes FatalError = new ZipCodes(2, "Fatal error");
        public static ZipCodes CommandLineError = new ZipCodes(7, "Command line error");
        public static ZipCodes NotEnoughMemory = new ZipCodes(8, "Not enough memory for operation");
        public static ZipCodes UserStoppedProcess = new ZipCodes(255, "User stopped the process");

        public ZipCodes() : base() { }

        public ZipCodes(int value, string displayName) : base (value, displayName) { }
    }

    //public class ZipCode
    //{
    //    public int Code;
    //    public string Message;
    //    public ZipCode(int code, string message)
    //    {
    //        this.Code = code;
    //        this.Message = message;
    //    }
    //}

    public class Zip
    {
        public static string _7Z_EXE = "7z.exe";
        public static string[] _7Z_FILES = { _7Z_EXE, "7z.dll", "7-zip.dll" };
        public static string _7Z_DIR1 = @"C:\Program Files\7-Zip\";
        public static string _7Z_DIR2 = @"C:\Program Files (x86)\7-Zip\";

        //public static ZipCode[] ZipCodes = { 
        //    new ZipCode(0, "No error"),
        //    new ZipCode(1, "Warning (Non fatal error(s)). For example, one or more files were locked by some other application, so they were not compressed."),
        //    new ZipCode(2, "Fatal error"),
        //    new ZipCode(7, "Command line error"),
        //    new ZipCode(8, "Not enough memory for operation"),
        //    new ZipCode(255, "User stopped the process")};

        public static Boolean IsExist = false;
        public static string FullPath = /*Config.ZipPath +*/ ".\\" + _7Z_EXE;


        public static void Init(string zipPath)
        {
            IsExist = true;
            if (FileSystem.IsFilesExist(_7Z_FILES, zipPath))
            {
                Log.Add("'7Zip' path: [{0}]", zipPath);
                FullPath = zipPath;
            }
            else
            {
                Log.Add("'7Zip' don't exists on path [{0}]", zipPath);

                if (FileSystem.IsFilesExist(_7Z_FILES))
                    // Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    // System.Environment.CurrentDirectory
                    // Directory.GetCurrentDirectory();
                    FullPath = Path.GetDirectoryName(Application.ExecutablePath);
                else if (FileSystem.IsFilesExist(_7Z_FILES, _7Z_DIR1))
                    FullPath = _7Z_DIR1;
                else if (FileSystem.IsFilesExist(_7Z_FILES, _7Z_DIR2))
                    FullPath = _7Z_DIR2;
                else IsExist = false;

                if (IsExist)
                    Log.Add("'7Zip' was found on path [{0}]", FullPath);
            }
            FullPath += "\\" + _7Z_EXE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool Is7ZipExist()
        {
            return (FileSystem.IsFilesExist(_7Z_FILES)
                || FileSystem.IsFilesExist(_7Z_FILES, _7Z_DIR1)
                || FileSystem.IsFilesExist(_7Z_FILES, _7Z_DIR2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="archiveName"></param>
        /// <returns></returns>
        public static int AddTo_Zip_R_Y_Archive(string source, string dest, string archiveName, string zipArgs)
        {
            try
            {
                DirectoryInfo srcDir = new DirectoryInfo(source);
                if (!srcDir.Exists)
                {
                    Log.Add("Archiving canceled..");
                    Log.Add("(missing source directory: [{0}])", source);
                    return -1;
                }
            }
            catch(Exception ex)
            {
                Log.Add("Archiving canceled..");
                Log.Add("(invalid source path: [{0}])", source);
                return -1;
            }

            string arguments = String.Format("{0} \"{1}\\{2}\" \"{3}\"", zipArgs, dest, archiveName, source);
            int result = AddToArchive(arguments);

            //Log.Add("Archiving: [{0}]. Results: [{1}]", arguments, GetZipMessage(result));
            Log.Add("Archiving: [{0}]. Results: [{1}]", arguments, Enumeration.DisplayNameFromValue<ZipCodes>(result));

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static int AddToArchive(string arguments)
        {
            ProcessStartInfo start = new ProcessStartInfo(FullPath, arguments);
            start.WindowStyle = Display.WindowStyle;
            int exitCode = -1;
            try
            {
                // run 7z.exe
                using (Process proc = Process.Start(start))
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
            }
            return exitCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exitCode"></param>
        /// <returns></returns>
        //public static string GetZipMessage(int exitCode)
        //{
        //    foreach (ZipCode code in ZipCodes)
        //    {
        //        if (code.Code == exitCode) return code.Message;
        //    }
        //    return "Unknown exit code from 7-Zip";
        //}
    }
}
