using System;
using System.Collections.Generic;
using UnityEngine;

/****************
 * 连接对应平台的配置类型
 **/
public enum EWCGPlatType
{
    NONE,
    CUSTOM = 50,             //自定义
    SELF = 100,               //自己

    MASTER = 200,           //内服        
    INTERNAL = 201,           //内服   
    EXTERNAL = 202,           //内服   

    DEV_MARK = 210,//马克
    DEV_COOPER = 211,//杨总

    MASTER_CDN = 250,//走cdn的master
    INTERNAL_CDN = 251,//走cdn的internal
    EXTERNAL_CDN = 252,//走cdn的external

    IN_TEST = 300,            //内部测试服    
    OUT_TEST = 301,            //外部部测试服    

    EXTERNAL_CHECK = 400,     //送审服

    OFFICIAL_CHINA = 500,     //正式服-国内   
    OFFICIAL_EURP = 501,      //正式服-欧洲  
    OFFICIAL_AMER = 502,      //正式服-美洲

    MJ_GOOGLE_TEST_US = 600,//封测正式服-谷歌-美洲
    MJ_GOOGLE_TEST_RU = 601,//封测正式服-谷歌-俄罗斯
    MJ_GOOGLE_US = 602,//正式服-谷歌-美洲
    MJ_GOOGLE_RU = 603,//正式服-谷歌-俄罗斯
    MJ_HUAWEI = 604,//正式服-华为

    QA_CUST = 1000,     //QA专属

    EXTRA_PLAT,         //额外的平台信息
}


public enum EUIQueueStageType
{
    NONE,
    LOGIN, //登录部分
    MAIN,//游戏主界面部分
    BATTLE,//战斗场景
    MINI_GAME,//小游戏相关节点
}


/** 游戏内Layer对应枚举 */
public enum ENPLayer
{
    DEFAULT,
    TRANSPARENT_FX,
    IGNORE_RAYCAST,
    LAYER_3,
    WATER,
    UI,
    UI_Ignore_RT,
    LAYER_7,

    //--- 以下可修改
    POST_PROCESSING,
    LAYER_9,
    GO_TRIGGER,
    GAME_UNIT,
    GAME_UNIT_SHADOW,
    GAME_ACTOR,
    GAME_SCENE,
    MOVE_AGENT,
    LAYER_16,
    GAME_OVERLAY_VFX,
    LAYER_18,
    SCREEN_LIGHTS,
    GAME_IGNORE_LAYER,
    GAME_SPECIAL_ACTOR,//特殊展示单位
    GAME_MINI_MAP_UNIT,
    GAME_MINI_MAP_AND_UNIT,//同时在大地图以及右上角展示的层级
    SHOWCASE,
    GAME_SCENE_ENTIY,
    GAME_SCENE_DISEMBODIED_OUTLINE,
    GAME_SCENE_ENTIY_OUTLINE,
    LAYER_28,
    LAYER_29,
    GAME_FIELD_OF_VIEW,
    UI_TOP,
}


/** 游戏内Scene的类型 */
public enum ENPSceneType
{
    NONE,
    UI_SCENE,       //UI视图
    TD_SCENE,       //3D视图
}

/*************
 * Gate服务器的客户端连接类型
 **/
public enum ENPGSLoginType
{
    NORMAL,
    WITH_RELOGIN_KEY,
}

//玩家状态
public enum ENPPlayerCompType
{
    NONE,
    BASIC_INFO,
    FRIEND,
    ACHIEVE,
    TUTORIAL, //引导数据，用clientdata方式存储
    RESOURCE,//玩家资源
    BAG,//背包
    MISSION,//战役
    BUILDING,//建筑信息
    PLAYER_BUFF,//玩家buff
    PLAYER_TITLE,//玩家称号
    PLAYER_ICON,//玩家头像
    PLAYER_ICON_BGK,//玩家头像框
    PLAYER_BUBBLE,//气泡框
    CHAT,//聊天组件
    MAIL,//邮件组件
    SPACE,//世界探索
    HERO,//骑士
    // COOKING,//烹饪
    RANK_COMMON,//通用排行榜
    LAZY_CD,//CD组件
    QUEST,//任务
    MUSEUM,//博物馆
    PLAYER_AVATAR,//玩家形象
    PLAYER_AVATAR_SNAPSHOT,//玩家形象预设
    RECORD,//次数记录组件
    PET_MAP,//宠物图鉴组件
    PLAYER_FLOURISH,//繁荣度
    FUNC_UNLOCK,//功能解锁
    LAYOUT,//阵型系统
    DELAY_MSG,//延迟消息
    // DIGGINGS,//大地图矿区
    EVENT_RECORD,//玩家行为记录组件
    SPACE_INTERACTION_RECORD,//大地图交互管理类
    DAILY_QUEST,//日常 周常任务
    MINI_GAME,//小游戏组件
    FIXED_CD,//固定时间恢复CD组件
    SHOP,//商店组件
    ARENA,//竞技场
    OFF_LINE_REWARD,//离线奖励组件
    TREASURE_MAP,//藏宝图组件
    // STELLA,//星座图腾组件
    SPACE_SIDE,//大地图侧边信息
    PRIVATE_ITEM,//个人物件
    // WISH_QUEST,//心愿任务
    // RES_ISLAND,//资源岛
    // HATCH_SEED_V2,//孵化V2
    DAILY_CHECK,//签到
    CHILD,//子嗣
    CONSORT,//情人
    LEVY,//征收
    Chapter,//关卡
    COLLEGE,//大学
    MAKE_FACE,//捏脸
    COMMON_ACTIVITY,//活动组件
    CHILD_ADULT,//成年子嗣
    DINNER,//宴会
    ANECDOTE,//政务
    TRAVEL,//游历
    HERO_RECOMMEND,//骑士推荐
    MARKET,//集市
    MARQUEE,//跑马灯
    CUTE_ACTOR,//Q版形象
    WEEK_CARD,//周卡
    AVATAR_GACHA,//抽卡
    QUESTIONNAIRE,//问卷调查
    GUILD,//联盟
    SPECIAL_ITEM,
    EQIUP,//藏品
    GACHA,//抽卡
    RECRUIT,//招募
    STATION,//贸易站
    STAGE_GOAL,//阶段目标
    COMMON_TARGET_REWARD,//通用目标奖励
    COMMON_REFRESH,//通用刷新
    TOWER,//爬塔
    COMMON_ITEM_COUNT,//通用道具数量
    MIDDAY_DUNGEON,//中午副本
    PLAYER_SKIN,//玩家皮肤
    EVENING_DUNGEON,//晚间副本
    SEVEN_DAY_LOGIN,//七天登录
    SEVEN_DAY_GOALS,//七日目标
    EARNING_GOAL,//千万目标
    COUNTDOWN_EVENT,//倒计时事件
    CONSORT_CHAT,//情人聊天
    INN,//旅店
    SYSTEM_QUEST,//系统任务
    GIFT_PACK,//礼包
    TREASURE_HUNT,//太空寻宝
    GRAVE,//杰出者大厅
    PAY_ORDER,//充值订单
    MARS,//火星基地
    GUILD_DUNGEON, //联盟PVE副本
    GUILD_COOPERATE,//联盟协作任务
    COMMON_ACTIVITY_HOT_REF,//通用活动热门配表
    RECHARGE_REBATE,//充值返利
    RED_DOT,//红点
    PRIVILEGE_CARD,//特权卡
    PLAYER_PERMISSIONS,//玩家权限
    PUSH_GIFT,//推送礼包
    GUILD_MARS_HELP, //联盟火星互助
    PLAYER_FOREVER_ADD,//玩家永久加成
    GUILD_BOX, //联盟宝箱
    FUND, //基金
    RANK_GIFT_PACK, //排行榜礼包
    RUSH_EXCHANGE,//限时兑换
    PLAYER_ROOM_SKIN,//玩家房间皮肤
    LOVER_COLLECT,//情人收集
}


/*语言枚举 注意该枚举需要跟PHP的枚举对应，即PHP语言枚举：
 * (1=>"English",2=>"China",3=>"Taiwan",4=>"Russia",5=>"Korea",6=>"Japan",7=>"Arabic",8=>"Turkey",9=>"German",10=>"French",11=>"Spanish",12=>"Portuguese",13=>"Italian");
 */
public enum ENPLanguage
{
    NONE,
    EN_US = 1,//英语(美国)
    ZH_CN,//2简体中文
    ZH_TW,//3中国台湾
    RU_RU,//4俄语(俄罗斯)
    KO_KR,//5韩国
    JA_JP,//6日文
    AR_AR,//7阿拉伯
    TR_TR,//8土耳其语
    DE_DE,//9德语
    FR_FR,//10法语    
    ES_ES,//11西班牙语
    PT_PT,//12葡萄牙语
    IT_IT,//13意大利语（意大利）
}
public static class ENPLanguageExtension
{
    /// <summary>
    /// 转换成后台统一语言代码
    /// http://public-api.mjyx.com//web/#/p/705c9e613e35eec1645c016ffbb495b2
    /// </summary>
    public static string toPHPLanguageCode(this ENPLanguage _language)
    {
        switch (_language)
        {
            case ENPLanguage.EN_US:
                return "en";
            case ENPLanguage.ZH_CN:
                return "zh-CN";
            case ENPLanguage.ZH_TW:
                return "zh-TW";
            case ENPLanguage.RU_RU:
                return "ru";
            case ENPLanguage.KO_KR:
                return "ko";
            case ENPLanguage.JA_JP:
                return "ja";
            case ENPLanguage.AR_AR:
                return "ar";
            case ENPLanguage.TR_TR:
                return "tr";
            case ENPLanguage.DE_DE:
                return "de";
            case ENPLanguage.FR_FR:
                return "fr";
            case ENPLanguage.ES_ES:
                return "es";
            case ENPLanguage.PT_PT:
                return "pt";
            case ENPLanguage.IT_IT:
                return "it";
            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// 将后台统一语言代码转成我们游戏的语言枚举
    /// http://public-api.mjyx.com//web/#/p/705c9e613e35eec1645c016ffbb495b2
    /// </summary>
    public static ENPLanguage phpLanguageCodeToLanguageEnum(string _phpLanguageCode)
    {
        switch (_phpLanguageCode)
        {
            case "en":
                return ENPLanguage.EN_US;
            case "zh-CN":
                return ENPLanguage.ZH_CN;
            case "zh-TW":
                return ENPLanguage.ZH_TW;
            case "ko":
                return ENPLanguage.KO_KR;
            case "ja":
                return ENPLanguage.JA_JP;
            case "ru":
                return ENPLanguage.RU_RU;
            case "ar":
                return ENPLanguage.AR_AR;
            case "tr":
                return ENPLanguage.TR_TR;
            case "de":
                return ENPLanguage.DE_DE;
            case "fr":
                return ENPLanguage.FR_FR;
            case "es":
                return ENPLanguage.ES_ES;
            case "pt":
                return ENPLanguage.PT_PT;
            case "it":
                return ENPLanguage.IT_IT;
            default:
                return ENPLanguage.NONE;
        }
    }
}


/*****************
 * 属性类型枚举
 **/
public enum ENPPropertyType
{
    NONE,
    HP,                     //生命 
    HP_PER,                 //生命万分比
    SP,                     //能量值
    SP_PER,                 //能量值万分比
    ATK,                    //物理攻击力
    ATK_PER,                //物理攻击力万分比
    POW,                    //法术强度
    POW_PER,                //法术强度万分比
    ARMOR,                  //护甲
    ARMOR_PER,              //护甲万分比
    MAGIC_ARMOR,              //魔抗
    MAGIC_ARMOR_PER,          //魔抗万分比
    ARMOR_IGNORE,           //护甲穿透
    ARMOR_IGNORE_PER,       //护甲穿透万分比
    MAGIC_ARMOR_IGNORE,       //魔抗穿透
    MAGIC_ARMOR_IGNORE_PER,   //魔抗穿透万分比
    HEALED,                   //被治疗效果加强值
    HEALED_PER,               //被治疗效果加强万分比
    DMG_REDUCE,             //伤害减免绝对值
    DMG_REDUCE_RATE,        //伤害减免系数 
    PHYS_REDUCE,            //物理伤害减免绝对值
    PHYS_REDUCE_RATE,       //物理伤害减免系数 
    MAGIC_REDUCE,           //魔法伤害减免绝对值
    MAGIC_REDUCE_RATE,      //魔法伤害减免系数 
    ATK_SPEED,              //攻击速度加成万分比
    MOVE_SPEED,             //移动速度
    MOVE_SPEED_PER,         //移动速度加成万分比
    HP_STEAL,               //生命偷取绝对值
    HP_STEAL_PER,           //生命偷取百分比
    ATK_RANGE,              //攻击范围（100 = 1米）

    MODEL_SCALE,        //模型缩放万分比
    LOCK_RANGE,        //锁敌范围固定加成
    PRODUCE_PER,       //生产速度加成

    CRIT_V,               //暴击值
    CRIT_PER,               //暴击率
    CRIT_DEF_V,            //抗暴值
    CRIT_DEF_PER,           //抗暴率
    CRIT_DMG_PER,           //暴击伤害
    CRIT_DMG_DEF_PER,         //暴击伤害减免

    DODGE_V,              //闪避值
    DODGE_PER,          //闪避率
    HIT_V,                //命中值
    HIT_PER,            //命中率

    WEIGHT,              //重量

    ADD_MOVE_SPEED,         //移动速度实际数值加成，只用于速度的计算，方便策划调整 
    SUMMON_TIME,        //召唤时间
    DEPLOY_TIME,        //放置时间

    DMG_ADD_RATE,           //伤害增加系数
    PHYS_ADD_RATE,      //物理伤害增加系数
    MAGIC_ADD_RATE,     //魔法伤害增加系数
}

/// <summary>
/// 关卡类型
/// </summary>
public enum ENPMissionType {
    NORMAL,//普通
    BOSS,//精英
}

public enum ENPCityBuildCardState // 枚举值和排序有关，越小越前面
{
    NONE,
    CAN_BUY,                // 正常可购买状态
    CAN_ON,                // 正常可放置状态
    INSUFFICENT_COST,       // 花费不足的状态
    CONDITION_LIMIT,        // 条件限制
    QUANTITY_LIMIT,         // 数量限制
}

/// <summary>
/// 引导高亮位置类型
/// </summary>
public enum ETutorialHighlightLocationType
{
    NONE,
    CHAPTER_MAP_NODE,//当前关卡地图node位置
    HERO_MAIN_LIST_P,//骑士主页面列表位置
    CHILD_MAIN_LIST_P,//子嗣列表位置
    BAG_MAIN_USE_BAG_PAGE_ITEM_P,//背包主页面可使用物品位置
    BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN,//背包主页面可使用物品使用栏使用按钮位置
    SHOP_LIST,//商店列表位置
    HOME_ENTRY_POINT,//主城入口entry_point位置
    GUILD_COOPERATE_REWARD_POS_CAN_GET,//联盟协作奖励据点可领取奖励位置
    GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT,//联盟协作属性据点可建造位置
    CONSORT_MAIN_LIST_P, //妃子主页面列表位置
}

/****************
 * 教程获取对应信息窗口位置的处理
 **/
public enum EWCGTutorialNoticeRectType
{
    NONE,
    ACTOR_SCR_P,        //根据单位的绝对ID获取对应单位的UI位置
    MAIN_PET_LIST_P,        //根据界面索引index获取对应列表里代表pet卡牌的UI位置
    CITY_BUILD_P, //根据建筑id找到第一个建筑位置
    HERO_MAIN_LIST_P,//骑士主页面列表位置
    CHILD_MAIN_LIST_P,//子嗣列表位置
    SHOP_LIST,//商店列表位置
    GUILD_COOPERATE_REWARD_POS_CAN_GET,//联盟协作奖励据点可领取奖励位置
    GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT,//联盟协作属性据点可建造位置
    CONSORT_MAIN_LIST_P,//妃子主页面列表位置
}

public enum ENPUpgradeState
{
    NONE,
    NORMAL,         // 正常状态
    LEVEL_MAX,      // 满级
}

/*****************
 * 引导触发的类型
 **/
public enum ENPTutorialTriggerType
{
    NONE,
    [InspectorName("主城摄像头移动完成 CITY_MOVE_FOCUS_DONE")]
    CITY_MOVE_FOCUS_DONE,
    [InspectorName("操作失败 FAIL")]
    FAIL,
    [InspectorName("对话结束 DIALOG_END")]
    DIALOG_END,
    [InspectorName("引导下一步的消息，用于专属引导处理的按钮 TURORIAL_NEXT")]
    TURORIAL_NEXT,
    [InspectorName("小游戏结束消息，引导会监听此消息 MINI_GAME_END")]
    MINI_GAME_END,
    [InspectorName("漫画结束 COMIC_END")]
    COMIC_END,
    [InspectorName("取名成功 CREATE_PLAYER_NAME_SCU")]
    CREATE_PLAYER_NAME_SCU,
    [InspectorName("远程效果处理完成 REMOTE_EFFECT_DEAL_DONE")]
    REMOTE_EFFECT_DEAL_DONE,
    [InspectorName("主城场景进入完成 CITY_SCENE_ENTER_DONE")]
    CITY_SCENE_ENTER_DONE,
    [InspectorName("卧室场景进入完成 ROOM_SCENE_ENTER_DONE")]
    ROOM_SCENE_ENTER_DONE,
    [InspectorName("系统解锁表现播放结束 ON_FUNC_UNLOCK_SHOW_DONE")]
    ON_FUNC_UNLOCK_SHOW_DONE,
    [InspectorName("关卡进入完成(云关闭后) CHAPTER_ENTER_DONE")]
    CHAPTER_ENTER_DONE,
    [InspectorName("子嗣首次赐名成功 CHILD_FIRST_SET_NAME_SUC")]
    CHILD_FIRST_SET_NAME_SUC,
    [InspectorName("进入主城时的loading窗口隐藏完成时 ENTER_CITY_LOADING_WND_HIDE")]
    ENTER_CITY_LOADING_WND_HIDE,
    [InspectorName("开篇剧情视频播放结束 VIDEO_END")]
    VIDEO_END,
    [InspectorName("建筑建造表现完成 BUILD_BUILD_EFFECT_END")]
    BUILD_BUILD_EFFECT_END,
    [InspectorName("事件结束表现完成 ANECDOTE_END_EFFECT_END")]
    ANECDOTE_END_EFFECT_END,
    [InspectorName("建筑建造表现窗口完成 BUILD_BUILD_EFFECT_WND_END")]
    BUILD_BUILD_EFFECT_WND_END,
    [InspectorName("收集完成 HARVEST_DONE")]
    HARVEST_DONE,
    [InspectorName("选择预设成功 SELECT_PREFAB_SCU")]
    SELECT_PREFAB_SCU,
    [InspectorName("招聘体验完成 HIRE_MAIN_CLOSE")]
    HIRE_MAIN_CLOSE,
    [InspectorName("主线任务领奖完成 SYSTEM_QUEST_REWARD_DONE")]
    SYSTEM_QUEST_REWARD_DONE,
    [InspectorName("抽卡表现完成 SUMMON_SHOW_DONE")]
    SUMMON_SHOW_DONE,
    [InspectorName("午间副本入场表现播完 MIDDAY_DUNGEON_BATTLE_SHOW_DONE")]
    MIDDAY_DUNGEON_BATTLE_SHOW_DONE,
    [InspectorName("骑士主页面进入完成 HERO_MAIN_WND_ENTER_DONE")]
    HERO_MAIN_WND_ENTER_DONE,
    [InspectorName("阶段目标到达新阶段动画播完 STAGE_GOAL_PLAY_NEW_STAGE_ANI_DONE")]
    STAGE_GOAL_PLAY_NEW_STAGE_ANI_DONE,
    [InspectorName("登录火星选中登录地点 MARS_SELECT_LANDING_AREA")]
    MARS_SELECT_LANDING_AREA,
    [InspectorName("火星基地收集奖励 MARS_COLLECT_HOME_REWARD")]
    MARS_COLLECT_HOME_REWARD,
    [InspectorName("情人获得表现完成 GAIN_CONSORT_SHOW_FINISH")]
    GAIN_CONSORT_SHOW_FINISH,
    [InspectorName("妃子指定邀约结果展示完成  CONSORT_APPOINT_CALL_SHOW_DONE")]
    CONSORT_APPOINT_CALL_SHOW_DONE,
    [InspectorName("竞技场主页面显示完成 ARENA_MAIN_SHOW")]
    ARENA_MAIN_SHOW,
    [InspectorName("联盟协作奖励据点加载完成 GUILD_COOPERATE_REWARD_POS_LOAD_DONE")]
    GUILD_COOPERATE_REWARD_POS_LOAD_DONE,
    [InspectorName("顾问列表加载完成 HERO_LIST_LOAD_DONE")]
    HERO_LIST_LOAD_DONE,
    [InspectorName("背包列表加载完成 BAG_LIST_LOAD_DONE")]
    BAG_LIST_LOAD_DONE,
    [InspectorName("经营事件处理完成 ANECDOTE_EVENT_PROCESS_END")]
    ANECDOTE_EVENT_PROCESS_END,
    [InspectorName("旅店结算奖励领取完成 INN_CASH_REGISTER_COLLECT_REWARD_DONE")] 
    INN_CASH_REGISTER_COLLECT_REWARD_DONE,
    [InspectorName("所有游历事件处理完成  TRAVEL_EVENT_DEAL_DONE")]
    ALL_TRAVEL_EVENT_DEAL_DONE,
}

//新手引导运动遮罩Element类型
public enum TutorialMoveMaskElementType
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT,
    MID,
}

public enum ENPCityBuildType
{
    NONE,
    FUNCTION_BUILDING,      //功能建筑
    RESOURCE_BUILDING,      //资源建筑
    DECORATION_BUILDING,    //装饰建筑
}

//ui stage 类型
public enum ENPUIStageType
{
    NONE,
    MAIN,
    BATTLE,
    PREPARE,
    PRELAYOUT,
    MOB_LAYOUT,
}

//客户端存储在服务器的数据类型
public enum ENPClientDataType
{
    [InspectorName("引导数据")]
    TUTORIAL,
    [InspectorName("功能解锁数据")]
    FUNC_UNLOCK,
    [InspectorName("妃子")]
    CONSORT,
    [InspectorName("跑马灯")]
    MARQUEE,
    [InspectorName("玩家升级")]
    PLAYER_UPGRADE,
    [InspectorName("商店")]
    SHOP,
    [InspectorName("抽卡")]
    AVATAR_GACHA,
    [InspectorName("游历")]
    TRAVEL, 
    [InspectorName("运营公告")]
    ANNOUNCEMENT,
    [InspectorName("联盟建设捐献")]
    GUILD_CONSTRUCT,
    [InspectorName("限时冲榜")]
    RANK_RUSH,
    [InspectorName("通用客户端数据")]
    COMMON_CLIENT_DATA,
}


//物品替换系统类型
public enum ENPInsteadItemType
{
    NONE,//所有情况都可以替换
    STARUP,//英雄升星
    TOKEN_SHOP,//代币商城
}

public enum ENPWildAudioType
{
    BG_MUSIC,   //野生背景音乐
    NORMAL_MUSIC,//野生音效
}

//材质类型
public enum ENPDefenseMaterialType
{
    NONE,
    WOOD,//木头
    METAL,//金属
    BODY,//血肉
    STONE,//石头
}

public enum ENPHitMaterialType
{
    NONE,
    WOOD_HEAVY,//木头--重
    METAL_HEAVY,//金属--重
    STONE_HEAVY,//石头--重
    BODY_HEAVY,//血肉--重
    METALBLADE_HEAVY,//金属刀刃--重
    WOOD_LIGHT,//木头--轻
    METAL_LIGHT,//金属--轻
    STONE_LIGHT,//石头--轻
    BODY_LIGHT,//血肉--轻
    METALBLADE_LIGHT,//金属刀刃--轻
    BOW_LIGHT,//弓箭--轻
    BOW_HEAVY,//弓箭-重
    MAGIC_LIGHT,//魔法--轻
    MAGIC_HEAVY,//魔法--重
    CLAW_LIGHT,//爪击--轻
    CLAW_HEAVY,//爪击--重
}

public enum ENPSpaceUnitLoadType
{
    NONE,
    DIRECTLY,   // 直接加载
    CACHE,      // 缓存加载
}

public enum EBuildingOpType
{
    NONE,
    INFO,//详情
    LEVEL_UP,//升级
    SPC_BTN,//特殊事件按钮
    REMOVE,//移除按钮
    REPAIR,//修复
}


/// <summary>
/// 时装显示数据类型
/// </summary>
public enum ENPAvatarShowDataType
{
    SINGLE,//单件时装
    SUIT,//套装
    SNAPSHOT,//预设
}

/// <summary>
/// 时装预设解锁类型
/// </summary>
public enum ENPAvatarSnapshotUnlockType 
{
    CONDITION_UNLOCK,//条件解锁，点击后有反馈的
    COST_UNLOCK,//消耗解锁
}

//收藏品筛选的枚举
public enum ENPMuseumItemToggleType
{
    NONE,
    QUALITY, // 品质
    SUIT, // 套装
    PROP, // 属性
    ACTIVITY, // 活动
    STAR_UP, // 升星状态
    
    //以下特殊筛选，使用默认筛选
    ItemChip,// 收藏品碎片筛选
}

//收藏品状态的枚举
public enum ENPMuseumItemStatType
{
    NONE,
    CAN_POUR, // 可注能
    CAN_UPGRADE, // 可升级
    CONDITION_LIMITE, // 升级条件限制
    LVL_MAX, // 已达最高级
}

//收藏品升星的状态的枚举
public enum ENPMuseumItemStarUpStatType
{
    NONE,
    COST_LIMIT, // 升星消耗限制
    CAN_STARUP, // 可升星
    STAR_MAX, // 已达最高星级
}

/// <summary>
/// 宠物资质评级枚举
/// </summary>
public enum ENPPetAttrEvaluation
{
    NONE,
    C,
    B,
    A,
    S,
    SS,
    SSS,
}

/// <summary>
/// 队列提示类型
/// </summary>
public enum ENPQueueDealerTipType
{
    FLOURISH,//繁荣度提示
    PET_PROP_TIP,//宠物属性提示
    PET_POWER_TIP,//宠物战力提示
    BUILD_DONE_TIP,//建造完成提示
    HERO_TOTAL_ATTR,//骑士总属性提示
}


/// <summary>
/// 建筑显示属性类型
/// </summary>
public enum ENPBuildingPropType
{
    FLOURISH,//繁荣度
    LEVEL,//等级
}

/// <summary>
/// 警告提示类型
/// </summary>
public enum ENPWarningType
{
    AVATAR_MAIN_QUIT_WITHOUT_SAVE,//衣柜未保存退出主界面
    AVATAR_SNAPSHOT_COVER_CONFIRM,//衣柜预设覆盖二次确认
    AVATAR_SNAPSHOT_UNLOCK_COST_CONFIRM,//衣柜解锁消耗二次确认
    AVATAR_EXPIRE_TIP,//衣柜时装过期提醒

    HATCH_USE_DIAMOND_QUICK_ITEM_CONFIRM,//孵化使用钻石加速二次确认
    PET_WASH_ATTR_CONFIRM,//宠物洗练资质二次确认

    MINI_GAME_QUEST_DROP_CONFIRM,//放弃悬赏任务二次确认
    MINI_GAME_QUEST_QUICK_FINISH_COST_CONFIRM,//悬赏任务一键完成消耗二次确认
    MINI_GAME_QUEST_REFRESH_CONFIRM,//悬赏任务刷新二次确认

    SHOP_FREE_REFRESH_CONFIRM,//商店免费刷新二次确认
    SHOP_COST_REFRESH_CONFIRM,//商店消耗刷新二次确认
    SHOP_BUY_ITEM_CONFIRM,//商店购买商品二次确认

    ARENA_RETREAT_CONFIRM,//比武擂台撤退确认
    ARENA_LOW_RANK_BATTLE_CONFIRM,//比武擂台向低排名的玩家，战斗二次确认
    ARENA_OTHER_ARENA_BATTLE_CONFIRM,//比武擂台已占领其他擂台，战斗二次确认

    TREASURE_MAP_USE_CONFIRM,//藏宝图使用二次确认

    MACHINE_SHARE_JOINED_GOTO_CONFIRM,//机关分享banner前往二次确认（已参与）
    DIGGINGS_SHARE_JOINED_GOTO_CONFIRM,//矿区分享banner前往二次确认（已参与）

    STELLA_SAVE__CONFIRM,//占星保存确认弹窗
    STELLA_GIVEUP__CONFIRM,//占星放弃确认弹窗

    DIGGINGS_BACK_CONFIRM,//矿区撤回二次确认弹窗
    MISSION_POWER_TIP_CONFIRM,//关卡战力不足二次确认弹窗
    
    HINT_MAP_TELEPORT,//区域提示地图的传送确认弹窗
    CHILD_TRAIN_CONFIRM,//子嗣训练二次确认
    
    CHAPTER_PVE_EVNET_LOSS_DEGREE_TIP,//关卡pve事件损耗比太大提示
    
    COMMON_PVE_EVENT_DISPATCH_NOTALLCONDENABLE_TIP,//通用派遣事件条件没有全部满足时提示

    COMMON_DIALOGUE_SKIP_CONFIRM,//通用对话跳过提示
    
    CHAPTER_BOSS_FIGHT_UNDERSTRENGTH_TIP,//关卡BOSS战, 战力不足提示

    ADD_NEW_COLLEGE_POS_COST_TIP,//扩增大学席位消耗提示

    NO_CHILD_CAN_TRAIN_CONFIRM,//没有可培养子嗣二次确认
    ADD_CHILD_POS_COST_TIP,//子嗣位置扩充消耗提示
    CHILD_TRAIN_RECOVER_COST_TIP,//子嗣培养恢复消耗提示
    CHILD_ONE_KEY_RECOVER_COST_TIP,//子嗣一键恢复消耗提示
    
    MARKET_SHOP_COST_ITEM_GET_REWARD_TIP,//集市店铺消耗道具领取奖励提示
    CHILD_MARRY_SERVER_REFRESH_COST_ITEM_TIP,//全服联姻刷新消耗物品提示
    CHILD_GRADUATE_SUCCESS,//子嗣毕业成功提示
    CHILD_STEP_UP_SUCCESS,//子嗣升学成功提示

    CHECK_LOW_FRAME_RATE,//检测低帧率提示

    EQUIP_NO_HERO_REBUILD,//藏品未佩戴到伙伴重铸时提示
    
    CONSORT_BUSINESS_SKILL_UNLOCK,//妃子经营技能解锁提示
    TREASURE_HUNT_GAMEPLAY_QUIT,//太空寻宝游戏退出
    CHAPTER_QUICK_FORWARD,//关卡快速前进
    
    MARS_TIME_REDUCE_OVERFLOW,// 火星减少时间溢出提示
    MARS_TIME_REDUCE_USED_GENERAL_ITEM_TIP,//火星时间减少使用了通用道具提示
    MARS_TIME_COMPLETE_NOW_CONFIRM,//火星时间立即完成确认
    MARS_RESIDENT_REPLENISH_PEOPLE_LIMIT,//火星居民补充人口上限提示
    MARS_EXPLORE_ATTACK_CONFIRM,//火星探索进攻确认
    
    ACTIVITY_MAIN_CITY_PUSH_NOTICE_TODAY_IGNORE,//活动主城推送弹窗今日不再弹出
    INN_CASH_REGISTER_REWARD_TODAY,//旅店结算奖励今日已查看
}

/// <summary>
/// 提示队列类型，只用于区分队列，不跟位置绑死，命名方便策划理解
/// </summary>
public enum ENPTipShowType
{
    CENTER,//默认
    SIDE,//侧边提示，成就、任务、建筑完工等
    TOP,//顶部提示
}

/// <summary>
/// 追踪任务类型
/// </summary>
public enum ENPFollowQuestType
{
	NONE,//无
	MAIN,//主线
	BRANCH,//支线
	MINI_GAME_QUEST,//悬赏任务
	TREASURE_MAP,//藏宝图
    WISH_QUEST,//心愿任务
}

/// actor的表现类型
/// </summary>
public enum ENPActorEffectType
{
    EFFECT1,//表现1
    EFFECT2,//表现2
    EFFECT3,//表现3
}

/// <summary>
/// NPC显示类型
/// </summary>
public enum ENPNPCType
{
    NONE,
    SELF,//玩家自身
    ACTOR,//Actor类型
    GO,//独立模型
}

/// <summary>
/// 比较常见的一些常规领取状态枚举
/// </summary>
public enum ENPCommonGetStat
{
    NONE,
    [InspectorName("CAN_GET（可以领取）")]
    CAN_GET,//可以领取
    [InspectorName("CAN_NOT_GET（还不能领取,指没达成或者没有奖励）")]
    CAN_NOT_GET,//还不能领取
    [InspectorName("HAS_GET（已经领取完奖励）")]
    HAS_GET,//已经领取完成
}

/// <summary>
/// 只区分是否可领取的状态
/// </summary>
public enum ENPCommonOnlyGetStat
{
    CAN_NOT_GET,//还不能领取
    CAN_GET,//可以领取
}

/// <summary>
/// 玩家模型的渲染类型
/// </summary>
public enum ENPPlayerRenderType
{
    NORMAL,
    STATUE,
}
/// <summary>
/// 建筑当前的显示状态
/// </summary>
public enum EBuildingShowType
{
    NONE,
    PLAN,           // 准备开始建造，还没建造
    BUILD,          // 当前还在建造
    CHECK_WAITING,  // 建造完了在等待确认
    HIDE,           // 隐藏显示
    NORMAL,         // 建筑正常状态
    START,          // 开始状态，会根据建筑当前的数据情况分配一个正确的状态
    SELECT,         // 被选择的状态
    BUILD_SELECT,   // 建造建筑时的选择状态
}
/// <summary>
/// 建筑的操作状态
/// </summary>
public enum ENPBuildingOperationType
{
    NONE,
    BUILD,
    SELECT,
}

/// <summary>
/// 相对一个聚会 玩家的所处状态
/// </summary>
public enum ENPPartyPlayerSata
{
    NONE, // 玩家与这个聚会无关
    OWNER,//玩家是聚会发起者
    JOINER,//玩家是聚会参与者
}

/// <summary>
/// 相对一个聚会座位 状态
/// </summary>
public enum ENPPartySeatSata
{
    NONE, 
    EMPTY,// 空闲
    SEATED,//已有玩家
}

/// <summary>
/// 一个聚会 状态
/// </summary>
public enum ENPPartySata
{
    NONE,//没有聚会
    EMPTY, //全部空闲
    SEATED_PART,//部分空闲
    SEATED_FULL,//座位已满
}


/// <summary>
/// 相对一个聚会座位 玩家的所处状态
/// </summary>
public enum ENPPartySeatPlayerSata
{
    CAN_DO_NOTHING, //玩家与这个聚会座位无关,就是不能攻击,不能加入,不在座位,已经在聚会中
    OWNER,//玩家是座位拥有者
    CAN_JOIN,//玩家是可以加入这个座位
    CAN_ATTACK,//可以攻击
    CAN_KICK,//可以踢人
    IN_PROTECTED,//保护罩保护中
}

/// <summary>
/// 摄像机移动边界类型枚举
/// </summary>
public enum ENPCameraMoveBoundaryType
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT,
}

/// <summary>
/// 宠物技能显示类型
/// </summary>
public enum ENPPetSkillShowType
{
    MAIN,//主动技能
    TALENT,//天赋技能
}

/// <summary>
/// 星座图腾的家族状态
/// </summary>
public enum ENPStellaFamilySata
{
    ACTIVATE,//激活
    DIS_ACTIVATE,//未激活
    WAIT_LAUNCH,//等待投放
}

/// <summary>
/// 星座图腾的家族占星状态
/// </summary>
public enum ENPStellaFamilyStellaSata
{
    UNLOCK,//占星已解锁
    LOCK,//占星未解锁
}

/// <summary>
/// 星座图腾的占星属性变化状态
/// </summary>
public enum ENPStellaAllAttrSata
{
    NONE,//没有变化
    ALL_UP,//全部属性上涨
    ALL_DOWN,//全部属性下降
    PART_UP_AND_DOWN,//部分上涨部分下降
}

/// <summary>
/// 星座图腾的占星属性变化状态
/// </summary>
public enum ENPStellaAttrSata
{
    NONE_VALUE,//没有数值
    UP,//属性上涨
    DOWN,//属性下降
    EQUAL,//没变化
}

public enum EAttachType
{
    NONE,       // 无效数值
    BE_CHILD,   // 附加成为子对象
    SAME_POSITION, // 只附加到和附加点相同世界坐标的地方
}

public enum EAttachmentItemType
{
    NONE,
    GAME_OBJECT,// GameObject
    SFX,        // 特效
}

/// <summary>
/// 关卡状态
/// </summary>
public enum ENPMissionStat
{
    HAS_PASS,//已经通关的关卡
    FIGHTING,//当前关卡
    WAITINT,//还未开始的关
}

/// <summary>
/// 资源 状态
/// </summary>
public enum ENPSomeResSata
{
    EMPTY, //没有资源
    PART,//部分资源
    FULL,//资源已满
}

/// <summary>
/// 聊天聚会状态
/// </summary>
public enum ENPChatPartyStat
{
    HAS_JOINED,//已经加入
    HAS_JOINED_OTHER,//已经加入其它聚会
    DIABLE,//不存在
    PART,//已有部分座位
    FULL,//已满
}

/// <summary>
/// 右上角上的item筛选类型
/// </summary>
public enum ENPSmallMapFilterType
{
    FUNCTION_BUILDING, // 功能点
    NPC,   //NPC
    TELEPORT,//传送点
    ARENA,//擂台点
    DIGGINGS,//矿点
    PARTY,//聚会
    PLAYER,//玩家
    COLLECTION,//采集点
    STONE,//奇石
    MECHANISM,//机关
    FOOD_MACHINE,//食神
    CATCH_AREA,//捕捉区域
    OTHER_PLAYER,//其它玩家
    LOCKED_AREA,//迷雾区域
}

/// <summary>
/// 玩家的聚会建筑状态
/// </summary>
public enum ENPPartyBuildingStat
{
    NONE,//没有聚会也没有奖励
    NONE_HAS_REWARD,//没有聚会 有奖励
    NONE_NO_REWARD,//没有聚会 没有奖励
    JOINED_HAS_REWARD,//有聚会 有奖励
    JOINED_NO_REWARD,//有聚会 没有奖励
}

/// <summary>
/// 游戏画质
/// </summary>
public enum ENPGameQuality
{
    VERY_LOW,// 极低
    LOW,//低
    NORMAL,//中
    HIGH,//高
    ULTRA,//极高
}

/// <summary>
/// 游戏内存分级
/// </summary>
public enum ENPGameMemoryLevel
{
    LOW,//低
    NORMAL,//中
    HIGH,//高
}

/// <summary>
/// 第三方埋点类型
/// </summary>
public enum EThirdCustomEventType
{
    NONE,
    LOGIN,//登录成功
    ROLE,//创角成功
    TUTORIAL,//引导完成
    STAGE_2,//完成阶段目标大阶段2
    STAGE_8,//完成阶段目标大阶段8
}

/** 客户端执行平台 */
public enum EWCGClientPlat
{
    NONE,
    IOS,
    ANDROID,
    PC,
}

/// <summary>
/// 物品使用类型
/// </summary>
public enum ENPBagItemUseType
{
    NONE,//直接使用
    NX,//多选一道具
    PERCENT,//概率道具
    CULTURE,//培养道具
}

//大学位置状态
public enum ECollegePosStatus
{
    NONE,
    EMPTY, //空闲
    LEARNING, //正在学习
    FINISH,//学习完成
    LOCK,//未解锁
}

/// <summary>
/// 关卡状态
/// </summary>
public enum EChapterLevelState
{
    Locked,         // 未解锁
    NotReached,     // 未达到
    CurrentLevel,   // 当前关卡
    PassedLevel,    // 已通过的关卡
}

/// <summary>
/// 进度值格式化显示方式
/// </summary>
public enum EValueFormatType
{
    NORMAL,//普通显示（会转为大数）
    PLAYER_LEVEL, //玩家等级
    PLAYER_CHAPTER, //玩家关卡
    NORMAL_NOT_LARGE_STR,//普通的非大数显示
    GOLD, //金币大数值
    GOLD_NOT_LARGE_STR, //金币非大数值
    BATTLE, //战斗大数值
    BATTLE_NOT_LARGE_STR, //战斗非大数值
    EARNING,//赚速
}

/// <summary>
/// 通用选中状态
/// </summary>
public enum ECommonSelectState
{
    SELECTED,//选中
    UNSELECTED,//未选中
}

public enum ECommonSaveState
{
    SAVED,//已保存
    UNSAVED,//未保存
}

public enum ECommonLockState
{
    LOCKED,//锁定
    UNLOCKED,//解锁
}

public enum ECommonProgressState
{
    PASSED,
    CURRENT,
    LOCKED,
}

//使用状态
public enum EItemUseStatus
{
    CAN_USE, //可使用
    CAN_NOT_USE, //不可使用
    USING,//正在使用中
}

//邮件详情预制体类型
public enum EMailDetailPrefabType
{
    NORMAL,//常规邮件
    ITEM_EXPIRED,//==  物品过期通知邮件
    HERO_GET,//==  骑士获得通知邮件
    HERO_UPGRADE_STEP,//==  骑士升阶邮件
}

//showcase效果特殊加载类型
public enum EShowcaseLoadType
{
    NONE,
    TRAVEL,//游历
    TRAVEL_BG,//游历背景
}

/// <summary>
/// 提示信息队列类型
/// </summary>
public enum ETipQueueType
{
    NONE,
    ATTR,//基础属性
    NATION_POWER,//国力
    TALENT,//资质
    MARS_POWER,//火星实力
}

public enum ESceneMoveType
{
    XZ,
    XY,
    YZ
}

/// <summary>
/// 关卡进度
/// </summary>
public enum EChapterProgressState
{
    PRE_PASSED,//已通过 且 实际所在关卡已经与本关卡不同
    CURRENT_UNDERWAY,//当前关卡正在进行中(还未完成)
    LOCKED,//未解锁
    UNREACHED,//已经解锁但未达到进度
    CURRENT_PASSED,//当前故事/章节/关卡已经通过 但是由于下一故事/章节/关卡未解锁, 导致数据仍然在本阶段
}

/// <summary>
/// 功能隶属哪个界面
/// </summary>
public enum EFuncBelongType
{
    BUILDING,//主城
    SPACE_STATION,//空间站
    ALL,//全部
}

public enum EScrollRectMoveType
{
    LEFT,
    RIGHT,
    TOP,
    BOTTOM,
}

/// <summary>
/// 字体类型
/// </summary>
public enum EFontType
{
    [InspectorName("NORMAL")]
    CONTENT,//正文内容
    [InspectorName("SPE")]
    TITLE,//标题
    [InspectorName("SPE2")]
    NAME_SIGN,//签名
}

/// <summary>
/// 侧边栏类型
/// </summary>
public enum ESideBarType
{
    WIN_MAIN_BELOW,//主界面下面收纳栏
}

/// <summary>
/// 亲密历程类型
/// </summary>
public enum EConsortExperienceType
{
    NONE,
    CONSORT_SKILL,//知己技能
    CONSORT_STORY,//剧情
    CONSORT_VOICE,//配音
}

/// <summary>
/// 妃子配音类型
/// </summary>
public enum EConsortVoiceType
{
    NONE,
    Locked,// 未获得时，点击查看情人时播放
    Unlock,// 玩家初次获得情人，在情人获得界面播放
    Entrance,// 点击到情人主页时触发，可能是一些与玩家打招呼的话
    Viewed,// randomidle：每个角色1个语气词，将在主界面点击角色后触发
    Happy, //开心语音
    Gift, // 送礼时语音
    RankUp,// 羁绊等级提升音效
}

/// <summary>
/// 大臣配音类型
/// </summary>
public enum EHeroVoiceType
{
    NONE,
    LOCK,//未获得时点击查看顾问时播放
    UNLOCK,//玩家初次获得顾问，在顾问获得界面播放
    VIEW,//已获得的顾问，进入详情界面或点击详情界面时在三条语音中随机播放
    APPOINT,//顾问委任进建筑时语音
    LEVEL_UP,//顾问等级提升时语音
    SKILL_UP,//顾问经营技能、资质技能升级时语音
    ADVANCE,//顾问觉醒时播放
    PROMOTION,//顾问晋升时播放
    FIGHT,//顾问战斗语音，出战表现时播放
    KILL_BLOW,//顾问击杀语音，战斗中击杀时播放
}

/// <summary>
/// 学徒配音类型
/// </summary>
public enum EChildVoiceType
{
    NONE,
    TEACH_STEP_1,//阶段1培养语音
    TEACH_STEP_2,//阶段2培养语音
    TEACH_STEP_3,//阶段3培养语音
}

/// <summary>
/// 提示的显示方式
/// </summary>
public enum EHintShowWay
{
    NONE,
    TIP,//tip提示
    POP_WND,//弹窗提示
}

/// <summary>
/// 主界面的按钮类型
/// </summary>
public enum EMainFunctionTabType
{
    NONE,
    BUILDING,//经营厂家
    SPACE_STATION,//空间站
    HERO,//大臣
    CHAPTER,//关卡
    BAG,//背包
    MARS, //火星
}

/// <summary>
/// BonusMgr 的类型标签
/// </summary>
public enum EUnionBonusMgrTag
{
    [InspectorName("TOTAL === 总的额外加成")]
    TOTAL,
    [InspectorName("HERO === 来自伙伴的额外加成")]
    HERO,
    [InspectorName("FARMING === 来自农田的额外加成")]
    FARMING,
    [InspectorName("HERO_SKIN === 来自伙伴皮肤的额外加成")]
    HERO_SKIN,
    [InspectorName("CONSORT === 来自妃子的额外加成")]
    CONSORT,
    [InspectorName("GUILD === 来自联盟的额外加成")]
    GUILD,
    [InspectorName("DEVELOP === 来自经营业务的额外加成")]
    DEVELOP,
    [InspectorName("PRODUCT === 来自经营产品的额外加成")]
    PRODUCT,
    [InspectorName("INN === 来自旅店的额外加成")]
    INN,
    [InspectorName("MUSEUM === 来自博物馆的额外加成")]
    MUSEUM,
    [InspectorName("TREASURE_HUNT === 来太空寻宝的额外加成")]
    TREASURE_HUNT,
    [InspectorName("PLAYER_BUFF === 来自玩家buff的额外加成")]
    PLAYER_BUFF,
    [InspectorName("PRIVILEGE_CARD === 来自玩家特权卡的额外加成")]
    PRIVILEGE_CARD,
}

/// <summary>
/// 竞技场指定谈判使用道具类型
/// </summary>
public enum EArenaSelectAttackConsumeItemType
{
    SIMPLE,//普通
    ADVANCED,//高级
    SUPER,//特级
}

public enum ECityMainJumpType
{
    MINIMUM_EMPLOYEE,//最低员工数的建筑
    UPGRADABLE,//可升级的建筑
    HERO_SETTABLE,//可设置骑士的建筑
    HERO_ANECDOTE,//骑士事件
    EMPLOYEE_LOWEST_COST,//招聘员工最低消耗
}

/// <summary>
/// 火星建筑跳转类型
/// </summary>
public enum EMarsBuildingJumpType
{
    NONE,
    MINIMUM_LEVEL,//最低等级建筑
}

public enum EBusinessBuildingType
{
    NONE,
    MINIMUM_EMPLOYEE,//最低员工数的建筑
    UPGRADABLE,//可升级的建筑
    HERO_SETTABLE,//可设置骑士的建筑
    EMPLOYEE_LOWEST_COST,//招聘员工最低消耗
}

public enum EAnecdoteType
{
    NONE,
    HERO_GAIN,//骑士获得
}

public enum EAnecdoteEventType
{
    SPECIAL,//特殊事件
    NORMAL,//普通事件
}
public enum EAnecdoteEventEndType
{
    NONE,
    TO_BE_CONTINUED,//待续
    END,//结束
}

/// <summary>
/// 礼包组显示类型
/// </summary>
public enum EGiftPackGroupShowType
{
    NONE,
    ACTIVITY,//活动
    DAILY,//每日
    WEEKLY,//每周
    RANK_RUSH,//冲榜
    GUILD,//联盟
    GEM,//钻石
    MARS_DAILY,//火星每日
    MARS_WEEKLY,//火星每周
}



public enum EMarsExploreTeamUIState
{
    Exploring,
    Back,
    Collecting,
    Idle,
    IdleSoldierLoss,
    Repairing,
    Empty,
    Lock,
    CanAskHelp,
}

/// <summary>
/// 推送礼包类型
/// </summary>
public enum EPushGiftPackType
{
    NONE,
    [InspectorName("抽卡道具礼包")]
    SUMMON_ITEM_GIFT_PACK,
    [InspectorName("伙伴升阶礼包")]
    HERO_STEP_UP_GIFT_PACK,
    [InspectorName("妃子礼包")]
    CONSORT_GIFT_PACK,
    [InspectorName("玩家等级提升礼包")]
    PLAYER_LEVEL_UP_GIFT_PACK,
    [InspectorName("建筑升级道具不足礼包")]
    BUILDING_UP_GIFT_PACK,
}

public enum ERushExchangeState
{
    [InspectorName("未解锁")]
    Lock,
    [InspectorName("可兑换")]
    Ready,
    [InspectorName("兑换中")]
    Exchange,
    [InspectorName("兑换完成可领奖")]
    Reward,
    [InspectorName("等待中")]
    Wait,
}
