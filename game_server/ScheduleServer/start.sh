#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./ScheduleServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/schedule_log_$(date +%Y%m%d%H%M%S).out &