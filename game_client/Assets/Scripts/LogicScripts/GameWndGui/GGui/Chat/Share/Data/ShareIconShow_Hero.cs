using ChatPackage;
using NPEnum;

namespace GOE
{
    public class ShareIconShow_Hero:_IShareIconSHow
    {
        private readonly HeroInfo _m_info;

        public ShareIconShow_Hero(HeroInfo _info)
        {
            _m_info = _info;
        }

        public HeroInfo info { get => _m_info; }

        public NPGTextureIndex getIcon()
        {
            return _m_info.getIcon();
        }

        public NPGSpriteIndex getIconBg()
        {
            return GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_info.id)?.hero_head_bg;
        }

        public bool needShowSpecial()
        {
            return false;
        }

        public string getName()
        {
            return _m_info.heroRefObj.transName;
        }

        public _AMsgDetailInfo creatShareInfo()
        {
            return NPMsgDetailInfoFactory.createShareHeroInfo(_m_info);
        }
    }
}