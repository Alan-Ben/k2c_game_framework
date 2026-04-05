using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家条件类型
/// </summary>
public enum ENPPlayerConditionType {
	NONE, //0 ==== 
	[InspectorName("UNUSE_S_LOGIC_EVENT - idx[1] - 废除 监听到事件触发 S_LOGIC_EVENT:事件名称:事件参数:n:m")]
	UNUSE_S_LOGIC_EVENT, //1 ==== 废除 监听到事件触发 S_LOGIC_EVENT:事件名称:事件参数:n:m
	[InspectorName("CS_VALUE - idx[2] - 值范围 CS_VALUE:ENPPlayerValueType:min:max")]
	CS_VALUE, //2 ==== 值范围 CS_VALUE:ENPPlayerValueType:min:max
	[InspectorName("CS_BUF_LAYER - idx[3] - BUFF层级 CS_BUF_LAYER:buf_id:min:max")]
	CS_BUF_LAYER, //3 ==== BUFF层级 CS_BUF_LAYER:buf_id:min:max
	[InspectorName("CS_HAS_ITEM - idx[4] - 拥有物品  CS_HAS_ITEM:itemType-item_id:min:max")]
	CS_HAS_ITEM, //4 ==== 拥有物品  CS_HAS_ITEM:itemType-item_id:min:max
	[InspectorName("CS_HAS_BUILDING - idx[5] - 玩家已放置建筑 CS_HAS_BUILDING:building_id")]
	CS_HAS_BUILDING, //5 ==== 玩家已放置建筑 CS_HAS_BUILDING:building_id
	[InspectorName("CS_JUD_SIM_UNLOCK - idx[6] - 判断简要解锁的解锁信息 CS_JUD_SIM_UNLOCK:sim_unlock_id")]
	CS_JUD_SIM_UNLOCK, //6 ==== 判断简要解锁的解锁信息 CS_JUD_SIM_UNLOCK:sim_unlock_id
	[InspectorName("CS_SPECIAL - idx[7] - 特殊判断条件 CS_SPECIAL:ENPPlayer_CS_SpecialCondition")]
	CS_SPECIAL, //7 ==== 特殊判断条件 CS_SPECIAL:ENPPlayer_CS_SpecialCondition
	[InspectorName("C_ID_JUDGE - idx[8] - 针对Id进行判断 C_ID_JUDGE:EPlayer_C_IdJudgeFunc:id")]
	C_ID_JUDGE, //8 ==== 针对Id进行判断 C_ID_JUDGE:EPlayer_C_IdJudgeFunc:id
	[InspectorName("CS_ID_JUDGE - idx[9] - 针对Id进行判断 CS_ID_JUDGE:ENPPlayer_CS_IdJudgeFunc:id")]
	CS_ID_JUDGE, //9 ==== 针对Id进行判断 CS_ID_JUDGE:ENPPlayer_CS_IdJudgeFunc:id
	[InspectorName("CS_RECORD_PARAM - idx[10] - 玩家计数 CS_RECORD_PARAM:ENPPlayerRecordParam:min_value（:max_value）")]
	CS_RECORD_PARAM, //10 ==== 玩家计数 CS_RECORD_PARAM:ENPPlayerRecordParam:min_value（:max_value）
	[InspectorName("CS_QUEST_COUNT - idx[11] - 任务完成次数 CS_QUEST_COUNT:quest_id:min_value（:max_value）")]
	CS_QUEST_COUNT, //11 ==== 任务完成次数 CS_QUEST_COUNT:quest_id:min_value（:max_value）
	[InspectorName("CS_QUEST_DOING - idx[12] - 正在进行的任务 CS_QUEST_DOING:quest_id（:quest_step_ID:quest_target_ID）")]
	CS_QUEST_DOING, //12 ==== 正在进行的任务 CS_QUEST_DOING:quest_id（:quest_step_ID:quest_target_ID）
	[InspectorName("CS_EVENT_RECORD - idx[13] - 玩家事件记录计数 CS_EVENT_RECORD:ENPPlayerEventRecordType:sub_id:min_value（:max_value），sub_id取-1表示所有子计数之和注意，sub_id使用负数时需要用括号括起来，否则会被当成分隔符无法读取例： '（-1）'")]
	CS_EVENT_RECORD, //13 ==== 玩家事件记录计数 CS_EVENT_RECORD:ENPPlayerEventRecordType:sub_id:min_value（:max_value），sub_id取-1表示所有子计数之和注意，sub_id使用负数时需要用括号括起来，否则会被当成分隔符无法读取例： '（-1）'
	[InspectorName("C_GUILD_SELF_HAVE_PERMISSION - idx[14] - 联盟自己的职位是否拥有对应权限 C_GUILD_SELF_HAVE_PERMISSION:EGuildPermissionType")]
	C_GUILD_SELF_HAVE_PERMISSION, //14 ==== 联盟自己的职位是否拥有对应权限 C_GUILD_SELF_HAVE_PERMISSION:EGuildPermissionType
	[InspectorName("C_HAS_TREASURE_MAP - idx[15] - 拥有藏宝图 C_HAS_TREASURE_MAP（:id配置id）")]
	C_HAS_TREASURE_MAP, //15 ==== 拥有藏宝图 C_HAS_TREASURE_MAP（:id配置id）
	[InspectorName("CS_QUEST_STEP_IS_DONE - idx[16] - 是否完成任务 CS_QUEST_STEP_IS_DONE:quest_id:step_id")]
	CS_QUEST_STEP_IS_DONE, //16 ==== 是否完成任务 CS_QUEST_STEP_IS_DONE:quest_id:step_id
	[InspectorName("CS_VARIABLE - idx[17] - 某高级公式的值范围 CS_VARIABLE:min:max:高级公式")]
	CS_VARIABLE, //17 ==== 某高级公式的值范围 CS_VARIABLE:min:max:高级公式
	[InspectorName("C_PLAYER_CAN_LEVEL_UP - idx[18] - 玩家可以升级 C_PLAYER_CAN_LEVEL_UP")]
	C_PLAYER_CAN_LEVEL_UP, //18 ==== 玩家可以升级 C_PLAYER_CAN_LEVEL_UP
	[InspectorName("C_PLAYER_IN_SPACE_POS - idx[19] - 玩家在大地图指定范围 C_PLAYER_IN_SPACE_POS:mapid（地图id）:位置（Vector3）:范围（float）")]
	C_PLAYER_IN_SPACE_POS, //19 ==== 玩家在大地图指定范围 C_PLAYER_IN_SPACE_POS:mapid（地图id）:位置（Vector3）:范围（float）
	[InspectorName("CS_HAS_HERO - idx[20] - 拥有指定大臣中的n个 CS_HAS_HERO:hero1&hero2:（n）")]
	CS_HAS_HERO, //20 ==== 拥有指定大臣中的n个 CS_HAS_HERO:hero1&hero2:（n）
	[InspectorName("CS_BUILDING_FUNC_LEVEL - idx[21] - 玩家已放置建筑功能等级 CS_BUILDING_FUNC_LEVEL:building_id:EBuildingFuncEnum（:minLevel:maxLevel）")]
	CS_BUILDING_FUNC_LEVEL, //21 ==== 玩家已放置建筑功能等级 CS_BUILDING_FUNC_LEVEL:building_id:EBuildingFuncEnum（:minLevel:maxLevel）
	[InspectorName("CS_CHAPTER_STAGE_PASSED - idx[22] - 章节是否通过指定关卡 CS_CHAPTER_STAGE_PASSED:stageId")]
	CS_CHAPTER_STAGE_PASSED, //22 ==== 章节是否通过指定关卡 CS_CHAPTER_STAGE_PASSED:stageId
	[InspectorName("CS_CONSORT_SKILL - idx[23] - 情人技能解锁检查 CS_CONSORT_SKILL:情人ID:技能ID:最小等级（:最大等级 默认-1）")]
	CS_CONSORT_SKILL, //23 ==== 情人技能解锁检查 CS_CONSORT_SKILL:情人ID:技能ID:最小等级（:最大等级 默认-1）
	[InspectorName("C_FUNC_UNLOCK_IS_SHOW - idx[24] - 系统解锁弹窗是否已经展示过 C_FUNC_UNLOCK_IS_SHOW:ENPFunctionType")]
	C_FUNC_UNLOCK_IS_SHOW, //24 ==== 系统解锁弹窗是否已经展示过 C_FUNC_UNLOCK_IS_SHOW:ENPFunctionType
	[InspectorName("CS_HERO_REACH_STAR_NUM - idx[25] - 达到指定星级大臣数量 CS_HERO_REACH_STAR_NUM:星级:min（:max）")]
	CS_HERO_REACH_STAR_NUM, //25 ==== 达到指定星级大臣数量 CS_HERO_REACH_STAR_NUM:星级:min（:max）
	[InspectorName("CS_LEVY_SUM - idx[26] - 征收累计数量 CS_LEVY_SUM:ELevy_Type:min（:max）")]
	CS_LEVY_SUM, //26 ==== 征收累计数量 CS_LEVY_SUM:ELevy_Type:min（:max）
	[InspectorName("C_ACHIEVE_IS_DONE - idx[27] - 成就是否已经全部完成领奖 C_ACHIEVE_IS_DONE:achieve_id")]
	C_ACHIEVE_IS_DONE, //27 ==== 成就是否已经全部完成领奖 C_ACHIEVE_IS_DONE:achieve_id
	[InspectorName("C_WEEK_CARD_STAT - idx[28] - 周卡状态是否满足 C_WEEK_CARD_STAT:stat（EWeekCardStat）")]
	C_WEEK_CARD_STAT, //28 ==== 周卡状态是否满足 C_WEEK_CARD_STAT:stat（EWeekCardStat）
	[InspectorName("C_ADD_PACK_NEED_DOWNLOAD - idx[29] - 是否有增量包需要下载 C_ADD_PACK_NEED_DOWNLOAD")]
	C_ADD_PACK_NEED_DOWNLOAD, //29 ==== 是否有增量包需要下载 C_ADD_PACK_NEED_DOWNLOAD
	[InspectorName("C_CHAPTER_BLOCK_EVENT_DEAL_DONE - idx[30] - 关卡格子上事件是否处理完成 C_CHAPTER_BLOCK_EVENT_DEAL_DONE:关卡格子配表id chapter_stage_block_id （:挂起事件是否视为事件已经完成bool, 默认为true）")]
	C_CHAPTER_BLOCK_EVENT_DEAL_DONE, //30 ==== 关卡格子上事件是否处理完成 C_CHAPTER_BLOCK_EVENT_DEAL_DONE:关卡格子配表id chapter_stage_block_id （:挂起事件是否视为事件已经完成bool, 默认为true）
	[InspectorName("C_CLOTHES_HANDBOOK_CAN_REWARD - idx[31] - 图鉴是否可以领奖 C_CLOTHES_HANDBOOK_CAN_REWARD:TRUE（或者FALSE）")]
	C_CLOTHES_HANDBOOK_CAN_REWARD, //31 ==== 图鉴是否可以领奖 C_CLOTHES_HANDBOOK_CAN_REWARD:TRUE（或者FALSE）
	[InspectorName("C_CONSORT_INTIMACY_STEP_REACHED - idx[32] - 妃子亲密度阶段是否达到 C_CONSORT_INTIMACY_STEP_REACHED:妃子consort_id:阶段intimacy_step")]
	C_CONSORT_INTIMACY_STEP_REACHED, //32 ==== 妃子亲密度阶段是否达到 C_CONSORT_INTIMACY_STEP_REACHED:妃子consort_id:阶段intimacy_step
	[InspectorName("CS_CONSORT_RES_COUNT - idx[33] - 指定家人指定资源数量  CS_CONSORT_RES_COUNT:家人ID:EBagItemUse_ConsortDrawShowType:min:max")]
	CS_CONSORT_RES_COUNT, //33 ==== 指定家人指定资源数量  CS_CONSORT_RES_COUNT:家人ID:EBagItemUse_ConsortDrawShowType:min:max
	[InspectorName("CS_CONSORT_LIKE_COUNT - idx[34] - 指定家人（未获得）好感度数值  CS_CONSORT_LIKE_COUNT:家人ID:min:max")]
	CS_CONSORT_LIKE_COUNT, //34 ==== 指定家人（未获得）好感度数值  CS_CONSORT_LIKE_COUNT:家人ID:min:max
	[InspectorName("C_HAS_OWNER_DINNER_REWARD - idx[35] - 是否有自身宴会奖励未领取 C_HAS_OWNER_DINNER_REWARD")]
	C_HAS_OWNER_DINNER_REWARD, //35 ==== 是否有自身宴会奖励未领取 C_HAS_OWNER_DINNER_REWARD
	[InspectorName("C_EVENING_DUNGEON_ACTIVITY_STATE - idx[36] - 晚间副本活动状态 C_EVENING_DUNGEON_ACTIVITY_STATE:（EEveningDungeonActivityState:PREVIEW:ONGOING:END:CLOSE）")]
	C_EVENING_DUNGEON_ACTIVITY_STATE, //36 ==== 晚间副本活动状态 C_EVENING_DUNGEON_ACTIVITY_STATE:（EEveningDungeonActivityState:PREVIEW:ONGOING:END:CLOSE）
	[InspectorName("C_HOTFIX_CONDITION - idx[37] - 热更条件类型 C_HOTFIX_CONDITION:热更条件类型:参数")]
	C_HOTFIX_CONDITION, //37 ==== 热更条件类型 C_HOTFIX_CONDITION:热更条件类型:参数
	[InspectorName("C_MIDDAY_DUNGEON_ACTIVITY_STATE - idx[38] - 午间副本活动状态 C_MIDDAY_DUNGEON_ACTIVITY_STATE:（EMiddayDungeonActivityState:PREVIEW:ONGOING:END）")]
	C_MIDDAY_DUNGEON_ACTIVITY_STATE, //38 ==== 午间副本活动状态 C_MIDDAY_DUNGEON_ACTIVITY_STATE:（EMiddayDungeonActivityState:PREVIEW:ONGOING:END）
	[InspectorName("C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT - idx[39] - 七天登录已领取奖励数量 C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT:int")]
	C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT, //39 ==== 七天登录已领取奖励数量 C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT:int
	[InspectorName("CS_HAS_CONSORT - idx[40] - 拥有指定妃子中的n个 CS_HAS_CONSORT:consort1&consort2:（n）")]
	CS_HAS_CONSORT, //40 ==== 拥有指定妃子中的n个 CS_HAS_CONSORT:consort1&consort2:（n）
	[InspectorName("C_ACTIVITY_STATE - idx[41] - 活动状态 C_ACTIVITY_STATE:EActivityState:activityId")]
	C_ACTIVITY_STATE, //41 ==== 活动状态 C_ACTIVITY_STATE:EActivityState:activityId
	[InspectorName("CS_HAD_UNLOCK_INN_DISH - idx[42] - 是否解锁旅店菜品 CS_HAD_UNLOCK_INN_DISH:菜品id")]
	CS_HAD_UNLOCK_INN_DISH, //42 ==== 是否解锁旅店菜品 CS_HAD_UNLOCK_INN_DISH:菜品id
	[InspectorName("CS_HAD_UNLOCK_BUILDING_PRODUCT - idx[43] - 是否解锁建筑产品 CS_HAD_UNLOCK_BUILDING_PRODUCT:建筑id:产品id")]
	CS_HAD_UNLOCK_BUILDING_PRODUCT, //43 ==== 是否解锁建筑产品 CS_HAD_UNLOCK_BUILDING_PRODUCT:建筑id:产品id
	[InspectorName("C_CONSORT_CALL_DIALOG_NOT_SHOW_CG - idx[44] - 妃子邀约对话是否不展示CG")]
	C_CONSORT_CALL_DIALOG_NOT_SHOW_CG, //44 ==== 妃子邀约对话是否不展示CG
	[InspectorName("S_IS_SYSTEM_QUEST_GROUP_DONE - idx[45] - 是否完成系统任务组 S_IS_SYSTEM_QUEST_GROUP_DONE:group_id")]
	S_IS_SYSTEM_QUEST_GROUP_DONE, //45 ==== 是否完成系统任务组 S_IS_SYSTEM_QUEST_GROUP_DONE:group_id
	[InspectorName("CS_MARS_BUILDING_LVL - idx[46] - 火星指定建筑等级判断 CS_MARS_BUILDING_LVL:建筑id:min_value（:max_value）")]
	CS_MARS_BUILDING_LVL, //46 ==== 火星指定建筑等级判断 CS_MARS_BUILDING_LVL:建筑id:min_value（:max_value）
	[InspectorName("CS_MARS_TECH_LVL - idx[47] - 火星指定科技等级判断 CS_MARS_TECH_LVL:科技id:min_value（:max_value）")]
	CS_MARS_TECH_LVL, //47 ==== 火星指定科技等级判断 CS_MARS_TECH_LVL:科技id:min_value（:max_value）
	[InspectorName("CS_CONSORT_UNLOCK_CG_NUM - idx[48] - 解锁妃子CG的数量 CS_CONSORT_UNLOCK_CG_NUM:min_value（:max_value）")]
	CS_CONSORT_UNLOCK_CG_NUM, //48 ==== 解锁妃子CG的数量 CS_CONSORT_UNLOCK_CG_NUM:min_value（:max_value）
	[InspectorName("CS_CHECK_PLAYER_PERMISSION - idx[49] - 检查玩家是否拥有指定权限 CS_CHECK_PLAYER_PERMISSION:权限配置ID")]
	CS_CHECK_PLAYER_PERMISSION, //49 ==== 检查玩家是否拥有指定权限 CS_CHECK_PLAYER_PERMISSION:权限配置ID
	[InspectorName("CS_MARS_PEOPLE_NUM - idx[50] - 火星人口数量判断 CS_MARS_PEOPLE_NUM:min_value（:max_value）")]
	CS_MARS_PEOPLE_NUM, //50 ==== 火星人口数量判断 CS_MARS_PEOPLE_NUM:min_value（:max_value）
	[InspectorName("CS_MARS_IDLE_PEOPLE_NUM - idx[51] - 火星空闲人口数量判断 CS_MARS_IDLE_PEOPLE_NUM:min_value（:max_value）")]
	CS_MARS_IDLE_PEOPLE_NUM, //51 ==== 火星空闲人口数量判断 CS_MARS_IDLE_PEOPLE_NUM:min_value（:max_value）
	[InspectorName("C_LOVER_COLLECT_HAS_TARGET - idx[52] - 情人收集是否有指定目标情人")]
	C_LOVER_COLLECT_HAS_TARGET, //52 ==== 情人收集是否有指定目标情人
	[InspectorName("C_LOVER_COLLECT_HAD_DRAW - idx[53] - 情人收集是否已抽取指定情人")]
	C_LOVER_COLLECT_HAD_DRAW, //53 ==== 情人收集是否已抽取指定情人
	[InspectorName("C_SPECIAL - idx[54] - 客户端特殊判断条件 C_SPECIAL:EClientSpecialConditionType")]
	C_SPECIAL, //54 ==== 客户端特殊判断条件 C_SPECIAL:EClientSpecialConditionType
}

public class ENPPlayerConditionTypeComparer : IEqualityComparer<ENPPlayerConditionType>{
	public bool Equals(ENPPlayerConditionType x, ENPPlayerConditionType y) { return x == y; }
	public int GetHashCode(ENPPlayerConditionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 55;
}
}

