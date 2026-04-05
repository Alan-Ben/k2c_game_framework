using System.Collections.Generic;
using Common.HeroObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴套系数据对象
    /// </summary>
    public class HeroSuitInfo
    {
        //套系id
        private long _m_lSuitId;
        //套系配置数据
        private HeroSuitRefObj _m_suitRef;
        //套系技能列表
        [NotNull] private List<HeroSuitSkillInfo> _m_lSuitSkillInfoList = new List<HeroSuitSkillInfo>();
        //本套系的属性加成信息
        [NotNull] private PlayerAttrPropertyContainer _m_attrContainer = new PlayerAttrPropertyContainer();

        /// <summary>
        /// 套系id
        /// </summary>
        public long suitId { get { return _m_lSuitId; } }
        /// <summary>
        /// 套系配置数据
        /// </summary>
        public HeroSuitRefObj suitRef { get { return _m_suitRef; } }
        /// <summary>
        /// 套系技能列表
        /// </summary>
        public List<HeroSuitSkillInfo> suitSkillInfoList { get { return _m_lSuitSkillInfoList; } }

        /// <summary>
        /// 本套系的属性加成信息
        /// </summary>
        /// <returns></returns>
        public PlayerAttrPropertyContainer attrContainer { get { return _m_attrContainer; } }

        /// <summary>
        /// 该套系总伙伴数量
        /// </summary>
        public long suitTotalHeroCount { get { return _m_suitRef != null ? _m_suitRef.heroRefList.Count : 0; } }
        /// <summary>
        /// 该套系已拥有伙伴数量
        /// </summary>
        public long suitOwnHeroCount
        {
            get
            {
                long ownCount = 0;
                if (_m_suitRef != null)
                {
                    for (int i = 0; i < _m_suitRef.heroRefList.Count; i++)
                    {
                        if (_m_suitRef.heroRefList[i] != null && NPPlayer.instance.heroComponent.isHeroUnlock(_m_suitRef.heroRefList[i].id))
                            ownCount++;
                    }
                }
                return ownCount;
            }
        }

        public HeroSuitInfo(Hero_SuitInfo _info)
        {
            _initSuitInfo(_info);
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        /// <param name="_info"></param>
        private void _initSuitInfo(Hero_SuitInfo _info)
        {
            if (_info == null)
                return;

            _m_lSuitId = _info.getSuitId();
            _m_suitRef = GRefdataCoreMgr.instance.heroHaloSuitRefCore.getRef(_m_lSuitId);
            if (_info.getSuitSkillList() != null)
            {
                _m_lSuitSkillInfoList.Clear();
                for (int i = 0; i < _info.getSuitSkillList().Count; i++)
                {
                    if (_info.getSuitSkillList()[i] != null)
                    {
                        HeroSuitSkillInfo suitSkillInfo = new HeroSuitSkillInfo(_info.getSuitSkillList()[i]);
                        _m_lSuitSkillInfoList.Add(suitSkillInfo);

                        //初始化属性加成容器
                        if (suitSkillInfo.baseSuitSkillLevelRef != null)
                        {
                            _m_attrContainer.addModifier(suitSkillInfo.baseSuitSkillLevelRef.attr_prop_modifier);
                            _m_attrContainer.addModifier(suitSkillInfo.baseSuitSkillLevelRef.attr_prop_modifier_per_lvl, suitSkillInfo.levelBonusStack);

                            _reCalcHero();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Hero_SuitInfo _info)
        {
            if (_info == null || _info.getSuitId() != _m_lSuitId)
                return;

            for (int i = 0; i < _info.getSuitSkillList().Count; i++)
            {
                Hero_SuitSkillInfo tempInfo = _info.getSuitSkillList()[i];
                if(tempInfo == null)
                    continue;

                HeroSuitSkillInfo skillInfo = getSuitSkillInfo(tempInfo.getSuitSkillId());

                //获取旧等级数据
                HeroSuitSkillLevelRefObj preSkillLevel = skillInfo == null ? null : skillInfo.baseSuitSkillLevelRef;
                long preStack = skillInfo == null ? 0 : skillInfo.levelBonusStack;

                if (skillInfo == null)
                {
                    skillInfo = new HeroSuitSkillInfo(tempInfo);
                    _m_lSuitSkillInfoList.Add(skillInfo);
                }

                //设置新等级
                skillInfo.setLevel(tempInfo.getLevel());

                //计算属性调整
                HeroSuitSkillLevelRefObj newLevelRef = skillInfo.baseSuitSkillLevelRef;
                //调用本对象属性加成处理，分为加成倍率属性和基础属性
                _m_attrContainer.replaceModifier(null == preSkillLevel ? null : preSkillLevel.attr_prop_modifier_per_lvl, preStack
                    , null == newLevelRef ? null : newLevelRef.attr_prop_modifier_per_lvl, skillInfo.levelBonusStack);
                //等级区间不同才对基础属性进行额外处理
                if (preSkillLevel != newLevelRef)
                    _m_attrContainer.replaceModifier(null == preSkillLevel ? null : preSkillLevel.attr_prop_modifier, null == newLevelRef ? null : newLevelRef.attr_prop_modifier);

                _reCalcHero();
            }
        }

        /// <summary>
        /// 根据套系技能id获取套系技能信息
        /// </summary>
        /// <param name="_suitSkillId"></param>
        /// <returns></returns>
        public HeroSuitSkillInfo getSuitSkillInfo(long _suitSkillId)
        {
            for (int i = 0; i < _m_lSuitSkillInfoList.Count; i++)
            {
                if (_m_lSuitSkillInfoList[i] != null && _m_lSuitSkillInfoList[i].suitSkillId == _suitSkillId)
                    return _m_lSuitSkillInfoList[i];
            }

            return null;
        }

        /// <summary>
        /// 刷新伙伴的属性
        /// </summary>
        private void _reCalcHero()
        {
            if (_m_suitRef == null || _m_suitRef.heroRefList.Count == 0)
                return;

            for (int i = 0; i < _m_suitRef.heroRefList.Count; i++)
            {
                HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_suitRef.heroRefList[i].id);
                if(heroInfo != null)
                    heroInfo.reCalcHero();
            }
        }
    }
}