using ALPackage;

namespace GOE
{
    public class GGUIWndConsortSystemRedTip : _ATALBasicUISubWnd<GGUIMonoConsortSystemRedTip>
    {
        private long _m_lConsortId;
        
        private NPGGUIWndCommonRedTip _m_redTip;
        
        public GGUIWndConsortSystemRedTip(GGUIMonoConsortSystemRedTip _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.monoRed != null)
                _m_redTip = new NPGGUIWndCommonRedTip(wnd.monoRed);
        }
        
        protected override void _onDiscard()
        {
            _m_redTip?.discard();
            _m_redTip = null;
        }
        
        protected override void _onShowWnd()
        {
            if (wnd != null && wnd.redParentId > 0 && wnd.monoRed != null)
                WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, refreshRed);
        }

        protected override void _onHideWnd()
        {
            if (wnd != null && wnd.redParentId > 0 && wnd.monoRed != null)
                WinMsg.UnregisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, refreshRed);
            
            _m_redTip?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_redTip?.resetWnd();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setConsortId(long _consortId)
        {
            _m_lConsortId = _consortId;

            refreshRed();
        }
        
        public void refreshRed()
        {
            if (wnd == null || null == _m_redTip || _m_lConsortId <= 0)
                return;
        
            _ARedTipNode _redTipNode = RedTipMgr.instance.getNodeBySaveKeyRecursive(GConsortComponent.getConsortSaveKey(wnd.redParentId, _m_lConsortId));
            _m_redTip.showWnd();
            _m_redTip.showRedTipNum((_redTipNode != null) ? (int)_redTipNode.getCount() : 0);
        }
    }
}