using System;
namespace GOE
{
    //红点id常量
    public class RedTipConst
    {
        #region 邮件相关
        public const long RED_MAIL_ENTER = 20600;//邮件入口
        #endregion

        #region 骑士

        public const long RED_HERO_FIRST_GET = 20101;//顾问首次获得
        public const long RED_HERO_CAN_WEAR_EQUIP = 20102;//顾问可穿戴藏品红点
        public const long RED_HERO_LEVEL_UP = 20103;//顾问升级红点
        public const long RED_HERO_STEP_UP = 20104;//顾问升阶红点
        public const long RED_HERO_BUSINESS_UPGRADE = 20105;//顾问经营红点
        public const long RED_HERO_TALENT_UPGRADE = 20106;//顾问资质红点
        public const long RED_HERO_STAR_UPGRADE = 20107;//顾问深造红点
        public const long RED_HERO_HALO_UPGRADE = 20108;//顾问威望红点

        #endregion

        #region 藏品

        public const long RED_EQUIP_RECYCLE = 22401;//藏品有可回收红点

        #endregion

        #region 钻石礼包

        public const long RED_CRYSTAL_GIFT_PACK = 31100;//钻石礼包入口红点

        #endregion

        #region 妃子相关
        public const long RED_CONSORT_ENTER = 20200;//妃子入口
        public const long RED_CONSORT_HAS_INTITE_CD = 20201;//妃子入口 - 有邀约体力时红点
        public const long RED_CONSORT_GAIN = 20202;//妃子入口 - 妃子item - 获取妃子红点
        public const long RED_CONSORT_BUSINESS_SKILL = 20203;//妃子入口 - 妃子item - 经营技能红点
        public const long RED_CONSORT_BLESS_SKILL = 20204;//妃子入口 - 妃子item - 加护技能红点
        public const long RED_CONSORT_FETTER = 20205;//妃子入口 - 妃子item - 羁绊红点
        
        public const long RED_CONSORT_SKIN_ENTER = 20206;//妃子皮肤入口
        public const long RED_CONSORT_CG_REWARD = 20207;//妃子CG奖励可领取红点
        public const long RED_CONSORT_SEND_GIFT = 20208;//妃子入口 - 妃子送礼红点
        
        #endregion

        #region 背包相关
        public const long RED_ITEM_CONVERT_PAGE = 20300;//背包合成道具页签
        public const long RED_ITEM_CAN_USE_PAGE = 20301;//背包可使用页签
        public const long RED_ITEM_PAGE = 20302;//背包道具页签
        #endregion

        #region 关卡 20401~204xx

        public const long RED_CHAPTER_ENTRANCE = 10203;//关卡 - 入口红点
        public const long RED_CHAPTER_PLOT_ENTRANCE = 20401;//关卡 - 剧情入口红点

        #endregion
        
        #region 聊天
        public const long RED_CHAT = 20550;//聊天入口
        public const long RED_CHAT_CHANLE = 20551;//聊天频道列表
        public const long RED_PRIVATE_CHAT = 20552;//私聊
        public const long RED_GUILD_CHAT = 20553;//联盟聊天频道
        #endregion

        #region 好友相关
        public const long RED_FRIEND_ENTER = 20500;//好友入口        
        public const long RED_FRIEND_APPLY_ENTER = 20501;//好友申请入口
        #endregion

        #region 经营建筑

        public const long RED_BUILDING_BUILD = 10103;//经营建筑 - 建造红点

        #endregion

        #region 主线任务
        public const long RED_MAIN_QUEST_PAGE = 20701;//主线任务页签        
        public const long RED_FUNC_PREVIEW_PAGE = 20702;//功能预告页签
        #endregion

        #region 玩家信息
        public const long RED_PLAYER_INFO_ENTER = 20800;//玩家信息入口
        public const long RED_PLAYER_INFO_LEVEL_UP = 20801;//玩家升级
        public const long RED_PLAYER_INFO_DAILY_REWARD = 20802;//每日奖励
        public const long RED_PLAYER_INFO_HERO_UNLOCK = 20803;//大臣解锁
        public const long RED_PLAYER_ICON = 20804;//玩家头像
        public const long RED_PLAYER_ICON_BGK = 20805;//玩家头像框
        public const long RED_PLAYER_BUBBLE = 20806;//玩家气泡
        public const long RED_PLAYER_TITLE_PREFIX = 20810;//玩家称号前缀
        public const long RED_PLAYER_TITLE_SUFFIX = 20811;//玩家称号后缀
        public const long RED_PLAYER_TITLE_BG = 20812;//玩家称号底图
        public const long RED_PLAYER_TITLE_FIXED = 20813;//玩家普通称号
        public const long RED_PLAYER_TITLE_LIMIT = 20814;//玩家限时称号
        public const long RED_PLAYER_SKIN = 20815;//玩家皮肤
        #endregion
        

        #region 每日任务
        public const long RED_DAILY_QUEST = 23001;//每日任务页签红点
        #endregion

        
        #region 宴会相关
        public const long RED_DINNER_ENTER = 21100;//宴会入口
        public const long RED_DINNER_CREATE = 21101;//宴会创建入口
        #endregion
        
        #region 商店相关
        public const long RED_SHOP_ENTER = 21200;//商店入口
        #endregion

        #region 冲榜相关
        public const long RED_RANK_RUSH_PAGE = 21301;//冲榜页签
        #endregion

        #region 阶段奖励
        public const long RED_STEP_REWARD_PAGE = 21401;//阶段奖励页签
        #endregion

        #region 游历相关
        public const long RED_TRAVEL_ENTER = 21500;//游历入口
        public const long RED_TRAVEL_COST = 21501;//游历消耗
        #endregion

        #region 排行榜相关
        public const long RED_RANK_ENTRANCE = 21600;//排行榜入口
        #endregion

        #region 运营公告
        public const long RED_ANNOUNCEMENT = 21700;//运营公告入口
        #endregion

        #region 问卷调查
        public const long RED_QUESTIONNAIRE = 21800;//问卷调查入口
        #endregion

        #region 竞技场
        public const long RED_ARENA_STATION = 21901;//竞技场贸易站满
        public const long RED_ARENA_FIGHT_COUNT = 21902;//竞技场次数
        public const long RED_ARENA_STATION_UPGRADE = 21903;//竞技场贸易站可升级
        #endregion

        #region 阶段目标
        public const long RED_STAGE_GOAL_ENTER = 22100;//阶段目标入口
        public const long RED_STAGE_GOAL_NOW = 22101;//阶段目标当前阶段
        public const long RED_STAGE_GOAL_BIG_STEP = 22102;//阶段目标大阶段
        public const long RED_STAGE_GOAL_NOW_TASK = 22103;//阶段目标当前阶段任务可领取奖励
        public const long RED_STAGE_GOAL_PEAK = 22104;//阶段目标时代之巅可领取奖励
        #endregion

        #region 子嗣相关
        public const long RED_PLAYER_CHILD_ENGAGE_REQUEST = 21001;//子嗣 - 订婚请求
        public const long RED_PLAYER_CHILD_SEAT = 20901;//子嗣 - 座位
        #endregion

        #region 七日登录
        public const long RED_SEVEN_DAY_LOGIN = 26000;//七日登录
        #endregion
        
        #region 七日目标
        public const long RED_SEVEN_DAY_GOALS = 22000;//七日目标
        #endregion
        
        #region 赚速目标
        public const long RED_EARNING_GOAL_SELF = 25001;//赚速目标个人成就
        public const long RED_EARNING_GOAL_GLOBAL = 25003;//赚速目标全民奖励
        public const long RED_EARNING_GOAL_HONOR = 25004;//赚速目标荣誉奖励
        #endregion

        public const long RED_NATIONAL_POWER_TARGET_UNLOCK = 27010;//国力目标解锁红点
        
        #region 情人互动

        public const long RED_CONSORT_CHAT_PRESET_PAGE = 28002;//妃子互动 - 聊天预设页签
        public const long RED_CONSORT_CHAT_AI_PAGE = 28003;//妃子互动 - AI聊天页签
        public const long RED_CONSORT_CHAT_MOMENT_PAGE = 28005;//妃子互动 - 朋友圈页签

        #endregion

        #region 爬塔

        public const long RED_TOWER_RESEARCH = 29001;//爬塔研究入口

        #endregion
        
        #region 杰出者大厅

        public const long RED_GRAVE_MAIN = 30001;//杰出者大厅入口

        #endregion

        #region 礼包

        public const long RED_CASH_GIFT_PACK_ACTIVITY_TAB = 31001;//现金礼包-活动页签
        public const long RED_CASH_GIFT_PACK_PERMANENT_TAB = 31002;//现金礼包-常驻页签
        public const long RED_VIP_RECHARGE_REWARD = 31003;//充值礼包-VIP充值详情奖励
        public const long RED_GEM_GIFT_PACK_FREE = 31101;//钻石礼包-免费红点
        public const long RED_MARS_CASH_GIFT_PACK_PERMANENT_TAB = 39015;//火星现金礼包-常驻页签

        #endregion
		
        #region 旅店相关
        public const long RED_INN_CREATE_GUEST_LAZY_CD_HIGH_PERCENT = 32000; //旅店 - 创建客人的体力值高于最大值的 70%
        public const long RED_INN_UPGRADABLE_OR_BUILDABLE_STATION = 32001; //旅店 - 有可升级或可建造的设施
        public const long RED_INN_UPGRADABLE_DISH = 32003; //旅店 - 有可升级的菜品
        public const long RED_INN_UNLOCKABLE_DISH = 32004; //旅店 - 有可解锁的菜品
        public const long RED_INN_MEDAL_LEVEL_UPGRADE = 32006; //旅店 - 有可升级的奖牌
        public const long RED_INN_NORMAL_GUEST_HANDBOOK_REWARD = 32008; //旅店 - 有可领取的普通客人图鉴奖励
        public const long RED_INN_SPECIAL_GUEST_HANDBOOK_REWARD = 32009; //旅店 - 有可领取的特殊客人图鉴奖励
        public const long RED_INN_CASH_REGISTER_REWARD = 32011; //旅店 - 收银台有可领取的奖励
        #endregion

        #region 博物馆相关
        public const long RED_MUSEUM_ITEM_UPGRADE_OR_ACTIVE = 32005; //博物馆 - 有可升级或可激活的物品
        #endregion

        #region 联盟

        public const long RED_GUILD_NOT_JOIN = 33001; //联盟未加入红点

        public const long RED_GUILD_FREE_DONATE = 33011; //联盟免费捐献红点
        public const long RED_GUILD_DONATE_REWARD = 33012; //联盟捐献奖励红点
        public const long RED_GUILD_CAN_DONATE_BY_COIN = 33013; //联盟捐献可使用金币捐献红点

        public const long RED_GUILD_CAN_DISPATCH = 33021; //联盟可派遣代表红点

        public const long RED_GUILD_HAVE_ENTRUST = 33031; //联盟有可处理事务红点

        public const long RED_GUILD_FREE_BOX = 33041; //联盟有免费宝箱红点
        public const long RED_GUILD_GIFT_BOX = 33042; //联盟有礼包宝箱红点
        public const long RED_GUILD_ACTIVE_BOX = 33043; //联盟有活跃宝箱红点
        
        public const long RED_GUILD_APPLY = 33061; //有联盟申请红点

        public const long RED_GUILD_DUNGEON = 33071; //联盟副本PVE有奖励或有未击败怪物红点
        public const long RED_GUILD_DUNGEON_UNLOCK = 33072; //联盟副本PVE解锁红点

        public const long RED_GUILD_COOPERATE_FREE_CONSTRUCT = 33081; //联盟协作有免费建设次数红点
        public const long RED_GUILD_COOPERATE_REWARD = 33082; //联盟协作有奖励红点
        public const long RED_GUILD_COOPERATE_UNLOCK_NEW_AREA = 33083; //联盟协作解锁新区域红点
        public const long RED_GUILD_COOPERATE_UNLOCK = 33084; //联盟协作解锁红点

        public const long RED_GUILD_MARS_HELP = 33091; //联盟火星求助有可帮助的求助红点

        #endregion

        #region 太空寻宝相关

        public const long RED_TREASURE_HUNT_PENDING_ORE = 34002; //太空寻宝 - 有待处理矿石红点
        public const long RED_TREASURE_HUNT_LAZY_CD_FULL = 34003; //太空寻宝 - 体力已满红点
        public const long RED_TREASURE_HUNT_LAB_ENTRANCE = 34004;// 太空寻宝 - 实验室入口红点
        public const long RED_TREASURE_HUNT_TREASURE_NOT_ACTIVATE = 34005; //太空寻宝 - 有未激活奇物红点
        public const long RED_TREASURE_HUNT_OUTPUT_CAN_DRAW = 34006; //太空寻宝 - 有奇物产出可领取红点
        public const long RED_TREASURE_HUNT_SYSTEM_QUEST = 34007; //太空寻宝 - 系统任务红点
        public const long RED_TREASURE_HUNT_PLAY_ENTRANCE = 34008;// 太空寻宝 - 游玩入口红点
        public const long RED_TREASURE_HUNT_HAS_ENERGY = 34009; //太空寻宝 - 有能源道具可探索红点
        public const long RED_TREASURE_HUNT_CHG_AREA_ENTRANCE = 34010;// 太空寻宝 - 切换探索区域红点
        // 34011~34030 留给探索区域红点
        public const long RED_TREASURE_HUNT_ACHIEVE = 34031;// 太空寻宝 - 成就(打捞奖励)红点
        public const long RED_TREASURE_HUNT_SHOP = 34032;// 太空寻宝 - 商店红点
        public const long RED_TREASURE_HUNT_CATALOG = 34033;// 太空寻宝 - 图鉴红点
        public const long RED_TREASURE_HUNT_ORE_CATALOG = 34034; //太空寻宝 - 矿石图鉴红点
        public const long RED_TREASURE_HUNT_TREASURE_CATALOG = 34035; //太空寻宝 - 奇物图鉴红点
        public const long RED_TREASURE_HUNT_TREASURE_SKILL_CAN_UPGRADE = 34036; //太空寻宝 - 奇物技能可升级红点
        public const long RED_TREASURE_HUNT_COMPOSITE_CATALOG = 34037; //太空寻宝 - 组合图鉴红点

        #endregion

        #region 午间副本

        public const long RED_MIDDAY_DUNGEON_ENTRY = 35001;//妃子互动 - 朋友圈页签
        #endregion

        #region 晚间副本

        public const long RED_EVENING_DUNGEON_ENTRY = 35002;//晚间活动入口红点

        #endregion
        
        #region VIP

        public const long RED_VIP_REWARD = 36001;//VIP奖励

        #endregion

        #region 首充礼包

        public const long RED_FIRST_RECHARGE = 22200;//首充礼包入口

        #endregion

        #region 权益卡

        public const long RED_PRIVILEGE_CARD = 22300;//权益卡入口

        #endregion.

        #region 充值返利

        public const long RED_RECHARGE_REBATE = 31200;//充值返利页签

        #endregion
        
        #region 基金
        
        public const long RED_FUND = 31300;//基金页签
        
        #endregion

        #region 网页充值

        public const long RED_DAILY_WEB_RECHARGE = 40000; //网页充值每日红点

        #endregion

        #region 火星相关

        public const long RED_MARS_BUILDING_CAN_BUILD = 39001;//火星 - 建筑 - 可建造红点
        public const long RED_MARS_HOME_COLLECT = 39002;//火星 - 基地 - 资源收集
        public const long RED_MARS_BUILDING_CAN_UPGRADE = 39003;//火星 - 建筑 - 可升级建筑红点
        public const long RED_MARS_BUILDING_CAN_COLLECT_ENERGY = 39004;//火星 - 建筑 - 资源可领取红点
        public const long RED_MARS_PEOPLE_REPLENISH = 39005;//火星 - 居民补充红点
        public const long RED_MARS_PEOPLE_HELP_PENDING = 39006;//火星 - 居民求助待处理红点
        public const long RED_MARS_BUILDING_CAN_DISPATCH_PEOPLE = 39007;//火星 - 建筑 - 建筑可派遣居民红点
        public const long RED_MARS_BUILDING_UPGRADED_OR_CONSTRUCTED = 39008;//火星 - 建筑 - 建筑升级或建造完成红点
        public const long RED_MARS_TECHNOLOGY_CAN_UPGRADE = 39009;//火星 - 科研所 - 有科技可升级红点
        public const long RED_MARS_INTELLIGENT_CONTROL_CAN_USE = 39011;//火星 - AI智能控制 - 有控制可使用红点
        public const long RED_MARS_EXPLORE_LAZY_CD = 39101;//火星 - 探索 - 体力红点
        public const long RED_MARS_EXPLORE_EVENT_REWARD = 39102;//火星 - 探索 - 事件奖励可领取红点
        public const long RED_MARS_EXPLORE_PVP_LOG = 39103;//火星 - 探索 - 有新的战报内容
        public const long RED_MARS_EXPLORE_SHARED_MINE = 39104;//火星 - 探索 - 共享矿红点
        public const long RED_MARS_EXPLORE_GUILD_BATTLE_REPORT = 39105;//火星 - 探索 - 联盟战报红点
        public const long RED_MARS_GO_TO_NEXT = 39200;//火星 - 前往 - 可前往下一阶段或登录火星红点

        #endregion

        #region 限时兑换
        public const long RED_RUSH_EXCHANGE_READY = 44001; //	限时兑换-可兑换
        public const long RED_RUSH_EXCHANGE_REWARD = 44002; //	限时兑换-兑换完成
        

        #endregion
    }
}