using System;
using System.Collections.Generic;
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
        Semaphore sem = new Semaphore(0,1);


        public matrixMultiplicationParallel(int size)
        {
            //set the size of the matrixes
            this.size = size;

            //intialize the matrixes
            mult = new int[size, size];
            result = new int[size, size];

            //intialize a matrix
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                {
                    mult[i,j] = 0;
                }

            //set parallel index
            parallelIndex = 0;
        }

        public void compute()
        {
            Console.WriteLine("compute");
            // Multiplying matrix a and b and storing in array mult.
            Thread t1 = new Thread(new ThreadStart(computeLine));
            Thread t2 = new Thread(new ThreadStart(computeLine));
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

        }

        //compute the line for each cell of the matrix
        public void computeLine()
        {
            while(parallelIndex < size)
            {
                Console.WriteLine("start");
                sem.WaitOne();
                int i = parallelIndex;
                parallelIndex++;
                Console.WriteLine("parallel index : {0}",parallelIndex);
                sem.Release();
                Console.WriteLine("end");

                for (int j = 0; j < size; ++j)
                    for (int k = 0; k < size; ++k)
                    {
                        result[i, j] += mult[i, k] * mult[k, j];
                    }
            }
            
        }
    }
}
