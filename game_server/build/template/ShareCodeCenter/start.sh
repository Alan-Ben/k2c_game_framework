#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./ShareCodeCenter.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/sharecode_log_$(date +%Y%m%d%H%M%S).out &