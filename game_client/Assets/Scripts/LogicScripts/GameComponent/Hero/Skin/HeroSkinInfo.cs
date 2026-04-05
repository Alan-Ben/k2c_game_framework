using Common.HeroObj;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤数据对象
    /// </summary>
    public class HeroSkinInfo
    {
        //皮肤id
        private long _m_lSkinId;
        //皮肤静态数据
        private HeroSkinRefObj _m_skinRefObj;
        //皮肤等级
        private int _m_iLevel;
        //皮肤当前等级数据
        private HeroSkinLevelRefObj _m_curSkinLevelRef;
        //皮肤下一等级数据
        private HeroSkinLevelRefObj _m_nextSkinLevelRef;

        /// <summary>
        /// 皮肤id
        /// </summary>
        public long skinId { get { return _m_lSkinId; } }
        /// <summary>
        /// 皮肤静态数据
        /// </summary>
        public HeroSkinRefObj skinRefObj { get { return _m_skinRefObj; } }
        /// <summary>
        /// 皮肤等级
        /// </summary>
        public int level { get { return _m_iLevel; } }
        /// <summary>
        /// 皮肤当前等级数据
        /// </summary>
        public HeroSkinLevelRefObj curSkinLevelRef { get { return _m_curSkinLevelRef; } }
        /// <summary>
        /// 皮肤下一等级数据
        /// </summary>
        public HeroSkinLevelRefObj nextSkinLevelRef { get { return _m_nextSkinLevelRef; } }

        public HeroSkinInfo(Hero_SkinInfo _info)
        {
            updateInfo(_info);
        }

        public HeroSkinInfo(long _skinId, int _skinLevel)
        {
            updateInfo(_skinId, _skinLevel);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Hero_SkinInfo _info)
        {
            if(null == _info)
                return;

            _m_lSkinId = _info.getSkinId();
            _m_iLevel = _info.getLevel();
            _m_skinRefObj = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_lSkinId);
            _m_curSkinLevelRef = GRefdataCoreMgr.instance.getHeroSkinLevelRef(_m_lSkinId, _m_iLevel);
            _m_nextSkinLevelRef = GRefdataCoreMgr.instance.getHeroSkinLevelRef(_m_lSkinId, _m_iLevel + 1);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_skinId"></param>
        /// <param name="_skinLevel"></param>
        public void updateInfo(long _skinId, int _skinLevel)
        {
            if (_skinId <= 0 || _skinLevel <= 0)
                return;

            _m_lSkinId = _skinId;
            _m_iLevel = _skinLevel;
            _m_skinRefObj = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_lSkinId);
            _m_curSkinLevelRef = GRefdataCoreMgr.instance.getHeroSkinLevelRef(_m_lSkinId, _m_iLevel);
            _m_nextSkinLevelRef = GRefdataCoreMgr.instance.getHeroSkinLevelRef(_m_lSkinId, _m_iLevel + 1);

            if(_m_skinRefObj == null)
                Debug.LogError($"【伙伴】未获取到皮肤数据，id:{_m_lSkinId}");
            if(_m_curSkinLevelRef == null)
                Debug.LogError($"【伙伴】未获取到皮肤等级数据，id:{_m_lSkinId}，level:{_m_iLevel}");
        }
    }
}