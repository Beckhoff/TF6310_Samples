using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcTcpIpSampleClient
{
    /// <summary>
    /// This class implements a simple TCP/IP client that connects to a TCP/IP server, sends messages, and receives responses. It provides methods to enable/disable the client, send messages, and get the current connection status. The client runs a background task to maintain the connection to the server and automatically reconnect if the connection is lost.
    /// </summary>
    internal class TcpIpClient
    {
        /// <summary>
        /// The size of the receive buffer used for receiving responses from the server. This constant defines the maximum number of bytes that can be received in a single operation. It is set to 256 bytes, which should be sufficient for most simple messages. If larger messages are expected, this value may need to be increased accordingly.
        /// </summary>
        private const int RCVBUFFERSIZE = 256; // buffer size for receive buffer

        /// <summary>
        /// The default IP address used when enabling the client without specifying a host. This constant is set to "
        /// </summary>
        public const string DEFAULTIP = "127.0.0.1";

        /// <summary>
        /// The default port number used when enabling the client without specifying a port. This constant is set to "200", which is the port number that the sample TCP/IP server listens on. If the server is configured to listen on a different port, this value should be changed accordingly.
        /// </summary>
        public const string DEFAULTPORT = "200";

        /// <summary>
        /// Indicates whether the client is currently connected to the TCP/IP server. This boolean field is set to true when a successful connection is established and set to false when the connection is lost or the client is disabled. It is used to determine whether messages can be sent to the server and whether the client should attempt to reconnect in the background.
        /// </summary>
        private bool _isConnected;

        /// <summary>
        /// Indicates whether the client is currently enabled. This boolean field is set to true when the Enable method is called and set to false when the Disable method is called. It is used to determine whether the client should attempt to connect to the server and whether messages can be sent. If the client is not enabled, any attempts to send messages will result in an error message prompting the user to enable the client first.
        /// </summary>
        private bool _isEnabled;

        /// <summary>
        /// The socket object used for the TCP/IP connection to the server. This field is initialized when a connection is established and is used to send and receive data over the network. If the connection is lost or the client is disabled, this field is set to null. It is important to check whether this field is null before attempting to send or receive data to avoid null reference exceptions.
        /// </summary>
        private Socket? _socket;

        /// <summary>
        /// The IP endpoint representing the server's address and port. This field is set when the Enable method is called with a valid host and port. It is used to establish the TCP/IP connection to the server. If the host or port cannot be parsed, this field remains null, and an error message is displayed to the user. It is important to ensure that this field is not null before attempting to connect to the server.
        /// </summary>
        private IPEndPoint? _ipAddress;

        /// <summary>
        /// The receive buffer used for receiving responses from the server. This byte array is initialized with a size defined by the RCVBUFFERSIZE constant and is used to store incoming data from the server. When a message is sent to the server, the client waits for a response and reads the data into this buffer. After receiving the data, it is decoded and printed to the console. It is important to ensure that the buffer size is sufficient for the expected response size to avoid truncation of data.
        /// </summary>
        private readonly byte[] _rcvBuffer = new byte[RCVBUFFERSIZE];

        /// <summary>
        /// The cancellation token source used to signal the background reconnect loop to stop. This field is initialized when the Enable method is called and is used to cancel the reconnect loop when the Disable method is called. It allows the background task to exit gracefully and ensures that resources are released properly. If the client is not enabled, this field remains null, and no cancellation is necessary.
        /// </summary>
        private CancellationTokenSource? _reconnectCts;

        /// <summary>
        /// The background task that runs the reconnect loop. This field is initialized when the Enable method is called and is used to maintain a connection to the server. The reconnect loop attempts to connect to the server if the client is not connected and waits for a specified delay between attempts. When the Disable method is called, this task is canceled, and the client waits for it to complete before proceeding. If the client is not enabled, this field remains null, and no background task is running.
        /// </summary>
        private Task? _reconnectTask;

        /// <summary>
        /// Gets a value indicating whether the TCP/IP client is currently enabled. This property returns true if the Enable method has been called and the client is actively attempting to connect to the server; otherwise, it returns false. It can be used to check the client's state before attempting to send messages or perform other operations that require an active connection.
        /// </summary>
        public bool IsEnabled => _isEnabled;

        /// <summary>
        /// Gets a value indicating whether the TCP/IP client is currently connected to the server. This property returns true if a successful connection has been established and the client is able to send and receive messages; otherwise, it returns false. It can be used to check the connection status before attempting to send messages or perform other operations that require an active connection.
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// Gets the host (IP address) of the TCP/IP server to which the client is connected. This property returns the string representation of the IP address if the client is enabled and has a valid endpoint; otherwise, it returns null. It can be used to display the current server address or for logging purposes.
        /// </summary>
        public string? Host => _ipAddress?.Address.ToString();

        /// <summary>
        /// Gets the port number of the TCP/IP server to which the client is connected. This property returns the port number if the client is enabled and has a valid endpoint; otherwise, it returns null. It can be used to display the current server port or for logging purposes.
        /// </summary>
        public int? Port => _ipAddress?.Port;

        /// <summary>
        /// Enables the TCP/IP client by parsing the provided host and port, establishing a connection to the server, and starting a background task to maintain the connection. If the client is already enabled, it will prompt the user to disable it first. If the host or port cannot be parsed, an error message will be displayed.
        /// </summary>
        /// <param name="host">The IP address or hostname of the server to connect to.</param>
        /// <param name="port">The port number of the server to connect to.</param>
        /// <returns>True if the client was successfully enabled; otherwise, false.</returns>
        public bool Enable(string host, string port)
        {
            if (_isEnabled)
            {
                Console.WriteLine("Client is already enabled. Use 'disable' first.");
                return false;
            }

            try
            {
                _ipAddress = new IPEndPoint(IPAddress.Parse(host), Convert.ToInt32(port));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not parse entered IP address. Please check spelling and retry. " + ex.Message);
                return false;
            }

            _isEnabled = true;
            _reconnectCts = new CancellationTokenSource();
            var token = _reconnectCts.Token;
            _reconnectTask = Task.Run(() => ReconnectLoop(token), token);

            Console.WriteLine($"Client enabled for {host}:{port}.");
            return true;
        }

        /// <summary>
        /// Runs a loop that attempts to maintain a connection to the TCP/IP server. If the client is not connected, it will call the Connect method to establish a connection. The loop will continue until the provided cancellation token is signaled, at which point it will exit gracefully. The loop includes a delay of 1 second between connection attempts to avoid overwhelming the server with rapid reconnection attempts.
        /// </summary>
        /// <param name="token">The cancellation token used to signal when the loop should exit.</param>
        private void ReconnectLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!_isConnected)
                    Connect();

                try
                {
                    Task.Delay(1000, token).Wait(token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Attempts to establish a TCP/IP connection to the server using the IP address and port specified in the _ipAddress field. If the connection is successful, it sets the _isConnected flag to true and outputs a status message. If the connection fails, it catches any exceptions and outputs an error message. This method is called by the ReconnectLoop method when the client is not connected to the server.
        /// </summary>
        private void Connect()
        {
            // Check if the client is enabled and the IP address is set before attempting to connect
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                _socket.Connect(_ipAddress!);
                _isConnected = true;
                if (_socket.Connected)
                    Console.WriteLine("[STATUS] Connection to host established!");
                else
                    Console.WriteLine("[STATUS] A connection to the host could not be established!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while establishing a connection to the server: " + ex.Message);
            }
        }

        /// <summary>
        /// Sends a message to the connected TCP/IP server. If the client is not enabled or not connected, it will output an appropriate message and return without sending. The message is converted to a byte array with a termination character appended, and then sent over the established socket connection. After sending, it waits for a response from the server and outputs the received message to the console. Any exceptions during sending or receiving are caught and logged.
        /// </summary>
        /// <param name="message">The message to send to the server.</param>
        public void Send(string message)
        {
            if (!_isEnabled)
            {
                Console.WriteLine("Client is not enabled. Use 'enable <host> <port>' first.");
                return;
            }

            if (!_isConnected || _socket == null)
            {
                Console.WriteLine("Not connected to host yet. Please wait for a connection or check server availability.");
                return;
            }

            byte[] tempBuffer = Encoding.ASCII.GetBytes(message);
            byte[] sendBuffer = new byte[tempBuffer.Length + 1];
            Array.Copy(tempBuffer, sendBuffer, tempBuffer.Length);
            sendBuffer[tempBuffer.Length] = 0;

            // Send the message to the TCP/IP server and wait for a response. If the send operation fails or returns zero bytes sent, an exception is thrown. After sending, the client begins receiving data from the server asynchronously and waits for the receive operation to complete. Once the response is received, it is decoded and printed to the console.
            try
            {
                int send = _socket.Send(sendBuffer);
                if (send == 0)
                    throw new Exception("Zero bytes sent.");

                // If the send operation is successful, print a status message and begin receiving the response from the server.
                Console.WriteLine("[STATUS] Message successfully sent!");
                IAsyncResult asyncRes = _socket.BeginReceive(_rcvBuffer, 0, RCVBUFFERSIZE, SocketFlags.None, null, null);
                if (asyncRes.AsyncWaitHandle.WaitOne())
                {
                    int res = _socket.EndReceive(asyncRes);
                    char[] resChars = new char[res + 1];
                    Decoder d = Encoding.UTF8.GetDecoder();
                    int charLength = d.GetChars(_rcvBuffer, 0, res, resChars, 0, true);
                    string result = new string(resChars);
                    Console.WriteLine("[RECV] " + result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while sending the message: " + ex.Message);
            }
        }

        /// <summary>
        /// Disables the TCP/IP client by canceling the background reconnect loop, waiting for it to complete, and disconnecting from the server if connected. If the client is not enabled, it will output a message indicating that it is already disabled. After disabling, it sets the _isConnected and _isEnabled flags to false and outputs a status message indicating that the connection to the host has been closed.
        /// </summary>
        public void Disable()
        {
            if (!_isEnabled)
            {
                Console.WriteLine("Client is not enabled.");
                return;
            }

            _reconnectCts?.Cancel();
            try
            {
                _reconnectTask?.Wait();
            }
            catch (AggregateException)
            {
                // expected when the reconnect loop task observes cancellation
            }

            if (_socket != null)
            {
                try
                {
                    if (_socket.Connected)
                        _socket.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured while disconnecting: " + ex.Message);
                }
            }

            _isConnected = false;
            _isEnabled = false;
            Console.WriteLine("[STATUS] Connection to host closed!");
        }

        /// <summary>
        /// Gets the current status of the TCP/IP client, indicating whether it is enabled and connected to the server. If the client is disabled, it returns a message stating that it is disabled. If the client is enabled but not connected, it returns a message indicating that it is trying to connect to the specified host and port. If the client is enabled and connected, it returns a message confirming the connection to the specified host and port.
        /// </summary>
        /// <returns>A string representing the current status of the TCP/IP client.</returns>
        public string GetStatus()
        {
            if (!_isEnabled)
                return "Disabled.";

            return _isConnected
                ? $"Enabled and connected to {Host}:{Port}."
                : $"Enabled, trying to connect to {Host}:{Port}...";
        }
    }
}
