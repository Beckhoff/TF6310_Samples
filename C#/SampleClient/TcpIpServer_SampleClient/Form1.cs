using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

/* ##########################################################################################
 * This sample TCP/IP client connects to a TCP/IP-Server, sends a message and waits for the
 * response. It is being delivered together with our TCP-Sample, which implements an echo server
 * in PLC.
 * ########################################################################################## */
namespace TcpIpServer_SampleClient
{
    public partial class Form1 : Form
    {
        /* ##########################################################################################
         * Constants
         * ########################################################################################## */
        private const int RCVBUFFERSIZE = 256; // buffer size for receive buffer
        private const string DEFAULTIP = "127.0.0.1";
        private const string DEFAULTPORT = "200";

        /* ##########################################################################################
         * Global variables
         * ########################################################################################## */
        private static bool _isConnected; // signals whether socket connection is active or not
        private static Socket _socket; // object used for socket connection to TCP/IP-Server
        private static IPEndPoint _ipAddress; // contains IP address as entered in text field
        private static byte[] _rcvBuffer; // receive buffer used for receiving response from TCP/IP-Server

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _rcvBuffer = new byte[RCVBUFFERSIZE];

            /* ##########################################################################################
             * Prepare GUI
             * ########################################################################################## */
            cmd_send.Enabled = false;
            cmd_enable.Enabled = true;
            cmd_disable.Enabled = false;
            rtb_rcvMsg.Enabled = false;
            rtb_sendMsg.Enabled = false;
            rtb_statMsg.Enabled = false;
            txt_host.Text = DEFAULTIP;
            txt_port.Text = DEFAULTPORT;

            timer1.Enabled = false;
            _isConnected = false;
        }

        private void cmd_enable_Click(object sender, EventArgs e)
        {
            /* ##########################################################################################
             * Parse IP address in text field, start background timer and prepare GUI
             * ########################################################################################## */
            try
            {
                _ipAddress = new IPEndPoint(IPAddress.Parse(txt_host.Text), Convert.ToInt32(txt_port.Text));
                timer1.Enabled = true;
                cmd_enable.Enabled = false;
                cmd_disable.Enabled = true;
                rtb_sendMsg.Enabled = true;
                cmd_send.Enabled = true;
                txt_host.Enabled = false;
                txt_port.Enabled = false;
                rtb_sendMsg.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not parse entered IP address. Please check spelling and retry. " + ex);
            }
        }

        /* ##########################################################################################
         * Timer periodically checks for connection to TCP/IP-Server and reestablishes if not connected
         * ########################################################################################## */
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_isConnected)
                connect();
        }

        private void connect()
        {
            /* ##########################################################################################
             * Connect to TCP/IP-Server using the IP address specified in the text field
             * ########################################################################################## */
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                _socket.Connect(_ipAddress);
                _isConnected = true;
                if (_socket.Connected)
                    rtb_statMsg.AppendText("Connectection to host established!\n");
                else
                    rtb_statMsg.AppendText("A connection to the host could not be established!\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while establishing a connection to the server: " + ex);
            }
        }

        private void cmd_send_Click(object sender, EventArgs e)
        {
            /* ##########################################################################################
             * Read message from text field and prepare send buffer, which is a byte[] array. The last
             * character in the buffer needs to be a termination character, so that the TCP/IP-Server knows
             * when the TCP stream ends. In this case, the termination character is '0'.
             * ########################################################################################## */
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] tempBuffer = enc.GetBytes(rtb_sendMsg.Text);
            byte[] sendBuffer = new byte[tempBuffer.Length + 1];
            for (int i = 0; i < tempBuffer.Length; i++)
                sendBuffer[i] = tempBuffer[i];
            sendBuffer[tempBuffer.Length] = 0;

            /* ##########################################################################################
             * Send buffer content via TCP/IP connection
             * ########################################################################################## */
            try
            {
                int send = _socket.Send(sendBuffer);
                if (send == 0)
                    throw new Exception();
                else
                {
                    /* ##########################################################################################
                     * As the TCP/IP-Server returns a message, receive this message and store content in receive buffer.
                     * When message receive is complete, show the received message in text field.
                     * ########################################################################################## */
                    rtb_statMsg.AppendText("Message successfully sent!\n");
                    IAsyncResult asynRes = _socket.BeginReceive(_rcvBuffer, 0, 256, SocketFlags.None, null, null);
                    if (asynRes.AsyncWaitHandle.WaitOne())
                    {
                        int res = _socket.EndReceive(asynRes);
                        char[] resChars = new char[res + 1];
                        Decoder d = Encoding.UTF8.GetDecoder();
                        int charLength = d.GetChars(_rcvBuffer, 0, res, resChars, 0, true);
                        String result = new String(resChars);
                        rtb_rcvMsg.AppendText("\n" + result);
                        rtb_sendMsg.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while sending the message: " + ex);
            }
        }

        private void cmd_disable_Click(object sender, EventArgs e)
        {
            /* ##########################################################################################
             * Disconnect from TCP/IP-Server, stop the timer and prepare GUI
             * ########################################################################################## */
            timer1.Enabled = false;
            _socket.Disconnect(true);
            if (!_socket.Connected)
            {
                _isConnected = false;
                cmd_disable.Enabled = false;
                cmd_enable.Enabled = true;
                txt_host.Enabled = true;
                txt_port.Enabled = true;
                rtb_sendMsg.Enabled = false;
                cmd_send.Enabled = false;
                rtb_statMsg.AppendText("Connectection to host closed!\n");
                rtb_rcvMsg.Clear();
                rtb_statMsg.Clear();
            }
        }
    }
}
