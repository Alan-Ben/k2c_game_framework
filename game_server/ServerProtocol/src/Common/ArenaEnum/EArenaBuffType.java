package Common.ArenaEnum;

/*********
 * 竞技场buff类型
 **/
public enum EArenaBuffType {
	NONE, //0 ==== 
	CRYSTAL, //1 ==== 水晶增益
	TWO_COIN, //2 ==== 2硬币增益
	ONE_COIN, //3 ==== 1硬币增益
	;
public static final EArenaBuffType[]  EArenaBuffType_Values = EArenaBuffType.values();
public static final int EArenaBuffType_Length = EArenaBuffType_Values.length;
public static EArenaBuffType EArenaBuffType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EArenaBuffType_Length){ return null; }
	return EArenaBuffType_Values[_ivalue];
}
}

