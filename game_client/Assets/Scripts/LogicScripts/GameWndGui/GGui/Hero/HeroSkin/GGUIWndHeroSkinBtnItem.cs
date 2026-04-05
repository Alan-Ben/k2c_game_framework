using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤入口按钮item
    /// </summary>
    public class GGUIWndHeroSkinBtnItem : _ATALBasicUISubWnd<GGUIMonoHeroSkinBtnItem>
    {
        //皮肤图标
        private NPGGuiWndTexture _m_wIcon;
        //皮肤品质框
        private GGuiWndSprite _m_wQualityBg;
        //星级
        private GGUIWndHeroCommonStar _m_wStar;

        public GGUIWndHeroSkinBtnItem(GGUIMonoHeroSkinBtnItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wQualityBg?.hideWnd();
            _m_wStar?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wQualityBg?.discardTexture();
            _m_wStar?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wQualityBg?.discard();
            _m_wQualityBg = null;
            _m_wStar?.discard();
            _m_wStar = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgCurSkinIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgCurSkinIcon);

            if (wnd.imgCurSkinQualityBg != null)
                _m_wQualityBg = new GGuiWndSprite(wnd.imgCurSkinQualityBg);

            if (wnd.monoSkinStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoSkinStar);
        }

        /// <summary>
        /// 设置星级
        /// </summary>
        /// <param name="_heroInfo"></param>
        public void setInfo(HeroInfo _heroInfo)
        {
            if (wnd == null || _heroInfo == null)
                return;

            HeroSkinInfo skinInfo = _heroInfo.heroSkinInfoMgr.getSkinInfo(_heroInfo.curSkinId);
            long level = skinInfo != null ? skinInfo.level : 0;

            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, _heroInfo.curSkinId));
            }

            if (_m_wQualityBg != null)
            {
                _m_wQualityBg.showWnd();
                _m_wQualityBg.setTexture(GCommon.getItemQualitySpIcon(ENPItemType.HERO_SKIN, _heroInfo.curSkinId));
            }

            if (_m_wStar != null)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO_SKIN, _heroInfo.curSkinId);
                _m_wStar.showWnd();
                _m_wStar.setInfo(qualityExtRef != null ? qualityExtRef.hero_skin_star : 0);
            }

            ALUGUICommon.setLabelTxt(wnd.txtSkinLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, level));

            //是否默认
            bool isDefault = _heroInfo.heroRefObj != null && _heroInfo.heroRefObj.default_skin_id == _heroInfo.curSkinId;
            ALUGUICommon.setGameObjEnable(wnd.goDefaultSkinShowList, isDefault);
            ALUGUICommon.setGameObjEnable(wnd.goDefaultSkinHideList, !isDefault);
        }
    }
}
