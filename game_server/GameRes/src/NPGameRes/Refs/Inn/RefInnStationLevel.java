package NPGameRes.Refs.Inn;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "inn_station_level")
public class RefInnStationLevel extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnStationLevelMgr _g_mgr = new RefInnStationLevelMgr();

    // 2. 静态方法
    public static RefInnStationLevelMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnStationLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnStationLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnStationLevel newRef = (RefInnStationLevel) _newRef;
        id = newRef.id;
        station_id = newRef.station_id;
        level = newRef.level;
        upgrade_cost = newRef.upgrade_cost;
        popularity_add = newRef.popularity_add;
        affection_add = newRef.affection_add;
        finesse_add = newRef.finesse_add;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefInnStationLevelMgr extends RefTableContainer<RefInnStationLevel>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public long id; // 唯一ID（主键）
    public long station_id; // 设施ID
    public int level; // 等级
    public List<NPCommonCostItem> upgrade_cost; // 升级消耗
    public long popularity_add; // 做菜人气加成
    public long affection_add; // 做菜心意值加成（升级礼物用）
    public long finesse_add; // 做菜熟练度加成
}