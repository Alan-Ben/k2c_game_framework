package NPUSServer.NPUSUserMgr.UserComp.HeroComp.Talent;

import Common.HeroObj.Hero_TalentSkillInfo;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;
import NPGameRes.Refs.Hero.RefHeroTalentSkill;
import NPGameRes.Refs.Hero.RefHeroTalentSkillLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.Skin.HeroSkinInfo;
import USDB.Bo.PlayerHeroTalentSkillBO;

import java.util.ArrayList;
import java.util.List;

/************
 * 大臣资质技能总管理器
 */
public class HeroTalentSkillMgr
{
    //大臣对象
    private HeroInfo _m_heroInfo;
    //技能列表
    private List<HeroTalentSkillInfo> _m_skillList;

    public HeroTalentSkillMgr(HeroInfo _heroInfo)
    {
        _m_heroInfo = _heroInfo;
        _m_skillList = new ArrayList<>();

        //初始化数据
        _initSkillInfo();
    }

    /**************
     * 带入大臣之后将根据大臣数据直接进行相关技能数据的初始化
     * 此初始化为单纯内存的初始化
     */
    private void _initSkillInfo()
    {
        //数据非法判断
        if (null == _m_heroInfo || null == _m_heroInfo.getRef())
            return;

        //初始化基础光环技能数据
        for (Long talentSkillId : _m_heroInfo.getRef().default_talent_skill_id_list)
        {
            //如果已经存在对应技能则不添加
            if (null != lookupSkillInfo(talentSkillId))
                continue;

            //逐个初始化经营技能内存数据
            RefHeroTalentSkill refSkill = RefHeroTalentSkill.getMgr().get(talentSkillId);
            //数据非法判断
            if (null == refSkill)
            {
                CommLog.error("HeroTalentSkillMgr _initSkillInfo refSkill not found, skillId:{}", talentSkillId);
                continue;
            }

            //初始化技能数据
            HeroTalentSkillInfo skillInfo = new HeroTalentSkillInfo(this, refSkill);
            _m_skillList.add(skillInfo);

            //如果是自动升级的技能，则用阶段初始化等级
            if (getHeroInfo().getRef().auto_upgrade_talent_skill_id_list.contains(talentSkillId))
                skillInfo._setLevelByStep(_m_heroInfo.getStep(), false);

            //应用属性
            _applySkillLevelProperty(skillInfo.getLevelRef(), skillInfo.getLevelBonusStack());
        }

        //初始化可能的额外解锁数据
        for (WCGPairLong talentSkillPair : _m_heroInfo.getRef().extra_talent_skill_id_list)
        {
            //第一个数据为星级，星级超过则解锁，否则不解锁
            if (talentSkillPair.first() > _m_heroInfo.getStar())
                continue;

            //如果已经存在对应技能则不添加
            if (null != lookupSkillInfo(talentSkillPair.second()))
                continue;

            //添加基础技能数据
            //逐个初始化经营技能内存数据
            RefHeroTalentSkill refSkill = RefHeroTalentSkill.getMgr().get(talentSkillPair.second());
            //数据非法判断
            if (null == refSkill)
            {
                CommLog.error("HeroTalentSkillMgr _initExtraSkillInfo refSkill not found, skillId:{}", talentSkillPair.second());
                continue;
            }

            //添加到数据集
            HeroTalentSkillInfo skillInfo = new HeroTalentSkillInfo(this, refSkill);
            _m_skillList.add(skillInfo);

            //应用属性
            _applySkillLevelProperty(skillInfo.getLevelRef(), skillInfo.getLevelBonusStack());
        }
    }

    /**
     * 应用技能等级属性
     * @param levelRef   技能等级配置
     * @param bonusStack 等级加成堆叠数
     */
    private void _applySkillLevelProperty(RefHeroTalentSkillLevel levelRef, int bonusStack)
    {
        if (levelRef == null)
            return;

        // 添加基础属性
        getHeroInfo().getPropertyContainer().addModifier(levelRef.self_attr_prop_modifier);
        getHeroInfo().getUserdata().getBonusMgr().addBonus(levelRef.union_bonus);

        // 添加等级加成
        if (bonusStack > 0)
        {
            getHeroInfo().getPropertyContainer().addModifier(levelRef.self_attr_prop_modifier_per_level, bonusStack);
            getHeroInfo().getUserdata().getBonusMgr().addBonus(levelRef.union_bonus_per_level, bonusStack);
        }
    }

    /**
     * 处理技能等级属性变更
     * @param _oldLevelRef 旧等级配置
     * @param _newLevelRef 新等级配置
     * @param _oldBonusStack 旧等级加成堆叠数
     * @param _newBonusStack 新等级加成堆叠数
     */
    private void _replaceSkillLevelProperty(RefHeroTalentSkillLevel _oldLevelRef, RefHeroTalentSkillLevel _newLevelRef, int _oldBonusStack, int _newBonusStack)
    {
        // 1. 处理基础自身属性修饰符变化
        if (_oldLevelRef != _newLevelRef)
        {
            // 英雄自身属性修饰符
            PlayerAttrPropertyModifier oldSelfModifier = _oldLevelRef != null ? _oldLevelRef.self_attr_prop_modifier : null;
            PlayerAttrPropertyModifier newSelfModifier = _newLevelRef != null ? _newLevelRef.self_attr_prop_modifier : null;
            getHeroInfo().getPropertyContainer().replaceModifier(oldSelfModifier, newSelfModifier);

            // 联合加成
            UnionBonus oldUnionBonus = _oldLevelRef != null ? _oldLevelRef.union_bonus : null;
            UnionBonus newUnionBonus = _newLevelRef != null ? _newLevelRef.union_bonus : null;
            getHeroInfo().getUserdata().getBonusMgr().replaceBonus(oldUnionBonus, newUnionBonus);
        }

        // 2. 处理每级自身属性修饰符变化
        PlayerAttrPropertyModifier oldSelfLevelModifier = _oldLevelRef != null ? _oldLevelRef.self_attr_prop_modifier_per_level : null;
        PlayerAttrPropertyModifier newSelfLevelModifier = _newLevelRef != null ? _newLevelRef.self_attr_prop_modifier_per_level : null;
        getHeroInfo().getPropertyContainer().replaceModifier(oldSelfLevelModifier, _oldBonusStack,
                newSelfLevelModifier, _newBonusStack);

        // 3. 处理每级联合加成变化
        UnionBonus oldUnionLevelBonus = _oldLevelRef != null ? _oldLevelRef.union_bonus_per_level : null;
        UnionBonus newUnionLevelBonus = _newLevelRef != null ? _newLevelRef.union_bonus_per_level : null;
        getHeroInfo().getUserdata().getBonusMgr().replaceBonus(oldUnionLevelBonus, _oldBonusStack,
                newUnionLevelBonus, _newBonusStack);
    }

    public HeroInfo getHeroInfo()
    {
        return _m_heroInfo;
    }

    /**
     * 数据库初始化技能数据
     */
    public void initCheckSkinSKill()
    {
        //根据皮肤数据进行初始化，将皮肤加成的资质技能添加到数据中
        for (HeroSkinInfo skinInfo : _m_heroInfo.getSkinMgr().getSkinList())
        {
            for (Long talentSkillId : skinInfo.getRef().unlock_gain_talent_skill_id_list)
            {
                //如果已经存在对应技能则不添加
                if (null != lookupSkillInfo(talentSkillId))
                    continue;

                //逐个初始化经营技能内存数据
                RefHeroTalentSkill refSkill = RefHeroTalentSkill.getMgr().get(talentSkillId);
                //数据非法判断
                if (null == refSkill)
                {
                    CommLog.error("HeroTalentSkillMgr _initSkillInfo refSkill from skin not found, skillId:{}", talentSkillId);
                    continue;
                }

                //初始化技能数据
                HeroTalentSkillInfo skillInfo = new HeroTalentSkillInfo(this, refSkill);
                _m_skillList.add(skillInfo);

                //应用属性
                _applySkillLevelProperty(skillInfo.getLevelRef(), skillInfo.getLevelBonusStack());
            }
        }
    }

    /**
     * 数据库初始化技能数据
     * @param _skillBo 数据bo
     */
    public void initTalentSkill(PlayerHeroTalentSkillBO _skillBo)
    {
        //从已有数据检查是否有匹配数据
        HeroTalentSkillInfo skillInfo = lookupSkillInfo(_skillBo.getTalentSkillId());
        //无数据则报错
        if (null == skillInfo)
        {
            CommLog.error("HeroTalentSkillMgr initSkill skillInfo not found, skillId:{}", _skillBo.getTalentSkillId());
            return;
        }

        //获取原等级信息
        RefHeroTalentSkillLevel preLevelRef = skillInfo.getLevelRef();
        //获取原stack
        int preBonusStack = skillInfo.getLevelBonusStack();

        //初始化Bo数据
        skillInfo._initBo(_skillBo);

        //处理属性变化
        _replaceSkillLevelProperty(preLevelRef, skillInfo.getLevelRef(), preBonusStack, skillInfo.getLevelBonusStack());
    }

    /**
     * 查找技能信息
     * @param _skillId 技能id
     * @return 技能对象
     */
    public HeroTalentSkillInfo lookupSkillInfo(long _skillId)
    {
        _m_heroInfo.lock();
        try
        {
            for (HeroTalentSkillInfo skillInfo : _m_skillList)
            {
                if (skillInfo.getTalentSkillId() == _skillId)
                    return skillInfo;
            }
            return null;
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 检查解锁额外技能
     * 需要在以下场景调用：
     * 1、大臣星级变化
     */
    public void checkUnlockExtraSkill(NPPlayerContext _context)
    {
        int star = _m_heroInfo.getStar();

        List<WCGPairLong> starSkillPairList = _m_heroInfo.getRef().extra_talent_skill_id_list;
        //遍历检查技能解锁
        for (WCGPairLong starSkillPair : starSkillPairList)
        {
            //检查星级是否达到要求
            if (starSkillPair.first() > star)
                continue;

            //检查是否有对应数据，如无则添加
            HeroTalentSkillInfo skillInfo = lookupSkillInfo(starSkillPair.second());
            if (null != skillInfo)
                continue;

            _unlockTalentSkill(starSkillPair.second(), _context);
        }
    }

    /**
     * 解锁资质技能
     * @param _skillId 技能id
     * @param _context 上下文
     */
    private HeroTalentSkillInfo _unlockTalentSkill(long _skillId, NPPlayerContext _context)
    {
        _m_heroInfo.lock();
        try
        {
            //查找是否已经解锁
            if (lookupSkillInfo(_skillId) != null)
                return null;

            //查找资质技能配置
            RefHeroTalentSkill refSkill = RefHeroTalentSkill.getMgr().get(_skillId);
            if (refSkill == null)
                return null;

            //添加技能
            HeroTalentSkillInfo skillInfo = new HeroTalentSkillInfo(this, refSkill);
            _m_skillList.add(skillInfo);

            //应用属性
            _applySkillLevelProperty(skillInfo.getLevelRef(), skillInfo.getLevelBonusStack());

            //调用变化处理函数
            skillInfo.onSkillChg();

            return skillInfo;
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 使用道具升级
     * @param _skillId 技能id
     * @param _isTen
     * @param _context 上下文
     * @return 结果
     */
    public Result upgrade(long _skillId, boolean _isTen, NPPlayerContext _context)
    {
        _m_heroInfo.lock();
        try
        {
            HeroTalentSkillInfo skillInfo = lookupSkillInfo(_skillId);
            if (skillInfo == null)
                return HeroErr.HERO_TALENT_SKILL_NOT_EXIST;

            //记录原先等级信息
            RefHeroTalentSkillLevel preLevelRef = skillInfo.getLevelRef();
            //获取原stack
            int preBonusStack = skillInfo.getLevelBonusStack();

            //处理升级
            Result res = skillInfo.upgradeUseItem(_isTen, _context);
            if (!res.isSucc())
                return res;

            //处理属性变化
            _replaceSkillLevelProperty(preLevelRef, skillInfo.getLevelRef(), preBonusStack, skillInfo.getLevelBonusStack());

            //返回结果
            return res;
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 填充资质技能信息
     * @param _skillList 技能信息列表
     */
    public void fillTalentSkillProto(ArrayList<Hero_TalentSkillInfo> _skillList)
    {
        _m_heroInfo.lock();
        try
        {
            for (HeroTalentSkillInfo skillInfo : _m_skillList)
            {
                _skillList.add(skillInfo.toProto());
            }
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 解锁技能,当需要解锁技能的时候通过本函数尝试添加新技能
     * 目前在皮肤解锁的时候会调用
     * @param _unlockGainTalentSkillIdList
     * @param _context
     */
    public void unlockSkill(List<Long> _unlockGainTalentSkillIdList, NPPlayerContext _context)
    {
        _m_heroInfo.lock();
        try
        {
            for (Long skillId : _unlockGainTalentSkillIdList)
            {
                _unlockTalentSkill(skillId, _context);
            }
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 大臣阶段变化
     * 提升升阶自动升级技能等级
     */
    public void onStepChg()
    {
        //遍历检查技能解锁
        for (Long talentSkillId : _m_heroInfo.getRef().auto_upgrade_talent_skill_id_list)
        {
            //检查是否有对应数据，如无则添加
            HeroTalentSkillInfo skillInfo = lookupSkillInfo(talentSkillId);
            if (null == skillInfo)
                continue;

            //记录原先等级信息
            RefHeroTalentSkillLevel preLevelRef = skillInfo.getLevelRef();
            //获取原stack
            int preBonusStack = skillInfo.getLevelBonusStack();

            skillInfo._setLevelByStep(_m_heroInfo.getStep(), true);

            //处理属性变化
            _replaceSkillLevelProperty(preLevelRef, skillInfo.getLevelRef(), preBonusStack, skillInfo.getLevelBonusStack());
        }
    }

    @Override
    public String toString()
    {
        StringBuilder defaultSb = new StringBuilder();
        StringBuilder extraSb = new StringBuilder();
        defaultSb.append("初始资质技能列表:").append("\n");
        extraSb.append("额外资质技能列表:").append("\n");
        _m_heroInfo.lock();
        try
        {
            for (HeroTalentSkillInfo skillInfo : _m_skillList)
            {
                if (_m_heroInfo.getRef().isDefaultTalentSkill(skillInfo.getTalentSkillId()))
                {
                    defaultSb.append(skillInfo.getTalentSkillId()).append(":").append(skillInfo.getTalentSkillLevel()).append("\n");
                } else
                {
                    extraSb.append(skillInfo.getTalentSkillId()).append(":").append(skillInfo.getTalentSkillLevel()).append("\n");
                }
            }
        } finally
        {
            _m_heroInfo.unlock();
        }
        return defaultSb.append(extraSb).toString();
    }
}
