﻿using System;
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
        private List<Point> coords;
        private Bitmap img;
        private int triggerKeyIdx;


        public VisualScriptElement()
        {
            triggerKeyIdx = -1;
            coords = new List<Point>();
        }

        public VisualScriptElement(Bitmap img, List<Point> coords, int triggerKeyIdx)
        {
            this.img = img;
            this.coords = coords;
            this.triggerKeyIdx = triggerKeyIdx;
        }

        public bool VisualElementFoundIn(Bitmap srcImg)
        {
            foreach(Point p in coords)
            {
                if (NestedImageSearch.ImageRoughlyContains(srcImg, this.img, p.X, p.Y)) return true;
            }
            return false;
        }

        public void AddCoordinates(Point xy)
        {
            if(!coords.Contains(xy)) coords.Add(xy);
        }
        
        public bool IsValid()
        {
            return coords.Count > 0 && img != null && triggerKeyIdx != -1;
        }

        public List<Point> Coords
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
            }
        }

        public Bitmap Image
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
            }
        }

        public int Key
        {
            get
            {
                return triggerKeyIdx;
            }
            set
            {
                triggerKeyIdx = value;
            }
        }

    }
}