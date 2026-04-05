
namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能数据对象
    /// </summary>
    public class HeroStarSkillInfo
    {
        //伙伴id
        private long _m_lHeroId;
        //觉醒技能id
        private long _m_lStarSkillId;
        //等级
        private long _m_lLevel;
        //觉醒技能数据
        private HeroStarSkillRefObj _m_starSkillRefObj;
        //资质技能当前等级数据
        private HeroStarSkillLevelRefObj _m_curStarSkillLevelRefObj;
        //资质技能下一等级数据
        private HeroStarSkillLevelRefObj _m_nextStarSkillLevelRefObj;

        /// <summary>
        /// 伙伴id
        /// </summary>
        public long heroId { get { return _m_lHeroId; } }
        /// <summary>
        /// 觉醒技能id
        /// </summary>
        public long starSkillId { get { return _m_lStarSkillId; } }
        /// <summary>
        /// 等级
        /// </summary>
        public long level { get { return _m_lLevel; } }
        /// <summary>
        /// 觉醒技能数据
        /// </summary>
        public HeroStarSkillRefObj starSkillRefObj { get { return _m_starSkillRefObj; } }
        /// <summary>
        /// 资质技能当前等级数据
        /// </summary>
        public HeroStarSkillLevelRefObj curStarSkillLevelRefObj { get { return _m_curStarSkillLevelRefObj; } }
        /// <summary>
        /// 资质技能下一等级数据
        /// </summary>
        public HeroStarSkillLevelRefObj nextStarSkillLevelRefObj { get { return _m_nextStarSkillLevelRefObj; } }


        public HeroStarSkillInfo(long _heroId, long _starSkillId, long _star)
        {
            updateInfo(_heroId, _starSkillId, _star);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_info"></param>
        public void updateInfo(long _heroId, long _starSkillId, long _star)
        {
            _m_lHeroId = _heroId;
            _m_lStarSkillId = _starSkillId;
            _m_lLevel = _star + 1;
            _m_starSkillRefObj = GRefdataCoreMgr.instance.heroStarSkillRefCore.getRef(_starSkillId);
            if (_m_starSkillRefObj != null && _m_starSkillRefObj.maxLevel < _m_lLevel)
                _m_lLevel = _m_starSkillRefObj.maxLevel;
            _m_curStarSkillLevelRefObj = GRefdataCoreMgr.instance.getHeroStarSkillLevelRef(_starSkillId, _m_lLevel);
            _m_nextStarSkillLevelRefObj = GRefdataCoreMgr.instance.getHeroStarSkillLevelRef(_starSkillId, _m_lLevel + 1);
        }

        /// <summary>
        /// 更新星级
        /// </summary>
        /// <param name="_star"></param>
        public void updateStar(long _star)
        {
            _m_lLevel = _star + 1;
            if (_m_starSkillRefObj != null && _m_starSkillRefObj.maxLevel < _m_lLevel)
                _m_lLevel = _m_starSkillRefObj.maxLevel;
            _m_curStarSkillLevelRefObj = GRefdataCoreMgr.instance.getHeroStarSkillLevelRef(_m_lStarSkillId, _m_lLevel);
            _m_nextStarSkillLevelRefObj = GRefdataCoreMgr.instance.getHeroStarSkillLevelRef(_m_lStarSkillId, _m_lLevel + 1);
        }
    }
}