using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家特殊的条件判断
/// </summary>
public enum ENPPlayer_CS_SpecialCondition {
	NONE, //0 ==== 
	[InspectorName("C_IS_IN_TUTORIAL - idx[1] - 是否在引导中")]
	C_IS_IN_TUTORIAL, //1 ==== 是否在引导中
	[InspectorName("C_CITY_SCENE_IS_SHOW - idx[2] - 主城场景是否显示")]
	C_CITY_SCENE_IS_SHOW, //2 ==== 主城场景是否显示
	[InspectorName("C_ROOM_SCENE_IS_SHOW - idx[3] - 卧室场景是否显示")]
	C_ROOM_SCENE_IS_SHOW, //3 ==== 卧室场景是否显示
	[InspectorName("C_CHILD_HAS_UNMARRIED_ADULT - idx[4] - 是否有可以联姻的成年子嗣")]
	C_CHILD_HAS_UNMARRIED_ADULT, //4 ==== 是否有可以联姻的成年子嗣
	[InspectorName("C_CHILD_CAN_TRAIN - idx[5] - 是否有可以培养的子嗣")]
	C_CHILD_CAN_TRAIN, //5 ==== 是否有可以培养的子嗣
	[InspectorName("C_HAVE_ANNOUNCEMENT - idx[6] - 是否拥有运营公告")]
	C_HAVE_ANNOUNCEMENT, //6 ==== 是否拥有运营公告
	[InspectorName("C_HAVE_QUESTIONNAIRE - idx[7] - 是否有问卷调查")]
	C_HAVE_QUESTIONNAIRE, //7 ==== 是否有问卷调查
	[InspectorName("C_HAVE_RANK_RUSH_ACTIVITY - idx[8] - 是否有限时冲榜活动")]
	C_HAVE_RANK_RUSH_ACTIVITY, //8 ==== 是否有限时冲榜活动
	[InspectorName("C_ANECDOTE_IN_EVENT_PROCESS - idx[9] - 是否有经营事件处理中")]
	C_ANECDOTE_IN_EVENT_PROCESS, //9 ==== 是否有经营事件处理中
	[InspectorName("CS_QUEST_ALL_DONE - idx[10] - 所有主线任务完成")]
	CS_QUEST_ALL_DONE, //10 ==== 所有主线任务完成
	[InspectorName("C_FUNCTION_REWARD_ALL_GET - idx[11] - 功能解锁奖励是否全部领取完毕")]
	C_FUNCTION_REWARD_ALL_GET, //11 ==== 功能解锁奖励是否全部领取完毕
	[InspectorName("C_HAVE_STEP_REWARD_ACTIVITY - idx[12] - 是否有阶段奖励活动")]
	C_HAVE_STEP_REWARD_ACTIVITY, //12 ==== 是否有阶段奖励活动
	[InspectorName("C_CONSORT_HAS_UNCLAIMED_CG_REWARD - idx[13] - 是否有未领取的妃子cg奖励")]
	C_CONSORT_HAS_UNCLAIMED_CG_REWARD, //13 ==== 是否有未领取的妃子cg奖励
	[InspectorName("CS_IS_ARRIVE_MARS - idx[14] - 是否已到达火星")]
	CS_IS_ARRIVE_MARS, //14 ==== 是否已到达火星
	[InspectorName("C_FIRST_RECHARGE_REWARD_IS_ALL_GET - idx[15] - 首充奖励是否已经全部领取")]
	C_FIRST_RECHARGE_REWARD_IS_ALL_GET, //15 ==== 首充奖励是否已经全部领取
	[InspectorName("C_HAVE_AVAIABLE_GEM_GIFT_PACK - idx[16] - 是否有可用钻石礼包")]
	C_HAVE_AVAIABLE_GEM_GIFT_PACK, //16 ==== 是否有可用钻石礼包
	[InspectorName("C_CAN_USE_WEB_RECHARGE - idx[17] - 是否可以使用网页充值")]
	C_CAN_USE_WEB_RECHARGE, //17 ==== 是否可以使用网页充值
	[InspectorName("C_STAGE_GOAL_IS_ALL_DONE - idx[18] - 阶段目标是否全部完成")]
	C_STAGE_GOAL_IS_ALL_DONE, //18 ==== 阶段目标是否全部完成
	[InspectorName("C_IS_LANDING_MARS - idx[19] - 是否已经登录火星")]
	C_IS_LANDING_MARS, //19 ==== 是否已经登录火星
	[InspectorName("C_CUR_WND_HERO_CAN_UPGRADE - idx[20] - 当前窗口的顾问是否可以升级")]
	C_CUR_WND_HERO_CAN_UPGRADE, //20 ==== 当前窗口的顾问是否可以升级
	[InspectorName("C_TRAVEL_HAS_EVENT_UNTREATED - idx[21] - 游历中是否有未处理事件")]
	C_TRAVEL_HAS_EVENT_UNTREATED, //21 ==== 游历中是否有未处理事件
	[InspectorName("C_HAVE_COUNT_DOWN_EVENT - idx[22] - 是否有倒计时事件")]
	C_HAVE_COUNT_DOWN_EVENT, //22 ==== 是否有倒计时事件
	[InspectorName("C_COUNT_DOWN_EVENT_CAN_GET_REWARD - idx[23] - 倒计时事件是否可以领取奖励")]
	C_COUNT_DOWN_EVENT_CAN_GET_REWARD, //23 ==== 倒计时事件是否可以领取奖励
	[InspectorName("C_CUR_IS_IN_CHAPTER_BOSS - idx[24] - 当前是否在章节boss战中")]
	C_CUR_IS_IN_CHAPTER_BOSS, //24 ==== 当前是否在章节boss战中
	[InspectorName("C_CUR_CHAPTER_BOSS_POWER_IS_ENOUGH - idx[25] - 当前boss战战力是否足够")]
	C_CUR_CHAPTER_BOSS_POWER_IS_ENOUGH, //25 ==== 当前boss战战力是否足够
}

public class ENPPlayer_CS_SpecialConditionComparer : IEqualityComparer<ENPPlayer_CS_SpecialCondition>{
	public bool Equals(ENPPlayer_CS_SpecialCondition x, ENPPlayer_CS_SpecialCondition y) { return x == y; }
	public int GetHashCode(ENPPlayer_CS_SpecialCondition obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 26;
}
}

