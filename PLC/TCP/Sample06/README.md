# TF6310 PLC TCP Sample 06: Client and Server with TLS Certificates

## Overview
This sample demonstrates secured TCP communication in TwinCAT 3 using TLS with certificate-based configuration.

## Contents
| Item | Description |
| --- | --- |
| `TcpIp_CLIENT_with_TLS/TcpIp_CLIENT_with_TLS.sln` | Solution for the TLS client project. |
| `TcpIp_SERVER_with_TLS/TcpIp_SERVER_with_TLS.sln` | Solution for the TLS server project. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- TwinCAT 3 target runtime (local or remote)
- TF6310 TCP/IP runtime support available on the target
- Certificate files deployed on the target (CA, server/client certificate, key, optional CRL)

## Quick start
1. Open both TLS client and TLS server solutions in TwinCAT XAE.
2. Configure host, port, and certificate paths in each `MAIN` program.
3. Build and activate both projects.
4. Start the TLS server and then the TLS client.
5. Verify secure connection establishment and payload exchange.

## What this sample does
- Uses TLS socket function blocks for secure client/server communication
- Demonstrates certificate-based trust and endpoint configuration
- Shows where to configure CA, certificate/key, and optional CRL paths in PLC code

Detailed data exchange behavior:

- The client side uses up to five instances (`fbClient1` ... `fbClient5`) enabled by `bEnable1` ... `bEnable5` in the client `MAIN`.
- The server side uses `fbServer` in the server `MAIN` with listener status via `bListening` and accepted-client count via `nAccepted`.
- TLS certificate configuration is provided through `fbTls` in both projects:
	- Client setup in `MAIN`: `fbTls.AddCa(...)`, `fbTls.AddCert(...)`, `fbTls.AddCrl(...)`
	- Server setup in `MAIN`: `fbTls.AddCa(...)`, `fbTls.AddCert(...)`, `fbTls.AddCrl(...)`
- Application payload exchange remains string-based:
	- Client `FB_ClientApplication`: send value `sToServer`, receive value `sFromServer`
	- Server `FB_ServerApplication`: receive value `sFromClient`, echoed send value `sToClient`

Variables to monitor for send/receive values:

- Client payload and state: `fbClient1.fbApplication.sToServer`, `fbClient1.fbApplication.sFromServer`, `fbClient1.bConnected`
- Server payload and state: `fbServer.aApplications[1].sFromClient`, `fbServer.aApplications[1].sToClient`, `fbServer.bListening`, `fbServer.nAccepted`
- TLS configuration values: `fbTls.sCaPath`, `fbTls.sCertPath`, `fbTls.sKeyPath`, `fbTls.sCrlPath`

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
