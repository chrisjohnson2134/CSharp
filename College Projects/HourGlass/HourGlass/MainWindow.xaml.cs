using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.ComponentModel;

namespace HourGlass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] currHourGlassArr;
        private Queue<int[,]> computationFIFO = new Queue<int[,]>();
        private int arrHeight = 20;
        private int arrWidth = 11;
        private BackgroundWorker generateQueueBackgroundWorker = new BackgroundWorker();
        int length = 0;
        bool pieceMoved = true;
        DispatcherTimer updateTimer = new DispatcherTimer();
        DispatcherTimer flipTimer = new DispatcherTimer();
        int moveCounter = 0;
        int movesMadeCounter = 0;
        bool autoFlip = false;
        int numSandGrains = 30;
        int angle = 0;

        public MainWindow()
        {
            InitializeComponent();
            currHourGlassArr = new int[arrHeight, arrWidth];
            //setup the hour glass
            setupHourGlassArr();
            //draw hourglass
            drawHourGlass();
            //start generating the arrangements 
            setupThreads();


            //start timer//maybe delay it by a second or two.


        }

        private void setupHourGlassArr()
        {
            //int numSandGrains = Convert.ToInt16(amountSandTextBox.Text);

            int ja = 0;
            for (int i = 5; i < 10; i++)
            {
                for (int j = 0; j <= ja; j++)
                    currHourGlassArr[i, j] = 2;
                ja++;
            }

            int jb = arrWidth - 1;
            for (int i = 5; i < 10; i++)
            {
                for (int j = jb; j < arrWidth; j++)
                    currHourGlassArr[i, j] = 2;
                jb--;
            }

            ja--;
            for (int i = 10; i < 15; i++)
            {
                for (int j = 0; j <= ja; j++)
                    currHourGlassArr[i, j] = 2;
                ja--;
            }

            jb++;
            for (int i = 10; i < 15; i++)
            {
                for (int j = jb; j < arrWidth; j++)
                    currHourGlassArr[i, j] = 2;
                jb++;
            }

            int grains = numSandGrains;

            if (!autoFlip)
            {
                for (int i = 0; i < arrHeight; i++)
                    for (int j = 0; j < arrWidth; j++)
                    {
                        if (grains > 0)
                        {
                            grains--;
                            if (currHourGlassArr[i, j] != 2)
                                currHourGlassArr[i, j] = 1;
                        }
                        else
                        {
                            if (currHourGlassArr[i, j] != 2)
                                currHourGlassArr[i, j] = 0;
                        }
                    }
            }
            else
            {
                for (int i = 0; i < arrHeight; i++)
                    for (int j = 0; j < arrWidth; j++)
                    {
                        if (currHourGlassArr[i, j] == 1)
                        {
                            currHourGlassArr[arrHeight - 1 - i, arrWidth - 1 - j] = currHourGlassArr[i, j];
                            currHourGlassArr[i, j] = 0;
                        }
                    }
            }

        }

        private void computeArr()
        {
            for (int i = arrHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < arrWidth; j++)
                {
                    if (currHourGlassArr[i, j] == 1)
                        updateSandGrain(i, j);
                }
            }

            int[,] a = new int[arrHeight, arrWidth];

            for (int i = 0; i < arrHeight; i++)
                for (int j = 0; j < arrWidth; j++)
                    a[i, j] = currHourGlassArr[i, j];

            computationFIFO.Enqueue(a);

            length++;//makes sure we don't call an empty queue
        }

        private void updateSandGrain(int x, int y)
        {
            bool flagLeft = false, flagRight = false;

            if (x + 1 >= 0 && y >= 0)
                if (x + 1 < arrHeight && y < arrWidth)
                    if (currHourGlassArr[x + 1, y] == 0)
                    {
                        currHourGlassArr[x + 1, y] = 1;
                        currHourGlassArr[x, y] = 0;
                        pieceMoved = true;
                        return;
                    }


            if (x + 1 >= 0 && y - 1 >= 0)
                if (x + 1 < arrHeight && y - 1 < arrWidth)
                    if (currHourGlassArr[x + 1, y - 1] == 0)
                    { flagLeft = true; pieceMoved = true; }


            if (x + 1 >= 0 && y + 1 >= 0)
                if (x + 1 < arrHeight && y + 1 < arrWidth)
                    if (currHourGlassArr[x + 1, y + 1] == 0)
                    { flagRight = true; pieceMoved = true; }




            if (flagRight && flagLeft)
            {
                Random rnd = new Random();
                if (rnd.Next(0, 1) == 1)
                    currHourGlassArr[x + 1, y - 1] = 1;
                else
                    currHourGlassArr[x + 1, y + 1] = 1;

                currHourGlassArr[x, y] = 0;
                return;
            }

            else if (flagRight)
                currHourGlassArr[x + 1, y + 1] = 1;


            else if (flagLeft)
                currHourGlassArr[x + 1, y - 1] = 1;

            if (flagLeft | flagRight)
                currHourGlassArr[x, y] = 0;

        }

        private void drawHourGlass()
        {
            Pen blkPen = new Pen(Brushes.Blue, 1);
            Pen greenPen = new Pen(Brushes.Green, 1);
            Pen grayPen = new Pen(Brushes.Gray, 5);
            Pen orangePen = new Pen(Brushes.Orange, 1);
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            //dc.DrawText(fmtxt, new Point(10, 10));

            dc.DrawLine(grayPen, new Point(7, 0), new Point(7, 128));
            dc.DrawLine(grayPen, new Point(7, 0), new Point(77, 0));
            dc.DrawLine(grayPen, new Point(77, 0), new Point(77, 128));
            dc.DrawLine(grayPen, new Point(7, 125), new Point(77, 125));
            if (length > 0)
            {
                var glassItem = computationFIFO.Dequeue();
                length--;
                for (int i = 0; i < arrHeight; i++)
                    for (int j = 0; j < arrWidth; j++)
                        if (j == 5 && i == 9 || j == 5 && i == 10)
                        {
                            if (glassItem[i, j] == 1)
                                dc.DrawEllipse(Brushes.Orange, orangePen, new Point(10 + 2 + 6 * j, 3 + 2 + 6 * i), 2, 2);
                        }
                        else
                        {
                            if (glassItem[i, j] == 1)
                                dc.DrawEllipse(Brushes.Blue, blkPen, new Point(10 + 2 + 6 * j, 3 + 2 + 6 * i), 2, 2);
                            else if (glassItem[i, j] == 2)
                                dc.DrawEllipse(Brushes.Green, greenPen, new Point(10 + 2 + 6 * j, 3 + 2 + 6 * i), 2, 2);
                        }


                dc.Close();
                RenderTargetBitmap bmp = new RenderTargetBitmap(100, 125, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(vis);
                hourGlassImage.Source = bmp;
            }
            else
            {
                var glassItem = currHourGlassArr;
                for (int i = 0; i < arrHeight; i++)
                    for (int j = 0; j < arrWidth; j++)
                        if (glassItem[i, j] == 1)
                            dc.DrawEllipse(Brushes.Blue, blkPen, new Point(10 + 2 + 6 * j, 3 + 2 + 6 * i), 2, 2);
                        else if (glassItem[i, j] == 2)
                            dc.DrawEllipse(Brushes.Green, greenPen, new Point(10 + 2 + 6 * j, 3 + 2 + 6 * i), 2, 2);

                dc.Close();
                RenderTargetBitmap bmp = new RenderTargetBitmap(100, 125, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(vis);
                hourGlassImage.Source = bmp;
            }


        }

        private void setupThreads()
        {
            //start computing all array iterations//soon
            generateQueueBackgroundWorker.WorkerReportsProgress = false;
            generateQueueBackgroundWorker.DoWork += generateQueueWork;
            //generateQueueBackgroundWorker.RunWorkerAsync();
            //start computing images//soon
            updateTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(timeIncSlider.Value));
            //flip timer
            flipTimer.Tick += new EventHandler(flipTimer_Tick);
            flipTimer.Interval = new TimeSpan(0, 0, 0, 0, 200/15);//200
        }

        private void generateQueueWork(object sender, DoWorkEventArgs e)
        {
            while (pieceMoved)
            {
                moveCounter++;
                pieceMoved = false;
                computeArr();
            }
            pieceMoved = true;
            Console.WriteLine("done");

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (length > 0)
                drawHourGlass();

            if (moveCounter == movesMadeCounter && !autoFlip)
            {
                //Console.WriteLine("timer stoped");
                updateTimer.Stop();
                moveCounter = movesMadeCounter = 0;
            }
            else if (moveCounter == movesMadeCounter && autoFlip)
            {
                flipTimer.Start();

                if (angle == 180 )
                {
                    angle = 0;
                    setupHourGlassArr();
                    RotateTransform rotate = new RotateTransform(0);
                    hourGlassImage.RenderTransform = rotate;
                    drawHourGlass();
                    flipTimer.Stop();
                    moveCounter = movesMadeCounter = 0;
                    generateQueueBackgroundWorker.RunWorkerAsync();
                }

            }

            if (!(moveCounter == movesMadeCounter))
                movesMadeCounter++;
        }

        private void runOnceButton_Click(object sender, RoutedEventArgs e)
        {
            RotateTransform rotate = new RotateTransform(0);
            hourGlassImage.RenderTransform = rotate;

            autoFlip = false;
            generateQueueBackgroundWorker.RunWorkerAsync();
            updateTimer.Start();
        }

        private void flipTimer_Tick(object sender, EventArgs e)
        {
            if (angle == 180)
            {
                flipTimer.Stop();
                return;
            }


            angle += 1;
            RotateTransform rotate = new RotateTransform(angle);
            hourGlassImage.RenderTransform = rotate;
        }

        private void runAutoButton_Click(object sender, RoutedEventArgs e)
        {
            autoFlip = true;
            generateQueueBackgroundWorker.RunWorkerAsync();
            updateTimer.Start();
        }

        private void setButton_Click(object sender, RoutedEventArgs e)
        {
            int num;
            autoFlip = false;
            try
            {
                num = Convert.ToInt16(amountSandTextBox.Text);
                if (num < 0)
                    num = 1;
                else if (num > 97)
                    num = 97;
            }
            catch { num = 20; }

            numSandGrains = num;
            amountSandTextBox.Text = Convert.ToString(num);

            //setupHourGlassArr();
            //drawHourGlass();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            updateTimer.Stop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("triggered");
            if (e.Key == Key.Space && autoFlip)
            {
                updateTimer.Stop();
                this.Close();
            }
        }

        private void timeIncSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(timeIncSlider.Value));
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            angle = 0;
            RotateTransform rotate = new RotateTransform(0);
            hourGlassImage.RenderTransform = rotate;
            if (flipTimer.IsEnabled)
                flipTimer.Stop();
            if (updateTimer.IsEnabled)
                updateTimer.Stop();
            if (generateQueueBackgroundWorker.IsBusy)
                generateQueueBackgroundWorker.CancelAsync();
            autoFlip = false;
            length = 0;
            computationFIFO.Clear();
            updateTimer.Stop();
            setupHourGlassArr();
            drawHourGlass();
        }
    }
}
