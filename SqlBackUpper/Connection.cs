using System;
using System.Collections.Generic;
using System.Text;

namespace SqlBackUpper
{
    public class Connection
    {
        public const string INST_NAME_TAG = "InstName";
        public const string BASE_NAME_TAG = "BaseName";
        public const string USER_NAME = "UserName";
        public const string PASSWORD = "Password";
        public const string BACKUP_PATH_NAME = "BackupPath";
        public const string MAX_BACKUPS_TAG = "MaxBackups";

        public string InstName;
        public string BaseName;
        public string UserName;
        public string Password;
        public string BackupPath;
        public int MaxBackups;

        public Connection()
        {
            this.InstName = "";
            this.BaseName = "";
            this.UserName = "";
            this.Password = "";
            this.BackupPath = ".\\";
            this.MaxBackups = 5;
        }

        public Connection(string instName, string baseName, string userName, string password, 
            string backupPath, int maxBackups)
        {
            this.InstName = instName;
            this.BaseName = baseName;
            this.UserName = userName;
            this.Password = password;
            this.BackupPath = backupPath;
            this.MaxBackups = maxBackups;
        }
    }
}
