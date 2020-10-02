using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace matrixSolver
{
    class matrixMultiplicationParallel : matrix
    {
        int size;
        int[,] mult;
        int[,] result;
        int parallelIndex;
        int threadCount;
        public double progress;
        Stopwatch watch = new Stopwatch();
        System.Windows.Controls.TextBox time;
        Semaphore sem = new Semaphore(0, 1);


        public matrixMultiplicationParallel(int size,int threadCount, System.Windows.Controls.TextBox time)
        {
            //set the size of the matrixes
            this.size = size;
            this.threadCount = threadCount;
            this.time = time;
            //intialize the matrixes
            mult = new int[size, size];
            result = new int[size, size];

            //intialize a matrix
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                {
                    mult[i, j] = 0;
                }

            //set parallel index
            parallelIndex = 0;
        }

        public void compute()
        {
            watch.Start();
            // Multiplying matrix a and b and storing in array mult.
            Thread[] threadList = new Thread[threadCount];
            sem.Release();
            for (int i = 0; i < threadCount; i++)
            {
                threadList[i] = new Thread(new ThreadStart(computeLine));
                threadList[i].Start();
            }
        }

        //compute the line for each cell of the matrix
        public void computeLine()
        {
            while (parallelIndex < size)
            {
                sem.WaitOne();
                int i = parallelIndex;
                
                progress =(double) parallelIndex / size;
                
                sem.Release();

                for (int j = 0; j < size; ++j)
                    for (int k = 0; k < size; ++k)
                    {
                        result[i, j] += mult[i, k] * mult[k, j];
                    }

                parallelIndex++;
            }

            watch.Stop();
            TimeSpan b = watch.Elapsed;

            time.Dispatcher.Invoke(() => { time.Text = watch.ElapsedMilliseconds.ToString(); });
            

        }
    }
}
