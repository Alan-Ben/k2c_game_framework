package CommonEnum;

/*********
 * 特殊物品类型
 **/
public enum ESpecialItemType {
	NONE, //0 ==== 
	GOLD, //1 ==== 金币
	ARENA_STATION, //2 ==== 竞技场贸易站
	FARM_MULTIPLE, //3 ==== 农场暴击
	MARS_ENERGY, //4 ==== 火星系统-能量
	PAID_GEM, //5 ==== 付费钻石
	PAID_VOUCHER, //6 ==== 付费代金券
	;
public static final ESpecialItemType[]  ESpecialItemType_Values = ESpecialItemType.values();
public static final int ESpecialItemType_Length = ESpecialItemType_Values.length;
public static ESpecialItemType ESpecialItemType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ESpecialItemType_Length){ return null; }
	return ESpecialItemType_Values[_ivalue];
}
}

