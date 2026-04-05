package NPEnum;

/*********
 * 排行榜实例服务器类型
 **/
public enum ERankingInstanceServerType {
	NONE, //0 ==== 无
	USER, //1 ==== 玩家服务器
	;
public static final ERankingInstanceServerType[]  ERankingInstanceServerType_Values = ERankingInstanceServerType.values();
public static final int ERankingInstanceServerType_Length = ERankingInstanceServerType_Values.length;
public static ERankingInstanceServerType ERankingInstanceServerType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERankingInstanceServerType_Length){ return null; }
	return ERankingInstanceServerType_Values[_ivalue];
}
}

