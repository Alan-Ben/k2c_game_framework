using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 物品大类型
/// </summary>
public enum ENPItemType {
	NONE, //0 ==== 
	[InspectorName("CURRENCY - idx[1] - 货币，count: 数量")]
	CURRENCY, //1 ==== 货币，count: 数量
	[InspectorName("BAG_ITEM - idx[2] - 背包物品，count: 数量")]
	BAG_ITEM, //2 ==== 背包物品，count: 数量
	[InspectorName("TITLE - idx[3] - 普通称号，count: 时间（秒）")]
	TITLE, //3 ==== 普通称号，count: 时间（秒）
	[InspectorName("ICON - idx[4] - 头像，count: 时间（秒）")]
	ICON, //4 ==== 头像，count: 时间（秒）
	[InspectorName("ICON_BGK - idx[5] - 头像框，count: 时间（秒）")]
	ICON_BGK, //5 ==== 头像框，count: 时间（秒）
	[InspectorName("BUBBLE - idx[6] - 气泡框，count: 时间（秒）")]
	BUBBLE, //6 ==== 气泡框，count: 时间（秒）
	[InspectorName("REMOTE_EFFECT - idx[7] - 远程效果，id为Remote配表ID，count: 执行次数")]
	REMOTE_EFFECT, //7 ==== 远程效果，id为Remote配表ID，count: 执行次数
	[InspectorName("MAIL - idx[8] - 邮件，预设邮件")]
	MAIL, //8 ==== 邮件，预设邮件
	[InspectorName("SYS_INFO - idx[9] - 系统信息（如解锁信息），count: 数量")]
	SYS_INFO, //9 ==== 系统信息（如解锁信息），count: 数量
	[InspectorName("BUILDING - idx[10] - 给予建筑信息，count: 数量")]
	BUILDING, //10 ==== 给予建筑信息，count: 数量
	[InspectorName("REWARD - idx[11] - 奖励, count: 数量")]
	REWARD, //11 ==== 奖励, count: 数量
	[InspectorName("CUTE_ACTOR - idx[12] - Q版形象，count: 时间（秒）")]
	CUTE_ACTOR, //12 ==== Q版形象，count: 时间（秒）
	[InspectorName("WEEK_CARD - idx[13] - 周卡，count:时间（秒）")]
	WEEK_CARD, //13 ==== 周卡，count:时间（秒）
	[InspectorName("SYS_UNLOCK - idx[14] - 系统解锁类型，对应simple_unlock数据，可以增加系统解锁展示，count: 数量")]
	SYS_UNLOCK, //14 ==== 系统解锁类型，对应simple_unlock数据，可以增加系统解锁展示，count: 数量
	[InspectorName("ANECDOTE_EVENT - idx[15] - 政务事件 ----- itemId：事件id，count: 位置id")]
	ANECDOTE_EVENT, //15 ==== 政务事件 ----- itemId：事件id，count: 位置id
	[InspectorName("EQUIP - idx[16] - 藏品，count: 数量")]
	EQUIP, //16 ==== 藏品，count: 数量
	[InspectorName("TITLE_PRE - idx[17] - 组合称号前缀")]
	TITLE_PRE, //17 ==== 组合称号前缀
	[InspectorName("TITLE_SFX - idx[18] - 组合称号后缀")]
	TITLE_SFX, //18 ==== 组合称号后缀
	[InspectorName("TITLE_BG - idx[19] - 组合称号底色")]
	TITLE_BG, //19 ==== 组合称号底色
	[InspectorName("PLAYER_SKIN - idx[20] - 玩家形象，count: 时间（秒）")]
	PLAYER_SKIN, //20 ==== 玩家形象，count: 时间（秒）
	[InspectorName("ACTIVITY_CURRENCY - idx[21] - 活动兑换券，id为活动id, count: 数量")]
	ACTIVITY_CURRENCY, //21 ==== 活动兑换券，id为活动id, count: 数量
	[InspectorName("LAZY_CD - idx[22] - CD，count: 数量")]
	LAZY_CD, //22 ==== CD，count: 数量
	[InspectorName("INN_RECIPE - idx[23] - 旅店菜谱，count: 1")]
	INN_RECIPE, //23 ==== 旅店菜谱，count: 1
	[InspectorName("MUSEUM_ITEM - idx[24] - 博物馆物品，count:1")]
	MUSEUM_ITEM, //24 ==== 博物馆物品，count:1
	[InspectorName("PAY - idx[25] - 现金支付，count:无意义")]
	PAY, //25 ==== 现金支付，count:无意义
	[InspectorName("BUFF - idx[26] - BUFF，count:有效时长（秒）")]
	BUFF, //26 ==== BUFF，count:有效时长（秒）
	[InspectorName("FOREVER_ADD - idx[27] - 永久加成，id：player_forever_add配表id，count: 次数")]
	FOREVER_ADD, //27 ==== 永久加成，id：player_forever_add配表id，count: 次数
	[InspectorName("TOOL_TIP_ITEM - idx[28] - 点击提示，客户端使用")]
	TOOL_TIP_ITEM, //28 ==== 点击提示，客户端使用
	[InspectorName("EXCHANGE_ITEM - idx[29] - 转换道具，id为转换配表id,count:转换次数")]
	EXCHANGE_ITEM, //29 ==== 转换道具，id为转换配表id,count:转换次数
	[InspectorName("COUNTDOWN_EVENT - idx[30] - 倒计时事件，id为倒计时事件配表id，count：数量")]
	COUNTDOWN_EVENT, //30 ==== 倒计时事件，id为倒计时事件配表id，count：数量
	[InspectorName("QUEST - idx[31] - 任务，count无意义")]
	QUEST, //31 ==== 任务，count无意义
	[InspectorName("ITEM_DEF - idx[32] - 自定义道具 id为itemRef表id，count：数量")]
	ITEM_DEF, //32 ==== 自定义道具 id为itemRef表id，count：数量
	[InspectorName("FIXED_CD - idx[33] - 固定点增加cd，count: 数量")]
	FIXED_CD, //33 ==== 固定点增加cd，count: 数量
	[InspectorName("RECORD - idx[34] - 玩家记录，id枚举：ENPPlayerRecordParam，count：数量")]
	RECORD, //34 ==== 玩家记录，id枚举：ENPPlayerRecordParam，count：数量
	[InspectorName("TREASURE_MAP - idx[35] - 藏宝图，count:数量")]
	TREASURE_MAP, //35 ==== 藏宝图，count:数量
	[InspectorName("UNUSE_36 - idx[36] - 未使用 ----- 随机宠物，count:数量")]
	UNUSE_36, //36 ==== 未使用 ----- 随机宠物，count:数量
	[InspectorName("SHARE - idx[37] - 分享物品，id为share表ID，count:分享次数")]
	SHARE, //37 ==== 分享物品，id为share表ID，count:分享次数
	[InspectorName("CLOTHES_UNIT_RAND_DYE - idx[38] - 随机获得指定单品色盘，id为单品ID，count：1")]
	CLOTHES_UNIT_RAND_DYE, //38 ==== 随机获得指定单品色盘，id为单品ID，count：1
	[InspectorName("CLOTHES_DYE_PALETTE - idx[39] - 色盘, id为配表ID，count：1")]
	CLOTHES_DYE_PALETTE, //39 ==== 色盘, id为配表ID，count：1
	[InspectorName("UNUSE_40 - idx[40] - 未使用 ----- 精灵，id为elf表id，count为数量")]
	UNUSE_40, //40 ==== 未使用 ----- 精灵，id为elf表id，count为数量
	[InspectorName("HERO - idx[41] - 大臣，id为hero表id，count：1")]
	HERO, //41 ==== 大臣，id为hero表id，count：1
	[InspectorName("HERO_SKIN - idx[42] - 大臣皮肤，id为hero_skin表id，count：1")]
	HERO_SKIN, //42 ==== 大臣皮肤，id为hero_skin表id，count：1
	[InspectorName("CONSORT - idx[43] - 情人，id为consort表id，count：1")]
	CONSORT, //43 ==== 情人，id为consort表id，count：1
	[InspectorName("CONSORT_SKIN - idx[44] - 情人皮肤，id为consort_skin表id，count：1")]
	CONSORT_SKIN, //44 ==== 情人皮肤，id为consort_skin表id，count：1
	[InspectorName("CLOTHES_UNIT - idx[45] - 时装单品，id为配表ID，count：1")]
	CLOTHES_UNIT, //45 ==== 时装单品，id为配表ID，count：1
	[InspectorName("CLOTHES_BG - idx[46] - 时装背景，id为配表ID，count：1")]
	CLOTHES_BG, //46 ==== 时装背景，id为配表ID，count：1
	[InspectorName("ACHIEVE_POINT - idx[47] - 成就点，id为EAchieveTyped的id，count：数量")]
	ACHIEVE_POINT, //47 ==== 成就点，id为EAchieveTyped的id，count：数量
	[InspectorName("CLOTHES_SUIT - idx[48] - 时装套装，id为配表ID，count：1")]
	CLOTHES_SUIT, //48 ==== 时装套装，id为配表ID，count：1
	[InspectorName("HERO_RECOMMEND - idx[49] - 大臣推荐事件")]
	HERO_RECOMMEND, //49 ==== 大臣推荐事件
	[InspectorName("CHAT_EMOTE_GROUP - idx[50] - 聊天表情包，count: 时间（秒）")]
	CHAT_EMOTE_GROUP, //50 ==== 聊天表情包，count: 时间（秒）
	[InspectorName("CLOTHES_ACT - idx[51] - 时装动作，id为配表ID，count：1")]
	CLOTHES_ACT, //51 ==== 时装动作，id为配表ID，count：1
	[InspectorName("CLOTHES_POSE - idx[52] - 时装姿势，id为配表ID，count：1")]
	CLOTHES_POSE, //52 ==== 时装姿势，id为配表ID，count：1
	[InspectorName("CLOTHES_HAND_POSE - idx[53] - 时装手势，id为配表ID，count：1")]
	CLOTHES_HAND_POSE, //53 ==== 时装手势，id为配表ID，count：1
	[InspectorName("CONSORT_CG - idx[54] - 情人CG，id为consort_cg表id，count：1")]
	CONSORT_CG, //54 ==== 情人CG，id为consort_cg表id，count：1
	[InspectorName("PRIVILEGE_CARD - idx[55] - 权益卡，id为EPrivilegeCardType枚举，count：数量")]
	PRIVILEGE_CARD, //55 ==== 权益卡，id为EPrivilegeCardType枚举，count：数量
	[InspectorName("GUILD_BOX - idx[56] - 联盟宝箱")]
	GUILD_BOX, //56 ==== 联盟宝箱
	[InspectorName("ROOM_SKIN - idx[57] - 房间皮肤")]
	ROOM_SKIN, //57 ==== 房间皮肤
}

public class ENPItemTypeComparer : IEqualityComparer<ENPItemType>{
	public bool Equals(ENPItemType x, ENPItemType y) { return x == y; }
	public int GetHashCode(ENPItemType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 58;
}
}

