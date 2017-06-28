using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSegmentation
{
    /// <summary>
    /// A pixel that stores 5 elements for processing segmentation
    /// </summary>
    public class Pixel
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int Label { get; set; }

        public Pixel() { }

        public Pixel(int R, int G, int B, int X, int Y)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.X = X;
            this.Y = Y;
        }
    }
}
