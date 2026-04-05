package NPEnum;

/*********
 * 登录服务器的客户端连接类型
 **/
public enum ENPMonitorClientType {
	NONE, //0 ==== 
	PHP, //1 ==== 
	;
public static final ENPMonitorClientType[]  ENPMonitorClientType_Values = ENPMonitorClientType.values();
public static final int ENPMonitorClientType_Length = ENPMonitorClientType_Values.length;
public static ENPMonitorClientType ENPMonitorClientType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPMonitorClientType_Length){ return null; }
	return ENPMonitorClientType_Values[_ivalue];
}
}

