using System;
using System.Collections.Generic;
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

namespace matrixSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        matrixMultiplicationParallel para;
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("compute");
            // para = new matrixMultiplicationParallel(5);
        }

        private void computeButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("compute");
           // para.compute();
        }
    }
}
