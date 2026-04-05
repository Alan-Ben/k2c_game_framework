package NPEnum;

/*********
 * 玩家条件类型
 **/
public enum ENPPlayerConditionType {
	NONE, //0 ==== 
	UNUSE_S_LOGIC_EVENT, //1 ==== 废除 监听到事件触发 S_LOGIC_EVENT:事件名称:事件参数:n:m
	CS_VALUE, //2 ==== 值范围 CS_VALUE:ENPPlayerValueType:min:max
	CS_BUF_LAYER, //3 ==== BUFF层级 CS_BUF_LAYER:buf_id:min:max
	CS_HAS_ITEM, //4 ==== 拥有物品  CS_HAS_ITEM:itemType-item_id:min:max
	CS_HAS_BUILDING, //5 ==== 玩家已放置建筑 CS_HAS_BUILDING:building_id
	CS_JUD_SIM_UNLOCK, //6 ==== 判断简要解锁的解锁信息 CS_JUD_SIM_UNLOCK:sim_unlock_id
	CS_SPECIAL, //7 ==== 特殊判断条件 CS_SPECIAL:ENPPlayer_CS_SpecialCondition
	C_ID_JUDGE, //8 ==== 针对Id进行判断 C_ID_JUDGE:EPlayer_C_IdJudgeFunc:id
	CS_ID_JUDGE, //9 ==== 针对Id进行判断 CS_ID_JUDGE:ENPPlayer_CS_IdJudgeFunc:id
	CS_RECORD_PARAM, //10 ==== 玩家计数 CS_RECORD_PARAM:ENPPlayerRecordParam:min_value（:max_value）
	CS_QUEST_COUNT, //11 ==== 任务完成次数 CS_QUEST_COUNT:quest_id:min_value（:max_value）
	CS_QUEST_DOING, //12 ==== 正在进行的任务 CS_QUEST_DOING:quest_id（:quest_step_ID:quest_target_ID）
	CS_EVENT_RECORD, //13 ==== 玩家事件记录计数 CS_EVENT_RECORD:ENPPlayerEventRecordType:sub_id:min_value（:max_value），sub_id取-1表示所有子计数之和注意，sub_id使用负数时需要用括号括起来，否则会被当成分隔符无法读取例： '（-1）'
	C_GUILD_SELF_HAVE_PERMISSION, //14 ==== 联盟自己的职位是否拥有对应权限 C_GUILD_SELF_HAVE_PERMISSION:EGuildPermissionType
	C_HAS_TREASURE_MAP, //15 ==== 拥有藏宝图 C_HAS_TREASURE_MAP（:id配置id）
	CS_QUEST_STEP_IS_DONE, //16 ==== 是否完成任务 CS_QUEST_STEP_IS_DONE:quest_id:step_id
	CS_VARIABLE, //17 ==== 某高级公式的值范围 CS_VARIABLE:min:max:高级公式
	C_PLAYER_CAN_LEVEL_UP, //18 ==== 玩家可以升级 C_PLAYER_CAN_LEVEL_UP
	C_PLAYER_IN_SPACE_POS, //19 ==== 玩家在大地图指定范围 C_PLAYER_IN_SPACE_POS:mapid（地图id）:位置（Vector3）:范围（float）
	CS_HAS_HERO, //20 ==== 拥有指定大臣中的n个 CS_HAS_HERO:hero1&hero2:（n）
	CS_BUILDING_FUNC_LEVEL, //21 ==== 玩家已放置建筑功能等级 CS_BUILDING_FUNC_LEVEL:building_id:EBuildingFuncEnum（:minLevel:maxLevel）
	CS_CHAPTER_STAGE_PASSED, //22 ==== 章节是否通过指定关卡 CS_CHAPTER_STAGE_PASSED:stageId
	CS_CONSORT_SKILL, //23 ==== 情人技能解锁检查 CS_CONSORT_SKILL:情人ID:技能ID:最小等级（:最大等级 默认-1）
	C_FUNC_UNLOCK_IS_SHOW, //24 ==== 系统解锁弹窗是否已经展示过 C_FUNC_UNLOCK_IS_SHOW:ENPFunctionType
	CS_HERO_REACH_STAR_NUM, //25 ==== 达到指定星级大臣数量 CS_HERO_REACH_STAR_NUM:星级:min（:max）
	CS_LEVY_SUM, //26 ==== 征收累计数量 CS_LEVY_SUM:ELevy_Type:min（:max）
	C_ACHIEVE_IS_DONE, //27 ==== 成就是否已经全部完成领奖 C_ACHIEVE_IS_DONE:achieve_id
	C_WEEK_CARD_STAT, //28 ==== 周卡状态是否满足 C_WEEK_CARD_STAT:stat（EWeekCardStat）
	C_ADD_PACK_NEED_DOWNLOAD, //29 ==== 是否有增量包需要下载 C_ADD_PACK_NEED_DOWNLOAD
	C_CHAPTER_BLOCK_EVENT_DEAL_DONE, //30 ==== 关卡格子上事件是否处理完成 C_CHAPTER_BLOCK_EVENT_DEAL_DONE:关卡格子配表id chapter_stage_block_id （:挂起事件是否视为事件已经完成bool, 默认为true）
	C_CLOTHES_HANDBOOK_CAN_REWARD, //31 ==== 图鉴是否可以领奖 C_CLOTHES_HANDBOOK_CAN_REWARD:TRUE（或者FALSE）
	C_CONSORT_INTIMACY_STEP_REACHED, //32 ==== 妃子亲密度阶段是否达到 C_CONSORT_INTIMACY_STEP_REACHED:妃子consort_id:阶段intimacy_step
	CS_CONSORT_RES_COUNT, //33 ==== 指定家人指定资源数量  CS_CONSORT_RES_COUNT:家人ID:EBagItemUse_ConsortDrawShowType:min:max
	CS_CONSORT_LIKE_COUNT, //34 ==== 指定家人（未获得）好感度数值  CS_CONSORT_LIKE_COUNT:家人ID:min:max
	C_HAS_OWNER_DINNER_REWARD, //35 ==== 是否有自身宴会奖励未领取 C_HAS_OWNER_DINNER_REWARD
	C_EVENING_DUNGEON_ACTIVITY_STATE, //36 ==== 晚间副本活动状态 C_EVENING_DUNGEON_ACTIVITY_STATE:（EEveningDungeonActivityState:PREVIEW:ONGOING:END:CLOSE）
	C_HOTFIX_CONDITION, //37 ==== 热更条件类型 C_HOTFIX_CONDITION:热更条件类型:参数
	C_MIDDAY_DUNGEON_ACTIVITY_STATE, //38 ==== 午间副本活动状态 C_MIDDAY_DUNGEON_ACTIVITY_STATE:（EMiddayDungeonActivityState:PREVIEW:ONGOING:END）
	C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT, //39 ==== 七天登录已领取奖励数量 C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT:int
	CS_HAS_CONSORT, //40 ==== 拥有指定妃子中的n个 CS_HAS_CONSORT:consort1&consort2:（n）
	C_ACTIVITY_STATE, //41 ==== 活动状态 C_ACTIVITY_STATE:EActivityState:activityId
	CS_HAD_UNLOCK_INN_DISH, //42 ==== 是否解锁旅店菜品 CS_HAD_UNLOCK_INN_DISH:菜品id
	CS_HAD_UNLOCK_BUILDING_PRODUCT, //43 ==== 是否解锁建筑产品 CS_HAD_UNLOCK_BUILDING_PRODUCT:建筑id:产品id
	C_CONSORT_CALL_DIALOG_NOT_SHOW_CG, //44 ==== 妃子邀约对话是否不展示CG
	S_IS_SYSTEM_QUEST_GROUP_DONE, //45 ==== 是否完成系统任务组 S_IS_SYSTEM_QUEST_GROUP_DONE:group_id
	CS_MARS_BUILDING_LVL, //46 ==== 火星指定建筑等级判断 CS_MARS_BUILDING_LVL:建筑id:min_value（:max_value）
	CS_MARS_TECH_LVL, //47 ==== 火星指定科技等级判断 CS_MARS_TECH_LVL:科技id:min_value（:max_value）
	CS_CONSORT_UNLOCK_CG_NUM, //48 ==== 解锁妃子CG的数量 CS_CONSORT_UNLOCK_CG_NUM:min_value（:max_value）
	CS_CHECK_PLAYER_PERMISSION, //49 ==== 检查玩家是否拥有指定权限 CS_CHECK_PLAYER_PERMISSION:权限配置ID
	CS_MARS_PEOPLE_NUM, //50 ==== 火星人口数量判断 CS_MARS_PEOPLE_NUM:min_value（:max_value）
	CS_MARS_IDLE_PEOPLE_NUM, //51 ==== 火星空闲人口数量判断 CS_MARS_IDLE_PEOPLE_NUM:min_value（:max_value）
	C_LOVER_COLLECT_HAS_TARGET, //52 ==== 情人收集是否有指定目标情人
	C_LOVER_COLLECT_HAD_DRAW, //53 ==== 情人收集是否已抽取指定情人
	C_SPECIAL, //54 ==== 客户端特殊判断条件 C_SPECIAL:EClientSpecialConditionType
	;
public static final ENPPlayerConditionType[]  ENPPlayerConditionType_Values = ENPPlayerConditionType.values();
public static final int ENPPlayerConditionType_Length = ENPPlayerConditionType_Values.length;
public static ENPPlayerConditionType ENPPlayerConditionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerConditionType_Length){ return null; }
	return ENPPlayerConditionType_Values[_ivalue];
}
}

