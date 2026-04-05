package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;

/*********
 * 大臣星级技能信息
 */
@RefTable(tableName = "hero_star_skill")
public class RefHeroStarSkill extends RefBase
{
    private static RefTableContainer<RefHeroStarSkill> _g_mgr = new RefTableContainer<RefHeroStarSkill>();

    public static RefTableContainer<RefHeroStarSkill> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroStarSkill> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroStarSkill>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroStarSkill newRef = (RefHeroStarSkill) _newRef;
        skill_id = newRef.skill_id;
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
        return skill_id;
    }

    public long skill_id;//技能id

    @RefField(isIgnore = true)
    private _TLevelAreaMgr<RefHeroStarSkillLevel> _m_levelAreaMgr = new _TLevelAreaMgr<>();
    public _TLevelAreaMgr<RefHeroStarSkillLevel> getLevelAreaMgr()
    {
        return _m_levelAreaMgr;
    }

    public void setLevelAreaMgr(_TLevelAreaMgr<RefHeroStarSkillLevel> _mgr)
    {
        _m_levelAreaMgr = _mgr;
    }
}
