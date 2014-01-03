using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MultyThreadTCPListener
{
    public partial class ConfigurationsForm : Form
    {

        string fileLoc = @"config.ini";
        public ConfigurationsForm()
        {
            InitializeComponent();
        }

        private bool conStringChanged = false;

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

        public void createDefaultConfFile()
        {

            FileStream fs = null;
            fs = File.Create(fileLoc);
            fs.Close();
            using (StreamWriter sw = new StreamWriter(fileLoc))
            {
                sw.WriteLine("ServerIP=10.11.100.88");
                sw.WriteLine("serverPort=6080");
                sw.WriteLine("Threads=5");
                sw.WriteLine("conString=Data Source="+System.Environment.MachineName+"\\SQLEXPRESS;Initial Catalog=Point;"
                + "Persist Security Info=True;User ID=sa;Password=12345");// IKA-FDF3AA55734
                sw.WriteLine("autostart=0");
                sw.WriteLine("useAnyIP=0");
                sw.WriteLine("ConnectionTimeout=00:02:30");
                sw.WriteLine("MessageTimeout=00:00:30");
                sw.Close();
            }
        }

        public void loadSettings()
        {
            
            if (File.Exists(fileLoc))
            {
                using (TextReader tr = new StreamReader(fileLoc))
                {
                    string str;
                    while (true)
                    {
                        str = tr.ReadLine();
                        if (str == null)
                        {
                            break;
                        }
                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "ServerIP")
                        {
                            int symbpos = GetNthIndex(str,'=',1);
                            Form1.SynListener.setIP(str.Substring(symbpos+1,str.Length-symbpos-1));
                        }
                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "serverPort")
                        {
                            int symbpos = GetNthIndex(str, '=', 1);
                            Form1.SynListener.setPort(System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1)));
                        }
                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "conString")
                        {
                            int symbpos = GetNthIndex(str, '=', 1);
                            Form1.SQLConnectionString = str.Substring(symbpos + 1, str.Length - symbpos - 1);
                        }

                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "Threads")
                        {
                            int symbpos = GetNthIndex(str, '=', 1);
                            ClientService.setThreadNum(System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1)));
                        }
                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "autostart")
                        {
                            int symbpos = GetNthIndex(str, '=', 1);
                            int b = System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1));
                            if(b==0)
                            {
                            Form1.autostart= false;
                            }if(b==1)
                            {
                                Form1.autostart=true;
                            }
                           // ClientService.setThreadNum(System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1)));
                        }

                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "useAnyIP")
                        {
                            int symbpos = GetNthIndex(str, '=', 1);
                            int b = System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1));
                            if (b == 0)
                            {
                                Form1.anyIP = false;
                                textBox_IP.Enabled = true;
                                checkBox_AnyIP.Checked = false;
                            } if (b == 1)
                            {
                                Form1.anyIP = true;
                                textBox_IP.Enabled = false;
                                checkBox_AnyIP.Checked = true;
                            }
                            // ClientService.setThreadNum(System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1)));
                        }

                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "ConnectionTimeout")
                        {
                            int symbpos = GetNthIndex(str, '=', 1);
                            string hours = str.Substring(symbpos + 1, 2);
                            string minutes = str.Substring(GetNthIndex(str, ':', 1) + 1, 2);
                            string seconds = str.Substring(GetNthIndex(str, ':', 2) + 1, 2);


                            ClientHandler.connectionTime = new TimeSpan(System.Convert.ToInt32(hours), System.Convert.ToInt32(minutes), System.Convert.ToInt32(seconds));
                            // ClientService.setThreadNum(System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1)));(System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1)));
                        }

                        if (str.Substring(0, GetNthIndex(str, '=', 1)) == "MessageTimeout")
                        {
                            int symbpos = GetNthIndex(str, '=', 1);
                            string hours =  str.Substring(symbpos+1,2);
                            string minutes = str.Substring(GetNthIndex(str, ':', 1)+1,2);
                            string seconds = str.Substring(GetNthIndex(str, ':', 2)+1,2);

                          
                            ClientHandler.messageTime = new TimeSpan(System.Convert.ToInt32(hours), System.Convert.ToInt32(minutes), System.Convert.ToInt32(seconds));
                           // ClientService.setThreadNum(System.Convert.ToInt32(str.Substring(symbpos + 1, str.Length - symbpos - 1)));
                        }
                        
                       // Form1.SynListener.setIP(textBox_IP.Text);
                       // Form1.SynListener.setPort(System.Convert.ToInt32(textBox_Port.Text));
                       // Form1.SQLConnectionString = textBox_connectionString.Text;
                       // MessageBox.Show(str);
                    }
                } 
            }
            else
            {
                createDefaultConfFile();   
            }

          
        }

        public void saveConfig()
        {
            if (File.Exists(fileLoc))
            {
                using (StreamWriter sw = new StreamWriter(fileLoc))
                {
                    sw.WriteLine("ServerIP="+textBox_IP.Text);
                    sw.WriteLine("serverPort="+textBox_Port.Text);
                    sw.WriteLine("Threads="+textBox_threads.Text);
                    sw.WriteLine("conString=" + textBox_connectionString.Text);//Data Source=IKA-FDF3AA55734\\SQLEXPRESS;Initial Catalog=Point;Persist Security Info=True;User ID=sa;Password=12345");
                    
                    string auto;
                    if(check_auto.Checked==true)
                        auto="1";
                    else auto="0";
                    sw.WriteLine("autostart=" + auto);

                    string anyIP;
                    if (checkBox_AnyIP.Checked == true) 
                        anyIP = "1";
                    else anyIP = "0";
                    sw.WriteLine("useAnyIP=" + anyIP);

                    sw.WriteLine("ConnectionTimeout=" + maskedTextBox_ConTime.Text);//00:02:30");
                    sw.WriteLine("MessageTimeout=" + maskedTextBox_MesTime.Text); //00:00:30");

                    sw.Close();
                }
            }
        }

        public void fillConfForm()
        {
            textBox_IP.Text = Form1.SynListener.getIP().ToString();
            textBox_Port.Text = Form1.SynListener.getPort().ToString();
            textBox_threads.Text = ClientService.getThreadNum().ToString();
            textBox_connectionString.Text = Form1.SQLConnectionString.ToString();
            check_auto.Checked = Form1.autostart;
            maskedTextBox_ConTime.Text = ClientHandler.connectionTime.ToString();
            maskedTextBox_MesTime.Text = ClientHandler.messageTime.ToString();
        }

        private void ConfigurationsForm_Load(object sender, EventArgs e) //load form
        {
            loadSettings();
            fillConfForm();
            conStringChanged = false;
        }

        private void button2_Click(object sender, EventArgs e) //save
        {

            saveConfig();
            if (conStringChanged == true)
            {
                MessageBox.Show("Connection string changed. Restart program to changes take effect");
                conStringChanged = false;
            }
            loadSettings();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) //cancel
        {
            this.Hide();
        }

        private void ConfigurationsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void checkBox_AnyIP_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox_AnyIP.Checked == true)
            {
                textBox_IP.Enabled = false;
            }
            else {
                textBox_IP.Enabled = true;
            }
        }

        private void textBox_connectionString_TextChanged(object sender, EventArgs e)
        {
            conStringChanged = true;
            
        }
    }
}
