
namespace GOE
{
    public class NoticeDealer_ChildGraduateSuccess : NPUINoticeMgr._ANPUINoticeDealer
    {
        private bool _m_bWndLoaded;
        
        private readonly _IChildInfo _m_childInfo;
        
        
        public NoticeDealer_ChildGraduateSuccess(_IChildInfo _childInfo)
        {
            _m_childInfo = _childInfo;
        }
        
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        

        public override void dealShowNotice()
        {
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                GGUIWndChildGraduateSuccess.instance.load(GGUIWndChildGraduateSuccess.instance.showWnd);
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndChildGraduateSuccess.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndChildGraduateSuccess.instance.rectTransform);
            }
            
            GGUIWndChildGraduateSuccess.instance.refreshWnd(_m_childInfo);
        }
        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                if (GGUIWndChildGraduateSuccess.instance.dontShowAgainToday)
                    AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.CHILD_GRADUATE_SUCCESS);
                
                GGUIWndChildGraduateSuccess.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
        }
    }
}