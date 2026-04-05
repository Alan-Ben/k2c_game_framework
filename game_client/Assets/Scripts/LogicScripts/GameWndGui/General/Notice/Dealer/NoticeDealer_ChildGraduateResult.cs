
namespace GOE
{
    public class NoticeDealer_ChildGraduateResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private bool _m_bWndLoaded;
        
        private readonly _IChildInfo _m_childInfo;
        private readonly _IItem _m_presentItem;
        
        
        public NoticeDealer_ChildGraduateResult(_IChildInfo _childInfo, _IItem _presentItem)
        {
            _m_childInfo = _childInfo;
            _m_presentItem = _presentItem;
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

                GGUIWndChildGraduationResult.instance.load(GGUIWndChildGraduationResult.instance.showWnd);
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndChildGraduationResult.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndChildGraduationResult.instance.rectTransform);
            }
            
            GGUIWndChildGraduationResult.instance.refreshWnd(_m_childInfo, _m_presentItem);
        }
        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndChildGraduationResult.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
        }
    }
}