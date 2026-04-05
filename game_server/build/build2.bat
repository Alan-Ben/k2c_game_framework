del .\version.txt >nul
python ./build_version.py

call ./apache-ant-1.9.4/bin/ant.bat build -buildfile ./build2.xml
