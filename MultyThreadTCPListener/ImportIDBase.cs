using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MultyThreadTCPListener
{
    public partial class ImportIDBase : Form
    {
        public ImportIDBase()
        {
            InitializeComponent();
        }

        private void ImortIDBase_Load(object sender, EventArgs e)
        {
            button_import.Enabled = false;
        }

        string connectionString1;
        string connectionString2;

        bool imported = false;

        bool baseConnect1 = false;
        bool baseConnect2 = false;

        System.Data.SqlClient.SqlConnection con1 = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlConnection con2 = new System.Data.SqlClient.SqlConnection();

        private void ImportUnitIDs()
        {

           

           //"Data Source=IKA-FDF3AA55734\\SQLEXPRESS;Initial Catalog=Point;Persist Security Info=True;User ID=sa;Password=12345";
            // "Data Source=IKA-FDF3AA55734\\SQLEXPRESS;Initial Catalog=TracerDB;Persist Security Info=True;User ID=sa;Password=12345";
            System.Data.SqlClient.SqlDataReader myReader = null;
            string str; 


            string comm = "SELECT * FROM [Unit]";
           // con.Open();

            System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(comm, con1);
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                str = myReader["FK_UnitID"].ToString();
               // MessageBox.Show(str);

                //con2.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [MessageSchedule](FKUnit_ID) VALUES(" + str + ")", con2);
                cmd.ExecuteNonQuery();
                

            }
            con1.Close();
            con2.Close();
        }


        private void ClearIDBase()
        {
            string comm = "DELETE FROM [MessageSchedule]";
            // con.Open();

            System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(comm, con2);
            myCommand.ExecuteNonQuery();
        }

        private void button_connect1_Click(object sender, EventArgs e)
        {
            if (textBox_DataBaseName1.Text != "" &&
                textBox_PAssword1.Text != "" &&
                textBox_PSI1.Text == "True" ||
                 textBox_PSI1.Text == "False" &&
                textBox_serverName1.Text != "" &&
                textBox_UserID1.Text != "")
            {
                connectionString1 = "Data Source="+textBox_serverName1.Text+
                    "\\SQLEXPRESS;Initial Catalog="+textBox_DataBaseName1.Text+
                    ";Persist Security Info=" + textBox_PSI1.Text +
                    ";User ID=" + textBox_UserID1.Text +
                    ";Password=" + textBox_PAssword1.Text;
             //   MessageBox.Show(connectionString1);
                con1.Close();
                con1.ConnectionString = connectionString1;
                try
                {
                    
                    con1.Open();
                    Status1.Text = "Connected";
                    baseConnect1 = true;
                    checkBothConnections();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Status1.Text = "Not Connected";
                }
            }
        }




        private void button_connect2_Click(object sender, EventArgs e)
        {
            if (textBox_DataBaseName2.Text != "" &&
               textBox_PAssword2.Text != "" &&
               textBox_PSI2.Text == "True" ||
               textBox_PSI1.Text == "False" &&
               textBox_serverName2.Text != "" &&
               textBox_UserID2.Text != "")
            {
                connectionString2 = "Data Source=" + textBox_serverName2.Text +
                    "\\SQLEXPRESS;Initial Catalog=" + textBox_DataBaseName2.Text +
                    ";Persist Security Info=" + textBox_PSI2.Text +
                    ";User ID=" + textBox_UserID2.Text +
                    ";Password=" + textBox_PAssword2.Text;
              //  MessageBox.Show(connectionString2);
                con2.Close();
                con2.ConnectionString = connectionString2;
                try
                {

                    con2.Open();
                    Status2.Text = "Connected";
                    baseConnect2 = true;
                    checkBothConnections();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Status2.Text = "Not Connected";
                }
            }
        }

        private void checkBothConnections()
        
        {

            if (baseConnect1 == true && baseConnect2 == true)
            {
                button_import.Enabled = true;
            }
        }

        private void button_import_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to import new database? Old one will be deleted", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ClearIDBase();
                ImportUnitIDs();
                MessageBox.Show("ID database imported sucessfully", "", MessageBoxButtons.OK);
                imported = true;
                this.Close();
            }
        }


        private void ImportIDBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (imported == true)
            {
                Form1.sh.Close();
            }
        }




    }
}
