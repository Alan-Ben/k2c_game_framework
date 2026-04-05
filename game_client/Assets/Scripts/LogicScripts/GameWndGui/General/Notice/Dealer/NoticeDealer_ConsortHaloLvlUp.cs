namespace GOE
{
    /// <summary>
    /// 星辉等级提升弹窗
    /// </summary>
    public class NoticeDealer_ConsortHaloLvlUp : NPUINoticeMgr._ANPUINoticeDealer
    {
        private ConsortHaloInfo _m_iHaloInfo;//星辉信息
        private bool _m_bIsUnlock;//是否是解锁弹窗
        
        public NoticeDealer_ConsortHaloLvlUp(ConsortHaloInfo _consortHaloInfo, bool _isUnlock)
        {
            _m_iHaloInfo = _consortHaloInfo;
            _m_bIsUnlock = _isUnlock;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        public override string noticeTag { get; }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_HALO_LEVELUP; } }
        
        public override void dealShowNotice()
        {
            if (_m_iHaloInfo == null)
            {
                setDealerDone();
                return;
            }
            
            GUISceneMain.instance.showAddWnd(GGUIWndConsortHaloLvlUp.instance, () =>
            {
                GGUIWndConsortHaloLvlUp.instance.showWnd();
                GGUIWndConsortHaloLvlUp.instance.setData(_m_iHaloInfo, _m_bIsUnlock, setDealerDone);
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortHaloLvlUp.instance.hideWnd();
        }
                
        protected override void _onDealerDone()
        {
        }
    }
}