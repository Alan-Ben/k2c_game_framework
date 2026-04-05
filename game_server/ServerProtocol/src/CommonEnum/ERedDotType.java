package CommonEnum;

/*********
 * 红点类型
 **/
public enum ERedDotType {
	NONE, //0 ==== 无
	;
public static final ERedDotType[]  ERedDotType_Values = ERedDotType.values();
public static final int ERedDotType_Length = ERedDotType_Values.length;
public static ERedDotType ERedDotType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERedDotType_Length){ return null; }
	return ERedDotType_Values[_ivalue];
}
}

