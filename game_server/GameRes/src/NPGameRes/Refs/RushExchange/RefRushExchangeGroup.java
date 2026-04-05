package NPGameRes.Refs.RushExchange;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * 急速兑换礼包组表配置
 *
 * 主要功能：
 * 1. 管理礼包分组信息
 * 2. 提供分组刷新条件配置
 *
 * 容器类型：普通表（RefTableContainer）
 */
@RefTable(tableName = "rush_exchange_group")
public class RefRushExchangeGroup extends RefBase
{
    private static RefRushExchangeGroupMgr _g_mgr = new RefRushExchangeGroupMgr();

    /**
     * 获取配置表管理器
     */
    public static RefRushExchangeGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRushExchangeGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRushExchangeGroupMgr) _mgr;
    }

    /**
     * 礼包组表管理器
     *
     * 继承RefTableContainer支持多条记录管理
     * _onTableLoaded()用于在表加载完成后进行初始化处理
     */
    public static class RefRushExchangeGroupMgr extends RefTableContainer<RefRushExchangeGroup>
    {
        @Override
        public void _onTableLoaded()
        {
            // 表加载完成后的初始化逻辑（如需要可在此处理跨表关联）
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRushExchangeGroup newRef = (RefRushExchangeGroup) _newRef;
        group_id = newRef.group_id;
        condition = newRef.condition;
    }

    /**
     * 获取对象数据Id，用于唯一标识本条配置记录
     */
    @Override
    public long Id()
    {
        return group_id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 配置字段定义
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /** 礼包组ID（主键） */
    public long group_id;

    /** 刷新条件 */
    public NPPlayerConditionGroupObj condition;

}
