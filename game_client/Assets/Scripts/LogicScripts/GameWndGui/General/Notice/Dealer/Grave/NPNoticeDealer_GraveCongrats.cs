using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    ///  杰出者庆祝 notice
    /// </summary>
    public class NPNoticeDealer_GraveCongrats : NPUINoticeMgr._ANPUINoticeDealer
    {
        private bool _m_isNewProminent; // 是否是新晋杰出者
        private long _m_playerCid; // 玩家cid
        private List<NPCommonCostItem> _m_rewardList; // 奖励列表
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        private Action _m_onDealerDone;

        public NPNoticeDealer_GraveCongrats(bool _isNewProminent, long _playerCid, List<NPCommonCostItem> _rewardList, Action _onDealDone = null)
        {
            _m_isNewProminent = _isNewProminent;
            _m_playerCid = _playerCid;
            _m_rewardList = _rewardList;
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
                GGUIWndGraveCongrats.instance.discard();
                _m_bWndLoaded = false;
            }
            _m_onDealerDone?.Invoke();
        }

        public override void dealShowNotice()
        {
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndGraveCongrats.instance.load(() =>
                {
                    GCommon.reqPlayerInfo(_m_playerCid, (_info) =>
                    {
                        GGUIWndGraveCongrats.instance.showWnd();
                        GGUIWndGraveCongrats.instance.setInfo(_m_isNewProminent, _info, _m_rewardList, setDealerDone);
                    });
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndGraveCongrats.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndGraveCongrats.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
