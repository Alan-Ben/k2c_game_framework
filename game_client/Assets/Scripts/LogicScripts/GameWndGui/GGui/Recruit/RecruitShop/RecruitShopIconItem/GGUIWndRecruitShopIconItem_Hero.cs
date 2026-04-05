using NPEnum;

namespace GOE
{
    public class GGUIWndRecruitShopIconItem_Hero : _AGGUIWndRecruitShopIconItem<GGUIMonoRecruitShopIconItem_Hero, RecruitHeroItemInfo>
    {
        private NPGGuiWndTexture _m_wHeroHeadIcon;
        private GGuiWndSprite _m_wHeroHeadBg;
        
        public GGUIWndRecruitShopIconItem_Hero(GGUIMonoRecruitShopIconItem_Hero _mono) : base(_mono)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.heroHeadIcon != null)
                _m_wHeroHeadIcon = new NPGGuiWndTexture(wnd.heroHeadIcon);

            if (wnd.heroHeadBg != null)
                _m_wHeroHeadBg = new GGuiWndSprite(wnd.heroHeadBg);
        }

        protected override void _onDiscardSub()
        {
            _m_wHeroHeadIcon?.discard();
            _m_wHeroHeadIcon = null;
            
            _m_wHeroHeadBg?.discard();
            _m_wHeroHeadBg = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wHeroHeadIcon?.hideWnd();
            _m_wHeroHeadBg?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wHeroHeadIcon?.discardTexture();
            _m_wHeroHeadBg?.discardTexture();
        }

        protected override void _onSetData()
        {
        }

        protected override void _onRefreshWnd()
        {
            if(wnd == null || _m_iRecruitItemInfo == null)
                return;

            HeroCardShowInfo heroShowInfo = _m_iRecruitItemInfo.getHeroShowInfo(true);
            if (heroShowInfo != null)
            {
                if (_m_wHeroHeadIcon != null)
                {
                    _m_wHeroHeadIcon.showWnd();
                    _m_wHeroHeadIcon.setTexture(heroShowInfo.getIcon());
                }

                EQuality quality = GCommon.getItemQuality(ENPItemType.HERO, heroShowInfo.id);
                GGUIMonoCommonQualityImgConfig qualityImgConfig = _getQualityImgConfig(quality);
                if (_m_wHeroHeadBg != null)
                {
                    _m_wHeroHeadBg.showWnd();

                    if (qualityImgConfig != null)
                    {
                        _m_wHeroHeadBg.setTexture(qualityImgConfig.spriteIndex);
                    }
                    else
                    {
                        NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)quality);
                        _m_wHeroHeadBg.setTexture(qualityExtRefObj?.hero_head_bg);
                    }
                }
            }
        }
    }
}