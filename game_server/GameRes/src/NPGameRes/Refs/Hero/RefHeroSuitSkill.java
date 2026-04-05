package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;

@RefTable(tableName = "hero_halo_suit_skill")
public class RefHeroSuitSkill extends RefBase
{
    private static RefTableContainer<RefHeroSuitSkill> _g_mgr = new RefTableContainer<RefHeroSuitSkill>();

    public static RefTableContainer<RefHeroSuitSkill> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroSuitSkill> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroSuitSkill>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroSuitSkill newRef = (RefHeroSuitSkill) _newRef;
        id = newRef.id;
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

    public long id; // 唯一id

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelAreaMgr<RefHeroSuitSkillLevel> _m_lmLevelMapMgr = new _TLevelAreaMgr<RefHeroSuitSkillLevel>();

    public _TLevelAreaMgr<RefHeroSuitSkillLevel> getLevelMapMgr()
    {
        return _m_lmLevelMapMgr;
    }

    public void setLevelMapMgr(_TLevelAreaMgr<RefHeroSuitSkillLevel> _mgr)
    {
        _m_lmLevelMapMgr = _mgr;
    }
}
