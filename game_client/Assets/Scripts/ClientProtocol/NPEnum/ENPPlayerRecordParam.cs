using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家记录枚举
/// </summary>
public enum ENPPlayerRecordParam {
	[InspectorName("NONE - idx[0] - 无效默认值")]
	NONE, //0 ==== 无效默认值
	[InspectorName("HERO_BUSINESS_SKILL_UPGRADE_TIMES - idx[1] - 伙伴累计提升经营技能次数")]
	HERO_BUSINESS_SKILL_UPGRADE_TIMES, //1 ==== 伙伴累计提升经营技能次数
	[InspectorName("HERO_PLACE_TO_BUILDING_TIME - idx[2] - 向建筑累计委任伙伴次数")]
	HERO_PLACE_TO_BUILDING_TIME, //2 ==== 向建筑累计委任伙伴次数
	[InspectorName("MAX_SERVER_MAIL_ID - idx[3] - 已处理最大全服邮件id")]
	MAX_SERVER_MAIL_ID, //3 ==== 已处理最大全服邮件id
	[InspectorName("EQUIP_RESHARP_ACTIVE_TIMES - idx[4] - 藏品重塑激活次数")]
	EQUIP_RESHARP_ACTIVE_TIMES, //4 ==== 藏品重塑激活次数
	[InspectorName("ARENA_STATION_COLLECT_TIMES - idx[5] - 领取竞技场收益次数")]
	ARENA_STATION_COLLECT_TIMES, //5 ==== 领取竞技场收益次数
	[InspectorName("ARENA_DEFEAT_HERO_COUNT - idx[6] - 竞技场累计击败大臣数量")]
	ARENA_DEFEAT_HERO_COUNT, //6 ==== 竞技场累计击败大臣数量
	[InspectorName("HERO_WEAR_EQUIP_TIMES - idx[7] - 大臣穿戴藏品次数")]
	HERO_WEAR_EQUIP_TIMES, //7 ==== 大臣穿戴藏品次数
	[InspectorName("CONSORT_SEND_GIFT_TIMES - idx[8] - 妃子赠送礼物次数")]
	CONSORT_SEND_GIFT_TIMES, //8 ==== 妃子赠送礼物次数
	[InspectorName("CONSORT_CALL_TIMES - idx[9] - 妃子指定宠幸次数")]
	CONSORT_CALL_TIMES, //9 ==== 妃子指定宠幸次数
	[InspectorName("CONSORT_RND_CALL_TIMES - idx[10] - 妃子随机宠幸次数")]
	CONSORT_RND_CALL_TIMES, //10 ==== 妃子随机宠幸次数
	[InspectorName("GAIN_EQUIP_TIMES - idx[11] - 获得藏品次数")]
	GAIN_EQUIP_TIMES, //11 ==== 获得藏品次数
	[InspectorName("EQUIP_UPGRADE_TIMES - idx[12] - 藏品升级次数")]
	EQUIP_UPGRADE_TIMES, //12 ==== 藏品升级次数
	[InspectorName("CHILD_GAIN_NUM - idx[13] - 子嗣获得数量")]
	CHILD_GAIN_NUM, //13 ==== 子嗣获得数量
	[InspectorName("CHILD_TRAIN_TIMES - idx[14] - 子嗣指导次数")]
	CHILD_TRAIN_TIMES, //14 ==== 子嗣指导次数
	[InspectorName("FRAM_COLLECT - idx[15] - 农场收获次数")]
	FRAM_COLLECT, //15 ==== 农场收获次数
	[InspectorName("CONSORT_BUSINESS_SKILL_UPGRADE_TIMES - idx[16] - 累计领悟家人经营技能次数")]
	CONSORT_BUSINESS_SKILL_UPGRADE_TIMES, //16 ==== 累计领悟家人经营技能次数
	[InspectorName("MARRIED_COUNT - idx[17] - 联姻次数")]
	MARRIED_COUNT, //17 ==== 联姻次数
	[InspectorName("START_DINNER_COUNT - idx[18] - 举办宴会次数")]
	START_DINNER_COUNT, //18 ==== 举办宴会次数
	[InspectorName("JOIN_DINNER_COUNT - idx[19] - 参加宴会次数")]
	JOIN_DINNER_COUNT, //19 ==== 参加宴会次数
	[InspectorName("FRIEND_NUM_MAX_RECORD - idx[20] - 玩家好友数量历史记录最高值")]
	FRIEND_NUM_MAX_RECORD, //20 ==== 玩家好友数量历史记录最高值
	[InspectorName("CONSORT_BLESSING_SKILL_UPGRADE_TIMES - idx[21] - 妃子加护技能升级次数")]
	CONSORT_BLESSING_SKILL_UPGRADE_TIMES, //21 ==== 妃子加护技能升级次数
	[InspectorName("FINISH_DAILY_QUEST - idx[22] - 完成日常任务")]
	FINISH_DAILY_QUEST, //22 ==== 完成日常任务
	[InspectorName("ADULT_GAIN_NUM - idx[23] - 子嗣毕业次数")]
	ADULT_GAIN_NUM, //23 ==== 子嗣毕业次数
	[InspectorName("ANECDOTE_DEAL_COUNT - idx[24] - 政务处理数量")]
	ANECDOTE_DEAL_COUNT, //24 ==== 政务处理数量
	[InspectorName("TRAVEL_COUNT - idx[25] - 游历数量")]
	TRAVEL_COUNT, //25 ==== 游历数量
	[InspectorName("CHAPTER_PASS_STAGE_COUNT - idx[26] - 章节通过关卡数量")]
	CHAPTER_PASS_STAGE_COUNT, //26 ==== 章节通过关卡数量
	[InspectorName("SHOP_BUY_COUNT - idx[27] - 商店购买商品次数")]
	SHOP_BUY_COUNT, //27 ==== 商店购买商品次数
	[InspectorName("ARENA_MAX_DEFEAT_HERO_NUM - idx[28] - 竞技场单场最大击败大臣数量")]
	ARENA_MAX_DEFEAT_HERO_NUM, //28 ==== 竞技场单场最大击败大臣数量
	[InspectorName("PAID_GEM_COUNT - idx[29] - 付费钻石数量")]
	PAID_GEM_COUNT, //29 ==== 付费钻石数量
	[InspectorName("PAID_VOUCHER_COUNT - idx[30] - 付费代金券数量")]
	PAID_VOUCHER_COUNT, //30 ==== 付费代金券数量
	[InspectorName("DEAL_HERO_RECOMMEND - idx[31] - 处理大臣推荐")]
	DEAL_HERO_RECOMMEND, //31 ==== 处理大臣推荐
	[InspectorName("ADD_FRIEND - idx[32] - 添加好友")]
	ADD_FRIEND, //32 ==== 添加好友
	[InspectorName("SEND_CHAT_TO_ROOM - idx[33] - 聊天频道发言")]
	SEND_CHAT_TO_ROOM, //33 ==== 聊天频道发言
	[InspectorName("EQUIP_RESHARP_TIMES - idx[34] - 藏品重塑次数")]
	EQUIP_RESHARP_TIMES, //34 ==== 藏品重塑次数
	[InspectorName("RANK_FIXED_LIKE - idx[35] - 常驻排行榜点赞")]
	RANK_FIXED_LIKE, //35 ==== 常驻排行榜点赞
	[InspectorName("AVATAR_GACHA_ROLL - idx[36] - 时装抽卡")]
	AVATAR_GACHA_ROLL, //36 ==== 时装抽卡
	[InspectorName("UNLOCK_NEW_EQUIP_TYPE_COUNT - idx[37] - 解锁藏品种类数量")]
	UNLOCK_NEW_EQUIP_TYPE_COUNT, //37 ==== 解锁藏品种类数量
	[InspectorName("MAX_EARNINGS_CHILD - idx[38] - 赚速最高子嗣")]
	MAX_EARNINGS_CHILD, //38 ==== 赚速最高子嗣
	[InspectorName("ARENA_BEEN_DEFEAT_HERO_COUNT - idx[39] - 竞技场累计被击败大臣数量")]
	ARENA_BEEN_DEFEAT_HERO_COUNT, //39 ==== 竞技场累计被击败大臣数量
	[InspectorName("JOIN_GUILD_TIMES - idx[40] - 加入联盟次数")]
	JOIN_GUILD_TIMES, //40 ==== 加入联盟次数
	[InspectorName("ARENA_ATTACK_TIMES - idx[41] - 竞技场挑战次数")]
	ARENA_ATTACK_TIMES, //41 ==== 竞技场挑战次数
	[InspectorName("PLAYER_CONSORT_INTIMACY - idx[42] - 玩家妃子亲密度记录")]
	PLAYER_CONSORT_INTIMACY, //42 ==== 玩家妃子亲密度记录
	[InspectorName("PLAYER_CONSORT_CHARM - idx[43] - 玩家妃子加护值记录")]
	PLAYER_CONSORT_CHARM, //43 ==== 玩家妃子加护值记录
	[InspectorName("PLAYER_CONSORT_INTIMACY_ADD - idx[44] - 玩家妃子亲密度增加记录（不包括初始值）")]
	PLAYER_CONSORT_INTIMACY_ADD, //44 ==== 玩家妃子亲密度增加记录（不包括初始值）
	[InspectorName("PLAYER_CONSORT_CHARM_ADD - idx[45] - 玩家妃子加护值增加记录（不包括初始值）")]
	PLAYER_CONSORT_CHARM_ADD, //45 ==== 玩家妃子加护值增加记录（不包括初始值）
	[InspectorName("FRAM_COLLECT_SUM - idx[46] - 农场收获金币数量")]
	FRAM_COLLECT_SUM, //46 ==== 农场收获金币数量
	[InspectorName("ARENA_SELECT_ATTACK_TIME - idx[47] - 竞技场选择攻击次数")]
	ARENA_SELECT_ATTACK_TIME, //47 ==== 竞技场选择攻击次数
	[InspectorName("GUILD_CONSTRUCT_TIMES - idx[48] - 联盟建设次数")]
	GUILD_CONSTRUCT_TIMES, //48 ==== 联盟建设次数
	[InspectorName("GUILD_DEAL_ENTRUST_TIMES - idx[49] - 联盟处理委托次数")]
	GUILD_DEAL_ENTRUST_TIMES, //49 ==== 联盟处理委托次数
	[InspectorName("TOWER_FIGHT_SUCCESS_TIMES - idx[50] - 迷宫挑战成功次数")]
	TOWER_FIGHT_SUCCESS_TIMES, //50 ==== 迷宫挑战成功次数
	[InspectorName("HERO_IMPROVE_STEP_TIMES - idx[51] - 大臣提升阶数次数")]
	HERO_IMPROVE_STEP_TIMES, //51 ==== 大臣提升阶数次数
	[InspectorName("TREASURE_HUNT_CAPTURE_TIMES - idx[52] - 太空寻宝累计捕捉次数")]
	TREASURE_HUNT_CAPTURE_TIMES, //52 ==== 太空寻宝累计捕捉次数
	[InspectorName("DRAW_ACHIEVE_REWARD_TIMES - idx[53] - 领取成就奖励次数")]
	DRAW_ACHIEVE_REWARD_TIMES, //53 ==== 领取成就奖励次数
	[InspectorName("DEAL_HIRE_EMPLOYEE_PLOT - idx[54] - 雇佣员工剧情")]
	DEAL_HIRE_EMPLOYEE_PLOT, //54 ==== 雇佣员工剧情
	[InspectorName("GAIN_GIFTED_CHILD_COUNT - idx[55] - 获得卷王子嗣数量")]
	GAIN_GIFTED_CHILD_COUNT, //55 ==== 获得卷王子嗣数量
	[InspectorName("INN_DISH_UPGRADE_TIMES - idx[56] - 旅店菜品升级次数")]
	INN_DISH_UPGRADE_TIMES, //56 ==== 旅店菜品升级次数
	[InspectorName("TREASURE_HUNT_UPGRADE_ORE_NORMAL_SKILL_TIMES - idx[57] - 太空寻宝升级矿石普通技能次数")]
	TREASURE_HUNT_UPGRADE_ORE_NORMAL_SKILL_TIMES, //57 ==== 太空寻宝升级矿石普通技能次数
	[InspectorName("TREASURE_HUNT_UPGRADE_ORE_ADVANCED_SKILL_TIMES - idx[58] - 太空寻宝升级矿石高级技能次数")]
	TREASURE_HUNT_UPGRADE_ORE_ADVANCED_SKILL_TIMES, //58 ==== 太空寻宝升级矿石高级技能次数
	[InspectorName("GUILD_COOPERATE_DRAW_RECORD_LAST_REFRESH_TIME_MS - idx[59] - 联盟协作领取记录记录最后刷新时间 毫秒时间戳")]
	GUILD_COOPERATE_DRAW_RECORD_LAST_REFRESH_TIME_MS, //59 ==== 联盟协作领取记录记录最后刷新时间 毫秒时间戳
	[InspectorName("MAIL_PLAN_HAD_SEND_DAYS - idx[60] - 邮件计划已发送天数")]
	MAIL_PLAN_HAD_SEND_DAYS, //60 ==== 邮件计划已发送天数
	[InspectorName("EVENING_DUNGEON_BOSS_KILL_COUNT - idx[61] - 累计击杀晚间副本boss次数")]
	EVENING_DUNGEON_BOSS_KILL_COUNT, //61 ==== 累计击杀晚间副本boss次数
	[InspectorName("MARS_MAX_POWER - idx[62] - 火星实力记录")]
	MARS_MAX_POWER, //62 ==== 火星实力记录
	[InspectorName("MARS_TEAM_MAX_POWER - idx[63] - 火星所有队伍实力记录（系数general.mars_explore_team_power_coef计算后再放入，只用于计算火星实力）")]
	MARS_TEAM_MAX_POWER, //63 ==== 火星所有队伍实力记录（系数general.mars_explore_team_power_coef计算后再放入，只用于计算火星实力）
	[InspectorName("MARS_EXPLORER_ATTACK_BOSS - idx[64] - 火星探险-击败boss次数")]
	MARS_EXPLORER_ATTACK_BOSS, //64 ==== 火星探险-击败boss次数
	[InspectorName("MARS_EXPLORER_OCCUPY_MINE_NUM - idx[65] - 火星探险-成功占领火星矿次数")]
	MARS_EXPLORER_OCCUPY_MINE_NUM, //65 ==== 火星探险-成功占领火星矿次数
	[InspectorName("MARS_BUILDING_MAX_POWER - idx[66] - 火星所有建筑实力记录")]
	MARS_BUILDING_MAX_POWER, //66 ==== 火星所有建筑实力记录
	[InspectorName("MARS_TECH_MAX_POWER - idx[67] - 火星所有科技实力记录")]
	MARS_TECH_MAX_POWER, //67 ==== 火星所有科技实力记录
	[InspectorName("MARS_BUILDING_EQUIP_UP_NUM - idx[68] - 火星所有建筑部件升级次数")]
	MARS_BUILDING_EQUIP_UP_NUM, //68 ==== 火星所有建筑部件升级次数
	[InspectorName("MARS_PEOPLE_IMMIGRATION_NUM - idx[69] - 火星移民次数")]
	MARS_PEOPLE_IMMIGRATION_NUM, //69 ==== 火星移民次数
	[InspectorName("MARS_BUILDING_TEMP_QUEUE_TIMES - idx[70] - 火星建筑临时队列创建次数")]
	MARS_BUILDING_TEMP_QUEUE_TIMES, //70 ==== 火星建筑临时队列创建次数
	[InspectorName("MARS_BUILDING_DISPATCH_NUM - idx[71] - 火星探险-火星建筑派遣居民数量历史记录")]
	MARS_BUILDING_DISPATCH_NUM, //71 ==== 火星探险-火星建筑派遣居民数量历史记录
	[InspectorName("MARS_PEOPLE_IMMIGRATION_SUM - idx[72] - 火星移民数量")]
	MARS_PEOPLE_IMMIGRATION_SUM, //72 ==== 火星移民数量
	[InspectorName("MARS_TIME_REDUCED_MIN_SUM - idx[73] - 火星累计使用道具加速时长之和（分钟）")]
	MARS_TIME_REDUCED_MIN_SUM, //73 ==== 火星累计使用道具加速时长之和（分钟）
	[InspectorName("MARS_TECH_TIME_REDUCED_MIN_SUM - idx[74] - 火星研究科技累计使用道具加速之和（分钟）")]
	MARS_TECH_TIME_REDUCED_MIN_SUM, //74 ==== 火星研究科技累计使用道具加速之和（分钟）
	[InspectorName("MARS_BUILD_TIME_REDUCED_MIN_SUM - idx[75] - 火星建筑升级累计使用道具加速之和（分钟）")]
	MARS_BUILD_TIME_REDUCED_MIN_SUM, //75 ==== 火星建筑升级累计使用道具加速之和（分钟）
	[InspectorName("MARS_GET_ENERGY_COUNT - idx[76] - 火星收取能源次数")]
	MARS_GET_ENERGY_COUNT, //76 ==== 火星收取能源次数
	[InspectorName("MARS_TEAM_COLLECT_SUM - idx[77] - 火星矿场采集资源量")]
	MARS_TEAM_COLLECT_SUM, //77 ==== 火星矿场采集资源量
	[InspectorName("TREASURE_HUNT_ACTIVE_ORE_SKILL_TIMES - idx[78] - 太空寻宝激活矿石技能次数")]
	TREASURE_HUNT_ACTIVE_ORE_SKILL_TIMES, //78 ==== 太空寻宝激活矿石技能次数
	[InspectorName("MARS_TEAM_REPAIR_TIME_REDUCED_MIN_SUM - idx[79] - 火星队伍修复累计使用道具加速之和（分钟）")]
	MARS_TEAM_REPAIR_TIME_REDUCED_MIN_SUM, //79 ==== 火星队伍修复累计使用道具加速之和（分钟）
	[InspectorName("TUTORIAL_DATA_MIGRATED - idx[80] - 引导数据旧版本迁移已完成（仅服务端使用）")]
	TUTORIAL_DATA_MIGRATED, //80 ==== 引导数据旧版本迁移已完成（仅服务端使用）
}

public class ENPPlayerRecordParamComparer : IEqualityComparer<ENPPlayerRecordParam>{
	public bool Equals(ENPPlayerRecordParam x, ENPPlayerRecordParam y) { return x == y; }
	public int GetHashCode(ENPPlayerRecordParam obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 81;
}
}

