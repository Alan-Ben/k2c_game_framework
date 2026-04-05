using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 活动类型
/// </summary>
public enum ECommonActivityType {
	NONE, //0 ==== 
	[InspectorName("RUSH_RANK - idx[1] - 冲榜活动")]
	RUSH_RANK, //1 ==== 冲榜活动
	[InspectorName("EARNINGS_GOAL - idx[2] - 赚速目标")]
	EARNINGS_GOAL, //2 ==== 赚速目标
	[InspectorName("SEVEN_DAY_GOALS - idx[3] - 七日目标")]
	SEVEN_DAY_GOALS, //3 ==== 七日目标
	[InspectorName("REGULAR_EVENT - idx[4] - 万能活动")]
	REGULAR_EVENT, //4 ==== 万能活动
	[InspectorName("TILE_MATCH - idx[5] - 三消")]
	TILE_MATCH, //5 ==== 三消
	[InspectorName("RECHARGE_REBATE - idx[6] - 充值返利")]
	RECHARGE_REBATE, //6 ==== 充值返利
	[InspectorName("NUM_MERGE - idx[7] - 2048合成")]
	NUM_MERGE, //7 ==== 2048合成
	RESERVE_1, //8 ==== 
	RESERVE_2, //9 ==== 
	RESERVE_3, //10 ==== 
	RESERVE_4, //11 ==== 
	RESERVE_5, //12 ==== 
	RESERVE_6, //13 ==== 
	RESERVE_7, //14 ==== 
	RESERVE_8, //15 ==== 
	RESERVE_9, //16 ==== 
	RESERVE_10, //17 ==== 
	[InspectorName("ACTIVITY_FUND - idx[18] - 活动基金")]
	ACTIVITY_FUND, //18 ==== 活动基金
	[InspectorName("RANK_GIFT_PACK - idx[19] - 排行榜礼包")]
	RANK_GIFT_PACK, //19 ==== 排行榜礼包
	[InspectorName("FIRST_TEAM - idx[20] - 组队活动")]
	FIRST_TEAM, //20 ==== 组队活动
}

public class ECommonActivityTypeComparer : IEqualityComparer<ECommonActivityType>{
	public bool Equals(ECommonActivityType x, ECommonActivityType y) { return x == y; }
	public int GetHashCode(ECommonActivityType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 21;
}
}

