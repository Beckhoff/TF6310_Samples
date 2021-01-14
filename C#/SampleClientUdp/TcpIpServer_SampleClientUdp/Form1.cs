using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace TcpIpServer_SampleClientUdp
{
    public partial class Form1 : Form
    {
        /* ##########################################################################################
         * Constants
         * ########################################################################################## */
        private const string DEFAULTIP = "127.0.0.1";
        private const int DEFAULTDESTPORT = 1001; // Used as destination port
        private const int DEFAULTOWNPORT = 1002; // Used for binding when listening for UDP messages
        private const int DEFAULTSOURCEPORT = 11000; // Only used as source port when sending UDP messages


        /* ##########################################################################################
         * Global variables
         * ########################################################################################## */
        private static UdpClient _udpClient;
        private static IPEndPoint _ipAddress; // Contains IP address as entered in text field
        private static Thread _rcvThread; // Background thread used to listen for incoming UDP packets


        public Form1()
        {
            InitializeComponent();
        }


        /* ##########################################################################################
         * Event handler method called when button "Send" is pressed
         * ########################################################################################## */
        private void cmd_send_Click(object sender, EventArgs e)
        {
            byte[] sendBuffer = null;

            /* ##########################################################################################
             * Preparing UdpClient, connecting to UDP server and sending content of text field
             * ########################################################################################## */
            try
            {
                _ipAddress = new IPEndPoint(IPAddress.Parse(txt_host.Text), Convert.ToInt32(txt_port.Text));
                _udpClient = new UdpClient(DEFAULTSOURCEPORT);
                _udpClient.Connect(_ipAddress);
                
                sendBuffer = Encoding.ASCII.GetBytes(txt_send.Text);
                _udpClient.Send(sendBuffer, sendBuffer.Length);
                
                _udpClient.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unknown error occured: " + ex);
            }
        }


        /* ##########################################################################################
         * Event handler method called when application starts
         * ########################################################################################## */
        private void Form1_Load(object sender, EventArgs e)
        {
            txt_host.Text = DEFAULTIP;
            txt_port.Text = DEFAULTDESTPORT.ToString();
            rtb_rcv.Enabled = false;

            /* ##########################################################################################
             * Creating background thread which synchronously listens for incoming UDP packets
             * ########################################################################################## */
            _rcvThread = new Thread(rcvThreadMethod);
            _rcvThread.Start();
        }


        /* ##########################################################################################
         * Delegate, so that background thread may write into text field on GUI
         * ########################################################################################## */
        public delegate void rcvThreadCallback(string text);


        /* ##########################################################################################
         * Method called by background thread
         * ########################################################################################## */
        private void rcvThreadMethod()
        {
            /* ##########################################################################################
             * Listen on any available local IP address and specified port (DEFAULTOWNPORT)
             * ########################################################################################## */
            byte[] rcvBuffer = null;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, DEFAULTOWNPORT);;
            UdpClient udpClient = new UdpClient(ipEndPoint);

            /* ##########################################################################################
             * Continously start a synchronous listen for incoming UDP packets. If a packet has arrived,
             * write its content to receive buffer and then into the text field. After that, start circle
             * again.
             * ########################################################################################## */
            while (true)
            {
                rcvBuffer = udpClient.Receive(ref ipEndPoint); // synchronous call
                rtb_rcv.Invoke(new rcvThreadCallback(this.AppendText), new object[] { "\n" + DateTime.Now.ToString() + ": " + Encoding.ASCII.GetString(rcvBuffer) });
            }
        }


        /* ##########################################################################################
         * Helper method for delegate
         * ########################################################################################## */
        private void AppendText(string text)
        {
            rtb_rcv.AppendText(text);
        }


        /* ##########################################################################################
         * Stop background thread when application closes
         * ########################################################################################## */
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rcvThread.Abort();
        }
    }
}
