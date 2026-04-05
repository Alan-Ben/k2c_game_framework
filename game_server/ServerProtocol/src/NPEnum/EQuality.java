package NPEnum;

/*********
 * 通用品质枚举
 **/
public enum EQuality {
	NONE, //0 ==== 
	WHITE, //1 ==== 白
	GREEN, //2 ==== 绿
	BLUE, //3 ==== 蓝
	PURPLE, //4 ==== 紫
	ORANGE, //5 ==== 橙
	ORANGE1, //6 ==== 橙1
	RED, //7 ==== 红
	;
public static final EQuality[]  EQuality_Values = EQuality.values();
public static final int EQuality_Length = EQuality_Values.length;
public static EQuality EQuality_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EQuality_Length){ return null; }
	return EQuality_Values[_ivalue];
}
}

