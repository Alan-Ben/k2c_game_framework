#!/bin/bash
mkdir -p ../log/$(date +%Y%m%d)
nohup java -jar ./HttpServer.jar GOE_VERSION >> ../log/$(date +%Y%m%d)/http_log_$(date +%Y%m%d%H%M%S).out &