<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="zh">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>NP - Monitor</title>
</head>

<script src="js/jquery.js"></script>
<script>
function openCustom()
{
	window.open('./server/PHP2MS_000_001_ReqGetServerList_Custom.php?ip=' + $("#custom_ip").val() + '&port=' + $('#custom_port').val(), '_blank');
}
</script>

<body>

<?php include 'area.php';?>

<?php foreach($areaConfig as $k => $v) { ?>
<br /><br />
<span>
&nbsp;&nbsp;&nbsp;&nbsp;
<a href="./server/PHP2MS_000_001_ReqGetServerList.php?id=<?php echo $k; ?>"><?php echo $v[2]; ?></a>
&nbsp;&nbsp;&nbsp;&nbsp;
<span><?php echo $v[0]; ?></span>
&nbsp;:&nbsp;
<span><?php echo $v[1]; ?></span>
</span>
<?php } ?>

<br /><br />
<span>
&nbsp;&nbsp;&nbsp;&nbsp;
<a href="javascript:void(0);" onClick="openCustom();">自选服务器</a> 
&nbsp;&nbsp;&nbsp;&nbsp;
<input type="text" id="custom_ip" name="custom_ip" value="" />
&nbsp;:&nbsp;
<input type="text" id="custom_port" name="custom_port" value="" />
</span>

</body>
</html>