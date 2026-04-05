package Common.CrossDataType;

/*********
 * 跨服数据类型枚举
 **/
public enum ECrossDataType {
	NONE, //0 ==== 
	MARS_MINE, //1 ==== 火星矿
	;
public static final ECrossDataType[]  ECrossDataType_Values = ECrossDataType.values();
public static final int ECrossDataType_Length = ECrossDataType_Values.length;
public static ECrossDataType ECrossDataType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ECrossDataType_Length){ return null; }
	return ECrossDataType_Values[_ivalue];
}
}

