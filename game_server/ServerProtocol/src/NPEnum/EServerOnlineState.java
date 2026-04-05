package NPEnum;

/*********
 * 服务器状态
 **/
public enum EServerOnlineState {
	OPEN, //0 ==== 在线-所有人可访问
	CLOSED, //1 ==== 维护-白名单可以访问
	TEMP_CLOSED, //2 ==== 测试-白名单可以访问
	;
public static final EServerOnlineState[]  EServerOnlineState_Values = EServerOnlineState.values();
public static final int EServerOnlineState_Length = EServerOnlineState_Values.length;
public static EServerOnlineState EServerOnlineState_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EServerOnlineState_Length){ return null; }
	return EServerOnlineState_Values[_ivalue];
}
}

