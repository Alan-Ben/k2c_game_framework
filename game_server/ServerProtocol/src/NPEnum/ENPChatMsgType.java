package NPEnum;

/*********
 * 聊天消息内容分类
 **/
public enum ENPChatMsgType {
	NONE, //0 ==== 
	TEXT, //1 ==== 普通私聊内容,代表消息内容需要用 ChatContent_TextMsg
	TIME, //2 ==== 时间消息,代表消息内容是个时间戳
	SYSTEM, //3 ==== 系统消息
	MIDDAY_DUNGEON_BOX, //4 ==== 午间副本宝箱
	SYSTEM_LOG, //5 ==== 系统日志消息
	COMM_BOX, //6 ==== 通用宝箱
	EVENING_DUNGEON_BOX, //7 ==== 晚间副本宝箱
	ADULT_MARRY_SERVER_APPLY, //8 ==== 子嗣全服联姻
	DINNER_INVITE, //9 ==== 宴会邀请
	EMOTE, //10 ==== 表情
	SHARE_HERO, //11 ==== 骑士分享
	SHARE_CONSORT, //12 ==== 妃子分享
	SHARE_CHILD, //13 ==== 子嗣分享
	ACTIVITY_RANK_BOX, //14 ==== 冲榜宝箱
	GUILD_PRIVATE_INFORM, //15 ==== 联盟私聊通知
	GUILD_LOG, //16 ==== 联盟日志消息
	GUILD_RECRUIT, //17 ==== 联盟招募
	SHARE_CONSORT_CG, //18 ==== 情人CG分享
	GUILD_MARS_MINE_OCCUPY, //19 ==== 联盟成员火星矿被攻击
	SHARE_MARS_EXPLORE_MINE, //20 ==== 火星探险矿分享
	;
public static final ENPChatMsgType[]  ENPChatMsgType_Values = ENPChatMsgType.values();
public static final int ENPChatMsgType_Length = ENPChatMsgType_Values.length;
public static ENPChatMsgType ENPChatMsgType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPChatMsgType_Length){ return null; }
	return ENPChatMsgType_Values[_ivalue];
}
}

