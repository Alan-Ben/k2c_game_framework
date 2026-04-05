using ALPackage;

namespace GOE
{
    public class GGUISubWndInnStationDishGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoInnStationDishGridItem>
    {
        private InnDishInfo _m_dishInfo;
        private NPGGuiWndTexture _m_dishIconWnd;
        private NPGGuiWndTexture _m_smallTipIconWnd;


        public GGUISubWndInnStationDishGridItem(GGUIMonoInnStationDishGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_dishIconWnd?.showWnd();
            _m_smallTipIconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_dishIconWnd?.hideWnd();
            _m_smallTipIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_dishIconWnd?.discardTexture();
            _m_smallTipIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_dishIconWnd?.discard();
            _m_dishIconWnd = null;
            _m_smallTipIconWnd?.discard();
            _m_smallTipIconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgDishIcon != null)
                _m_dishIconWnd = new NPGGuiWndTexture(wnd.imgDishIcon);
            if (wnd.imgUnlockSmallTipIcon != null)
                _m_smallTipIconWnd = new NPGGuiWndTexture(wnd.imgUnlockSmallTipIcon);
        }
        protected override void _resetGridItem()
        {
        }


        public void refreshWnd(InnDishInfo _dishInfo)
        {
            _m_dishInfo = _dishInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_dishInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDishName, _m_dishInfo.nameTranslated);
            _m_dishIconWnd?.setTexture(_m_dishInfo.refObj.icon);
            _m_smallTipIconWnd?.setTexture(_m_dishInfo.refObj.unlock_small_icon);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockSmallTip, _m_dishInfo.unlockSmallTipTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtDishNum, TextTranslate.instance.getLanguage(TransKeyConst.inn_dishNum_num, _m_dishInfo.num));
            wnd.setShowState(_m_dishInfo.uiState);
        }
    }
}
