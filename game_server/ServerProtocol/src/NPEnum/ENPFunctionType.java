package NPEnum;

/*********
 * 系统功能类型
 **/
public enum ENPFunctionType {
	NONE, //0 ==== 
	CHAT, //1 ==== 聊天
	CHAT_INPUT, //2 ==== 聊天输入
	ACHIEVE, //3 ==== 成就
	FRIEND, //4 ==== 好友
	GUILD, //5 ==== 联盟
	DAILY_QUEST, //6 ==== 每日任务
	CHAPTER, //7 ==== 关卡
	RANK_COMMON, //8 ==== 排行榜
	SHOP, //9 ==== 商店
	QUEST, //10 ==== 主线任务
	BAG, //11 ==== 背包
	WEEKLY_QUEST, //12 ==== 周任务
	EQUIP, //13 ==== 藏品
	ANECDOTE, //14 ==== 政务
	ARENA, //15 ==== 竞技场
	RANK_RUSH, //16 ==== 冲榜
	CONSORT, //17 ==== 情人
	CHILD, //18 ==== 子嗣
	CHILD_MARRY, //19 ==== 子嗣联姻
	GACHA, //20 ==== 抽卡
	DINNER, //21 ==== 宴会
	HERO, //22 ==== 骑士
	ROOM, //23 ==== 公主卧室
	CITY, //24 ==== 主城
	MARKET, //25 ==== 集市
	DAILY_CHECK, //26 ==== 每日签到
	TOWER, //27 ==== 爬塔
	STAGE_GOAL, //28 ==== 阶段目标
	PLAYER_INFO, //29 ==== 玩家信息
	TRAVEL, //30 ==== 游历
	MAIL, //31 ==== 邮件
	HERO_RECOMMEND, //32 ==== 骑士推荐
	RANK, //33 ==== 排行榜
	ANNOUNCEMENT, //34 ==== 运营公告
	COMMON_TARGET, //35 ==== 家人获得
	MIDDAY_DUNGEON, //36 ==== 午间副本
	EVENING_DUNGEON, //37 ==== 晚间副本
	WALL_STREET, //38 ==== 华尔街
	CHILD_TRAIN, //39 ==== 子嗣培养
	DUNGEON_ENTRANCE, //40 ==== 午间＆晚间boss合并入口
	INN_MAIN, //41 ==== 旅店主界面
	TREASURE_HUNT, //42 ==== 太空寻宝
	MARS, //43 ==== 火星系统
	SCHOOL, //44 ==== 子嗣外围入口
	GRAVE, //45 ==== 杰出者大厅
	GUILD_COOPERATE, //46 ==== 联盟协作
	LAND_MARS, //47 ==== 登录火星
	MARS_EXPLORE, //48 ==== 火星探索
	ACTIVITY_FUND, //49 ==== 活动基金
	ROOM_SKIN, //50 ==== 卧室皮肤
	;
public static final ENPFunctionType[]  ENPFunctionType_Values = ENPFunctionType.values();
public static final int ENPFunctionType_Length = ENPFunctionType_Values.length;
public static ENPFunctionType ENPFunctionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPFunctionType_Length){ return null; }
	return ENPFunctionType_Values[_ivalue];
}
}

