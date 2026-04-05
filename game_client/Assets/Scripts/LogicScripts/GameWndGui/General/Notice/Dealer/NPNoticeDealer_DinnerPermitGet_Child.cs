using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// DinnerPermitGet_Child notice（子嗣庆功宴凭证获得通知）
    /// </summary>
    public class NPNoticeDealer_DinnerPermitGet_Child : NPUINoticeMgr._ANPUINoticeDealer
    {
        private DinnerPermit _m_info;
        private Action _m_onDealDone;
        private bool _m_bWndLoaded;

        public NPNoticeDealer_DinnerPermitGet_Child(DinnerPermit _info, Action _onDealDone = null)
        {
            _m_info = _info;
            _m_onDealDone = _onDealDone;
        }

        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        protected override void _onDealerDone()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndDinnerPermitGet_Child.instance.discard();
                _m_bWndLoaded = false;
            }

            _m_onDealDone?.Invoke();
            _m_onDealDone = null;
        }

        public override void dealShowNotice()
        {
            if (_m_info == null)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndDinnerPermitGet_Child.instance.load(() =>
                {
                    GGUIWndDinnerPermitGet_Child.instance.showWnd();
                    GGUIWndDinnerPermitGet_Child.instance.setInfo(_m_info, setDealerDone);
                });
            }
            else
            {
                if (null != GGUIWndDinnerPermitGet_Child.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndDinnerPermitGet_Child.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
