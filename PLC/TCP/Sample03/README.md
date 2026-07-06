# TF6310 PLC TCP Sample 03: Multi Client and Server (String)

## Overview
This sample demonstrates string-based TCP communication with multiple concurrent client instances connecting to a server.

## Contents
| Item | Description |
| --- | --- |
| `TcSocketHelper_MultiClientString/TcSocketHelper_MultiClientString.sln` | Solution for the multi string client. |
| `TcSocketHelper_MultiServerString/TcSocketHelper_MultiServerString.sln` | Solution for the multi string server. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- TwinCAT 3 target runtime (local or remote)
- TF6310 TCP/IP runtime support available on the target

## Quick start
1. Open both multi client and multi server solutions in TwinCAT XAE.
2. Build and activate both projects.
3. Start the server project on port `200`.
4. Enable multiple client connections in the client project.
5. Monitor simultaneous string communication and log messages.

## What this sample does
- Demonstrates multiplexed TCP client connections
- Handles multiple string-based streams against one server
- Shows scalable connection handling using helper function blocks

Detailed data exchange behavior:

- Up to five client instances (`fbClient1` ... `fbClient5`) are triggered in the client `MAIN` and enabled individually with `bEnable1` ... `bEnable5`.
- Up to five server connection instances (`fbServer1` ... `fbServer5`) are triggered in the server `MAIN` and share one server handle `hServer`.
- In each client `FB_ClientApplication`, outgoing text is generated in `sToServer` and queued via `fbTx.AddTail(...)`; incoming text is read into `sFromServer`.
- In each server `FB_ServerApplication`, received text is read into `sFromClient`, copied to `sToClient`, and sent back (echo behavior).

Variables to monitor for send/receive values:

- Client 1 send and receive: `fbClient1.fbApplication.sToServer`, `fbClient1.fbApplication.sFromServer`
- Server 1 receive and echo: `fbServer1.fbApplication.sFromClient`, `fbServer1.fbApplication.sToClient`
- Scale-out control and state: `bEnable1` ... `bEnable5`, `fbClient1.eState`, `fbServer1.eState`

The same variable pattern applies to instances 2 to 5.

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
