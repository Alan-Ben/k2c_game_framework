using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// MiddayDungeonResult notice
    /// </summary>
    public class NPNoticeDealer_MiddayDungeonResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private List<NPCommon.NPCommon_ItemInfo> _m_info;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        private Action _m_onDealerDone;
        private float _m_autoHideTime; //自动隐藏时间，-1表示不自动隐藏

        public NPNoticeDealer_MiddayDungeonResult(List<NPCommon.NPCommon_ItemInfo> _info, float _autoHideTime = -1, Action _onDealDone = null)
        {
            _m_info = _info;
            _m_onDealerDone = _onDealDone;
            _m_autoHideTime = _autoHideTime;
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
                GGUIWndMiddayDungeonResult.instance.discard();
                _m_bWndLoaded = false;
            }
            _m_onDealerDone?.Invoke();
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
                GGUIWndMiddayDungeonResult.instance.load(() =>
                {
                    GGUIWndMiddayDungeonResult.instance.showWnd();
                    GGUIWndMiddayDungeonResult.instance.setInfo(_m_info, setDealerDone);
                    if(_m_autoHideTime > 0)
                        ALCommonActionMonoTask.addMonoTask(setDealerDone, _m_autoHideTime);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndMiddayDungeonResult.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndMiddayDungeonResult.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
