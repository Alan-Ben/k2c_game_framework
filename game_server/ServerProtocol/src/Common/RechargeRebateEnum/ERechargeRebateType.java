package Common.RechargeRebateEnum;

/*********
 * 充值返利类型枚举
 **/
public enum ERechargeRebateType {
	DAILY, //0 ==== 每日充值
	TOTAL, //1 ==== 累计充值
	DAYS, //2 ==== 累天充值
	;
public static final ERechargeRebateType[]  ERechargeRebateType_Values = ERechargeRebateType.values();
public static final int ERechargeRebateType_Length = ERechargeRebateType_Values.length;
public static ERechargeRebateType ERechargeRebateType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERechargeRebateType_Length){ return null; }
	return ERechargeRebateType_Values[_ivalue];
}
}

