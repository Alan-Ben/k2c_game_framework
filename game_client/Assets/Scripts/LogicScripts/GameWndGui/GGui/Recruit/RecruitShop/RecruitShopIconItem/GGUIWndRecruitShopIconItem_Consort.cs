namespace GOE
{
    public class GGUIWndRecruitShopIconItem_Consort : _AGGUIWndRecruitShopIconItem<GGUIMonoRecruitShopIconItem_Consort, RecruitConsortItemInfo>
    {
        private NPGGuiWndTexture _m_wConsortHeadIcon;
        private GGuiWndSprite _m_wConsortHeadBg;
        
        public GGUIWndRecruitShopIconItem_Consort(GGUIMonoRecruitShopIconItem_Consort _mono) : base(_mono)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.consortHeadIcon != null)
                _m_wConsortHeadIcon = new NPGGuiWndTexture(wnd.consortHeadIcon);

            if (wnd.consortHeadBg != null)
                _m_wConsortHeadBg = new GGuiWndSprite(wnd.consortHeadBg);
        }

        protected override void _onDiscardSub()
        {
            _m_wConsortHeadIcon?.discard();
            _m_wConsortHeadIcon = null;
            
            _m_wConsortHeadBg?.discard();
            _m_wConsortHeadBg = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wConsortHeadIcon?.hideWnd();
            _m_wConsortHeadBg?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wConsortHeadIcon?.discardTexture();
            _m_wConsortHeadBg?.discardTexture();
        }

        protected override void _onSetData()
        {
        }

        protected override void _onRefreshWnd()
        {
            if(wnd == null || _m_iRecruitItemInfo == null)
                return;

            _IConsortShowInfo consortShowInfo = _m_iRecruitItemInfo.consortInfo;
            if (consortShowInfo != null)
            {
                if (_m_wConsortHeadIcon != null)
                {
                    _m_wConsortHeadIcon.showWnd();
                    _m_wConsortHeadIcon.setTexture(consortShowInfo?.consortSkinShowInfo?.consortHeadIcon);
                }

                GGUIMonoCommonQualityImgConfig qualityImgConfig = _getQualityImgConfig(consortShowInfo.consortQuality);
                if (_m_wConsortHeadBg != null)
                {
                    _m_wConsortHeadBg.showWnd();
                    if (qualityImgConfig != null)
                    {
                        _m_wConsortHeadBg.setTexture(qualityImgConfig.spriteIndex);
                    }
                    else
                    {
                        _m_wConsortHeadBg.setTexture(GCommon.getQualityExtRefObj(NPEnum.ENPItemType.CONSORT, consortShowInfo.consortId)?.consort_head_bg);
                    }
                }
            }
        }
    }
}