# TF6310 PLC UDP Sample 01: Peer-to-Peer

## Overview
This sample demonstrates UDP peer-to-peer communication with two TwinCAT PLC projects (`PeerToPeerA` and `PeerToPeerB`).

## Contents
| Item | Description |
| --- | --- |
| `PeerToPeerA/PeerToPeerA.sln` | Solution for peer A. |
| `PeerToPeerB/PeerToPeerB.sln` | Solution for peer B. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- One or two TwinCAT target runtimes connected via network
- TF6310 TCP/IP runtime support available on the target(s)

## Quick start
1. Open both `PeerToPeerA` and `PeerToPeerB` solutions in TwinCAT XAE.
2. Configure local and remote host/port settings in each `MAIN` program.
3. Build and activate both projects.
4. Start both PLC runtimes.
5. Trigger send actions and verify receive behavior on both peers.

## What this sample does
- Demonstrates connectionless UDP send/receive using peer role projects
- Shows bidirectional communication with mirrored local/remote port settings
- Includes message queue and logging helper patterns for PLC diagnostics

Detailed data exchange behavior:

- Each peer project (`PeerToPeerA`, `PeerToPeerB`) runs one `fbPeerToPeer` instance with its own `nLocalPort` and opposite `nRemotePort`.
- Sending is triggered from `MAIN` by pulse variables:
	- `bSendOnceToRemote` creates `stSendTo` with `stSendTo.sMessage := 'Hello remote host!'`
	- `bSendOnceToItself` creates `stSendTo` with `stSendTo.sMessage := 'Hello itself!'`
- The prepared `stSendTo` frames are queued with `fbTx.AddTail(...)` and transmitted by `FB_PeerToPeer`.
- Received frames are collected in `stReceivedFrom` (including remote address/port and message text) and pushed to `fbRx`; `MAIN` reads them via `stReceivedFrom := fbRx.stGet`.

Variables to monitor for send/receive values:

- Send trigger and payload: `bSendOnceToRemote`, `bSendOnceToItself`, `stSendTo.sMessage`, `stSendTo.stRemoteAddr.sAddr`, `stSendTo.stRemoteAddr.nPort`
- Receive payload and source: `stReceivedFrom.sMessage`, `stReceivedFrom.stRemoteAddr.sAddr`, `stReceivedFrom.stRemoteAddr.nPort`
- Communication state: `fbPeerToPeer.bCreated`, `nLocalPort`, `nRemotePort`

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
