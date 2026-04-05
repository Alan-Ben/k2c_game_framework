package NPEnum;

/*********
 * 白名单操作类型
 **/
public enum EWhiteAccOpType {
	ADD, //0 ==== 添加账号
	REMOVE, //1 ==== 删除账号
	;
public static final EWhiteAccOpType[]  EWhiteAccOpType_Values = EWhiteAccOpType.values();
public static final int EWhiteAccOpType_Length = EWhiteAccOpType_Values.length;
public static EWhiteAccOpType EWhiteAccOpType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EWhiteAccOpType_Length){ return null; }
	return EWhiteAccOpType_Values[_ivalue];
}
}

