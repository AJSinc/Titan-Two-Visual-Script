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
        
        public static bool ImageRoughlyContains(Bitmap srcImg, Bitmap nestedImg, int xCoord, int yCoord)
        {
            long nonMatchCount = 0;
            int width = nestedImg.Width;
            int height = nestedImg.Height;
            int totalPix = width * height;
            if ((srcImg.Width < (width + xCoord)) || (srcImg.Height < (height + yCoord))) return false;
            // speed all this up by shrinking images to < 20 pixels
            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Color nestedCurPix = nestedImg.GetPixel(x, y);
                    Color SrcPix = srcImg.GetPixel(xCoord+x, yCoord+y);

                    if (nestedCurPix.A == 0) continue;
                    if ((Math.Abs(nestedCurPix.R - SrcPix.R) > 10) || (Math.Abs(nestedCurPix.G - SrcPix.G) > 10) || (Math.Abs(nestedCurPix.R - SrcPix.R) > 10))
                    {
                        if (((double)++nonMatchCount / (double)totalPix) > 0.05) return false;
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
