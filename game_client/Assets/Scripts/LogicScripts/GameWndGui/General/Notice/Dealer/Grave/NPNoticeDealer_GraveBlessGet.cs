using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    ///  杰出者祝福 notice
    /// </summary>
    public class NPNoticeDealer_GraveBlessGet : NPUINoticeMgr._ANPUINoticeDealer
    {
        private long _m_buffId;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        private Action _m_onDealerDone;

        public NPNoticeDealer_GraveBlessGet(long _buffId, Action _onDealDone = null)
        {
            _m_buffId = _buffId;
            _m_onDealerDone = _onDealDone;
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
                GGUIWndGraveBlessGet.instance.discard();
                _m_bWndLoaded = false;
            }
            _m_onDealerDone?.Invoke();
        }

        public override void dealShowNotice()
        {
            if (_m_buffId == null)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndGraveBlessGet.instance.load(() =>
                {
                    GGUIWndGraveBlessGet.instance.showWnd();
                    GGUIWndGraveBlessGet.instance.setInfo(_m_buffId, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndGraveBlessGet.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndGraveBlessGet.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
