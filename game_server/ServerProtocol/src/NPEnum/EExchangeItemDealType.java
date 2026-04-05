package NPEnum;

/*********
 * 物品转换生效时机
 **/
public enum EExchangeItemDealType {
	NONE, //0 ==== 
	GAIN_ITEM_TIME, //1 ==== 获得物品时
	MUSEUM_ITEM_STAR_MAX, //2 ==== 博物馆藏品满星
	GAIN_ACTIVE_ITEM, //3 ==== 获得活跃度时
	;
public static final EExchangeItemDealType[]  EExchangeItemDealType_Values = EExchangeItemDealType.values();
public static final int EExchangeItemDealType_Length = EExchangeItemDealType_Values.length;
public static EExchangeItemDealType EExchangeItemDealType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EExchangeItemDealType_Length){ return null; }
	return EExchangeItemDealType_Values[_ivalue];
}
}

