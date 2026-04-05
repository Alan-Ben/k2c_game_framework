package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

@RefTable(tableName = "mars_building_equipment_belong")
public class RefMarsBuildingEquipmentBelong extends RefBase
{
    private static RefMarsBuildingEquipmentBelongMgr _g_mgr = new RefMarsBuildingEquipmentBelongMgr();

    public static RefMarsBuildingEquipmentBelongMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsBuildingEquipmentBelongMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsBuildingEquipmentBelongMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsBuildingEquipmentBelong newRef = (RefMarsBuildingEquipmentBelong) _newRef;
        group_id = newRef.group_id;
        main_equipment_id_list = newRef.main_equipment_id_list;
        other_equipment_id_list = newRef.other_equipment_id_list;
    }

    @Override
    public long Id()
    {
        return group_id;
    }

    public static class RefMarsBuildingEquipmentBelongMgr extends RefTableContainer<RefMarsBuildingEquipmentBelong>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public int group_id;//建筑组 id
    public ArrayList<Long> main_equipment_id_list = new ArrayList<>();//主要部件列表（升级需要先把这些部件升满）
    public ArrayList<Long> other_equipment_id_list = new ArrayList<>();//其它部件列表（与建筑升级无关）
}