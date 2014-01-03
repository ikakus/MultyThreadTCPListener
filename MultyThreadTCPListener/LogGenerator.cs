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
    public class LogGenerator
    {
        string fileLoc = @"Log.txt";

        public void writeLastReceivedLogFile(DateTime date)
        {


            if (File.Exists(fileLoc))
            {
                using (StreamWriter sw = new StreamWriter(fileLoc))
                {
                    try
                    {

                        sw.WriteLine("Started = " + Form1.StartedTime.ToString());
                        sw.WriteLine("Last received data  was at = " + date.ToString());
                        sw.Close();
                    }
                    catch (Exception )
                    {
 
                    }
                }

            }
            else
            {
                try
                {
                    File.Create(fileLoc);
                }
                catch(Exception)
                {

                }
            }
        }

      
    }
}
