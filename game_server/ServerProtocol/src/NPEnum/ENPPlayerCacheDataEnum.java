package NPEnum;

/*********
 * 玩家缓存数据类型
 **/
public enum ENPPlayerCacheDataEnum {
	NAME, //0 ==== 玩家昵称
	LVL, //1 ==== 玩家等级
	;
public static final ENPPlayerCacheDataEnum[]  ENPPlayerCacheDataEnum_Values = ENPPlayerCacheDataEnum.values();
public static final int ENPPlayerCacheDataEnum_Length = ENPPlayerCacheDataEnum_Values.length;
public static ENPPlayerCacheDataEnum ENPPlayerCacheDataEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerCacheDataEnum_Length){ return null; }
	return ENPPlayerCacheDataEnum_Values[_ivalue];
}
}

