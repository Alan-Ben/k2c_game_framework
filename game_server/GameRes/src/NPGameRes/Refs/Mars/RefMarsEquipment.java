package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

@RefTable(tableName = "mars_equipment")
public class RefMarsEquipment extends RefBase
{
    private static RefMarsEquipmentMgr _g_mgr = new RefMarsEquipmentMgr();

    public static RefMarsEquipmentMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsEquipmentMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsEquipmentMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEquipment newRef = (RefMarsEquipment) _newRef;
        id = newRef.id;
        upgrade_group_id = newRef.upgrade_group_id;
        unlock_level = newRef.unlock_level;
        level_limit_ratio = newRef.level_limit_ratio;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsEquipmentMgr extends RefTableContainer<RefMarsEquipment>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//唯一 id
    public int upgrade_group_id;//升级组 id
    public int unlock_level;//主建筑达到这个等级时解锁
    public int level_limit_ratio;//可以升到的等级（为和所属的建筑等级的比值）

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsEquipmentLevel> _m_lmLevelMapMgr = new _TLevelMapMgr<RefMarsEquipmentLevel>();
    public void setLevelMapMgr(_TLevelMapMgr<RefMarsEquipmentLevel> _mgr) {_m_lmLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsEquipmentLevel> getLevelMapMgr() {return _m_lmLevelMapMgr;}

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsEquipmentEnergyLevel> _m_lmEnergyLevelMapMgr = new _TLevelMapMgr<RefMarsEquipmentEnergyLevel>();
    public void setEnergyLevelMapMgr(_TLevelMapMgr<RefMarsEquipmentEnergyLevel> _mgr) {_m_lmEnergyLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsEquipmentEnergyLevel> getEnergyLevelMapMgr() {return _m_lmEnergyLevelMapMgr;}

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsEquipmentFoodLevel> _m_lmFoodLevelMapMgr = new _TLevelMapMgr<RefMarsEquipmentFoodLevel>();
    public void setFoodLevelMapMgr(_TLevelMapMgr<RefMarsEquipmentFoodLevel> _mgr) {_m_lmFoodLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsEquipmentFoodLevel> getFoodLevelMapMgr() {return _m_lmFoodLevelMapMgr;}

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsEquipmentHospitalLevel> _m_lmHospitalLevelMapMgr = new _TLevelMapMgr<RefMarsEquipmentHospitalLevel>();
    public void setHospitalLevelMapMgr(_TLevelMapMgr<RefMarsEquipmentHospitalLevel> _mgr) {_m_lmHospitalLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsEquipmentHospitalLevel> getHospitalLevelMapMgr() {return _m_lmHospitalLevelMapMgr;}

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsEquipmentLivingLevel> _m_lmLivingLevelMapMgr = new _TLevelMapMgr<RefMarsEquipmentLivingLevel>();
    public void setLivingLevelMapMgr(_TLevelMapMgr<RefMarsEquipmentLivingLevel> _mgr) {_m_lmLivingLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsEquipmentLivingLevel> getLivingLevelMapMgr() {return _m_lmLivingLevelMapMgr;}
}