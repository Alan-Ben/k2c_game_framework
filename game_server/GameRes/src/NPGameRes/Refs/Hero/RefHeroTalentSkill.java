package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;

@RefTable(tableName = "hero_talent_skill")
public class RefHeroTalentSkill extends RefBase
{
    private static RefTableContainer<RefHeroTalentSkill> _g_mgr = new RefTableContainer<RefHeroTalentSkill>();

    public static RefTableContainer<RefHeroTalentSkill> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroTalentSkill> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroTalentSkill>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroTalentSkill newRef = (RefHeroTalentSkill) _newRef;
        id = newRef.id;
        level_limit = newRef.level_limit;
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
        return id;
    }

    public long id;//技能id
    public int level_limit;//等级上限

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelAreaMgr<RefHeroTalentSkillLevel> _m_lmLevelMapMgr = new _TLevelAreaMgr<>();

    public _TLevelAreaMgr<RefHeroTalentSkillLevel> getLevelMapMgr()
    {
        return _m_lmLevelMapMgr;
    }

    public void setLevelMapMgr(_TLevelAreaMgr<RefHeroTalentSkillLevel> _mgr)
    {
        _m_lmLevelMapMgr = _mgr;
    }
}
