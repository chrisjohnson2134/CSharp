using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragAndDrop
{
    /// <summary>
    /// Interaction logic for DragDropCanvas.xaml
    /// </summary>
    public partial class DragDropCanvas : UserControl
    {
        public static DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(DragDropCanvas), new PropertyMetadata(true));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(DragDropCanvas),
            new PropertyMetadata(Brushes.Blue));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public DragDropCanvas()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildHitTestVisible= false;
                DragDrop.DoDragDrop(rectangle, new DataObject(DataFormats.Serializable, rectangle), DragDropEffects.Move);
                IsChildHitTestVisible = true;
            }
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);

                if(!canvas.Children.Contains(element))
                    canvas.Children.Add(element);
            }
        }

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);

                if (!canvas.Children.Contains(element))
                    canvas.Children.Add(element);
            }
        }

        private void canvas_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == canvas)
            {
                object data = e.Data.GetData(DataFormats.Serializable);

                if (data is UIElement element)
                {
                    canvas.Children.Remove(element);
                }
            }

        }
    }
}
