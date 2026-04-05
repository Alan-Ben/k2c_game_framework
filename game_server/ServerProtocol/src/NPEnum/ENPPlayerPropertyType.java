package NPEnum;

/*********
 * 玩家属性枚举
 **/
public enum ENPPlayerPropertyType {
	NONE, //0 ==== 
	OFFLINE_OUTPUT_LIMIT_SEC, //1 ==== 离线产出上限时间（秒）
	EXTRA_EARNINGS_ADD_PER, //2 ==== 额外赚速万分比加成
	UNUSE_ARENA_STATION_STORAGE_UNLIMITED, //3 ==== 已废弃 竞技场贸易站存储无上限 0-限制 1-无限制
	ARENA_IDENTITY_EFFECT_VALUE, //4 ==== 竞技场身份效果值
	TREASURE_HUNT_CAPTURE_GAIN_EXP_ADD_PER, //5 ==== 太空寻宝拾取处理经验万分比加成
	TREASURE_HUNT_CD_RECOVER_TIME_ADD_SEC, //6 ==== 太空寻宝CD恢复时间秒数加成
	TREASURE_HUNT_CD_LIMIT_ADD, //7 ==== 太空寻宝CD上限加成
	TREASURE_HUNT_ADVANCED_ORE_WEIGHT_ADD, //8 ==== 太空寻宝高级矿石权重加成
	UNUSE_TREASURE_HUNT_DAILY_GEM_OUTPUT_ADD, //9 ==== 已废弃 太空寻宝每日钻石产出加成
	GOLD_JOIN_DINNER_DAILY_LIMIT_ADD, //10 ==== 宴会金币赴宴每日上限次数绝对值加成
	TRAVEL_EVENT_GAIN_PLAYER_EXP_ADD, //11 ==== 游历事件获得玩家经验绝对值加成
	EQUIP_LEVEL_LIMIT_ADD, //12 ==== 藏品等级上限加成
	UNUSE_LVL_GOLD_BASE, //13 ==== 已废弃 等级金币基础值,随着等级提升
	UNUSE_SLOT_MACHINE_BASE, //14 ==== 已废弃 老虎机基础值,随着等级提升
	UNUSE_PET_LVL_MAX, //15 ==== 已废弃 宠物最大等级上限
	UNUSE_LVL_P_EXP_BASE, //16 ==== 已废弃 等级经验基础值,随着等级提升
	UNUSE_SPACE_DIGGING_PER, //17 ==== 已废弃 大地图上自动挖矿效率提升系数 万分比
	UNUSE_SPACE_MANUAL_DIGGING_PER, //18 ==== 已废弃 大地图上手动挖矿效率提升系数 万分比
	UNUSE_LVL_CEREALS_BASE, //19 ==== 已废弃 等级食物基础值,随着等级提升
	UNUSE_LVL_DUST_BASE, //20 ==== 已废弃 等级粉尘基础值,随着等级提升
	UNUSE_LVL_BUILDING_RES_BASE, //21 ==== 已废弃 等级建筑材料基础值,随着等级提升
	UNUSE_LVL_ENERGY_BASE, //22 ==== 已废弃 等级注能材料基础值,随着等级提升
	UNUSE_COOKING_SCORE_ADD_PER, //23 ==== 已废弃 烹饪分数提升 万分比加成
	UNUSE_DIGGINGS_SPEED_ADD, //24 ==== 已废弃 挖矿速度加成
	UNUSE_DIGGINGS_SPEED_ADD_PER, //25 ==== 已废弃 挖矿速度万分比加成
	UNUSE_DIGGINGS_MAX_TEAM_NUM, //26 ==== 已废弃 挖矿最大队伍数量
	UNUSE_ELF_NURSERY_NUM, //27 ==== 已废弃 精灵培养室数量
	UNUSE_LEVY_SILVER_CD_MIN, //28 ==== 已废弃：征收银币最小时间间隔（分钟）
	UNUSE_LEVY_SILVER_CD_MAX, //29 ==== 已废弃 征收银币最大时间间隔（分钟）
	UNUSE_LEVY_SOLDIER_CD_LIMIT, //30 ==== 已废弃 征收士兵CD数量上限
	UNUSE_EXTRA_CUSTOM_SUIT_COUNT, //31 ==== 已废弃 额外自定义套装数量
	ANECDOTE_LIMIT, //32 ==== 玩家可拥有的政务数量上限
	CUSTOM_FRIEND_GROUP_NUM, //33 ==== 自定义好友分组数量上限
	FRIEND_NUM, //34 ==== 好友数量上限
	CHILD_SEAT_ENERGY_NUM, //35 ==== 子嗣系统训练位脑力值上限
	CHILD_BONUS_PER, //36 ==== 子嗣转速系数万分比
	DINNER_OWNER_SCORE_PER, //37 ==== 开宴玩家宴会人气加成万分比
	DINNER_JOINER_SCORE_PER, //38 ==== 赴宴玩家宴会人气加成万分比
	CONSORT_CALL_ADD_CHARM_POINT, //39 ==== 所有妃子邀约时增加的加护点数值
	TRAVEL_ENERGY_LIMIT, //40 ==== 游历体力上限
	CONSORT_RANDCALL_ENERGY_LIMIT, //41 ==== 家人邀约精力上限
	INTIMACY, //42 ==== 亲密度
	CHARM, //43 ==== 加护力
	INN_RECEIVE_GUEST_LIMIT, //44 ==== 旅店接待次数上限
	MARS_ENERGY_OUTPUT_PER, //45 ==== 火星能源产出万分比加成
	MARS_TECH_SPEED_PER, //46 ==== 科研所研究速度加成万分比
	MARS_TECH_COST_PER, //47 ==== 科研所研究消耗减少万分比
	MARS_REPAIR_SPEED_PER, //48 ==== 维修室维修速度加成万分比
	MARS_REPAIR_COST_PER, //49 ==== 维修室维修消耗减少万分比
	MARS_BUILDING_UP_TIME_PER, //50 ==== 建筑创建/升级完成时间加成
	MARS_BUILDING_UP_COST_PER, //51 ==== 建筑创建/升级消耗减少万分比
	MARS_POWER_PER, //52 ==== 火星实力加成万分比
	MARS_EXPLORE_COUNT, //53 ==== 火星探索次数
	MARS_TEAM_TROOP_NUM_PER, //54 ==== 火星队伍带兵量加成万分比
	MARS_TEAM_TROOP_NUM, //55 ==== 火星队伍带兵基础数量
	MARS_TEAM_TROOP_EXT_NUM, //56 ==== 火星队伍带兵额外数量
	MARS_TEAM_SOLDIER_POWER_PER, //57 ==== 火星队伍单兵实力值加成万分比
	MARS_TEAM_SOLDIER_POWER, //58 ==== 火星队伍单兵基础实力值
	MARS_TEAM_MARCH_SPEED_PER, //59 ==== 火星探索行进时间加成万分比
	MARS_BUILDING_QUEUE_NUM, //60 ==== 火星建筑队列数量
	MARS_HELP_ACCEPT_LIMIT, //61 ==== 火星互助单次可以接收帮助的次数
	MARS_HELP_DOWN_SECS, //62 ==== 火星互助单次可以减少的时长（秒）
	;
public static final ENPPlayerPropertyType[]  ENPPlayerPropertyType_Values = ENPPlayerPropertyType.values();
public static final int ENPPlayerPropertyType_Length = ENPPlayerPropertyType_Values.length;
public static ENPPlayerPropertyType ENPPlayerPropertyType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerPropertyType_Length){ return null; }
	return ENPPlayerPropertyType_Values[_ivalue];
}
}

