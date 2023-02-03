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

namespace ListDragToCanvas.Views
{
    /// <summary>
    /// Interaction logic for DragListingView.xaml
    /// </summary>
    public partial class TransactionsDragListView : UserControl
    {
        public TransactionsDragListView()
        {
            InitializeComponent();
        }



        public ICommand RemoveItemCommand
        {
            get { return (ICommand)GetValue(RemoveItemCommandProperty); }
            set { SetValue(RemoveItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveItemCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveItemCommandProperty =
            DependencyProperty.Register("RemoveItemCommand", typeof(ICommand), typeof(TransactionsDragListView));


        public object RemovedItem
        {
            get { return (object)GetValue(RemovedItemProperty); }
            set { SetValue(RemovedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemovedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemovedItemProperty =
            DependencyProperty.Register("RemovedItem", typeof(object), typeof(TransactionsDragListView),
                new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




        private void ListViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && 
                sender is FrameworkElement frameworkElement)
            {
                object draggableItem = frameworkElement.DataContext;

                DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement,
                    new DataObject(DataFormats.Serializable, draggableItem),
                    DragDropEffects.Move);

                //if(dragDropResult == DragDropEffects.None)
                //{
                //    addItem(draggableItem);
                //}
            }
        }

        private void DragListView_DragLeave(object sender, DragEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(DragListView, e.GetPosition(DragListView));

            if(result == null)
            {
                if(RemoveItemCommand?.CanExecute(null) ?? false)
                {
                    RemovedItem = e.Data.GetData(DataFormats.Serializable);
                    RemoveItemCommand?.Execute(null);
                }
            }
        }
    }
}
