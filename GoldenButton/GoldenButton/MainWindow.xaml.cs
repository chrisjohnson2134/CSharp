using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using GoldenButton.Drawing;

namespace GoldenButton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> grid = new List<string>();
        Dictionary<string, Ellipse> listEllipse = new Dictionary<string, Ellipse>();//make a dictionary

        Storyboard pathAnimationStoryboard = new Storyboard();
        TranslateTransform animatedTranslateTransform = new TranslateTransform();
        int LEFTCLICKPIECE = -1;
        string PIECESELECTED = "";
        double FINALX;
        int PLAYERTURN = 0;

        public MainWindow()
        {
            InitializeComponent();

            //add complete event
            pathAnimationStoryboard.Completed += new EventHandler(story_complete);

            // Register the transform's name with the page
            // so that they it be targeted by a Storyboard.
            gridCanvas.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

        }

        private void story_complete(object sender, EventArgs e)
        {

            gridCanvas.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);
            listEllipse[PIECESELECTED].RenderTransform = null;
            listEllipse[PIECESELECTED].Margin = new Thickness(FINALX + 12, (int)(gridCanvas.Height) / 2 - 5, 0, 0);
            //foreach (string a in grid)
            //    Console.Write(a + " ");
            if (listEllipse[PIECESELECTED].Fill == Brushes.Green)
                listEllipse[PIECESELECTED].Fill = Brushes.Blue;
            else
                listEllipse[PIECESELECTED].Fill = Brushes.Goldenrod;
            
            oponentMakeMove();

            if (!checkWin())
                oponentMakeMove();

            if(youLostLabel.Visibility != Visibility.Visible && checkWin())
                youWonLabel.Visibility = Visibility.Visible;

        }

        private void oponentMakeMove()
        {
            if (PLAYERTURN == 1)
            {
                int start = 0, end = 0;
                for (int i = 0; i < Convert.ToInt32(numSpacesTextBox.Text); i++)
                {
                    if (!grid[i].Equals("X") && start == 0)
                    { start = i; break; }


                }
                foreach (string a in grid)
                    Console.Write(a + " ");
                Console.WriteLine(start + " " + end);
                if (listEllipse[grid[start]].Fill == Brushes.Blue)
                    listEllipse[grid[start]].Fill = Brushes.Green;
                else
                    listEllipse[grid[start]].Fill = Brushes.Khaki;
                Console.WriteLine(grid[start]);
                PIECESELECTED = grid[start];
                listEllipse[grid[start]].RenderTransform = animatedTranslateTransform;
                if (start == 0)
                {
                    listEllipse[PIECESELECTED].RenderTransform = null;
                    listEllipse[grid[0]].Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    listEllipse.Remove(grid[0]);
                    takeButton();
                }
                else
                {
                    moveButton(start, 0);
                    animate(start, 0);
                }
                if (checkWin() )
                    youLostLabel.Visibility = Visibility.Visible;

                //PLAYERTURN = 0;
            }
            PLAYERTURN = 0;
        }

        private void animate(int right, int left)
        {
            // Create the animation path.
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();
            pFigure.StartPoint = new Point(0, 0);
            PolyBezierSegment pBezierSegment = new PolyBezierSegment();
            //Point start = new Point(0, 0);
            //Point end = new Point(445, 185);
            LoadPathPoints(pBezierSegment, left, right);
            pFigure.Segments.Add(pBezierSegment);
            animationPath.Figures.Add(pFigure);

            // Freeze the PathGeometry for performance benefits.
            animationPath.Freeze();
            // Create a DoubleAnimationUsingPath to move the
            // rectangle horizontally along the path by animating 
            // its TranslateTransform.
            DoubleAnimationUsingPath translateXAnimation =
                new DoubleAnimationUsingPath();
            translateXAnimation.PathGeometry = animationPath;
            translateXAnimation.Duration = TimeSpan.FromSeconds(2);

            // Set the Source property to X. This makes
            // the animation generate horizontal offset values from
            // the path information. 
            translateXAnimation.Source = PathAnimationSource.X;

            // Set the animation to target the X property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateXAnimation, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(translateXAnimation,
                new PropertyPath(TranslateTransform.XProperty));
            // Create a DoubleAnimationUsingPath to move the
            // rectangle vertically along the path by animating 
            // its TranslateTransform.
            DoubleAnimationUsingPath translateYAnimation =
                new DoubleAnimationUsingPath();
            translateYAnimation.PathGeometry = animationPath;
            translateYAnimation.Duration = TimeSpan.FromSeconds(2);

            // Set the Source property to Y. This makes
            // the animation generate vertical offset values from
            // the path information. 
            translateYAnimation.Source = PathAnimationSource.Y;

            // Set the animation to target the Y property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateYAnimation, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(translateYAnimation,
                new PropertyPath(TranslateTransform.YProperty));

            // Create a Storyboard to contain and apply the animations.
            //Storyboard pathAnimationStoryboard = new Storyboard();//moved to top
            //pathAnimationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
            pathAnimationStoryboard.Children.Add(translateXAnimation);
            pathAnimationStoryboard.Children.Add(translateYAnimation);
            // Start the animations.
            pathAnimationStoryboard.Begin(gridCanvas);


        }

        private void LoadPathPoints(PolyBezierSegment pBezierSegment, int start, int end)
        {
            FINALX = (start * ((int)gridCanvas.Width / Convert.ToInt32(numSpacesTextBox.Text)));
            pBezierSegment.Points.Add(new Point(0, 0));
            pBezierSegment.Points.Add(new Point(((start - end) * ((int)gridCanvas.Width / Convert.ToInt32(numSpacesTextBox.Text))) / 2, -100));
            pBezierSegment.Points.Add(new Point(((start - end) * ((int)gridCanvas.Width / Convert.ToInt32(numSpacesTextBox.Text))), 0));
        }

        

        

        private void drawGrid(int spaces)
        {
            gridCanvas.Children.Add(Draw.drawLine(5, 5, gridCanvas.Width - 2, 5));
            gridCanvas.Children.Add(Draw.drawLine(5, gridCanvas.Height - 5, gridCanvas.Width - 2, gridCanvas.Height - 5));
            Console.WriteLine(spaces);
            for (int i = 0; i < spaces + 1; i++)
            {
                //Console.WriteLine(i * ((int)gridCanvas.Width/spaces));
                double width = (i * (gridCanvas.Width / spaces)) + 5;
                if (width > gridCanvas.Width)
                    width = gridCanvas.Width-2;

                Console.WriteLine(width);

                gridCanvas.Children.Add(Draw.drawLine(width, 5, width, gridCanvas.Height - 5));
            }

            for (int i = 0; i < spaces; i++)
            {
                if (grid[i][0] == 'B')
                {
                    Ellipse created = Draw.drawBlueEllipse(gridCanvas,numSpacesTextBox.Text,i * (gridCanvas.Width / spaces) + (gridCanvas.Width / spaces) / 2, ((gridCanvas.Height) / 2) - (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox.Text)) / 2/2);
                    listEllipse.Add(grid[i], created);
                    gridCanvas.Children.Add(created);//add to canvas
                }
                if (grid[i][0] == 'G')
                {
                    Ellipse created = Draw.drawGoldEllipse(gridCanvas, numSpacesTextBox.Text,i * (gridCanvas.Width / spaces) + (gridCanvas.Width / spaces) / 2, ((gridCanvas.Height) / 2) - (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox.Text)) / 2/2);
                    listEllipse.Add(grid[i], created);
                    gridCanvas.Children.Add(created);//add to canvas
                }
            }

        }

        private bool createGrid(int spaces, int buttons)
        {

            if (spaces < 15 || spaces > 49)
            {
                MessageBox.Show("Values Must be between 16 and 48!!");
                return false;
            }

            for (int i = 0; i < spaces; i++)
                grid.Add("X");

            Random rnd = new Random();
            grid[rnd.Next(spaces * 3 / 4, spaces - 1)] = "G";
            int randTemp = 0;
            for (int i = 0; i < buttons; i++)
            {
                randTemp = rnd.Next(0, spaces - 1);
                if (grid[randTemp].Equals("X"))
                    grid[randTemp] = "B" + i;
                else
                    i--;
            }
            return true;
        }

        private bool moveButton(int from, int to)
        {
            if (to > from)
            { MessageBox.Show("Must Move from left to right."); return false; }

            if (grid[from][0] == 'B' || grid[from][0] == 'G' && from > to)
            {
                for (int i = from - 1; i > to - 1; i--)
                {
                    //Console.Write("from: " + from + " to: " + to);
                    //Console.WriteLine(grid[i]);
                    if (grid[i][0] == 'B' || grid[i][0] == 'G')
                    { MessageBox.Show("You Cannot jump over other pieces."); return false; }
                }
                grid[to] = grid[from];
                grid[from] = "X";
                return true;
            }
            else
                MessageBox.Show("Invalid moves");
            return false;
        }

        private void takeButton()
        {
            if (grid[0][0] != 'X')
            {
                grid[0] = "X";
            }
        }

        private bool checkWin()
        {
            foreach (string a in grid)
                if (a.Equals("G"))
                    return false;
            return true;
        }

        private void gridCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PLAYERTURN == 0)
            {
                Point p = Mouse.GetPosition(gridCanvas);
                LEFTCLICKPIECE = (int)(p.X / (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox.Text)));

                foreach (KeyValuePair<string, Ellipse> entry in listEllipse)
                {
                    if (entry.Value.Fill == Brushes.Khaki)
                        entry.Value.Fill = Brushes.Goldenrod;
                    if (entry.Value.Fill != Brushes.Goldenrod)
                        entry.Value.Fill = Brushes.Blue;
                }

                PIECESELECTED = grid[LEFTCLICKPIECE];
                if (PIECESELECTED != "X")
                    if (listEllipse[PIECESELECTED].Fill != Brushes.Goldenrod)
                        listEllipse[PIECESELECTED].Fill = Brushes.Green;
                    else
                        listEllipse[PIECESELECTED].Fill = Brushes.Khaki;
            }


        }

        private void gridCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PLAYERTURN == 0)
            {
                Point p = Mouse.GetPosition(gridCanvas);
                int rightPiecePiece = (int)(p.X / (gridCanvas.Width / Convert.ToInt32(numSpacesTextBox.Text)));
                Console.WriteLine(LEFTCLICKPIECE);
                if (LEFTCLICKPIECE != -1)
                {
                    if (moveButton(LEFTCLICKPIECE, rightPiecePiece))
                    {
                        listEllipse[PIECESELECTED].RenderTransform = animatedTranslateTransform;
                        animate(LEFTCLICKPIECE, rightPiecePiece);
                        LEFTCLICKPIECE = -1;
                        PLAYERTURN = 1;
                    }
                }
                else if (LEFTCLICKPIECE == -1 && grid[0][0] != 'X')
                {
                    listEllipse[grid[0]].Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    listEllipse.Remove(grid[0]);
                    takeButton();
                    LEFTCLICKPIECE = -1;
                    //Console.WriteLine();
                    //foreach (string a in grid)
                    //    Console.Write(a + " ");
                    PLAYERTURN = 1;
                    if (!checkWin())
                        oponentMakeMove();
                    else 
                    youWonLabel1.Visibility = Visibility.Visible; 

                }
                Console.WriteLine();
                foreach (string a in grid)
                    Console.Write(a + " ");

                //if (checkWin())
                //    youLostLabel.Visibility = Visibility.Visible; 
            }

        }
        

        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (createGrid(Convert.ToInt32(numSpacesTextBox.Text), Convert.ToInt32(numButtonsTextBox.Text)))
                drawGrid(Convert.ToInt32(numSpacesTextBox.Text));
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            gridCanvas.Children.Clear();
            listEllipse.Clear();
            grid.Clear();
            youWonLabel1.Visibility = Visibility.Hidden;
            youLostLabel.Visibility = Visibility.Hidden;
            LEFTCLICKPIECE = -1;
        }

        private void author_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Author Name: Christopher Johnson\n" +
                "School: University Of Evansville\n" +
                "Major: Computer Science\n" +
                "Employment: Embry Automation and Controls Position: Software Speacialist\n" +
                "Field of Interest: Embedded Systems");
        }

        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Rules:\n" +
                "The idea of the game is to pick up the gold button.\n" +
                "You can move any piece to the left but you cannot jump any piece\n" +
                "->To move a piece left click on the piece and right click where you want it to move to.\n" +
                "You can pick up a piece if it is in the far right position.\n" +
                "->To do this right click.\n" +
                "When you Pick up the gold you have won.");
        }
    }
}
