using Common.HeroObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴经营技能数据对象
    /// </summary>
    public class HeroBusinessSkillInfo
    {
        //伙伴id
        private long _m_lHeroId;
        //经营技能id
        private long _m_lBusinessSkillId;
        //经营技能静态数据
        private HeroBusinessSkillRefObj _m_businessSkillRefObj;
        //经营技能等级
        private int _m_iLevel;

        /// <summary>
        /// 经营技能id
        /// </summary>
        public long businessSkillId { get { return _m_lBusinessSkillId; } }
        /// <summary>
        /// 经营技能静态数据
        /// </summary>
        public HeroBusinessSkillRefObj businessSkillRefObj { get { return _m_businessSkillRefObj; } }
        /// <summary>
        /// 经营技能等级
        /// </summary>
        public int level { get { return _m_iLevel; } }

        public HeroBusinessSkillInfo(long _heroId, Hero_BusinessSkillInfo _info)
        {
            _m_lHeroId = _heroId;
            updateInfo(_info);
        }

        public HeroBusinessSkillInfo(long _heroId, long _skinId, int _skinLevel)
        {
            _m_lHeroId = _heroId;
            updateInfo(_skinId, _skinLevel);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Hero_BusinessSkillInfo _info)
        {
            if(null == _info)
                return;

            _m_lBusinessSkillId = _info.getBusinessSkillId();
            _m_iLevel = _info.getLevel();
            _m_businessSkillRefObj = GRefdataCoreMgr.instance.heroBusinessSkillRefCore.getRef(_m_lBusinessSkillId);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_businessSkillId"></param>
        /// <param name="_skinLevel"></param>
        public void updateInfo(long _businessSkillId, int _skinLevel)
        {
            if (_businessSkillId <= 0 || _skinLevel <= 0)
                return;

            _m_lBusinessSkillId = _businessSkillId;
            _m_iLevel = _skinLevel;
            _m_businessSkillRefObj = GRefdataCoreMgr.instance.heroBusinessSkillRefCore.getRef(_m_lBusinessSkillId);

            if(_m_businessSkillRefObj == null)
                Debug.LogError($"【伙伴】未获取到经营技能数据，id:{_m_lBusinessSkillId}");
        }

        /// <summary>
        /// 获取经营技能属性加成值
        /// </summary>
        /// <param name="_businessBuildingRef"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getBusinessSkillAddPropValue(BusinessBuildingRefObj _businessBuildingRef, EBonusPropertyType _type, NPVarInfo _varInfo = null)
        {
            if (_m_businessSkillRefObj == null)
                return 0;

            if (_m_businessSkillRefObj.limit_range != null &&
                !_m_businessSkillRefObj.limit_range.isEmpty &&
                !_m_businessSkillRefObj.limit_range.IsEnable(_businessBuildingRef, _varInfo))
                return 0;

            long targetValue = 0;
            //添加基础加成
            if (_m_businessSkillRefObj.bonus_prop_modifier != null)
                targetValue = _m_businessSkillRefObj.bonus_prop_modifier.getPropValue(_type);
            //添加每级加成
            if(_m_businessSkillRefObj.bonus_prop_modifier_per_level != null)
                targetValue += ((_m_iLevel - 1) * _m_businessSkillRefObj.bonus_prop_modifier_per_level.getPropValue(_type));

            return targetValue;
        }
    }
}