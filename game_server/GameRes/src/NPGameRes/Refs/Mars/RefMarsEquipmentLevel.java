package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

@RefTable(tableName = "mars_equipment_level")
public class RefMarsEquipmentLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsEquipmentLevelMgr _g_mgr = new RefMarsEquipmentLevelMgr();

    public static RefMarsEquipmentLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsEquipmentLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsEquipmentLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEquipmentLevel newRef = (RefMarsEquipmentLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        upgrade_cost = newRef.upgrade_cost;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsEquipmentLevelMgr extends RefTableContainer<RefMarsEquipmentLevel>
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
    public int group_id;//组 id 
    public int level;//等级
    public NPCommonCostItem upgrade_cost;//升级消耗
}