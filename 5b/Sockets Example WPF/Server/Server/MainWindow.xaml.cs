using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void SetTextCallback(String text);
        delegate void SetIntCallbCk(int theadnum);
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        int[,] board;
        int turn = 1;
        public MainWindow()
        {
            board = new int[6,8]; // 0 for empty 1 for player1 2 for player2
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j] = 0;
            InitializeComponent();
            drawBoard();

        }

        BackgroundWorker[] bkw1 = new BackgroundWorker[100];
        Socket client;
        NetworkStream[] ns = new NetworkStream[100];
        StreamReader[] sr = new StreamReader[100];
        StreamWriter[] sw = new StreamWriter[100];
        List <int> AvailableClientNumbers = new List<int>(100);
        List<int>  UsedClientNumbers = new List<int>(100);

        int clientcount = 0;
        public void drawBoard()
        {
            SolidColorBrush white = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            SolidColorBrush player1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            SolidColorBrush player2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));


            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Ellipse e = new Ellipse();
                    e.Height = 40;
                    e.Width = 40;
                    e.Margin = new Thickness(50*j + 10, 50*i+10,0,0);
                    if (board[i, j] == 0)
                        e.Fill = white;
                    else if (board[i, j] == 1)
                        e.Fill = player1;
                    else
                        e.Fill = player2;
                    cnv1.Children.Add(e);
                }
            }
        }
        public bool win(int x, int y, int turn)
        {
            if (checkLine(x, y, 0, 1, turn)) return true; // check up and down
            if (checkLine(x, y, 1, 0, turn)) return true; // check right and left
            if (checkLine(x, y, 1, -1, turn)) return true; // check  45 degrees
            if (checkLine(x, y, 1, 1, turn)) return true; // check -45 degrees


            return false;
        }
        public bool checkLine(int x,int y, int xChange, int yChange, int turn)
        {
            int count, tempX = x, tempY = y;
            //check up
            count = 1;
            while (true)
            {
                tempX += xChange;
                tempY +=yChange;
                if (inBounds(tempX, tempY) == false) break;
                else if (board[tempX, tempY] == turn) count++;
                else break;
            }
            //check down
            tempY = y; tempX = x;
            while (true)
            {
                tempX -= xChange;
                tempY -= yChange;
                if (inBounds(tempX, tempY) == false) break;
                else if (board[tempX, tempY] == turn) count++;
                else break;
            }

            if (count >= 4)
                return true;
            return false;
        }
        public bool inBounds(int x, int y)
        {
            if (!(x >= 0 && x <= 5))
                return false;
            if (!(y >= 0 && y <= 7))
                return false;
            return true;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            String printtext;
            TcpListener newsocket = new TcpListener(IPAddress.Any, 9090);  //Create TCP Listener on server
            newsocket.Start();
            for(int i=0; i<100;i++)
            {
                AvailableClientNumbers.Add(i);
            }
            while (AvailableClientNumbers.Count > 0)
            {
                InsertText("waiting for client");                   //wait for connection
                printtext = "Available Clients = " + AvailableClientNumbers.Count;
                InsertText(printtext);                   //wait for connection
                client = newsocket.AcceptSocket();     //Accept Connection
                clientcount = AvailableClientNumbers.First();
                AvailableClientNumbers.Remove(clientcount);
                ns[clientcount] = new NetworkStream(client);                            //Create Network stream
                sr[clientcount] = new StreamReader(ns[clientcount]);
                sw[clientcount] = new StreamWriter(ns[clientcount]);
                string welcome = "Welcome";
                InsertText("client connected");
                sw[clientcount].WriteLine(welcome);     //Stream Reader and Writer take away some of the overhead of keeping track of Message size.  By Default WriteLine and ReadLine use Line Feed to delimit the messages
                sw[clientcount].Flush();
                bkw1[clientcount] = new BackgroundWorker();
                bkw1[clientcount].DoWork += new DoWorkEventHandler(client_DoWork);
                bkw1[clientcount].RunWorkerAsync(clientcount);
                UsedClientNumbers.Add(clientcount);
            }
        }

        private void client_DoWork(object sender, DoWorkEventArgs e)
        {
            int clientnum = (int)e.Argument;
            Console.WriteLine(clientnum);
            bkw1[clientnum].WorkerSupportsCancellation = true; ;

            while (true)
            {
                string inputStream;
                try
                {
                    inputStream = sr[clientnum].ReadLine();
                    if (inputStream == "disconnect")
                    {
                        sr[clientnum].Close();
                        sw[clientnum].Close();
                        ns[clientnum].Close();
                        InsertText("Client " + clientnum + " has disconnected");
                        KillMe(clientnum);
                        break;
                    }

                    InsertText(inputStream, clientnum);

                }
                catch
                {
                    sr[clientnum].Close();
                    sw[clientnum].Close();
                    ns[clientnum].Close();
                    InsertText("Client " + clientnum + " has disconnected");
                    KillMe(clientnum);
                    break;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            //int input = Convert.ToInt16(textBox1.Text) - 1;
            //while(inBounds(0,input) == false)
            //{
            //    MessageBox.Show("Out of bounds. Try again.");
            //    input = Convert.ToInt16(textBox1.Text);

            //}
            //int i;
            //for ( i = 5; i >= 0; i--)
            //{
            //    if (board[i, input] == 0)
            //    {
            //        board[i, input] = turn;
            //        break;
            //    }

            //    else continue;
            //}
            //drawBoard();
            //if (win(i, input, turn))
            //    MessageBox.Show("Winner is player: " + turn);
            //turn = turn % 2 + 1;





            foreach (int t in UsedClientNumbers)
            {
                sw[t].WriteLine(textBox1.Text);
                sw[t].Flush();
            }
            textBox1.Text = "";

        }

        private void InsertText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox1.Dispatcher.CheckAccess())
            {
                this.listBox1.Items.Insert(0, text);

            }
            else
            {
                listBox1.Dispatcher.BeginInvoke(new SetTextCallback(InsertText), text);
            }

            // send message to other clients
            
        }
        private void InsertText(string text, int sender)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox1.Dispatcher.CheckAccess())
            {
                this.listBox1.Items.Insert(0, text);

            }
            else
            {
                listBox1.Dispatcher.BeginInvoke(new SetTextCallback(InsertText), text);
            }

            // send message to other clients
              foreach (int t in UsedClientNumbers)
            {   
                sw[t].WriteLine(text);
                sw[t].Flush();
            }
        }

        private void KillMe(int threadnum)
        {
            if (this.listBox1.Dispatcher.CheckAccess())
            {
                UsedClientNumbers.Remove(threadnum);
                AvailableClientNumbers.Add(threadnum);
                bkw1[threadnum].CancelAsync();
                bkw1[threadnum].Dispose();
                bkw1[threadnum] = null;
                GC.Collect();

            }
            else
            {
                listBox1.Dispatcher.BeginInvoke(new SetIntCallbCk(KillMe), threadnum);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync("Message to Worker");

               
            

        }
    }
}
