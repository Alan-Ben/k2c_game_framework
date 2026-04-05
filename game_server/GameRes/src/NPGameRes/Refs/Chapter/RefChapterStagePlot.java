package NPGameRes.Refs.Chapter;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.List;

@RefTable(tableName = "chapter_stage_plot")
public class RefChapterStagePlot extends RefBase
{
    private static RefChapterStagePlotMgr _g_mgr = new RefChapterStagePlotMgr();

    public static RefChapterStagePlotMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterStagePlotMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterStagePlotMgr) _mgr;
    }

    public static class RefChapterStagePlotMgr extends RefTableContainer<RefChapterStagePlot>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterStagePlot newRef = (RefChapterStagePlot) _newRef;
        plot_id = newRef.plot_id;
        unlock_condition = newRef.unlock_condition;
        reward_item_list = newRef.reward_item_list;
    }

    @Override
    public long Id()
    {
        return plot_id;
    }

    public long plot_id;//剧情id
    public NPPlayerConditionGroupObj unlock_condition;//解锁条件
    public List<NPCommonCostItem> reward_item_list;//奖励列表
}
