package Common.ConsortEnum;

/*********
 * 家人来源类型
 **/
public enum EConsortSourceType {
	NONE, //0 ==== 
	TRAVEL, //1 ==== 游历获取
	CHAPTER, //2 ==== 关卡获取
	;
public static final EConsortSourceType[]  EConsortSourceType_Values = EConsortSourceType.values();
public static final int EConsortSourceType_Length = EConsortSourceType_Values.length;
public static EConsortSourceType EConsortSourceType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EConsortSourceType_Length){ return null; }
	return EConsortSourceType_Values[_ivalue];
}
}

