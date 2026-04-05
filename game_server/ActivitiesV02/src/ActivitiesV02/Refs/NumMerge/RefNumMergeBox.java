package ActivitiesV02.Refs.NumMerge;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "num_merge_box")
public class RefNumMergeBox extends RefBase
{
    // 全局管理器实例
    private static RefNumMergeBoxMgr _g_mgr = new RefNumMergeBoxMgr();

    public static RefNumMergeBoxMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefNumMergeBoxMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefNumMergeBoxMgr) _mgr;
    }

    /**
     * 配置表管理器
     */
    public static class RefNumMergeBoxMgr extends RefTableContainer<RefNumMergeBox>
    {
        @Override
        public void _onTableLoaded()
        {
            // 配置表加载完成后的处理逻辑
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefNumMergeBox newRef = (RefNumMergeBox) _newRef;
        step = newRef.step;
        upgrade_need_score = newRef.upgrade_need_score;
        reward_id = newRef.reward_id;
    }

    /**
     * 获取对象数据Id，使用阶段作为唯一ID
     */
    @Override
    public long Id()
    {
        return step;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 配置字段

    // 阶段（0-10）
    public int step;

    // 升级到下一阶段所需积分
    public long upgrade_need_score;

    // 奖励ID（对应奖励配置表的ID）
    public long reward_id;
}
