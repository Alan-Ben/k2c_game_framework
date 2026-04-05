package CommonEnum;

/*********
 * 子嗣性别
 **/
public enum EChildSexType {
	NONE, //0 ==== 
	BOY, //1 ==== 
	GIRL, //2 ==== 
	;
public static final EChildSexType[]  EChildSexType_Values = EChildSexType.values();
public static final int EChildSexType_Length = EChildSexType_Values.length;
public static EChildSexType EChildSexType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EChildSexType_Length){ return null; }
	return EChildSexType_Values[_ivalue];
}
}

