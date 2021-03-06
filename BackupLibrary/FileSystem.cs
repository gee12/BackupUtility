﻿using System;
using System.IO;
using System.Collections.Generic;

namespace BackupLibrary
{
    public class FileSystem
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool IsFilesExist(string[] files, string dir)
        {
            foreach (string fileName in files)
            {
                FileInfo fi = new FileInfo(dir + "\\" + fileName);
                if (!fi.Exists) return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsFilesExist(string[] files)
        {
            foreach (string fileName in files)
            {
                FileInfo fi = new FileInfo(fileName);
                if (!fi.Exists) return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateIfMissing(string dirPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);

                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch(IOException ioEx)
            {
                Log.Add("I/O error with creating directory on path: [{0}]", dirPath);
            }
            catch(Exception ex)
            {
                Log.Add("Invalid directory path: [{0}]", dirPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteOldFiles(string path, int maxCount, string mask="*.*")
        {
            DirectoryInfo dir = null;
            try
            {
                dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    Log.Add("Deleting old files is not possible..");
                    Log.Add("(directory [{0}] is missing)", dir.FullName);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Add("Deleting old files is not possible..");
                Log.Add("(invalid directory path: [{0}])", path);
                return;
            }

            FileInfo[] files = dir.GetFiles(mask);

            if (files.Length > maxCount)
            {
                Log.Add("Deleting old files. Path: [{0}], MaxFilesCount: [{1}]", path, maxCount);
                // sort files array from creation time & delete oldest
                Array.Sort(files, new FileCreationTimeDescComparer());
                for (int i = 0; i < files.Length - maxCount; i++)
                {
                    DeleteFile(files[i]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteFiles(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + path);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
            Log.Add("File [{0]] was deleted", fileName);
        }

        public static void DeleteFile(FileInfo file)
        {
            file.Delete();
            Log.Add("File [{0}] was deleted", file.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetFileLenth(string fileName)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            string zeroBytes = "0" + suf[0];
            FileInfo f;
            long byteCount = 0;

            try
            {
                f = new FileInfo(fileName);
                byteCount = f.Length;
            }
            catch (Exception ex)
            {
                Log.Add("Error with getting file size: [{0}]", fileName);
            }

            if (byteCount == 0)
                return zeroBytes;

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileCreationTimeDescComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return (x.CreationTime.CompareTo(y.CreationTime));
        }

    }
}