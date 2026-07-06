# TF6310 PLC TCP Sample 05: Multi Client and Server (Binary)

## Overview
This sample demonstrates binary TCP communication with multiple concurrent PLC client instances.

## Contents
| Item | Description |
| --- | --- |
| `TcSocketHelper_MultiClientBinary/TcSocketHelper_MultiClientBinary.sln` | Solution for the multi binary client. |
| `TcSocketHelper_MultiServerBinary/TcSocketHelper_MultiServerBinary.sln` | Solution for the multi binary server. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- TwinCAT 3 target runtime (local or remote)
- TF6310 TCP/IP runtime support available on the target

## Quick start
1. Open both multi binary client and server solutions in TwinCAT XAE.
2. Build and activate both projects.
3. Start the server project on port `200`.
4. Enable multiple client connections in the client project.
5. Monitor parallel binary data exchange and logs.

## What this sample does
- Demonstrates handling of multiple binary TCP client streams
- Shows multi-connection architecture in PLC code
- Reuses helper blocks for robust connection and frame handling

Detailed data exchange behavior:

- Five client instances (`fbClient1` ... `fbClient5`) are available in the client `MAIN`; each is enabled by `bEnable1` ... `bEnable5`.
- Five server instances (`fbServer1` ... `fbServer5`) are available in the server `MAIN`; all share the server handle `hServer`.
- In each client `FB_ClientApplication`, binary transmit payload is built in `stToServer` and received payload is stored in `stFromServer`.
- In each server `FB_ServerApplication`, incoming payload is read into `stFromClient`, copied/adjusted into `stToClient`, and sent back to the corresponding client connection.

Variables to monitor for send/receive values:

- Client 1 send and receive: `fbClient1.fbApplication.stToServer`, `fbClient1.fbApplication.stFromServer`
- Server 1 receive and reply: `fbServer1.fbApplication.stFromClient`, `fbServer1.fbApplication.stToClient`
- Multi-connection control and state: `bEnable1` ... `bEnable5`, `fbClient1.eState`, `fbServer1.eState`

The same variable pattern applies to instances 2 to 5.

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
