using System;
using System.Collections.Generic;

using UnityEngine;


namespace GOE
{
	public enum ENPExportSettingEnum {
	    NONE,
	    LANGUAGE,//
	    GENERAL,//常量表
	    CITY_REF,
	    SKIN_PROPERTY,
	    ATTACK_TYPE,
	    SKIN,
	    ACTOR_QUALITY,
	    ACTOR,
	    RACE,
	    TEST_HOTFIX,//测试活动配表
	    DUNGEON,
	    HERO_CARD,
	    MAP,
	    MINION_CARD,
	    CARD_SKILL,
	    PLAYER_RES,//玩家货币资源
	    useless_17,//无效字段============================================
	    BAG_ITEM,//背包物品表
	    PLAYER_ICON,//玩家头像图标
	    PLAYER_ICON_BGK,//玩家头像背景框
	    PLAYER_BUBBLE,//玩家气泡框
	    PLAYER_HONOR,//玩家勋章

	    EXP_LEVEL,//玩家等级经验表
	    HERO_PROPERTY,//英雄属性

	    ACTOR_SKILL,//单位技能表
	    ACTOR_BUFF,//战斗内buff
	    EFFECT,//战斗内效果
	    POS_EFFECT,//战斗位置内效果
	    BULLET,//战斗子弹信息
	    SFX,//特效信息
	    SFX_3D,//特效的3d信息
	    SFX_3D_BONE_TAG,//特效的3d骨骼标记信息
	    PRELAYOUT_DUNGEON,//布局副本

	    useless_30, //无效字段============================================
	    CARD_UI_PROPERTY,//卡牌ui面板属性的设置
	    SERVER_EFFECT,//服务器效果
	    REMOTE_EFFECT,//远程效果
	    useless_34,//无效字段================================================
	    MISSION,//关卡信息
	    
	    CHAPTER,//章节信息
	    CHAPTER_EVENT,//关卡事件表
	    CHAPTER_EVENT_REWARD,//关卡事件奖励表
	    CHAPTER_EVENT_CHOICE,//关卡选择事件表
	    CHAPTER_EVENT_CHOICE_OPTION,//关卡选择事件选项表
	    CHAPTER_EVENT_DISPATCH,//关卡派遣事件表
	    CHAPTER_EVENT_DISPATCH_SHOW,//关卡派遣事件展示表
	    CHAPTER_EVENT_DISPATCH_COND,//关卡派遣事件条件表
	    CHAPTER_EVENT_DISPATCH_REWARD,
	    CHAPTER_STAGE,//"关卡 节-Stage 表（chapter_stage）
	    CHAPTER_STAGE_PLOT,// 关卡节剧情表
	    CHAPTER_STORY,//关卡故事表
	    
	    __MISSION,//暂时不用信息

	    CITY_AREA,//主城区域

	    P_LANGUAGE,//平台翻译表
	    G_LANGUAGE,//登陆游戏内翻译表

	    DETECTOR_CHARACTER,//屏蔽字库
	    DETECTOR_PLAYER_NAME,//取名屏蔽字库
	    BATTLE_RES,//战斗内资源
	    TEAM_PROPERTY,//队伍属性信息
	    BAG_ITEM_USE,//背包物品使用表

	    TUTORIAL,//战斗外引导
	    TUTORIAL_EDGE,//引导的边
	    SIMPLE_TUTORIAL,//简易引导
	    SIMPLE_TUTORIAL_EDGE,//简易引导的边

	    FUNCTION_UNLOCK,//功能解锁

	    PLAYER_BUFF,//玩家buff
	    useless_53,//无效字段=========================================
	    REWARD,//奖励
	    useless_55,//无效字段=========================================
	    useless_56,//无效字段=========================================
	    MOB,//怪物表
	    LORD_LEVEL,//领主等级表
	    ITEM_ALTER,//物品替换

	    BOOK_EXP,

	    CONDITION_PREFAB,   //预制条件集合
	    SYS_INFO,           //系统信息对象
	    SIMPLE_UNLOCK,      //简易解锁模型

	    useless_64,//无效字段=========================================

	    ACTIVITY,           //活动
	    AUDIO,              //音效
	    QUALITY_GROUP,		//音效组
	    VOICE_KEY,			//配音对应key表
	    
	    NATIONAL_ICON,      //国籍图标
	    VERSION_UP_REWARD,  //升级版本的奖励信息

	    CONTROL_STATE,      //战斗中状态对应信息

	    PROPERTY,           //战斗属性信息导出
	    DEFENCE_TYPE,       //防御类型数据

	    TEAM_SKILL,         //队伍技能信息
	    ACHIEVE,            //成就系统
	    AUDIO_HIT,          //击中音效

	    MAP_TERRAIN,        //地图数据

	    LOGIN_WAY,          //登录方式数据

	    SCENE_INFO,         //预制场景信息

	    UNUSED_80,  //军队通用等级
	    UNUSED_81,         //军队等级
	    UNUSED_82,       //军队品质
	    UNUSED_83,           //军队基础信息

	    ADDITION_LINEUP_CARD_INFO,  //附加阵容的卡牌信息
	    UNUSED_85,  //附加阵容的军队信息
	    ADDITION_LINEUP_POS_INFO,   //附加阵容的单位位置信息

	    DUNGEON_MOB_INSTANCE,           //副本初始化单位信息表
	    MISSION_MOB_INSTANCE,       //关卡的初始化实例对象数据

	    REWARD_GROUP,       //奖励集合信息
	    REWARD_ITEM,        //奖励单项奖励信息

	    MISSION_LINEUP_MOB,     //关卡中的怪物阵容对应单位索引的信息
	    MISSION_LINEUP,         //关卡的怪物阵容模板信息

	    //其他枚举为存储路径使用，导出枚举请往上加
	    COLLECT_DEPENDENCIES,//文件引用依赖
	    FILE_RENAME_FILE,//文件重命名—文件路径
	    FILE_RENAME_FLODER,//文件重命名—文件夹路径
	    APP_PAY_PLATFORM,
	    IOS_COUNTRY_LOGIN,


	    HERO_CARD_LEVEL,
	    HERO_COMMON_LEVEL,
	    HERO_COMMON_STAR,
	    CARD_SKILL_LEVEL,
    
	    SPACE,
	    SPACE_AREA,
	    SPACE_UNIT,
	    SPACE_SQLITE_PATH,

	    G_LANGUAGE_EXPAND_1,//登陆游戏内翻译表拓展1
	    G_LANGUAGE_EXPAND_2,//登陆游戏内翻译表拓展2
	    G_LANGUAGE_EXPAND_3,//登陆游戏内翻译表拓展3
	    G_LANGUAGE_EXPAND_4,//登陆游戏内翻译表拓展4
	    G_LANGUAGE_EXPAND_5,//登陆游戏内翻译表拓展5
	    G_LANGUAGE_EXPAND_6,//登陆游戏内翻译表拓展6
	    G_LANGUAGE_EXPAND_7,//登陆游戏内翻译表拓展7
	    G_LANGUAGE_EXPAND_8,//登陆游戏内翻译表拓展8

	    CITY_LAND,//主城地块信息
	    MAIL,//邮件
	    MAIL_TYPE,//邮件类型
	    MAIL_SENDER,//邮件发送者

	    PLAYER_TITLE,//玩家称号表
	    REWARD_SUB,//奖励子表 - 服务端用

	    HATCH_SEED,//孵化种子表
	    BAG_ITEM_HATCH_SPEED,//孵化加速道具子表
	    CHAT_ROOM,//聊天室
    
	    PET,//宠物表
	    PET_EVOLUTION,//宠物进化表
	    PET_SKIN,//宠物皮肤表
	    PET_LVL,//宠物等级表
	    PET_STEP,//宠物阶段表
	    PET_STAR,//宠物星级表
	    PET_SKILL,//宠物技能表
	    PET_SKILL_LVL,//宠物技能等级表
	    PET_ELEMENT,//宠物元素表
	    PET_ZODIAC,//宠物星座表
	    PET_PROP_POWER,//宠物战力表
	    PET_ATTR,//宠物资质表
	    PET_ATTR_GRADE,//宠物资质等级表
	    PET_RAND,//宠物随机表
		PET_RAND_ATTR_POOL,//宠物随机资质池
    
	    QUALITY,//通用品质表
	    QUALITY_EXT,//品质额外表

	    TIMES_PRICE,//价格递增表
    
	    HATCH_FRUIT,//孵化果实表
    
	    PARTICLE,
    
	    UI_RES_PATH,         //UI资源路径信息
    
	    COOKING_RECIPE,//烹饪食谱表
	    COOKING_RECIPE_LV,//烹饪食谱等级表
	    COOKING_QTE,//烹饪QTE表
	    COOKING_LV,//烹饪等级表
	    COOKING_BAGITEM_DISHES,//烹饪菜肴 道具子表
	    COOKING_INGREDIENT, //烹饪食材 道具子表

	    SPECIAL_TYPE,//特殊类型表，itemtype的子项
	    UNICODE_LENGTH_CHECK,//字符长度表
	    CHAT_NPC,//聊天NPC
	    PLAYER_GENDER,//性别表

	    CENTER_TIPS,//上浮提示表
	    SPACE_OTHER,//大地图其它配置

	    PLAYER_LVL,//玩家等级表

	    RANK,//排行榜通用表
	    RANK_FIXED,//排行榜常驻表
    
	    SERVER_STAT,//服务器显示状态
	    LAZY_CD,//CD表

	    QUEST,//任务表
	    QUEST_STEP,//任务步骤表
	    QUEST_TARGET,//任务目标表
	    QUEST_TARGET_SPACE_MOVE,//任务目标大地图移动子表

	    QUEST_GROUP,//任务章节表    
	    TOOL_TIP_ITEM,//图标提示工具类

	    SPACE_ITEM,//大地图交互物体表
	    SPACE_ITEM_VIEW,//大地图物体显示表
	    ITEM_FUNC_TELEPORT,//大地图传送点表
	    MINI_MAP_TELEPORT,//地图传送点表
	    SPACE_ITEM_ROYAL_CITY,//大地图主城表
    
	    ITEM_EXCHANGE,//物品转换
	    QUEUE_DEALER_TIPS,//队列处理提示表

	    TELEPORT_SUB,//大地图传送功能子表

	    COOKING_DISHES_TYPE,//烹饪菜肴使用效果表

	    ACCESS,//获取途径表
	    SPACE_MAP,//大地图 地图表
	    ITEM_FUNC_EFFECT,//大地图 物体效果

	    PLAYER_LVL_STAGE,//玩家等级阶段表
	    PLAYER_LVL_PERIOD,//玩家等级时期表
	    PLAYER_LVL_LAYER,//玩家等级几重表
	    ENTRY_POINT,//入口点表


	    MAIN_NODE_TO_FUNCTION_TYPE,//界面跳转类型和系统功能的映射表
	    LAYOUT,//阵型表
	    LAYOUT_POS,//阵型位置表
    
	    DYNAMIC_ITEM_REFRESH,//动态 item 表
	    DYNAMIC_ITEM_REFRESH_GROUP,//动态 item 刷新组表
	    DYNAMIC_ITEM_REFRESH_POSITION,//动态 item 刷新顶点表
	    DYNAMIC_ITEM_REFRESH_SUB,//动态 item 刷新表
	    
	    STATIC_ITEM_REFRESH,//静态 item 刷新表
	    SPACE_ITEM_EFFECT,//大地图特殊刷新规则表
	    
	    ITEM_FUNC_CAPTURE,//动态 item 捕捉点
	    ITEM_FUNC_COLLECTION,//动态 item 采集点
	    ITEM_FUNC_MACHINE,//动态 item 交互机关点
	    ITEM_FUNC_MINI_GAME,//动态 item 交互小游戏点
	    ITEM_FUNC_DIGGINGS,//动态 item 矿区点
	    ITEM_FUNC_DIGGINGS_MANUAL,
	    ITEM_FUNC_PARTY,//动态 item 聚会关点
	    ITEM_FUNC_ARENA,//大地图比武擂台表
	    SPACE_ACTION,//大地图表现表
	    SPACE_ACTION_ADDITION_VIEW,//大地图动作可交互附加物表
	    SPACE_GOTO,//大地图前往表

	    LAN_ARGS,//特殊翻译替换表

	    SPACE_ITEM_VIEW_NPC,//大地图物体显示NPC子表
	    SPACE_COMM_EFFECT_DIALOG,//大地图 物体效果-对话

	    MINI_GAME_QUEST,//悬赏任务表
	    MINI_GAME_QUEST_LVL,//悬赏任务等级表
	    MINI_GAME_QUEST_RANDOM_SHOW,//悬赏任务随机包装表
	    MINI_GAME_QUEST_SPACE_AREA_REFRESH,//悬赏任务地图刷新规则表

	    DAILY_QUEST,//日常任务
	    DAILY_QUEST_REWARD,//日常任务奖励

		ITEM_DEF,//ItemDef表

        DAILY_QUEST_REFRESH,//日常任务刷新表
        DAILY_QUEST_GROUP,//日常任务随机组表

		FIXED_CD,//固定时间恢复的CD表

		FLAPPY_BIRD_OTHER,//Flappy Bird常量表
		FLAPPY_BIRD_STAGE,//Flappy Bird关卡表
		FLAPPY_BIRD_GAME_PARAM,//Flappy Bird游戏参数表

		MINI_GAME,//小游戏主表
		
		
		DIALOGUE,//对话主表
		DIALOGUE_SENTENCE,//对话句子表
		NPC,//NPC表
		NPC_ACTOR,//NPC-Actor类型子表
		NPC_GO,//NPC-独立模型子表

		ACHIEVE_STEP,//成就步骤
		ACHIEVE_TYPE,//成就类型
		ACHIEVE_POINT_STEP,//成就点数阶段
		ACHIEVE_POINT,//成就点数

		SHOP, //商店表
		SHOP_ITEM,//商店商品表
		SHOP_ITEM_GROUP,// 商店商品组表
		SHOP_ITEM_DISCOUNT,//商品打折表

		ARENA,//比武擂台主表
		ARENA_RANK_REWARD,//比武擂台排行奖励表
		ARENA_NPC,//比武擂台NPC表
		ARENA_MSG,//比武擂台消息表
		ARENA_NPC_PET,//比武擂台NPC宠物表
		
		BAG_ITEM_PARTY,		//聚会保护罩道具子表
		
		RECORD_LOG,//记录log表

		PVP_MISSION,//PVP关卡
		
		PLAYER_PROPERTY,//玩家属性表

		SLOT_MACHINE,//老虎机

		TREASURE_MAP,//藏宝图主表
		TREASURE_MAP_REWARD,//藏宝图宝箱表
		TREASURE_MAP_SPACE_ITEM,//藏宝图大地图物件表

		MACHINE,//机关主表
		MACHINE_REWARD,//机关奖励表
		
		PET_FAMILY,//宠物家族
		PET_STELLA_RULE,//宠物占星模板
		PET_FAMILY_STEP_REWARD,//宠物家族星级阶段奖励
		PET_STELLA_ZODIAC,//宠物星座
		PET_STELLA_ZODIAC_STEP_REWARD,//宠物星座阶段奖励
		
        BAG_ITEM_COOKING_CAPTURE,//食物捕捉表

		PRIVATE_ITEM,//个人物件
		PLAYER_NAME,//玩家起名表
		RED,//红点表
		ATTACHMENT_ITEM,//附加物表

        ITEM_FUNC_STONE,//动态 item 奇石点
        
		COOKING_DISHES,//烹饪菜肴表
		COOKING_NO_RECIPE_DISHES,//非食谱菜肴表
		QTE_RESULT,
		COOKING_FOOD_SCORE,
		COOKING_NO_RECIPE,

        COMMON_BOX,//通用宝箱表
        COMMON_BOX_GROUP,//通用宝箱组表
        
        ITEM_FUNC_PVE,//大地图pve
        DIALOGUE_RESPONESE_OPTION,//对话选项

        SHARE,//分享

		PLAYER_PREFAB,//默认形象
		LOGIN_AREA,//登入大区
		
		MINI_MAP_ITEM,//MINI_MAP_ITEM
		MINI_MAP_OTHER,//mini_map一些奇怪的配置

        COOKING_LOOP_REWARD,//烹饪额外奖励
        COOKING_SCORE_RANGE,//烹饪评分范围

		ITEM_CONVERT,//物品兑换表

		COOKING_RECIPE_EXTRA,//烹饪额外获得
		MINI_MAP_SCALE_SHOW,//MINI_MAP_SCALE_SHOW
		WISH_QUEST,//心愿任务表
		WISH_QUEST_TYPE,//心愿任务品质表
		
		SHOW_CASE_ACTOR_BEHAVIOR,//showcase模型额外表现表
		
		HINT_MAP,//区域传送表
		HINT_MAP_ITEM,//区域传送的 item 表
		HINT_MAP_ARENA_ITEM,//区域传送的 arena item 表
		HINT_MAP_MINE_ITEM,//区域传送的 mine item 表
		HINT_MAP_CAPTURE_ITEM,//区域传送的 capture item 表

		DIGGINGS_LEVEL,//矿区等级
		DIGGINGS_ZONE,//矿区刷新表
		
		RES_ISLAND,//资源岛

		PLAYER_STAGE,//玩家段位表
		QUEST_STEP_EXTRA_TRIGGER,//任务步骤额外效果表

		LINEUP_LAYOUT,//怪物阵型资源表
		EFFECT_GOTO,//GoTo效果表

		RANK_REWARD,//冲榜排名奖励表

		LOCAL_PUSH,//本地推送表
		MAIN_CITY_PUSH_NOTICE,//主城推送弹窗表
		PUSH_NOTICE_ACTIVITY_MERGE,//活动合并展示推送表

		RULE,//规则表
		RULE_SUB,//规则表子表

		BATCH_MISSION,//批量战斗关卡
		BATCH_PVP,//批量战斗pvp
		BATCH_PET_GROUP,//批量战斗宠物组
		BATCH_PET_PROPERTY,//批量战斗宠物属性
		BATCH_PET_TEMPLATE,//批量战斗宠物技能

        DAILY_CHECK,//签到
        DAILY_CHECK_REWARD,//签到奖励
        DAILY_CHECK_LOOP_REWARD,//签到累计奖励
        DAILY_CHECK_DESSERT,//签到甜心
        DAILY_CHECK_CONSORT,//签到妃子

        ACTIVITY_MAIN,//活动主表
		ACTIVITY_EVENT,//活动事件表
		ACTIVITY_STEP_REWARD_STEP,//活动阶段奖励表
		ACTIVITY_RANK_REWARD,//活动排行奖励表
		
		STEP_REWARD_SET,//阶段奖励设置表
		STEP_REWARD_SET_EVENT_TASK,//阶段奖励设置事件任务表
		
		CONSORT,//情人表
		CONSORT_STORY,//妃子故事表
		CONSORT_STORY_BG,//妃子故事背景表
		CONSORT_FETTERS_LVL,//家人羁绊等级
		CONSORT_FETTERS_SKILL,//家人羁绊技能
		CONSORT_FETTERS_SKILL_LVL, //家人羁绊技能等级
		CONSORT_BUSINESS_SKILL,//家人经营技能
		CONSORT_BUSINESS_POTENTIAL,//家人经营潜力
		CONSORT_BLESS_SKILL,//家人加护技能
		CONSORT_BLESS_SKILL_LVL,//家人加护技能等级
		CONSORT_SKIN,//情人皮肤
		CONSORT_SKIN_LVL,//情人皮肤等级
		CONSORT_TRAVEL,//妃子旅游表
		CONSORT_HALO_LVL,//妃子星辉等级表
		CONSORT_HALO_SKILL,//妃子星辉技能表
		CONSORT_HALO_SKILL_LVL,//妃子星辉技能等级表
		CONSORT_VOICE_GROUP,//妃子配音组表
		CONSORT_CG,//妃子CG表

		PRO_ADD,//加成概率表
		OP_COST,//操作消耗表
		
		HERO,//骑士表
		HERO_LEVEL,//骑士等级表
		HERO_STAR,//骑士觉醒表
		HERO_STAR_SKILL,//骑士觉醒技能表
		HERO_STAR_SKILL_LEVEL,//骑士觉醒技能等级表
		HERO_SKIN,//骑士皮肤表
        HERO_SKIN_LEVEL,//骑士皮肤等级表
        HERO_STEP,//骑士阶段表
        HERO_TALENT_SKILL,//骑士资质技能表
        HERO_TALENT_SKILL_LEVEL,//骑士资质技能等级表
		HERO_BUSINESS_SKILL,//骑士经营技能表
		HERO_BUSINESS_SKILL_UPGRADE,//骑士经营技能等级表
		HERO_VOICE_GROUP,//骑士配音组表

		BASIC_ATTR,//通用属性表

		CHILD_ATTR,//子嗣相性表
		CHILD_CAREER,//子嗣职业表
		CHILD_INIT_RES,//子嗣初始资源表
		CHILD_QUALITY,//子嗣天资表
		CHILD_RES,//子嗣资源形象表
		CHILD_SEAT,//子嗣席位表

		BAG_ITEM_HERO,//骑士物品表
		BAG_ITEM_CONSORT,//情人物品表
		BAG_ITEM_DYE,//染色物品表
						
		DINNER_TYPE,//宴会类型
		DINNER_JOIN_COST,//宴会消耗
		DINNER_PERMIT,//宴会凭证
		
        HERO_HALO,//骑士光环表
		HERO_HALO_LEVEL,//骑士光环等级表
		HERO_HALO_SUIT,//伙伴套系表
		HERO_HALO_SUIT_SKILL,//伙伴套系技能表
		HERO_HALO_SUIT_SKILL_LEVEL,//伙伴套系技能等级表

		CHILD_NAME,//子嗣起名表

		#region 通用事件表

		COMMON_EVENT_AWARD,//通用事件-奖励事件实例表
		COMMON_EVENT_AWARD_SHOW,//通用事件-奖励事件表现表
		COMMON_EVENT_CHOICE_OPTION,//通用事件-选择事件选项表
		COMMON_EVENT_CHOICE,//通用事件-选择事件实例表
		COMMON_EVENT_CHOICE_SHOW,//通用事件-选择事件展示表
		COMMON_EVENT_DIALOG,//通用事件-对话事件实例表
		COMMON_EVENT_DISPATCH_COND,//通用事件-派遣事件条件表
		COMMON_EVENT_DISPATCH,//用事件-派遣事件实例表
		COMMON_EVENT_DISPATCH_RESULT,//通用事件-派遣事件结果表
		COMMON_EVENT_DISPATCH_SHOW,//通用事件-派遣事件问题表
		COMMON_EVENT,//通用事件主表
		COMMON_EVENT_REWARD,//通用事件-事件奖励
		COMMON_EVENT_PLOT_DIALOG,//通用事件-剧情对话事件实例表
		COMMON_EVENT_MINI_GAME,//通用事件-小游戏事件实例表
		COMMON_EVENT_MINI_GAME_SHOW,//通用事件-小游戏事件表现表
		COMMON_EVENT_FITTING,//通用事件 - 试穿衣服事件表
		COMMON_EVENT_FITTING_SHOW,//通用事件 - 试穿衣服事件表现表
		COMMON_EVENT_FITTING_OPTION,//通用事件 - 试穿衣服事件选项表

		#endregion
		
		ANECDOTE_EVENT_CHOICE_OPTION,
		ANECDOTE_EVENT_CHOICE,
		ANECDOTE_EVENT_EARNINGS,
		ANECDOTE_EVENT,//政务事件表
		ANECDOTE_EVENT_REWARD,
		ANECDOTE_POS,
		

        PLAYER_TITLE_RANK_TYPE,//玩家称号档位表
        PLAYER_HERO_UNLOCK_SHOW,//玩家大臣解锁展示表

        #region 游历

        TRAVEL_POS,//游历地点
        TRAVEL_CONSORT,//游历妃子表
		TRAVEL_EVENT_TYPE,//游历事件类型表
		TRAVEL_EVENT,//游历事件表
		TRAVEL_EVENT_ONCE,//游历单次事件表
		TRAVEL_EVENT_CONSORT_LIKE,//游历妃子好感度事件表
		TRAVEL_EVENT_CONSORT_INTIMACY,//游历妃子亲密度事件表
		TRAVEL_EVENT_CONSORT_BAR,//游历妃子酒馆事件表
		TRAVEL_EVENT_CONSORT_BAR_COST,//游历妃子酒馆事件消耗表
		TRAVEL_EVENT_CHANGE,//游历交换事件表
		TRAVEL_EVENT_INVITATION,//游历邀约事件表
		TRAVEL_EVENT_GIFTDE,//游历卷王事件表
		TRAVEL_EVENT_GAMBLE,//游历博彩事件表
		TRAVEL_EVENT_ADD_POWER,//游历大臣实力事件表
		TRAVEL_EVENT_AKEY,//游历事件一键表

        #endregion

		HERO_RECOMMEND,//骑士推荐表

		#region 小游戏

		MINI_GAME_MAIN,//小游戏主表
		PUZZLE_GAME,//拼图游戏表
		FIND_THINGS_GAME,//找东西游戏表
		TAKE_THINGS_SEQUENTIALLY_GAME,//顺序取东西游戏表
		QTE_GAME,//QTE游戏表
		QTE_CLICK_OPPORTUNITY_GAME,//Qte点击时机小游戏表
		DRAG_BOX_GAME,//

		#endregion
		
		SIMPLE_COMIC,//简易漫画表

		MARKET_SHOP,//集市店铺表
		MARKET_SHOP_LVL,//集市店铺等级表

		MARQUEE,//跑马灯表
		MARQUEE_POS,//跑马灯位置表
		
		CHAT_EMOTE_GROUP,//聊天表情组
		CHAT_EMOTE_ITEM,//聊天表情
		
		CUTE_ACTOR,//Q版形象表
		CAT_BUBBLE,//猫咪气泡表
		
		ADD_PACK,//增量包表
		ADD_PACK_PATHS,//增量包路径表

		COUNTRY_AREA,//国家大区表

		ANNOUNCEMENT_BANNER,//运营公告海报图表
        ANNOUNCEMENT_JUMP,//运营公告游戏内跳转表
        ANNOUNCEMENT_TAB,//运营公告页签图表

		QUESTIONNAIRE,//问卷调查
		GUILD_LEVEL,//联盟等级表
		GUILD_POSITION,//联盟职位表
		GUILD_FLAG,//联盟旗帜表
		GUILD_CONSTRUCT,//联盟建设表
		GUILD_LOG,//联盟日志表
		GUILD_JOIN_LIMIT,//加盟限制表
		GUILD_CONSTRUCT_REWARD,//联盟捐赠进度奖励表
		GUILD_RANDOM_ENTRUST,//联盟杂物委托表
		GUILD_RANDOM_ENTRUST_QUALITY,//联盟杂物委托品质表
		
		BUILDING,//建筑表
		BUSINESS_BUILDING,//经营建筑表
		BUSINESS_BUILDING_HIRE_COST,//经营建筑雇佣费用表
		BUSINESS_BUILDING_LEVEL,//经营建筑等级表
		BUSINESS_BUILDING_VIDEO_GROUP,//经营建筑视频组表
		FARMING_BUILDING,//农场建筑表
		FARMING_BUILDING_LEVEL,//农场建筑等级表
		BUSINESS_BUILDING_DEVELOP,//经营建筑业务表
		BUSINESS_BUILDING_PRODUCT,//经营建筑产品表
		SPECIAL_ITEM,//特殊物品表
		COMMENT_RANDOM_GROUP,//关卡弹幕表
		EQUIP,//藏品表
		CHAPTER_COST,//章节金币消耗倍率配表
		CHAPTER_BUILD_UNLOCK,//章节建筑解锁表
		CHAPTER_BOSS_STYLE,//章节boss表现表
		CHAPTER_NODE_STYLE,//章节地图节点表现表
		COMMENT_PREFAB_RANDOM,//弹幕组随机表
		COMMENT_NAME_RANDOM,//弹幕名字随机表
		COMMENT_ICON_RANDOM,//弹幕头像随机表
		
		GACHA_POOL,//抽卡卡池表
		GACHA_ITEM,//抽卡道具表
		GACHA_ITEM_SHOW_INFO,//抽卡道具展示信息表
		GACHA_POOL_STEP,//抽卡阶段表
		GACHA_QUALITY_WEIGHT,//抽卡品质权重表
		GACHA_GUARANTEE,//抽卡保底表
		
		RECRUIT,//招募表
		RECRUIT_SHOP,//招募商店表
		ARENA_SELECT_ATTACK_CONSUME,//竞技场指定谈判道具表
		ARENA_STATION_LEVEL,//竞技场贸易站等级表
		ARENA_BUFF,//竞技场临时增益表
		ARENA_ROUND_REWARD,//竞技场轮次奖励表
		ARENA_FINAL_REWARD,//竞技场最终奖励表
		ARENA_BOT_RANDOM,//竞技场机器人随机表
		ARENA_BOT_TEMPLATE,//竞技场机器人模板表

		STAGE_GOAL_BIG_STEP,//大阶段表
        STAGE_GOAL,//阶段
		STAGE_GOAL_TASK,//阶段目标
		COMMON_TARGET_REWARD,//通用目标奖励表
		COMMON_REFRESH,//通用刷新表
		TOWER_CHAPTER,//爬塔表
		TOWER_CHAPTER_STAGE,//爬塔关卡阶段表
		TOWER_CHAPTER_STAGE_SHOW,//爬塔调整展示阶梯表
		ACTIVITY_RANK_RUSH,//活动限时冲榜表
		ACTIVITY_PLAN,//活动计划表
		MIDDAY_DUNGEON_WAVE,//午间副本Boss波次表
		MIDDAY_DUNGEON_BOX,//午间副本宝箱表
		SERVER_START_DAYS,//副本开服天数相关表
		
		EVENING_DUNGEON_RANK_REWARD,//晚间副本排行榜奖励表
		EVENING_DUNGEON_DAMAGE_RATIO,//晚间副本伤害比率表
		EVENING_DUNGEON_BOSS_BLOOD_ADD,//晚间副本boss血量增加表
		PLAYER_SKIN,//玩家皮肤表
		PLAYER_SKIN_LEVEL,//玩家皮肤等级表
		PLAYER_TITLE_LIMIT_GROUP,//玩家限时称号分组表
		PLAYER_TITLE_PREFIX,//玩家组合称号前缀表
		PLAYER_TITLE_SUFFIX,//玩家组合称号后缀表
		PLAYER_TITLE_BG,//玩家组合称号底框表
		EARNING_GOAL_REWARD,//千万目标奖励
		EARNING_GOAL_HONOR_REWARD,//千万目标荣耀奖励表
		SEVEN_DAY_LOGIN,//七日登录
		SHOP_MAIN,//商店总表
		BAG_ITEM_ACTIVITY,//活动道具表
		ACTIVITY_SHOP,//活动商店表
		ACTIVITY_SHOP_ITEM,//活动商店道具表
		CRYSTAL_GIFT_PACK_GROUP,//钻石礼包表
		CRYSTAL_GIFT_PACK,//钻石礼包表
		ACTIVITY_CURRENCY,//活动兑换卷表
		ACTIVITY_PREFAB_SKIN,//活动换皮表
		REGULAR_EVENT,//万能活动表
		PLAYER_CREATE_PLAYER_PREFAB,//创角预设表
		
		SEVEN_DAY_GOALS_TASK,//开服七日任务模板表
		SEVEN_DAY_GOALS_TASK_REWARD,//开服七日每天的任务表
		SEVEN_DAY_GOALS_STEP_REWARD,//开服七日分数奖励表
		SEVEN_DAY_GOALS_GIFT_PACK,//开服七日礼包表
		TILEMATCH_ACTIVITY,// 三消活动表
		NUMMERGE_ACTIVITY,// 2048 活动表
		ACTIVITY_CENTER,//活动中心表
		COUNTDOWN_EVENT,//倒计时事件表
		COUNTDOWN_EVENT_TASK,//倒计时事件任务表
		CONSORT_CHAT_DIALOGUE,//妃子预设对话表
		CONSORT_CHAT_DIALOGUE_SENTENCE,//妃子预设句子表
		CONSORT_CHAT_RESPONSE_OPTION,//妃子预设选项表
		CONSORT_CHAT_AI,//妃子AI对话表
		CONSORT_CHAT_MOMENTS,//妃子朋友圈表
		CONSORT_CHAT_IMAGE_GROUP,//妃子朋友圈照片表
		CONSORT_CHAT_AI_CODE,//妃子AI角色码表
		INN_LEVEL,//旅店等级表
		INN_MEDAL_LEVEL,//旅店奖牌等级表
		INN_STATION,//旅店设施表
		INN_STATION_LEVEL,//旅店设施等级表
		INN_DISH,//旅店菜品表
		INN_DISH_LEVEL,//旅店菜品等级表
		INN_GUEST,//旅店客人表
		INN_SPECIAL_GUEST,//旅店特殊客人表
		INN_RECIPE,//旅店菜谱表
		INN_GIFT,//旅店珍宝表
		INN_GIFT_LEVEL,//旅店珍宝等级表
		INN_GIFT_UPGRADE_COST,//旅店珍宝等级表
		MUSEUM_ITEM,//博物馆表
		MUSEUM_ITEM_LEVEL,//博物馆表
		MUSEUM_ITEM_UPGRADE_COST,//博物馆表
		HIRE,//招聘体验表
		SYSTEM_QUEST,//系统任务表
		TOWER_RESEARCH,//爬塔研究表
		GIFT_PACK,//礼包表
		GIFT_PACK_GROUP,//礼包组表
		PAY,//支付档位表
		TREASURE_HUNT_ORE,//太空寻宝 - 矿石表
		TREASURE_HUNT_TREASURE,//太空寻宝 - 奇物表
		TREASURE_HUNT_LAB,//太空寻宝 - 实验室表
		TREASURE_HUNT_AREA,//太空寻宝 - 太空区域表
		TREASURE_HUNT_CATALOG_TAB,//太空寻宝 - 图鉴页签表
		TREASURE_HUNT_STATION_LVL,//太空寻宝 - 太空舱等级表
		TREASURE_HUNT_SKILL,//太空寻宝 - 技能表
		TREASURE_HUNT_SKILL_LEVEL,//太空寻宝 - 技能等级表
		TREASURE_HUNT_COMPOSITE_CATALOG,//太空寻宝 - 组合图鉴表
		GRAVE_MAIN,//杰出者大厅
		GRAVE_TYPE,//杰出者类型
		PLAYER_BUFF_EVENT,//玩家buff事件表
		TREASURE_HUNT_AREA_DISTANCE,//寻宝区域距离表
		SYSTEM_QUEST_GROUP,//系统任务组表
		GUILD_DUNGEON,//联盟PVE副本
		GUILD_DUNGEON_LVL,//联盟PVE副本等级表
		GUILD_DUNGEON_MONSTER,//联盟PVE副本怪物表
		GUILD_DUNGEON_MONSTER_SHOW,//联盟PVE副本怪物展示
		MARS_BUILDING,//火星建筑
		MARS_BUILDING_LEVEL,//火星建筑等级
		MARS_BUILDING_HOME_LEVEL,//火星主基地等级
		MARS_BUILDING_EQUIPMENT_BELONG,//火星建筑部件归属
		MARS_BUILDING_SETTLE_LEVEL,//火星建筑派遣等级
		MARS_EQUIPMENT,//火星部件
		MARS_EQUIPMENT_LEVEL,//火星部件等级
		MARS_EQUIPMENT_ENERGY_LEVEL,//火星部件能源等级
		MARS_EQUIPMENT_LIVING_LEVEL,//火星部件生活等级
		MARS_EQUIPMENT_FOOD_LEVEL,//火星部件食物等级
		MARS_EQUIPMENT_HOSPITAL_LEVEL,//火星部件医院等级
		MARS_BAG_ITEM_TIME_REDUCE,//火星背包物品时间减少
		MARS_GO_ROUTE,//火星航行表
		MARS_GO_ROUTE_LOG,//火星航行日志表
		MARS_INTELLIGENT_CONTROL,//火星智能控制表
		MARS_SATISFACTION_DEGREE,//火星满意度表
		MARS_PEOPLE_LETTER,//火星人民信件表
		MARS_PEOPLE_HELP,//火星居民求助表
		MARS_PEOPLE_CHOICE_HELP,//火星居民选择求助表
		MARS_PEOPLE_REWARD_HELP,//火星居民奖励求助表
		MARS_EVENT,//火星基地事件表
		MARS_EVENT_TRIGGER_PER,//火星基地事件触发概率表
		MARS_IMMIGRATION,//火星移民表
		MARS_IMMIGRATION_PER,//火星移民概率表（仅服务端）
		CONSORT_MOMENTS_BG_GROUP,//妃子朋友圈背景组表
		CONSORT_MOMENTS_CONSORT_GROUP,//妃子朋友圈妃子图片组表
		CONSORT_MOMENTS_CONSORT,//妃子朋友圈妃子图片表
		CONSORT_MOMENTS_BG,//妃子朋友圈背景图片表
		GUILD_COOPERATE_AREA,//公会协作区域表
		GUILD_COOPERATE_AREA_POS,//公会协作据点表
		TREASURE_HUNT_TREASURE_OUTPUT,//太空寻宝奇物产出表
		RED_MONITOR,//红点监听表
		MARS_BUILDING_CONDITION,//火星建筑建造条件表
		CHILD_VOICE_GROUP,//子嗣配音组表
		MAIL_PLAN,//邮件计划表
		INN_SPECIAL_GUEST_CHOICE,//特殊客人选择表
		INN_SPECIAL_GUEST_CHOICE_OPTION,//特殊客人选择选项表
		VIP,//VIP表
		MARS_EXPLORE_LVL,//火星探索等级表
		MARS_EXPLORE_POS,//火星探索位置表
		MARS_EXPLORE_EVENT,//火星探索事件表
		MARS_EXPLORE_EVENT_BATTLE,//火星探索战斗事件表
		MARS_EXPLORE_EVENT_COLLECT,//火星探索收集事件表
		MARS_EXPLORE_EVENT_BOSS,//火星探索PVE事件表
		MARS_EXPLORE_TEAM,//火星探索队伍表
		MARS_TECHNOLOGY,//火星科技表
		MARS_TECHNOLOGY_LEVEL,//火星科技等级表
		GIFT_PACK_EXTRA_GAIN,//礼包额外获得表
		FIRST_RECHARGE_DAY,//首充天数表
		RECHARGE_REBATE_GROUP,//充值返利组表
		RECHARGE_REBATE_STEP,//充值返利阶段表
		MARS_TECHNOLOGY_TYPE,//火星科技类型表
		PLAYER_PROPERTY_SHOW,//玩家属性展示表
		MARS_PROPERTY_SHOW, //火星属性展示表
		MARS_EXPLORE_MINE,//火星探索矿点表
		MARS_BAG_ITEM_TIME_TYPE,//火星道具时间类型表
		MARS_EXPLORE_COLLECT_BONUS,//火星探索收集奖励表
		PRIVILEGE_CARD,//权益卡表
		PLAYER_PERMISSIONS,//玩家权限表
		PUSH_GIFT_GROUP,//推送礼包组表
		PUSH_GIFT_PACK,//推送礼包表
		PUSH_GIFT_ITEM_TRIGGER,//缺少物品触发推送礼包表
		MARS_ALL_BUILDING,//火星所有建筑表
		GUILD_BOX,//联盟宝箱
		PLAYER_FOREVER_ADD,//玩家永久加成表
		GUILD_BOX_EVENT,//联盟宝箱事件表
		ACTIVITY_FUND,//基金主表
		ACTIVITY_FUND_LEVEL,//基金等级表
		ACTIVITY_FUND_STEP,//基金阶段表
		ACTIVITY_FUND_TASK,//基金任务表
		CHAT_SYSTEM_LOG,//聊天系统通知表
		RANK_GIFT_PACK,//
		RUSH_EXCHANGE,//限时兑换
		RUSH_EXCHANGE_ITEM,//限时兑换物品价值
		RUSH_EXCHANGE_GROUP,//限时兑换组
		PLAYER_ROOM_SKIN,//玩家卧室皮肤表
		ACTIVITY_TEAM,//活动组队表
		PERFORM_GROUP,//通用表现组主表
		PERFORM_GROUP_ITEM,//通用表现组子项表
		PERFORM_GROUP_DIALOGUE,//通用表现组对话类型子表
    }
}