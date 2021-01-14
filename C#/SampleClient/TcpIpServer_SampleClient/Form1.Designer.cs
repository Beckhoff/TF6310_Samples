namespace TcpIpServer_SampleClient
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.txt_host = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.cmd_disable = new System.Windows.Forms.Button();
            this.cmd_enable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtb_sendMsg = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rtb_rcvMsg = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rtb_statMsg = new System.Windows.Forms.RichTextBox();
            this.cmd_send = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txt_host
            // 
            this.txt_host.Location = new System.Drawing.Point(81, 31);
            this.txt_host.Name = "txt_host";
            this.txt_host.Size = new System.Drawing.Size(100, 20);
            this.txt_host.TabIndex = 0;
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(253, 31);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(100, 20);
            this.txt_port.TabIndex = 1;
            // 
            // cmd_disable
            // 
            this.cmd_disable.Location = new System.Drawing.Point(253, 67);
            this.cmd_disable.Name = "cmd_disable";
            this.cmd_disable.Size = new System.Drawing.Size(100, 23);
            this.cmd_disable.TabIndex = 2;
            this.cmd_disable.Text = "Disable";
            this.cmd_disable.UseVisualStyleBackColor = true;
            this.cmd_disable.Click += new System.EventHandler(this.cmd_disable_Click);
            // 
            // cmd_enable
            // 
            this.cmd_enable.Location = new System.Drawing.Point(81, 67);
            this.cmd_enable.Name = "cmd_enable";
            this.cmd_enable.Size = new System.Drawing.Size(100, 23);
            this.cmd_enable.TabIndex = 3;
            this.cmd_enable.Text = "Enable";
            this.cmd_enable.UseVisualStyleBackColor = true;
            this.cmd_enable.Click += new System.EventHandler(this.cmd_enable_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Host:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port:";
            // 
            // rtb_sendMsg
            // 
            this.rtb_sendMsg.Location = new System.Drawing.Point(14, 129);
            this.rtb_sendMsg.Name = "rtb_sendMsg";
            this.rtb_sendMsg.Size = new System.Drawing.Size(415, 96);
            this.rtb_sendMsg.TabIndex = 6;
            this.rtb_sendMsg.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Send to host:";
            // 
            // rtb_rcvMsg
            // 
            this.rtb_rcvMsg.Location = new System.Drawing.Point(15, 267);
            this.rtb_rcvMsg.Name = "rtb_rcvMsg";
            this.rtb_rcvMsg.Size = new System.Drawing.Size(414, 96);
            this.rtb_rcvMsg.TabIndex = 8;
            this.rtb_rcvMsg.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Received from host:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 394);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Status messages:";
            // 
            // rtb_statMsg
            // 
            this.rtb_statMsg.Location = new System.Drawing.Point(15, 410);
            this.rtb_statMsg.Name = "rtb_statMsg";
            this.rtb_statMsg.Size = new System.Drawing.Size(414, 96);
            this.rtb_statMsg.TabIndex = 11;
            this.rtb_statMsg.Text = "";
            // 
            // cmd_send
            // 
            this.cmd_send.Location = new System.Drawing.Point(354, 231);
            this.cmd_send.Name = "cmd_send";
            this.cmd_send.Size = new System.Drawing.Size(75, 23);
            this.cmd_send.TabIndex = 12;
            this.cmd_send.Text = "Send";
            this.cmd_send.UseVisualStyleBackColor = true;
            this.cmd_send.Click += new System.EventHandler(this.cmd_send_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 524);
            this.Controls.Add(this.cmd_send);
            this.Controls.Add(this.rtb_statMsg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rtb_rcvMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rtb_sendMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmd_enable);
            this.Controls.Add(this.cmd_disable);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.txt_host);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(457, 562);
            this.MinimumSize = new System.Drawing.Size(457, 562);
            this.Name = "Form1";
            this.Text = "TCP/IP Sample Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_host;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Button cmd_disable;
        private System.Windows.Forms.Button cmd_enable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtb_sendMsg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtb_rcvMsg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtb_statMsg;
        private System.Windows.Forms.Button cmd_send;
        private System.Windows.Forms.Timer timer1;
    }
}

