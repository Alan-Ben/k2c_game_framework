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
@RefTable(tableName = "consort_bless_skill")
public class RefConsortBlessSkill extends RefBase
{
    private static RefConsortBlessSkillMgr _g_mgr = new RefConsortBlessSkillMgr();
    public static RefConsortBlessSkillMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortBlessSkill> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortBlessSkillMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortBlessSkill newRef = (RefConsortBlessSkill) _newRef;
        bless_skill_id = newRef.bless_skill_id;
    }
    
    public static class RefConsortBlessSkillMgr extends RefTableContainer<RefConsortBlessSkill>
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
        return bless_skill_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long bless_skill_id;

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefConsortBlessSkillLvl> _m_lmLevelMapMgr = new _TLevelMapMgr<RefConsortBlessSkillLvl>();
    public void setLevelMapMgr(_TLevelMapMgr<RefConsortBlessSkillLvl> _mgr) {_m_lmLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefConsortBlessSkillLvl> getLevelMapMgr() {return _m_lmLevelMapMgr;}
}
