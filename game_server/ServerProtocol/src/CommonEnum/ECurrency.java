package CommonEnum;

/*********
 * 货币类型
 **/
public enum ECurrency {
	NONE, //0 ==== 
	GEM, //1 ==== 钻石
	SILVER, //2 ==== 金币
	ARENA_COIN, //3 ==== 商会硬币
	VIP_EXP, //4 ==== VIP经验
	P_EXP, //5 ==== 玩家经验
	DINNER_COIN, //6 ==== 宴会币
	DUNGEON_COIN, //7 ==== 副本币
	DAILY_QUEST_ACTIVE_POINT, //8 ==== 日常任务活跃点
	MARKET_POINT, //9 ==== 废弃 集市专用-繁荣度
	GUILD_COIN, //10 ==== 联盟币
	HERO_EXP, //11 ==== 大臣经验
	TOWER_COIN, //12 ==== 迷宫币
	INN_AFFECTION, //13 ==== 旅店心意值
	INN_STATION_BLUEPRINT, //14 ==== 旅店设施图纸
	SATISFY, //15 ==== 满意值
	MARS_ENERGY, //16 ==== 火星系统-能量
	MARS_POINT, //17 ==== 火星币
	BATTLE_PASS_COIN, //18 ==== 战令币
	;
public static final ECurrency[]  ECurrency_Values = ECurrency.values();
public static final int ECurrency_Length = ECurrency_Values.length;
public static ECurrency ECurrency_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ECurrency_Length){ return null; }
	return ECurrency_Values[_ivalue];
}
}

