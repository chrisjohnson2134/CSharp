using ListDragToCanvas.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ListDragToCanvas
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            TransactionsDragListViewModel transactionDragListView = new TransactionsDragListViewModel();

            DragSortViewModel dragSortViewModel = new DragSortViewModel(transactionDragListView);

            MainWindow = new MainWindow()
            {
                DataContext = dragSortViewModel
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
