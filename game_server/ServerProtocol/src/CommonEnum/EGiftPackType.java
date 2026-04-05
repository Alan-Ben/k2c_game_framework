package CommonEnum;

/*********
 * 礼包类型
 **/
public enum EGiftPackType {
	NONE, //0 ==== 
	PUSH_GIFT, //1 ==== 推送礼包
	FIRST_RECHARGE, //2 ==== 首充礼包
	;
public static final EGiftPackType[]  EGiftPackType_Values = EGiftPackType.values();
public static final int EGiftPackType_Length = EGiftPackType_Values.length;
public static EGiftPackType EGiftPackType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGiftPackType_Length){ return null; }
	return EGiftPackType_Values[_ivalue];
}
}

