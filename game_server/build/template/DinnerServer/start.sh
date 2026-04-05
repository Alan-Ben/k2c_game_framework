#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./DinnerServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/dinner_log_$(date +%Y%m%d%H%M%S).out &