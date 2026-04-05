#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./MonitorServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/monitor_log_$(date +%Y%m%d%H%M%S).out &