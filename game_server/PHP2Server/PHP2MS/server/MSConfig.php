<?php

/********************

public enum EServerType
    {
        NONE,           //0无，不使用本枚举
        SINGLE,         //1独立服务器（全服务器架构只存在一个的特殊类型服务器）
        LOGIN,          //2登录服
        USER,           //3用户服
        GATE,           //4网关
        ROOM,           //5房间
        CROSS_GAME,    //6跨服游戏服务器
    }

public enum ENPSingleServerType
    {
        NONE,
        LOGIN_CHECK,         //1登录检测的处理服务器
        COMMON,              //2通用服务器，一般用于匹配处理等通用操作
        HTTP,                //3HTTP服务器，一般用于与PHP对接操作
        INTERFACE,           //4接口服务器
        RECORD,              //5记录服务器
	}

 ********************/
function makeServerKey($t, $tid)
{
	if(9999 == $t || 10000 == $t)
		return -$t;
	else if(1 == $t)
		return $t * 10000 + $tid;
	else
		return $t;
}
$serverConfig = [
	10001 => 'LoginCheck',
	10002 => 'Common',
	10003 => 'Http',
	10004 => 'Interface',
	10005 => 'Record',
	
	2 => 'Login',
	3 => 'User',
	4 => 'Gateway',
	5 => 'Room',
	6 => 'CrossGame',
	
	-9999  => 'Bus',
	-10000 => 'Monitor',
];

$config = [
	'host'        => '127.0.0.1',
	'port'        => 17202,
	'client'      => 1,
	'socket_read' => 128, //每次从socket中读取长度       
	'user'        => 'MG-PHP',
	'password'    => 'MG-20190708',
	'key'         => 'MG-OL7NV8H1BS',
];