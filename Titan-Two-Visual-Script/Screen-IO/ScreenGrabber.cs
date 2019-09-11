using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Titan_Two_Visual_Script
{
    public static class ScreenGrabber
    {
        

        public static Bitmap CaptureScreen()
        {
            var image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(image);
            gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            gfx.Dispose();
            return image;
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowRect(IntPtr handle, out Rectangle rect);

        public static Bitmap CaptureScreen(IntPtr handle)
        {
            Rectangle rect = new Rectangle();
            GetWindowRect(handle, out rect);
            Rectangle rectangle2 = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            Bitmap image = new Bitmap(rectangle2.Width, rectangle2.Height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.CopyFromScreen(new Point(rectangle2.Left, rectangle2.Top), Point.Empty, rectangle2.Size);
            }
            return image;
        }
    }


}
