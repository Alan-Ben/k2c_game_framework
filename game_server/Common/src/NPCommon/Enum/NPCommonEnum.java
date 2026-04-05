package NPCommon.Enum;

//NP游戏枚举
public class NPCommonEnum
{

    /*************
     * 游戏组件
     */
    public enum ENPPlayerCompType
    {
        NONE,
        PLAYER_COMP, //玩家基础组件
        CURRENCY_COMP, //货币组件
        ICON_COMP, //头像组件
        ICON_BGK_COMP, //头像框组件
        TITLE_COMP,     //称号组件
        BUBBLE_COMP,    //气泡框组件
        ROOM_SKIN_COMP, //房间皮肤组件
        BUFF_COMP,//Buff组件
        BAG_ITEM, //背包物品组建
        CLIENT_DATA,//客户端数据
        MAIL_COMP, //邮件组件
        PLAYER_LAZY_CD_COMP, //CD组件
        QUEST_COMP, //任务组件
        DAILY_QUEST,//日常任务组件
        RECORD_COMP, //记录组件
        EVENT_RECORD_COMP,//事件计数器组件
        CLOCK_REWARD,//定点发放奖励
        FIXED_CD,//固定刷新cd
        FRIEND,//好友组件
        ACHIEVE,//成就数据组件
        SHOP,//商店数据组件
        OFFLINE_REWARD,//离线奖励组件
        CONSORT,//情人
        CHILD,//未成年子嗣
        HERO,//骑士
        CHAPTER,//章节
        SPECIAL_ITEM,//特殊物品
        WEEK_CARD,//周卡
        PLAYER_SHOW,//玩家展示数据
        DINNER,//宴会组件
        ANECDOTE,//政务组件
        DAILY_CHECK,//每日签到组件
        TRAVEL,//游历组件
        HERO_RECOMMEND,//大臣推荐组件
        MARKET,//集市组件
        EMOTE_GROUP,//聊天表情包组件
        FUNC_UNLOCK,//功能解锁组件
        CUTE_ACTOR,//Q版形象组件
        ANNOUNCEMENT,//公告数据组件
        LIKE_RECORD,//点赞记录组件
        STAGE_GOAL,//阶段任务
        GUILD,//联盟
        BUILDING,//建筑
        EQUIP,//藏品
        GACHA,//抽卡
        RECRUIT,//兑换
        ARENA,//兑换
        TARGET_REWARD,//目标奖励
        REFRESH,//通用刷新
        TOWER,//爬塔
        MIDDAY_DUNGEON,//午间副本
        EVENING_DUNGEON,//晚间副本
        COMBO_TITLE,//组合称号
        PLAYER_SKIN,//玩家皮肤
        SEVEN_LOGIN,//七日登录
        SEVEN_DAY_GOALS,//七日目标
        ACTIVITY_CURRENCY,//活动积分
        CONSORT_CHAT,//情人聊天
        COUNTDOWN_EVENT,//倒计时事件
        INN,//旅店
        MUSEUM,//博物馆
        SYSTEM_QUEST,//系统任务组件
        TREASURE_HUNT,//太空寻宝
        GUILD_DUNGEON,//公会副本
        MARS,//火星-火星各系统数据合并汇总
        MARS_GO_ROUTE,//火星-前往火星
        MARS_BUILDING,//火星-建筑
        MARS_PEOPLE,//火星-居民
        MARS_TECH,//火星-科研
        MARS_EXPLORE,//火星-探索
        MARS_MINE,//火星-矿产
        MARS_SOLDIER,//火星-士兵
        ORDER,//订单组件
        GIFT_PACK,//礼包组件
        PUSH_GIFT_PACK,//推送礼包组件
        GEM_RECHARGE,//钻石充值组件
        GUILD_COOPERATE_COMP,//联盟协作组件
        MAIL_PLAN_COMP,//邮件计划组件
        RED_DOT_COMP,//红点组件
        PRIVILEGE_CARD,//权益卡组件
        PLAYER_PERMISSIONS,//玩家权限组件
        GUILD_BOX,//联盟宝箱
        FOREVER_ADD,//永久加成
        ACTIVITY_FUND,//活动基金
        RANK_GIFT_PACK,//冲榜礼包
        RUSH_EXCHANGE,//急速兑换
        LOVER_COLLECT,//情人收集
        PLAYER_CACHE,//玩家缓存数据管理组件
        FORBID_CHAT,//禁言组件
        REPORT,//举报组件
    }

    /**************
     * 物品日志对应类型字段
     */
    public enum ELogItem_Type
    {
        NONE,
        GAIN, //获取
        CONSUME, //消耗
        SET, //设置
        SPEND, //扣除
        AUTO_RECOVER,//自动恢复
        INIT,//初始化获得
    }

    /**************
     * 客户端类型
     */
    public enum EClientType
    {
        PC,
        IOS,
        ANDROID,
    }

    /**************
     * 触发事件类型
     */
    public enum ENPContextType
    {
        NONE,
        COMMON_CONTEXT, //通用上下文
        ITEM_USE_CONTEXT, //物品使用触发
        SPACE_CONTEXT,//空间上下文
    }

    public enum ENPVariableCalType
    {
        NONE,
        ADD,        //加
        SUB,        //减
        MUL,        //乘
        DIV,        //除

        MAX,        //取二者最大值
        MIN,        //取二者最小值
    }

    public enum ENPValueFixType
    {
        ADD_VALUE,
        ADD_PERCENT,
        LIMIT_VALUE,
    }

    public enum ENPValueModifier
    {
        EXP_WEEK_LIMIT,
        SLIVER_WEEK_LIMIT,
        ACTIVE_SCORE_WEEK_LIMIT,
        GUILD_BONUS,

    }


    //卡牌角色类型
    public enum ENPActorType
    {
        NONE,
        NORMAL,//普通
        HERO,//英雄
        BUILDING,//建筑
    }


    /*语言枚举 注意该枚举需要跟PHP的枚举对应，即PHP语言枚举：
     * (1=>"English",2=>"China",3=>"Taiwan",4=>"Russia",5=>"Korea",6=>"Japan",7=>"Arabic",8=>"Turkey",9=>"German",10=>"French",11=>"Spanish",12=>"Portuguese",13=>"Italian");
     */
    public enum ENPLanguage
    {
        NONE,
        EN_US,//英语(美国)
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

    //攻击类型
    public enum ENPAttackType
    {
        NONE,
        NORMAL,//普通
        STRIKE,//穿刺
        MAGIC,//魔法
        SIEGE,//攻城
        HERO,//英雄攻击
    }

    //防御类型
    public enum ENPDefenseType
    {
        NONE,
        NO_ARMOR,//无甲
        SOFT,//轻甲  软甲
        HEAVY,//重甲
        WALL, // 城墙  城甲
        HERO,  //英雄甲
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

    public enum ENPTeamPropertyType
    {
        NONE,
        COLLECT_PER,//(金币)采集资源加成,默认为0 万分比,公式为 现采集数量 = 原采集数量 * (1 + 采集资源加成/10000)
        PRODUCE_PER,//生产加速
        OPERATION_PER,//操作加速
        RECYCLE_PER,//操作加速
        TEAMSKILL_TIME_PER,
        TEAMSKILL_COST_PER,
        BATTLE_SPEED,//战斗速度加成
        ROUGH_COLLECT_PER,//(原石)采集资源加成,默认为0 万分比,公式为 现采集数量 = 原采集数量 * (1 + 采集资源加成/10000)
    }

    //技能类型
    public enum ENPSkillType
    {
        ACTIVE,//主动技能
        PASSIVE,//被动技能
    }

    //卡牌属性面板显示的属性类型
    public enum ENPCardPanelProperty
    {
        NONE,
        HP,
    }

    //CS同步参数枚举 枚举另外定义
//  public enum ENPPlayerParam
//  {
//      NONE,
//      LEVEL,//玩家等级
//      __NO_USE_2, //无用枚举
//      VIP_LVL, //VIP等级
//      GM_LEVEL,//GM权限
//      ICON,//玩家头像
//      ICON_BGK,//玩家头像框ID
//      BANNED_CHATEXPIREDTIEM,// 禁言过期时间
//      BANNED_LOGINEXPIREDTIEM,//禁登过期时间
//      CREATE_TIME,//创角时间
//      REGCHAGED_GEM,//累计充值宝石
//      LANGUAGE,//玩家语言类型
//      LOGIN_DAY_COUNT,   //登录天数
//      LAST_LOGIN_DATE,   //最后一次登录的日期标记
//      LAST_TAKE_VERSION, //最后领取客户端版本奖励的版本。
//      LAST_LEFT_BAG_TIME,//最后一次查看背包物品的时间
//      LAST_OFFLINE_MS,//最后一次离线时间戳（毫秒）
//      LAST_MISSION,//最后一次关卡
//      LAST_MISSION_MIN_MS,//最后一次关卡通关时间（毫秒）
//      CHAT_BANNED_END_TIME,//禁言过期时间
//  }

    //攻击方式
    public enum ENPAtkTarType
    {
        GROUND,
        AIR,
    }

    public enum ENPMissionType
    {
        NORMAL,//普通
        ELITE,//精英
    }


    public enum ENPPVEState
    {
        IDLE,
        LOCAL_BATTLE,     //客户端申请进入pve，通过服务器审核后进入本地战斗阶段
        CREATEING_ROOM,   //在等待创建对应房间的状态
        IN_ROOM,          //已经创建房间的状态
        IN_BATTLE,
        DIGGING_BATTLE,//客户端申请进入矿区战斗，服务器审核后保存战斗用数据
    }

    /**
     * 关卡类型
     */
    public enum ENPPEVType
    {
        NORMAL, //普通关卡
        ELITE,
    }


    //mission中的挑战dungeon类型
    //发送pve的时候需要带上这个枚举，决定实际进入哪个dungeon
    public enum ENPMissionDungeonType
    {
        NONE,
        UNUSED_1,              //暂未使用
        SINGLE_PVE,                 //boss战
        DIGGING_PVP,                 //矿区pvp
        ARENA,//比武擂台
        ;
        public static final ENPMissionDungeonType[] ENPMissionDungeonType_Values = ENPMissionDungeonType.values();
        public static final int ENPMissionDungeonType_Length = ENPMissionDungeonType_Values.length;

        public static ENPMissionDungeonType ENPMissionDungeonType_FromInt(int _ivalue)
        {
            if (_ivalue < 0 || _ivalue >= ENPMissionDungeonType_Length)
            {
                return null;
            }
            return ENPMissionDungeonType_Values[_ivalue];
        }
    }

    public enum ENPGainSource
    {
        NONE,
        HANG_UP,              //挂机战
    }


    public enum ENPCityBuildCardState // 枚举值和排序有关，越小越前面
    {
        NONE,
        IN_STOCK,               // 显示库存数量的状态
        CAN_BUY,                // 正常可购买状态
        INSUFFICENT_COST,       // 花费不足的状态
        CONDITION_LIMIT,        // 条件限制
        QUANTITY_LIMIT,         // 数量限制
    }


    public enum ENPCityBuildUpgradeState
    {
        NONE,
        NORMAL,         // 正常状态
        LEVEL_MAX,      // 满级
        CONDITION_LIMIT,// 条件限制
    }

    //窗口事件类型
    public enum WinMsgType
    {
        NONE,
        ON_PLAYER_NAME_CHANGE,//玩家改名
        ON_PLAYER_PARAM_CHANGE,//玩家信息变化
        ON_PLAYER_STATE_CHANGE,//玩家状态变化
        ON_PLAYER_SPEAKER_CHANGE,//玩家扬声器状态发生变化
        ON_PLAYER_MICRO_CHANGE,//玩家麦克风状态发生变化

        TRIGGER_TUTORIAL,//触发战斗外引导
        SET_TUTORIAL_DONE,//设置引导完成
        COMPLETE_ALL_TUTORIAL,//完成所有引导
        RESET_ALL_TUTORIAL,//重置所有引导
        QUIT_CURRENT_TUTORIAL,//退出当前引导

        CONTROL_TUROTIAL_SETP,//控制引导步骤
        TUTORIAL_DEAL_NEXT_STEP_BUTTON,//执行到下一步
        UI_CLICK,   //UI点击处理

        ON_BAG_ITEM_ADD,//背包物品 增
        ON_BAG_ITEM_REMOVE,//背包物品 删
        ON_BAG_ITEM_UPDATE,//背包物品 改

        ON_PLAYER_RES_CHANGE,//玩家资源改变
        ON_HERO_CARD_EAT_EXPBOOK,//英雄卡牌吃经验书

        ON_SWITCH_NEW_HERO_CARD,//英雄卡牌详情切换新卡

        ON_ADD_SELECT_MOB_CARD, //新增选中怪物卡牌
        ON_REMOVE_SELECT_MOB_CARD,//移除选择的怪物卡牌
        ON_SELECT_MOB_CARD,//选中怪物卡牌
        ON_DESELECT_MOB_CARD,//取消选中当前卡牌

        BATTLE_RESYNC,      //重新同步数据

        ON_CAMERA_POS_CHANGE, // 相机位置发生变化
    }

    /*****************
     * 引导触发的类型
     **/
    public enum ENPTutorialTriggerType
    {
        NONE,
        RENAME_DONE,        //重命名结束
        MOVE_FOCUS_DONE,    //摄像头移动完成
        FAIL,//操作失败
        CLOSE_SUMMON_BOOK,//退出召唤书界面
        BATTLE_RESULT_DONE,//结算界面展示完毕
        CUSTOM_BATTLE_READY,//自定义房间引导触发
        SELECE_RACE_REWARD,//获取选择种族后的奖励
        CLOSE_LEVEL_UP,//关卡升级解锁窗口触发引导
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
        OUT_COMBAT_TUTORIAL,
        PVE_NEW_CHAPTER_LOCK_CLICK,//pve新章节锁是否点击标记
    }

    //产出参数类型
    public enum ENPTimerProductParam
    {
        MULTIPLE, //倍数
        DURATION,//耗时（毫秒数）
    }

    //掉落类型枚举
    public enum ENPRewardDropType
    {
        PRO, // 概率掉落，列表中格子随机获得
        WEI,// 权重掉落，列表中取一个
    }

    public enum ENPCityBuildingMonoType
    {
        BUILDED,            // 已建造
        BUILD               // 正要建造
    }

    public enum ENPInsteadItemType
    {
        NONE,//所有情况都可以替换
        STARUP,//英雄升星
        TOKEN_SHOP,//代币商城
        ARMY_QUALITYUP,//军队升级品质
    }

    //数据库枚举
    public enum EDBTag
    {
        NONE,
        plat,//ps主库
        main,//us主库
        us_log,//us日志库
        comm_main,//CommonServer
        account_db,//账户库
        is_db,//interface 主库
        is_log,//interface 日志库
        rcs_db,//record 主库
        crossrank_main,//cross-rank 主库
        crossrank_log,//cross-rank log库
        crossgame_main,//cross-game 主库
        crossgame_log,//cross-game log库
        hs_db,//http主库
        hs_log,//http 日志库
        ss_db,//schedule主库
        ss_log,//schedule日志库
        scc_db,//share_code主库
        pc_db,//pay主库
        crossteam_main,//cross-team 主库
        crossteam_log,//cross-team 日志库
        gamelogic_main,//game-logic 主库
        gamelogic_log,//game-logic 日志库

        //预留多开US的库
        us_1,
        us_2,
        us_3,
        us_4,
        us_5,
        us_6,
        us_7,
        us_8,
        us_9,

        //预留多开UsLog数据库
        us_log_1,
        us_log_2,
        us_log_3,
        us_log_4,
        us_log_5,
        us_log_6,
        us_log_7,
        us_log_8,
        us_log_9,
    }
}
