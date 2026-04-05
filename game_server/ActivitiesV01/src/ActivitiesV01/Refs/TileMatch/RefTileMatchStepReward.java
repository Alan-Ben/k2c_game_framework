package ActivitiesV01.Refs.TileMatch;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "tilematch_step_reward")
public class RefTileMatchStepReward extends RefBase
{
    private static RefTileMatchStepRewardMgr _g_mgr = new RefTileMatchStepRewardMgr();

    public static RefTileMatchStepRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTileMatchStepRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchStepRewardMgr) _mgr;
    }

    public static class RefTileMatchStepRewardMgr extends RefTableContainer<RefTileMatchStepReward>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchStepReward newRef = (RefTileMatchStepReward) _newRef;
        step = newRef.step;
        goal = newRef.goal;
        jackpot_group_id = newRef.jackpot_group_id;
        draw_item_num = newRef.draw_item_num;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return step;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int step;//任务id
    public long goal;//目标值
    public long jackpot_group_id;//奖池组id
    public int draw_item_num;//道具抽取数量
}