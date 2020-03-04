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
        Ellipse currElipse;
        gameLogic logic = new gameLogic();
        int currPosEllipse = 0;
        List<int> playerQueue = new List<int>();
        int winnerBoxClientNum = 0;
        int oponentBoxClientNum = 0;
        public MainWindow()
        {
            InitializeComponent();
            drawBoard();
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

        private void insertPuck(int i, int j)
        {
            if (logic.currPlayer == 'r')
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

        private void drawBoard()
        {
            boardCanvas.Children.Clear();
            for (int j = 0; j < 6; j++)
                for (int i = 0; i < 7; i++)
                    boardCanvas.Children.Add(drawEllipse(i * 65 + 15, j * 50 + 15, Brushes.White));

            currElipse = drawEllipse(15, -50, Brushes.Red);
            boardCanvas.Children.Add(currElipse);
        }
        #endregion

        #region server
        BackgroundWorker[] bkw1 = new BackgroundWorker[5];
        Socket client;
        NetworkStream[] ns = new NetworkStream[5];
        StreamReader[] sr = new StreamReader[5];
        StreamWriter[] sw = new StreamWriter[5];
        List<int> AvailableClientNumbers = new List<int>(5);
        List<int> UsedClientNumbers = new List<int>(5);
        Dictionary<int, string> clientNames = new Dictionary<int, string>();
        bool winFlag = false;

        int clientcount = 0;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            String printtext;
            TcpListener newsocket = new TcpListener(IPAddress.Any, 9090);  //Create TCP Listener on server
            newsocket.Start();
            for (int i = 0; i < 5; i++)
            {
                AvailableClientNumbers.Add(i);
            }
            while (AvailableClientNumbers.Count > 0)
            {
                InsertText("waiting for client");                   //wait for connection
                printtext = "Available Clients = " + AvailableClientNumbers.Count;
                InsertText(printtext);                   //wait for connection
                client = newsocket.AcceptSocket();     //Accept Connection
                Console.WriteLine("block");
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
            bkw1[clientnum].WorkerSupportsCancellation = true; ;

            while (true)
            {
                string inputStream;
                int row = 0;
                int col = 0;
                try
                {
                    inputStream = sr[clientnum].ReadLine();
                    Console.WriteLine(inputStream);

                    if (inputStream.Split(':')[0].Equals("name"))
                        nameInput(clientnum, inputStream);


                    else if (inputStream.Split(':')[0].Equals("update"))
                    {
                        row = Convert.ToInt16(inputStream.Split(':')[1].Split(',')[0]);
                        col = Convert.ToInt16(inputStream.Split(':')[1].Split(',')[1]);
                        this.Dispatcher.Invoke(() => { insertPuck(col, row); });
                    }

                    else if (inputStream.Split(':')[0].Equals("move") && !winFlag)
                        moveInput(clientnum, inputStream);


                    else if (inputStream.Split(':')[0].Equals("reset"))
                    {
                        winFlag = false;
                        Console.WriteLine("resetting");
                        //nameInput(clientnum, inputStream);
                        this.Dispatcher.Invoke(() =>
                        {
                            
                            logic = new gameLogic();
                            boardCanvas.Children.Clear();
                            drawBoard();

                        });
                        foreach (int t in UsedClientNumbers)
                        {
                            currPosEllipse = 0;
                            sw[t].WriteLine("reset:");
                            sw[t].Flush();
                        }
                        
                    }



                    else if (inputStream.Split(':')[0].Equals("curse") && !winFlag)
                        curseInput(clientnum, inputStream);


                    else if (inputStream.Split(':')[0].Equals("msg"))
                    {
                        messageClientsExcept(clientnum, "msg: " + clientNames[clientnum] + " -> "+ inputStream.Split(':')[1]);
                        InsertText(clientNames[clientnum]+" -> "+inputStream.Split(':')[1]);
                    }


                    if (inputStream == "disconnect")
                    {
                        sr[clientnum].Close();
                        sw[clientnum].Close();
                        ns[clientnum].Close();
                        InsertText("Client " + clientNames[clientnum] + " has disconnected");
                        KillMe(clientnum);
                        break;
                    }
                }

                catch
                {
                    sr[clientnum].Close();
                    sw[clientnum].Close();
                    ns[clientnum].Close();
                    InsertText("Client " + clientnum + " has disconnected");
                    KillMe(clientnum);
                }
            }
        }

        private void loosingPlayer(int player)
        {
            playerQueue.Add(playerQueue.ElementAt(player));
            playerQueue.RemoveAt(player);

            int counter = 0;
            this.Dispatcher.Invoke(() =>
            {
                clientListBox.Items.Clear();
                foreach (int a in playerQueue)
                {
                    Console.WriteLine(a);
                    if (counter == 0)
                        winnerTextBox.Text = clientNames[a];
                    if (counter == 1)
                        oponentTextBox.Text = clientNames[a];
                    else if (counter > 1)
                        clientListBox.Items.Add(clientNames[a]);

                    counter++;
                }
            });

            string mess = "players:";
            foreach (int t in playerQueue)
            {
                mess += clientNames[t] + ",";
            }
            Console.WriteLine(mess);
            foreach (int t in UsedClientNumbers)
            {
                sw[t].WriteLine(mess);
                sw[t].Flush();
            }
        }


        private void nameInput(int clientnum, string inputStream)
        {
            clientNames.Add(clientnum, inputStream.Split(':')[1]);
            playerQueue.Add(clientnum);

            int counter = 0;
            this.Dispatcher.Invoke(() =>
            {
                clientListBox.Items.Clear();
                foreach (int a in playerQueue)
                {
                    Console.WriteLine(a);
                    if (counter == 0)
                        winnerTextBox.Text = clientNames[a];
                    if (counter == 1)
                        oponentTextBox.Text = clientNames[a];
                    else if (counter > 1)
                        clientListBox.Items.Add(clientNames[a]);

                    counter++;
                }
            });

            string mess = "players:";
            foreach (int t in UsedClientNumbers)
            {
                mess += clientNames[t] + ",";
            }
            Console.WriteLine(mess);
            foreach (int t in UsedClientNumbers)
            {
                sw[t].WriteLine(mess);
                sw[t].Flush();
            }

            sw[clientnum].WriteLine(logic.boardSpots());
            sw[clientnum].Flush();
        }

        private void curseInput(int clientnum, string inputStream)
        {
            if (logic.currPlayer == 'r' && clientnum == playerQueue[0] || logic.currPlayer == 'y' && clientnum == playerQueue[1])
            {
                currPosEllipse = Convert.ToInt16(inputStream.Split(':')[1]);
                Console.WriteLine(currPosEllipse);
                this.Dispatcher.Invoke(() =>
                {
                    currElipse.Margin = new Thickness(currPosEllipse * 65 + 15, -50, 0, 0);
                    foreach (int t in UsedClientNumbers)
                    {
                        sw[t].WriteLine("curse:" + currPosEllipse);
                        sw[t].Flush();
                    }
                });
            }
        }

        private void moveInput(int clientnum, string inputStream)
        {
            if (logic.currPlayer == 'r' && clientnum == playerQueue[0] || logic.currPlayer == 'y' && clientnum == playerQueue[1])
            {
                int col = Convert.ToInt16(inputStream.Split(':')[1]);
                Tuple<int, int> temp;
                this.Dispatcher.Invoke(() =>
                {
                    temp = logic.move(col);
                    if (temp.Item1 != -1)
                    {
                        insertPuck(temp.Item1, temp.Item2);
                        foreach (int t in UsedClientNumbers)
                        {
                            sw[t].WriteLine("update:" + temp.Item2 + "," + temp.Item1 + "," + logic.currPlayer);
                            sw[t].Flush();
                        }
                    }

                    if (logic.winner(temp.Item1, temp.Item2, 'r') || logic.winner(temp.Item1, temp.Item2, 'y'))
                    {
                        winFlag = true;
                        InsertText("winner is " + clientNames[clientnum]);
                        Console.WriteLine("winner");

                        foreach (int t in UsedClientNumbers)
                        {
                            sw[t].WriteLine("msg:" + "winner is " + clientNames[clientnum]);
                            sw[t].Flush();
                        }

                        if (playerQueue[0] == clientnum)
                        {
                            loosingPlayer(1);
                        }
                        else
                        {
                            loosingPlayer(0);
                        }
                    }
                });
            }
        }

        private void messageClientsExcept(int except, string message)
        {
            foreach (int t in UsedClientNumbers)
            {
                if (t != except)
                {
                    sw[t].WriteLine(message);
                    sw[t].Flush();
                }
            }
        }

        private void sendButton_ClickEvent(object sender, RoutedEventArgs e)
        {
            foreach (int t in UsedClientNumbers)
            {
                sw[t].WriteLine("msg:"+"server -> "+textBox1.Text);
                sw[t].Flush();
            }
            textBox1.Text = "";
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

        private void KillMe(int threadnum)
        {
            if (this.chatListBox.Dispatcher.CheckAccess())
            {

                clientNames.Remove(threadnum);
                UsedClientNumbers.Remove(threadnum);
                AvailableClientNumbers.Add(threadnum);
                bkw1[threadnum].CancelAsync();
                bkw1[threadnum].Dispose();
                bkw1[threadnum] = null;
                GC.Collect();

                clientListBox.Items.Clear();
                int counter = 0;
                playerQueue.Remove(threadnum);
                oponentTextBox.Text = "";
                winnerTextBox.Text = "";

                foreach (int a in playerQueue)
                {
                    Console.WriteLine(a);
                    
                        if (counter == 0)
                            winnerTextBox.Text = clientNames[a];
                        if (counter == 1 && clientNames.Count() > 1)
                            oponentTextBox.Text = clientNames[a];
                        else if (counter > 1 )
                            clientListBox.Items.Add(clientNames[a]);
                    counter++;
                }

                string mess = "players:";
                foreach (int t in UsedClientNumbers)
                {
                    mess += clientNames[t] + ",";
                }
                Console.WriteLine(mess);
                foreach (int t in UsedClientNumbers)
                {
                    sw[t].WriteLine(mess);
                    sw[t].Flush();
                }
            }
            else
            {
                chatListBox.Dispatcher.BeginInvoke(new SetIntCallbCk(KillMe), threadnum);
            }

        }


        private void ServerButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

            backgroundWorker1.RunWorkerAsync("Message to Worker");
        }

        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            List<int> copy = new List<int>(UsedClientNumbers);
            foreach (int t in copy)
            {
                sw[t].WriteLine("disconnect");
                sw[t].Flush();
            }

        }
    }
}
