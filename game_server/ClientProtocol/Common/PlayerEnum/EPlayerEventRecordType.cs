using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.PlayerEnum
{

/// <summary>
/// 玩家行为计数器类型，sub_id取（-1）表示所有子计数器之和，因为每个类型是两层结构，设计时应该避免第二次id的数量过大
/// </summary>
public enum EPlayerEventRecordType {
	NONE, //0 ==== 
	[InspectorName("LOGIN_DAY_SUM - idx[1] - 登录天数统计 LOGIN_DAY_SUM（@0），id无意义只能配置为0")]
	LOGIN_DAY_SUM, //1 ==== 登录天数统计 LOGIN_DAY_SUM（@0），id无意义只能配置为0
	[InspectorName("ACHIEVE_POINT_REWARDED - idx[2] - 成就点领取记录 ACHIEVE_POINT_REWARDED@id，id：achieve_step_reward配表id")]
	ACHIEVE_POINT_REWARDED, //2 ==== 成就点领取记录 ACHIEVE_POINT_REWARDED@id，id：achieve_step_reward配表id
	[InspectorName("STAGE_GOAL_FINISH_TIME_MS - idx[3] - 阶段任务完成时间 STAGE_GOAL_FINISH_TIME_MS@id，id：stage_goal配表id")]
	STAGE_GOAL_FINISH_TIME_MS, //3 ==== 阶段任务完成时间 STAGE_GOAL_FINISH_TIME_MS@id，id：stage_goal配表id
	[InspectorName("BAG_ITEM_SPEND - idx[4] - 累计背包物品消耗，id为bag_item主键ID")]
	BAG_ITEM_SPEND, //4 ==== 累计背包物品消耗，id为bag_item主键ID
	[InspectorName("CURRENCY_SPEND - idx[5] - 累计货币物品消耗，id为ECurrency枚举")]
	CURRENCY_SPEND, //5 ==== 累计货币物品消耗，id为ECurrency枚举
	[InspectorName("GAIN_EQUIP - idx[6] - 累计获得藏品次数，id为藏品id")]
	GAIN_EQUIP, //6 ==== 累计获得藏品次数，id为藏品id
	[InspectorName("DRAW_ACHIEVE_STEP_REWARD - idx[7] - 领取成就阶段奖励次数，id为achieve_step主键ID即成就id")]
	DRAW_ACHIEVE_STEP_REWARD, //7 ==== 领取成就阶段奖励次数，id为achieve_step主键ID即成就id
	[InspectorName("DRAW_DAILY_QUEST_ACTIVE_REWARD - idx[8] - 领取每日任务活跃奖励次数，id为daily_quest_active_reward主键ID")]
	DRAW_DAILY_QUEST_ACTIVE_REWARD, //8 ==== 领取每日任务活跃奖励次数，id为daily_quest_active_reward主键ID
	[InspectorName("DINNER_OPEN - idx[9] - 宴会开启，id为宴会类型")]
	DINNER_OPEN, //9 ==== 宴会开启，id为宴会类型
	[InspectorName("DINNER_JOIN - idx[10] - 宴会参与，id为宴会类型")]
	DINNER_JOIN, //10 ==== 宴会参与，id为宴会类型
	[InspectorName("COUNTDOWN_EVENT_ADD_TIMES - idx[11] - 倒计时事件添加次数，id为倒计时事件类型")]
	COUNTDOWN_EVENT_ADD_TIMES, //11 ==== 倒计时事件添加次数，id为倒计时事件类型
	[InspectorName("ANECDOTE_EVENT_DEAL_TIMES - idx[12] - 经营事件处理次数，id为经营事件id")]
	ANECDOTE_EVENT_DEAL_TIMES, //12 ==== 经营事件处理次数，id为经营事件id
	[InspectorName("GEM_RECHARGE_TIMES - idx[13] - 钻石充值次数，id为gem_recharge配表id")]
	GEM_RECHARGE_TIMES, //13 ==== 钻石充值次数，id为gem_recharge配表id
}

public class EPlayerEventRecordTypeComparer : IEqualityComparer<EPlayerEventRecordType>{
	public bool Equals(EPlayerEventRecordType x, EPlayerEventRecordType y) { return x == y; }
	public int GetHashCode(EPlayerEventRecordType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 14;
}
}

