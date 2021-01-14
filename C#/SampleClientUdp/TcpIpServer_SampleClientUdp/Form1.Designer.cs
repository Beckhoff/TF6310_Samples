namespace TcpIpServer_SampleClientUdp
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
            this.rtb_rcv = new System.Windows.Forms.RichTextBox();
            this.txt_send = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.txt_host = new System.Windows.Forms.TextBox();
            this.cmd_send = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtb_rcv
            // 
            this.rtb_rcv.Location = new System.Drawing.Point(0, 0);
            this.rtb_rcv.Name = "rtb_rcv";
            this.rtb_rcv.Size = new System.Drawing.Size(284, 96);
            this.rtb_rcv.TabIndex = 0;
            this.rtb_rcv.Text = "";
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(64, 130);
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(141, 20);
            this.txt_send.TabIndex = 1;
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(211, 102);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(61, 20);
            this.txt_port.TabIndex = 2;
            // 
            // txt_host
            // 
            this.txt_host.Location = new System.Drawing.Point(64, 102);
            this.txt_host.Name = "txt_host";
            this.txt_host.Size = new System.Drawing.Size(106, 20);
            this.txt_host.TabIndex = 3;
            // 
            // cmd_send
            // 
            this.cmd_send.Location = new System.Drawing.Point(211, 127);
            this.cmd_send.Name = "cmd_send";
            this.cmd_send.Size = new System.Drawing.Size(61, 23);
            this.cmd_send.TabIndex = 4;
            this.cmd_send.Text = "&Send";
            this.cmd_send.UseVisualStyleBackColor = true;
            this.cmd_send.Click += new System.EventHandler(this.cmd_send_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Host:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Message:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 173);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmd_send);
            this.Controls.Add(this.txt_host);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.rtb_rcv);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 211);
            this.MinimumSize = new System.Drawing.Size(300, 211);
            this.Name = "Form1";
            this.Text = "UDP Sample Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_rcv;
        private System.Windows.Forms.TextBox txt_send;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.TextBox txt_host;
        private System.Windows.Forms.Button cmd_send;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

