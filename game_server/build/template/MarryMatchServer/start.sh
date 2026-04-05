#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./MarryMatchServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/marry_match_log_$(date +%Y%m%d%H%M%S).out &