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
        private MyFigure currentFigure;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fList.Add(new MyFigure());
            MainCanvas.Children.Add(fList.Last().FigureShape);
        }

        private Point? startPoint = null;
        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition((IInputElement)sender);
            if (startPoint is { } point) {
                fList.ForEach(
                    f => { if (f.ContainsPoint(point)) currentFigure = f; }
                );
            }
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            startPoint = null;
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (startPoint is { } point)
            {
                if (currentFigure.ContainsPoint(point))
                {
                    var currentPoint = e.GetPosition((IInputElement)sender);
                    currentFigure.Move(point, currentPoint);
                    startPoint = currentPoint;
                }
            }
        }
    }
}
