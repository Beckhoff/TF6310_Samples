#!/bin/sh
# SPDX-License-Identifier: Zero-Clause BSD

# Exit immediately if a command exits with a non-zero status
set -e

# Indicate the script's start for logging purposes
echo "Starting TcSystemServiceUm..."

# Replaces the shell process with the TcSystemServiceUm process, ensuring proper signal handling
exec /usr/bin/TcSystemServiceUm -f 0x7 -i "${AMS_NETID}" -p /var/run/TcSystemServiceUm.pid
