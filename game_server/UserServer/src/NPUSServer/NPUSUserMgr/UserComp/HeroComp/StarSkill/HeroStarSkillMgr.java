package NPUSServer.NPUSUserMgr.UserComp.HeroComp.StarSkill;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Hero.RefHeroStarSkill;
import NPGameRes.Refs.Hero.RefHeroStarSkillLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

import java.util.ArrayList;
import java.util.List;

public class HeroStarSkillMgr
{
    //大臣对象
    private HeroInfo _m_heroInfo;
    //技能列表
    private List<HeroStarSkillInfo> _m_skillList;

    public HeroStarSkillMgr(HeroInfo _heroInfo)
    {
        _m_heroInfo = _heroInfo;
        _m_skillList = new ArrayList<>();

        //初始化技能等级信息
        _initSkillInfo();
    }

    public HeroInfo getHeroInfo()
    {
        return _m_heroInfo;
    }

    /**************
     * 初始化皮肤信息，主要是放入默认皮肤数据
     */
    private void _initSkillInfo()
    {
        //数据非法判断
        if (null == _m_heroInfo || null == _m_heroInfo.getRef())
            return;

        //遍历解锁技能，等级根据星级直接设置
        for (Long skillId : _m_heroInfo.getRef().star_skill_id_list)
        {
            //如果已经存在对应技能则不添加
            if (null != lookupStarSkill(skillId))
                continue;

            RefHeroStarSkill starSkillRef = RefHeroStarSkill.getMgr().get(skillId);
            if (null == starSkillRef)
            {
                CommLog.error("HeroStarSkillMgr _initSkillInfo starSkillRef not found, skillId:{}", skillId);
                continue;
            }

            //根据星级直接构造等级数据放入数据集
            HeroStarSkillInfo starSkillInfo = new HeroStarSkillInfo(this, starSkillRef, _m_heroInfo.getStar());
            _m_skillList.add(starSkillInfo);

            //累加属性
            RefHeroStarSkillLevel skillLevelRef = starSkillInfo.getSkillLevelRef();
            if (skillLevelRef != null)
            {
            	getHeroInfo().getUserdata().getBonusMgr().addBonus(skillLevelRef.union_bonus);
            	//玩家属性
            	getHeroInfo().getComp().getPlayerPropertyContainer().addModifier(skillLevelRef.add_player);
            }
        }
    }

    /**
     * 查询星级技能对象
     * @param _skillId
     * @return
     */
    public HeroStarSkillInfo lookupStarSkill(long _skillId)
    {
        for (HeroStarSkillInfo starSkillInfo : _m_skillList)
        {
            if (_skillId == starSkillInfo.getStarSkillId())
                return starSkillInfo;
        }
        return null;
    }

    /**
     * 检查解锁默认技能
     * 需要在以下场景调用:
     * 1、升星后
     * 2、初始化完成
     * @param _context
     */
    public void onStarLevelChg(int _level, NPPlayerContext _context)
    {
        //遍历解锁技能，等级根据星级直接设置
        for (Long skillId : _m_heroInfo.getRef().star_skill_id_list)
        {
            HeroStarSkillInfo starSkillInfo = lookupStarSkill(skillId);
            if (starSkillInfo == null)
            {
                RefHeroStarSkill starSkillRef = RefHeroStarSkill.getMgr().get(skillId);
                if (null == starSkillRef)
                {
                    CommLog.error("HeroStarSkillMgr _initSkillInfo starSkillRef not found, skillId:{}", skillId);
                    continue;
                }

                starSkillInfo = new HeroStarSkillInfo(this, starSkillRef, _level);
                _m_skillList.add(starSkillInfo);

                //累加属性
                RefHeroStarSkillLevel skillLevelRef = starSkillInfo.getSkillLevelRef();
                if (skillLevelRef != null)
                {
                    getHeroInfo().getUserdata().getBonusMgr().addBonus(skillLevelRef.union_bonus);
                    //玩家属性
                    getHeroInfo().getComp().getPlayerPropertyContainer().addModifier(skillLevelRef.add_player);
                }
            } else
            {
                //记录原等级，用于后续属性处理
                RefHeroStarSkillLevel preSkillLevel = starSkillInfo.getSkillLevelRef();

                starSkillInfo.onStarLevelChg(_level);

                RefHeroStarSkillLevel newLevelRef = starSkillInfo.getSkillLevelRef();
                //调用玩家加成处理
                getHeroInfo().getUserdata().getBonusMgr().replaceBonus(preSkillLevel == null ? null : preSkillLevel.union_bonus,
                        newLevelRef == null ? null : newLevelRef.union_bonus);
                //玩家属性
            	getHeroInfo().getComp().getPlayerPropertyContainer().replaceModifier(preSkillLevel == null ? null : preSkillLevel.add_player,
                        newLevelRef == null ? null : newLevelRef.add_player);
            }
        }
    }
}
