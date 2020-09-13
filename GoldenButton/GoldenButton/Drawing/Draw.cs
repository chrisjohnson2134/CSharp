using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace GoldenButton.Drawing
{
    public static class Draw
    {
        public static Ellipse drawBlueEllipse(Canvas gridCanvas,string numSpacesTextBox, double x, double y)
        {
            SolidColorBrush bluBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            Ellipse ell1 = new Ellipse();
            ell1.Height = (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox)) / 2;
            ell1.Width = (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox)) / 2;
            ell1.Fill = bluBrush;
            ell1.Margin = new Thickness(x, y, 0, 0);//placement
            return ell1;
        }

        public static Ellipse drawGoldEllipse(Canvas gridCanvas,string numSpacesTextBox, double x, double y)
        {
            SolidColorBrush bluBrush = Brushes.Goldenrod;
            Ellipse ell1 = new Ellipse();
            ell1.Height = (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox)) / 2;
            ell1.Width = (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox)) / 2;
            ell1.Fill = bluBrush;
            ell1.Margin = new Thickness(x, y, 0, 0);//placement
            return ell1;
        }

        public static Line drawLine(double x1, double y1, double x2, double y2)
        {
            Line currLine = new Line();
            currLine.Stroke = System.Windows.Media.Brushes.Black;
            currLine.X1 = x1;
            currLine.Y1 = y1;
            currLine.X2 = x2;
            currLine.Y2 = y2;
            currLine.StrokeThickness = 1;
            return currLine;
        }
    }
}
