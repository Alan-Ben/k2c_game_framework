package NPGameRes.Refs.Museum;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "museum_item_upgrade_cost")
public class RefMuseumItemUpgradeCost extends RefBase
{
    // 1. 静态管理器实例
    private static RefMuseumItemUpgradeCostMgr _g_mgr = new RefMuseumItemUpgradeCostMgr();

    // 2. 静态方法
    public static RefMuseumItemUpgradeCostMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefMuseumItemUpgradeCostMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMuseumItemUpgradeCostMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMuseumItemUpgradeCost newRef = (RefMuseumItemUpgradeCost) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        upgrade_cost_item = newRef.upgrade_cost_item;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefMuseumItemUpgradeCostMgr extends RefTableContainer<RefMuseumItemUpgradeCost>
    {
        private Map<Long, List<RefMuseumItemUpgradeCost>> _m_groupMap = new HashMap<>();

        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
            Map<Long, List<RefMuseumItemUpgradeCost>> groupMap = new HashMap<>();
            for (RefMuseumItemUpgradeCost ref : getList())
            {
                groupMap.computeIfAbsent(ref.group_id, k -> new ArrayList<>()).add(ref);
            }
            _m_groupMap = groupMap;
        }

        /**
         * 根据组ID和等级获取升级消耗列表
         */
        public RefMuseumItemUpgradeCost getUpgradeCostByGroupAndLevel(long groupId, int level)
        {
            List<RefMuseumItemUpgradeCost> refList = _m_groupMap.get(groupId);
            if (refList != null)
            {
                for (RefMuseumItemUpgradeCost ref : refList)
                {
                    if (ref.level == level)
                    {
                        return ref;
                    }
                }
            }
            return null; // 如果没有找到对应的消耗列表
        }
    }

    // 5. 配表字段定义
    public long id; // 唯一ID（主键）
    public long group_id; // 组ID
    public int level; // 等级
    public List<NPCommonCostItem> upgrade_cost_item; // 升级消耗
}