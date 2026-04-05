package NPUSServer.NPUserMsgDispather.Write;

import Common.InnObj.*;
import GS2GC.p034_InnOp.*;

/**
 * p034 旅店操作协议 writer
 */
public class US2GCWriter_034_InnOp
{
    // ============= 返回协议 (Ret) =============

    /**
     * 创建旅店接待客人返回消息
     * @return 返回消息
     */
    public static GS2GC_034_001_RetInnReceiveGuest make_001_RetInnReceiveGuest()
    {
        return new GS2GC_034_001_RetInnReceiveGuest();
    }

    /**
     * 创建旅店设施解锁返回消息
     * @return 返回消息
     */
    public static GS2GC_034_002_RetInnStationUnlock make_002_RetInnStationUnlock()
    {
        return new GS2GC_034_002_RetInnStationUnlock();
    }

    /**
     * 创建旅店设施升级返回消息
     * @return 返回消息
     */
    public static GS2GC_034_003_RetInnStationUpgrade make_003_RetInnStationUpgrade()
    {
        return new GS2GC_034_003_RetInnStationUpgrade();
    }

    /**
     * 创建旅店菜品解锁返回消息
     * @return 返回消息
     */
    public static GS2GC_034_004_RetInnDishUnlock make_004_RetInnDishUnlock()
    {
        return new GS2GC_034_004_RetInnDishUnlock();
    }

    /**
     * 创建旅店菜品升级返回消息
     * @return 返回消息
     */
    public static GS2GC_034_005_RetInnDishUpgrade make_005_RetInnDishUpgrade()
    {
        return new GS2GC_034_005_RetInnDishUpgrade();
    }

    /**
     * 创建旅店结算返回消息
     * @param settleInfo 结算信息
     * @return 返回消息
     */
    public static GS2GC_034_006_RetInnSettle make_006_RetInnSettle(Inn_SettleInfo settleInfo)
    {
        return new GS2GC_034_006_RetInnSettle(settleInfo);
    }

    /**
     * 创建旅店解锁客人返回消息
     * @return 返回消息
     */
    public static GS2GC_034_008_RetInnUnlockGuest make_008_RetInnUnlockGuest()
    {
        return new GS2GC_034_008_RetInnUnlockGuest();
    }

    /**
     * 创建旅店招待特殊客人返回消息
     * @return 返回消息
     */
    public static GS2GC_034_009_RetInnServeSpecialGuest make_009_RetInnServeSpecialGuest()
    {
        return new GS2GC_034_009_RetInnServeSpecialGuest();
    }

    /**
     * 创建领取客人手册奖励返回消息
     * @return 返回消息
     */
    public static GS2GC_034_010_RetDrawGuestHandbookReward make_010_RetDrawGuestHandbookReward()
    {
        return new GS2GC_034_010_RetDrawGuestHandbookReward();
    }

    /**
     * 创建领取特殊客人手册奖励返回消息
     * @return 返回消息
     */
    public static GS2GC_034_011_RetDrawSpecialGuestHandbookReward make_011_RetDrawSpecialGuestHandbookReward()
    {
        return new GS2GC_034_011_RetDrawSpecialGuestHandbookReward();
    }

    /**
     * 创建旅店奖牌升级返回消息
     * @return 返回消息
     */
    public static GS2GC_034_012_RetInnMedalUpgrade make_012_RetInnMedalUpgrade()
    {
        return new GS2GC_034_012_RetInnMedalUpgrade();
    }

    // ============= 推送协议 (On) =============

    /**
     * 创建旅店新增接待推送消息
     * @param _receiveList 接待列表
     * @return 推送消息
     */
    public static GS2GC_034_050_OnInnReceiveChg make_050_OnInnReceiveChg(Inn_ReceiveList _receiveList)
    {
        return new GS2GC_034_050_OnInnReceiveChg(_receiveList);
    }

    /**
     * 创建旅店菜品信息变更推送消息
     * @param dishInfo 菜品信息
     * @return 推送消息
     */
    public static GS2GC_034_052_OnInnDishChg make_052_OnInnDishChg(Inn_DishInfo dishInfo)
    {
        return new GS2GC_034_052_OnInnDishChg(dishInfo);
    }

    /**
     * 创建旅店设施信息变更推送消息
     * @param stationInfo 设施信息
     * @return 推送消息
     */
    public static GS2GC_034_053_OnInnStationChg make_053_OnInnStationChg(Inn_StationInfo stationInfo)
    {
        return new GS2GC_034_053_OnInnStationChg(stationInfo);
    }

    /**
     * 创建旅店菜品信息新增推送消息
     * @param dishInfo 菜品信息
     * @return 推送消息
     */
    public static GS2GC_034_055_OnInnDishAdd make_055_OnInnDishAdd(Inn_DishInfo dishInfo)
    {
        return new GS2GC_034_055_OnInnDishAdd(dishInfo);
    }

    /**
     * 创建旅店设施信息新增推送消息
     * @param stationInfo 设施信息
     * @return 推送消息
     */
    public static GS2GC_034_056_OnInnStationAdd make_056_OnInnStationAdd(Inn_StationInfo stationInfo)
    {
        return new GS2GC_034_056_OnInnStationAdd(stationInfo);
    }

    /**
     * 创建旅店等级变更推送消息
     * @param newLevel 新等级
     * @return 推送消息
     */
    public static GS2GC_034_057_OnInnLevelChg make_057_OnInnLevelChg(int newLevel)
    {
        return new GS2GC_034_057_OnInnLevelChg(newLevel);
    }

    /**
     * 创建旅店人气值变更推送消息
     * @param newPopularity 新人气值
     * @return 推送消息
     */
    public static GS2GC_034_058_OnInnPopularityChg make_058_OnInnPopularityChg(long newPopularity)
    {
        return new GS2GC_034_058_OnInnPopularityChg(newPopularity);
    }

    /**
     * 创建旅店奖牌等级变更推送消息
     * @param medalLevel 新奖牌等级
     * @return 推送消息
     */
    public static GS2GC_034_059_OnInnMedalLevelChg make_059_OnInnMedalLevelChg(int medalLevel)
    {
        return new GS2GC_034_059_OnInnMedalLevelChg(medalLevel);
    }

    /**
     * 创建旅店客人变更推送消息
     * @param guestInfo 客人信息
     * @return 推送消息
     */
    public static GS2GC_034_060_OnInnGuestChg make_060_OnInnGuestChg(Inn_GuestInfo guestInfo)
    {
        return new GS2GC_034_060_OnInnGuestChg(guestInfo);
    }

    /**
     * 创建旅店特殊客人变更推送消息
     * @param specialGuestInfo 特殊客人信息
     * @return 推送消息
     */
    public static GS2GC_034_061_OnInnSpecialGuestChg make_061_OnInnSpecialGuestChg(Inn_SpecialGuestInfo specialGuestInfo)
    {
        return new GS2GC_034_061_OnInnSpecialGuestChg(specialGuestInfo);
    }

    public static GS2GC_034_063_OnInnFirstTimeUpgradeTimeMsChg make_063_OnInnFirstTimeUpgradeTimeMsChg(long _timeMs)
    {
        return new GS2GC_034_063_OnInnFirstTimeUpgradeTimeMsChg(_timeMs);
    }
}