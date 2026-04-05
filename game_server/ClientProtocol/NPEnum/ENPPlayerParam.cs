using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家参数枚举
/// </summary>
public enum ENPPlayerParam {
	[InspectorName("NONE - idx[0] - 无效默认值")]
	NONE, //0 ==== 无效默认值
	[InspectorName("LEVEL - idx[1] - 玩家（领主）等级")]
	LEVEL, //1 ==== 玩家（领主）等级
	[InspectorName("VIP_LVL - idx[2] - VIP等级")]
	VIP_LVL, //2 ==== VIP等级
	[InspectorName("GM_LVL - idx[3] - 未接入 GM权限")]
	GM_LVL, //3 ==== 未接入 GM权限
	[InspectorName("PLAYER_SKIN - idx[4] - 玩家使用皮肤Id")]
	PLAYER_SKIN, //4 ==== 玩家使用皮肤Id
	[InspectorName("ICON - idx[5] - 玩家头像")]
	ICON, //5 ==== 玩家头像
	[InspectorName("ICON_BGK - idx[6] - 头像框ID")]
	ICON_BGK, //6 ==== 头像框ID
	[InspectorName("BUBBLE - idx[7] - 玩家使用气泡框Id")]
	BUBBLE, //7 ==== 玩家使用气泡框Id
	[InspectorName("CREATE_TIME - idx[8] - 创角时间")]
	CREATE_TIME, //8 ==== 创角时间
	[InspectorName("RECHARGED_GEM - idx[9] - 未接入 累计充值宝石")]
	RECHARGED_GEM, //9 ==== 未接入 累计充值宝石
	[InspectorName("BIRTH_GIFTDE_COUM - idx[10] - 下次出生卷王次数")]
	BIRTH_GIFTDE_COUM, //10 ==== 下次出生卷王次数
	[InspectorName("LOGIN_DAY_COUNT - idx[11] - 登录天数")]
	LOGIN_DAY_COUNT, //11 ==== 登录天数
	[InspectorName("LAST_LOGIN_DATE - idx[12] -  最后一次登录的日期标记")]
	LAST_LOGIN_DATE, //12 ====  最后一次登录的日期标记
	[InspectorName("LAST_TAKE_VERSION - idx[13] - 未接入 最后领取客户端版本奖励的版本")]
	LAST_TAKE_VERSION, //13 ==== 未接入 最后领取客户端版本奖励的版本
	[InspectorName("LAST_LEFT_BAG_TIME - idx[14] - 最后一次查看背包物品的时间")]
	LAST_LEFT_BAG_TIME, //14 ==== 最后一次查看背包物品的时间
	[InspectorName("LAST_OFFLINE_MS - idx[15] - 最后一次离线时间戳（毫秒）")]
	LAST_OFFLINE_MS, //15 ==== 最后一次离线时间戳（毫秒）
	[InspectorName("CUTE_ACTOR - idx[16] - 玩家Q版形象ID")]
	CUTE_ACTOR, //16 ==== 玩家Q版形象ID
	[InspectorName("OFFLINE_PERIOD_REWARD_DURATION_MS - idx[17] - 离线期间奖励时长（毫秒） 大于0则有奖励需要展示")]
	OFFLINE_PERIOD_REWARD_DURATION_MS, //17 ==== 离线期间奖励时长（毫秒） 大于0则有奖励需要展示
	[InspectorName("POWER_MAX_RECORD - idx[18] - 玩家战力历史记录最高值")]
	POWER_MAX_RECORD, //18 ==== 玩家战力历史记录最高值
	[InspectorName("EARNINGS_MAX_RECORD - idx[19] - 玩家赚速历史记录最高值")]
	EARNINGS_MAX_RECORD, //19 ==== 玩家赚速历史记录最高值
	[InspectorName("PREFAB - idx[20] - 玩家预制形象")]
	PREFAB, //20 ==== 玩家预制形象
	[InspectorName("LAST_DRAW_DAILY_REWARD_DATE - idx[21] - 最后一次领取每日奖励的日期标记")]
	LAST_DRAW_DAILY_REWARD_DATE, //21 ==== 最后一次领取每日奖励的日期标记
	[InspectorName("IS_SPACE_CAM_DRAG - idx[22] - 大地图摄像机是否拖拽 0-不可以 1-可以")]
	IS_SPACE_CAM_DRAG, //22 ==== 大地图摄像机是否拖拽 0-不可以 1-可以
	[InspectorName("SERVER_START_DAYS - idx[23] - 服务器开启天数")]
	SERVER_START_DAYS, //23 ==== 服务器开启天数
	[InspectorName("EXTRA_EARNINGS - idx[24] - 额外赚速")]
	EXTRA_EARNINGS, //24 ==== 额外赚速
	[InspectorName("IS_SET_DEFAULT - idx[25] - 已完成创角操作")]
	IS_SET_DEFAULT, //25 ==== 已完成创角操作
	[InspectorName("BUILDINGS_EARNINGS_MAX_RECORD - idx[26] - 建筑赚速历史记录最高值")]
	BUILDINGS_EARNINGS_MAX_RECORD, //26 ==== 建筑赚速历史记录最高值
	[InspectorName("CHILD_EARNINGS_MAX_RECORD - idx[27] - 子嗣赚速历史记录最高值")]
	CHILD_EARNINGS_MAX_RECORD, //27 ==== 子嗣赚速历史记录最高值
	[InspectorName("PLAYER_STAGE - idx[28] - 已废弃 玩家段位")]
	PLAYER_STAGE, //28 ==== 已废弃 玩家段位
	[InspectorName("LAST_LOGIN_WEEK_TAG - idx[29] - 最后一次登录周标记")]
	LAST_LOGIN_WEEK_TAG, //29 ==== 最后一次登录周标记
	[InspectorName("WEEK_LOGIN_DAY_COUNT - idx[30] - 本周登录天数")]
	WEEK_LOGIN_DAY_COUNT, //30 ==== 本周登录天数
	[InspectorName("LATEST_LOGIN_TIME_MS - idx[31] - 最近一次登陆时间（毫秒）")]
	LATEST_LOGIN_TIME_MS, //31 ==== 最近一次登陆时间（毫秒）
	[InspectorName("SEVEN_DAYS_LOGIN_COUNT - idx[32] - 七日登录天数")]
	SEVEN_DAYS_LOGIN_COUNT, //32 ==== 七日登录天数
	[InspectorName("HAD_DRAW_VIP_REWARD_LIST - idx[33] - 已领取VIP奖励列表 二进制位表示")]
	HAD_DRAW_VIP_REWARD_LIST, //33 ==== 已领取VIP奖励列表 二进制位表示
	[InspectorName("HAD_DRAW_VIP_RECHARGE_REWARD_LIST - idx[34] - 已领取VIP充值奖励列表 二进制位表示")]
	HAD_DRAW_VIP_RECHARGE_REWARD_LIST, //34 ==== 已领取VIP充值奖励列表 二进制位表示
	[InspectorName("NATION_POWER_MAX_RECORD - idx[35] - 已废弃 玩家国力历史记录最高值")]
	NATION_POWER_MAX_RECORD, //35 ==== 已废弃 玩家国力历史记录最高值
	[InspectorName("MARKET_NEXT_REFRESH_MS - idx[36] - 集市下次刷新时间")]
	MARKET_NEXT_REFRESH_MS, //36 ==== 集市下次刷新时间
	[InspectorName("HERO_ATTR_MAX_RECORD - idx[37] - 已废弃 单一大臣属性值历史记录最高值")]
	HERO_ATTR_MAX_RECORD, //37 ==== 已废弃 单一大臣属性值历史记录最高值
	[InspectorName("CONSORT_CALL_NEXT_REFRESH_MS - idx[38] - 情人指定邀约下次刷新时间 毫秒时间戳")]
	CONSORT_CALL_NEXT_REFRESH_MS, //38 ==== 情人指定邀约下次刷新时间 毫秒时间戳
	[InspectorName("LAST_GAIN_VISIT_REWARD_DAY - idx[39] - 最近一次领取拜访其他玩家奖励日期 YYYYMMDD")]
	LAST_GAIN_VISIT_REWARD_DAY, //39 ==== 最近一次领取拜访其他玩家奖励日期 YYYYMMDD
	[InspectorName("ADULT_RECORD_BONUS - idx[40] - 子嗣记录收益总值（因为移除记录在玩家身上）")]
	ADULT_RECORD_BONUS, //40 ==== 子嗣记录收益总值（因为移除记录在玩家身上）
	[InspectorName("IS_SET_PREFAB - idx[41] - 已设置预制形象")]
	IS_SET_PREFAB, //41 ==== 已设置预制形象
	[InspectorName("DAY_HAD_SEND_PAY_AI_TIMES - idx[42] - 每日已发送付费AI次数")]
	DAY_HAD_SEND_PAY_AI_TIMES, //42 ==== 每日已发送付费AI次数
	[InspectorName("PENDING_ORDER_DB_ID - idx[43] - 待处理订单ID")]
	PENDING_ORDER_DB_ID, //43 ==== 待处理订单ID
	[InspectorName("IS_REFUSE_MARRY_REQUEST - idx[44] - 是否拒绝所有联姻请求 0-否 1-是")]
	IS_REFUSE_MARRY_REQUEST, //44 ==== 是否拒绝所有联姻请求 0-否 1-是
	[InspectorName("HAD_MARS_POWER_RANK_OPENED - idx[45] - 是否开启过火星实力排行榜 0-否 1-是")]
	HAD_MARS_POWER_RANK_OPENED, //45 ==== 是否开启过火星实力排行榜 0-否 1-是
	[InspectorName("ROOM_SKIN - idx[46] - 房间皮肤Id")]
	ROOM_SKIN, //46 ==== 房间皮肤Id
	[InspectorName("MARS_GO_ROUTE_ARRIVE_COUNT - idx[47] - 累计抵达火星的玩家数量")]
	MARS_GO_ROUTE_ARRIVE_COUNT, //47 ==== 累计抵达火星的玩家数量
	[InspectorName("GIFTDE_CHILD_GRADUATE_COUNT - idx[48] - 卷王子嗣毕业历史数量，第一个毕业时无视概率发放宴会凭证（仅服务端使用）")]
	GIFTDE_CHILD_GRADUATE_COUNT, //48 ==== 卷王子嗣毕业历史数量，第一个毕业时无视概率发放宴会凭证（仅服务端使用）
}

public class ENPPlayerParamComparer : IEqualityComparer<ENPPlayerParam>{
	public bool Equals(ENPPlayerParam x, ENPPlayerParam y) { return x == y; }
	public int GetHashCode(ENPPlayerParam obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 49;
}
}

