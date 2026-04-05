package Common.PrivilegeCardEnum;

/*********
 * 权益卡类型
 **/
public enum EPrivilegeCardType {
	NONE, //0 ==== 
	MONTH, //1 ==== 月卡
	YEAR, //2 ==== 年卡
	;
public static final EPrivilegeCardType[]  EPrivilegeCardType_Values = EPrivilegeCardType.values();
public static final int EPrivilegeCardType_Length = EPrivilegeCardType_Values.length;
public static EPrivilegeCardType EPrivilegeCardType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EPrivilegeCardType_Length){ return null; }
	return EPrivilegeCardType_Values[_ivalue];
}
}

