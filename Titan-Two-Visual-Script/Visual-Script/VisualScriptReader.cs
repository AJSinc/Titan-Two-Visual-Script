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
        private static string text;
        private static VisualScriptElement vselement;
        private static List<VisualScriptElement> group;
        private static List<List<VisualScriptElement>> groups;
        private static string gvsPath;
        
        public static List<List<VisualScriptElement>> ReadGVS(string path)
        {
            gvsPath = path;
            groups = new List<List<VisualScriptElement>>();
            if (!File.Exists(path)) throw new FileNotFoundException();
            xmlReader = XmlReader.Create(new StringReader(File.ReadAllText(path)));
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Comment:
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.XmlDeclaration:
                        break;
                    case XmlNodeType.Element:
                        HandleGVSElement(xmlReader.Name);
                        break;
                    case XmlNodeType.EndElement:
                        HandleGVSEndElement(xmlReader.Name);
                        break; 
                    default:
                    case XmlNodeType.Text:
                        text = xmlReader.Value;
                        break;
                }
            }
            return groups;
        }
        
        private static void HandleGVSElement(string element)
        {
            switch (element.ToLower())
            {
                case "visualgroup":
                    group = new List<VisualScriptElement>();
                    break;
                case "visualelement":
                    text = null;
                    vselement = new VisualScriptElement();
                    break;
                default:
                    break;

            }

        }

        private static void HandleGVSEndElement(string element)
        {
            switch (element.ToLower())
            {
                case "visualgroup":
                    groups.Add(group);
                    group = null;
                    vselement = null;
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
                case "imgpath":
                    if(text != null)
                    {
                        String bmpPath = Path.GetDirectoryName(gvsPath) + "\\" + text;
                        if (!File.Exists(bmpPath)) throw new FileNotFoundException();
                        vselement.Image = new Bitmap(Image.FromFile(bmpPath));
                    }
                    text = null;
                    break;
                case "coords":
                    if (text != null)
                    {
                        String[] xy = text.Split(',');
                        if (xy.Length != 2) throw new InvalidDataException("Coordinate element must have 2 coordinates seperated by a ','");
                        vselement.AddCoordinates(new Point(Int32.Parse(xy[0]), Int32.Parse(xy[1])));
                    }
                    text = null;
                    break;
                case "trigkey":
                    if (text != null)
                    {
                        vselement.Key = Int32.Parse(text);
                    }
                    text = null;
                    break;
                default:
                    break;
            }

        }
        
    }
}
