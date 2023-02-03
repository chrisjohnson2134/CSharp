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

namespace LineStuff.DrawLibrary
{
    /// <summary>
    /// Interaction logic for BoxLabel.xaml
    /// </summary>
    public partial class BoxLabel : UserControl
    {
        public BoxLabel()
        {
            InitializeComponent();
        }

        void onDragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            //Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            //Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);

            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        }

        private void Thumb_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Thumb_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
