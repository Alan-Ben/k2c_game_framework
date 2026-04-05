package NPEnum;

/*********
 * 聊天房间类型
 **/
public enum ENPChatRoomType {
	NONE, //0 ==== 
	TOTAL_AREA, //1 ==== 全区玩家聊天
	US_SERVER, //2 ==== 单个玩家服聊天
	GUILD, //3 ==== 联盟聊天
	ACTIVITY_TEAM, //4 ==== 活动队伍聊天
	;
public static final ENPChatRoomType[]  ENPChatRoomType_Values = ENPChatRoomType.values();
public static final int ENPChatRoomType_Length = ENPChatRoomType_Values.length;
public static ENPChatRoomType ENPChatRoomType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPChatRoomType_Length){ return null; }
	return ENPChatRoomType_Values[_ivalue];
}
}

