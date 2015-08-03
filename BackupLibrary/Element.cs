using System;
using System.Collections.Generic;
using System.Text;

namespace BackupLibrary
{
    class XMLElement
    {
        public string Name;
        public Dictionary <string, XMLAttribute<object>> Attribs;

        public XMLElement(string name, Dictionary<string, XMLAttribute<object>> attribs)
        {
            this.Name = name;
            this.Attribs = attribs;
        }

        public XMLAttribute<object> GetAttribute(string name)
        {
            return Attribs[name];
        }
    }
}
