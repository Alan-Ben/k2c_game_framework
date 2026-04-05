@echo off
chcp 65001
cd /d %~dp0..\..\bat

rem 拷贝热更类到指定目录
call "%~dp0..\..\bat\cp_hotifx.bat"

rem 生成RPC类和处理类
call python build_rpc.py

rem 生成事件类
call python build_events.py

rem 生成枚举总文件
call python build_enum.py

rem 生成错误码
call python build_err.py

rem 处理配表文件热更方法
java -jar resetRefAutoDeal.jar ..\GameRes\src\NPGameRes\Refs
java -jar resetRefAutoDeal.jar ..\ActivitiesV01\src\ActivitiesV01\Refs
java -jar resetRefAutoDeal.jar ..\ActivitiesV02\src\ActivitiesV02\Refs
