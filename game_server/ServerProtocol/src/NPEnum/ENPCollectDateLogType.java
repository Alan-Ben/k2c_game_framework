package NPEnum;

/*********
 * 日志收集数据类型
 **/
public enum ENPCollectDateLogType {
	NONE, //0 ==== 
	LOGIN, //1 ==== 登录日志
	CREATE, //2 ==== 创角日志
	ONLINE, //3 ==== 在线日志
	RECHARGE, //4 ==== 充值日志
	ITEM, //5 ==== 物品日志
	ADMOB, //6 ==== 广告日志
	;
public static final ENPCollectDateLogType[]  ENPCollectDateLogType_Values = ENPCollectDateLogType.values();
public static final int ENPCollectDateLogType_Length = ENPCollectDateLogType_Values.length;
public static ENPCollectDateLogType ENPCollectDateLogType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCollectDateLogType_Length){ return null; }
	return ENPCollectDateLogType_Values[_ivalue];
}
}

