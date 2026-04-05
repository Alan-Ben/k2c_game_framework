package CommonEnum;

/*********
 * 礼包日志类型
 **/
public enum EGiftPackLogType {
	NONE, //0 ==== 
	GEM, //1 ==== 钻石充值
	VOUCHER, //2 ==== 代金券充值
	;
public static final EGiftPackLogType[]  EGiftPackLogType_Values = EGiftPackLogType.values();
public static final int EGiftPackLogType_Length = EGiftPackLogType_Values.length;
public static EGiftPackLogType EGiftPackLogType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGiftPackLogType_Length){ return null; }
	return EGiftPackLogType_Values[_ivalue];
}
}

