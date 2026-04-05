#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./CommonServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/common_log_$(date +%Y%m%d%H%M%S).out &