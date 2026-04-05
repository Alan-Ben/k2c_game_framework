package Common.MarsEnum;

/*********
 * 火星居民指数类型
 **/
public enum EMarsPeopleValueType {
	NONE, //0 ==== 
	HEALTH, //1 ==== 健康指数
	HAPPY, //2 ==== 幸福指数
	;
public static final EMarsPeopleValueType[]  EMarsPeopleValueType_Values = EMarsPeopleValueType.values();
public static final int EMarsPeopleValueType_Length = EMarsPeopleValueType_Values.length;
public static EMarsPeopleValueType EMarsPeopleValueType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsPeopleValueType_Length){ return null; }
	return EMarsPeopleValueType_Values[_ivalue];
}
}

