using System;
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

        private Point firstPoint;

        public MyFigure(
            ToolType type,
            Point topLeft
        ) {
            FigureShape = type switch
            {
                ToolType.Pen => new Polyline(),
                ToolType.Ellipse => new Ellipse(),
                ToolType.Rectangle => new Rectangle(),
                _ => new Rectangle()
            };
            firstPoint = topLeft;
            if (type != ToolType.Pen)
            {
                FigureShape.Width = 0;
                FigureShape.Height = 0;
            } else
            {
                ((Polyline)FigureShape).Points.Add(topLeft);
                FigureShape.StrokeStartLineCap = PenLineCap.Round;
                FigureShape.StrokeEndLineCap = PenLineCap.Round;
                FigureShape.StrokeLineJoin = PenLineJoin.Round;
            }
            FigureShape.Fill = Brushes.Green;
            FigureShape.Stroke = Brushes.Blue;
            FigureShape.StrokeThickness = 4;
        }

        public void Move(double deltaX, double deltaY)
        {
            firstPoint.X = FigureShape.Margin.Left + deltaX;
            firstPoint.Y = FigureShape.Margin.Top + deltaY;
            FigureShape.Margin = new Thickness(firstPoint.X, firstPoint.Y, 0, 0);
        }

        public bool ContainsPoint( Point point )
        {
            return point.X >= X
                && point.X <= X + Width
                && point.Y >= Y
                && point.Y <= Y + Height;
        }

        public void Modify(Point toPoint)
        {
            if (FigureShape is Polyline)
            {
                ModifyLine(toPoint);
            }
            else
            {
                ModifyClosedFigure(toPoint);
            }
        }

        private void ModifyLine(Point toPoint)
        {
            if (FigureShape is Polyline pl)
            {
                pl.Points.Add(toPoint);
            }
        }

        private void ModifyClosedFigure(Point toPoint)
        {
            var r = new RectArea(firstPoint, toPoint);
            FigureShape.Margin = new Thickness(r.X, r.Y, 0, 0);
            FigureShape.Width = r.Width;
            FigureShape.Height = r.Height;
        }
    }
}
