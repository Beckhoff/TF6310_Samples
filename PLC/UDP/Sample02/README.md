# TF6310 PLC UDP Sample 02: Multicast Devices

## Overview
This sample demonstrates UDP multicast communication with two TwinCAT PLC projects (`UdpMulticastDevice1` and `UdpMulticastDevice2`).

## Contents
| Item | Description |
| --- | --- |
| `UdpMulticastDevice1/UdpMulticastDevice1.sln` | Solution for multicast device 1. |
| `UdpMulticastDevice2/UdpMulticastDevice2.sln` | Solution for multicast device 2. |

## Prerequisites
- TwinCAT 3 XAE installed on Windows
- One or two TwinCAT target runtimes with multicast-capable network path
- TF6310 TCP/IP runtime support available on the target(s)

## Quick start
1. Open both multicast device solutions in TwinCAT XAE.
2. Verify local port, remote port, and multicast group settings in each `MAIN` program.
3. Build and activate both projects.
4. Start both PLC runtimes.
5. Observe cyclic counter exchange over the multicast address.

## What this sample does
- Creates UDP sockets and joins/leaves a multicast group
- Sends and receives cyclic counter payloads via multicast
- Demonstrates multicast lifecycle handling in PLC code

Detailed data exchange behavior:

- Both device projects use the same multicast group `sMulticastAddr` and opposite local/remote ports (`nLocalPort`, `nRemotePort`).
- After socket creation (`bUdpCreated`) and multicast join (`bMulticastAdded`), each runtime cyclically increments `nCounterOut` and sends it via `fbSocketSend` to `sMulticastAddr`.
- Incoming multicast payload is received via `fbSocketReceive` and written to `nCounterIn`.
- Runtime state machine handling is visible through `eStep`, including create, join, send/receive, drop multicast, and close states.

Variables to monitor for send/receive values:

- Send value and destination: `nCounterOut`, `nRemotePort`, `sMulticastAddr`
- Receive value: `nCounterIn`
- Multicast/socket state: `bUdpCreated`, `bMulticastAdded`, `eStep`

## Support
Should you have any questions regarding this sample, please contact your local Beckhoff support team. Contact information can be found on the official Beckhoff website at https://www.beckhoff.com/contact/.
