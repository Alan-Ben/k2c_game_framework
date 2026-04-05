package NPEnum;

/*********
 * 玩家称号类型
 **/
public enum ENPPlayerTitleType {
	NONE, //0 ==== 
	COMMON, //1 ==== 普通称号
	COMBO, //2 ==== 组合称号
	;
public static final ENPPlayerTitleType[]  ENPPlayerTitleType_Values = ENPPlayerTitleType.values();
public static final int ENPPlayerTitleType_Length = ENPPlayerTitleType_Values.length;
public static ENPPlayerTitleType ENPPlayerTitleType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerTitleType_Length){ return null; }
	return ENPPlayerTitleType_Values[_ivalue];
}
}

