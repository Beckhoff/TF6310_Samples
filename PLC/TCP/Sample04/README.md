# TF6310 PLC TCP Sample 04: Single Client and Server (Binary)

## Overview
This sample demonstrates binary TCP communication using the TcSocketHelper pattern with one client and one server.

## Contents
| Item | Description |
| --- | --- |
| `TcSocketHelper_SingleClientBinary/TcSocketHelper_SingleClientBinary.sln` | Solution for the single binary client. |
| `TcSocketHelper_SingleServerBinary/TcSocketHelper_SingleServerBinary.sln` | Solution for the single binary server. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- TwinCAT 3 target runtime (local or remote)
- TF6310 TCP/IP runtime support available on the target

## Quick start
1. Open both single binary client and server solutions in TwinCAT XAE.
2. Build and activate both projects.
3. Start the server project on port `200`.
4. Start the client project and connect to the server.
5. Observe binary payload transfer and diagnostics.

## What this sample does
- Demonstrates binary payload exchange over TCP
- Uses one PLC client connection and one PLC server endpoint
- Provides a basis for structured/binary protocol implementation

Detailed data exchange behavior:

- The client is started in the client `MAIN` with `fbClient` (`bEnable := TRUE`), the server in the server `MAIN` with `fbServer` and `hServer` on port `200`.
- In `FB_ClientApplication`, the binary payload is prepared in `stToServer` (for example `dtTimeStamp`, `fPos`, `nVelo`, `sMsg`, `arr[0]`) and queued via `fbTx.AddTail(stPut := stToServer)`.
- In `FB_ServerApplication`, received binary data is read into `stFromClient`, copied to `stToClient`, modified (`stToClient.sMsg := 'Hello world from server!'`), and queued for transmission.
- The client reads returned binary data into `stFromServer`.

Variables to monitor for send/receive values:

- Client send and receive: `fbClient.fbApplication.stToServer`, `fbClient.fbApplication.stFromServer`
- Server receive and reply: `fbServer.fbApplication.stFromClient`, `fbServer.fbApplication.stToClient`
- Connection state: `fbClient.eState`, `fbServer.eState`

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
