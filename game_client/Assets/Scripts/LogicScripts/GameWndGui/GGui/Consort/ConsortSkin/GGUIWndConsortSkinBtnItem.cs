using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 妃子皮肤入口按钮item
    /// </summary>
    public class GGUIWndConsortSkinBtnItem : _AGGUIWndOptionItem<GGUIMonoConsortSkinBtnItem, GGUIWndConsortSkinBtnItem>
    {
        //皮肤图标
        private NPGGuiWndTexture _m_wIcon;
        //皮肤品质框
        private GGuiWndSprite _m_wQualityBg;
        //星级
        private GGUIWndHeroCommonStar _m_wStar;

        public GGUIWndConsortSkinBtnItem(GGUIMonoConsortSkinBtnItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _m_wIcon?.hideWnd();
            _m_wQualityBg?.hideWnd();
            _m_wStar?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_wIcon?.discardTexture();
            _m_wQualityBg?.discardTexture();
            _m_wStar?.resetWnd();
        }

        protected override void _onDiscardEx()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wQualityBg?.discard();
            _m_wQualityBg = null;
            _m_wStar?.discard();
            _m_wStar = null;
        }

        protected override void _onWndInitDoneEx()
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
        /// <param name="_consortInfo"></param>
        public void setInfo(GGottenConsortInfo _consortInfo)
        {
            if (wnd == null || _consortInfo == null)
                return;

            long skinId = _consortInfo?.consortSkinShowInfo?.skinId ?? 0;
            long level = _consortInfo.curSkinInfo != null ? _consortInfo.curSkinInfo.skinLvl : 0;

            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.CONSORT_SKIN, skinId));
            }

            if (_m_wQualityBg != null)
            {
                _m_wQualityBg.showWnd();
                _m_wQualityBg.setTexture(GCommon.getItemQualityIcon(ENPItemType.CONSORT_SKIN, skinId));
            }

            if (_m_wStar != null)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.CONSORT_SKIN, skinId);
                _m_wStar.showWnd();
                _m_wStar.setInfo(qualityExtRef != null ? qualityExtRef.consort_skin_star : 0);
            }

            ALUGUICommon.setLabelTxt(wnd.txtSkinLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, level));

            //是否默认
            bool isDefault = _consortInfo.consortRefObj != null && _consortInfo.consortRefObj.default_skin_id == skinId;
            ALUGUICommon.setGameObjEnable(wnd.goDefaultSkinShowList, isDefault);
            ALUGUICommon.setGameObjEnable(wnd.goDefaultSkinHideList, !isDefault);
        }
    }
}