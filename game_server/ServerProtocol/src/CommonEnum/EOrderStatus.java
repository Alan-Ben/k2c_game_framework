package CommonEnum;

/*********
 * 订单状态
 **/
public enum EOrderStatus {
	WAIT_PAY, //0 ==== 待支付
	PAY_SUCCESS, //1 ==== 支付成功
	DELIVERY_COMPLETED, //2 ==== 支付成功发货完成
	DELIVERY_FAILED, //3 ==== 支付成功发货失败
	PAY_CANCELED, //4 ==== 支付取消
	;
public static final EOrderStatus[]  EOrderStatus_Values = EOrderStatus.values();
public static final int EOrderStatus_Length = EOrderStatus_Values.length;
public static EOrderStatus EOrderStatus_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EOrderStatus_Length){ return null; }
	return EOrderStatus_Values[_ivalue];
}
}

