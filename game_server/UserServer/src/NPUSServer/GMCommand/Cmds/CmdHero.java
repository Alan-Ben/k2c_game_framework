package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Hero.RefHero;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.BusinessSkill.HeroBusinessSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroCalculator;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.Talent.HeroTalentSkillInfo;

import java.util.List;


/**
 * 玩家属性作弊命令
 */
@ACommander(comment = "大臣", name = "hero")
public class CmdHero extends UsCmdBase
{
    @ACommand(comment = "获得大臣[大臣id]")
    public String gain(long _heroId)
    {
        getOwner().gainItem(ENPItemType.HERO, _heroId, getContext());
        return "done";
    }

    @ACommand(comment = "获得所有大臣")
    public String gainAll()
    {
        List<RefHero> list = RefHero.getMgr().getList();
        for (RefHero refHero : list)
        {
            getOwner().gainItem(ENPItemType.HERO, refHero.Id(), getContext());
        }
        return "done";
    }

    @ACommand(comment = "打印所有大臣")
    public String printAll()
    {
        return getOwner().getHeroComponent().toString();
    }

    @ACommand(comment = "打印玩家所有大臣套系数据")
    public String printSuits()
    {
        return getOwner().getHeroComponent().getSuitMgr().toString();
    }

    @ACommand(comment = "打印大臣实力计算过程[大臣id]")
    public String printPowerCal(long _heroId)
    {
        HeroInfo heroInfo = getOwner().getHeroComponent().lookupHero(_heroId);
        if (null == heroInfo)
            return "hero not found!";

        StringBuilder sb = new StringBuilder();
        HeroCalculator.calPower(heroInfo, sb);
        return sb.toString();
    }

    @ACommand(comment = "设置大臣等级[大臣id][等级]")
    public String setLevel(long _heroId, int _level)
    {
        HeroInfo heroInfo = getOwner().getHeroComponent().lookupHero(_heroId);
        if (null == heroInfo)
            return "hero not found!";

        Result result = heroInfo.gmSetLevel(_level, getContext());
        if (!result.isSucc())
            return result.toString();

        return "done";
    }

    @ACommand(comment = "设置大臣阶段[大臣id][阶段]")
    public String setStep(long _heroId, int _step)
    {
        HeroInfo heroInfo = getOwner().getHeroComponent().lookupHero(_heroId);
        if (null == heroInfo)
            return "hero not found!";

        Result result = heroInfo.gmSetStep(_step, getContext());
        if (!result.isSucc())
            return result.toString();

        return "done";
    }

    /**
     * 设置大臣资质技能等级
     *
     * @param _heroId 大臣ID
     * @param _skillId 资质技能ID
     * @param _level 目标等级
     * @return 命令执行结果
     */
    @ACommand(comment = "设置大臣资质技能等级[大臣id][技能id][等级]")
    public String setTalentSkillLevel(long _heroId, long _skillId, int _level)
    {
        HeroInfo heroInfo = getOwner().getHeroComponent().lookupHero(_heroId);
        if (null == heroInfo)
            return "hero not found!";

        // 查找资质技能
        HeroTalentSkillInfo skillInfo = heroInfo.getTalentSkillMgr().lookupSkillInfo(_skillId);
        if (null == skillInfo)
            return "talent skill not found!";

        // 调用GM设置方法
        Result result = skillInfo.gmSetLevel(_level, getContext());
        if (!result.isSucc())
            return result.toString();

        return "done";
    }

    /**
     * 设置大臣经营技能等级
     *
     * @param _heroId 大臣ID
     * @param _skillId 经营技能ID
     * @param _level 目标等级
     * @return 命令执行结果
     */
    @ACommand(comment = "设置大臣经营技能等级[大臣id][技能id][等级]")
    public String setBusinessSkillLevel(long _heroId, long _skillId, int _level)
    {
        HeroInfo heroInfo = getOwner().getHeroComponent().lookupHero(_heroId);
        if (null == heroInfo)
            return "hero not found!";

        // 查找经营技能
        HeroBusinessSkillInfo skillInfo = heroInfo.getBusinessSkillMgr().lookupSkillInfo(_skillId);
        if (null == skillInfo)
            return "business skill not found!";

        // 调用GM设置方法
        Result result = skillInfo.gmSetLevel(_level, getContext());
        if (!result.isSucc())
            return result.toString();

        return "done";
    }

    /**
     * 设置大臣道具加成实力
     *
     * @param _heroId 大臣ID
     * @param _power 目标实力值
     * @return 命令执行结果
     */
    @ACommand(comment = "设置大臣道具加成实力[大臣id][实力值]")
    public String setItemAddPower(long _heroId, long _power)
    {
        HeroInfo heroInfo = getOwner().getHeroComponent().lookupHero(_heroId);
        if (null == heroInfo)
            return "hero not found!";

        // 调用GM设置方法
        heroInfo.gmSetItemAddPower(_power, getContext());

        return "done";
    }

    /**
     * 增减大臣道具加成实力
     *
     * @param _heroId 大臣ID
     * @param _addPower 变化值（正数增加，负数减少）
     * @return 命令执行结果
     */
    @ACommand(comment = "增减大臣道具加成实力[大臣id][变化值(正数增加,负数减少)]")
    public String addItemAddPower(long _heroId, long _addPower)
    {
        HeroInfo heroInfo = getOwner().getHeroComponent().lookupHero(_heroId);
        if (null == heroInfo)
            return "hero not found!";

        // 调用增减方法
        heroInfo.addItemAddPower(_addPower, getContext());

        return "done";
    }
}
