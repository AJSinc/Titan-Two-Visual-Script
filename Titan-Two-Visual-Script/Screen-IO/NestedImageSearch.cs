using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan_Two_Visual_Script
{
    public static class NestedImageSearch
    {
        public static bool ImageRoughlyContainsScaled(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            using (Bitmap section = srcImg.Clone(new Rectangle(xy.X, xy.Y, nestedImg.Width, nestedImg.Height), srcImg.PixelFormat))
            using (Bitmap resizedSrc = new Bitmap(section, new Size(16, 16)))
            using (Bitmap resizedNestedImg = new Bitmap(nestedImg, new Size(16, 16)))
            {
                return ImageRoughlyContains(resizedSrc, resizedNestedImg, 0, 0);
            }
        }

        public static bool ImageRoughlyContains(Bitmap srcImg, Bitmap nestedImg, int xCoord, int yCoord)
        {
            long nonMatchCount = 0;
            int width = nestedImg.Width;
            int height = nestedImg.Height;
            int totalPix = width * height;
            if ((srcImg.Width < (width + xCoord)) || (srcImg.Height < (height + yCoord))) return false;
            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Color nestedCurPix = nestedImg.GetPixel(x, y);
                    Color SrcPix = srcImg.GetPixel(xCoord+x, yCoord+y);

                    if (nestedCurPix.A == 0) continue;
                    if ((Math.Abs(nestedCurPix.R - SrcPix.R) > 10) || (Math.Abs(nestedCurPix.G - SrcPix.G) > 10) || (Math.Abs(nestedCurPix.R - SrcPix.R) > 10))
                    {
                        if (((double)++nonMatchCount / (double)totalPix) > 0.05)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool ImageRoughlyContains(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            return ImageRoughlyContains(srcImg, nestedImg, xy.X, xy.Y);
        }

    }
}
