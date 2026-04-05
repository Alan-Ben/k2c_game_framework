package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

@RefTable(tableName = "mars_building_settle_level")
public class RefMarsBuildingSettleLevel extends RefBase implements _ILevelBasicObj
{
    private static RefMarsBuildingSettleLevelMgr _g_mgr = new RefMarsBuildingSettleLevelMgr();

    public static RefMarsBuildingSettleLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsBuildingSettleLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsBuildingSettleLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsBuildingSettleLevel newRef = (RefMarsBuildingSettleLevel) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        slot_num = newRef.slot_num;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsBuildingSettleLevelMgr extends RefTableContainer<RefMarsBuildingSettleLevel>
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
    public int slot_num;//槽位数量
}