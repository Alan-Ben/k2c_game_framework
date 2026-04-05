package NPEnum;

/*********
 * 战斗表现类型
 **/
public enum ENPBattleShowType {
	NORMAL, //0 ==== 普通
	MISSION_BOOS, //1 ==== 关卡boss
	;
public static final ENPBattleShowType[]  ENPBattleShowType_Values = ENPBattleShowType.values();
public static final int ENPBattleShowType_Length = ENPBattleShowType_Values.length;
public static ENPBattleShowType ENPBattleShowType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPBattleShowType_Length){ return null; }
	return ENPBattleShowType_Values[_ivalue];
}
}

