#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./BusServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/bus_log_$(date +%Y%m%d%H%M%S).out &