using System;
using System.Collections.Generic;
using System.Xml;
using BackupLibrary;
   
namespace BackupArchiver
{
    public class Config : BackupLibrary.Config
    {
        public const string FOLDER_ELEMENT_TAG = "Folder";

        public const string ZIP_PATH_TAG = "ZipPath";
        public const string ZIP_ARGS_TAG = "ZipArgs";
        public const string TAIL_MASK_TAG = "TailMask";

        public string TailMask = "{0:dd-MM-yyyy_HH-mm-ss}.zip";
        public string ZipArgs = "a -tzip -y";
        public string ZipPath = ".\\";

        public List<Folder> Folders = new List<Folder>();

        public Config(string configFileName) : base(configFileName)
        {
            Init(configFileName);
        }

        public Config(string[] args) : base(args)
        {
            Init(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        override public  bool ReadConfig(string fileName)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(fileName);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals(CONFIG_ELEMENT_TAG))
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                string name = reader.Name;
                                string value = reader.Value;
                                switch (name)
                                {
                                    case ZIP_PATH_TAG: ZipPath = value; break;
                                    case ZIP_ARGS_TAG: ZipArgs = value; break;
                                    case TAIL_MASK_TAG: TailMask = value; break;
                                }
                                //
                                ReadBaseAttributes(name, value);
                            }
                        }
                        else if (reader.Name.Equals(FOLDER_ELEMENT_TAG))
                        {
                            Folder folder = new Folder();
                            while (reader.MoveToNextAttribute())
                            {
                                string value = reader.Value;
                                switch (reader.Name)
                                {
                                    case Folder.HEAD_MASK_TAG: folder.HeadMask = value; break;
                                    case Folder.SOURCE_PATH_TAG: folder.SourcePath = value; break;
                                    case Folder.DEST_PATH_TAG: folder.DestPath = value; break;
                                    case Folder.MAX_ARCHIVES_TAG: folder.MaxArchives = Int32.Parse(value); break;
                                }
                            }
                            Folders.Add(folder);
                        }
                    }
                    // 
                    //ReadBaseElements(reader);
                }
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
