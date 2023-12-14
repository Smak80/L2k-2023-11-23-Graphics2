using System;
using System.Collections.Generic;
using System.IO;
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
            }
            else if (sender == ToolEllipse)
            {
                currentTool = ToolType.Ellipse;
            }
            else if (sender == ToolPen)
            {
                currentTool = ToolType.Pen;
            }
        }

        private void MainCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            downPoint = null;
        }

        private void File_Save(object sender, RoutedEventArgs e)
        {
            var rtb = GetRenderTargetBitmapFromControl(MainCanvas);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            var result = new BitmapImage();

            using (var memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = memoryStream;
                result.EndInit();
            }

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(result));

            using (var fs = File.OpenWrite("pic.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        private static BitmapSource GetRenderTargetBitmapFromControl(Visual targetControl, double dpi = 96d)
        {
            if (targetControl == null) return null;

            var bounds = VisualTreeHelper.GetDescendantBounds(targetControl);
            var renderTargetBitmap = new RenderTargetBitmap((int)(bounds.Width * dpi / 96.0),
                                                            (int)(bounds.Height * dpi / 96.0),
                                                            dpi,
                                                            dpi,
                                                            PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();

            using (var drawingContext = drawingVisual.RenderOpen())
            {
                var visualBrush = new VisualBrush(targetControl);
                drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
            }

            renderTargetBitmap.Render(drawingVisual);
            return renderTargetBitmap;
        }
    }
}
