#!/bin/bash
cd `dirname $0`

cd ./BusServer
sh ./start.sh
cd ..

cd ./CommonServer
sh ./start.sh
cd ..

cd ./CrossGameServer
sh ./start.sh
cd ..

cd ./DinnerServer
sh ./start.sh
cd ..

cd ./GatewayServer
sh ./start.sh
cd ..

cd ./HttpServer
sh ./start.sh
cd ..

cd ./InterfaceServer
sh ./start.sh
cd ..

cd ./LoginCheckServer
sh ./start.sh
cd ..

cd ./LoginServer
sh ./start.sh
cd ..

cd ./MarryMatchServer
sh ./start.sh
cd ..

cd ./MonitorServer
sh ./start.sh
cd ..

cd ./PlatServer
sh ./start.sh
cd ..

cd ./RecordServer
sh ./start.sh
cd ..

cd ./RoomServer
sh ./start.sh
cd ..

cd ./UserServer
sh ./start.sh
cd ..

