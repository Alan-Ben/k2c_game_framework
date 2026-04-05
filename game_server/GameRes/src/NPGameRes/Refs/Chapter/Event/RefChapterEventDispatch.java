package NPGameRes.Refs.Chapter.Event;

import Common.ChapterEnum.EChapterEventType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "chapter_event_dispatch")
public class RefChapterEventDispatch extends _ARefChapterEvent
{
    private static RefChapterEventDispatchMgr _g_mgr = new RefChapterEventDispatchMgr();

    public static RefChapterEventDispatchMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterEventDispatchMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterEventDispatchMgr) _mgr;
    }

    @Override
    public EChapterEventType getEventType()
    {
        return EChapterEventType.DISPATCH;
    }

    @Override
    public List<NPCommonCostItem> getAKeyForwordRewardList()
    {
        return a_key_forward_reward;
    }

    public static class RefChapterEventDispatchMgr extends RefTableContainer<RefChapterEventDispatch>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterEventDispatch newRef = (RefChapterEventDispatch) _newRef;
        id = newRef.id;
        condition_id_list = newRef.condition_id_list;
        event_reward_list = newRef.event_reward_list;
        hero_num = newRef.hero_num;
        a_key_forward_reward = newRef.a_key_forward_reward;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public List<Long> condition_id_list;
    public List<Long> event_reward_list;
    public int hero_num;
    public List<NPCommonCostItem> a_key_forward_reward;
}
