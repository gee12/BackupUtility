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
        public const string CONFIG_ELEMENT_TAG = "Config";

        public const string LOG_PATH_TAG = "LogPath";
        public const string MAX_LOGS_TAG = "MaxLogs";
        public const string READ_KEY_IN_FINISH_TAG = "ReadKeyInFinish";
        public const string WINDOW_STYLE_TAG = "WindowStyle";
        public string DEFAULT_CONFIG_PATH = System.AppDomain.CurrentDomain.FriendlyName + ".cfg";

        public string LogPath = ".\\";
        public int MaxLogs = 5;
        public bool ReadKeyInFinish = true;
        public ProcessWindowStyle WindowStyle = ProcessWindowStyle.Hidden;

        public bool IsSetConfig = false;

        public Config(string configFileName)
        {
            //Init(configFileName);
        }

        public Config(string[] args)
        {
            //Init(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Init(string[] args)
        {
            // set new config file, if set as argument
            if (args != null && args.Length > 0)
            {
                string newConfigName = args[0];
                Init(newConfigName);
            }
            else SetConfig(DEFAULT_CONFIG_PATH);
        }

        public void Init(string configFileName)
        {
            if (!SetConfig(configFileName))
            {
                // set default config file
                SetConfig(DEFAULT_CONFIG_PATH);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool SetConfig(string configFileName)
        {
            if (new FileInfo(configFileName).Exists)
            {
                if (ReadConfig(configFileName))
                {
                    IsSetConfig = true;
                    // init log
                    Log.Init(LogPath);
                    Log.Add("Config file [{0}] set successfully", configFileName);
                    return true;
                }
                else Log.Add("Error with config file [{0}]", configFileName);
            }
            else
            {
                Log.Add("Config file [{0}] is missing..!", configFileName);
            }
            return false;
        }

        public abstract bool ReadConfig(string configFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ReadBaseElements(XmlTextReader reader)
        {
            if (reader == null) return;

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name.Equals(CONFIG_ELEMENT_TAG))
                {
                    while (reader.MoveToNextAttribute())
                    {
                        string value = reader.Value;
                        switch (reader.Name)
                        {
                            case LOG_PATH_TAG: LogPath = value; break;
                            case MAX_LOGS_TAG: MaxLogs = Int32.Parse(value); break;
                            case READ_KEY_IN_FINISH_TAG: ReadKeyInFinish = Boolean.Parse(value); break;
                            //   ?!
                            case WINDOW_STYLE_TAG: WindowStyle = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), value); break;
                        }
                    }
                }
            }
        }

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
    }
}
