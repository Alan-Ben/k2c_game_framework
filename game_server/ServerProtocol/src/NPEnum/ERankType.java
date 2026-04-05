package NPEnum;

/*********
 * 排行榜类型
 **/
public enum ERankType {
	NONE, //0 ==== 
	PLAYER, //1 ==== 玩家排行榜
	GUILD, //2 ==== 联盟排行榜
	ACTIVITY_TEAM, //3 ==== 活动组队排行榜
	;
public static final ERankType[]  ERankType_Values = ERankType.values();
public static final int ERankType_Length = ERankType_Values.length;
public static ERankType ERankType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERankType_Length){ return null; }
	return ERankType_Values[_ivalue];
}
}

