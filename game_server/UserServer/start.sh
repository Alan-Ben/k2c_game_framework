#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./UserServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/user_log_$(date +%Y%m%d%H%M%S).out &