#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./InterfaceServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/interface_log_$(date +%Y%m%d%H%M%S).out &