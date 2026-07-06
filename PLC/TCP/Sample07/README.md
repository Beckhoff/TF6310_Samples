# TF6310 PLC TCP Sample 07: Client and Server with TLS PSK

## Overview
This sample demonstrates secured TCP communication in TwinCAT 3 using TLS with a pre-shared key (PSK).

## Contents
| Item | Description |
| --- | --- |
| `TcpIp_CLIENT_with_TLS_PSK/TcpIp_CLIENT_with_TLS_PSK.sln` | Solution for the TLS PSK client project. |
| `TcpIp_SERVER_with_TLS_PSK/TcpIp_SERVER_with_TLS_PSK.sln` | Solution for the TLS PSK server project. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- TwinCAT 3 target runtime (local or remote)
- TF6310 TCP/IP runtime support available on the target
- Matching PSK and identity configured on both client and server projects

## Quick start
1. Open both TLS PSK client and server solutions in TwinCAT XAE.
2. Configure host, port, PSK key, and identity in each `MAIN` program.
3. Build and activate both projects.
4. Start the TLS PSK server and then the client.
5. Verify secured data exchange using PSK authentication.

## What this sample does
- Uses TLS socket function blocks with PSK configuration
- Demonstrates secure client/server setup without certificate provisioning
- Shows where to configure PSK key material and identity in PLC code

Detailed data exchange behavior:

- The client side uses up to five instances (`fbClient1` ... `fbClient5`) enabled by `bEnable1` ... `bEnable5` in the client `MAIN`.
- The server side uses `fbServer` in the server `MAIN` with listener status `bListening` and accepted-client count `nAccepted`.
- PSK-based TLS is configured through `fbTls` with switch `bPSK`:
	- Client `MAIN`: `fbTls.AddPsk(key:=key, sIdentity:='MyIdentity')`
	- Server `MAIN`: `fbTls.AddPsk(key:=key, sIdentity:='MyIdentity')`
- Application payload exchange is string-based and equivalent to the non-TLS samples:
	- Client `FB_ClientApplication`: send value `sToServer`, receive value `sFromServer`
	- Server `FB_ServerApplication`: receive value `sFromClient`, echoed send value `sToClient`

Variables to monitor for send/receive values:

- Client payload and state: `fbClient1.fbApplication.sToServer`, `fbClient1.fbApplication.sFromServer`, `fbClient1.bConnected`
- Server payload and state: `fbServer.aApplications[1].sFromClient`, `fbServer.aApplications[1].sToClient`, `fbServer.bListening`, `fbServer.nAccepted`
- PSK configuration values: `bPSK`, `key`, `fbTls.sIdentity`, `fbTls.pskKeyLen`

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
