#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./PlatServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/plat_log_$(date +%Y%m%d%H%M%S).out &