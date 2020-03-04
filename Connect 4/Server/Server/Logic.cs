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

        public bool winner(int x,int y,char curr)
        {
            if (checkLine(x, y, 0, 1,curr)) return true; // check up and down
            if (checkLine(x, y, 1, 0,curr)) return true; // check right and left
            if (checkLine(x, y, 1, -1,curr)) return true; // check  45 degrees
            if (checkLine(x, y, 1, 1,curr)) return true; // check -45 degrees


            return false;
        }
        public bool checkLine(int x, int y, int xChange, int yChange,char curr)
        {
            int count, tempX = x, tempY = y;
            //check up
            count = 1;
            while (true)
            {
                tempX += xChange;
                tempY += yChange;
                if (inBounds(tempX, tempY) == false) break;
                else if (board[tempX, tempY] == curr) count++;
                else break;
            }
            //check down
            tempY = y; tempX = x;
            while (true)
            {
                tempX -= xChange;
                tempY -= yChange;
                if (inBounds(tempX, tempY) == false) break;
                else if (board[tempX, tempY] == curr) count++;
                else break;
            }

            if (count >= 4)
                return true;
            return false;
        }
        public bool inBounds(int x, int y)
        {
            if (!(x >= 0 && x < 7))
                return false;
            if (!(y >= 0 && y <= 5))
                return false;
            return true;
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

        public string boardSpots()
        {
            string output = "board:";
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                    if (board[j, i] == 'r' || board[j, i] == 'y')
                        output += i + "," + j + "," + board[j, i] + ";";
            }
            return output;
        }
    }
}
