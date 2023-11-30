using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace L2k_2023_11_23_Graphics2
{
    public class RectArea
    {
        private Point p1;
        private Point p2;

        public double X => Math.Min(p1.X, p2.X);
        public double Y => Math.Min(p1.Y, p2.Y);

        public double Width => Math.Abs(p2.X - p1.X);
        public double Height => Math.Abs(p2.Y - p1.Y);

        public RectArea(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
