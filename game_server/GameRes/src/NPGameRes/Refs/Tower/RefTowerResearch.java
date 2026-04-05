package NPGameRes.Refs.Tower;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "tower_research")
public class RefTowerResearch extends RefBase
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
        RefTowerResearch newRef = (RefTowerResearch) _newRef;
        id = newRef.id;
        chapter_id = newRef.chapter_id;
        level = newRef.level;
        building_profit_add_per = newRef.building_profit_add_per;
    }

    public static class RefTowerChapterMgr extends RefTableContainer<RefTowerResearch>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
        
        /**
         * 获取对应章节等级的配置数据
         * @param _chapterId
         * @param _lvl
         * @return
         */
        public RefTowerResearch lookupByChapterLvl(long _chapterId, int _lvl)
        {
        	for (RefTowerResearch refTowerResearch : getList())
            {
                if (refTowerResearch.chapter_id == _chapterId && refTowerResearch.level == _lvl)
                {
                    return refTowerResearch;
                }
            }
        	
        	return null;
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
    public long chapter_id;//章节id
    public int level;//楼层数
    public int building_profit_add_per;//建筑收益提升万分比
}
