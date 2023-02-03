using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Server
{
    class Connect4Draw : Shapes
    {
        Canvas boardCanvas;
        /*
         * Constructor:
         * Parameters:
         * Description:
         */
         public Connect4Draw(Canvas canvas)
        {
            boardCanvas = canvas;
        }

        /*
         * Function:
         * Parameters:
         * Description:
         */
        public void insertPuck(int i, int j,char player)
        {
            if (player == 'r')
            {
                boardCanvas.Children.Add(drawEllipse(i * 65 + 15, j * 50 + 15, Brushes.Yellow));
            }

            else
            {
                boardCanvas.Children.Add(drawEllipse(i * 65 + 15, j * 50 + 15, Brushes.Red));
            }

        }

    }
}
