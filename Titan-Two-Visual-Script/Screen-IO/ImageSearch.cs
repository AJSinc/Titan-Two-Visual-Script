using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using ImageDiff;

namespace Titan_Two_Visual_Script
{
    public static class ImageSearch
    {

        public static bool CvImageContains(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            using (Bitmap section = srcImg.Clone(new Rectangle(xy.X, xy.Y, nestedImg.Width, nestedImg.Height), srcImg.PixelFormat))
            {
                var srcBgrImg = new Image<Bgr, Byte>(section);
                var nestedBgrImg = new Image<Bgr, Byte>(nestedImg);
                ImageSSIM diffCalc = new ImageSSIM();
                double diff = diffCalc.CalcSSIM(srcBgrImg, nestedBgrImg);
                if(diff > 0.5)  Console.WriteLine(diff);
                srcBgrImg.Dispose();
                nestedBgrImg.Dispose();
            }
            return true;
        }
        
        public static bool ImageRoughlyContainsScaled(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            using (Bitmap section = srcImg.Clone(new Rectangle(xy.X, xy.Y, nestedImg.Width, nestedImg.Height), srcImg.PixelFormat))
            using (Bitmap resizedSrc = new Bitmap(section, new Size(16, 16)))
            using (Bitmap resizedNestedImg = new Bitmap(nestedImg, new Size(16, 16)))
            {
                return ImageRoughlyContains(resizedSrc, resizedNestedImg, new Point(0,0));
            }
        }

        public static bool ImageRoughlyContains(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            using (Bitmap section = srcImg.Clone(new Rectangle(xy.X, xy.Y, nestedImg.Width, nestedImg.Height), srcImg.PixelFormat))
            {
                return ImageMatches(section, nestedImg);
            }
        }

        public static bool ImageMatches(Bitmap srcImg, Bitmap nestedImg)
        {
            long nonMatchCount = 0;
            int width = nestedImg.Width;
            int height = nestedImg.Height;
            int totalPix = width * height;
            if ((srcImg.Width < (width)) || (srcImg.Height < (height))) return false;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color nestedCurPix = nestedImg.GetPixel(x, y);
                    Color SrcPix = srcImg.GetPixel(x, y);

                    if (nestedCurPix.A < 20) continue;
                    if ((Math.Abs(nestedCurPix.R - SrcPix.R) > 10) || (Math.Abs(nestedCurPix.G - SrcPix.G) > 10) || (Math.Abs(nestedCurPix.R - SrcPix.R) > 10))
                    {
                        if (((double)++nonMatchCount / (double)totalPix) > 0.075)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        
    }
}
