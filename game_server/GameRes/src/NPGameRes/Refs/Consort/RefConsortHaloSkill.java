package NPGameRes.Refs.Consort;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

/**
 * @author mark
 */
@RefTable(tableName = "consort_halo_skill")
public class RefConsortHaloSkill extends RefBase
{
    private static RefConsortHaloSkillMgr _g_mgr = new RefConsortHaloSkillMgr();
    public static RefConsortHaloSkillMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortHaloSkill> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortHaloSkillMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortHaloSkill newRef = (RefConsortHaloSkill) _newRef;
        halo_skill_id = newRef.halo_skill_id;
    }
    
    public static class RefConsortHaloSkillMgr extends RefTableContainer<RefConsortHaloSkill>
    {
    	@Override
        public void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return halo_skill_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long halo_skill_id;
    
    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefConsortHaloSkillLvl> _m_lmLevelMapMgr = new _TLevelMapMgr<RefConsortHaloSkillLvl>();
    public void setLevelMapMgr(_TLevelMapMgr<RefConsortHaloSkillLvl> _mgr) {_m_lmLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefConsortHaloSkillLvl> getLevelMapMgr() {return _m_lmLevelMapMgr;}
}
