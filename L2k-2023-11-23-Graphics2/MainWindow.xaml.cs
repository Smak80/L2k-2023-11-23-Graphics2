using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace L2k_2023_11_23_Graphics2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<MyFigure> fList = new List<MyFigure>();
        private MyFigure? currentFigure = null;
        private ToolType currentTool = ToolType.Ellipse;

        private bool newFigure = true;

        private Point? downPoint = null;
        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            downPoint = e.GetPosition((IInputElement)sender);
            currentFigure = null;
            if (downPoint is { } point)
            {
                fList.ForEach(
                    f => { if (f.ContainsPoint(point)) currentFigure = f; }
                );
                newFigure = currentFigure == null;
                if (newFigure)
                {
                    fList.Add(new MyFigure(
                        currentTool, 
                        point, 
                        new SolidColorBrush(BtnFillColor.Color),
                        new SolidColorBrush(BtnStrokeColor.Color)
                        )
                    );
                    currentFigure = fList.Last();
                    MainCanvas.Children.Add(currentFigure.FigureShape);
                }
            }
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            downPoint = null;
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPoint = e.GetPosition((IInputElement)sender);
            if (downPoint is { } point)
            {
                if (newFigure)
                    currentFigure?.Modify(currentPoint);
                else
                {
                    currentFigure?.Move(
                        currentPoint.X - point.X, currentPoint.Y - point.Y
                    );
                    downPoint = currentPoint;
                }
            }
        }

        private void Tool_Click(object sender, RoutedEventArgs e)
        {
            if (sender == ToolRectangle)
            {
                currentTool = ToolType.Rectangle;
            } else if (sender == ToolEllipse)
            {
                currentTool = ToolType.Ellipse;
            }
        }
    }
}
