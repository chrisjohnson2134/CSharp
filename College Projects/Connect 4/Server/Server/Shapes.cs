using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


/*
 * This Class Contains all of the game logic
 * Functions:
 *      drawEllipse
 */


namespace Server
{
    class Shapes
    {

        /*
         * Function: drawEllipse
         * Parameters: double x, double y,SolidcolorBrush brush
         * Description: Using the x,y variable to specify the offset position in the margin & brush to specify the color
         */
        public Ellipse drawEllipse(double xPos, double yPos, SolidColorBrush brush)
        {
            //SolidColorBrush bluBrush = Brushes;
            Ellipse ell1 = new Ellipse();
            ell1.Height = 40;
            ell1.Width = 40;
            ell1.Fill = brush;
            ell1.Margin = new Thickness(xPos, yPos, 0, 0);//placement
            return ell1;
        }

    }
}
