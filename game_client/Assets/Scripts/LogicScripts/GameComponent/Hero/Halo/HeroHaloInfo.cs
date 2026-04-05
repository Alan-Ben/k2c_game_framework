using Common.HeroObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴光环数据对象
    /// </summary>
    public class HeroHaloInfo
    {
        //伙伴id
        private long _m_lHeroId;
        //光环id
        private long _m_lHaloId;
        //光环静态数据
        private HeroHaloRefObj _m_haloRefObj;
        //光环等级
        private int _m_iLevel;
        //是否解锁
        private bool _m_bIsUnlock;
        //光环等级数据
        private HeroHaloLevelRefObj _m_curHaloLevelRef;
        //光环下一等级数据
        private HeroHaloLevelRefObj _m_nextHaloLevelRef;

        /// <summary>
        /// 伙伴id
        /// </summary>
        public long heroId { get { return _m_lHeroId; } }
        /// <summary>
        /// 光环id
        /// </summary>
        public long haloId { get { return _m_lHaloId; } }
        /// <summary>
        /// 光环静态数据
        /// </summary>
        public HeroHaloRefObj haloRefObj { get { return _m_haloRefObj; } }
        /// <summary>
        /// 光环等级
        /// </summary>
        public int level { get { return _m_iLevel; } }
        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool isUnlock { get { return _m_bIsUnlock; } }
        /// <summary>
        /// 光环当前等级数据
        /// </summary>
        public HeroHaloLevelRefObj curHaloLevelRef { get { return _m_curHaloLevelRef; } }
        /// <summary>
        /// 光环下一等级数据
        /// </summary>
        public HeroHaloLevelRefObj nextHaloLevelRef { get { return _m_nextHaloLevelRef; } }

        public HeroHaloInfo(HeroInfo _heroInfo, Hero_HaloInfo _info)
        {
            updateInfo(_heroInfo, _info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_info"></param>
        public void updateInfo(HeroInfo _heroInfo, Hero_HaloInfo _info)
        {
            if(null == _info || _heroInfo == null)
                return;

            //先记录旧数据
            HeroHaloLevelRefObj oldLevelRef = _m_curHaloLevelRef;
            bool oldIsUnlock = _m_bIsUnlock;

            //更新数据
            _m_lHeroId = _info.getHeroId();
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_info.getHeroId());
            _m_lHaloId = heroRef != null ? heroRef.halo_id : 0;
            _m_iLevel = _info.getLevel();
            _m_bIsUnlock = _info.getIsUnlock();
            _m_haloRefObj = GRefdataCoreMgr.instance.heroHaloRefCore.getRef(_m_lHaloId);
            _m_curHaloLevelRef = GRefdataCoreMgr.instance.getHeroHaloLevelRef(_m_lHaloId, _m_iLevel);
            _m_nextHaloLevelRef = GRefdataCoreMgr.instance.getHeroHaloLevelRef(_m_lHaloId, _m_iLevel + 1);

            //等级有变化或者解锁状态变化
            if ((oldLevelRef != _m_curHaloLevelRef || oldIsUnlock != _m_bIsUnlock ) && _m_bIsUnlock)
            {
                //添加伙伴自身属性，如果是从未解锁到解锁，直接添加
                if (oldLevelRef == null || !oldIsUnlock)
                    _heroInfo.addSelfAttrModifier(_m_curHaloLevelRef?.self_attr_prop_modifier);
                else
                    _heroInfo.replaceSelfAttrModifier(oldLevelRef?.self_attr_prop_modifier, _m_curHaloLevelRef?.self_attr_prop_modifier);
            }
        }
    }
}