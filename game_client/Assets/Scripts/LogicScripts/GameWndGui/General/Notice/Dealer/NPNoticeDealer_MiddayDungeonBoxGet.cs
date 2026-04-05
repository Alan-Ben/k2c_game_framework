using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// MiddayDungeonBoxGet notice
    /// </summary>
    public class NPNoticeDealer_MiddayDungeonBoxGet : NPUINoticeMgr._ANPUINoticeDealer
    {
        private List<NPCommon.NPCommon_ItemInfo> _m_info;

        private long _m_boxId;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        private Action _m_onDealerDone;

        public NPNoticeDealer_MiddayDungeonBoxGet(List<NPCommon.NPCommon_ItemInfo> _info, long _boxId, Action _onDealDone = null)
        {
            _m_info = _info;
            _m_boxId = _boxId;
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
                GGUIWndMiddayDungeonBoxGet.instance.discard();
                _m_bWndLoaded = false;
            }
            GCommon.dealGainItem(_m_info, TransKeyConst.common_getreward_tip, _m_onDealerDone);
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
                GGUIWndMiddayDungeonBoxGet.instance.load(() =>
                {
                    GGUIWndMiddayDungeonBoxGet.instance.showWnd();
                    GGUIWndMiddayDungeonBoxGet.instance.setInfo(_m_boxId, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndMiddayDungeonBoxGet.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndMiddayDungeonBoxGet.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
