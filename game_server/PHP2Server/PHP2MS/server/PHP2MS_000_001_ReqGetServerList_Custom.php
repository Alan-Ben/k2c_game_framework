<?php

include 'library/MGHS_SOCKET.php';
include 'MSConfig.php';
include '../area.php';

header('Expires: Mon, 29 Jan 2007 08:56:01 GMT');

if(empty($_GET['ip']) || empty($_GET['port']))
{
	echo 'error, not find ip/config';
	exit;
}

$config['host'] = trim($_GET['ip']);
$config['port'] = trim($_GET['port']);

$ret = MGHS_SOCKET::getInstance($config)->call(0, 1);

if(empty($ret))
{
	echo 'error!';
	exit;
}

if(0 != $ret['code'])
{
	echo 'error! - errCode:', $ret['code'], ', errMsg:', $ret['msg'];
	exit;
}

asort($ret['data']['list']);

foreach($ret['data']['list'] as $k => $v)
{
	echo '<a href="PHP2MS_000_002_ReqGetServerInfo_Custom.php?ip=' . $config['host'] . '&port=' . $config[port] . '&t=' . $v[t] . '&tid=' . $v['tid'] . '"><p>', $serverConfig[makeServerKey($v['t'], $v['tid'])], '-', $v['tid'], '</p></a>';
}
