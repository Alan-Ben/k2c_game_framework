package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

@RefTable(tableName = "mars_equipment_living_level")
public class RefMarsEquipmentLivingLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsEquipmentLivingLevelMgr _g_mgr = new RefMarsEquipmentLivingLevelMgr();

    public static RefMarsEquipmentLivingLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsEquipmentLivingLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsEquipmentLivingLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEquipmentLivingLevel newRef = (RefMarsEquipmentLivingLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        comfort_yield = newRef.comfort_yield;
        mood_yield = newRef.mood_yield;
        sleep_yield = newRef.sleep_yield;
        people_num_limit = newRef.people_num_limit;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsEquipmentLivingLevelMgr extends RefTableContainer<RefMarsEquipmentLivingLevel>
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
    public int comfort_yield;//产出舒适值
    public int mood_yield;//产出心情值
    public int sleep_yield;//产出睡眠值
    public int people_num_limit;//提升居民上限(覆盖值)
}