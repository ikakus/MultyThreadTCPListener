using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultyThreadTCPListener
{
    public partial class AddNewID : Form
    {
        public AddNewID()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                bool exists = false;
                System.Data.SqlClient.SqlDataReader myReader = null;
                string str1 = this.textBox1.Text;
                string comm1 = "SELECT * FROM [MessageSchedule]";
                System.Data.SqlClient.SqlCommand myCommand1 = new System.Data.SqlClient.SqlCommand(comm1, Form1.sh.con);
                myReader = myCommand1.ExecuteReader();
                while (myReader.Read())
                {
                    if (str1 == myReader["FKUnit_ID"].ToString())
                    {
                        MessageBox.Show("This ID Already exists!");
                        exists = true;
                        break;
                    }
                    //str= myReader["FKUnit_ID"].ToString();  
                }
                myReader.Dispose();

                if (exists == false)
                {

                    try
                    {
                        string comm = "INSERT INTO  [MessageSchedule](FKUnit_ID) VALUES(" + this.textBox1.Text + ")";
                        System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(comm, Form1.sh.con);
                        myCommand.ExecuteNonQuery();
                        string[] str = new string[6];
                        str[0] = this.textBox1.Text;
                        str[1] = "";
                        str[2] = "";
                        str[3] = "";
                        str[4] = "";
                        str[5] = "";

                        ListViewItem itm = new ListViewItem(str);
                        itm.Name = str[0];
                        Form1.sh.AddToQuerry(itm);
                        Form1.sh.UpdateScheduleGrid();
                        this.textBox1.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
    }
}
