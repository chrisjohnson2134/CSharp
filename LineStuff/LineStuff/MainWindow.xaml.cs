using LineStuff.DrawLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LineStuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Line aLine = new Line() { Stroke = Brushes.Blue, StrokeThickness = 10, X1 = 10, X2 = 300, Y1 = 10, Y2 = 10 };
        Line bLine = new Line() { Stroke = Brushes.Blue, StrokeThickness = 10, X1 = 10, X2 = 300, Y1 = 10, Y2 = 10 };
        Line cLine = new Line() { Stroke = Brushes.Blue, StrokeThickness = 10, X1 = 10, X2 = 300, Y1 = 10, Y2 = 10 };

        BoxLabel bxLabel1 = new BoxLabel() { Name = "b1", Width = 50, Height = 25 };
        BoxLabel bxLabel2 = new BoxLabel() { Name = "b2", Width = 50, Height = 25 };
        BoxLabel bxLabel3 = new BoxLabel() { Name = "b3", Width = 50, Height = 25 };

        BoxLabel bxLabel4 = new BoxLabel() { Name = "b4", Width = 50, Height = 25 };
        BoxLabel bxLabel5 = new BoxLabel() { Name = "b5", Width = 50, Height = 25 };
        BoxLabel bxLabel6 = new BoxLabel() { Name = "b6", Width = 50, Height = 25 };

        Dictionary<string, List<BoxLabel>> map = new Dictionary<string, List<BoxLabel>>();

        public MainWindow()
        {
            InitializeComponent();

            addBx(bxLabel1, 10, 10);
            addBx(bxLabel2, 60, 10);
            addBx(bxLabel3, 120, 10);

            addBx(bxLabel4, 10, 70);
            addBx(bxLabel5, 60, 70);
            addBx(bxLabel6, 120, 70);

            //paintSurface.Children.Add(aLine);

        }

        private void addBx(BoxLabel bxLabel,int top, int left)
        {
            paintSurface.Children.Add(bxLabel);
            Canvas.SetTop(bxLabel, top);
            Canvas.SetLeft(bxLabel, left);
            bxLabel.MouseEnter += MainThumb_MouseEnter;
            bxLabel.MouseLeave += MainThumb_MouseLeave;
            bxLabel.MouseDown += BxLabel_MouseDown;
        }

        bool firstClick;
        string output;

        private void BxLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var bx = (sender as BoxLabel);
            if (!firstClick)
            {
                output = String.Empty;
                output += bx.Name;
                firstClick = true;
            }
            else
            {
                if (!map.ContainsKey(output))
                    map[output] = new List<BoxLabel>();

                map[output].Add(bx);
                output += " + " + bx.Name;
                firstClick = false;
                ConnectionsListBox.Items.Add(output);
            }
        }

        List<Line> tempLines = new List<Line>();

        private void MainThumb_MouseEnter(object sender, MouseEventArgs e)
        {
            var bxlbl = (sender as BoxLabel);
            if (!map.ContainsKey(bxlbl.Name))
                return;

            foreach (var item in map[bxlbl.Name])
            {
                var line = drawLine(bxlbl, item);
                tempLines.Add(line);
                paintSurface.Children.Add(line);
            }
            
        }

        private void MainThumb_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach (var item in tempLines)
            {
                paintSurface.Children.Remove(item);
            }

        }

        private Line drawLine(BoxLabel from,BoxLabel to)
        {
            Line tempLine = new Line();
            tempLine.X1 = Canvas.GetLeft(from);
            tempLine.Y1 = Canvas.GetTop(from);

            tempLine.X2 = Canvas.GetLeft(to);
            tempLine.Y2 = Canvas.GetTop(to);

            tempLine.StrokeThickness = 4;
            tempLine.Stroke = Brushes.Green;

            return tempLine;
        }



        Point currentPoint = new Point();

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (e.ButtonState == MouseButtonState.Pressed)
            //    currentPoint = e.GetPosition(this);

            //if (e.ChangedButton == MouseButton.Left)
            //{
            //    Point mousePosition = Mouse.GetPosition(this);
            //    if (paintSurface.Children.Contains((UIElement)e.Source) && e.ClickCount == 1)
            //    {
            //        _selectedElement = (UIElement)e.Source;
            //        double x = Canvas.GetLeft(_selectedElement);
            //        double y = Canvas.GetTop(_selectedElement);
            //        Point elementPosition = new Point(x, y);
            //        _draggingDelta = elementPosition - mousePosition;
            //    }
            //    _dragging = true;

            //}
        }

        private void Canvas_MouseUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        { 
            //_dragging = false;
        }


        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //if (e.RightButton == MouseButtonState.Pressed)
            //{
            //    Point mousePosition = _transform.Inverse.Transform(e.GetPosition(this));
            //    Vector delta = Point.Subtract(mousePosition, _initialMousePosition);
            //    var translate = new TranslateTransform(delta.X, delta.Y);
            //    _transform.Matrix = translate.Value * _transform.Matrix;

            //    foreach (UIElement child in paintSurface.Children)
            //    {
            //        child.RenderTransform = _transform;
            //    }
            //}

            //if (_dragging && e.LeftButton == MouseButtonState.Pressed)
            //{
            //    double x = Mouse.GetPosition(this).X;
            //    double y = Mouse.GetPosition(this).Y;

            //    if (_selectedElement != null)
            //    {
            //        Canvas.SetLeft(_selectedElement, x + _draggingDelta.X);
            //        Canvas.SetTop(_selectedElement, y + _draggingDelta.Y);
            //    }
            //}

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //var newpoint = e.GetPosition(this);

                //aLine.X2 = paintSurface.Width / 2;
                //aLine.Y2 = newpoint.Y / 2;

                //bLine.X1 = aLine.X2;
                //bLine.X2 = newpoint.X;
                //bLine.Y2 = newpoint.Y / 2;
                //bLine.Y1 = newpoint.Y / 2;

                //cLine.X1 = newpoint.X;
                //cLine.X2 = newpoint.X;
                //cLine.Y1 = newpoint.Y;
                //cLine.Y2 = newpoint.Y - (newpoint.Y / 2);

            }
        }

    }
}
