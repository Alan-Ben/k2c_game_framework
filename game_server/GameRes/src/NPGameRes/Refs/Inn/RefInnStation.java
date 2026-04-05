package NPGameRes.Refs.Inn;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "inn_station")
public class RefInnStation extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnStationMgr _g_mgr = new RefInnStationMgr();

    // 2. 静态方法
    public static RefInnStationMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnStationMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnStationMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnStation newRef = (RefInnStation) _newRef;
        id = newRef.id;
        need_receive_guest_num = newRef.need_receive_guest_num;
        build_cost = newRef.build_cost;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefInnStationMgr extends RefTableContainer<RefInnStation>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public long id; // 站点ID（主键）
    public long need_receive_guest_num; // 解锁所需迎宾人数
    public List<NPCommonCostItem> build_cost; // 建造消耗

    @RefField(isIgnore = true)
    private List<RefInnStationLevel> _m_levelRefList = new ArrayList<>(); // 设施等级列表

    public void setLevelRefList(List<RefInnStationLevel> _levelRefList)
    {
        _m_levelRefList = _levelRefList;
    }

    /**
     * 根据等级获取设施等级参考数据
     * @param _level
     * @return
     */
    public RefInnStationLevel getLevelRef(int _level)
    {
        for (RefInnStationLevel ref : _m_levelRefList)
        {
            if (ref.level == _level)
            {
                return ref;
            }
        }
        return null; // 如果没有找到对应等级的设施
    }
}