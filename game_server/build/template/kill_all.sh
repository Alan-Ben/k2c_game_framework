#!/bin/bash
ps -ef|grep GOE_VERSION|grep -v grep|cut -c 9-15|xargs kill -9 