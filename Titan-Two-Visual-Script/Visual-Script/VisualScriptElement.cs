using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Titan_Two_Visual_Script
{
    class VisualScriptElement
    {
        public List<Point> Coords { get; set; }
        public Bitmap Image { get; set; }
        public string TriggerKey { get; set; }
        public ImageCompareMethod CompareMethod { get; set; }

        private double toleranceLevel;

        public double ToleranceLevel {
            get { return toleranceLevel; }
            set
            {
                if (value < 0.0 && value >= 1.0) throw new System.IO.InvalidDataException("VisualElement Tolerance Levels must be between 0.0 and 1.0");
                this.toleranceLevel = value;
            }
        }
        
        public VisualScriptElement()
        {
            TriggerKey = "";
            CompareMethod = ImageCompareMethod.COMPARE_PIXELS;
            Coords = new List<Point>();
            ToleranceLevel = 0.1;
        }

        public VisualScriptElement(Bitmap img, List<Point> coords, string triggerKeyIdx, ImageCompareMethod method, double t)
        {
            Image = img;
            Coords = coords;
            TriggerKey = triggerKeyIdx;
            CompareMethod = method;
            ToleranceLevel = t;
        }
        
        public void AddCoordinates(Point xy)
        {
            if(!Coords.Contains(xy)) Coords.Add(xy);
        }
        
        public bool IsValid()
        {
            return Coords.Count > 0 && Image != null && TriggerKey.Length != 0;
        }
        
    }
}
