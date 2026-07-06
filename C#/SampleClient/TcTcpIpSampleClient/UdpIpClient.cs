using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcTcpIpSampleClient
{
    /// <summary>
    /// Represents a UDP client that can send and receive UDP messages. It provides methods to start listening for incoming messages, send messages to a specified host and port, and stop listening. The class implements IDisposable to ensure proper cleanup of resources.
    /// </summary>
    internal class UdpIpClient : IDisposable
    {
        public const string DEFAULTIP = "127.0.0.1"; // Used as default destination IP address
        public const string DEFAULTDESTPORT = "10000"; // Used as default destination port
        private const int DEFAULTOWNPORT = 11000; // Used for binding when listening for UDP messages
        private const int DEFAULTSOURCEPORT = 11001; // Only used as source port when sending UDP messages

        private UdpClient? _listener; // used to listen for incoming UDP packets
        private CancellationTokenSource? _listenCts; // cancellation for the background receive loop
        private Task? _listenTask; // background task for the receive loop

        /// <summary>
        /// Starts listening for incoming UDP packets on the default port.
        /// </summary>
        public void StartListening()
        {
            _listenCts = new CancellationTokenSource();
            _listener = new UdpClient(new IPEndPoint(IPAddress.Any, DEFAULTOWNPORT));
            _listenTask = Task.Run(() => ListenLoop(_listenCts.Token));
            Console.WriteLine($"[STATUS] Listening for incoming UDP packets on port {DEFAULTOWNPORT}...");
        }

        /// <summary>
        /// Listen for incoming UDP packets in a loop until the cancellation token is triggered. When a packet is received, print its content to the console with a timestamp.
        /// </summary>
        /// <param name="token">The cancellation token used to stop the listening loop.</param>
        /// <returns>A task representing the asynchronous listening operation.</returns>
        private async Task ListenLoop(CancellationToken token)
        {
            // This loop will run until the cancellation token is triggered. It listens for incoming UDP packets and prints their content to the console.
            try
            {
                while (!token.IsCancellationRequested)
                {
                    UdpReceiveResult result = await _listener!.ReceiveAsync(token);
                    string text = Encoding.ASCII.GetString(result.Buffer);
                    Console.WriteLine($"\n[RECV] {DateTime.Now}: {text}");
                    Console.Write("> ");
                }
            }
            catch (OperationCanceledException)
            {
                // expected when StopListening() cancels the loop
            }
            catch (ObjectDisposedException)
            {
                // expected when the underlying socket is closed while a receive is pending
            }
        }

        /// <summary>
        /// Sends a UDP message to the specified host and port. It creates a UdpClient, connects to the destination, and sends the provided message. If an error occurs during sending, it catches the exception and prints an error message to the console.
        /// </summary>
        /// <param name="host">The destination host IP address.</param>
        /// <param name="port">The destination port number.</param>
        /// <param name="message">The message to be sent.</param>
        public void Send(string host, string port, string message)
        {
            try
            {
                IPEndPoint ipAddress = new IPEndPoint(IPAddress.Parse(host), Convert.ToInt32(port));
                using UdpClient udpClient = new UdpClient(DEFAULTSOURCEPORT);
                udpClient.Connect(ipAddress);

                byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
                udpClient.Send(sendBuffer, sendBuffer.Length);

                Console.WriteLine("[STATUS] Message successfully sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while sending the message: " + ex.Message);
            }
        }

        /// <summary>
        /// Stops the background listener by canceling the listening task and closing the UDP client. It waits for the listening task to complete, ignoring any exceptions that may occur during the shutdown process.
        /// </summary>
        public void StopListening()
        {
            _listenCts?.Cancel();
            _listener?.Close();
            try
            {
                _listenTask?.Wait(500);
            }
            catch (Exception)
            {
                // ignore exceptions raised while unwinding the listen loop
            }
        }

        /// <summary>
        /// Disposes the UDP client by stopping the background listener.
        /// </summary>
        public void Dispose()
        {
            StopListening();
        }
    }
}
