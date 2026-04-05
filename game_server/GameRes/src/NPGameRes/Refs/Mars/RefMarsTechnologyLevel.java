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

@RefTable(tableName = "mars_technology_level")
public class RefMarsTechnologyLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsTechnologyLevelMgr _g_mgr = new RefMarsTechnologyLevelMgr();

    public static RefMarsTechnologyLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsTechnologyLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsTechnologyLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsTechnologyLevel newRef = (RefMarsTechnologyLevel) _newRef;
        id = newRef.id;
        level = newRef.level;
        technology_id = newRef.technology_id;
        condition_id_list = newRef.condition_id_list;
        upgrade_consume_list = newRef.upgrade_consume_list;
        upgrade_time_sec = newRef.upgrade_time_sec;
        player_property = newRef.player_property;
        mars_property = newRef.mars_property;
        mars_power = newRef.mars_power;
    }

    @Override
    public long Id()
    {
        return id;
    }

    @Override
    public int getLevel()
    {
    	return level;
    }

    public static class RefMarsTechnologyLevelMgr extends RefTableContainer<RefMarsTechnologyLevel>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//科技id
    public int level;//等级
    public long technology_id;//科技id
    public ArrayList<Long> condition_id_list = new ArrayList<>();//升到下级的条件列表
    public ArrayList<NPCommonCostItem> upgrade_consume_list = new ArrayList<>();//升到下级消耗
    public long upgrade_time_sec;//升到下级时间(秒)
    public NPPlayerPropertyModifier player_property;//玩家属性
    public PlayerMarsPropertyModifier mars_property;//火星系统属性
    public long mars_power;//火星实力

    @RefField(isIgnore = true)
    public ArrayList<RefMarsBuildingCondition> conditionRefList = new ArrayList<>();
}