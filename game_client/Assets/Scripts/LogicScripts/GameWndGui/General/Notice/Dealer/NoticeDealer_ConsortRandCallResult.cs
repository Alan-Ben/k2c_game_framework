using System;

namespace GOE
{
    /// <summary>
    /// 妃子随机邀约结果
    /// </summary>
    public class NoticeDealer_ConsortRandCallResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private Func<bool> _m_fIsEnableFunc;
        private GS2GC.p015_ConsortOp.GS2GC_015_004_RetCallRand _m_RetMsg;
        private Action _m_aOnDealDone;

        public NoticeDealer_ConsortRandCallResult(Func<bool> _isEnableFunc, GS2GC.p015_ConsortOp.GS2GC_015_004_RetCallRand _retMsg, Action _onDealDone = null)
        {
            _m_fIsEnableFunc = _isEnableFunc;
            _m_RetMsg = _retMsg;
            _m_aOnDealDone = _onDealDone;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        public override string noticeTag { get; }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_RANDOM_INVITE_REWARD; } }
        
        public override void dealShowNotice()
        {
            if (_m_RetMsg == null || _m_RetMsg.getRes() == null || (_m_fIsEnableFunc != null && !_m_fIsEnableFunc()))
            {
                setDealerDone();
                return;
            }
            
            GUISceneMain.instance.showAddWnd(GGUIWndConsortRandomInviteReward.instance, () =>
            {
                GGUIWndConsortRandomInviteReward.instance.showWnd();
                GGUIWndConsortRandomInviteReward.instance.setData(_m_RetMsg.getRes());
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortRandomInviteReward.instance.hideWnd();
        }
        
                
        protected override void _onDealerDone()
        {
            _m_aOnDealDone?.Invoke();
            _m_aOnDealDone = null;
        }
    }
}