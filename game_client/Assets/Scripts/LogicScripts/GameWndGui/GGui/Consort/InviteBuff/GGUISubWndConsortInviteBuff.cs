namespace GOE
{
    /// <summary>
    /// 妃子邀约buff子窗口
    /// </summary>
    public class GGUISubWndConsortInviteBuff : _ANPGGUIBasicSubWnd<GGUISubMonoConsortInviteBuff>
    {
        private GGUIWndConsortInviteBuffItem_AssignInviteBuff _m_wAssignInviteBuff;// 指定邀约buff item
        private GGUIWndConsortInviteBuffItem_GiftedChildBuff _m_wGiftedChildBuff;// 卷王子嗣buff item
        
        public GGUISubWndConsortInviteBuff(GGUISubMonoConsortInviteBuff _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.assignInviteBuff != null)
                _m_wAssignInviteBuff = new GGUIWndConsortInviteBuffItem_AssignInviteBuff(wnd.assignInviteBuff);
            
            if(wnd.giftedChildBuff != null)
                _m_wGiftedChildBuff = new GGUIWndConsortInviteBuffItem_GiftedChildBuff(wnd.giftedChildBuff);
        }
        
        protected override void _onDiscard()
        {
            _m_wAssignInviteBuff?.discard();
            _m_wAssignInviteBuff = null;
            
            _m_wGiftedChildBuff?.discard();
            _m_wGiftedChildBuff = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wAssignInviteBuff?.showWnd();
            _m_wGiftedChildBuff?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wAssignInviteBuff?.hideWnd();
            _m_wGiftedChildBuff?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wAssignInviteBuff?.resetWnd();
            _m_wGiftedChildBuff?.resetWnd();
        }
    }
}