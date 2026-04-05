<?php

include 'library/MGHS_SOCKET.php';
include 'MSConfig.php';
include '../area.php';

header('Expires: Mon, 29 Jan 2007 08:56:01 GMT');

if(empty($_GET['t']) || !is_numeric($_GET['t']) || empty($_GET['tid']) || !is_numeric($_GET['tid']))
{
	echo 'error, error config param';
	exit;
}

$curArea = $areaConfig[$_GET['id']];

$config['host'] = $curArea[0];
$config['port'] = $curArea[1];

$data = pack('N', $_GET['t']).pack('N', $_GET['tid']).pack('n', 0).pack('a0', '');
$ret = MGHS_SOCKET::getInstance($config)->call(0, 2, $data);

if(empty($ret))
{
	echo 'error!';
	exit;
}

if(0 != $ret['code'] || empty($ret['data']))
{
	echo 'error! - errCode:', $ret['code'], ', errMsg:', $ret['msg'];
	exit;
}

echo '<p>', '<a href="PHP2MS_000_001_ReqGetServerList.php?id=' . $_GET['id'] . '">-> ServerList</a>', '</p>';
echo '<p>', $serverConfig[makeServerKey($ret['data']['type'], $ret['data']['type_id'])], '-' , $ret['data']['type_id'], '</p><hr/>';

$info = json_decode($ret['data']['info'], true);

$memoryKey = array('totalMemory', 'maxMemory', 'freeMemory');

foreach($info as $k => $v)
{
	if(is_array($v))
	{
		echo '<div style="background-color:#CCFFFF;"><p>', $k, ' => ', '</p>';
		
		foreach($v as $vk1 => $vv1)
		{
			if(is_array($vv1))
			{
				foreach($vv1 as $vk2 => $vv2)
				{
					echo '<p>', $vk2, ' => ', $vv2, '</p>';
				}
			}
			else
			{
				echo '<p>', $vk1, ' => ', $vv1, '</p>';
			}
		}

		echo '</div>';
	}
	else
	{
		//针对内存的处理
		if(in_array($k, $memoryKey))
		{

			$value = $v;
			$v =  $value . ' [' . number_format(round($value/1048576, 3), 2) . 'M]';
		}

		echo '<p style="background-color:#FFFF99;">', $k, ' => ', $v, '</p>';
	}
}