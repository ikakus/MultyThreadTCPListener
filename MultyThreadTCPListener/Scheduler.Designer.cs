namespace MultyThreadTCPListener
{
    partial class Scheduler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_scheduleMessage = new System.Windows.Forms.Button();
            this.textBox_ScheduleMessage = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button_select = new System.Windows.Forms.Button();
            this.button_deselect = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 40);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(610, 373);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 100;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Mes1";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Mes2";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Mes3";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Mes4";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Mes5";
            this.columnHeader5.Width = 100;
            // 
            // button_scheduleMessage
            // 
            this.button_scheduleMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_scheduleMessage.Location = new System.Drawing.Point(12, 419);
            this.button_scheduleMessage.Name = "button_scheduleMessage";
            this.button_scheduleMessage.Size = new System.Drawing.Size(85, 36);
            this.button_scheduleMessage.TabIndex = 1;
            this.button_scheduleMessage.Text = "Schedule message";
            this.button_scheduleMessage.UseVisualStyleBackColor = true;
            this.button_scheduleMessage.Click += new System.EventHandler(this.button_scheduleMessage_Click);
            // 
            // textBox_ScheduleMessage
            // 
            this.textBox_ScheduleMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_ScheduleMessage.Location = new System.Drawing.Point(103, 419);
            this.textBox_ScheduleMessage.Name = "textBox_ScheduleMessage";
            this.textBox_ScheduleMessage.Size = new System.Drawing.Size(323, 20);
            this.textBox_ScheduleMessage.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(540, 419);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "Remove form all selected";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_select
            // 
            this.button_select.Location = new System.Drawing.Point(12, 6);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(79, 28);
            this.button_select.TabIndex = 4;
            this.button_select.Text = "Select All";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // button_deselect
            // 
            this.button_deselect.Location = new System.Drawing.Point(97, 6);
            this.button_deselect.Name = "button_deselect";
            this.button_deselect.Size = new System.Drawing.Size(79, 28);
            this.button_deselect.TabIndex = 5;
            this.button_deselect.Text = "Deselect All";
            this.button_deselect.UseVisualStyleBackColor = true;
            this.button_deselect.Click += new System.EventHandler(this.button_deselect_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(515, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "Load ID Base";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(438, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(71, 28);
            this.button3.TabIndex = 7;
            this.button3.Text = "Add ID";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // Scheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 467);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_deselect);
            this.Controls.Add(this.button_select);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_ScheduleMessage);
            this.Controls.Add(this.button_scheduleMessage);
            this.Controls.Add(this.listView1);
            this.MinimumSize = new System.Drawing.Size(642, 501);
            this.Name = "Scheduler";
            this.Text = "Scheduler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Scheduler_FormClosing);
            this.Load += new System.EventHandler(this.Scheduler_Load);
            this.SizeChanged += new System.EventHandler(this.Scheduler_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button button_scheduleMessage;
        private System.Windows.Forms.TextBox textBox_ScheduleMessage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.Button button_deselect;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;

    }
}