package NPEnum;

/*********
 * 聊天宝箱状态
 **/
public enum ENPBoxChatStatus {
	NONE, //0 ==== 
	IS_INVAILD, //1 ==== 已失效
	IS_LIMIT, //2 ==== 达到上限
	IS_EMPTY, //3 ==== 已领完
	IS_GAINED, //4 ==== 已领取
	;
public static final ENPBoxChatStatus[]  ENPBoxChatStatus_Values = ENPBoxChatStatus.values();
public static final int ENPBoxChatStatus_Length = ENPBoxChatStatus_Values.length;
public static ENPBoxChatStatus ENPBoxChatStatus_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPBoxChatStatus_Length){ return null; }
	return ENPBoxChatStatus_Values[_ivalue];
}
}

