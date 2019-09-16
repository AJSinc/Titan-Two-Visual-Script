using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Titan_Two_Visual_Script.Visual_Script;

namespace Titan_Two_Visual_Script
{
    class VisualScript
    {
        public string ScriptName { get; set; }
        public string ScriptAuthor { get; set; }

        private List<List<VisualScriptElement>> ElementGroups;
        public VisualScript(String path)
        {
            ScriptName = VisualScriptReader.ReadGVSName(path);
            ScriptAuthor = VisualScriptReader.ReadGVSAuthor(path);
            ElementGroups = VisualScriptReader.ReadGVSElements(path);
        }
        
        public void RunScriptInstance()
        {
            for(int i = 0; i < ElementGroups.Count; i++)
            {
                bool imgFound = false;
                for(int k = 0; k < ElementGroups[i].Count; k++)
                {
                    using (Bitmap screen = ScreenGrabber.CaptureScreen())
                    foreach(Point coords in ElementGroups[i][k].Coords)
                    {
                        if(ImageSearch.ImageRoughlyContains(screen, ElementGroups[i][k].Image, coords)) {
                            Console.Write("Found");
                            imgFound = true;
                            break;
                        }
                    }

                    if (imgFound)
                    {
                        // send key data
                        SendKeys.SendWait(ElementGroups[i][k].Key);
                        break;
                    }
                }

            }
        }

    }
}
