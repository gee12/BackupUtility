using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BackupLibrary
{
    class XMLReader
    {
        //public static void Parse(string fileName, Dictionary<string, XMLElement> elements)
        //{
        //    if (elements == null)
        //    {
        //        ArgumentNullException ex = new ArgumentNullException("Dictionary<string, XMLElement> elements");
        //        Log.Add(ex.Message);
        //        throw ex;
        //    }

        //    Dictionary<string, XMLElement> res = new Dictionary<string, XMLElement>();
        //    XmlTextReader reader = null;
        //    try
        //    {
        //        reader = new XmlTextReader(fileName);
        //        reader.WhitespaceHandling = WhitespaceHandling.None;

        //        while (reader.Read())
        //        {
        //            if (reader.NodeType == XmlNodeType.Element && elements.ContainsKey(reader.Name))
        //            {
        //                XMLElement elem = elements[reader.Name];
        //                Dictionary<string, XMLAttribute<Type>> attribs = new Dictionary<string, XMLAttribute<Type>>();
        //                while (reader.MoveToNextAttribute())
        //                {
        //                    string attrName = reader.Name;
        //                    if (elem.Attribs.ContainsKey(attrName))
        //                    {
        //                        XMLAttribute<Type> srcAttr = elem.Attribs[attrName];
        //                        XMLAttribute<Type> attr = new XMLAttribute<Type>(srcAttr, reader.Value);
        //                        attribs.Add(attrName, attr);
        //                    }
        //                }
        //                XMLElement newElem = new XMLElement(elem.Name, attribs);
        //                res.Add(elem.Name, newElem);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Add(ex.Message);
        //    }
        //}
    }
}
