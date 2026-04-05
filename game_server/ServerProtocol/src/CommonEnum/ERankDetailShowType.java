package CommonEnum;

/*********
 * 排行榜详细信息展示类型
 **/
public enum ERankDetailShowType {
	NONE, //0 ==== 
	PLAYER, //1 ==== 玩家
	HERO, //2 ==== 大臣
	;
public static final ERankDetailShowType[]  ERankDetailShowType_Values = ERankDetailShowType.values();
public static final int ERankDetailShowType_Length = ERankDetailShowType_Values.length;
public static ERankDetailShowType ERankDetailShowType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERankDetailShowType_Length){ return null; }
	return ERankDetailShowType_Values[_ivalue];
}
}

