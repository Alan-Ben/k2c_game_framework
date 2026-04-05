package NPGameRes.Refs.Tower;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.Tower.TowerStageRefObj;

import java.util.ArrayList;
import java.util.List;

/**
 * @author mark
 */
@RefTable(tableName = "tower_chapter")
public class RefTowerChapter extends RefBase
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
        RefTowerChapter newRef = (RefTowerChapter) _newRef;
        id = newRef.id;
        gain_item_list = newRef.gain_item_list;
        initial_stage_id = newRef.initial_stage_id;
        if_pve_chapter = newRef.if_pve_chapter;
        research_finish_reward = newRef.research_finish_reward;
    }

    public static class RefTowerChapterMgr extends RefTableContainer<RefTowerChapter>
    {
        @Override
        protected void _onTableLoaded()
        {
        }

        public TowerStageRefObj getStageRefObj(long _chapterId, int _chapterLevel)
        {
            RefTowerChapter refTowerChapter = get(_chapterId);
            if (refTowerChapter == null)
                return null;

            return refTowerChapter.getStageRefObj(_chapterLevel);
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

    public long id;//章节id
    public ArrayList<NPCommonCostItem> gain_item_list;//到达该章节的奖励物品列表
    public long initial_stage_id;//起始stage_id
    public boolean if_pve_chapter;//是否是PVE章节
    public List<NPCommonCostItem> research_finish_reward;//章节研究完成奖励


    @RefField(isIgnore = true)
    public List<TowerStageRefObj> stageList = new ArrayList<>();

    public void setStageList(List<TowerStageRefObj> _stageList)
    {
        stageList = _stageList;
    }

    /**
     * 获取指定章节等级对应的阶段对象
     * @param _chapterLevel
     * @return
     */
    public TowerStageRefObj getStageRefObj(int _chapterLevel)
    {
        if (stageList == null)
            return null;

        TowerStageRefObj tempObj = null;

        for (TowerStageRefObj refObj : stageList)
        {
            //因为传入值是从1开始的，所以需要减1
            if (refObj.getChapterStartLevel() > _chapterLevel)
                return tempObj;

            tempObj = refObj;
        }

        return tempObj;
    }
}
