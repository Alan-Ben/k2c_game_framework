package NPGameRes.Refs.Chapter.Event;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "chapter_event_choice_option")
public class RefChapterEventChoiceOption extends RefBase
{
    private static RefChapterEventChoiceOptionMgr _g_mgr = new RefChapterEventChoiceOptionMgr();

    public static RefChapterEventChoiceOptionMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterEventChoiceOptionMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterEventChoiceOptionMgr) _mgr;
    }

    public static class RefChapterEventChoiceOptionMgr extends RefTableContainer<RefChapterEventChoiceOption>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterEventChoiceOption newRef = (RefChapterEventChoiceOption) _newRef;
        id = newRef.id;
        reward_item_list = newRef.reward_item_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//事件id
    public List<NPCommonCostItem> reward_item_list;//事件id
}
