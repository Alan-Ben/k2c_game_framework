package NPGameRes.Refs.Building;

import CommonEnum.ESpecAttrType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;

import java.util.List;


@RefTable(tableName = "business_building")
public class RefBusinessBuilding extends RefBase
{
    private static RefBusinessBuildingMgr _g_mgr = new RefBusinessBuildingMgr();
    public static RefBusinessBuildingMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefBusinessBuildingMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefBusinessBuildingMgr) _mgr;
    }

    public static class RefBusinessBuildingMgr extends RefTableContainer<RefBusinessBuilding>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBusinessBuilding newRef = (RefBusinessBuilding) _newRef;
        building_id = newRef.building_id;
        employee_earnings = newRef.employee_earnings;
        employee_base_max_count = newRef.employee_base_max_count;
        hero_slot_employee_num_list = newRef.hero_slot_employee_num_list;
        addition_employee_count_per_level = newRef.addition_employee_count_per_level;
        hire_cost_multiple = newRef.hire_cost_multiple;
        attr_type = newRef.attr_type;
    }

    @Override
    public long Id()
    {
        return building_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long building_id;
    public int employee_earnings;//每个员工每秒赚取的资源数
    public int employee_base_max_count;//基础最大能容纳的员工数
    public List<Integer> hero_slot_employee_num_list;//随从插槽所需的雇员数
    public int addition_employee_count_per_level;//每级所提供的额外员工容量
    public int hire_cost_multiple;//招募员工消耗倍率
    public ESpecAttrType attr_type = ESpecAttrType.NONE;//特长属性类型;//偏向属性

    @RefField(isIgnore = true)
    private _TLevelAreaMgr<RefBusinessBuildingLevel> _m_levelMapMgr = new _TLevelAreaMgr<>();
    public _TLevelAreaMgr<RefBusinessBuildingLevel> getLevelMapMgr()
    {
        return _m_levelMapMgr;
    }

    public void setLevelMapMgr(_TLevelAreaMgr<RefBusinessBuildingLevel> _mgr)
    {
        _m_levelMapMgr = _mgr;
    }

    @RefField(isIgnore = true)
    private List<RefBusinessBuildingDevelop> _m_developList;
    public List<RefBusinessBuildingDevelop> getDevelopList()
    {
        return _m_developList;
    }
    public void setDevelopList(List<RefBusinessBuildingDevelop> _value)
    {
        _m_developList = _value;
    }


}
