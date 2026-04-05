package NPGameRes.Refs.Inn;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "inn_dish_level")
public class RefInnDishLevel extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnDishLevelMgr _g_mgr = new RefInnDishLevelMgr();

    // 2. 静态方法
    public static RefInnDishLevelMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnDishLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnDishLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnDishLevel newRef = (RefInnDishLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        up_need_finesse = newRef.up_need_finesse;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefInnDishLevelMgr extends RefTableContainer<RefInnDishLevel>
    {
        private Map<Long, List<RefInnDishLevel>> _m_groupMap = new HashMap<>();

        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的分组缓存逻辑
            Map<Long, List<RefInnDishLevel>> groupMap = new HashMap<>();
            for (RefInnDishLevel ref : getList())
            {
                groupMap.computeIfAbsent(ref.group_id, k -> new ArrayList<>()).add(ref);
            }
            _m_groupMap = groupMap;
        }

        /**
         * 根据组ID和等级获取菜品等级配置
         */
        public RefInnDishLevel getByGroupAndLevel(long groupId, int level)
        {
            List<RefInnDishLevel> refList = _m_groupMap.get(groupId);
            if (refList != null)
            {
                for (RefInnDishLevel ref : refList)
                {
                    if (ref.level == level)
                    {
                        return ref;
                    }
                }
            }
            return null;
        }
    }

    // 5. 配表字段定义
    public long id; // 唯一ID（主键）
    public long group_id; // 组ID
    public int level; // 等级
    public long up_need_finesse; // 升级所需熟练度
}