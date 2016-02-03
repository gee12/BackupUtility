using System;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;

namespace BackupLibrary
{
    public abstract class Config
    {
        private static readonly string ASSEMBLY_NAME = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public const string CONFIG_ELEMENT_TAG = "Config";
        
        public const string LOG_PATH_TAG = "LogPath";
        public const string MAX_LOGS_TAG = "MaxLogs";
        public const string READ_KEY_IN_FINISH_TAG = "ReadKeyInFinish";
        public const string WINDOW_STYLE_TAG = "WindowStyle";

        public static string DEF_CONFIG_PATH = System.AppDomain.CurrentDomain.FriendlyName + ".cfg";
        public const string DEF_LOG_PATH = ".\\";
        public const int DEF_MAX_LOGS = 5;
        public const bool DEF_READKEY_IN_FINISH = true;
        public const ProcessWindowStyle DEF_WINDOW_STYLE = ProcessWindowStyle.Hidden;

        public string LogPath { get; set; }
        public int MaxLogs { get; set; }
        public bool ReadKeyInFinish { get; set; }
        public ProcessWindowStyle WindowStyle { get; set; }

        public bool IsSetConfig = false;
        public string ConfigFilePath = DEF_CONFIG_PATH;


        /// <summary>
        /// 
        /// </summary>
        public Config(bool splitLog = true)
        {
            Init(ASSEMBLY_NAME, DEF_CONFIG_PATH, splitLog);
        }

        public Config(string assemblyName, bool splitLog = true)
        {
            Init(assemblyName, DEF_CONFIG_PATH, splitLog);
        }

        public Config(string assemblyName, string configFileFullName, bool splitLog = true)
        {
            Init(assemblyName, configFileFullName, splitLog);
        }

        public Config(string assemblyName, string[] args, bool splitLog = true)
        {
            Init(assemblyName, args, splitLog);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Init(string assemblyName, string[] args, bool splitLog)
        {
            LogPath = DEF_LOG_PATH;
            MaxLogs = DEF_MAX_LOGS;
            ReadKeyInFinish = DEF_READKEY_IN_FINISH;
            WindowStyle = DEF_WINDOW_STYLE;

            // set new config file, if set as argument
            if (args != null && args.Length > 0)
            {
                string newConfigName = args[0];
                Init(assemblyName, newConfigName, splitLog);
            }
            else Init(assemblyName, DEF_CONFIG_PATH, splitLog);
        }

        public virtual void Init(string assemblyName, string configFileFullName, bool splitLog)
        {
            if (!SetConfig(configFileFullName))
            {
                // set default config file
                SetConfig(DEF_CONFIG_PATH);
            }

            // init log
            Log.Init(assemblyName, LogPath, splitLog);
            Log.Add("Config file [{0}] set successfully", configFileFullName);
        }

        /// <summary>
        /// 
        /// </summary>
        private bool SetConfig(string configFileFullName)
        {
            IsSetConfig = false;
            this.ConfigFilePath = configFileFullName;

            if (new FileInfo(configFileFullName).Exists)
            {
                if (ReadConfig(configFileFullName))
                {
                    IsSetConfig = true;
                }
                else Log.Add("Error with config file [{0}]", configFileFullName);
            }
            else
            {
                Log.Add("Config file [{0}] is missing..!", configFileFullName);
            }
            return IsSetConfig;
        }

        public abstract bool ReadConfig(string configFileName);

        /// <summary>
        /// 
        /// </summary>
        public void ReadBaseElements(XmlTextReader reader)
        {
            if (reader == null) return;

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name.Equals(CONFIG_ELEMENT_TAG))
                {
                    while (reader.MoveToNextAttribute())
                    {
                        //string value = reader.Value;
                        //switch (reader.Name)
                        //{
                        //    case LOG_PATH_TAG: LogPath = value; break;
                        //    case MAX_LOGS_TAG: MaxLogs = Int32.Parse(value); break;
                        //    case READ_KEY_IN_FINISH_TAG: ReadKeyInFinish = Boolean.Parse(value); break;
                        //    case WINDOW_STYLE_TAG: WindowStyle = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), value); break;
                        //}

                        ReadBaseAttributes(reader.Name, reader.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReadBaseAttributes(string name, string value)
        {
            switch (name)
            {
                case LOG_PATH_TAG: LogPath = value; break;
                case MAX_LOGS_TAG: MaxLogs = Int32.Parse(value); break;
                case READ_KEY_IN_FINISH_TAG: ReadKeyInFinish = Boolean.Parse(value); break;
                case WINDOW_STYLE_TAG: WindowStyle = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), value); break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WriteBaseAttributes(XmlTextWriter writer)
        {
            if (writer == null) return false;

            try
            {
                writer.WriteAttributeString(LOG_PATH_TAG, LogPath);
                writer.WriteAttributeString(MAX_LOGS_TAG, MaxLogs.ToString());
                writer.WriteAttributeString(WINDOW_STYLE_TAG, WindowStyle.ToString());
                writer.WriteAttributeString(READ_KEY_IN_FINISH_TAG, ReadKeyInFinish.ToString());
                
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
                return false;
            }
            return true;
        }
    }
}
