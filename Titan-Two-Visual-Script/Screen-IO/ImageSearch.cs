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

    public enum ImageCompareMethod
    {
        COMPARE_PIXELS = 1,
        SCALED_COMPARE_PIXELS = 2,
        EMGU_CV = 3,
    };

    public static class ImageSearch
    {
        private static double tolerance;
        public static bool ImageContains(Bitmap srcImg, Bitmap nestedImg, Point xy, ImageCompareMethod method, double t)
        {
            t = tolerance;
            if (method == ImageCompareMethod.COMPARE_PIXELS) return ImageRoughlyContains(srcImg, nestedImg, xy);
            if (method == ImageCompareMethod.SCALED_COMPARE_PIXELS) return ImageRoughlyContainsScaled(srcImg, nestedImg, xy);
            if (method == ImageCompareMethod.EMGU_CV) return CvImageContains(srcImg, nestedImg, xy);
            return false;
        }

        private static bool CvImageContains(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            using (Bitmap section = srcImg.Clone(new Rectangle(xy.X, xy.Y, nestedImg.Width, nestedImg.Height), srcImg.PixelFormat))
            {
                double compareDiff = 10.0;
                var srcBgrImg = new Image<Gray, Byte>(section).ThresholdBinary(new Gray(100), new Gray(255));
                var nestedBgrImg = new Image<Gray, Byte>(nestedImg).ThresholdBinary(new Gray(100), new Gray(255));
                Emgu.CV.Util.VectorOfVectorOfPoint srcContour = new Emgu.CV.Util.VectorOfVectorOfPoint();
                Emgu.CV.Util.VectorOfVectorOfPoint nestedContour = new Emgu.CV.Util.VectorOfVectorOfPoint();
                Mat heir = new Mat();
                Mat heir2 = new Mat();
                CvInvoke.FindContours(srcBgrImg, srcContour, heir, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                CvInvoke.FindContours(nestedBgrImg, nestedContour, heir2, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                if (srcContour.Size > 0 && nestedContour.Size > 0)
                {
                    compareDiff = CvInvoke.MatchShapes(srcContour[0], nestedContour[0], Emgu.CV.CvEnum.ContoursMatchType.I3);
                }
                srcBgrImg.Dispose();
                nestedBgrImg.Dispose();
                srcContour.Dispose();
                nestedContour.Dispose();
                heir.Dispose();
                heir2.Dispose();

                return (compareDiff < 0.05);
                /* SSIM
                    var srcBgrImg = new Image<Bgr, Byte>(section);
                    var nestedBgrImg = new Image<Bgr, Byte>(nestedImg);
                    ImageSSIM diffCalc = new ImageSSIM();
                    double diff = diffCalc.CalcSSIM(srcBgrImg, nestedBgrImg);
                    if (diff > 0.5) Console.WriteLine(diff);
                    srcBgrImg.Dispose();
                    nestedBgrImg.Dispose();
                */
            }
        }

        private static bool ImageRoughlyContainsScaled(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            using (Bitmap section = srcImg.Clone(new Rectangle(xy.X, xy.Y, nestedImg.Width, nestedImg.Height), srcImg.PixelFormat))
            using (Bitmap resizedSrc = new Bitmap(section, new Size(16, 16)))
            using (Bitmap resizedNestedImg = new Bitmap(nestedImg, new Size(16, 16)))
            {
                return ImageMatches(resizedSrc, resizedNestedImg);
            }
        }

        private static bool ImageRoughlyContains(Bitmap srcImg, Bitmap nestedImg, Point xy)
        {
            using (Bitmap section = srcImg.Clone(new Rectangle(xy.X, xy.Y, nestedImg.Width, nestedImg.Height), srcImg.PixelFormat))
            {
                return ImageMatches(section, nestedImg);
            }
        }

        private static bool ImageMatches(Bitmap srcImg, Bitmap nestedImg)
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
                        if (((double)++nonMatchCount / (double)totalPix) > tolerance)
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
