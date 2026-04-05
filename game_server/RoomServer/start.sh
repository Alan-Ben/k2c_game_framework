#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./RoomServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/room_log_$(date +%Y%m%d%H%M%S).out &