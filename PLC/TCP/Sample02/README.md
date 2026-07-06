# TF6310 PLC TCP Sample 02: Single Client and Server (String)

## Overview
This sample demonstrates string-based TCP communication using the TcSocketHelper pattern with one client and one server.

## Contents
| Item | Description |
| --- | --- |
| `TcSocketHelper_SingleClientString/TcSocketHelper_SingleClientString.sln` | Solution for the single string client. |
| `TcSocketHelper_SingleServerString/TcSocketHelper_SingleServerString.sln` | Solution for the single string server. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- TwinCAT 3 target runtime (local or remote)
- TF6310 TCP/IP runtime support available on the target

## Quick start
1. Open both single client and single server solutions in TwinCAT XAE.
2. Build and activate each configuration.
3. Start the server project on port `200`.
4. Start the client project and connect to the configured server endpoint.
5. Observe exchanged string payloads and diagnostics.

## What this sample does
- Demonstrates string payload transfer over TCP
- Uses one PLC client connection and one PLC server endpoint
- Provides a simple starting point for request/response style messaging

Detailed data exchange behavior:

- The client is started in `TcSocketHelper_SingleClientString/POUs/MAIN.TcPOU` via `fbClient` (`bEnable := TRUE`).
- The server is started in `TcSocketHelper_SingleServerString/POUs/MAIN.TcPOU` via `fbServer` and a server handle `hServer` on port `200`.
- In `FB_ClientApplication`, outgoing text is generated in `sToServer` (using `nCounter`) and queued via `fbTx.AddTail(...)`.
- In `FB_ServerApplication`, received text is read into `sFromClient`, copied to `sToClient` (echo), and queued for transmit via `fbTx.AddTail(...)`.
- Back on the client side, received text is read into `sFromServer`.

Variables to monitor for send/receive values:

- Client send and receive: `fbClient.fbApplication.sToServer`, `fbClient.fbApplication.sFromServer`
- Server receive and echo: `fbServer.fbApplication.sFromClient`, `fbServer.fbApplication.sToClient`
- Message counters and state: `fbClient.fbApplication.nCounter`, `fbServer.fbApplication.nCounter`, `fbClient.eState`, `fbServer.eState`

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
