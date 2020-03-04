using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Controls;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace imageToAscii
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage origonalImageBMP;
        WriteableBitmap newImageBMP;
        List<Tuple<int, char>> weights = new List<Tuple<int, char>>();
        int widthCharPixels, heightCharPixels;
        int FONT_SIZE = 1;
        string FONT = "Consolas";

        public MainWindow()
        {
            InitializeComponent();
        }

        //parsing the image to the other image box
        private void parse_Click(object sender, RoutedEventArgs e)
        {
            //button will load the image the user would like to convert to the screen and make a copy in the global scope for manipulation.
            Console.WriteLine("PARSER STARTED");
            WriteableBitmap img;
            try
            {
                img = new WriteableBitmap(origonalImageBMP);
            }
            catch { MessageBox.Show("You Must Load an Image.");return; }
            
            //WriteableBitmap newImg = img;
            string imgText = "";
            List<string> listText = new List<string>();
            List<int> avg = new List<int>();
            int sum = 0;
            double averagetemp = 0;
            double diffFromAvg = 1000;


            Console.WriteLine(widthCharPixels + " " + heightCharPixels);
            //read in an width by height of the space that a char takes up and measure the weight
            //find the closest weight to the pre-calculated weight found.
            if (widthCharPixels != 0 | heightCharPixels != 0)
            {
                for (int j = 0; j < img.PixelHeight; j += heightCharPixels)
                {
                    for (int i = 0; i < img.PixelWidth; i += widthCharPixels)
                    {
                        for (int y = j; y < heightCharPixels + j; y++)
                        {
                            for (int x = i; x < widthCharPixels + i; x++)
                                avg.Add(GetPixel(img, x, y).R);
                        }
                        foreach (int a in avg)
                            sum = sum + a;
                        averagetemp = (double)sum / avg.Count;
                        char charTemp = ' ';
                        //Console.WriteLine(averagetemp);
                        foreach (Tuple<int, char> a in weights)
                        {
                            if (diffFromAvg > Math.Abs(averagetemp - a.Item1))
                            {
                                charTemp = a.Item2;
                                diffFromAvg = Math.Abs(averagetemp - a.Item1);
                            }

                        }
                        //Console.WriteLine(charTemp);
                        listText.Add(charTemp.ToString());
                        avg.Clear();
                        sum = 0;
                        diffFromAvg = 1000;
                    }
                    listText.Add("\n");
                }

                //list.Add("\n")
                //newImage.Source = newImg;
                imgText = string.Join("", listText);
                newImageBMP = new WriteableBitmap(textImager(imgText, img.PixelWidth, img.PixelHeight));
                newImage.Source = textImager(imgText, origonalImageBMP.PixelWidth, origonalImageBMP.PixelWidth);

                Console.WriteLine("PARSER FINISHED");
            }
            else
                MessageBox.Show("FONT NOT SELECTED");
        }


        public RenderTargetBitmap textImager(string toDraw,int pixelHeight,int pixelWidth)
        {
            Pen blkPen = new Pen(Brushes.Black, 1);
            FormattedText fmtxt;
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            Console.WriteLine(FONT);
            fmtxt = GetFormattedText(toDraw,FONT, FONT_SIZE);
            dc.DrawText(fmtxt, new Point(10, 10));
            dc.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap(pixelHeight, pixelWidth, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(vis);
            return bmp;
        }

        private FormattedText GetFormattedText(string sTmp,string font, int typeSize)
        {
            FormattedText fmtxt;
            Console.WriteLine("formatted "+font + " " + typeSize);
            
            fmtxt = new FormattedText(sTmp, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(font), typeSize, System.Windows.Media.Brushes.Black);
            return fmtxt;
        }

        public void SetPixel(WriteableBitmap wbm, int x, int y, Color c)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1) return;
            if (y < 0 || x < 0) return;
            if (!wbm.Format.Equals(PixelFormats.Bgr32)) return;
            wbm.Lock();
            IntPtr buff = wbm.BackBuffer;
            int Stride = wbm.BackBufferStride;
            unsafe
            {
                byte* pbuff = (byte*)buff.ToPointer();
                int loc = y * Stride + x * 4;
                pbuff[loc] = c.B;
                pbuff[loc + 1] = c.G;
                pbuff[loc + 2] = c.R;
                pbuff[loc + 3] = c.A;
            }
            wbm.AddDirtyRect(new Int32Rect(x, y, 1, 1));
            wbm.Unlock();
        }

        public Color GetPixel(WriteableBitmap wbm, int x, int y)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1)
                return Color.FromArgb(0, 0, 0, 0);
            if (y < 0 || x < 0)
                return Color.FromArgb(0, 0, 0, 0);
            if (!wbm.Format.Equals(PixelFormats.Bgr32))
                return Color.FromArgb(0, 0, 0, 0); ;
            IntPtr buff = wbm.BackBuffer;
            int Stride = wbm.BackBufferStride;
            Color c;
            unsafe
            {
                byte* pbuff = (byte*)buff.ToPointer();
                int loc = y * Stride + x * 4;
                c = Color.FromArgb(pbuff[loc + 3], pbuff[loc + 2],
                                       pbuff[loc + 1], pbuff[loc]);
            }
            return c;
        }
        
        public double getPixelWeight(int width, int height, int posX, WriteableBitmap img)
        {
            int counterBlack = 0;
            int counterPixels = 0;

            for (int i = posX; i < posX + width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (GetPixel(img, i, j).B < 155)
                        counterBlack++;
                    counterPixels++;
                    //SetPixel(img, i, j, Colors.Black);
                }

            }
            

            return (double)counterBlack / counterPixels;
        }

        //Opens a save dialog so that the user can save it where they want
        private void fileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == false)
                return;

            using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(newImageBMP));
                encoder.Save(stream);
            }
        }

        //opens a load dialog so that the user can load an image
        private void fileLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false)
                return;

            origonalImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            origonalImageBMP = new BitmapImage(new Uri(openFileDialog.FileName));
        }









        private void Consolas_Click_1(object sender, RoutedEventArgs e)
        {
            FONT = "Consolas";
            fontStyleLabel.Content = "Font Style: " + FONT;
            choosen_font(FONT, FONT_SIZE);
        }

        private void Lucida_Click(object sender, RoutedEventArgs e)
        {
            FONT = "Lucida";
            fontStyleLabel.Content = "Font Style: " + FONT;
            choosen_font(FONT, FONT_SIZE);
        }

        private void Courier_New_Click(object sender, RoutedEventArgs e)
        {
            FONT = "Courier";
            fontStyleLabel.Content = "Font Style: " + FONT;
            choosen_font(FONT, FONT_SIZE);
        }

        private void twelve_Click(object sender, RoutedEventArgs e)
        {
            FONT_SIZE = 12;
            fontSizeLabel.Content = "Font Size: " + Convert.ToString(FONT_SIZE);
            choosen_font(FONT, FONT_SIZE);
        }

        private void ten_Click(object sender, RoutedEventArgs e)
        {
            FONT_SIZE = 10;
            fontSizeLabel.Content = "Font Size: " + Convert.ToString(FONT_SIZE);
            choosen_font(FONT, FONT_SIZE);
        }

        private void six_Click(object sender, RoutedEventArgs e)
        {
            FONT_SIZE = 6;
            fontSizeLabel.Content = "Font Size: " + Convert.ToString(FONT_SIZE);
            choosen_font(FONT, FONT_SIZE);
        }

        private void authorAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Author Name: Christopher Johsnon\nSchool: University Of Evansville\nMajor: Computer Science\nEmployment: Embry Automation and Controls Position: Software Speacialist\nField of Interest: Embedded Systems");
        }

        private void tutorial_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to the image to ASCII program!\n1.)To load an image press file on the menu and then load.\n->This will bring up a file explorer for you to go and select a greyscale picture." +
                "\n2.)Use the Font Style drop down to select a font style and the the Font Size drop down to select a font size.\n3.)When you have selected a font style and size press the parse button at the button of the screen display your image in ascii." +
                "\n->feel free to repeat step 2 and three with the same image to get different results" +
                "\n4.)When you are happy with the image feel free to save it by pressing file on the menu bar and selecting save, this will bring up a file explorer and allow you save your image.");
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //parses the font from the bmp's stored in bin and calculates the weights
        private void choosen_font(string font,int size)
        {
            Console.WriteLine("fdsakl: "+FONT + " " + FONT_SIZE);
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine(startupPath);
            string addPath = "";

            if (font.Equals("Courier") && size == 12)
                addPath = @"\Courier_New_12pt.png";

            else if (font.Equals("Courier") && size == 10)
                addPath = @"\Courier_10pt.bmp";

            else if (font.Equals("Courier") && size == 6)
                addPath = @"\Curier_6pt.bmp";

            else if (font.Equals("Consolas") && size == 12)
                addPath = @"\consolas_12pt.bmp";

            else if (font.Equals("Consolas") && size == 10)
                addPath = @"\consolas_10pt.bmp";

            else if (font.Equals("Consolas") && size == 6)
                addPath = @"\consolas_6pt.bmp";

            else if (font.Equals("Lucida") && size == 12)
                addPath = @"\LUCIDA_BRIGHT_12pt.bmp";

            else if (font.Equals("Lucida") && size == 10)
                addPath = @"\LUCIDA_BRIGHT_10pt.bmp";

            else if (font.Equals("Lucida") && size == 6)
            {
                addPath = @"\consolas_6pt.bmp";
                MessageBox.Show("defaulted consolas 6pt");
            }
                
            //addPath = @"\LUCIDA_BRIGHT_6pt.bmp";
            else
                addPath = @"\consolas_10pt.bmp";

            
            BitmapImage a = new BitmapImage(new Uri(startupPath + addPath));
            WriteableBitmap img = new WriteableBitmap( a );
            

            int height = 0;
            int width = 0;
            //button will load the image the user would like to convert to the screen and make a copy in the global scope for manipulation.


            //WriteableBitmap img = new WriteableBitmap(origonalImageBMP);

            for (int i = 0; i < img.PixelHeight; i++)
            {
                //BS shouldn't  it be zero???
                if (GetPixel(img, 0, i).R < 155)
                {
                    height = i;
                }
            }

            for (int i = 0; i < img.PixelWidth; i++)
            {
                if (GetPixel(img, i, height).R > 200)
                {
                    width = i;
                    break;
                }
            }

            //origonalImage.Source = img;
            widthCharPixels = width;
            heightCharPixels = height;
            Console.WriteLine(width + " " + height);
            List<Tuple<int, char>> list = new List<Tuple<int, char>>();
            int constant = 255;
            //Console.WriteLine(getPixelWeight(width, height, 0, img));

            try
            {


                //UNDERSCORE LETTER
                list.Add(new Tuple<int, char>(Convert.ToInt32((1 - getPixelWeight(width, height, 0, img)) * constant), '_'));


                List<int> eliminateDouble = new List<int>();
                int tempPixelweight = 0;

                //UPERCASE LETTERS
                for (int i = 1; i < 25; i++)
                {
                    //Console.WriteLine(getPixelWeight(width, height, i * width, img));
                    tempPixelweight = Convert.ToInt32((1 - getPixelWeight(width, height, i * width, img)) * constant);



                    //Console.WriteLine(tempPixelweight);
                    if (!eliminateDouble.Contains(tempPixelweight))
                    {
                        eliminateDouble.Add(tempPixelweight);
                        weights.Add(new Tuple<int, char>(tempPixelweight, (char)(64 + i)));
                    }
                }

                //LOWERCASE LETTERS
                for (int i = 25; i < 49; i++)
                {
                    tempPixelweight = Convert.ToInt32((1 - getPixelWeight(width, height, i * width, img)) * constant);


                    //Console.WriteLine(tempPixelweight);
                    if (!eliminateDouble.Contains(tempPixelweight))
                    {
                        eliminateDouble.Add(tempPixelweight);
                        weights.Add(new Tuple<int, char>(tempPixelweight, (char)(97 + (i - 24))));
                    }
                }

                //SPACE LETTER
                weights.Add(new Tuple<int, char>(Convert.ToInt32((1 - getPixelWeight(width, height, 49 * width, img)) * constant), ' '));

                ////Period LETTER
                //list.Add(new Tuple<int, char>(Convert.ToInt32((1-getPixelWeight(width, height,  50 * width, img)) * constant), '.'));

                //foreach (Tuple<int, char> a in weights)
                //    Console.WriteLine(Convert.ToInt16(a.Item1) + " " + a.Item2);
                Console.WriteLine("VALUES CALCULATED");
            }
            catch { MessageBox.Show("FAILED TO PARSE"); }

        }

        
    }
}
