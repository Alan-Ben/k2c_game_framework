package NPGameRes.Refs.Battle;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;

/**
 * @author ricci
 */
@RefTable(tableName = "clock_reward")
public class RefClockReward extends RefBase
{
    private static RefListContainer<RefClockReward> _g_mgr = new RefListContainer<RefClockReward>();

    public static RefListContainer<RefClockReward> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefClockReward> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefClockReward>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefClockReward newRef = (RefClockReward) _newRef;
        id = newRef.id;
        num = newRef.num;
        refresh_clock = newRef.refresh_clock;
        cond = newRef.cond;
        item_list = newRef.item_list;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int id;
    public int num;//发放次数
    public NPRefreshTimeObj refresh_clock = new NPRefreshTimeObj();//发放时间
    public NPPlayerConditionGroupObj cond;//发放条件0
    public ArrayList<NPCommonCostItem> item_list = new ArrayList<>();//奖励物品

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
