package NPEnum;

/*********
 * 玩家的值类型,高级公式 CS_VALUE的值类型
 **/
public enum ENPPlayerValueType {
	NONE, //0 ==== 
	LVL, //1 ==== 玩家等级
	VIP_LVL, //2 ==== 玩家VIP等级
	RMB_NUM, //3 ==== 玩家充值RMB值
	FRIEND_COUNT, //4 ==== 好友数量
	HERO_NUM, //5 ==== 大臣数量
	HERO_IN_BUILDING_NUM, //6 ==== 入驻建筑的大臣数量
	EARNINGS, //7 ==== 每秒赚速
	CHILD_SUM, //8 ==== 未成年子嗣数量
	TOTAL_HERO_POWER, //9 ==== 总大臣实力
	TOTAL_HERO_TALENT, //10 ==== 总大臣资质
	CONSORT_NUM, //11 ==== 情人数量
	CHAPTER_POINT, //12 ==== 章节位置 chapterId*1000+point
	BUILDING_NUM, //13 ==== 建筑数量
	TOTAL_HERO_LEVEL, //14 ==== 总大臣等级
	TOTAL_CONSORT_INTIMACY, //15 ==== 总情人亲密度
	TOTAL_CONSORT_CHARM, //16 ==== 总情人加护力
	DAILY_CHECK_SUM, //17 ==== 总签到天数
	DONE_MAIN_QUEST_COUNT, //18 ==== 主线任务完成数（如果多次完成也算1次）
	STAGE_GOAL_DONE_STEP, //19 ==== 阶段目标已完成阶段
	INN_POPULARITY, //20 ==== 旅店人气值
	INN_LEVEL, //21 ==== 旅店等级
	TOWER_PASSED_CHAPTER, //22 ==== 爬塔当前通关的章节
	INN_HAD_UNLOCK_DISH_NUM, //23 ==== 旅店已解锁菜品数量
	TOWER_PASSED_LVL, //24 ==== 爬塔当前通关的关卡层数
	INN_STATION_TOTAL_LEVEL, //25 ==== 旅店设施总等级
	INN_HAD_RECEIVE_GUEST_COUNT, //26 ==== 旅店已接待客人数量
	TREASURE_HUNT_STATION_LEVEL, //27 ==== 太空寻宝太空舱等级
	TREASURE_HUNT_HAD_GAIN_ORE_TYPE_COUNT, //28 ==== 太空寻宝已收集矿石类型数量
	TREASURE_HUNT_HAD_GAIN_TREASURE_TYPE_COUNT, //29 ==== 太空寻宝已收集奇物类型数量
	TREASURE_HUNT_HAD_COLLECT_COMPOSITE_COUNT, //30 ==== 太空寻宝已集齐组合数量
	NAMED_CHILD_NUM, //31 ==== 已命名未成年子嗣数量
	MUSEUM_ITEM_NUM, //32 ==== 已获得珍宝数量
	CHAPTER_ID, //33 ==== 当前章节ID
	GUILD_LEVEL, //34 ==== 所在联盟等级
	UNMARRY_ADULT_SUM, //35 ==== 未婚成年子嗣数量
	MARS_EXPLORER_NUM, //36 ==== 火星探索次数
	TOWER_ACTIVE_RESEARCH_COUNT, //37 ==== 爬塔已激活研究章节数量
	MARS_BUILDING_EQUIP_LVL_SUM, //38 ==== 火星所有建筑所有部件总等级
	MARS_BUILDING_LVL_SUM, //39 ==== 火星所有建筑总等级
	INN_MEDAL_LEVEL, //40 ==== 旅店奖牌等级
	TOTAL_GAIN_GOLD_COUNT, //41 ==== 获得的金币总数量
	;
public static final ENPPlayerValueType[]  ENPPlayerValueType_Values = ENPPlayerValueType.values();
public static final int ENPPlayerValueType_Length = ENPPlayerValueType_Values.length;
public static ENPPlayerValueType ENPPlayerValueType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerValueType_Length){ return null; }
	return ENPPlayerValueType_Values[_ivalue];
}
}

