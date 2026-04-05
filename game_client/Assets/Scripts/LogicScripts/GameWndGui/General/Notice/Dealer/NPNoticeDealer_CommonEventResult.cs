using System;

namespace GOE
{
    /// <summary>
    /// 通用奖励事件弹窗
    /// </summary>
    public class NPNoticeDealer_CommonEventResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private CommonEventShowResultInfo _m_iEventShowResultInfo;
        private Action _m_aOnShowWnd;
        private Action _m_aOnClose;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_CommonEventResult(CommonEventShowResultInfo _eventShowResultInfo, Action _onShowWnd, Action _onClose)
        {
            _m_iEventShowResultInfo = _eventShowResultInfo;
            _m_aOnShowWnd = _onShowWnd;
            _m_aOnClose = _onClose;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }

        public override bool isNoticeFullScreen { get { return true; } }//虽然他不是全屏的，但是由于某些特殊原因需要，他要隐藏前面的界面 https://www.teambition.com/task/66a316e22809ae5af67c41ec

        public override string nodeTag => UINodeTagConst_CommonEvent.C_COMMON_EVENT_RESULT_NODE;

        public override void dealShowNotice()
        {
            if (null == _m_iEventShowResultInfo)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndCommonEventResult.instance.load(() =>
                {
                    GGUIWndCommonEventResult.instance.showWnd();
                    _m_aOnShowWnd?.Invoke();
                    GGUIWndCommonEventResult.instance.setData(_m_iEventShowResultInfo, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndCommonEventResult.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndCommonEventResult.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndCommonEventResult.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
            _m_aOnClose?.Invoke();
        }
    }
}
