package NPEnum;

public enum ENPClientSpecialDoalogueDealType {
	NONE, //0 ==== 
	CONFIRM, //1 ==== 确认
	CANCEL, //2 ==== 取消
	;
public static final ENPClientSpecialDoalogueDealType[]  ENPClientSpecialDoalogueDealType_Values = ENPClientSpecialDoalogueDealType.values();
public static final int ENPClientSpecialDoalogueDealType_Length = ENPClientSpecialDoalogueDealType_Values.length;
public static ENPClientSpecialDoalogueDealType ENPClientSpecialDoalogueDealType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPClientSpecialDoalogueDealType_Length){ return null; }
	return ENPClientSpecialDoalogueDealType_Values[_ivalue];
}
}

