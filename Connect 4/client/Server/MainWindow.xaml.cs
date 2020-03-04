using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        delegate void SetTextCallback(String text);
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        Ellipse currElipse;
        gameLogic logic = new gameLogic();
        int currPosEllipse = 0;

        public MainWindow()
        {
            InitializeComponent();
            drawBoard();
            //connectButton.IsTabStop = false;
            //sendButton.IsTabStop = false;
            nameTextBox.IsTabStop = false;
            textBox1.IsTabStop = false;
            winnerTextBox.IsTabStop = false;
            oponentTextBox.IsTabStop = false;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            
        }

        


        #region draw
        private Ellipse drawEllipse(double x, double y, SolidColorBrush brush)
        {
            //SolidColorBrush bluBrush = Brushes;
            Ellipse ell1 = new Ellipse();
            ell1.Height = 40;
            ell1.Width = 40;
            ell1.Fill = brush;
            ell1.Margin = new Thickness(x, y, 0, 0);//placement
            return ell1;
        }

        private void insertPuck(int i, int j, char curr)
        {
            Console.WriteLine(curr);
            if (curr == 'r')
            {
                boardCanvas.Children.Add(drawEllipse(i * 65 + 15, j * 50 + 15, Brushes.Yellow));
                currElipse.Fill = Brushes.Red;
            }

            else
            {
                boardCanvas.Children.Add(drawEllipse(i * 65 + 15, j * 50 + 15, Brushes.Red));
                currElipse.Fill = Brushes.Yellow;
            }
        }

        private void configureBoard(string message)
        {
           
            foreach(string str in message.Split(':')[1].Split(';'))
            {
                if(str.Split(',')[0].Count() > 0)
                {
                    int b = Convert.ToInt16(str.Split(',')[0]);
                    int a = Convert.ToInt16(str.Split(',')[1]);
                    char c = Convert.ToChar(str.Split(',')[2]);
                    insertPuck(a, b, c);
                }
            }
        }

        private void drawBoard()
        {
            for (int j = 0; j < 6; j++)
                for (int i = 0; i < 7; i++)
                    boardCanvas.Children.Add(drawEllipse(i * 65 + 15, j * 50 + 15, Brushes.White));

            currElipse = drawEllipse(15, -50, Brushes.Red);
            boardCanvas.Children.Add(currElipse);
        }
        #endregion


        #region client

        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            sw.WriteLine("msg:"+textBox1.Text);
            sw.Flush();
            if (textBox1.Text == "disconnect")
            {
                sw.Close();
                sr.Close();
                ns.Close();
                System.Environment.Exit(System.Environment.ExitCode); //close all
            }
            textBox1.Text = "";
        }
        
        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TcpClient newcon = new TcpClient();
                newcon.Connect("127.0.0.1", 9090);  //IPAddress of Server
                ns = newcon.GetStream();
                sr = new StreamReader(ns);  //Stream Reader and Writer take away some of the overhead of keeping track of Message size.  By Default WriteLine and ReadLine use Line Feed to delimit the messages
                sw = new StreamWriter(ns);
                backgroundWorker1.RunWorkerAsync("Message to Worker");
            }
            catch { MessageBox.Show("Please Try again Later."); }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() => { sw.WriteLine("name:" + nameTextBox.Text); }); ;
            sw.Flush();
            while (true)
            {
                try
                {
                    string inputStream = sr.ReadLine();  //Note Read only reads into a byte array.  Also Note that Read is a "Blocking Function"
                    
                    if (inputStream.Split(':')[0].Equals("update"))
                    {
                        int row = Convert.ToInt16(inputStream.Split(':')[1].Split(',')[0]);
                        int col = Convert.ToInt16(inputStream.Split(':')[1].Split(',')[1]);
                        char currPlayer = Convert.ToChar(inputStream.Split(':')[1].Split(',')[2]);
                        this.Dispatcher.Invoke(() => { insertPuck(col, row, currPlayer); });
                    }

                    else if (inputStream.Split(':')[0].Equals("curse"))
                    {
                        currPosEllipse = Convert.ToInt16(inputStream.Split(':')[1]);
                        this.Dispatcher.Invoke(() => { currElipse.Margin = new Thickness(currPosEllipse * 65 + 15, -50, 0, 0); });//placement
                    }

                    else if (inputStream.Split(':')[0].Equals("msg"))
                    { InsertText(inputStream.Split(':')[1]); }

                    else if (inputStream.Split(':')[0].Equals("board"))
                    { this.Dispatcher.Invoke(() => { configureBoard(inputStream); }); }


                    else if (inputStream.Split(':')[0].Equals("reset"))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            currPosEllipse = 0;
                            boardCanvas.Children.Clear();
                            drawBoard();
                        });
                    }

                    else if (inputStream.Split(':')[0].Equals("players"))
                    {
                        int counter = 0;
                        this.Dispatcher.Invoke(() =>
                        {
                            playersListBox.Items.Clear();
                            foreach (string a in inputStream.Split(':')[1].Split(','))
                            {
                                if (counter == 0)
                                    winnerTextBox.Text = a;
                                if (counter == 1)
                                    oponentTextBox.Text = a;
                                else if (counter > 1)
                                    playersListBox.Items.Add(a);
                                counter++;


                            }
                        });

                    }
                    

                        if (inputStream == "disconnect")
                        {
                            sw.WriteLine("disconnect");
                            sw.Flush();
                            sr.Close();
                            sw.Close();
                            ns.Close();
                            System.Environment.Exit(System.Environment.ExitCode); //close all 

                            break;
                        }
                }
                catch
                {
                    ns.Close();
                    System.Environment.Exit(System.Environment.ExitCode); //close all 
                }

            }

        }

        private void InsertText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.chatListBox.Dispatcher.CheckAccess())
            {
                //this.listBox1.Items.Insert(0, text);
                chatListBox.Items.Add(text);

            }
            else
            {
                chatListBox.Dispatcher.BeginInvoke(new SetTextCallback(InsertText), text);
            }
        }


        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("hi");
            if (e.Key == Key.Right && currPosEllipse < 6)
            {
                currPosEllipse++;
                //Console.WriteLine(currPosEllipse);
                sw.WriteLine("curse:" + currPosEllipse);
                sw.Flush();
                //currElipse.Margin = new Thickness(currPosEllipse * 65 + 15, -50, 0, 0);//placement
            }

            if (e.Key == Key.Left && currPosEllipse > 0)
            {
                currPosEllipse--;
                //Console.WriteLine(currPosEllipse);
                sw.WriteLine("curse:" + currPosEllipse);
                sw.Flush();
                //currElipse.Margin = new Thickness(currPosEllipse * 65 + 15, -50, 0, 0);//placement
            }

            if (e.Key == Key.Down)
            {
                Console.WriteLine(currPosEllipse);
                sw.WriteLine("move:" + currPosEllipse);
                sw.Flush();
            }


        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            sw.WriteLine("reset:");
            sw.Flush();
        }

        private void mainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                sw.WriteLine("disconnect");
                sw.Flush();
                sr.Close();
                sw.Close();
                ns.Close();
                System.Environment.Exit(System.Environment.ExitCode); //close all 
            }
            catch { }
            
        }
    }
}
