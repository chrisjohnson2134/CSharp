using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace matrixSolver2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        BackgroundWorker _myWorker = new BackgroundWorker();
        private double _workerState;

        public event PropertyChangedEventHandler PropertyChanged;

        public double WorkerState
        {
            get { return _workerState; }
            set
            {
                _workerState = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("worker State"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            /*int threadCount = Convert.ToInt32(threadCountTB.Text);
            matrixMultiplicationParallel a = new matrixMultiplicationParallel(1000, threadCount);
            a.compute();*/


            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int threadCount = Convert.ToInt32(threadCountTB.Text);
            matrixMultiplicationParallel a = new matrixMultiplicationParallel(1000, threadCount,parallelBar);
            a.compute();
        }



    }



        

    }

