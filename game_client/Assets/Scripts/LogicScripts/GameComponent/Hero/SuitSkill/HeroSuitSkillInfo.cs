using Common.HeroObj;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能数据对象
    /// </summary>
    public class HeroSuitSkillInfo
    {
        //套系技能id
        private long _m_lSuitSkillId;
        //套系技能数据
        private HeroSuitSkillRefObj _m_lSuitSkillRef;

        //由于等级数据是分段存储的，所以这里等级单独变量存储
        //套系等级
        private long _m_lLevel;
        //套系基础等级数据
        private HeroSuitSkillLevelRefObj _m_baseSuitSkillLevelRef;

        /// <summary>
        /// 套系技能id
        /// </summary>
        public long suitSkillId { get { return _m_lSuitSkillId; } }
        /// <summary>
        /// 套系等级
        /// </summary>
        public long level { get { return _m_lLevel; } }
        /// <summary>
        /// 套系基础等级数据
        /// </summary>
        public HeroSuitSkillLevelRefObj baseSuitSkillLevelRef { get { return _m_baseSuitSkillLevelRef; } }
        /// <summary>
        /// 套系技能数据
        /// </summary>
        public HeroSuitSkillRefObj suitSkillRef { get { return _m_lSuitSkillRef; } }

        /// <summary>
        /// 获取每级加成的倍率
        /// </summary>
        /// <returns></returns>
        public long levelBonusStack { get { return null == _m_baseSuitSkillLevelRef ? _m_lLevel : _m_lLevel - _m_baseSuitSkillLevelRef.level; } }


        public HeroSuitSkillInfo(Hero_SuitSkillInfo _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Hero_SuitSkillInfo _info)
        {
            if (_info == null)
                return;

            _m_lSuitSkillId = _info.getSuitSkillId();
            _m_lSuitSkillRef = GRefdataCoreMgr.instance.heroHaloSuitSkillRefCore.getRef(_m_lSuitSkillId);
            setLevel(_info.getLevel());
        }

        /// <summary>
        /// 设置等级
        /// </summary>
        /// <param name="_level"></param>
        public void setLevel(long _level)
        {
            _m_lLevel = _level;
            _m_baseSuitSkillLevelRef = _m_lSuitSkillRef == null ? null : _m_lSuitSkillRef.getBaseSuitSkillLevelRefByLevel(_m_lLevel);
        }
    }
}