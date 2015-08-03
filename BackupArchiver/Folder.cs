using System;
using System.Collections.Generic;
using System.Text;

namespace BackupArchiver
{
    public class Folder
    {
        public const string HEAD_MASK_TAG = "HeadMask";
        public const string SOURCE_PATH_TAG = "SourcePath";
        public const string DEST_PATH_TAG = "DestPath";
        public const string MAX_ARCHIVES_TAG = "MaxArchives";

        public string HeadMask;
        public string SourcePath;
        public string DestPath;
        public int MaxArchives;

        public Folder()
        {
            this.HeadMask = "Backup";
            this.SourcePath = "";
            this.DestPath = "";
            this.MaxArchives = 5;
        }

        public Folder(string headMask, string sourcePath, string destPath, int maxArchives)
        {
            this.HeadMask = headMask;
            this.SourcePath = sourcePath;
            this.DestPath = destPath;
            this.MaxArchives = maxArchives;
        }
    }
}
