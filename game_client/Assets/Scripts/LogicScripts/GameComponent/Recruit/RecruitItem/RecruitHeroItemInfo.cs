using NPEnum;

namespace GOE
{
    public class RecruitHeroItemInfo : _ARecruitItemInfo
    {
        /// <summary>
        /// 大臣id
        /// </summary>
        private long _m_lHeroId;
        /// <summary>
        /// 大臣显示数据
        /// </summary>
        private HeroCardShowInfo _m_heroCardShowInfo;
        
        public RecruitHeroItemInfo(RecruitRefObj _recruitRefObj) : base(_recruitRefObj)
        {
            if (_recruitRefObj == null || _recruitRefObj.gain_item == null ||
                _recruitRefObj.gain_item.getItemType() != ENPItemType.HERO)
            {
                Debug.LogError($"[RecruitHeroItemInfo ctor] _recruitRefObj.gain_item.getItemType():{_recruitRefObj?.gain_item?.getItemType()} != ENPItemType.HERO");
                return;
            }

            _m_lHeroId = _recruitRefObj.gain_item.subId;
            _refreshHeroShowInfo();
        }

        public override ERecruitItemType recruitItemType { get { return ERecruitItemType.HERO; } }

        /// <summary>
        /// 不开放外部调用, 只在内部使用, , 外部调用getHeroShowInfo
        /// </summary>
        private HeroCardShowInfo heroShowInfo
        {
            get
            {
                _refreshHeroShowInfo();
                return _m_heroCardShowInfo;
            }
        }
        
        public override bool isHadRecruit()
        {
            return NPPlayer.instance.heroComponent.getHeroInfo(_m_lHeroId) != null;
        }
        
        public HeroCardShowInfo getHeroShowInfo(bool _needRefreshData)
        {
            if(_needRefreshData)
                _refreshHeroShowInfo();

            return _m_heroCardShowInfo;
        }
        
        /// <summary>
        /// 刷新大臣展示信息
        /// </summary>
        private void _refreshHeroShowInfo()
        {
            if(_m_heroCardShowInfo == null)
                _m_heroCardShowInfo = new HeroCardShowInfo(null, GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lHeroId));
            
            _m_heroCardShowInfo.updateSelfHeroInfo();
        }
    }
}