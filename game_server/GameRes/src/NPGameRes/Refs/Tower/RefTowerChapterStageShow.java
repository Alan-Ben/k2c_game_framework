package NPGameRes.Refs.Tower;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "tower_chapter_stage_show")
public class RefTowerChapterStageShow extends RefBase
{
    private static RefTowerChapterMgr _g_mgr = new RefTowerChapterMgr();

    public static RefTowerChapterMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTowerChapterMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTowerChapterMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTowerChapterStageShow newRef = (RefTowerChapterStageShow) _newRef;
        id = newRef.id;
        show_lvl = newRef.show_lvl;
    }

    public static class RefTowerChapterMgr extends RefTableContainer<RefTowerChapterStageShow>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public int show_lvl;//展示阶梯
}
