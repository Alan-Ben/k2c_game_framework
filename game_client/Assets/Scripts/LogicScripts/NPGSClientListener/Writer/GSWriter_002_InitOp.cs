using System.Collections.Generic;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;

namespace GOE
{
    public static class GSWriter_002_InitOp
    {
        //请求玩家信息
        public static GC2GS_002_001_ReqPlayerInfo make_001_ReqPlayerInfo()
        {
            return new GC2GS_002_001_ReqPlayerInfo();
        }

        //请求玩家身上的货币资源信息
        public static GC2GS_002_003_ReqCurrencyList make_003_ReqCurrencyList()
        {
            return new GC2GS_002_003_ReqCurrencyList();
        }

        /// <summary>
        /// 请求藏品初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_004_ReqEquipInit make_004_ReqEquipInit()
        {
            return new GC2GS_002_004_ReqEquipInit();
        }

        /// <summary>
        /// 请求玩家皮肤初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_005_ReqPlayerSkinInit make_005_ReqPlayerSkinInit()
        {
            return new GC2GS_002_005_ReqPlayerSkinInit();
        }

        //请求获取背包物品列表
        public static GC2GS_002_007_ReqBagItemList make_007_ReqBagItemList()
        {
            return new GC2GS_002_007_ReqBagItemList();
        }
        //请求游历初始化
        public static GC2GS_002_006_ReqTravelInit make_006_ReqTravelInit()
        {
            return new GC2GS_002_006_ReqTravelInit();
        }

        //请求玩家buff列表
        public static GC2GS_002_008_ReqPlayerBuffList make_008_ReqPlayerBuffList()
        {
            return new GC2GS_002_008_ReqPlayerBuffList();
        }

        /// <summary>
        /// 请求初始化政务信息
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_009_ReqAnecdoteInit make_009_ReqAnecdoteInit()
        {
            return new GC2GS_002_009_ReqAnecdoteInit();
        }
        
        public static GC2GS_002_010_ReqBuildingInit make_010_ReqBuildingInit()
        {
            return new GC2GS_002_010_ReqBuildingInit();
        }
        
        public static GC2GS_002_011_ReqGoldInit make_011_ReqGoldInit()
        {
            return new GC2GS_002_011_ReqGoldInit();
        }
        
        /// <summary>
        /// 初始化骑士
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_012_ReqHeroInit make_002_012_ReqHeroInit()
        {
            return new GC2GS_002_012_ReqHeroInit();
        }

        /// <summary>
        /// 初始化子嗣列表
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_013_ReqChildList make_013_ReqChildList()
        {
            return new GC2GS_002_013_ReqChildList();
        }

        public static GC2GS_002_014_ReqMailStatInfo make_014_ReqMailStatInfo()
        {
            return new GC2GS_002_014_ReqMailStatInfo();
        }
        /// <summary>
        /// 情人初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_015_ReqConsortList make_015_ReqConsortList()
        {
            return new GC2GS_002_015_ReqConsortList();
        }
        
        /// <summary>
        /// 征收初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_016_ReqLevyInit make_016_ReqLevyInit()
        {
            return new GC2GS_002_016_ReqLevyInit();
        }

        /// <summary>
        /// 初始化大学列表
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_017_ReqCollegeSeatList make_017_ReqCollegeSeatList()
        {
            return new GC2GS_002_017_ReqCollegeSeatList();
        }

        /// <summary>
        /// 请求活动数据列表(初始化数据)
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_018_ReqActivityList make_002_018_ReqActivityList()
        {
            return new GC2GS_002_018_ReqActivityList();
        }
        
        /// <summary>
        /// 请求关卡初始化
        /// </summary>
        public static GC2GS_002_019_ReqChapterInit make_019_ReqChapterInit()
        {
            return new GC2GS_002_019_ReqChapterInit();
        }
        
        //请求获取玩家称号列表
        public static GC2GS_002_020_ReqPlayerTitle make_020_ReqPlayerTitleList()
        {
            return new GC2GS_002_020_ReqPlayerTitle();
        }
        //请求获取玩家头像列表
        public static GC2GS_002_021_ReqPlayerIcon make_021_ReqPlayerIconList()
        {
            return new GC2GS_002_021_ReqPlayerIcon();
        }
        //请求获取玩家头像框列表
        public static GC2GS_002_022_ReqPlayerIconBgk make_022_ReqPlayerIconBgkList()
        {
            return new GC2GS_002_022_ReqPlayerIconBgk();
        }
        //请求获取玩家气泡框列表
        public static GC2GS_002_023_ReqPlayerBubble make_023_ReqPlayerBubbleList()
        {
            return new GC2GS_002_023_ReqPlayerBubble();
        }

        /// <summary>
        /// 请求初始化骑士推荐
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_024_ReqHeroRecommendInit make_024_ReqHeroRecommendInit()
        {
            return new GC2GS_002_024_ReqHeroRecommendInit();
        }

        /// <summary>
        /// 请求竞技场初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_025_ReqArenaInit make_025_ReqArenaInit()
        {
            return new GC2GS_002_025_ReqArenaInit();
        }
       
        //请求签到信息
        public static GC2GS_002_026_ReqPlayerDailyCheckInfo make_002_026_ReqPlayerDailyCheckInfo()
        {
            return new GC2GS_002_026_ReqPlayerDailyCheckInfo();
        }

        /// <summary>
        /// 请求权益卡初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_027_ReqPrivilegeCardInit make_002_027_ReqPrivilegeCardInit()
        {
            return new GC2GS_002_027_ReqPrivilegeCardInit();
        }

        //表情包初始化
        public static GC2GS_002_028_ReqChatEmoteGroupInit make_028_ReqChatEmoteGroupInit()
        {
            return new GC2GS_002_028_ReqChatEmoteGroupInit();
        }

        // 请求Q版形象初始化数据
        public static GC2GS_002_030_ReqCuteActorInit make_002_030_ReqCuteActorInit()
        {
            return new GC2GS_002_030_ReqCuteActorInit();
        }

        /// <summary>
        /// 请求贸易站初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_031_ReqPlayerStationInit make_002_031_ReqPlayerStationInit()
        {
            return new GC2GS_002_031_ReqPlayerStationInit();
        }
        
        // 请求CD数据初始化
        public static GC2GS_002_032_ReqPlayerLazyCDList make_002_032_ReqPlayerLazyCDList()
        {
            return new GC2GS_002_032_ReqPlayerLazyCDList();
        }

        //请求跑马灯初始化
        public static GC2GS_002_033_ReqMarqueeInit make_002_033_ReqMarqueeInit(List<Common.Common_MarqueeShowPosReadInfo> _marqueeReadList)
        {
            return new GC2GS_002_033_ReqMarqueeInit(_marqueeReadList);
        }

        //请求任务数据
        public static GC2GS_002_036_ReqQuestInit make_002_036_ReqQuestInit()
        {
            return new GC2GS_002_036_ReqQuestInit();

        }
        /// <summary>
        /// 周卡初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_037_ReqWeekCardInit make_037_ReqWeekCardInit()
        {
            return new GC2GS_002_037_ReqWeekCardInit();
        }

        // 请求玩家计数初始化
        public static GC2GS_002_038_ReqRecordInit make_002_038_ReqRecordInit()
        {
            return new GC2GS_002_038_ReqRecordInit();
        }

        // 请求任务计数初始化
        public static GC2GS_002_039_ReqQuestCountInit make_002_039_ReqQuestCountInit()
        {
            return new GC2GS_002_039_ReqQuestCountInit();
        }
  
        /// <summary>
        /// 请求招募初始化数据
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_041_ReqRecruitInfo make_002_041_ReqRecruitInfo()
        {
            return new GC2GS_002_041_ReqRecruitInfo();
        }
        
        //请求初始化问卷调查
        public static GS2GC_002_042_RetQuestionnaireInit make_002_042_RetQuestionnaireInit()
        {
            return new GS2GC_002_042_RetQuestionnaireInit();
        }

        //请求玩家行为计数初始化
        public static GC2GS_002_043_ReqEventRecordInit make_002_043_ReqEventRecordInit()
        {
            return new GC2GS_002_043_ReqEventRecordInit();
        }

        //请求日常 周常任务
        public static GC2GS_002_046_ReqDailyQuest make_002_046_ReqDailyQuest()
        {
            return new GC2GS_002_046_ReqDailyQuest();
        }

        /// <summary>
        /// 请求抽卡初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_047_ReqGachaInit make_002_047_ReqGachaInit()
        {
            return new GC2GS_002_047_ReqGachaInit();
        }
        
        //请求固定时间恢复CD初始化
        public static GC2GS_002_048_ReqFixedCdInfo make_002_048_ReqFixedCdInfo()
        {
            return new GC2GS_002_048_ReqFixedCdInfo();
        }

        //请求好友列表
        public static GC2GS_002_049_ReqFriendInit make_002_049_ReqFriendInit()
        {
            return new GC2GS_002_049_ReqFriendInit();
        }

        //请求成就信息
        public static GC2GS_002_050_ReqAchieveInit make_002_050_ReqAchieveInit()
        {
            return new GC2GS_002_050_ReqAchieveInit();
        }

        //请求商店信息
        public static GC2GS_002_051_ReqShopInit make_002_051_ReqShopInit()
        {
            return new GC2GS_002_051_ReqShopInit();
        }
        
        /// <summary>
        /// 请求晚间副本数据初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_052_ReqEveningDungeonInit make_002_052_ReqEveningDungeonInit()
        {
            return new GC2GS_002_052_ReqEveningDungeonInit();
        }
        
        //请求理想奖励数据初始化
        public static GC2GS_002_054_ReqOfflineRewardInit make_002_054_ReqOfflineRewardInit()
        {
            return new GC2GS_002_054_ReqOfflineRewardInit();
        }

        /// <summary>
        /// 倒计时事件初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_056_ReqCountdownEventInit make_002_056_ReqCountdownEventInit()
        {
            return new GC2GS_002_056_ReqCountdownEventInit();
        }

        /// <summary>
        /// 玩家权限初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_059_ReqPlayerPermissionsInit make_002_059_ReqPlayerPermissionsInit()
        {
            return new GC2GS_002_059_ReqPlayerPermissionsInit();
        }

        public static GC2GS_002_060_ReqDinnerInit make_002_060_ReqDinnerInit()
        {
            return new GC2GS_002_060_ReqDinnerInit();
        }

        //请求屏蔽数据组件初始化
        public static GC2GS_002_061_ReqShieldInit make_002_061_ReqShieldInit()
        {
            return new GC2GS_002_061_ReqShieldInit();
        }

        //请求阶段目标组件初始化
        public static GC2GS_002_062_ReqStageGoalInit make_002_062_ReqStageGoalInit()
        {
            return new GC2GS_002_062_ReqStageGoalInit();
        }
        
        /// <summary>
        /// 请求联盟数据初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_063_ReqGuildInit make_063_ReqGuildInit()
        {
            return new GC2GS_002_063_ReqGuildInit();
        }

        /// <summary>
        /// 请求活动货币列表
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_064_ReqActivityCurrencyList make_064_ReqActivityCurrencyList()
        {
            return new GC2GS_002_064_ReqActivityCurrencyList();
        }

        /// <summary>
        /// 请求系统任务初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_065_ReqSystemQuestInit make_065_ReqSystemQuestInit()
        {
            return new GC2GS_002_065_ReqSystemQuestInit();
        }
        
        //请求初始化
        public static GC2GS_002_034_ReqTargetRewardInit make_002_034_ReqTargetRewardInit()
        {
            return new GC2GS_002_034_ReqTargetRewardInit();
        }
        
        //请求爬塔初始化
        public static GC2GS_002_044_ReqTowerInit make_002_044_ReqTowerInit()
        {
            return new GC2GS_002_044_ReqTowerInit();
        }
        
        //请求午间副本初始化
        public static GC2GS_002_045_ReqMiddayDungeonInit make_002_045_ReqMiddayDungeonInit()
        {
            return new GC2GS_002_045_ReqMiddayDungeonInit();
        }
        
        public static GC2GS_002_053_ReqLoginCountInit make_002_053_ReqLoginCountInit()
        {
            return new GC2GS_002_053_ReqLoginCountInit();
        }
        
        public static GC2GS_002_055_ReqConsortChatInit make_002_055_ReqConsortChatInit()
        {
            return new GC2GS_002_055_ReqConsortChatInit();
        }

        public static GC2GS_002_066_ReqTreasureHuntInit make_002_066_ReqTreasureHuntInit()
        {
            return new GC2GS_002_066_ReqTreasureHuntInit();
        }

        /// <summary>
        /// 初始化礼包信息
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_070_ReqGiftPackInit make_002_070_ReqGiftPackInit()
        {
            return new GC2GS_002_070_ReqGiftPackInit();
        }

        /// <summary>
        /// 初始化前往火星数据
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_072_ReqMarsGoRouteInit make_002_072_ReqMarsGoRouteInit()
        {
            return new GC2GS_002_072_ReqMarsGoRouteInit();
        }
        
        /// <summary>
        /// 火星-火星居民数据初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_074_ReqMarsPeopleInit make_002_074_ReqMarsPeopleInit()
        {
            return new GC2GS_002_074_ReqMarsPeopleInit();
        }

        /// <summary>
        /// 联盟协作初始化请求
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_075_ReqGuildCooperateInit make_002_075_ReqGuildCooperateInit()
        {
            return new GC2GS_002_075_ReqGuildCooperateInit();
        }

        /// <summary>
        /// 请求红点初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_080_ReqRedDotInit make_002_080_ReqRedDotInit()
        {
            return new GC2GS_002_080_ReqRedDotInit();
        }

        /// <summary>
        /// 请求推送礼包列表初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_081_ReqPushGiftPackList make_002_081_ReqPushGiftPackList()
        {
            return new GC2GS_002_081_ReqPushGiftPackList();
        }

        /// <summary>
        /// 情人收集初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_088_ReqLoverCollectInit make_002_088_ReqLoverCollectInit()
        {
            return new GC2GS_002_088_ReqLoverCollectInit();
        }

        /// <summary>
        /// 请求禁言数据初始化
        /// </summary>
        /// <returns></returns>
        public static GC2GS_002_089_ReqForbidChatInit make_002_089_ReqForbidChatInit()
        {
            return new GC2GS_002_089_ReqForbidChatInit();
        }
    }
}
