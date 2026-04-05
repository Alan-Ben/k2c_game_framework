package NPEnum;

/*********
 * 玩家参数枚举
 **/
public enum ENPPlayerParam {
	NONE, //0 ==== 无效默认值
	LEVEL, //1 ==== 玩家（领主）等级
	VIP_LVL, //2 ==== VIP等级
	GM_LVL, //3 ==== 未接入 GM权限
	PLAYER_SKIN, //4 ==== 玩家使用皮肤Id
	ICON, //5 ==== 玩家头像
	ICON_BGK, //6 ==== 头像框ID
	BUBBLE, //7 ==== 玩家使用气泡框Id
	CREATE_TIME, //8 ==== 创角时间
	RECHARGED_GEM, //9 ==== 未接入 累计充值宝石
	BIRTH_GIFTDE_COUM, //10 ==== 下次出生卷王次数
	LOGIN_DAY_COUNT, //11 ==== 登录天数
	LAST_LOGIN_DATE, //12 ====  最后一次登录的日期标记
	LAST_TAKE_VERSION, //13 ==== 未接入 最后领取客户端版本奖励的版本
	LAST_LEFT_BAG_TIME, //14 ==== 最后一次查看背包物品的时间
	LAST_OFFLINE_MS, //15 ==== 最后一次离线时间戳（毫秒）
	CUTE_ACTOR, //16 ==== 玩家Q版形象ID
	OFFLINE_PERIOD_REWARD_DURATION_MS, //17 ==== 离线期间奖励时长（毫秒） 大于0则有奖励需要展示
	POWER_MAX_RECORD, //18 ==== 玩家战力历史记录最高值
	EARNINGS_MAX_RECORD, //19 ==== 玩家赚速历史记录最高值
	PREFAB, //20 ==== 玩家预制形象
	LAST_DRAW_DAILY_REWARD_DATE, //21 ==== 最后一次领取每日奖励的日期标记
	IS_SPACE_CAM_DRAG, //22 ==== 大地图摄像机是否拖拽 0-不可以 1-可以
	SERVER_START_DAYS, //23 ==== 服务器开启天数
	EXTRA_EARNINGS, //24 ==== 额外赚速
	IS_SET_DEFAULT, //25 ==== 已完成创角操作
	BUILDINGS_EARNINGS_MAX_RECORD, //26 ==== 建筑赚速历史记录最高值
	CHILD_EARNINGS_MAX_RECORD, //27 ==== 子嗣赚速历史记录最高值
	PLAYER_STAGE, //28 ==== 已废弃 玩家段位
	LAST_LOGIN_WEEK_TAG, //29 ==== 最后一次登录周标记
	WEEK_LOGIN_DAY_COUNT, //30 ==== 本周登录天数
	LATEST_LOGIN_TIME_MS, //31 ==== 最近一次登陆时间（毫秒）
	SEVEN_DAYS_LOGIN_COUNT, //32 ==== 七日登录天数
	HAD_DRAW_VIP_REWARD_LIST, //33 ==== 已领取VIP奖励列表 二进制位表示
	HAD_DRAW_VIP_RECHARGE_REWARD_LIST, //34 ==== 已领取VIP充值奖励列表 二进制位表示
	NATION_POWER_MAX_RECORD, //35 ==== 已废弃 玩家国力历史记录最高值
	MARKET_NEXT_REFRESH_MS, //36 ==== 集市下次刷新时间
	HERO_ATTR_MAX_RECORD, //37 ==== 已废弃 单一大臣属性值历史记录最高值
	CONSORT_CALL_NEXT_REFRESH_MS, //38 ==== 情人指定邀约下次刷新时间 毫秒时间戳
	LAST_GAIN_VISIT_REWARD_DAY, //39 ==== 最近一次领取拜访其他玩家奖励日期 YYYYMMDD
	ADULT_RECORD_BONUS, //40 ==== 子嗣记录收益总值（因为移除记录在玩家身上）
	IS_SET_PREFAB, //41 ==== 已设置预制形象
	DAY_HAD_SEND_PAY_AI_TIMES, //42 ==== 每日已发送付费AI次数
	PENDING_ORDER_DB_ID, //43 ==== 待处理订单ID
	IS_REFUSE_MARRY_REQUEST, //44 ==== 是否拒绝所有联姻请求 0-否 1-是
	HAD_MARS_POWER_RANK_OPENED, //45 ==== 是否开启过火星实力排行榜 0-否 1-是
	ROOM_SKIN, //46 ==== 房间皮肤Id
	MARS_GO_ROUTE_ARRIVE_COUNT, //47 ==== 累计抵达火星的玩家数量
	GIFTDE_CHILD_GRADUATE_COUNT, //48 ==== 卷王子嗣毕业历史数量，第一个毕业时无视概率发放宴会凭证（仅服务端使用）
	;
public static final ENPPlayerParam[]  ENPPlayerParam_Values = ENPPlayerParam.values();
public static final int ENPPlayerParam_Length = ENPPlayerParam_Values.length;
public static ENPPlayerParam ENPPlayerParam_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerParam_Length){ return null; }
	return ENPPlayerParam_Values[_ivalue];
}
}

