using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 系统功能类型
/// </summary>
public enum ENPFunctionType {
	NONE, //0 ==== 
	[InspectorName("CHAT - idx[1] - 聊天")]
	CHAT, //1 ==== 聊天
	[InspectorName("CHAT_INPUT - idx[2] - 聊天输入")]
	CHAT_INPUT, //2 ==== 聊天输入
	[InspectorName("ACHIEVE - idx[3] - 成就")]
	ACHIEVE, //3 ==== 成就
	[InspectorName("FRIEND - idx[4] - 好友")]
	FRIEND, //4 ==== 好友
	[InspectorName("GUILD - idx[5] - 联盟")]
	GUILD, //5 ==== 联盟
	[InspectorName("DAILY_QUEST - idx[6] - 每日任务")]
	DAILY_QUEST, //6 ==== 每日任务
	[InspectorName("CHAPTER - idx[7] - 关卡")]
	CHAPTER, //7 ==== 关卡
	[InspectorName("RANK_COMMON - idx[8] - 排行榜")]
	RANK_COMMON, //8 ==== 排行榜
	[InspectorName("SHOP - idx[9] - 商店")]
	SHOP, //9 ==== 商店
	[InspectorName("QUEST - idx[10] - 主线任务")]
	QUEST, //10 ==== 主线任务
	[InspectorName("BAG - idx[11] - 背包")]
	BAG, //11 ==== 背包
	[InspectorName("WEEKLY_QUEST - idx[12] - 周任务")]
	WEEKLY_QUEST, //12 ==== 周任务
	[InspectorName("EQUIP - idx[13] - 藏品")]
	EQUIP, //13 ==== 藏品
	[InspectorName("ANECDOTE - idx[14] - 政务")]
	ANECDOTE, //14 ==== 政务
	[InspectorName("ARENA - idx[15] - 竞技场")]
	ARENA, //15 ==== 竞技场
	[InspectorName("RANK_RUSH - idx[16] - 冲榜")]
	RANK_RUSH, //16 ==== 冲榜
	[InspectorName("CONSORT - idx[17] - 情人")]
	CONSORT, //17 ==== 情人
	[InspectorName("CHILD - idx[18] - 子嗣")]
	CHILD, //18 ==== 子嗣
	[InspectorName("CHILD_MARRY - idx[19] - 子嗣联姻")]
	CHILD_MARRY, //19 ==== 子嗣联姻
	[InspectorName("GACHA - idx[20] - 抽卡")]
	GACHA, //20 ==== 抽卡
	[InspectorName("DINNER - idx[21] - 宴会")]
	DINNER, //21 ==== 宴会
	[InspectorName("HERO - idx[22] - 骑士")]
	HERO, //22 ==== 骑士
	[InspectorName("ROOM - idx[23] - 公主卧室")]
	ROOM, //23 ==== 公主卧室
	[InspectorName("CITY - idx[24] - 主城")]
	CITY, //24 ==== 主城
	[InspectorName("MARKET - idx[25] - 集市")]
	MARKET, //25 ==== 集市
	[InspectorName("DAILY_CHECK - idx[26] - 每日签到")]
	DAILY_CHECK, //26 ==== 每日签到
	[InspectorName("TOWER - idx[27] - 爬塔")]
	TOWER, //27 ==== 爬塔
	[InspectorName("STAGE_GOAL - idx[28] - 阶段目标")]
	STAGE_GOAL, //28 ==== 阶段目标
	[InspectorName("PLAYER_INFO - idx[29] - 玩家信息")]
	PLAYER_INFO, //29 ==== 玩家信息
	[InspectorName("TRAVEL - idx[30] - 游历")]
	TRAVEL, //30 ==== 游历
	[InspectorName("MAIL - idx[31] - 邮件")]
	MAIL, //31 ==== 邮件
	[InspectorName("HERO_RECOMMEND - idx[32] - 骑士推荐")]
	HERO_RECOMMEND, //32 ==== 骑士推荐
	[InspectorName("RANK - idx[33] - 排行榜")]
	RANK, //33 ==== 排行榜
	[InspectorName("ANNOUNCEMENT - idx[34] - 运营公告")]
	ANNOUNCEMENT, //34 ==== 运营公告
	[InspectorName("COMMON_TARGET - idx[35] - 家人获得")]
	COMMON_TARGET, //35 ==== 家人获得
	[InspectorName("MIDDAY_DUNGEON - idx[36] - 午间副本")]
	MIDDAY_DUNGEON, //36 ==== 午间副本
	[InspectorName("EVENING_DUNGEON - idx[37] - 晚间副本")]
	EVENING_DUNGEON, //37 ==== 晚间副本
	[InspectorName("WALL_STREET - idx[38] - 华尔街")]
	WALL_STREET, //38 ==== 华尔街
	[InspectorName("CHILD_TRAIN - idx[39] - 子嗣培养")]
	CHILD_TRAIN, //39 ==== 子嗣培养
	[InspectorName("DUNGEON_ENTRANCE - idx[40] - 午间＆晚间boss合并入口")]
	DUNGEON_ENTRANCE, //40 ==== 午间＆晚间boss合并入口
	[InspectorName("INN_MAIN - idx[41] - 旅店主界面")]
	INN_MAIN, //41 ==== 旅店主界面
	[InspectorName("TREASURE_HUNT - idx[42] - 太空寻宝")]
	TREASURE_HUNT, //42 ==== 太空寻宝
	[InspectorName("MARS - idx[43] - 火星系统")]
	MARS, //43 ==== 火星系统
	[InspectorName("SCHOOL - idx[44] - 子嗣外围入口")]
	SCHOOL, //44 ==== 子嗣外围入口
	[InspectorName("GRAVE - idx[45] - 杰出者大厅")]
	GRAVE, //45 ==== 杰出者大厅
	[InspectorName("GUILD_COOPERATE - idx[46] - 联盟协作")]
	GUILD_COOPERATE, //46 ==== 联盟协作
	[InspectorName("LAND_MARS - idx[47] - 登录火星")]
	LAND_MARS, //47 ==== 登录火星
	[InspectorName("MARS_EXPLORE - idx[48] - 火星探索")]
	MARS_EXPLORE, //48 ==== 火星探索
	[InspectorName("ACTIVITY_FUND - idx[49] - 活动基金")]
	ACTIVITY_FUND, //49 ==== 活动基金
	[InspectorName("ROOM_SKIN - idx[50] - 卧室皮肤")]
	ROOM_SKIN, //50 ==== 卧室皮肤
}

public class ENPFunctionTypeComparer : IEqualityComparer<ENPFunctionType>{
	public bool Equals(ENPFunctionType x, ENPFunctionType y) { return x == y; }
	public int GetHashCode(ENPFunctionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 51;
}
}

