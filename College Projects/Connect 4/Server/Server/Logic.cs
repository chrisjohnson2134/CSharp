using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * This Class Contains all of the game logic
 * Functions:
 *      MOVE
 *      WINNER
 *      CHECKLINE
 *      INBOUNDS
 *      PRINTBOARD
 */

namespace Server
{
    class gameLogic
    {
        char[,] board = new char[7, 6];
        public char currPlayer = 'r';

        /*
         * Constructor:gameLogic()
         * Description-> By default a Connect 4 Board is Generated
         */
        public gameLogic()
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                    board[j, i] = 'x';
        }


        /*
         * Function:move
         * Parameteres: User Input (position of the puck submitted)
         * Returns: A tuple of the location of where to add a puck on the board
         * Description->  Checks to make sure that the user input is valid & the drop spot is empty
         */
        public Tuple<int,int> move(int userInp)
        {
            if (board[userInp, 0] == 'x' && inBounds(userInp,0))
            {
                for (int j = 0; j < 6; j++)
                    if (board[userInp, j] != 'x' || j==5)
                    {
                        if (j == 5 && board[userInp, j] == 'x')
                            board[userInp, j] = currPlayer;
                        else
                        { board[userInp, j - 1] = currPlayer; j = j - 1; }

                        if (currPlayer == 'r')
                            currPlayer = 'y';
                        else
                            currPlayer = 'r';
                        Console.WriteLine();
                        return new Tuple<int, int>(userInp,j);
                    }
            }

            else
                Console.WriteLine("invalid move");

            Console.WriteLine();
            return new Tuple<int, int>(-1, -1);
        }


        /*
         * Function: winner
         * 
         * Description->  
         */
        public bool winner(int x,int y,char curr)
        {
            if (checkLine(x, y, 0, 1,curr)) return true; // check up and down
            if (checkLine(x, y, 1, 0,curr)) return true; // check right and left
            if (checkLine(x, y, 1, -1,curr)) return true; // check  45 degrees
            if (checkLine(x, y, 1, 1,curr)) return true; // check -45 degrees


            return false;
        }



        /*
         * Function:checkLine
         * Parameters: x,y => represent the spot you want to check for a winner
         *             xChange,yChange => Represent which direction you would like to check for a winner
         * Description->  This function will check for 4 continous pucks until it is out of bounds.
         */
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

        /*
         * Function: inBounds
         * Parameters: x,y
         * Description->  checks to see if the position passed is in bounds of the board
         */
        public bool inBounds(int x, int y)
        {
            if (!(x >= 0 && x < 7))
                return false;
            if (!(y >= 0 && y <= 5))
                return false;
            return true;
        }

        /*
         * Function:printBoard()
         * Description-> Writes current Board to the console 
         */
        public void printBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                    Console.Write(board[j, i] + " ");
                Console.WriteLine();
            }

        }

        /*
         * Class:boardSpots()
         * Description-> This function creates a string of characters to send to the clients
         * Each Client is able to parse this string to keep all clients up to date with the game.
         */
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
