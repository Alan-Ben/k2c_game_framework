using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 命名空间内的常量定义类
    /// </summary>
    public class NPConst
    {
        public const string GAME_ID = "23";

        public const long C_FirstMissionId = 1001;
        //星阶段上限
        public const int C_PET_STAR_SETP_LIMIT = 5;

        /// <summary>长按提示延迟展示的时间</summary>
        public const float TOOL_TIP_DELAY_TIME = 0.1f;
        #region 上浮提示
        //TODO:默认id
        public const int C_DEFAULT_TEXT_TIP_REF_ID = 1;
        public const int C_DEFAULT_TEXT_NUM_TIP_REF_ID = 2;
        public const int C_DEFAULT_ICON_TEXT_TIP_REF_ID = 3;
        public const int C_DEFAULT_ICON_TEXT_TEXT_TIP_REF_ID = 5;
        public const int C_DEFAULT_ITEM_TEXT_TEXT_TIP_REF_ID = 10;
        //任务完成侧边tip
        public const int C_QUEST_TEXT_TIP_REF_ID = 11;
        //成为好友侧边tip
        public const int C_FRIENDS_TEXT_TIP_REF_ID = 12;

        //QTE烹饪完成tip
        public const int C_COOKING_QTE_TIP_REF_ID = 25;
        
        
        public const int C_RESISLAND_BATTLE_PASS_ICON_TEXT_TIP_REF_ID = 30; //资源岛挑战完成提示
        //每日任务完成侧边tip
        public const int C_DAILY_QUEST_TEXT_TIP_REF_ID = 12;
        //每周任务完成侧边tip
        public const int C_WEEK_QUEST_TEXT_TIP_REF_ID = 33;
        //约会成功tip
        public const int C_CONSORT_CALL_SUCC_TIP_REF_ID = 40;

        #region  博物馆 tip

        //博物馆注能暴击tipId，基础id，暴击n倍，id = C_DEFAULT_MUSEUM_CRIT_TIP_REF_ID + n
        public const int C_DEFAULT_MUSEUM_CRIT_TIP_REF_ID = 100;

        #endregion
        
        // 征收银币获得tip  id
        public const int C_LEVY_SILVER_TIP_ID = 50;
        //征收士兵
        public const int C_LEVY_SOILDER_TIP_ID = 51;
        //征收士兵暴击
        public const int C_LEVY_SOILDER_OVER_TIP_ID = 52;
        //征收粮食
        public const int C_LEVY_FOOD_TIP_ID = 53;
        //宴会-其他玩家参加宴会提示tip
        public const int C_DINNER_JOIN_MY_DINNER_TIP_ID = 60;
        #endregion
        //子嗣属性变化tip
        public const int C_CHILD_ATTR_CHG_TIP_ID = 54;

        //征收离线收益上浮提示
        public const int C_LEVY_FOOD_OFFLINE_TIP_REF_ID = 55;

        //static
        public static int G_iBasicAttrTypeCount = ALCommon.getEnumCount(typeof(EBasicAttrType));

        /// <summary>
        /// 基础属性数量
        /// </summary>
        public static int BASIC_ATTR_NUM = 4;
        /// <summary>
        /// 获得水元素宠物展示UI的id
        /// </summary>
        public static int GET_PET_WATER_UI_RES_ID = 1029;
        /// <summary>
        /// 获得火元素宠物展示UI的id
        /// </summary>
        public static int GET_PET_FIRE_UI_RES_ID = 1030;
        /// <summary>
        /// 获得草元素宠物展示UI的id
        /// </summary>
        public static int GET_PET_GRASS_UI_RES_ID = 1031;

        /// <summary>
        /// 宠物资质评分最大值
        /// </summary>
        public static long MAX_PET_ATTR_SCORE = 100;

        /// <summary>
        /// 获得宠物低资质notice标记
        /// </summary>
        public static string GET_PET_LOW_ATTR_NOTICE_TAG = "GET_PET_LOW_ATTR_NOTICE_TAG";

        /// <summary>
        /// 获得炫耀性物品notice标记
        /// </summary>
        public static string GET_SHOWOFF_NOTICE_TAG = "GET_SHOWOFF_NOTICE_TAG";

        /// <summary>
        /// 获得炫耀性物品总弹窗notice标记
        /// </summary>
        public static string GET_SHOWOFF_SUMMARY_NOTICE_TAG = "GET_SHOWOFF_SUMMARY_NOTICE_TAG";

        /// <summary>
        /// 宠物家族代表变更提示
        /// </summary>
        public static long PET_FAMILY_UPDATE_TIP = 26;
        /// <summary>
        /// 礼包无限制的购买次数
        /// </summary>
        public static int GIFT_PACK_NOT_LIMIT_COUNT = 999;
        
        /// <summary>
        /// actor战斗外表现类型对应动画索引，先代码写死，后续扩展可以加一张表，跟音效一起配置
        /// </summary>
        [NotNull]public static Dictionary<ENPActorEffectType, int> actorOutBattleEffectAni =
            new Dictionary<ENPActorEffectType, int>()
            {
                {ENPActorEffectType.EFFECT1, 0},
                {ENPActorEffectType.EFFECT2, 1},
                {ENPActorEffectType.EFFECT3, 2},
            };
    }

    /// <summary>
    /// 设置是否在引导中的标识
    /// </summary>
    public class IsInTutorialConst
    {
        //值只能设置1~64
        public const int FUNC_UNLOCK = 1;
        public const int CONSORT_CALL = 2;//妃子召见表现
        public const int PLAYER_UPGRADE = 3;//玩家升级表现
    }

    /// <summary>
    /// 通用协议错误码
    /// </summary>
    public class ErrorCodeConst
    {
        public const int COMMON_ERROR_CODE = 10023; //通用协议处理错误 
        
        public const int MARS_MINE_OTHER_PLAYER_OCCUPY = 560103; // 火星探索-火星矿被其他玩家占领
        public const int MARS_MINE_OTHER_PLAYER_FORWARD = 560105; // 火星探索-火星矿有其他玩家前往
        public const int MARS_MINE_SHARE_EXPIRED = 560094; // 火星探索-未找到火星矿
        public const int MARS_MINE_NOT_FOUND = 560094; // 火星探索-未找到火星矿
    }
}