using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// DinnerPermitGet notice
    /// </summary>
    public class NPNoticeDealer_DinnerPermitGet : NPUINoticeMgr._ANPUINoticeDealer
    {
        private DinnerPermit _m_info;
        private Action _m_onDealDone;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_DinnerPermitGet(DinnerPermit _info, Action _onDealDone = null)
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
                GGUIWndDinnerPermitGet.instance.discard();
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
                GGUIWndDinnerPermitGet.instance.load(() =>
                {
                    GGUIWndDinnerPermitGet.instance.showWnd();
                    GGUIWndDinnerPermitGet.instance.setInfo(_m_info, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndDinnerPermitGet.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndDinnerPermitGet.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
