using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using Alea;
using Alea.CSharp;
using Alea.Parallel;

namespace matrixSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Length;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Length = 5000;

        }

        private void compBTN_Click(object sender, RoutedEventArgs e)
        {
            //int threadCount = Convert.ToInt32(threadCountTB.Text);
            //matrixMultiplicationParallel para = new matrixMultiplicationParallel(Length, threadCount, parallelBar, paraTimeLBL);

            //para.compute();



            //Stopwatch a = new Stopwatch();


            //double[,] mult;
            //double[,] result;
            //int size = Length;
            //mult = new double[size, size];
            //result = new double[size, size];

            ////intialize a matrix
            //for (int i = 0; i < size; ++i)
            //    for (int j = 0; j < size; ++j)
            //    {
            //        mult[i, j] = 0;
            //    }

            //a.Start();
            //RunGpuPacked(mult, mult, result);
            //a.Stop();

            //Console.WriteLine(a.ElapsedMilliseconds);

        }

        private void resetBTN_Click(object sender, RoutedEventArgs e)
        {
            parallelBar.Value = 0;
            paraTimeLBL.Content = 0;
        }


        //gpu temp
        private const int BlockSize = 32;

        [GpuManaged]
        public static void RunGpuPacked(double[,] a, double[,] b, double[,] c)
        {
            var lp = LaunchParam(a, b, c);
            var aFlat = Pack(a);
            var bFlat = Pack(b);
            var cFlat = new double[c.Length];
            Gpu.Default.Launch(KernelPacked, lp, aFlat, bFlat, cFlat, a.GetLength(1), b.GetLength(1), c.GetLength(1));
            Unpack(cFlat, c);
        }

        private static LaunchParam LaunchParam(double[,] a, double[,] b, double[,] c)
        {
            Check(a, b, c);
            var blockSize = new dim3(BlockSize, BlockSize);
            var gridSize = new dim3(DivUp(a.GetLength(0), BlockSize), DivUp(b.GetLength(1), BlockSize));
            return new LaunchParam(gridSize, blockSize);
        }


        private static double[] Pack(double[,] a)
        {
            var flat = new double[a.Length];
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < cols; j++)
                    flat[i * cols + j] = a[i, j];
            return flat;
        }

        [GpuManaged]
        private static void Unpack(double[] aFlat, double[,] a)
        {
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < cols; j++)
                    a[i, j] = aFlat[i * cols + j];
        }

        private static void Check(double[,] a, double[,] b, double[,] c)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (c == null) throw new ArgumentNullException(nameof(c));
            Debug.Assert(a.GetLength(1) == b.GetLength(0));
            Debug.Assert(a.GetLength(0) == c.GetLength(0));
            Debug.Assert(b.GetLength(1) == c.GetLength(1));
        }

        private static int DivUp(int num, int den)
        {
            return (num + den - 1) / den;
        }

        private static void KernelPacked(double[] a, double[] b, double[] c, int colsA, int colsB, int colsC)
        {
            var blockRow = blockIdx.x;
            var blockCol = blockIdx.y;

            var valueC = 0.0;

            var row = threadIdx.x;
            var col = threadIdx.y;

            for (var m = 0; m < DivUp(colsA, BlockSize); ++m)
            {
                var subA = __shared__.Array2D<double>(BlockSize, BlockSize);
                var subB = __shared__.Array2D<double>(BlockSize, BlockSize);

                subA[row, col] = GetMatrixElement(colsA, a, blockRow, m, row, col);
                subB[row, col] = GetMatrixElement(colsB, b, m, blockCol, row, col);
                DeviceFunction.SyncThreads();

                for (var e = 0; e < BlockSize; ++e)
                {
                    valueC += subA[row, e] * subB[e, col];
                }
                DeviceFunction.SyncThreads();
            }

            SetMatrixElement(colsC, c, blockRow, blockCol, row, col, valueC);
        }

        private static void SetMatrixElement(int ld, double[] matrix, int blockRow, int blockCol, int row, int col,
            double value)
        {
            var globalRow = blockRow * BlockSize + row;
            var globalCol = blockCol * BlockSize + col;
            var globalIdx = globalRow * ld + globalCol;
            if (globalIdx < matrix.Length)
                matrix[globalIdx] = value;
        }

        private static double GetMatrixElement(int ld, double[] matrix, int blockRow, int blockCol, int row, int col)
        {
            var globalRow = blockRow * BlockSize + row;
            var globalCol = blockCol * BlockSize + col;
            var globalIdx = globalRow * ld + globalCol;
            if (globalIdx < matrix.Length)
                return matrix[globalIdx];
            else
                return 0.0;
        }
    }



        

    }

