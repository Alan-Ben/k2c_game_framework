package Common.MarsEnum;

/*********
 * 火星居民帮助类型
 **/
public enum EMarsPeopleHelpType {
	NONE, //0 ==== 
	REWARD, //1 ==== 直接奖励
	CHOICE, //2 ==== 需要选择回答
	;
public static final EMarsPeopleHelpType[]  EMarsPeopleHelpType_Values = EMarsPeopleHelpType.values();
public static final int EMarsPeopleHelpType_Length = EMarsPeopleHelpType_Values.length;
public static EMarsPeopleHelpType EMarsPeopleHelpType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsPeopleHelpType_Length){ return null; }
	return EMarsPeopleHelpType_Values[_ivalue];
}
}

