package NPGameRes.Refs.Chapter.Event;

import Common.ChapterEnum.EChapterEventType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "chapter_event_choice")
public class RefChapterEventChoice extends _ARefChapterEvent
{
    private static RefChapterEventChoiceMgr _g_mgr = new RefChapterEventChoiceMgr();

    public static RefChapterEventChoiceMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterEventChoiceMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterEventChoiceMgr) _mgr;
    }

    @Override
    public EChapterEventType getEventType()
    {
        return EChapterEventType.CHOICE;
    }

    @Override
    public List<NPCommonCostItem> getAKeyForwordRewardList()
    {
        return a_key_forward_reward;
    }

    public static class RefChapterEventChoiceMgr extends RefTableContainer<RefChapterEventChoice>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterEventChoice newRef = (RefChapterEventChoice) _newRef;
        id = newRef.id;
        option_id_list = newRef.option_id_list;
        a_key_forward_reward = newRef.a_key_forward_reward;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public List<Long> option_id_list;
    public List<NPCommonCostItem> a_key_forward_reward;
}
