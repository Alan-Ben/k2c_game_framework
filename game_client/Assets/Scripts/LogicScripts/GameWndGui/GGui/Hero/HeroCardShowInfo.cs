
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴卡牌展示接口
    /// </summary>
    public class HeroCardShowInfo : _IHeroCardShow
    {        
        //已获得的伙伴信息
        private HeroInfo _m_heroInfo;
        //伙伴配置信息
        private HeroRefObj _m_heroRef;

        public HeroCardShowInfo(HeroInfo _heroInfo, HeroRefObj _heroRef)
        {
            _m_heroInfo = _heroInfo;
            _m_heroRef = _heroRef;
        }

        public long id { get { return _m_heroRef != null ? _m_heroRef.id : 0; } }
        /// <summary>
        /// 伙伴配表数据
        /// </summary>
        public HeroRefObj heroRefObj { get { return _m_heroRef; } }
        /// <summary>
        /// 伙伴信息
        /// </summary>
        public HeroInfo heroInfo { get { return _m_heroInfo; } }
        /// <summary>
        /// 伙伴阶段数据
        /// </summary>
        public HeroStepRefObj curHeroStepRef { get { return _m_heroInfo != null ? _m_heroInfo.curHeroStepRef : GRefdataCoreMgr.instance.heroStepRefCore.getRef(1); } }
        /// <summary>
        /// 获取等级
        /// </summary>
        public long level { get { return _m_heroInfo != null ? _m_heroInfo.level : 0; } }
        /// <summary>
        /// 获取实力
        /// </summary>
        public long power { get { return _m_heroInfo != null ? _m_heroInfo.power : 0; } }
        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool isUnlock { get { return _m_heroInfo != null; } }
        /// <summary>
        /// 获取星级
        /// </summary>
        public long star { get { return _m_heroInfo != null ? _m_heroInfo.star : 0; } }
        
        /// <summary>
        /// 更新自身大臣信息
        /// </summary>
        public void updateSelfHeroInfo()
        {
            _m_heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(id);
        }

        public void updateHeroInfo(HeroInfo _heroInfo)
        {
            _m_heroInfo = _heroInfo;
        }
        /// <summary>
        /// 获取总资质
        /// </summary>
        /// <returns></returns>
        public long getTotalTalent()
        {
            if (_m_heroInfo != null)
                return HeroCommon.calTalent(_m_heroInfo);

            if (_m_heroRef != null)
                return HeroCommon.calInitTalent(_m_heroRef.id);

            return 0;
        }
        /// <summary>
        /// 显示头像
        /// </summary>
        /// <returns></returns>
        public NPGTextureIndex getIcon()
        {
            if (_m_heroInfo != null)
                return _m_heroInfo.getIcon();

            if (_m_heroRef != null)
                return GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, _m_heroRef.default_skin_id);

            return null;
        }
        /// <summary>
        /// 显示卡牌半身像
        /// </summary>
        /// <returns></returns>
        public NPGTextureIndex getCardImage()
        {
            if (_m_heroInfo != null)
                return _m_heroInfo.getCardImage();

            if (_m_heroRef != null)
            {
                HeroSkinRefObj skinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_heroRef.default_skin_id);
                if (skinRef != null)
                    return skinRef.card_image;
            }

            return null;
        }
        /// <summary>
        /// 显示卡牌背景图
        /// </summary>
        /// <returns></returns>
        public NPGTextureIndex getCardBg()
        {
            if (_m_heroRef != null)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroRef.id);
                return qualityExtRef?.hero_card_bg;
            }
            return null;
        }
        /// <summary>
        /// 显示全身形象
        /// </summary>
        /// <returns></returns>
        public NPGGoIndex getTdShow()
        {
            if (_m_heroInfo != null)
                return _m_heroInfo.getTdShow();

            if (_m_heroRef != null)
            {
                HeroSkinRefObj heroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_heroRef.default_skin_id);
                if (heroSkinRef != null)
                    return heroSkinRef.td_show;
            }

            return null;
        }

        /// <summary>
        /// 获取形象背景
        /// </summary>
        /// <returns></returns>
        public NPGGoIndex getTdBg()
        {
            if (_m_heroInfo != null)
                return _m_heroInfo.getTdBg();

            if (_m_heroRef != null)
                return _m_heroRef.td_bg_index;

            return null;
        }
    }
}