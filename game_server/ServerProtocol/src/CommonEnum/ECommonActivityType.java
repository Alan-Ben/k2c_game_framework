package CommonEnum;

/*********
 * 活动类型
 **/
public enum ECommonActivityType {
	NONE, //0 ==== 
	RUSH_RANK, //1 ==== 冲榜活动
	EARNINGS_GOAL, //2 ==== 赚速目标
	SEVEN_DAY_GOALS, //3 ==== 七日目标
	REGULAR_EVENT, //4 ==== 万能活动
	TILE_MATCH, //5 ==== 三消
	RECHARGE_REBATE, //6 ==== 充值返利
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
	ACTIVITY_FUND, //18 ==== 活动基金
	RANK_GIFT_PACK, //19 ==== 排行榜礼包
	FIRST_TEAM, //20 ==== 组队活动
	;
public static final ECommonActivityType[]  ECommonActivityType_Values = ECommonActivityType.values();
public static final int ECommonActivityType_Length = ECommonActivityType_Values.length;
public static ECommonActivityType ECommonActivityType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ECommonActivityType_Length){ return null; }
	return ECommonActivityType_Values[_ivalue];
}
}

