namespace GOE
{
    /// <summary>
    /// 妃子一键邀约结果
    /// </summary>
    public class NoticeDealer_ConsortAKeyCallResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private GS2GC.p015_ConsortOp.GS2GC_015_005_RetCallAkey _m_RetMsg;
        
        public NoticeDealer_ConsortAKeyCallResult(GS2GC.p015_ConsortOp.GS2GC_015_005_RetCallAkey _retMsg)
        {
            _m_RetMsg = _retMsg;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        public override string noticeTag { get; }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_AKEY_INVITE_REWARD; } }
        
        public override void dealShowNotice()
        {
            if (_m_RetMsg == null || _m_RetMsg.getResList() == null)
            {
                setDealerDone();
                return;
            }
            
            GUISceneMain.instance.showAddWnd(GGUIWndConsortAkeyInviteReward.instance, () =>
            {
                GGUIWndConsortAkeyInviteReward.instance.showWnd();
                GGUIWndConsortAkeyInviteReward.instance.setData(_m_RetMsg.getResList());
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortAkeyInviteReward.instance.hideWnd();
        }
                
        protected override void _onDealerDone()
        {
        }
    }
}