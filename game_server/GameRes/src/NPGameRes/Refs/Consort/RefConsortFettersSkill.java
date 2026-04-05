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
@RefTable(tableName = "consort_fetters_skill")
public class RefConsortFettersSkill extends RefBase
{
    private static RefConsortFettersSkillMgr _g_mgr = new RefConsortFettersSkillMgr();
    public static RefConsortFettersSkillMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortFettersSkill> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortFettersSkillMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortFettersSkill newRef = (RefConsortFettersSkill) _newRef;
        fetters_skill_id = newRef.fetters_skill_id;
    }
    
    public static class RefConsortFettersSkillMgr extends RefTableContainer<RefConsortFettersSkill>
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
        return fetters_skill_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long fetters_skill_id;

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefConsortFettersSkillLvl> _m_lmLevelMapMgr = new _TLevelMapMgr<RefConsortFettersSkillLvl>();
    public void setLevelMapMgr(_TLevelMapMgr<RefConsortFettersSkillLvl> _mgr) {_m_lmLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefConsortFettersSkillLvl> getLevelMapMgr() {return _m_lmLevelMapMgr;}
}
