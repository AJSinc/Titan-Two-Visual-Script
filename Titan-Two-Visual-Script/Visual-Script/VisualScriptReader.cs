using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Titan_Two_Visual_Script.Visual_Script
{
    static class VisualScriptReader
    {
        
        private static XmlReader xmlReader;
        private static VisualScriptElement vselement;
        private static List<VisualScriptElement> group;
        private static List<List<VisualScriptElement>> groups;
        private static string gvsPath;
        private static string gvsName;
        private static string gvsAuthor;

        public static List<List<VisualScriptElement>> ReadGVSElements(string path)
        {
            gvsName = "";
            gvsAuthor = "";
            gvsPath = path;
            groups = new List<List<VisualScriptElement>>();
            if (!File.Exists(path)) throw new FileNotFoundException();
            using (xmlReader = XmlReader.Create(new StringReader(File.ReadAllText(path))))
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            HandleGVSElement(xmlReader.Name);
                            break;
                        case XmlNodeType.EndElement:
                            HandleGVSEndElement(xmlReader.Name);
                            break;
                    }
                }
            }
            return groups;
        }
        
        public static String ReadGVSName(string path)
        {
            ReadGVSElements(path);
            return gvsName;
        }

        public static String ReadGVSAuthor(string path)
        {
            ReadGVSElements(path);
            return gvsAuthor;
        }

        private static void HandleGVSElement(string element)
        {
            switch (element.ToLower())
            {
                case "scriptname":
                    xmlReader.Read();
                    gvsName = xmlReader.Value;
                    break;
                case "scriptauthor":
                    xmlReader.Read();
                    gvsAuthor = xmlReader.Value;
                    break;
                case "visualgroup":
                    group = new List<VisualScriptElement>();
                    break;
                case "visualelement":
                    vselement = new VisualScriptElement();
                    break;
                case "imgpath":
                    xmlReader.Read();
                    String path = Path.GetDirectoryName(gvsPath) + "\\" + xmlReader.Value.Replace('/', '\\');
                    if (!File.Exists(path)) throw new FileNotFoundException();
                    vselement.Image = new Bitmap(Image.FromFile(path));
                    break;
                case "coords":
                    xmlReader.Read();
                    String[] xy = xmlReader.Value.Split(',');
                    if (xy.Length != 2) throw new InvalidDataException("Coordinate elements must have 2 coordinates seperated by a ','");
                    vselement.AddCoordinates(new Point(Int32.Parse(xy[0]), Int32.Parse(xy[1])));
                    break;
                case "trigkey":
                    xmlReader.Read();
                    vselement.TriggerKey = xmlReader.Value;
                    break;
                case "comparemethod":
                    xmlReader.Read();
                    vselement.CompareMethod = (ImageCompareMethod)Int32.Parse(xmlReader.Value);
                    break;
                case "tolerancelevel":
                    xmlReader.Read();
                    vselement.ToleranceLevel = Double.Parse(xmlReader.Value);
                    break;
            }
        }

        private static void HandleGVSEndElement(string element)
        {
            switch (element.ToLower())
            {
                case "visualgroup":
                    if (group != null && group.Count > 0)
                    {
                        groups.Add(group);
                        group = null;
                        vselement = null;
                    }
                    break;
                case "visualelement":
                    if (!vselement.IsValid()) throw new InvalidDataException("Invalid VisualElement, must contain a path to the file, coordinates, and a trigger key");
                    if (group == null) // elements do not have to be grouped
                    {
                        group = new List<VisualScriptElement>();
                        group.Add(vselement);
                        groups.Add(group);
                    }
                    else group.Add(vselement);
                    vselement = new VisualScriptElement();
                    break;
            }
        }
        
    }
}
