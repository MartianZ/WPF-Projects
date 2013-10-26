using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.IO;

namespace MusicSorter
{
    public class XMLHelper
    {
        private readonly static string Path = "Config.xml";

        public static string ReadConfiguration(string key)
        {
            if (!File.Exists(Path)) return null;
            XmlDocument myxml = new XmlDocument();
            myxml.Load(Path);
            XmlNode root = myxml.DocumentElement;
            foreach (XmlNode var in root.ChildNodes)
            {
                if (var.Name == key)
                {
                    return var.InnerText;
                }
            }
            return null;
        }
        
        public static void WriteConfiguration(string key, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot;
            if (File.Exists(Path))
            {
                xmlDoc.Load(Path);
                xmlRoot = xmlDoc.DocumentElement;
            }
            else
            {
                xmlRoot = xmlDoc.CreateElement("MusicSorter");
                xmlDoc.AppendChild(xmlRoot);
            }

            bool AlreadyHasConfig = false;
            foreach (XmlNode var in xmlRoot.ChildNodes)
            {
                if (var.Name == key)
                {
                    AlreadyHasConfig = true;
                    var.InnerText = value;
                    break;
                }
            }

            if (!AlreadyHasConfig)
            {
                XmlElement xmlele = xmlDoc.CreateElement(key);
                xmlele.InnerText = value;
                xmlRoot.AppendChild(xmlele);
            }

            xmlDoc.Save(Path);
        }
        
    }
}
