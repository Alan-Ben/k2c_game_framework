using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 货币类型
/// </summary>
public enum ECurrency {
	NONE, //0 ==== 
	[InspectorName("GEM - idx[1] - 钻石")]
	GEM, //1 ==== 钻石
	[InspectorName("SILVER - idx[2] - 金币")]
	SILVER, //2 ==== 金币
	[InspectorName("ARENA_COIN - idx[3] - 商会硬币")]
	ARENA_COIN, //3 ==== 商会硬币
	[InspectorName("VIP_EXP - idx[4] - VIP经验")]
	VIP_EXP, //4 ==== VIP经验
	[InspectorName("P_EXP - idx[5] - 玩家经验")]
	P_EXP, //5 ==== 玩家经验
	[InspectorName("DINNER_COIN - idx[6] - 宴会币")]
	DINNER_COIN, //6 ==== 宴会币
	[InspectorName("DUNGEON_COIN - idx[7] - 副本币")]
	DUNGEON_COIN, //7 ==== 副本币
	[InspectorName("DAILY_QUEST_ACTIVE_POINT - idx[8] - 日常任务活跃点")]
	DAILY_QUEST_ACTIVE_POINT, //8 ==== 日常任务活跃点
	[InspectorName("MARKET_POINT - idx[9] - 废弃 集市专用-繁荣度")]
	MARKET_POINT, //9 ==== 废弃 集市专用-繁荣度
	[InspectorName("GUILD_COIN - idx[10] - 联盟币")]
	GUILD_COIN, //10 ==== 联盟币
	[InspectorName("HERO_EXP - idx[11] - 大臣经验")]
	HERO_EXP, //11 ==== 大臣经验
	[InspectorName("TOWER_COIN - idx[12] - 迷宫币")]
	TOWER_COIN, //12 ==== 迷宫币
	[InspectorName("INN_AFFECTION - idx[13] - 旅店心意值")]
	INN_AFFECTION, //13 ==== 旅店心意值
	[InspectorName("INN_STATION_BLUEPRINT - idx[14] - 旅店设施图纸")]
	INN_STATION_BLUEPRINT, //14 ==== 旅店设施图纸
	[InspectorName("SATISFY - idx[15] - 满意值")]
	SATISFY, //15 ==== 满意值
	[InspectorName("MARS_ENERGY - idx[16] - 火星系统-能量")]
	MARS_ENERGY, //16 ==== 火星系统-能量
	[InspectorName("MARS_POINT - idx[17] - 火星币")]
	MARS_POINT, //17 ==== 火星币
	[InspectorName("BATTLE_PASS_COIN - idx[18] - 战令币")]
	BATTLE_PASS_COIN, //18 ==== 战令币
}

public class ECurrencyComparer : IEqualityComparer<ECurrency>{
	public bool Equals(ECurrency x, ECurrency y) { return x == y; }
	public int GetHashCode(ECurrency obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 19;
}
}

