using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Net;
using System.Reflection;
using System.Management;


namespace MultyThreadTCPListener
{

    public partial class Form1 : Form
    {
        Thread mainThread;
        Thread tickThread;

        public static SynchronousSocketListener SynListener;
        public static bool closing = false;
        public static bool started = false;
        public static string SQLConnectionString; //= "Data Source=IKA-FDF3AA55734\\SQLEXPRESS;Initial Catalog=Point;Persist Security Info=True;User ID=sa;Password=12345";//IKA-FDF3AA55734
        public static Scheduler sh;
        public static ConfigurationsForm conf;
        public static bool autostart = true;
        public static bool anyIP = true;
        public static bool filterChecked=false;
        public static DateTime lastReceived;
        public static DateTime StartedTime;
        private string name;
        public static LogGenerator logGen;
        public string serailKey;

        public Form1()
        {
            InitializeComponent();
            SynListener = new SynchronousSocketListener(this);
            name = this.Text;
        }
        #region Delegates---------------------------------------------------
        delegate void TextBoxDelegate(string message, Color color);
        delegate void ListViewUpdateDelegate(string ID, string IP);
        delegate void ListViewClearDelegate();
        delegate void AddToComboBoxDelegate(string str);
        delegate void ClearComboBoxClearDelegate();
        delegate void UpdateLabelDelegate(int num);
        delegate void UpdateTockLabelDelegate(int num);
        delegate void stopServerDelegate();
        delegate void closeServerDelegate();
        delegate void HideControlDelegate(Control o);
        delegate void ShowContrlolDelegate(Control o);
        delegate void SetControlText(Control o, string str);
        delegate void SetToolStripItemTextDelegate(ToolStripMenuItem it, string str);
        delegate void SetToEpicMode();
        delegate void SetToNormalMode();
        delegate void ClearTextBoxDelegate();
        delegate void SetListviewItemColorDeleate(int ind, Color col);
        delegate void RemoveFromListViewDelegate();
        delegate void RemoveFromComboboxDelegate();

        public void removeFromCombobox()
        {


            if (this.InvokeRequired)
                try
                {

                    this.Invoke(new RemoveFromComboboxDelegate(removeFromCombobox), new object[] { });
                }
                catch (Exception)
                { }
            else
            {
                
                for (int i = 0; i <= comboBox.Items.Count - 1; i++)
                {
                    bool contains = false;
                    for (int j = 0; j <= Connections.ConnArray.Count - 1; j++)
                    {
                        if (this.comboBox.Items[i].ToString() == Connections.ConnArray[j].getID().ToString())
                        {
                            contains = true;
                        }
                    }
                    if (contains == false)
                    {
                        try
                        {

                            this.comboBox.Items.RemoveAt(i);
                        }
                        catch (Exception)
                        { }
                      //  this.comboBox.Items[i].Remove();
                    }
                }
            }
 
        }

        public void removeFromListView()
        {
            if (this.InvokeRequired)
                try
                {

                    this.Invoke(new RemoveFromListViewDelegate(removeFromListView), new object[] { });
                }
                catch (Exception)
                { }
            else
            {
                
                for (int i = 0; i <= listView1.Items.Count - 1; i++)
                {
                    bool contains = false;
                    for (int j = 0; j <= Connections.ConnArray.Count - 1; j++)
                    {
                        if (this.listView1.Items[i].SubItems[1].Text == Connections.ConnArray[j].getID().ToString())
                        {
                            contains = true;
                        }
                    }
                    if (contains == false)
                    {
                        this.listView1.Items[i].Remove();
                       
                    }
                }
            }
        }

        public void setListviewItemColor(int ind, Color col)
        {
            if (this.InvokeRequired && Form1.closing == false && this.listView1.Disposing==false)
                try
                {
                    this.Invoke(new SetListviewItemColorDeleate(setListviewItemColor), new object[] { ind, col });
                }
                catch (Exception)
                {
 
                }
            else
                if (ind >= 0 )
                {
                    try
                    {
                        this.listView1.Items[ind].BackColor = col;
                    }
                    catch (Exception ex)
                    {
 
                    }
                }
 
        }


        private void clearTextBox()
        {
            if (this.InvokeRequired)
                this.Invoke(new ClearTextBoxDelegate(clearTextBox), new object[] { });
            else
                this.richTextBox1.Clear();

        }

        private void setToEpicMode()
        {
            if (this.InvokeRequired)
                this.Invoke(new SetToEpicMode(setToEpicMode), new object[] { });
            else
            {

                this.button1.Text = "Commence";
                this.button2.Text = "Halt";
                this.button_Send.Text = "Transfer";
                this.Text = "GSWB - Galaxy-Spanning Warp Beacon";
                startToolStripMenuItem.Text = "Commence";
                stopToolStripMenuItem.Text = "Halt";
                configureToolStripMenuItem.Text = "Adjudicate";
                SchedulerToolStripMenuItem.Text = "Agenda";
                fileToolStripMenuItem.Text = "Hololith";
               // this.BackgroundImage =Image.FromFile("C://Documents and Settings//All Users//Documents//My Pictures//Sample Pictures//Sunset.jpg");
            }
                       
        }

        private void setToNormalMode()
        {
            if (this.InvokeRequired)
                this.Invoke(new SetToNormalMode(setToNormalMode), new object[] { });
            else
            {

                

                this.button1.Text = "Start";
                this.button2.Text = "Stop";
                this.button_Send.Text = "Send";
                this.Text = name;
                startToolStripMenuItem.Text = "Start";
                stopToolStripMenuItem.Text = "Stop";
                configureToolStripMenuItem.Text = "Configure";
                SchedulerToolStripMenuItem.Text = "Scheduler";
                fileToolStripMenuItem.Text = "Server";
                
            }

        }

        private void setToolstripItemName(ToolStripMenuItem it, string str)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetToolStripItemTextDelegate(setToolstripItemName), new object[] { it, str });
            else
                it.Text = str;
        }
   

        public void setControlName(Control o, string str)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetControlText(setControlName), new object[] { o, str });
            else
                o.Text = str;
        }


        public void showControl(Control o)
        {
            if (this.InvokeRequired)
                this.Invoke(new ShowContrlolDelegate(showControl), new object[] { o });
            else
                o.Show();
        }

        public void hideControl(Control o)
        {

            if (this.InvokeRequired)
                this.Invoke(new HideControlDelegate(hideControl), new object[] { o });
            else
                o.Hide();
 
 
        }

        private void closeServer()
        {
            if (this.InvokeRequired)
                try
                {
                    this.Invoke(new closeServerDelegate(closeServer), new object[] { });
                }
                catch (Exception)
                { }
            else
                this.Close();
 
        }

        public void stopServerDeleg()
        {

            if (this.InvokeRequired)
                try
                {
                    this.Invoke(new stopServerDelegate(stopServerDeleg), new object[] { });
                }
                catch (Exception)
                { }
            else
                this.stopServer();
        }

        public void UpdateTickNumber(int num)
        {

            if (this.label1.InvokeRequired)
                try
                {
                    this.label1.Invoke(new UpdateTockLabelDelegate(UpdateTickNumber), new object[] { num });
                }
                catch (Exception)
                { }
            else
                this.toolStripStatusLabel2.Text = num.ToString();//.AppendText(msg);

        }

        public void UpdateClientNumber(int num)
        {

            if (this.label1.InvokeRequired)
                try
                {
                    this.label1.Invoke(new UpdateLabelDelegate(UpdateClientNumber), new object[] { num });
                }
                catch (Exception)
                { }
            else
                this.label1.Text = num.ToString();//.AppendText(msg);

        }

        public void AddToComboBox(string str)
        {


            if (this.comboBox.InvokeRequired)
                try
                {
                    this.comboBox.Invoke(new AddToComboBoxDelegate(AddToComboBox), new object[] { str });
                }
                catch (Exception)
                { }
            else
               // this.comboBox.Items.Add(str);//.AppendText(msg);
            {
                bool exists = false;
                for (int i = 0; i <= comboBox.Items.Count - 1; i++)
                {
                    if (this.comboBox.Items[i].ToString() == str)
                    {

                        exists = true;
                        this.comboBox.Items[i] = str;
                       // this.listView1.Sort();
                    }

                }
                if (exists == false)
                {
                    this.comboBox.Items.Add(str);
                    //this.listView1.Sort();

                }
            }//Items.Add(itm);
        }

        public void UpdatingTextBox(string msg, Color col)
        {



            if (this.richTextBox1.InvokeRequired && Form1.closing == false)
            {
                try
                {
                    this.richTextBox1.Invoke(new TextBoxDelegate(UpdatingTextBox), new object[] { msg, col });
                    
                }
                catch (Exception)
                {
                   // MessageBox.Show(ex.Message);
                }
            }
            else
            if(Form1.closing==false)
            {
                {
                    try
                    {

                        this.richTextBox1.SelectionStart = this.richTextBox1.TextLength;
                        this.richTextBox1.SelectionLength = 0;

                        this.richTextBox1.SelectionColor = col;
                        this.richTextBox1.AppendText(msg);
                       
                    }
                    catch(Exception ex)
                    {
                       // MessageBox.Show(ex.Message);
                    }

                }
            }
            
        }

        public void UpdatingListview(string ID, string IP)
        {
            string[] str = new string[2];
            str[1] = ID;
            str[0] = IP;
            ListViewItem itm = new ListViewItem(str);

            if (this.listView1.InvokeRequired)
                try
                {
                    this.listView1.Invoke(new ListViewUpdateDelegate(UpdatingListview), new object[] { ID, IP });
                }
                catch (Exception)
                { }
            else
            {
                bool exists = false;
                for (int i = 0; i <= listView1.Items.Count - 1; i++)
                {
                    if (this.listView1.Items[i].SubItems[1].Text == ID)
                    {

                        exists = true;
                        this.listView1.Items[i].SubItems[1].Text = ID;
                       

                    }
                    

                }
                if (exists == false)
                {
                    this.listView1.Items.Add(itm);
                   
                  
                }
            }

        }

        public void ClearComboBox()
        {
            if (this.comboBox.InvokeRequired)
                try
                {
                    this.comboBox.Invoke(new ClearComboBoxClearDelegate(ClearComboBox), new object[] { });
                }
                catch (Exception)
                { }
            else
                this.comboBox.Items.Clear();
        }

        public void ClearList()
        {
            if (this.listView1.InvokeRequired)
                try
                {
                    this.listView1.Invoke(new ListViewClearDelegate(ClearList), new object[] { });
                }
                catch(Exception)
                { }
            else
                this.listView1.Items.Clear();
        }

        #endregion
        public void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
              //  MessageBox.Show("Lol");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            // this.Opacity = 0;
            // this.ShowInTaskbar = false;

            
            sh = new Scheduler();
            conf = new ConfigurationsForm();
            button2.Enabled = false;
            stopToolStripMenuItem.Enabled = false;
           // this.richTextBox1.BackColor = Color.;
            conf.loadSettings();
            logGen = new LogGenerator();
            //button3.

            try
            {
                sh.fillScheduleGrid();
                //conf.loadSettings();
                if (autostart == true)
                {
                    startServer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.button1.Enabled = false;
                this.startToolStripMenuItem.Enabled = false;
                this.SchedulerToolStripMenuItem.Enabled = false;
               
            }


        }
        public void Tick()
        {
            DateTime startTime1 = DateTime.Now;
            DateTime startTime2 = DateTime.Now;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = Form1.SQLConnectionString;
            logGen = new LogGenerator();
            TimeSpan interval1 = new TimeSpan(0, 0, 5);// 5 seconds intrrval for Tick Write to base
            TimeSpan interval2 = new TimeSpan(1, 0, 0);// for TextBox Clear

            short count = 0;

            while (true)
            {
                Thread.Sleep(1000);



                if ((DateTime.Now - startTime2) > interval2)
                {         
                    clearTextBox();
                    startTime2 = DateTime.Now;
                }

                if ((DateTime.Now - startTime1) > interval1)
                {
                    startTime1 = DateTime.Now;
                    if (count == 9)
                    {
                        count = 0;
                    }
                    count++;



                    logGen.writeLastReceivedLogFile(DateTime.Now);

                    try
                    {
                        #region Some SQL checking shit
                        string comm = "UPDATE [Prov] SET pr= " + count.ToString() + ""; // "INSERT INTO [Prov] (pr)" + "Values(" + count.ToString() + ")";   
                        con.Open();
                        System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(comm, con);
                        myCommand.ExecuteNonQuery();
                        con.Close();
                        UpdateTickNumber(count);

                        System.Data.SqlClient.SqlDataReader myReader = null;
                        string comm2 = "SELECT * FROM [Prov]";
                        con.Open();
                        System.Data.SqlClient.SqlCommand myCommand2 = new System.Data.SqlClient.SqlCommand(comm2, con);
                        myReader = myCommand2.ExecuteReader();

                        string str;

                        while (myReader.Read())
                        {
                            str = myReader["onoff"].ToString();
                            if (str == "1")
                            {
                                closeServer();
                            }

                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        con.Close();
                    }
                    
                    #endregion

                    //if (checkKey() == true)
                    //{
                    //    setToEpicMode();

                    //}
                    //else
                    //{
                    //    setToNormalMode();
                    //}

                }
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            

            Form1.closing = true;

            if (started == true)
            {
               // e.Cancel = true;
                e.Cancel = true;
                if (stopServer() == true)
                {   
                    //e.Cancel=false;
                    this.richTextBox1.Clear();
                    Thread.Sleep(100);
                    this.Close();


                }


                
            }
            
           
        }
        public void startServer()
        {
            if (started == false)
            {

                conf.loadSettings();
                mainThread = new Thread(new ThreadStart(SynListener.StartListening));
                tickThread = new Thread(new ThreadStart(this.Tick));

                mainThread.Start();
                tickThread.Start();

                button2.Enabled = true;
                button1.Enabled = false;

                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;

                started = true;

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            startServer();
        }
        public bool stopServer()
        {
            if (started == true)
            {
                try
                {

                    SynListener.StopListening();
                    button2.Enabled = false;
                    button1.Enabled = true;

                    startToolStripMenuItem.Enabled = true;
                    stopToolStripMenuItem.Enabled = false;
                    listView1.Items.Clear();
                    comboBox.Items.Clear();


                    ClientService.connections.updateLabel(0);
                    tickThread.Abort();
                    mainThread.Abort();

                    // mainThread.ThreadState ==
                    started = false;
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
               
             

            }
            return false;


        }
        private void button2_Click(object sender, EventArgs e)
        {
            stopServer();
        }
        private void button_Send_Click(object sender, EventArgs e)
        {
            if (this.comboBox.Text != "" && this.textBox_CommLine.Text != "")
            {

                sh.addMessageToSend(this.comboBox.Text, this.textBox_CommLine.Text);
                this.textBox_CommLine.Text = "";

            }
        }
        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //conf = new ConfigurationsForm();
            conf.loadSettings();
            conf.fillConfForm();
            conf.Show();

        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startServer();
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopServer();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sh = new Scheduler();
            Form1.sh.Show();
          //  Form1.sh.RenewQuerryList();
           // Form1.sh.BringToFront();

        }
        private bool checkKey()
        {
            serailKey = "5C82554A";
            try
            {

                using (ManagementObjectSearcher DiskSearch =
                          new ManagementObjectSearcher(new SelectQuery("Select * from Win32_LogicalDisk")))
                {

                    using (ManagementObjectCollection moDiskCollection = DiskSearch.Get())
                    {

                        foreach (ManagementObject mo in moDiskCollection)
                        {
                            // LogicalDriveInfo dskinfo = new LogicalDriveInfo(mo, false);
                            if (mo["Description"].ToString() == "Removable Disk")
                            {
                                string inf = mo["Volumeserialnumber"].ToString();
                                if (inf == serailKey)
                                {   
                                    return true;
                                }

                                else
                                {
                                     return false;
                                }
                            }

                            mo.Dispose();
                        }
                    }
                }
            }
            catch
            {
            }
            return false;






        }
        private void shoUsbDriveSerial()
        {
            try
            {

                using (ManagementObjectSearcher DiskSearch =
                          new ManagementObjectSearcher(new SelectQuery("Select * from Win32_LogicalDisk")))
                {

                    using (ManagementObjectCollection moDiskCollection = DiskSearch.Get())
                    {

                        foreach (ManagementObject mo in moDiskCollection)
                        {
                            // LogicalDriveInfo dskinfo = new LogicalDriveInfo(mo, false);
                            if (mo["Description"].ToString() == "Removable Disk")
                            {
                                string str = mo["Volumeserialnumber"].ToString();
                                MessageBox.Show(str);
                            }
                            mo.Dispose();
                        }
                    }
                }
            }
            catch
            {


            }
 
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            if (checkKey() == true)
            {
                MessageBox.Show("Greetings Lord!");
            }
            else
            {
                MessageBox.Show("You even think about trying to trick us, and we'll short-circuit your nervous system, primitive!");
                                   
            }

          
        }
        private void checkBox_filter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_filter.Checked == true)
            {
               // this.checkBox_filter.Checked = false;
                Form1.filterChecked = true;
                //MessageBox.Show("asd");
            }
            else 
            {
                //this.checkBox_filter.Checked = true;
                Form1.filterChecked = false;
               // MessageBox.Show("asd2");
            }
        }
    }
 
}
