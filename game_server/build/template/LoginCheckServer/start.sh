#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./LoginCheckServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/login_check_log_$(date +%Y%m%d%H%M%S).out &