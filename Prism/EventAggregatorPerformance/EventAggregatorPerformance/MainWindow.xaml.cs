using EventAggregatorPerformance.ViewModel;
using System.Windows;

namespace EventAggregatorPerformance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new PerformanceViewModel();
        }
    }
}
