package NPGameRes.Refs.Chapter.Event;

import Common.ChapterEnum.EChapterEventType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "chapter_event_reward")
public class RefChapterEventReward extends _ARefChapterEvent
{
    private static RefChapterEventRewardMgr _g_mgr = new RefChapterEventRewardMgr();

    public static RefChapterEventRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterEventRewardMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterEventRewardMgr) _mgr;
    }

    @Override
    public EChapterEventType getEventType()
    {
        return EChapterEventType.REWARD;
    }

    @Override
    public List<NPCommonCostItem> getAKeyForwordRewardList()
    {
        return reward_item;
    }

    public static class RefChapterEventRewardMgr extends RefTableContainer<RefChapterEventReward>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterEventReward newRef = (RefChapterEventReward) _newRef;
        id = newRef.id;
        reward_item = newRef.reward_item;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//事件id
    public List<NPCommonCostItem> reward_item;
}
