package Hotfix.V01.Enum.TileMatchEnum;

/*********
 * 三消模式类型
 **/
public enum ETileMatch_ModeType {
	NONE, //0 ==== 
	NORMAL, //1 ==== 普通
	ADVANCED, //2 ==== 高级
	EXTREME, //3 ==== 极限
	;
public static final ETileMatch_ModeType[]  ETileMatch_ModeType_Values = ETileMatch_ModeType.values();
public static final int ETileMatch_ModeType_Length = ETileMatch_ModeType_Values.length;
public static ETileMatch_ModeType ETileMatch_ModeType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETileMatch_ModeType_Length){ return null; }
	return ETileMatch_ModeType_Values[_ivalue];
}
}

