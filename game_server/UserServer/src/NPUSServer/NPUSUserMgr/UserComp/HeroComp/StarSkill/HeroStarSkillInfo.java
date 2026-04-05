package NPUSServer.NPUSUserMgr.UserComp.HeroComp.StarSkill;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Hero.RefHeroStarSkill;
import NPGameRes.Refs.Hero.RefHeroStarSkillLevel;

/**********
 * 大臣觉醒技能数据
 * 技能等级跟随星级直接走
 */
public class HeroStarSkillInfo
{
    private HeroStarSkillMgr _m_mgr;

    private RefHeroStarSkill _m_sSkillRef;
    private RefHeroStarSkillLevel _m_lrSkillLevelRef;

    public HeroStarSkillInfo(HeroStarSkillMgr _mgr, RefHeroStarSkill _ref, int _starLevel)
    {
        _m_mgr = _mgr;

        _m_sSkillRef = _ref;
        //技能等级=星级+1
        _m_lrSkillLevelRef = _ref.getLevelAreaMgr().getLevelData(_starLevel + 1);

        if (null == _m_lrSkillLevelRef)
            CommLog.error("HeroStarSkillInfo _initBo levelRef not found, skillId:{}, level:{}", _ref.skill_id, _starLevel);
    }

    public long getStarSkillId()
    {
        return _m_sSkillRef.skill_id;
    }

    public RefHeroStarSkillLevel getSkillLevelRef()
    {
        return _m_lrSkillLevelRef;
    }

    /**
     * 等级变化处理
     * @param _starLevel
     */
    public void onStarLevelChg(int _starLevel)
    {
        if (null == _m_sSkillRef)
            return;

        //设置等级数据
        //技能等级=星级+1
        _m_lrSkillLevelRef = _m_sSkillRef.getLevelAreaMgr().getLevelData(_starLevel + 1);
    }
}
