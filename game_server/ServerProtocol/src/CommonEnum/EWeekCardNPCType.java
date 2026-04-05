package CommonEnum;

/*********
 * 周卡NPC类型
 **/
public enum EWeekCardNPCType {
	NONE, //0 ==== 
	HERO, //1 ==== 大臣
	CONSORT, //2 ==== 妃子
	;
public static final EWeekCardNPCType[]  EWeekCardNPCType_Values = EWeekCardNPCType.values();
public static final int EWeekCardNPCType_Length = EWeekCardNPCType_Values.length;
public static EWeekCardNPCType EWeekCardNPCType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EWeekCardNPCType_Length){ return null; }
	return EWeekCardNPCType_Values[_ivalue];
}
}

