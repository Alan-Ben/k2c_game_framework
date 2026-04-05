package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * RefTravelEventGamble - 博彩游历事件配表
 *
 * 主要功能：
 * 1. 记录博彩事件的押注范围、权重和倍率配置
 */
@RefTable(tableName = "travel_event_gamble")
public class RefTravelEventGamble extends RefBase
{
    private static RefTravelEventGambleMgr _g_mgr = new RefTravelEventGambleMgr();
    public static RefTravelEventGambleMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventGambleMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventGambleMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventGamble newRef = (RefTravelEventGamble) _newRef;
        event_id = newRef.event_id;
        min_ante = newRef.min_ante;
        max_ante = newRef.max_ante;
        win_weight = newRef.win_weight;
        lose_weight = newRef.lose_weight;
        jackpot_weight = newRef.jackpot_weight;
        win_rate = newRef.win_rate;
        lose_rate = newRef.lose_rate;
        jackpot_rate = newRef.jackpot_rate;
    }

    public static class RefTravelEventGambleMgr extends RefTableContainer<RefTravelEventGamble>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    @Override
    public long Id()
    {
        return event_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long event_id;           // 事件ID
    public int min_ante;            // 最少押注钻石
    public int max_ante;            // 最多押注钻石
    public int win_weight;          // 成功触发权重
    public int lose_weight;         // 失败触发权重
    public int jackpot_weight;      // 特别大奖触发权重
    public int win_rate;            // 胜利收益万分比（如20000=200%）
    public int lose_rate;           // 失败损失万分比（如2500=25%）
    public int jackpot_rate;        // 特别大奖收益万分比（如100000=1000%）
}
