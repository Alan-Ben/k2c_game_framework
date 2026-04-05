package NPUSServer.NPUSUserMgr.UserComp.HeroComp.SuitMgr;

import Common.HeroObj.Hero_SuitInfo;
import GS2GC.p013_HeroOp.GS2GC_013_060_OnHeroSuitChg;
import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyContainer;
import NPGameRes.Refs.Hero.RefHeroSuit;
import NPGameRes.Refs.Hero.RefHeroSuitSkill;
import NPGameRes.Refs.Hero.RefHeroSuitSkillLevel;
import NPUSServer.NPUSUserMgr.UserComp.Common.HeroSuitPropertyChgDealer;

import java.util.ArrayList;
import java.util.List;

/**********
 * 大臣套件信息
 */
public class HeroSuitInfo
{
    private HeroSuitsMgr _m_mgr;

    private RefHeroSuit _m_suitRef;
    //技能列表
    private List<HeroSuitSkillInfo> _m_lSkillList;

    //本套系的属性加成信息
    private PlayerAttrPropertyContainer _m_attrContainer;

    public HeroSuitInfo(HeroSuitsMgr _mgr, RefHeroSuit _ref)
    {
        _m_mgr = _mgr;

        _m_suitRef = _ref;
        _m_lSkillList = new ArrayList<>();
        _m_attrContainer = new PlayerAttrPropertyContainer();
    }

    public HeroSuitsMgr getSuitMgr()
    {
        return _m_mgr;
    }

    public RefHeroSuit getSuitRef()
    {
        return _m_suitRef;
    }

    public PlayerAttrPropertyContainer getAttrContainer()
    {
        return _m_attrContainer;
    }

    /**
     * 查询星级技能对象
     * @param _skillId
     * @return
     */
    public HeroSuitSkillInfo lookupSuitSkill(long _skillId)
    {
        for (HeroSuitSkillInfo starSkillInfo : _m_lSkillList)
        {
            if (_skillId == starSkillInfo.getStarSkillId())
                return starSkillInfo;
        }
        return null;
    }

    /**************
     * 运行过程中移除技能
     */
    public void removeSkillLevel(long _suitSkillId, int _level)
    {
        //获取已有数据
        HeroSuitSkillInfo skillInfo = lookupSuitSkill(_suitSkillId);
        if (null == skillInfo)
        {
            CommLog.error("HeroSuitSkillInfo suitSkillInfo not found for remove skill level, suitSkillId:{}", _suitSkillId);
            return;
        }

        //获取旧等级数据
        RefHeroSuitSkillLevel preSkillLevel = skillInfo.getSkillLevelRef();
        long preStack = skillInfo.getLevelBonusStack();

        //针对技能去除等级
        skillInfo.setLevel(skillInfo.getSkillLevel() - _level);

        //计算属性调整
        _replaceSkillLevelProperty(preSkillLevel, preStack, skillInfo.getSkillLevelRef(), skillInfo.getLevelBonusStack());
    }

    /**
     * 替换技能等级属性
     * @param _preLevelRef
     * @param _preStack
     * @param _newLevelRef
     * @param _newLevelStack
     */
    private void _replaceSkillLevelProperty(RefHeroSuitSkillLevel _preLevelRef, long _preStack, RefHeroSuitSkillLevel _newLevelRef, int _newLevelStack)
    {
        //等级区间不同才对基础属性进行额外处理
        if (_preLevelRef != _newLevelRef)
            _m_attrContainer.replaceModifier(null == _preLevelRef ? null : _preLevelRef.attr_prop_modifier
                    , null == _newLevelRef ? null : _newLevelRef.attr_prop_modifier);

        //调用本对象属性加成处理，分为加成倍率属性和基础属性
        _m_attrContainer.replaceModifier(null == _preLevelRef ? null : _preLevelRef.attr_prop_modifier_per_lvl, _preStack
                , null == _newLevelRef ? null : _newLevelRef.attr_prop_modifier_per_lvl, _newLevelStack);
    }

    /**************
     * 运行过程中添加技能
     */
    public void addSkillLevel(long _suitSkillId, int _level)
    {
        //获取已有数据
        HeroSuitSkillInfo skillInfo = lookupSuitSkill(_suitSkillId);
        if (null == skillInfo)
        {
            //获取技能数据
            RefHeroSuitSkill suitSkillRef = RefHeroSuitSkill.getMgr().get(_suitSkillId);
            if (null == suitSkillRef)
            {
                CommLog.error("HeroSuitSkillInfo suitSkillRef not found, suitSkillId:{}", _suitSkillId);
                return;
            }

            //如无数据则创建数据放入
            skillInfo = new HeroSuitSkillInfo(this, suitSkillRef);
            //放入队列
            _m_lSkillList.add(skillInfo);
        }

        //获取旧等级数据
        RefHeroSuitSkillLevel preSkillLevel = skillInfo.getSkillLevelRef();
        long preStack = skillInfo.getLevelBonusStack();

        //针对技能叠加等级
        skillInfo.setLevel(skillInfo.getSkillLevel() + _level);

        //计算属性调整
        _replaceSkillLevelProperty(preSkillLevel, preStack, skillInfo.getSkillLevelRef(), skillInfo.getLevelBonusStack());
    }

    /************
     * 初始化属性变化的处理对象
     */
    public void _initSuitPropertyChgDealer()
    {
        _m_attrContainer.setOnPropertyChg(new HeroSuitPropertyChgDealer(this));
    }

    /**
     * 打印套系调试信息（含套系内大臣ID列表及技能等级）
     */
    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("  suitId=").append(_m_suitRef.id)
                .append(", heroCount=").append(_m_suitRef.getHeroList().size())
                .append(", skillCount=").append(_m_lSkillList.size()).append("\n");
        // 打印套系内所有大臣的heroId
        sb.append("    heroIds=");
        for (NPGameRes.Refs.Hero.RefHero heroRef : _m_suitRef.getHeroList())
        {
            sb.append(heroRef.id).append(" ");
        }
        sb.append("\n");
        // 打印各技能等级
        for (HeroSuitSkillInfo skillInfo : _m_lSkillList)
        {
            sb.append(skillInfo.toString()).append("\n");
        }
        return sb.toString();
    }

    /**
     * 构造协议
     * @return
     */
    public Hero_SuitInfo toProto()
    {
        Hero_SuitInfo proto = new Hero_SuitInfo();
        proto.setSuitId(_m_suitRef.id);
        for (HeroSuitSkillInfo heroSuitSkillInfo : _m_lSkillList)
        {
            proto.getSuitSkillList().add(heroSuitSkillInfo.toProto());
        }
        return proto;
    }

    /**
     * 套系属性变化
     */
    public void onSuitChg()
    {
        GS2GC_013_060_OnHeroSuitChg proto = new GS2GC_013_060_OnHeroSuitChg();
        proto.setSuitInfo(toProto());
        _m_mgr.getComp().getUserData().sendMsgToGC(proto);
    }
}
