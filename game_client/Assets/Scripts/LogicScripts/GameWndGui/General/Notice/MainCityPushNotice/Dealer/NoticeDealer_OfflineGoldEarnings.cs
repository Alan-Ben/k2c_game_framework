namespace GOE
{
    public class NoticeDealer_OfflineGoldEarnings : _AMainCityCanJumpPushNotice
    {
        private bool _m_bWndLoaded;
        private OfflineGoldData _m_offlineData;
        
        
        public NoticeDealer_OfflineGoldEarnings(OfflineGoldData _offlineData, EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
            _m_offlineData = _offlineData;
        }
        
        protected override bool _isEnable { get { return true; } }
        protected override bool _canCurShow { get { return QueueMgr.instance._lastNode.nodeTag == UINodeTagConst.C_BUILDING; } }
        protected override string _noticeTag { get { return NoticeTagConst.OFFLINE_GOLD_EARNINGS; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        

        public override void dealShowNotice()
        {
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                GGUIWndPlayerOfflineGoldEarnings.instance.load(GGUIWndPlayerOfflineGoldEarnings.instance.showWnd);
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndPlayerOfflineGoldEarnings.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndPlayerOfflineGoldEarnings.instance.rectTransform);
            }
            
            GGUIWndPlayerOfflineGoldEarnings.instance.refreshWnd(_m_offlineData);
        }
        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndPlayerOfflineGoldEarnings.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void __onDealerDone()
        {
        }

        // 临时加上, 防止不能IF修改
        public override void showNotice()
        {
            base.showNotice();
        }

        // 临时加上, 防止不能IF修改
        protected override void _onGotoOtherMainViewNode()
        {
        }
    }
}