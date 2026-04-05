
using System.Collections.Generic;

namespace GOE
{
    public class NoticeDealer_ChildMultiGraduateResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private bool _m_bWndLoaded;
        
        private readonly List<_IChildInfo> _m_childList;
        private readonly List<_IItem> _m_presentList;
        
        
        public NoticeDealer_ChildMultiGraduateResult(List<_IChildInfo> _childList, List<_IItem> _presentList)
        {
            _m_childList = _childList;
            _m_presentList = _presentList;
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

                GGUIWndChildMultiGraduationResult.instance.load(GGUIWndChildMultiGraduationResult.instance.showWnd);
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndChildMultiGraduationResult.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndChildMultiGraduationResult.instance.rectTransform);
            }
            
            GGUIWndChildMultiGraduationResult.instance.refreshWnd(_m_childList, _m_presentList);
        }
        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndChildMultiGraduationResult.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
        }
    }
}