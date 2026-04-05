using ALPackage;

namespace GOE
{
    public class GGUISubWndInnLevelDetailGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoInnLevelDetailGridItem>
    {
        private InnLevelRefObj _m_levelRef;
        private GGUISubWndInnLevelIconContainer _m_starIconContainerWnd;
        private NPGGuiWndTexture _m_medalIconWnd;


        public GGUISubWndInnLevelDetailGridItem(GGUIMonoInnLevelDetailGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_starIconContainerWnd?.showWnd();
            _m_medalIconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_starIconContainerWnd?.hideWnd();
            _m_medalIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_starIconContainerWnd?.resetWnd();
            _m_medalIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_starIconContainerWnd?.discard();
            _m_starIconContainerWnd = null;
            _m_medalIconWnd?.discard();
            _m_medalIconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoStarIconContainer != null)
                _m_starIconContainerWnd = new GGUISubWndInnLevelIconContainer(wnd.monoStarIconContainer);
            
            if (wnd.imgMedalIcon != null)
                _m_medalIconWnd = new NPGGuiWndTexture(wnd.imgMedalIcon);
        }
        protected override void _resetGridItem()
        {
        }


        public void refreshWnd(InnLevelRefObj _levelRef)
        {
            _m_levelRef = _levelRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            if (_m_levelRef == null)
                return;

            // 设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_levelRef.level));
            // 设置等级名称
            ALUGUICommon.setLabelTxt(wnd.txtLevelName, _m_levelRef.nameTranslated);
            // 设置奖牌图标
            _m_medalIconWnd?.setTexture(_m_levelRef.medal_icon);
            // 设置需要人气值
            ALUGUICommon.setLabelTxt(wnd.txtPopularityRequire, _m_levelRef.need_popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            // 设置迎宾次数上限
            ALUGUICommon.setLabelTxt(wnd.txtReceiveGuestLimit, _m_levelRef.receive_guest_limit);
            ALUGUICommon.setLabelTxt(wnd.txtGuestUnlockNum, _m_levelRef.unlock_guest_num);
            // 设置当前等级显示状态
            bool isCurrentLevel = _m_levelRef.level == NPPlayer.instance.innComp.levelRef?.level;
            wnd.setCurrentShow(isCurrentLevel);
            
            // 刷新星级图标容器
            _m_starIconContainerWnd?.refreshWnd(_m_levelRef);
        }
    }
}