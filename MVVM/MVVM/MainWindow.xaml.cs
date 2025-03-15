using MVVM.Items;
using MVVM.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        public List<DataGridRow> GetDisplayedRows(DataGrid dataGrid)
        {
            var displayedRows = new List<DataGridRow>();
            foreach (var item in dataGrid.Items)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null && row.IsVisible)
                {
                    displayedRows.Add(row);
                }
            }
            return displayedRows;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var output = GetDisplayedRows(MyGrid);
            var output1 = output[0].Item as Item;
            var output2 = output.Last().Item as Item;
        }
    }
}
