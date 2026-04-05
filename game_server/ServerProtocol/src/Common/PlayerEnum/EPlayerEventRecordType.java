package Common.PlayerEnum;

/*********
 * 玩家行为计数器类型，sub_id取（-1）表示所有子计数器之和，因为每个类型是两层结构，设计时应该避免第二次id的数量过大
 **/
public enum EPlayerEventRecordType {
	NONE, //0 ==== 
	LOGIN_DAY_SUM, //1 ==== 登录天数统计 LOGIN_DAY_SUM（@0），id无意义只能配置为0
	ACHIEVE_POINT_REWARDED, //2 ==== 成就点领取记录 ACHIEVE_POINT_REWARDED@id，id：achieve_step_reward配表id
	STAGE_GOAL_FINISH_TIME_MS, //3 ==== 阶段任务完成时间 STAGE_GOAL_FINISH_TIME_MS@id，id：stage_goal配表id
	BAG_ITEM_SPEND, //4 ==== 累计背包物品消耗，id为bag_item主键ID
	CURRENCY_SPEND, //5 ==== 累计货币物品消耗，id为ECurrency枚举
	GAIN_EQUIP, //6 ==== 累计获得藏品次数，id为藏品id
	DRAW_ACHIEVE_STEP_REWARD, //7 ==== 领取成就阶段奖励次数，id为achieve_step主键ID即成就id
	DRAW_DAILY_QUEST_ACTIVE_REWARD, //8 ==== 领取每日任务活跃奖励次数，id为daily_quest_active_reward主键ID
	DINNER_OPEN, //9 ==== 宴会开启，id为宴会类型
	DINNER_JOIN, //10 ==== 宴会参与，id为宴会类型
	COUNTDOWN_EVENT_ADD_TIMES, //11 ==== 倒计时事件添加次数，id为倒计时事件类型
	ANECDOTE_EVENT_DEAL_TIMES, //12 ==== 经营事件处理次数，id为经营事件id
	GEM_RECHARGE_TIMES, //13 ==== 钻石充值次数，id为gem_recharge配表id
	;
public static final EPlayerEventRecordType[]  EPlayerEventRecordType_Values = EPlayerEventRecordType.values();
public static final int EPlayerEventRecordType_Length = EPlayerEventRecordType_Values.length;
public static EPlayerEventRecordType EPlayerEventRecordType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EPlayerEventRecordType_Length){ return null; }
	return EPlayerEventRecordType_Values[_ivalue];
}
}

