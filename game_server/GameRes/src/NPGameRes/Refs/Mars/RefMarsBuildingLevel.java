package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.PlayerMarsProperty.PlayerMarsPropertyModifier;

import java.util.ArrayList;

@RefTable(tableName = "mars_building_level")
public class RefMarsBuildingLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsBuildingMgr _g_mgr = new RefMarsBuildingMgr();

    public static RefMarsBuildingMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsBuildingMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsBuildingMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsBuildingLevel newRef = (RefMarsBuildingLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        upgrade_condition_id_list = newRef.upgrade_condition_id_list;
        upgrade_cost_list = newRef.upgrade_cost_list;
        upgrade_time_cost_sec = newRef.upgrade_time_cost_sec;
        mars_power_value = newRef.mars_power_value;
        player_property = newRef.player_property;
        mars_property = newRef.mars_property;
        building_level_cofficient = newRef.building_level_cofficient;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsBuildingMgr extends RefTableContainer<RefMarsBuildingLevel>
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
    public ArrayList<Long> upgrade_condition_id_list = new ArrayList<>() ;//条件列表
    public ArrayList<NPCommonCostItem> upgrade_cost_list = new ArrayList<>();//消耗列表
    public long upgrade_time_cost_sec;//花费时间（秒）
    public int mars_power_value;//火星实力值
    public NPPlayerPropertyModifier player_property;//玩家属性
    public PlayerMarsPropertyModifier mars_property;//火星系统属性
    public int building_level_cofficient;//等级修正系数
    
    @RefField(isIgnore = true)
    public ArrayList<RefMarsBuildingCondition> conditionRefList = new ArrayList<>();
}