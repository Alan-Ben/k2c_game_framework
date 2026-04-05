using ALPackage;

namespace GOE
{
    public class GGUISubWndMarsPosItemSelectContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsPosItemSelectContainerItem>
    {
        private _IMarsExplorePosItem _m_posItem;
        private _AALBasicLoadUIWndBasicClass _m_subWnd;


        public GGUISubWndMarsPosItemSelectContainerItem(GGUIMonoMarsPosItemSelectContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_subWnd?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_subWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_subWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _discardSubWnd();
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(_IMarsExplorePosItem _posItem)
        {
            _m_posItem = _posItem;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_posItem == null)
                return;

            _discardSubWnd();
            _loadSubWnd();
        }


        private void _loadSubWnd()
        {
            if (wnd == null || _m_posItem == null)
                return;

            if (_m_posItem is _IMarsExploreMineItem mineItem)
            {
                var subWnd = new GGUIPrefabSubWndMarsPosItemSelectContainerItem_Mine(wnd.subWndParent);
                _m_subWnd = subWnd;
                subWnd.refreshWnd(mineItem);
                subWnd.load(() =>
                {
                    if (_m_bIsShow)
                        subWnd.showWnd();
                });
            }
            else if (_m_posItem is MarsExploreBattleEventInfo battleItem)
            {
                var subWnd = new GGUIPrefabSubWndMarsPosItemSelectContainerItem_Battle(wnd.subWndParent);
                _m_subWnd = subWnd;
                subWnd.refreshWnd(battleItem);
                subWnd.load(() =>
                {
                    if (_m_bIsShow)
                        subWnd.showWnd();
                });
            }
            else if (_m_posItem is MarsExploreBossEventInfo bossItem)
            {
                var subWnd = new GGUIPrefabSubWndMarsPosItemSelectContainerItem_Boss(wnd.subWndParent);
                _m_subWnd = subWnd;
                subWnd.refreshWnd(bossItem);
                subWnd.load(() =>
                {
                    if (_m_bIsShow)
                        subWnd.showWnd();
                });
            }
        }
        private void _discardSubWnd()
        {
            _m_subWnd?.discard();
            _m_subWnd = null;
        }
    }
}
