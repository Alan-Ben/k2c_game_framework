#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./RecordServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/record_log_$(date +%Y%m%d%H%M%S).out &