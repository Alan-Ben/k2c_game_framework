#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./DynamicSpaceHandleServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/dynamic_space_handler_log_$(date +%Y%m%d%H%M%S).out &