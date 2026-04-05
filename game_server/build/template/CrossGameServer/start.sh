#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./CrossGameServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/cross_game_log_$(date +%Y%m%d%H%M%S).out &