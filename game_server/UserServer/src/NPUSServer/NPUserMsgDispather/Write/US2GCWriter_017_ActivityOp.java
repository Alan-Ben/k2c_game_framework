package NPUSServer.NPUserMsgDispather.Write;

import Common.ActivityEnum.EActivityState;
import Common.ActivityFundObj.ActivityFund_Info;
import Common.ActivityFundObj.ActivityFund_TaskInfo;
import Common.ActivityObj.Activity_HotRefInfo;
import Common.ActivityObj.Activity_Info;
import Common.ActivityObj.Activity_RankSettleInfo;
import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import GS2GC.p017_ActivityOp.*;

import java.util.List;

public class US2GCWriter_017_ActivityOp
{
    public static GS2GC_017_001_RetDrawActivityRankReward make_001_RetDrawActivityRankReward()
    {
        return new GS2GC_017_001_RetDrawActivityRankReward();
    }

    public static GS2GC_017_002_RetActivityRankSettleInfo make_002_RetActivityRankSettleInfo(Activity_RankSettleInfo _rankSettleInfo)
    {
        if (_rankSettleInfo == null)
            return new GS2GC_017_002_RetActivityRankSettleInfo();

        return new GS2GC_017_002_RetActivityRankSettleInfo(_rankSettleInfo);
    }

    public static GS2GC_017_003_RetActivityRankBaseList make_003_RetActivityRankBaseList(List<Rank_BaseItem> _itemList)
    {
        GS2GC_017_003_RetActivityRankBaseList proto = new GS2GC_017_003_RetActivityRankBaseList();
        proto.getBaseItemlist().addAll(_itemList);
        return proto;
    }

    public static GS2GC_017_004_RetActivityRankBaseInfoByRank make_004_RetActivityRankBaseInfoByRank(Rank_BaseItem _item)
    {
        if (_item == null)
            return new GS2GC_017_004_RetActivityRankBaseInfoByRank();

        return new GS2GC_017_004_RetActivityRankBaseInfoByRank(_item);
    }

    public static GS2GC_017_005_RetActivityRankBaseInfoByKey make_005_RetActivityRankBaseInfoByKey(Rank_BaseItem _item)
    {
        if (_item == null)
            return new GS2GC_017_005_RetActivityRankBaseInfoByKey();

        return new GS2GC_017_005_RetActivityRankBaseInfoByKey(_item);
    }

    public static GS2GC_017_006_RetActivityHotRefInfo make_006_RetActivityHotRefInfo(Activity_HotRefInfo _item)
    {
        return new GS2GC_017_006_RetActivityHotRefInfo(_item);
    }

    public static GS2GC_017_007_RetActivityRankBaseSubInfoList make_007_RetActivityRankBaseSubInfoList(List<Rank_BaseSubItem> _itemList)
    {
    	GS2GC_017_007_RetActivityRankBaseSubInfoList proto = new GS2GC_017_007_RetActivityRankBaseSubInfoList();
        if(null != _itemList)
            proto.getSubList().addAll(_itemList);
        return proto;
    }

    public static GS2GC_017_011_RetDrawActivityStepReward make_011_RetDrawActivityStepReward()
    {
        return new GS2GC_017_011_RetDrawActivityStepReward();
    }

    public static GS2GC_017_013_RetAKeyDrawActivityStepReward make_013_RetAKeyDrawActivityStepReward()
    {
        return new GS2GC_017_013_RetAKeyDrawActivityStepReward();
    }

    public static GS2GC_017_014_RetRefreshActivityShop make_014_RetRefreshActivityShop()
    {
        return new GS2GC_017_014_RetRefreshActivityShop();
    }

    public static GS2GC_017_015_RetBuyActivityShopItem make_015_RetBuyActivityShopItem()
    {
        return new GS2GC_017_015_RetBuyActivityShopItem();
    }

    public static GS2GC_017_016_RetRefreshActivityCrystalGiftPack make_016_RetRefreshActivityCrystalGiftPack()
    {
        return new GS2GC_017_016_RetRefreshActivityCrystalGiftPack();
    }

    public static GS2GC_017_017_RetBuyActivityCrystalGiftPack make_017_RetBuyActivityCrystalGiftPack()
    {
        return new GS2GC_017_017_RetBuyActivityCrystalGiftPack();
    }

    public static GS2GC_017_050_OnActivityAdd make_050_OnActivityAdd(Activity_Info _activityInfo)
    {
        return new GS2GC_017_050_OnActivityAdd(_activityInfo);
    }

    public static GS2GC_017_051_OnActivityChg make_051_OnActivityChg(Activity_Info _activityInfo)
    {
        return new GS2GC_017_051_OnActivityChg(_activityInfo);
    }

    public static GS2GC_017_052_OnActivityRemove make_052_OnActivityRemove(long _instanceId)
    {
        return new GS2GC_017_052_OnActivityRemove(_instanceId);
    }

    public static GS2GC_017_056_OnActivityStateChg make_056_OnActivityStateChg(long _instanceId, EActivityState _state)
    {
        return new GS2GC_017_056_OnActivityStateChg(_instanceId, _state);
    }

    /**
     * 构造活动基金一键领取奖励响应协议
     *
     * @return 一键领取奖励响应协议
     */
    public static GS2GC_017_018_RetActivityFundDrawStepReward make_018_RetActivityFundDrawStepReward()
    {
        return new GS2GC_017_018_RetActivityFundDrawStepReward();
    }

    /**
     * 构造活动基金任务信息响应协议
     * @param _taskList         任务信息列表
     * @param taskRefreshTimeMs
     * @return 任务信息响应协议
     */
    public static GS2GC_017_019_RetActivityFundTaskInfo make_019_RetActivityFundTaskInfo(List<ActivityFund_TaskInfo> _taskList, long taskRefreshTimeMs)
    {
        GS2GC_017_019_RetActivityFundTaskInfo proto = new GS2GC_017_019_RetActivityFundTaskInfo();
        if (_taskList != null)
        {
            proto.getTaskList().addAll(_taskList);
        }
        proto.setTaskNextRefreshTime(taskRefreshTimeMs);
        return proto;
    }

    /**
     * 构造活动基金分数变化推送协议
     *
     * @param _fundId 基金ID
     * @param _formulaScore 公式分数
     * @param _taskScore 任务分数
     * @return 分数变化推送协议
     */
    public static GS2GC_017_064_OnActivityFundScoreChg make_064_OnActivityFundScoreChg(long _fundId, long _formulaScore, long _taskScore)
    {
        GS2GC_017_064_OnActivityFundScoreChg proto = new GS2GC_017_064_OnActivityFundScoreChg();
        proto.setFundId(_fundId);
        proto.setFormulaScore(_formulaScore);
        proto.setTaskScore(_taskScore);
        return proto;
    }

    /**
     * 构造活动基金领取奖励后推送协议
     *
     * @param _fundId 基金ID
     * @param _hadDrawFreeStep 已领取免费档最大阶段
     * @param _hadDrawPayStep 已领取付费档最大阶段
     * @return 领取奖励后推送协议
     */
    public static GS2GC_017_066_OnActivityFundDrawRewardChg make_066_OnActivityFundDrawRewardChg(long _fundId, int _hadDrawFreeStep, int _hadDrawPayStep)
    {
        GS2GC_017_066_OnActivityFundDrawRewardChg proto = new GS2GC_017_066_OnActivityFundDrawRewardChg();
        proto.setFundId(_fundId);
        proto.setHadDrawFreeStep(_hadDrawFreeStep);
        proto.setHadDrawPayStep(_hadDrawPayStep);
        return proto;
    }

    /**
     * 构造新增活动基金推送协议
     *
     * @param _fundInfo 基金信息
     * @return 新增活动基金推送协议
     */
    public static GS2GC_017_067_OnActivityFundAdd make_067_OnActivityFundAdd(ActivityFund_Info _fundInfo)
    {
        GS2GC_017_067_OnActivityFundAdd proto = new GS2GC_017_067_OnActivityFundAdd();
        proto.setFundInfo(_fundInfo);
        return proto;
    }

    /**
     * 构造移除活动基金推送协议
     *
     * @param _fundId 基金ID
     * @return 移除活动基金推送协议
     */
    public static GS2GC_017_068_OnActivityFundRemove make_068_OnActivityFundRemove(long _fundId)
    {
        GS2GC_017_068_OnActivityFundRemove proto = new GS2GC_017_068_OnActivityFundRemove();
        proto.setFundId(_fundId);
        return proto;
    }
}
