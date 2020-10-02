using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace matrixSolver
{
    //TODO
    // -> pass in arrays
    // -> pass back final array
    // -> get size based off array passed
    // -> make similar changes to parallel algorithm
    class matrixMultiplication : matrix
    {
        int size = 0;
        int[,] mult;
        System.Windows.Controls.TextBox time;

        public matrixMultiplication(int size, System.Windows.Controls.TextBox time)
        {
            this.size = size;
            mult = new int[size, size];
            this.time = time;
        }

        public void compute()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int[,] result = new int[size, size];
            // Multiplying matrix a and b and storing in array mult.
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    for (int k = 0; k < size; ++k)
                    {
                        result[i, j] += mult[i, k] * mult[k, j];
                    }
            watch.Stop();

            time.Dispatcher.Invoke(() => { time.Text = watch.ElapsedMilliseconds.ToString(); });
        }

    }

}

