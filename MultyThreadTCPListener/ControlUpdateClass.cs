using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultyThreadTCPListener
{
    class ControlUpdateClass
    {
        Form1 form1;
        ControlUpdateClass(Form1 fo)
        {
            this.form1 = fo;

        }

        public void SomethingToDo(string str)
        {
            //I will create an example loop which, 
            //from which I will pass the value to textBox on form1 class:
           
                form1.UpdatingTextBox(str);
                //wait for a bit:
             
        }
    }
}
