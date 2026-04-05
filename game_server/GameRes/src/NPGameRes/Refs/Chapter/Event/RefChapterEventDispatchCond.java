package NPGameRes.Refs.Chapter.Event;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.HeroCondition.HeroConditionGroupObj;

@RefTable(tableName = "chapter_event_dispatch_cond")
public class RefChapterEventDispatchCond extends RefBase
{
    private static RefChapterEventDispatchCondMgr _g_mgr = new RefChapterEventDispatchCondMgr();

    public static RefChapterEventDispatchCondMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterEventDispatchCondMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterEventDispatchCondMgr) _mgr;
    }

    public static class RefChapterEventDispatchCondMgr extends RefTableContainer<RefChapterEventDispatchCond>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterEventDispatchCond newRef = (RefChapterEventDispatchCond) _newRef;
        id = newRef.id;
        condition = newRef.condition;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//事件id
    public HeroConditionGroupObj condition;//条件
}
