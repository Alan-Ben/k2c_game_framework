package NPEnum;

/*********
 * 登录服务器的客户端连接类型
 **/
public enum ENPLSGameClientType {
	NONE, //0 ==== 
	USER, //1 ==== 正常的用户名登录
	USER_CHECK_CODE, //2 ==== 用户使用验证串登录
	VISITORS, //3 ==== 游客登录
	CHEAT, //4 ==== 作弊登录，username为uid
	SDK, //5 ==== SDK登录
	;
public static final ENPLSGameClientType[]  ENPLSGameClientType_Values = ENPLSGameClientType.values();
public static final int ENPLSGameClientType_Length = ENPLSGameClientType_Values.length;
public static ENPLSGameClientType ENPLSGameClientType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPLSGameClientType_Length){ return null; }
	return ENPLSGameClientType_Values[_ivalue];
}
}

