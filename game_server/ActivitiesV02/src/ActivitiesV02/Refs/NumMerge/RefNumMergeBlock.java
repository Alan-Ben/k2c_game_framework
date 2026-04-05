package ActivitiesV02.Refs.NumMerge;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 数字合并棋子配置表
 *
 * 主要功能：
 * 1. 定义不同等级棋子的配置
 * 2. 配置每个等级的基础积分
 * 3. 配置每个等级的基础奖券
 */
@RefTable(tableName = "num_merge_block")
public class RefNumMergeBlock extends RefBase
{
    // 全局管理器实例
    private static RefNumMergeBlockMgr _g_mgr = new RefNumMergeBlockMgr();

    public static RefNumMergeBlockMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefNumMergeBlockMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefNumMergeBlockMgr) _mgr;
    }

    /**
     * 配置表管理器
     */
    public static class RefNumMergeBlockMgr extends RefTableContainer<RefNumMergeBlock>
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
        RefNumMergeBlock newRef = (RefNumMergeBlock) _newRef;
        level = newRef.level;
        merge_gain_score = newRef.merge_gain_score;
        delete_gain_score = newRef.delete_gain_score;
        merge_gain_ticket = newRef.merge_gain_ticket;
        buff_gen_weight = newRef.buff_gen_weight;
    }

    /**
     * 获取对象数据Id，使用等级作为唯一ID
     */
    @Override
    public long Id()
    {
        return level;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 棋子等级（1-11）
    public int level;

    // 每合成到当前等级获得积分
    public long merge_gain_score;

    // 消除积分
    public long delete_gain_score;

    // 每合成到当前等级获得奖券
    public long merge_gain_ticket;

    // 生成buff的权重
    public int buff_gen_weight;
}
