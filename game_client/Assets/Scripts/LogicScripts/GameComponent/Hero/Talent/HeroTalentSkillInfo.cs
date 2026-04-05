using Common.HeroObj;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴资质技能数据对象
    /// </summary>
    public class HeroTalentSkillInfo
    {
        //伙伴id
        private long _m_lHeroId;
        //技能id
        private long _m_lTalentSkillId;
        //资质技能数据
        private HeroTalentSkillRefObj _m_talentSkillRefObj;

        //由于等级数据是分段存储的，所以这里等级单独变量存储
        //等级
        private int _m_iLevel;
        //资质技能当前基础等级数据
        private HeroTalentSkillLevelRefObj _m_talentSkillLevelRefObj;

        /// <summary>
        /// 伙伴id
        /// </summary>
        public long heroId { get { return _m_lHeroId; } }

        /// <summary>
        /// 资质技能id
        /// </summary>
        public long talentSkillId { get { return _m_lTalentSkillId; } }
        /// <summary>
        /// 等级
        /// </summary>
        public int level { get { return _m_iLevel; } }
        /// <summary>
        /// 是否满级了
        /// </summary>
        public bool isMaxLevel { get { return _m_talentSkillRefObj == null || _m_talentSkillRefObj.level_limit <= _m_iLevel; } }
        /// <summary>
        /// 资质技能数据
        /// </summary>
        public HeroTalentSkillRefObj talentSkillRefObj { get { return _m_talentSkillRefObj; } }
        /// <summary>
        /// 资质技能当前等级数据
        /// </summary>
        public HeroTalentSkillLevelRefObj baseTalentSkillLevelRefObj { get { return _m_talentSkillLevelRefObj; } }
        /// <summary>
        /// 获取每级加成的倍率
        /// </summary>
        /// <returns></returns>
        public long levelBonusStack { get { return null == _m_talentSkillLevelRefObj ? _m_iLevel : _m_iLevel - _m_talentSkillLevelRefObj.level; } }

        public HeroTalentSkillInfo(long _heroId, Hero_TalentSkillInfo _info)
        {
            updateInfo(_heroId, _info);
        }
        public HeroTalentSkillInfo(long _heroId, long _talentSkillId, int _level)
        {
            updateInfo(_heroId, _talentSkillId, _level);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_info"></param>
        public void updateInfo(long _heroId, Hero_TalentSkillInfo _info)
        {
            if (_info == null)
                return;

            _m_lHeroId = _heroId;
            _m_lTalentSkillId = _info.getTealentSkillId();
            _m_iLevel = _info.getLevel();
            _m_talentSkillRefObj = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(_m_lTalentSkillId);
            _m_talentSkillLevelRefObj = _m_talentSkillRefObj?.getBaseTalentSkillLevelRefByLevel(_m_iLevel);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_talentSkillId"></param>
        /// <param name="_level"></param>
        public void updateInfo(long _heroId, long _talentSkillId, int _level)
        {
            _m_lHeroId = _heroId;
            _m_lTalentSkillId = _talentSkillId;
            _m_iLevel = _level;
            _m_talentSkillRefObj = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(_m_lTalentSkillId);
            _m_talentSkillLevelRefObj = _m_talentSkillRefObj?.getBaseTalentSkillLevelRefByLevel(_m_iLevel);
        }

        /// <summary>
        /// 是否可升级
        /// </summary>
        /// <returns></returns>
        public bool canUpgrade()
        {
            bool canUpgrade = false;
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lHeroId);
            if (heroRef == null || talentSkillRefObj == null)
                return false;

            //是否可升级（未解锁或条件不通过则不可升级）
            bool conditionEnable = baseTalentSkillLevelRefObj != null &&
                              baseTalentSkillLevelRefObj.cost != null &&
                              baseTalentSkillLevelRefObj.cost.getItemType() != ENPItemType.NONE &&
                              ((baseTalentSkillLevelRefObj.upgrade_condition == null ||
                                baseTalentSkillLevelRefObj.upgrade_condition.isEmpty ||
                                baseTalentSkillLevelRefObj.upgrade_condition.IsEnable(null)) &&
                               (baseTalentSkillLevelRefObj.upgrade_hero_condition == null ||
                                baseTalentSkillLevelRefObj.upgrade_hero_condition.isEmpty ||
                                baseTalentSkillLevelRefObj.upgrade_hero_condition.IsEnable(heroRef, null)));

            //未满级并且可升级，判断道具是否充足
            if (!isMaxLevel && conditionEnable)
            {
                NPCommonCostItem levelUpCostItem = talentSkillRefObj.getUpgradeCostItem(level, false);
                if (levelUpCostItem != null && GCommon.isItemEnough(levelUpCostItem, false))
                    canUpgrade = true;
            }

            return canUpgrade;
        }
    }
}