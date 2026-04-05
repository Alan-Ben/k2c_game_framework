package ActivitiesV02.Refs.NumMerge;

import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * 数字合并模式配置表
 *
 * 主要功能：
 * 1. 定义不同游戏模式的配置（普通、快速、极速）
 * 2. 配置模式的消耗和倍率
 * 3. 配置模式的生成权重
 */
@RefTable(tableName = "num_merge_mode")
public class RefNumMergeMode extends RefBase
{
    // 全局管理器实例
    private static RefNumMergeModeMgr _g_mgr = new RefNumMergeModeMgr();

    public static RefNumMergeModeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefNumMergeModeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefNumMergeModeMgr) _mgr;
    }

    /**
     * 配置表管理器
     */
    public static class RefNumMergeModeMgr extends RefTableContainer<RefNumMergeMode>
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
        RefNumMergeMode newRef = (RefNumMergeMode) _newRef;
        type = newRef.type;
        consume_cd = newRef.consume_cd;
        unlock_need_total_score = newRef.unlock_need_total_score;
        unlock_condition = newRef.unlock_condition;
        gen_block_level = newRef.gen_block_level;
    }

    /**
     * 获取对象数据Id，使用模式类型的序号作为唯一ID
     */
    @Override
    public long Id()
    {
        return type.ordinal();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 模式类型
    public ENumMerge_ModeType type;

    // 消耗体力数量
    public int consume_cd;

    // 解锁需要的累计积分
    public long unlock_need_total_score;

    // 解锁条件
    public NPPlayerConditionGroupObj unlock_condition = new NPPlayerConditionGroupObj();

    // 生成棋子等级
    public int gen_block_level;
}