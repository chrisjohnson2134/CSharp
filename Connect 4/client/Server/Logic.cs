using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class gameLogic
    {
        char[,] board = new char[7, 6];
        public char currPlayer = 'r';
        public gameLogic()
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                    board[j, i] = 'x';
        }

        public Tuple<int,int> move(int i)
        {
            if (board[i, 0] == 'x')
            {
                for (int j = 0; j < 6; j++)
                    if (board[i, j] != 'x' || j==5)
                    {
                        if (j == 5 && board[i, j] == 'x')
                            board[i, j] = currPlayer;
                        else
                        { board[i, j - 1] = currPlayer; j = j - 1; }

                        if (currPlayer == 'r')
                            currPlayer = 'y';
                        else
                            currPlayer = 'r';
                        Console.WriteLine();
                        return new Tuple<int, int>(i,j);
                    }
            }

            else
                Console.WriteLine("invalid move");

            Console.WriteLine();
            return new Tuple<int, int>(-1, -1);
        }

        public void printBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                    Console.Write(board[j, i] + " ");
                Console.WriteLine();
            }

        }
    }
}
