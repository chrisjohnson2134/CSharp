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
        

        public DragDropCanvas()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(DragDropCanvas),
                new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(DragDropCanvas),
                new PropertyMetadata(Brushes.Black));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty RectangleDropCommandProperty =
            DependencyProperty.Register("RectangleDropCommand", typeof(ICommand), typeof(DragDropCanvas),
                new PropertyMetadata(null));

        public ICommand RectangleDropCommand
        {
            get { return (ICommand)GetValue(RectangleDropCommandProperty); }
            set { SetValue(RectangleDropCommandProperty, value); }
        }

        public static readonly DependencyProperty RectangleRemoveCommandProperty =
            DependencyProperty.Register("RectangleRemoveCommand", typeof(ICommand), typeof(DragDropCanvas),
                new PropertyMetadata(null));

        public ICommand RectangleRemoveCommand
        {
            get { return (ICommand)GetValue(RectangleRemoveCommandProperty); }
            set { SetValue(RectangleRemoveCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveRectangleNameProperty =
            DependencyProperty.Register("RemoveRectangleName", typeof(string), typeof(DragDropCanvas),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string RemoveRectangleName
        {
            get { return (string)GetValue(RemoveRectangleNameProperty); }
            set { SetValue(RemoveRectangleNameProperty, value); }
        }

        private void RedRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildHitTestVisible = false;
                DragDrop.DoDragDrop(redRectangle, new DataObject(DataFormats.Serializable, redRectangle), DragDropEffects.Move);
                IsChildHitTestVisible = true;
            }
        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is FrameworkElement element)
            {
                canvas.Children.Remove(element);
                RemoveRectangleName = element.Name;
                RectangleRemoveCommand?.Execute(null);
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            // Save the location to a database?
            RectangleDropCommand?.Execute(null);
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);

                if (!canvas.Children.Contains(element))
                {
                    canvas.Children.Add(element);
                }
            }
        }
    }
}

