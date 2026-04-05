<?php

include 'library/MGHS_SOCKET.php';
include 'MSConfig.php';
include '../area.php';

header('Expires: Mon, 29 Jan 2007 08:56:01 GMT');

if(empty($_GET['id']) || !is_numeric($_GET['id']) || !isset($areaConfig[$_GET['id']]))
{
	echo 'error, not find config';
	exit;
}

$curArea = $areaConfig[$_GET['id']];

$config['host'] = $curArea[0];
$config['port'] = $curArea[1];

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
	echo '<a href="PHP2MS_000_002_ReqGetServerInfo.php?id=' . $_GET['id'] . '&t=' . $v[t] . '&tid=' . $v['tid'] . '"><p>', $serverConfig[makeServerKey($v['t'], $v['tid'])], '-', $v['tid'], '</p></a>';
}
