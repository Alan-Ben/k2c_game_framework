#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./GatewayServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/gate_log_$(date +%Y%m%d%H%M%S).out &