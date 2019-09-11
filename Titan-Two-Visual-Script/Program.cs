using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Titan_Two_Visual_Script
{
    class Program
    {
        static void Main(string[] args)
        {
            int tickCountTimer = 0;
            String gvsPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\test.gvs";
            VisualScript gvs = new VisualScript(gvsPath);
            while (true)
            {
                int currTick = Environment.TickCount & Int32.MaxValue;
                if (currTick > tickCountTimer)
                {
                    tickCountTimer = currTick + 500;
                    gvs.RunScriptInstance();
                }
                Thread.Sleep(10);
            }
        }
    }
}
