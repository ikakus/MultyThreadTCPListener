using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Timers;
using System.Data.SqlClient;
using System.Drawing;


namespace MultyThreadTCPListener
{

    public class Connections
    {
        Form1 form;
        //  public static ArrayList ConnArray = new ArrayList();
        public static List<ClientHandler> ConnArray = new List<ClientHandler>();



        public Connections(Form1 fo)
        {
            this.form = fo;
        }


        public void Remove(ClientHandler client)
        {
            int ind = findInConnections(client.getID().ToString());
            if (ind >= 0)
            {
                
                ConnArray.RemoveAt(ind);
            }

        }


        public bool Add(ClientHandler client)
        {
            if (ConnArray.Count > 0)
            {


               // ConnArray.Sort(new ClientComparer());
                int ind = findInConnections(client.getID().ToString());
                if (0>ind)
                {
                    ConnArray.Add(client);

                    return true;
                }else
                if (0 <= ind && ind<=ConnArray.Count-1)
                {
                    // ConnArray.

                    ConnArray.RemoveAt(ind);
                   
                    ConnArray.Insert(ind, client);
                    return true;
                }
                return false;
            }

            else
            {
                ConnArray.Add(client);
                return true;
            }
        }

        public void updateLabel(int i)
        {
            form.UpdateClientNumber(i);
        }

        public void UpdateList()
        {



            if (ConnArray.Count > 0)
            {

                
                for (int i = 0; i <= ConnArray.Count - 1; i++)
                {
                    if (i < ConnArray.Count)
                    {
                        try
                        {
                            form.UpdatingListview(ConnArray[i].getID().ToString(), ConnArray[i].getIP().ToString());
                            form.AddToComboBox(ConnArray[i].getID().ToString());
                        }
                        catch (Exception ex)
                        {
                            form.UpdatingTextBox(ex.Message, Color.Red);
                        }
                    }

                }
                SynchronousSocketListener.ClientNbr = ConnArray.Count;
            }

        }


        private int findInConnections(string id)
        {
            for (int i = 0; i <= ConnArray.Count - 1; i++)
            {
                if (ConnArray[i]!=null && ConnArray[i].getID() == System.Convert.ToInt32(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public void sendMessage(ComboBox com, TextBox tex)
        {

            if (com.Text != "" && tex.Text != "")
            {
                string ID = com.Text;
                string message = tex.Text;

                int ind;
                ind = findInConnections(ID);

                ConnArray[ind].setSentMessage(message);
                ConnArray[ind].setNeedToSend(true);

            }

        }

        public void sendMessageToAll(TextBox tex)
        {
            if (tex.Text != "")
            {
                
                string message = tex.Text;


                for (int i = 0; i <= ConnArray.Count-1; i++)
                {


                    ConnArray[i].setSentMessage(message);
                    ConnArray[i].setNeedToSend(true);
                }

            }
        }

    }

    #region Client Connection Pool Class
    public class ClientConnectionPool
    {

        // Creates a synchronized wrapper around the Queue.
        private Queue SyncdQ = Queue.Synchronized(new Queue());



        public void Enqueue(ClientHandler client)
        {
            if (!SyncdQ.Contains(client))
            {
                SyncdQ.Enqueue(client);

                //Connections.Add(client);
            }



        }

        public ClientHandler Dequeue()
        {
            return (ClientHandler)(SyncdQ.Dequeue());
        }

        public int Count
        {
            get { return SyncdQ.Count; }
        }

        public object SyncRoot
        {
            get { return SyncdQ.SyncRoot; }
        }



    } // class ClientConnectionPool
    #endregion

    #region CLient Service
    public class ClientService
    {

        Form1 form;



        private static int numOfThread=5;

        public ClientConnectionPool ConnectionPool;
        public static Connections connections;
        private bool ContinueProcess = false;
        private Thread[] ThreadTask = new Thread[numOfThread];
        

        public ClientService(ClientConnectionPool ConnectionPool, Form1 form)
        {
            this.ConnectionPool = ConnectionPool;

            connections = new Connections(form);

            this.form = form;
        }

        public static void setThreadNum(int n)
        {
            numOfThread = n;
        }

        public static int getThreadNum()
        {
            return numOfThread;
        }

        public void Start()
        {
            ContinueProcess = true;
            // Start threads to handle Client Task
            for (int i = 0; i < ThreadTask.Length; i++)
            {
                ThreadTask[i] = new Thread(new ThreadStart(this.Process));
                ThreadTask[i].Start();
            }
        }

        private void Process()
        {
            while (ContinueProcess)
            {

                ClientHandler client = null;
                lock (ConnectionPool.SyncRoot)
                {
                    if (ConnectionPool.Count > 0)
                    {
                        client = ConnectionPool.Dequeue();
                    }
                }

                if (client != null && Form1.closing==false)
                {

                    client.Process(); // Provoke client
                    //  client.setSentMessage("huehuehue");

                    // if client still connect, schedufor later processingle it 
                    if (client.Alive)
                    {
                        ConnectionPool.Enqueue(client);

                    }
                    else
                    {
                        
                        
                       // form.removeFromListView(client.getID().ToString(),client.getIP().ToString());

                        client.Close();
                       
                       

                        ClientService.connections.Remove(client);
                        //int index= Connections.ConnArray.IndexOf(client);
                        //form.removeFromListView(index);
                        ClientService.connections.UpdateList();
                        //SynchronousSocketListener.ClientNbr--;
                        form.removeFromListView();
                        form.removeFromCombobox();
                        ClientService.connections.updateLabel(Connections.ConnArray.Count);

                        form.UpdatingTextBox("Client " + client.getID().ToString() + " disconnected" + " At " + DateTime.Now.ToString() + "\r\n", Color.Blue);
                        form.UpdatingTextBox("Number Of clients left:" + Connections.ConnArray.Count.ToString()  + "\r\n", Color.Brown);
                    }

                }



                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            ContinueProcess = false;
            for (int i = 0; i < ThreadTask.Length; i++)
            {
                if (ThreadTask[i] != null && ThreadTask[i].IsAlive)
                {
                    //ThreadTask[i].Join();
                    ThreadTask[i].Abort();//Join();
                }
            }

            // Close all client connections
            while (ConnectionPool.Count > 0)
            {
                
                ClientHandler client = ConnectionPool.Dequeue();
                
              //  form.removeFromListView(client.getID().ToString(), client.getIP().ToString());
                client.Close();

                

                ClientService.connections.UpdateList();
              //  SynchronousSocketListener.ClientNbr--;
                form.removeFromListView();
                form.removeFromCombobox();
                ClientService.connections.updateLabel(Connections.ConnArray.Count);

                form.UpdatingTextBox(client.getID().ToString() + " Client connection is closed! " + " At " + DateTime.Now.ToString() + "\r\n", Color.Blue);
                form.UpdatingTextBox("Number Of clients left:" + Connections.ConnArray.Count.ToString() + "\r\n", Color.BurlyWood);
            }
        }

    } // class ClientService
    #endregion

    #region Class Sync Socket Listener

    public class SynchronousSocketListener
    {

        Form1 form;
        public SynchronousSocketListener(Form1 form)//, Int32 port, string ip)
        {
            this.form = form;
           // this.setIP(ip);
           // this.setPort(port);
           // this.setIP(ip);
        }

        public bool started=false;
        private  Int32 port;// = 6080;

        
        private IPAddress localAddr;// = IPAddress.Parse("10.11.100.88"); //IPAddress.Any;//= IPAddress.Parse("10.11.100.83"); //83 /90");


        public  IPAddress getIP()
        {
            return localAddr;
        }

        public  Int32 getPort()
        {
            return port;
        }

        public  void setIP(string ip)
        {
            try
            {

                localAddr = IPAddress.Parse(ip);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public  void setPort(Int32 p)
        {
            port = p;
        }


        
        public  TcpListener listener;
       
        public static ClientService ClientTask;
        public static int ClientNbr = 0;
       
        ClientHandler ClientH;

        #region Static Start Listening
        public void StartListening()
        {
            if (Form1.anyIP == true)
            {
                localAddr = IPAddress.Any;
                listener = new TcpListener(localAddr, port);

            }
            else
            {
                listener = new TcpListener(localAddr, port);
            }
           // listener2 = new TcpListener(localAddr2, port);
            //ClientService ClientTask;

            // Client Connections Pool
           
            ClientConnectionPool ConnectionPool = new ClientConnectionPool();

            // Client Task to handle client requests
            ClientTask = new ClientService(ConnectionPool, form);


           
            ClientTask.Start();

            

            try
            {


                listener.Start();
               // listener2.Start();
                form.UpdatingTextBox("Waiting for a connection..." + "\r\n", Color.Black);

                Form1.StartedTime = DateTime.Now;
               
               

                while (true)
                {

                    TcpClient handler = listener.AcceptTcpClient();
                   // TcpClient handler2 = listener2.AcceptTcpClient();
                    // handler.Connected

                    if (handler.Connected)//handler != null)
                    {

                        // Console.WriteLine("Client#{0} accepted!", ++ClientNbr);
                      //  SynchronousSocketListener.ClientNbr++;
                        form.UpdatingTextBox("New client approaching " +  "\r\n", Color.Blue);
                        // An incoming connection needs to be processed.


                        ClientH = new ClientHandler(handler, form);

                        ConnectionPool.Enqueue(ClientH);



                        //--TestingCycle;

                    }
                    else
                        break;


                }
                this.listener.Stop();
              
                // Stop client requests handling
                ClientTask.Stop();


            }
            catch (Exception e)
            {
               

               // form.UpdatingTextBox(e.Message.ToString() + "\r\n", Color.Red);
                if (Form1.closing != true)
                {
                    form.UpdatingTextBox(e.Message.ToString() + "\r\n", Color.Red);
                    form.stopServerDeleg();
                }

            }


        }
        #endregion

        public void StopListening()
        {

            this.listener.Stop();
            ClientTask.Stop();
        }

    }
    #endregion

    struct SyncMess// Synchronization message structure
    {
        ushort SyncHeader;
        ushort SyncID;
        uint UnitID;
    }

    #region Client Handler Calss

    public class ClientHandler
    {
        Form1 form;

        private TcpClient ClientSocket;
        private NetworkStream networkStream;
        bool ContinueProcess = false;
        private byte[] bytes = new byte[512]; 		// Data buffer for incoming data.
        private StringBuilder sb = new StringBuilder(); // Received data string.
        private string data = null; // Incoming data from the client.
        private int ID;
        private IPAddress IP;
        private DateTime ConnectionStartTime = DateTime.Now;// timer start time
        private DateTime MessageStartTime = DateTime.Now;
        private DateTime HilightstartTime = DateTime.Now;
        private string messageToSend = "";// message to be sent
        System.Data.SqlClient.SqlConnection con;// SQL connection to write to base
        private TimeSpan Connectioninterval;// interval for connection  timeout
        private TimeSpan MessageInterval; // interval for sent message timeout
        private TimeSpan HilightInterval; // interval for hiliting listview items
        public bool stop = false;
        private bool needToSend = false;
        private string answer = "";
        private bool busy = false;

        public static TimeSpan connectionTime;
        public static TimeSpan messageTime;


        public void setAnswer(string str)
        {
            this.answer = str;
        }

        public string getAnswer()
        {
            return answer;
        }

        public void setNeedToSend(bool b)
        {
            this.needToSend = b;
        }

        public void setSentMessage(string str)
        {
            messageToSend = str;
        }


        struct ReceivedData
        {
            public string ID;
            public string Time;
            public string Longitude;
            public string Latitude;
            public string Speed;
            public string Direction;
            public string Altitude;
            public string Satelites;

        };

        private int GetNthIndex(string s, char t, int n)//for Convert() Function
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        ReceivedData Convert(string str)
        {
            ReceivedData dat;
            if (str.Length > 55)
            {

                dat.ID = str.Substring(0, GetNthIndex(str, ',', 1));
              // dat.Time = str.Substring(GetNthIndex(str, ',', 1)+1 , 4) + "/" + str.Substring(15, 2) + "/" + str.Substring(17, 2) + " " + str.Substring(19, 2) + ":" + str.Substring(21, 2) + ":" + str.Substring(23, 2);
                dat.Time = str.Substring(GetNthIndex(str, ',', 1) + 1, 14);
                dat.Longitude = str.Substring(GetNthIndex(str, ',', 2) + 1, GetNthIndex(str, ',', 3) - (GetNthIndex(str, ',', 2) + 1));
                dat.Latitude = str.Substring(GetNthIndex(str, ',', 3) + 1, GetNthIndex(str, ',', 4) - (GetNthIndex(str, ',', 3) + 1));
                dat.Speed = str.Substring(GetNthIndex(str, ',', 4) + 1, GetNthIndex(str, ',', 5) - (GetNthIndex(str, ',', 4) + 1));
                dat.Direction = str.Substring(GetNthIndex(str, ',', 5) + 1, GetNthIndex(str, ',', 6) - (GetNthIndex(str, ',', 5) + 1));
                dat.Altitude = str.Substring(GetNthIndex(str, ',', 6) + 1, GetNthIndex(str, ',', 7) - (GetNthIndex(str, ',', 6) + 1));
                dat.Satelites = str.Substring(GetNthIndex(str, ',', 7) + 1, GetNthIndex(str, ',', 8) - (GetNthIndex(str, ',', 7) + 1));

                return dat;
            }
            return new ReceivedData();
            //return 0;
        }


        public ClientHandler(TcpClient cli, int id, IPAddress ip)
        {
            setClient(cli);
            setID(id);
            setIP(ip);
        }

        public void setClient(TcpClient client)
        {
            this.ClientSocket = client;
        }

        public void setID(int id)
        {
            this.ID = id;
        }
        public void setIP(IPAddress ip)
        {
            this.IP = ip;
        }

        public int getID()
        {
            return ID;
        }

        public IPAddress getIP()
        {
            return IP;
        }


        public ClientHandler(TcpClient ClientSocket, Form1 form)
        {
            ClientSocket.ReceiveTimeout = 100; // 100 miliseconds
            this.ClientSocket = ClientSocket;
            networkStream = ClientSocket.GetStream();
            bytes = new byte[ClientSocket.ReceiveBufferSize];
            ContinueProcess = true;
            this.form = form;
            //connections = new Connections(form);
        }

        #region Some interesting shit about AsyncRead/Write

        //public void Process()
        //{
        //    try
        //    {
        //        GetResponse(networkStream, out data);
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        form.UpdatingTextBox(ex.Message.ToString());
        //    }
        //}




        public bool GetResponse(NetworkStream stream, out string response)
        {
            byte[] readBuffer = new byte[32];
            var asyncReader = stream.BeginRead(readBuffer, 0, readBuffer.Length, null, null);
            WaitHandle handle = asyncReader.AsyncWaitHandle;

            // Give the reader 2seconds to respond with a value
            bool completed = handle.WaitOne(10000, false);
            if (completed)
            {
                int bytesRead = stream.EndRead(asyncReader);

                StringBuilder message = new StringBuilder();
                message.Append(Encoding.ASCII.GetString(readBuffer, 0, bytesRead));

                if (bytesRead == readBuffer.Length)
                {
                    // There's possibly more than 32 bytes to read, so get the next 
                    // section of the response
                    string continuedResponse;
                    if (GetResponse(stream, out continuedResponse))
                    {
                        message.Append(continuedResponse);
                    }
                }

                response = message.ToString();
                return true;
            }
            else
            {
                int bytesRead = stream.EndRead(asyncReader);
                if (bytesRead == 0)
                {
                    // 0 bytes were returned, so the read has finished
                    response = string.Empty;
                    return true;
                }
                else
                {
                    throw new TimeoutException(
                        "The device failed to read in an appropriate amount of time.");
                }
            }
        }
        #endregion

        public void sendData(string sendcommand)
        {
            if (this.needToSend==true && this.busy==false && messageToSend!="")
            {
                try
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendcommand);


                    this.networkStream.Write(sendBytes, 0, (int)sendBytes.Length);
                    form.UpdatingTextBox("Sent to " + ID.ToString() + " command: " + sendcommand + " At " + DateTime.Now.ToString() + "\r\n", Color.Purple);


                    this.busy = true;
                   

                }
                catch (Exception ex)
                {
                    //form.stopServerDeleg();
                    ContinueProcess = false;
                    form.UpdatingTextBox(ex.Message.ToString(), Color.Red);
                   // form.stopServerDeleg();
                  
                }

            }

        }


        public void doSending()
        {

            sendData(this.messageToSend);
            

        }

        public void CheckQuerry()// Check if there is message scheduled for this ID
        {
            if (this.busy == false)
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                con.ConnectionString = Form1.SQLConnectionString;
                System.Data.SqlClient.SqlDataReader myReader = null;

                string str;
                string id = getID().ToString();

                string comm = "SELECT * FROM [MessageSchedule] WHERE FKUnit_ID=" + id;
                con.Open();
                System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(comm, con);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    str = myReader["message1"].ToString();
                    if (str.Length > 2)
                    {
                        this.setSentMessage(str);
                        this.needToSend = true;
                        MessageStartTime = DateTime.Now;
                    }
                    else {

                        
                        this.needToSend = false;
                        this.setSentMessage("");
                        MessageStartTime = DateTime.Now;
 
                    }
                }

                con.Close();

            }

        }



        public void Process()
        {

            Connectioninterval = connectionTime;//new TimeSpan(0, 2, 30);
            MessageInterval = messageTime; //new TimeSpan(0, 0, 40);
            HilightInterval = new TimeSpan(0, 0, 1);
            //Checking connection timeout
            if ((DateTime.Now - ConnectionStartTime) > Connectioninterval)
            {
                form.UpdatingTextBox("Client " + this.getID().ToString() + " Timed out " + Connectioninterval.ToString() + "\r\n", Color.Blue);
                stop = true;
            }

            //Checking MESSAGE timeout
            if ((DateTime.Now - MessageStartTime) > MessageInterval && needToSend==true)
            {
                form.UpdatingTextBox("Message to " + this.getID().ToString() + " Timed out " + MessageInterval.ToString() +" Resending"+ "\r\n", Color.Purple);
                this.busy = false;
            }

            if ((DateTime.Now - HilightstartTime) > HilightInterval )
            {
                int index= Connections.ConnArray.IndexOf(this);
                form.setListviewItemColor(index, Color.White);
            }

            CheckQuerry();
            
            doSending();

            if (stop == true)
            {
                networkStream.Close();
                ClientSocket.Close();
                ContinueProcess = false;

            }
            else


                try
                {

                    #region MyCode block
                    int BytesRead = networkStream.Read(bytes, 0, (int)bytes.Length);

                    if (BytesRead == 8 && bytes[0] == 0xFA && bytes[1] == 0xF8)
                    {

                        ConnectionStartTime = DateTime.Now;

                        IPEndPoint ipend = (IPEndPoint)ClientSocket.Client.RemoteEndPoint;

                        byte[] b = new byte[4];
                        b[0] = bytes[4];
                        b[1] = bytes[5];
                        b[2] = bytes[6];
                        b[3] = bytes[7];

                        ID = BitConverter.ToInt32(b, 0);
                        IP = IPAddress.Parse(ipend.Address.ToString());


                        networkStream.Write(bytes, 0, BytesRead);
                        form.UpdatingTextBox("Handshake from " + ID.ToString() + " At " + DateTime.Now.ToString() + "\r\n", Color.DarkOliveGreen);

                        ClientService.connections.Add(this);//new ClientHandler(ClientSocket, ID, IP));
                        ClientService.connections.UpdateList();
                        ClientService.connections.updateLabel(SynchronousSocketListener.ClientNbr);

                        networkStream.Write(bytes, 0, BytesRead);

                    }

                    if (BytesRead > 0)
                    {
                        ConnectionStartTime = DateTime.Now;

                        // There might be more data, so store the data received so far.
                        sb.Append(Encoding.ASCII.GetString(bytes, 0, BytesRead));
                    }
                    else
                        // All the data has arrived; put it in response.
                        ProcessDataReceived();
                    #endregion

                }


                catch (IOException)
                {
                    // All the data has arrived; 
                    ProcessDataReceived();
                }
                catch (SocketException)
                {
                    networkStream.Close();
                    ClientSocket.Close();
                    ContinueProcess = false;
                    form.UpdatingTextBox("Conection is broken! \r\n", Color.Red);
                }




        }  // Process()



        private void ProcessDataReceived()
        {
            if (sb.Length > 0)
            {
                string[] separated;

                Form1.lastReceived = DateTime.Now;


                data = sb.ToString();
               
                int index= Connections.ConnArray.IndexOf(this);



                form.setListviewItemColor(index, Color.Green);
                HilightstartTime = DateTime.Now;

               // MessageBox.Show(index.ToString());
                
                sb.Length = 0; // Clear buffer
                data.IndexOf('\n');
                separated = data.Split('\n');

                for (int i = 0; i <= separated.Length - 2; i++)
                {
                    //Thread.Sleep(1000);
                    if (separated[i].Length > 6)
                    {
                        if (separated[i].Substring(0, 4) == "1010")
                        {

                            if (Form1.filterChecked == false)
                            {
                                form.UpdatingTextBox("received from " + this.getID().ToString() + " At " + DateTime.Now.ToString() + "\r\n", Color.DarkOliveGreen);
                                form.UpdatingTextBox(separated[i], Color.Black);
                                form.UpdatingTextBox("\r\n", Color.Black);
                                
                            }
                           
                            ReceivedData da;
                            

                            con = new System.Data.SqlClient.SqlConnection();
                            con.ConnectionString = Form1.SQLConnectionString; //"Data Source=SERVER\\SQLEXPRESS;Initial Catalog=Point;Persist Security Info=True;User ID=sa;Password=12345";//Form1.SQLConnectionString; //"Data Source=IKA-FDF3AA55734\\SQLEXPRESS;Initial Catalog=Point;Persist Security Info=True;User ID=sa;Password=12345";


                            da = Convert(separated[i]);


                            

                            if (i == 0)
                            {
                                IPEndPoint ipend = (IPEndPoint)ClientSocket.Client.RemoteEndPoint;
                                IP = IPAddress.Parse(ipend.Address.ToString());

                                ID = System.Convert.ToInt32(da.ID); // 11.09.14---------------------------------------------

                                ClientService.connections.Add(this);
                                //connections.UpdateList();
                            }





                            try
                            {
                                if (da.Satelites != "0")
                                {

                                    string comm = "INSERT INTO [cars] (DateUnit,FK_UnitID,PositionDateTime,Longitude,Latitude,Speed,Direction,Altitude,NumberOfSatelite)" +
                                                         "Values(" + (da.ID + da.Time) + "," + da.ID + ",'" + da.Time + "'," + da.Longitude + "," + da.Latitude + "," + da.Speed + "," + da.Direction + "," + da.Altitude + "," + da.Satelites + ")";   //125556,'11/08/26 14:09:08', '12.312312','14.23412','123.6',140,23,5)";/*'11/08/27 12:13:00'*/
                                    con.Open();
                                    SqlCommand myCommand = new SqlCommand(comm, con);
                                    myCommand.ExecuteNonQuery();
                                    con.Close();
                                    if (messageToSend == "$ST+GETPOSITION=0000")
                                    {
                                        Form1.sh.RemoveMesageFromQuerry(this.getID().ToString());
                                        Form1.sh.RenewQuerryList();
                                        // messageToSend = "";
                                        this.busy = false;
                                        this.needToSend = false;
                                    }
                                    // this.busy = false;
                                    // this.needToSend = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                form.UpdatingTextBox(ex.Message.ToString(), Color.Red);

                            }
                        }else

                            if (separated[i].Substring(0, 1) == "$" && messageToSend != "")//data[0] == '$' && messageToSend != "")
                            {
                                string str1 = messageToSend.Substring(4, GetNthIndex(messageToSend, '=', 1) - 4);
                                string formateddata = separated[i].Substring(0, str1.Length + 4);

                                if (formateddata == "$OK:" + str1 || formateddata == "$QR:" + str1)
                                {
                                    string ans;

                                    // data.IndexOf('\n');
                                    separated = data.Split('\n');
                                    ans = separated[i];

                                    setAnswer(ans);

                                    form.UpdatingTextBox("received from " + this.getID().ToString() + " At " + DateTime.Now.ToString() + "\r\n", Color.DarkOliveGreen);
                                    form.UpdatingTextBox(ans, Color.Green);
                                    form.UpdatingTextBox("\r\n", Color.Black);

                                    Form1.sh.RemoveMesageFromQuerry(this.getID().ToString());
                                    Form1.sh.RenewQuerryList();
                                    // Form1.sh.RenewQuerryList();
                                    this.busy = false;
                                    this.needToSend = false;
                                }

                                if (separated[i].Substring(0, 3) == "$ER")
                                {
                                    string str2 = messageToSend.Substring(4, GetNthIndex(messageToSend, '=', 1) - 3);

                                    string formated = separated[i].Substring(0, separated[i].Length - 1);
                                    if (formated == "$ER:" + str2 + '0')//Unknown command , delete from query
                                    {
                                        form.UpdatingTextBox("received from " + this.getID().ToString() + " At " + DateTime.Now.ToString() + "\r\n", Color.DarkRed);
                                        form.UpdatingTextBox("ERROR: Unknown command " + messageToSend + ". Command deleted from query ", Color.Red);
                                        form.UpdatingTextBox("\r\n", Color.Black);

                                        Form1.sh.RemoveMesageFromQuerry(this.getID().ToString());
                                        Form1.sh.RenewQuerryList();

                                        this.busy = false;
                                        this.needToSend = false;
                                    }
                                    else
                                        if (formated == "$ER:" + str2 + '2')//Invalid command parameters
                                        {
                                            form.UpdatingTextBox("received from " + this.getID().ToString() + " At " + DateTime.Now.ToString() + "\r\n", Color.DarkRed);
                                            form.UpdatingTextBox("ERROR: Invalid command parameters " + messageToSend + ". Command  deleted from query ", Color.Red);
                                            form.UpdatingTextBox("\r\n", Color.Black);

                                            Form1.sh.RemoveMesageFromQuerry(this.getID().ToString());
                                            Form1.sh.RenewQuerryList();

                                            this.busy = false;
                                            this.needToSend = false;
                                        }
                                        else
                                            if (formated == "$ER:" + str2 + '1')//Invalid password
                                            {
                                                form.UpdatingTextBox("received from " + this.getID().ToString() + " At " + DateTime.Now.ToString() + "\r\n", Color.DarkRed);
                                                form.UpdatingTextBox("ERROR: Invalid password " + messageToSend + ". Command deleted from query ", Color.Red);
                                                form.UpdatingTextBox("\r\n", Color.Black);

                                                Form1.sh.RemoveMesageFromQuerry(this.getID().ToString());

                                                Form1.sh.RenewQuerryList();
                                                this.busy = false;
                                                this.needToSend = false;
                                            }
                                            else
                                            {
                                                string ans;

                                                // data.IndexOf('\n');
                                                separated = data.Split('\n');
                                                ans = separated[i];

                                                setAnswer(ans);

                                                form.UpdatingTextBox("received from " + this.getID().ToString() + " At " + DateTime.Now.ToString() + "\r\n", Color.DarkRed);
                                                form.UpdatingTextBox(ans, Color.Red);
                                                form.UpdatingTextBox("\r\n", Color.Black);
                                            }


                                }
                            }

                    }

                    if (stop == true)
                    {
                        networkStream.Close();
                        ClientSocket.Close();
                        ContinueProcess = false;
                    }
                }
            }
        }

        public void Close()
        {
            networkStream.Close();
            ClientSocket.Close();
        }

        public bool Alive
        {
            get
            {
                return ContinueProcess;
            }
        }

    } // class ClientHandler 
    #endregion


}
