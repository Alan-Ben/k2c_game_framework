using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家的值类型,高级公式 CS_VALUE的值类型
/// </summary>
public enum ENPPlayerValueType {
	NONE, //0 ==== 
	[InspectorName("LVL - idx[1] - 玩家等级")]
	LVL, //1 ==== 玩家等级
	[InspectorName("VIP_LVL - idx[2] - 玩家VIP等级")]
	VIP_LVL, //2 ==== 玩家VIP等级
	[InspectorName("RMB_NUM - idx[3] - 玩家充值RMB值")]
	RMB_NUM, //3 ==== 玩家充值RMB值
	[InspectorName("FRIEND_COUNT - idx[4] - 好友数量")]
	FRIEND_COUNT, //4 ==== 好友数量
	[InspectorName("HERO_NUM - idx[5] - 大臣数量")]
	HERO_NUM, //5 ==== 大臣数量
	[InspectorName("HERO_IN_BUILDING_NUM - idx[6] - 入驻建筑的大臣数量")]
	HERO_IN_BUILDING_NUM, //6 ==== 入驻建筑的大臣数量
	[InspectorName("EARNINGS - idx[7] - 每秒赚速")]
	EARNINGS, //7 ==== 每秒赚速
	[InspectorName("CHILD_SUM - idx[8] - 未成年子嗣数量")]
	CHILD_SUM, //8 ==== 未成年子嗣数量
	[InspectorName("TOTAL_HERO_POWER - idx[9] - 总大臣实力")]
	TOTAL_HERO_POWER, //9 ==== 总大臣实力
	[InspectorName("TOTAL_HERO_TALENT - idx[10] - 总大臣资质")]
	TOTAL_HERO_TALENT, //10 ==== 总大臣资质
	[InspectorName("CONSORT_NUM - idx[11] - 情人数量")]
	CONSORT_NUM, //11 ==== 情人数量
	[InspectorName("CHAPTER_POINT - idx[12] - 章节位置 chapterId*1000+point")]
	CHAPTER_POINT, //12 ==== 章节位置 chapterId*1000+point
	[InspectorName("BUILDING_NUM - idx[13] - 建筑数量")]
	BUILDING_NUM, //13 ==== 建筑数量
	[InspectorName("TOTAL_HERO_LEVEL - idx[14] - 总大臣等级")]
	TOTAL_HERO_LEVEL, //14 ==== 总大臣等级
	[InspectorName("TOTAL_CONSORT_INTIMACY - idx[15] - 总情人亲密度")]
	TOTAL_CONSORT_INTIMACY, //15 ==== 总情人亲密度
	[InspectorName("TOTAL_CONSORT_CHARM - idx[16] - 总情人加护力")]
	TOTAL_CONSORT_CHARM, //16 ==== 总情人加护力
	[InspectorName("DAILY_CHECK_SUM - idx[17] - 总签到天数")]
	DAILY_CHECK_SUM, //17 ==== 总签到天数
	[InspectorName("DONE_MAIN_QUEST_COUNT - idx[18] - 主线任务完成数（如果多次完成也算1次）")]
	DONE_MAIN_QUEST_COUNT, //18 ==== 主线任务完成数（如果多次完成也算1次）
	[InspectorName("STAGE_GOAL_DONE_STEP - idx[19] - 阶段目标已完成阶段")]
	STAGE_GOAL_DONE_STEP, //19 ==== 阶段目标已完成阶段
	[InspectorName("INN_POPULARITY - idx[20] - 旅店人气值")]
	INN_POPULARITY, //20 ==== 旅店人气值
	[InspectorName("INN_LEVEL - idx[21] - 旅店等级")]
	INN_LEVEL, //21 ==== 旅店等级
	[InspectorName("TOWER_PASSED_CHAPTER - idx[22] - 爬塔当前通关的章节")]
	TOWER_PASSED_CHAPTER, //22 ==== 爬塔当前通关的章节
	[InspectorName("INN_HAD_UNLOCK_DISH_NUM - idx[23] - 旅店已解锁菜品数量")]
	INN_HAD_UNLOCK_DISH_NUM, //23 ==== 旅店已解锁菜品数量
	[InspectorName("TOWER_PASSED_LVL - idx[24] - 爬塔当前通关的关卡层数")]
	TOWER_PASSED_LVL, //24 ==== 爬塔当前通关的关卡层数
	[InspectorName("INN_STATION_TOTAL_LEVEL - idx[25] - 旅店设施总等级")]
	INN_STATION_TOTAL_LEVEL, //25 ==== 旅店设施总等级
	[InspectorName("INN_HAD_RECEIVE_GUEST_COUNT - idx[26] - 旅店已接待客人数量")]
	INN_HAD_RECEIVE_GUEST_COUNT, //26 ==== 旅店已接待客人数量
	[InspectorName("TREASURE_HUNT_STATION_LEVEL - idx[27] - 太空寻宝太空舱等级")]
	TREASURE_HUNT_STATION_LEVEL, //27 ==== 太空寻宝太空舱等级
	[InspectorName("TREASURE_HUNT_HAD_GAIN_ORE_TYPE_COUNT - idx[28] - 太空寻宝已收集矿石类型数量")]
	TREASURE_HUNT_HAD_GAIN_ORE_TYPE_COUNT, //28 ==== 太空寻宝已收集矿石类型数量
	[InspectorName("TREASURE_HUNT_HAD_GAIN_TREASURE_TYPE_COUNT - idx[29] - 太空寻宝已收集奇物类型数量")]
	TREASURE_HUNT_HAD_GAIN_TREASURE_TYPE_COUNT, //29 ==== 太空寻宝已收集奇物类型数量
	[InspectorName("TREASURE_HUNT_HAD_COLLECT_COMPOSITE_COUNT - idx[30] - 太空寻宝已集齐组合数量")]
	TREASURE_HUNT_HAD_COLLECT_COMPOSITE_COUNT, //30 ==== 太空寻宝已集齐组合数量
	[InspectorName("NAMED_CHILD_NUM - idx[31] - 已命名未成年子嗣数量")]
	NAMED_CHILD_NUM, //31 ==== 已命名未成年子嗣数量
	[InspectorName("MUSEUM_ITEM_NUM - idx[32] - 已获得珍宝数量")]
	MUSEUM_ITEM_NUM, //32 ==== 已获得珍宝数量
	[InspectorName("CHAPTER_ID - idx[33] - 当前章节ID")]
	CHAPTER_ID, //33 ==== 当前章节ID
	[InspectorName("GUILD_LEVEL - idx[34] - 所在联盟等级")]
	GUILD_LEVEL, //34 ==== 所在联盟等级
	[InspectorName("UNMARRY_ADULT_SUM - idx[35] - 未婚成年子嗣数量")]
	UNMARRY_ADULT_SUM, //35 ==== 未婚成年子嗣数量
	[InspectorName("MARS_EXPLORER_NUM - idx[36] - 火星探索次数")]
	MARS_EXPLORER_NUM, //36 ==== 火星探索次数
	[InspectorName("TOWER_ACTIVE_RESEARCH_COUNT - idx[37] - 爬塔已激活研究章节数量")]
	TOWER_ACTIVE_RESEARCH_COUNT, //37 ==== 爬塔已激活研究章节数量
	[InspectorName("MARS_BUILDING_EQUIP_LVL_SUM - idx[38] - 火星所有建筑所有部件总等级")]
	MARS_BUILDING_EQUIP_LVL_SUM, //38 ==== 火星所有建筑所有部件总等级
	[InspectorName("MARS_BUILDING_LVL_SUM - idx[39] - 火星所有建筑总等级")]
	MARS_BUILDING_LVL_SUM, //39 ==== 火星所有建筑总等级
	[InspectorName("INN_MEDAL_LEVEL - idx[40] - 旅店奖牌等级")]
	INN_MEDAL_LEVEL, //40 ==== 旅店奖牌等级
	[InspectorName("TOTAL_GAIN_GOLD_COUNT - idx[41] - 获得的金币总数量")]
	TOTAL_GAIN_GOLD_COUNT, //41 ==== 获得的金币总数量
}

public class ENPPlayerValueTypeComparer : IEqualityComparer<ENPPlayerValueType>{
	public bool Equals(ENPPlayerValueType x, ENPPlayerValueType y) { return x == y; }
	public int GetHashCode(ENPPlayerValueType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 42;
}
}

