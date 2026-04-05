package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

@RefTable(tableName = "mars_equipment_hospital_level")
public class RefMarsEquipmentHospitalLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsEquipmentHospitalLevelMgr _g_mgr = new RefMarsEquipmentHospitalLevelMgr();

    public static RefMarsEquipmentHospitalLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsEquipmentHospitalLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsEquipmentHospitalLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEquipmentHospitalLevel newRef = (RefMarsEquipmentHospitalLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        cure_rate = newRef.cure_rate;
        cure_num_range = newRef.cure_num_range;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsEquipmentHospitalLevelMgr extends RefTableContainer<RefMarsEquipmentHospitalLevel>
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

    public long id;//唯一id
    public int group_id;//部件组 id
    public int level;//等级
    public int cure_rate;//治愈率
    public WCGPairInt cure_num_range = new WCGPairInt();//同时治愈的生病居民数量范围
}