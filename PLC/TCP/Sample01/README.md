# TF6310 PLC TCP Sample 01: Basic Client and Server

## Overview
This sample provides baseline TCP client and TCP server implementations for TwinCAT 3 PLC projects.

## Contents
| Item | Description |
| --- | --- |
| `TcpIp_CLIENT/TcpIp_CLIENT.sln` | Solution for the TCP client project. |
| `TcpIp_SERVER/TcpIp_SERVER.sln` | Solution for the TCP server project. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- TwinCAT 3 target runtime (local or remote)
- TF6310 TCP/IP runtime support available on the target

## Quick start
1. Open and configure both client and server solutions in TwinCAT XAE.
2. Build and activate both projects.
3. Start the server on port `200`.
4. Start one or more client instances and connect to the server.
5. Verify data exchange and log output.

## What this sample does
- Shows TCP connection setup and teardown in PLC code
- Demonstrates single-server and multi-client operation
- Uses reusable helper function blocks for framing and logging

Detailed data exchange behavior:

- The server is started with `fbServer` in `TcpIp_SERVER/POUs/MAIN.TcPOU` (`nLocalPort := 200`, `bEnable := TRUE`).
- Up to five client instances are available in `TcpIp_CLIENT/POUs/MAIN.TcPOU` (`fbClient1` ... `fbClient5`). Communication is enabled per client by `bEnable1` ... `bEnable5`.
- In the client sample application (`FB_ClientApplication`), the send value is built in `sToServer` (for example `Client message: <counter>`) and queued into the transmit FIFO via `fbTx.AddTail(...)`.
- On the server side (`FB_ServerApplication`), received values are read from `fbRx` into `sFromClient`, copied to `sToClient` (echo behavior), and queued for transmission with `fbTx.AddTail(...)`.
- Back on the client side, received values are read from `fbRx` into `sFromServer`.

Variables to monitor for send/receive values:

- Client payload generation and receive:
	- `fbClient1.fbApplication.sToServer` (client send value)
	- `fbClient1.fbApplication.sFromServer` (client receive value)
	- `fbClient1.fbApplication.nCounter` (message counter)
- Server receive and echo:
	- `fbServer.aApplications[1].sFromClient` (server receive value)
	- `fbServer.aApplications[1].sToClient` (server send/echo value)
	- `fbServer.aApplications[1].nCounter` (received-message counter)
- Connection and session status:
	- `fbClient1.bConnected` (client connected state)
	- `fbServer.bListening` (server listener socket state)
	- `fbServer.nAccepted` (number of connected clients)

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
