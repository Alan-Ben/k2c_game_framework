package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

@RefTable(tableName = "mars_equipment_energy_level")
public class RefMarsEquipmentEnergyLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsEquipmentEnergyLevelMgr _g_mgr = new RefMarsEquipmentEnergyLevelMgr();

    public static RefMarsEquipmentEnergyLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsEquipmentEnergyLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsEquipmentEnergyLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEquipmentEnergyLevel newRef = (RefMarsEquipmentEnergyLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        output_value_per_min = newRef.output_value_per_min;
        max_storage = newRef.max_storage;
        people_output_value_per_min = newRef.people_output_value_per_min;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsEquipmentEnergyLevelMgr extends RefTableContainer<RefMarsEquipmentEnergyLevel>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    @Override
    public int getLevel()
    {
    	return level;
    }

    public long id;//唯一 id
    public int group_id;//部件组 id
    public int level;//等级
    public int output_value_per_min;//能源厂基础产出/分
    public long max_storage;//最大资源储存量
    public int people_output_value_per_min;//居民派遣基础产出/分
}