package NPGameRes.Refs.RushExchange;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 急速兑换礼包配置表
 *
 * 主要功能：
 * 1. 管理礼包的基本配置信息
 * 2. 维护礼包的消耗和奖励物品列表
 * 3. 关联礼包分组
 *
 * 容器类型：普通表（RefTableContainer）
 * 数据结构：支持列表类型字段（消耗列表、奖励列表）
 */
@RefTable(tableName = "rush_exchange")
public class RefRushExchange extends RefBase
{
    private static RefRushExchangeMgr _g_mgr = new RefRushExchangeMgr();

    /**
     * 获取配置表管理器
     */
    public static RefRushExchangeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRushExchangeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRushExchangeMgr) _mgr;
    }

    /**
     * 礼包配置表管理器
     *
     * 继承RefTableContainer支持多条记录管理
     * _onTableLoaded()用于在表加载完成后进行初始化处理
     */
    public static class RefRushExchangeMgr extends RefTableContainer<RefRushExchange>
    {
        private Map<Long,List<RefRushExchange>> groupIdToRefsMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            // 表加载完成后的初始化逻辑
            Map<Long,List<RefRushExchange>> tempMap = new HashMap<>();
            for (RefRushExchange ref : getList())
            {
                tempMap.computeIfAbsent(ref.group_id, k -> new ArrayList<>()).add(ref);
            }

            groupIdToRefsMap = tempMap;
        }

        /**
         * 根据礼包组ID获取该组下的所有礼包配置
         *
         * 执行流程：
         * 1. 遍历所有礼包配置
         * 2. 筛选出group_id匹配的配置
         * 3. 返回匹配结果列表
         *
         * @param _groupId 礼包组ID
         * @return 该组下的所有礼包配置列表，如果没有则返回空列表
         *
         * 线程安全：只读操作，无需加锁
         */
        public List<RefRushExchange> getListByGroupId(long _groupId)
        {
            return groupIdToRefsMap.get(_groupId);
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRushExchange newRef = (RefRushExchange) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        cost_list = newRef.cost_list;
        reward_list = newRef.reward_list;
    }

    /**
     * 获取对象数据Id，用于唯一标识本条配置记录
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 配置字段定义
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /** 配置ID（主键） */
    public long id;

    /** 礼包组ID（外键，关联RefRushExchangeGroup.group_id） */
    public long group_id;

    /** 消耗列表（包含需要消耗的物品和数量） */
    public List<NPCommonCostItem> cost_list = new ArrayList<>();

    /** 奖励列表（包含兑换获得的物品和数量） */
    public List<NPCommonCostItem> reward_list = new ArrayList<>();

}
