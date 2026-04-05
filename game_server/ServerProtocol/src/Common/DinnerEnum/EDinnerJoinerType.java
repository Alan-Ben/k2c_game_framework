package Common.DinnerEnum;

/*********
 * 赴宴对象类型
 **/
public enum EDinnerJoinerType {
	NONE, //0 ==== 
	PLAYER, //1 ==== 玩家
	HERO, //2 ==== 大臣
	;
public static final EDinnerJoinerType[]  EDinnerJoinerType_Values = EDinnerJoinerType.values();
public static final int EDinnerJoinerType_Length = EDinnerJoinerType_Values.length;
public static EDinnerJoinerType EDinnerJoinerType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EDinnerJoinerType_Length){ return null; }
	return EDinnerJoinerType_Values[_ivalue];
}
}

