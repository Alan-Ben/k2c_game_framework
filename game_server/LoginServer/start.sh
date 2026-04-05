#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./LoginServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/login_log_$(date +%Y%m%d%H%M%S).out &