package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

@RefTable(tableName = "mars_equipment_food_level")
public class RefMarsEquipmentFoodLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsPeopleRewardHelpMgr _g_mgr = new RefMarsPeopleRewardHelpMgr();

    public static RefMarsPeopleRewardHelpMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsPeopleRewardHelpMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsPeopleRewardHelpMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEquipmentFoodLevel newRef = (RefMarsEquipmentFoodLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        energy_consume_per_min = newRef.energy_consume_per_min;
        satiety_yield = newRef.satiety_yield;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsPeopleRewardHelpMgr extends RefTableContainer<RefMarsEquipmentFoodLevel>
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
    public int energy_consume_per_min;//每分钟资源消耗
    public int satiety_yield;//产出饱腹值
}