using System;

namespace GOE
{
    /// <summary>
    /// 妃子获取CG弹窗
    /// </summary>
    public class NoticeDealer_ConsortGetCG : NPUINoticeMgr._ANPUINoticeDealer
    {
        private Func<bool> _m_fIsEnableFunc;
        private ConsortCGRefObj _m_consortCgRef;
        private Action _m_aOnDealDone;

        public NoticeDealer_ConsortGetCG(Func<bool> _isEnableFunc, ConsortCGRefObj _consortCgRef, Action _onDealDone = null)
        {
            _m_fIsEnableFunc = _isEnableFunc;
            _m_consortCgRef = _consortCgRef;
            _m_aOnDealDone = _onDealDone;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        public override string noticeTag { get; }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_CG_GET; } }
        
        public override void dealShowNotice()
        {
            if (_m_consortCgRef == null || (_m_fIsEnableFunc != null && !_m_fIsEnableFunc()))
            {
                setDealerDone();
                return;
            }
            
            GGUIWndConsortCGGet.instance.load();
            GGUIWndConsortCGGet.instance.regLoadDoneDelegate(() =>
            {
                GGUIWndConsortCGGet.instance.setData(_m_consortCgRef);
                GGUIWndConsortCGGet.instance.showWnd();
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortCGGet.instance.discard();
        }
                
        protected override void _onDealerDone()
        {
            _m_aOnDealDone?.Invoke();
            _m_aOnDealDone = null;
        }
    }
}