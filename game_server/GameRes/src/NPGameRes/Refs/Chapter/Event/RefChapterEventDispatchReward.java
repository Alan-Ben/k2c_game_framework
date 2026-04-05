package NPGameRes.Refs.Chapter.Event;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "chapter_event_dispatch_reward")
public class RefChapterEventDispatchReward extends RefBase
{
    private static RefChapterEventDispatchRewardMgr _g_mgr = new RefChapterEventDispatchRewardMgr();

    public static RefChapterEventDispatchRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterEventDispatchRewardMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterEventDispatchRewardMgr) _mgr;
    }

    public static class RefChapterEventDispatchRewardMgr extends RefTableContainer<RefChapterEventDispatchReward>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterEventDispatchReward newRef = (RefChapterEventDispatchReward) _newRef;
        id = newRef.id;
        reward_item_list = newRef.reward_item_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public List<NPCommonCostItem> reward_item_list;
}
