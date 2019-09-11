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
        private List<List<VisualScriptElement>> ElementGroups;
        public VisualScript(String path)
        {
            ElementGroups = VisualScriptReader.ReadGVS(path);
        }
        
        public void RunScriptInstance()
        {
            for(int i = 0; i < ElementGroups.Count; i++)
            {
                bool imgFound = false;
                for(int k = 0; k < ElementGroups[i].Count; k++)
                {
                    for (int xy = 0; xy < ElementGroups[i][k].Coords.Count; xy++)
                    {
                        using (Bitmap screen = ScreenGrabber.CaptureScreen())
                        {
                            if(NestedImageSearch.ImageRoughlyContains(screen, ElementGroups[i][k].Image, ElementGroups[i][k].Coords[xy]))
                            {
                                imgFound = true;
                                break;
                            }
                        }
                    }
                    if(imgFound)
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
