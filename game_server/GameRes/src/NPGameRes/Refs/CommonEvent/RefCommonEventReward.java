package NPGameRes.Refs.CommonEvent;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "common_event_reward")
public class RefCommonEventReward extends RefBase
{
    private static RefTableContainer<RefCommonEventReward> _g_mgr = new RefTableContainer<RefCommonEventReward>();

    public static RefTableContainer<RefCommonEventReward> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonEventReward> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonEventReward>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonEventReward newRef = (RefCommonEventReward) _newRef;
        id = newRef.id;
        item_list = newRef.item_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//奖励id
    public List<NPCommonCostItem> item_list;//道具列表
}
