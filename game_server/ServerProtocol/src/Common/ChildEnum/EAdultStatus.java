package Common.ChildEnum;

/*********
 * 子嗣状态
 **/
public enum EAdultStatus {
	NONE, //0 ==== 
	APPLY_PLAYER, //1 ==== 指定联姻请求中
	APPLY_SERVER, //2 ==== 全服联姻请求中
	MARRIED, //3 ==== 已婚
	;
public static final EAdultStatus[]  EAdultStatus_Values = EAdultStatus.values();
public static final int EAdultStatus_Length = EAdultStatus_Values.length;
public static EAdultStatus EAdultStatus_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EAdultStatus_Length){ return null; }
	return EAdultStatus_Values[_ivalue];
}
}

