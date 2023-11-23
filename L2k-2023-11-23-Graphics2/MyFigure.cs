using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace L2k_2023_11_23_Graphics2
{
    public class MyFigure
    {
        public Shape FigureShape { get; set; }
        public double Width => FigureShape.Width;
        public double Height => FigureShape.Height;
        public double X => FigureShape.Margin.Left;
        public double Y => FigureShape.Margin.Top;
        public MyFigure() {
            FigureShape = new Ellipse();
            FigureShape.Width = 100;
            FigureShape.Height = 100;
            FigureShape.Fill = Brushes.Green;
            FigureShape.Stroke = Brushes.Blue;
            FigureShape.StrokeThickness = 4;
        }

        public void Move(Point from, Point to)
        {
            FigureShape.Margin = new Thickness(
                FigureShape.Margin.Left + to.X - from.X,
                FigureShape.Margin.Top + to.Y - from.Y,
                FigureShape.Margin.Right,
                FigureShape.Margin.Bottom
            );
        }

        public bool ContainsPoint( Point point )
        {
            return point.X >= X
                && point.X <= X + Width
                && point.Y >= Y
                && point.Y <= Y + Height;
        }
    }
}
