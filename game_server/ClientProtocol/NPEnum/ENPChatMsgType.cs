using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 聊天消息内容分类
/// </summary>
public enum ENPChatMsgType {
	NONE, //0 ==== 
	[InspectorName("TEXT - idx[1] - 普通私聊内容,代表消息内容需要用 ChatContent_TextMsg")]
	TEXT, //1 ==== 普通私聊内容,代表消息内容需要用 ChatContent_TextMsg
	[InspectorName("TIME - idx[2] - 时间消息,代表消息内容是个时间戳")]
	TIME, //2 ==== 时间消息,代表消息内容是个时间戳
	[InspectorName("SYSTEM - idx[3] - 系统消息")]
	SYSTEM, //3 ==== 系统消息
	[InspectorName("MIDDAY_DUNGEON_BOX - idx[4] - 午间副本宝箱")]
	MIDDAY_DUNGEON_BOX, //4 ==== 午间副本宝箱
	[InspectorName("SYSTEM_LOG - idx[5] - 系统日志消息")]
	SYSTEM_LOG, //5 ==== 系统日志消息
	[InspectorName("COMM_BOX - idx[6] - 通用宝箱")]
	COMM_BOX, //6 ==== 通用宝箱
	[InspectorName("EVENING_DUNGEON_BOX - idx[7] - 晚间副本宝箱")]
	EVENING_DUNGEON_BOX, //7 ==== 晚间副本宝箱
	[InspectorName("ADULT_MARRY_SERVER_APPLY - idx[8] - 子嗣全服联姻")]
	ADULT_MARRY_SERVER_APPLY, //8 ==== 子嗣全服联姻
	[InspectorName("DINNER_INVITE - idx[9] - 宴会邀请")]
	DINNER_INVITE, //9 ==== 宴会邀请
	[InspectorName("EMOTE - idx[10] - 表情")]
	EMOTE, //10 ==== 表情
	[InspectorName("SHARE_HERO - idx[11] - 骑士分享")]
	SHARE_HERO, //11 ==== 骑士分享
	[InspectorName("SHARE_CONSORT - idx[12] - 妃子分享")]
	SHARE_CONSORT, //12 ==== 妃子分享
	[InspectorName("SHARE_CHILD - idx[13] - 子嗣分享")]
	SHARE_CHILD, //13 ==== 子嗣分享
	[InspectorName("ACTIVITY_RANK_BOX - idx[14] - 冲榜宝箱")]
	ACTIVITY_RANK_BOX, //14 ==== 冲榜宝箱
	[InspectorName("GUILD_PRIVATE_INFORM - idx[15] - 联盟私聊通知")]
	GUILD_PRIVATE_INFORM, //15 ==== 联盟私聊通知
	[InspectorName("GUILD_LOG - idx[16] - 联盟日志消息")]
	GUILD_LOG, //16 ==== 联盟日志消息
	[InspectorName("GUILD_RECRUIT - idx[17] - 联盟招募")]
	GUILD_RECRUIT, //17 ==== 联盟招募
	[InspectorName("SHARE_CONSORT_CG - idx[18] - 情人CG分享")]
	SHARE_CONSORT_CG, //18 ==== 情人CG分享
	[InspectorName("GUILD_MARS_MINE_OCCUPY - idx[19] - 联盟成员火星矿被攻击")]
	GUILD_MARS_MINE_OCCUPY, //19 ==== 联盟成员火星矿被攻击
	[InspectorName("SHARE_MARS_EXPLORE_MINE - idx[20] - 火星探险矿分享")]
	SHARE_MARS_EXPLORE_MINE, //20 ==== 火星探险矿分享
}

public class ENPChatMsgTypeComparer : IEqualityComparer<ENPChatMsgType>{
	public bool Equals(ENPChatMsgType x, ENPChatMsgType y) { return x == y; }
	public int GetHashCode(ENPChatMsgType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 21;
}
}

