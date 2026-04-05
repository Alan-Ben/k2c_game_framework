package NPEnum;

/*********
 * 玩家buff类型
 **/
public enum ENPPlayerBuffFunctionType {
	NONE, //0 ==== 
	MARS_EVENT, //1 ==== 火星事件
	;
public static final ENPPlayerBuffFunctionType[]  ENPPlayerBuffFunctionType_Values = ENPPlayerBuffFunctionType.values();
public static final int ENPPlayerBuffFunctionType_Length = ENPPlayerBuffFunctionType_Values.length;
public static ENPPlayerBuffFunctionType ENPPlayerBuffFunctionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerBuffFunctionType_Length){ return null; }
	return ENPPlayerBuffFunctionType_Values[_ivalue];
}
}

