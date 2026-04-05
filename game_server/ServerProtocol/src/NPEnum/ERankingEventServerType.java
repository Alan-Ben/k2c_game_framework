package NPEnum;

/*********
 * 排行事件服务器类型
 **/
public enum ERankingEventServerType {
	NONE, //0 ==== 无
	USER, //1 ==== 玩家服务器
	;
public static final ERankingEventServerType[]  ERankingEventServerType_Values = ERankingEventServerType.values();
public static final int ERankingEventServerType_Length = ERankingEventServerType_Values.length;
public static ERankingEventServerType ERankingEventServerType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERankingEventServerType_Length){ return null; }
	return ERankingEventServerType_Values[_ivalue];
}
}

