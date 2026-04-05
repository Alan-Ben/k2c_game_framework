package NPGameRes.Refs.Mars;

import Common.MarsEnum.EMarsBuildingType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

import java.util.ArrayList;

@RefTable(tableName = "mars_building")
public class RefMarsBuilding extends RefBase
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
        RefMarsBuilding newRef = (RefMarsBuilding) _newRef;
        id = newRef.id;
        building_type = newRef.building_type;
        upgrade_group_id = newRef.upgrade_group_id;
        settle_group_id = newRef.settle_group_id;
        build_condition_id_list = newRef.build_condition_id_list;
        build_cost_list = newRef.build_cost_list;
        build_time_cost_sec = newRef.build_time_cost_sec;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsBuildingMgr extends RefTableContainer<RefMarsBuilding>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//唯一 id (每个 id 都是一个实例建筑）
    public EMarsBuildingType building_type = EMarsBuildingType.NONE;//建筑类型（EMarsBuildingType）
    
    public int upgrade_group_id;//升级组 id
    public int settle_group_id;//派遣的组 id
    public ArrayList<Long> build_condition_id_list = new ArrayList<>();//建造条件列表
    public ArrayList<NPCommonCostItem> build_cost_list = new ArrayList<>();//建造消耗列表
    public long build_time_cost_sec;//建造花费时间（秒）
    
    @RefField(isIgnore = true)
    public ArrayList<RefMarsBuildingCondition> conditionRefList = new ArrayList<>();
    
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsBuildingLevel> _m_lmLevelMapMgr = new _TLevelMapMgr<RefMarsBuildingLevel>();
    public void setLevelMapMgr(_TLevelMapMgr<RefMarsBuildingLevel> _mgr) {_m_lmLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsBuildingLevel> getLevelMapMgr() {return _m_lmLevelMapMgr;}

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsBuildingSettleLevel> _m_lmSettleLevelMapMgr = new _TLevelMapMgr<RefMarsBuildingSettleLevel>();
    public void setSettleLevelMapMgr(_TLevelMapMgr<RefMarsBuildingSettleLevel> _mgr) {_m_lmSettleLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsBuildingSettleLevel> getSettleLevelMapMgr() {return _m_lmSettleLevelMapMgr;}

    @RefField(isIgnore = true)
    public RefMarsBuildingEquipmentBelong equipmentBelongRef;
    
    @RefField(isIgnore = true)
    public ArrayList<RefMarsEquipment> equipmentRefList = new ArrayList<>();
}