using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Titan_Two_Visual_Script
{
    class Program
    {
        static string OpenFileWindow()
        {
            string path = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "gvs files (*.gvs)|*.gvs";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                }
            }
            return path;
        }

        [STAThread]
        static void Main(string[] args)
        {
            String gvsPath = OpenFileWindow();
            if (!File.Exists(gvsPath)) System.Environment.Exit(1);
            VisualScript gvs = null;
            try
            {
                gvs = new VisualScript(gvsPath);
                Console.WriteLine("GVS \"" + gvs.ScriptName + "\" by \"" + gvs.ScriptAuthor + "\" loaded\r\n");

            } catch (Exception e)
            {
                Console.WriteLine("Error opening GVS.\r\n" +
                    e.ToString() +
                    "\r\nPress any key to exit.");
                Console.ReadKey();
            }
            while (true)
            {
                gvs.RunScriptInstance();
                Thread.Sleep(200);
            }
        }
    }
}
