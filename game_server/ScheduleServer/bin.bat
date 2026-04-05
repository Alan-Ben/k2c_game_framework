@echo off
setlocal enabledelayedexpansion

:: 查找当前目录下的 .jar 文件
for %%F in (*.jar) do (
    set "jarfile=%%F"
    goto :found
)

:found
if not defined jarfile (
    echo ERROR: jar file not found
    pause
)

:: 去掉 .jar 扩展名
set "jarname=%jarfile:.jar=%"

:: 设置命令提示符窗口的标题为 .jar 文件名（不含扩展名）
title %jarname%

:: 设置日期和时间变量
for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /value') do set datetime=%%I
set "YYYY=%datetime:~0,4%"
set "MM=%datetime:~4,2%"
set "DD=%datetime:~6,2%"
set "HH=%datetime:~8,2%"
set "Min=%datetime:~10,2%"
set "Sec=%datetime:~12,2%"

:: 创建日志目录（如果不存在）
if not exist "..\log\%YYYY%%MM%%DD%" mkdir "..\log\%YYYY%%MM%%DD%"

:: 运行找到的 jar 文件并重定向输出
echo Run %jarfile%
java -jar -Dfile.encoding=UTF-8 "%jarfile%" GOD_VERSION > "..\log\%YYYY%%MM%%DD%\%jarname%_log_%YYYY%%MM%%DD%%HH%%Min%%Sec%.out" 2>&1

endlocal

pause