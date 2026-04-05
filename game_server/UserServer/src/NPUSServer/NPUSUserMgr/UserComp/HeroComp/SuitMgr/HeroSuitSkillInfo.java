package NPUSServer.NPUSUserMgr.UserComp.HeroComp.SuitMgr;

import Common.HeroObj.Hero_SuitSkillInfo;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Hero.RefHeroSuitSkill;
import NPGameRes.Refs.Hero.RefHeroSuitSkillLevel;

/**********
 * 大臣觉醒技能数据
 * 技能等级跟随星级直接走
 */
public class HeroSuitSkillInfo
{
    private HeroSuitInfo _m_suitInfo;

    private RefHeroSuitSkill _m_sSkillRef;

    //由于等级数据是分段存储的，所以这里等级单独变量存储
    private int _m_lLevel;
    private RefHeroSuitSkillLevel _m_lrSkillLevelRef;

    public HeroSuitSkillInfo(HeroSuitInfo _suitInfo, RefHeroSuitSkill _ref)
    {
        _m_suitInfo = _suitInfo;

        _m_sSkillRef = _ref;

        _m_lLevel = 0;
        _m_lrSkillLevelRef = null;
    }

    public long getStarSkillId()
    {
        return _m_sSkillRef.id;
    }

    public RefHeroSuitSkillLevel getSkillLevelRef()
    {
        return _m_lrSkillLevelRef;
    }

    public int getSkillLevel()
    {
        return _m_lLevel;
    }

    //获取每级加成的倍率
    public int getLevelBonusStack()
    {
        return null == _m_lrSkillLevelRef ? 0 : _m_lLevel - _m_lrSkillLevelRef.level;
    }

    /**
     * 等级变化处理
     * @param _level
     */
    public void setLevel(int _level)
    {
        if (null == _m_sSkillRef)
            return;

        //设置等级数据
        _m_lLevel = _level;
        _m_lrSkillLevelRef = _m_sSkillRef.getLevelMapMgr().getLevelData(_level);
        if (null == _m_lrSkillLevelRef)
        {
            CommLog.error("HeroSuitSkillInfo setLevel refSuitSkillLevel not found, skillId:{}, level:{}", _m_sSkillRef.id, _level);
        }
    }

    /**
     * 打印调试信息
     */
    @Override
    public String toString()
    {
        return "    skillId=" + _m_sSkillRef.id + ", level=" + _m_lLevel;
    }

    /**
     * 构造协议信息
     * @return
     */
    public Hero_SuitSkillInfo toProto()
    {
        Hero_SuitSkillInfo proto = new Hero_SuitSkillInfo();
        proto.setSuitSkillId(_m_sSkillRef.Id());
        proto.setLevel(_m_lLevel);
        return proto;
    }
}
