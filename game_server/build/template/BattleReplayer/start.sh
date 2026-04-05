#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./BattleReplayer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/battle_replayer_log_$(date +%Y%m%d%H%M%S).out &