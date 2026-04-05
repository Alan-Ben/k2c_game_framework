package CommonEnum;

/*********
 * 订单支付方式类型
 **/
public enum EOrderPayType {
	NONE, //0 ==== 无
	PLATFORM, //1 ==== 平台支付
	VOUCHER, //2 ==== 代金券支付
	GM, //3 ==== GM支付
	;
public static final EOrderPayType[]  EOrderPayType_Values = EOrderPayType.values();
public static final int EOrderPayType_Length = EOrderPayType_Values.length;
public static EOrderPayType EOrderPayType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EOrderPayType_Length){ return null; }
	return EOrderPayType_Values[_ivalue];
}
}

