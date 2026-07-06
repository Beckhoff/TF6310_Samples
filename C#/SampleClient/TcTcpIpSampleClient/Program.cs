using TcTcpIpSampleClient;

/* ##########################################################################################
 * Command line interpreter for TCP and UDP samples.
 * Instead of choosing the protocol via a command line/build argument, the user selects
 * it interactively with the 'tcp' / 'udp' command. Each protocol requires a different TF6310
 * PLC sample to be loaded:
 *
 *   tcp   - activates the TCP sample client (requires the "TCP Sample01" PLC project)
 *   udp   - activates the UDP sample client (requires the "UDP Sample01" PLC project)
 * ########################################################################################## */

string? activeProtocol = null; // "tcp" or "udp" when a protocol is selected, null otherwise
TcpIpClient? tcpClient = null; // The TCP sample client is only created when the user selects the 'tcp' protocol
UdpIpClient? udpClient = null; // The UDP sample client is only created when the user selects the 'udp' protocol

// Print the initial banner with instructions for selecting a protocol and getting help.
PrintBanner();

// Enter the command interpreter loop. Read a line of input, parse it into a command and an optional argument, and dispatch it to the appropriate handler. If no protocol is selected yet, only the 'tcp', 'udp', 'help' and 'exit' commands are valid.
bool running = true;
while (running)
{
    Console.Write("> ");
    string? line = Console.ReadLine();
    if (line == null)
        break;

    line = line.Trim();
    if (line.Length == 0)
        continue;

    string[] parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    string command = parts[0].ToLowerInvariant();
    string arg = parts.Length > 1 ? parts[1] : string.Empty;

    switch (command)
    {
        case "tcp":
            HandleSelect("tcp");
            break;

        case "udp":
            HandleSelect("udp");
            break;

        case "help":
            PrintHelp();
            break;

        case "exit":
        case "quit":
            Teardown();
            running = false;
            break;

        default:
            DispatchProtocolCommand(command, arg);
            break;
    }
}

// Print the initial banner with instructions for selecting a protocol and getting help.
void PrintBanner()
{
    Console.WriteLine("TCP/UDP Sample Client (.NET 10 console edition)");
    Console.WriteLine("No protocol selected yet. Select protocol with 'tcp' or 'udp' to get started.");
    Console.WriteLine("Type 'help' for a list of available commands.");
    Console.WriteLine();
}

// Print the list of available commands. If a protocol is selected, also print the protocol-specific commands.
void PrintHelp()
{
    Console.WriteLine("Available commands:");
    Console.WriteLine("  tcp            - activate the TCP sample client (requires the 'TCP Sample01' PLC sample)");
    Console.WriteLine("  udp            - activate the UDP sample client (requires the 'UDP Sample01' PLC sample)");
    Console.WriteLine("  help           - show this help text");
    Console.WriteLine("  exit / quit    - close the application");

    if (activeProtocol == "tcp")
    {
        Console.WriteLine();
        Console.WriteLine("TCP commands (active):");
        Console.WriteLine($"  enable [host] [port]  - connect to host:port (default {TcpIpClient.DEFAULTIP}:{TcpIpClient.DEFAULTPORT})");
        Console.WriteLine("  disable               - disconnect from host");
        Console.WriteLine("  send <message>        - send a message to the host and print the response");
        Console.WriteLine("  status                - print the current connection status");
    }
    else if (activeProtocol == "udp")
    {
        Console.WriteLine();
        Console.WriteLine("UDP commands (active):");
        Console.WriteLine($"  send <host> <port> <message>  - send a UDP datagram to host:port (e.g. send {UdpIpClient.DEFAULTIP} {UdpIpClient.DEFAULTDESTPORT} hello)");
        Console.WriteLine("  status                         - print the current listener status");
        Console.WriteLine("  Incoming UDP packets are printed automatically as they arrive.");
    }
}


// Switch the active protocol. Tears down whichever client is currently active (disconnecting / stopping its background task) before activating the newly selected one.
void HandleSelect(string protocol)
{
    if (protocol == activeProtocol)
    {
        Console.WriteLine($"'{protocol}' is already selected.");
        return;
    }

    Teardown();

    // Activate the selected protocol sample client. The TCP sample requires the user to explicitly connect to a host:port, while the UDP sample starts listening for incoming datagrams immediately.
    if (protocol == "tcp")
    {
        tcpClient = new TcpIpClient();
        activeProtocol = "tcp";
        Console.WriteLine("TCP sample client selected.");
        Console.WriteLine("Requires the 'TCP Sample01' PLC sample to be loaded.");
        Console.WriteLine("Use 'enable [host] [port]' to connect, then 'send <message>'. Type 'help' for details.");
    }
    else
    {
        udpClient = new UdpIpClient();
        activeProtocol = "udp";
        Console.WriteLine("UDP sample client selected.");
        Console.WriteLine("Requires the 'UDP Sample01' PLC sample to be loaded.");
        udpClient.StartListening();
        Console.WriteLine("Use 'send <host> <port> <message>' to send a datagram. Type 'help' for details.");
    }
}

// Tear down the currently active protocol client, if any. Disconnects the TCP client or stops the UDP listener, and disposes of the UDP client.
void Teardown()
{
    if (activeProtocol == "tcp" && tcpClient != null)
    {
        if (tcpClient.IsEnabled)
            tcpClient.Disable();
        tcpClient = null;
    }
    else if (activeProtocol == "udp" && udpClient != null)
    {
        udpClient.StopListening();
        udpClient.Dispose();
        udpClient = null;
    }

    activeProtocol = null;
}

// Dispatch the command to the currently active protocol client. If no protocol is selected, print an error message.
void DispatchProtocolCommand(string command, string arg)
{
    if (activeProtocol == null)
    {
        Console.WriteLine("No protocol selected. Use 'tcp' or 'udp' first.");
        return;
    }

    if (activeProtocol == "tcp")
        DispatchTcpCommand(command, arg);
    else
        DispatchUdpCommand(command, arg);
}

// TCP mode: enable, disable, send, status
void DispatchTcpCommand(string command, string arg)
{
    switch (command)
    {
        case "enable":
            HandleTcpEnable(arg);
            break;

        case "disable":
            tcpClient!.Disable();
            break;

        case "send":
            if (string.IsNullOrEmpty(arg))
                Console.WriteLine("Usage: send <message>");
            else
                tcpClient!.Send(arg);
            break;

        case "status":
            Console.WriteLine(tcpClient!.GetStatus());
            break;

        default:
            Console.WriteLine($"Unknown command: '{command}'. Type 'help' for a list of commands.");
            break;
    }
}

// Parse the optional host and port arguments for the 'enable' command, and call TcpIpClient.Enable() with the parsed values. If no arguments are provided, use the default host and port.
void HandleTcpEnable(string argument)
{
    string host = TcpIpClient.DEFAULTIP;
    string port = TcpIpClient.DEFAULTPORT;

    if (!string.IsNullOrEmpty(argument))
    {
        string[] hostPort = argument.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (hostPort.Length >= 1)
            host = hostPort[0];
        if (hostPort.Length >= 2)
            port = hostPort[1];
    }

    tcpClient!.Enable(host, port);
}

// Dispatch the command to the UDP client. The only valid commands are 'send' and 'status'.
void DispatchUdpCommand(string command, string arg)
{
    switch (command)
    {
        case "send":
            HandleUdpSend(arg);
            break;

        case "status":
            Console.WriteLine("Listening for incoming UDP packets. Use 'send <host> <port> <message>' to send a datagram.");
            break;

        default:
            Console.WriteLine($"Unknown command: '{command}'. Type 'help' for a list of commands.");
            break;
    }
}

// Parse the host, port and message arguments for the 'send' command, and call UdpIpClient.Send() with the parsed values. If any of the required arguments are missing, print a usage message.
void HandleUdpSend(string argument)
{
    string[] parts = argument.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length < 3)
    {
        Console.WriteLine("Usage: send <host> <port> <message>");
        return;
    }

    udpClient!.Send(parts[0], parts[1], parts[2]);
}
