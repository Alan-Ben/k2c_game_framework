package NPGameRes.Refs.Chapter;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.Chapter.Event._ARefChapterEvent;

@RefTable(tableName = "chapter_event")
public class RefChapterEvent extends RefBase
{
    private static RefChapterEventMgr _g_mgr = new RefChapterEventMgr();

    public static RefChapterEventMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterEventMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterEventMgr) _mgr;
    }

    public static class RefChapterEventMgr extends RefTableContainer<RefChapterEvent>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterEvent newRef = (RefChapterEvent) _newRef;
        id = newRef.id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//事件id

    @RefField(isIgnore = true)
    public _ARefChapterEvent detailRef;

    public void setDetailRef(_ARefChapterEvent _ref)
    {
        detailRef = _ref;
    }
}
